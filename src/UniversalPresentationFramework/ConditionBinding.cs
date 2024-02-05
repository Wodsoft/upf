using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Data;

namespace Wodsoft.UI
{
    internal abstract class ConditionBinding : IDisposable
    {
        public abstract event ConditionBindingEqualityChangedEventHandler? IsMatchedChanged;

        //protected void IsMatchedChanged(bool isMatched)
        //{
        //    IsEqualityChanged?.Invoke(this, isMatched);
        //}

        public abstract void Dispose();

        public abstract bool IsMatched { get; }

        public abstract void EnsureMatched();

        protected bool Match(object? left, object? right, ConditionLogic logic)
        {
            switch (logic)
            {
                case ConditionLogic.Equal:
                    return Equals(left, right);
                case ConditionLogic.NotEqual:
                    return !Equals(left, right);
            }
            if (left is IComparable comparable)
            {
                switch (logic)
                {
                    case ConditionLogic.Less:
                        return comparable.CompareTo(right) < 0;
                    case ConditionLogic.LessThan:
                        return comparable.CompareTo(right) <= 0;
                    case ConditionLogic.Greater:
                        return comparable.CompareTo(right) > 0;
                    case ConditionLogic.GreaterThan:
                        return comparable.CompareTo(right) >= 0;
                }
            }
            return false;
        }
    }

    internal delegate void ConditionBindingEqualityChangedEventHandler(ConditionBinding conditionBinding, bool isMatched);

    internal class DependencyConditionBinding : ConditionBinding, IDisposable
    {
        private readonly DependencyObject _target;
        private readonly DependencyProperty _property;
        private readonly object? _value;
        private readonly ConditionLogic _logic;
        private bool _isMatched;
        private bool _disposed;

        public DependencyConditionBinding(DependencyObject target, DependencyProperty property, object? value, ConditionLogic logic)
        {
            _target = target;
            _property = property;
            if (value != null)
            {
                var valueType = value.GetType();
                if (!_property.PropertyType.IsAssignableFrom(valueType))
                {
                    var converter = TypeDescriptor.GetConverter(_property.PropertyType);
                    if (converter.CanConvertFrom(valueType))
                        value = converter.ConvertFrom(value);
                }
            }
            _value = value;
            _logic = logic;
            target.DependencyPropertyChanged += Target_DependencyPropertyChanged;
        }

        public override event ConditionBindingEqualityChangedEventHandler? IsMatchedChanged;

        private void Target_DependencyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == _property)
            {
                if (!_isMatched && Match(e.NewValue, _value, _logic))
                {
                    _isMatched = true;
                    IsMatchedChanged?.Invoke(this, true);
                }
                else if (_isMatched && !Match(e.NewValue, _value, _logic))
                {
                    _isMatched = false;
                    IsMatchedChanged?.Invoke(this, false);
                }
            }
        }

        public override bool IsMatched => _isMatched;

        public override void Dispose()
        {
            if (_disposed)
                return;
            _disposed = true;
            _target.DependencyPropertyChanged -= Target_DependencyPropertyChanged;
        }

        public override void EnsureMatched()
        {
            var value = _target.GetValue(_property);
            if (!_isMatched && Match(value, _value, _logic))
            {
                _isMatched = true;
                IsMatchedChanged?.Invoke(this, true);
            }
            else if (_isMatched && !Match(value, _value, _logic))
            {
                _isMatched = false;
                IsMatchedChanged?.Invoke(this, false);
            }
        }
    }

    internal class ExpressionConditionBinding : ConditionBinding, IDisposable
    {
        private readonly BindingExpressionBase _expression;
        private readonly object? _value;
        private readonly ConditionLogic _logic;
        private TypeConverter? _converter;
        private object? _convertedValue;
        private bool _isMatched;
        private bool _disposed;

        public ExpressionConditionBinding(BindingExpressionBase expression, object? value, ConditionLogic logic)
        {
            _expression = expression;
            _value = value;
            _logic = logic;
            _expression.ValueChanged += ValueChanged;
        }

        public override event ConditionBindingEqualityChangedEventHandler? IsMatchedChanged;

        private void ValueChanged(object? sender, EventArgs e)
        {
            EnsureMatched();
        }

        public override bool IsMatched => _isMatched;

        public override void Dispose()
        {
            if (_disposed)
                return;
            _disposed = true;
            _expression.ValueChanged -= ValueChanged;
            _expression.Detach();
        }

        public override void EnsureMatched()
        {
            var expressionValue = _expression.Value;
            bool isMatched;
            if (expressionValue != null && _value != null)
            {
                var valueType = _value.GetType();
                var expressionType = expressionValue.GetType();
                if (expressionType.IsAssignableFrom(valueType))
                {
                    isMatched = Match(expressionValue, _value, _logic);
                }
                else
                {
                    var converter = TypeDescriptor.GetConverter(expressionType);
                    //Convert once
                    if (converter != _converter)
                    {
                        if (converter.CanConvertFrom(valueType))
                            _convertedValue = converter.ConvertFrom(_value);
                        else
                            _convertedValue = _value;
                        _converter = converter;
                    }
                    isMatched = Match(expressionValue, _convertedValue, _logic);
                }
            }
            else
                isMatched = Match(expressionValue, _value, _logic);
            if (!_isMatched && isMatched)
            {
                _isMatched = true;
                IsMatchedChanged?.Invoke(this, true);
            }
            else if (_isMatched && !isMatched)
            {
                _isMatched = false;
                IsMatchedChanged?.Invoke(this, false);
            }
        }
    }
}
