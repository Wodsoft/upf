using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Data;

namespace Wodsoft.UI
{
    internal abstract class ConditionBinding : IDisposable
    {
        public event ConditionBindingEqualityChangedEventHandler? IsEqualityChanged;

        protected void EqualityChanged(bool isEquality)
        {
            IsEqualityChanged?.Invoke(this, isEquality);
        }

        public abstract void Dispose();

        public abstract bool IsEquality { get; }

        public abstract void EnsureEquailty();
    }

    internal delegate void ConditionBindingEqualityChangedEventHandler(ConditionBinding conditionBinding, bool isEquality);

    internal class DependencyConditionBinding : ConditionBinding, IDisposable
    {
        private readonly DependencyObject _target;
        private readonly DependencyProperty _property;
        private readonly object? _value;
        private bool _isEquality;
        private bool _disposed;

        public DependencyConditionBinding(DependencyObject target, DependencyProperty property, object? value)
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
            target.DependencyPropertyChanged += Target_DependencyPropertyChanged;
        }

        private void Target_DependencyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == _property)
            {
                if (!_isEquality && Equals(e.NewValue, _value))
                {
                    _isEquality = true;
                    EqualityChanged(true);
                }
                else if (_isEquality && !Equals(e.NewValue, _value))
                {
                    _isEquality = false;
                    EqualityChanged(false);
                }
            }
        }

        public override bool IsEquality => _isEquality;

        public override void Dispose()
        {
            if (_disposed)
                return;
            _disposed = true;
            _target.DependencyPropertyChanged -= Target_DependencyPropertyChanged;
        }

        public override void EnsureEquailty()
        {
            var value = _target.GetValue(_property);
            if (!_isEquality && Equals(value, _value))
            {
                _isEquality = true;
                EqualityChanged(true);
            }
            else if (_isEquality && !Equals(value, _value))
            {
                _isEquality = false;
                EqualityChanged(false);
            }
        }
    }

    internal class ExpressionConditionBinding : ConditionBinding, IDisposable
    {
        private readonly BindingExpressionBase _expression;
        private readonly object? _value;
        private TypeConverter? _converter;
        private object? _convertedValue;
        private bool _isEquality;
        private bool _disposed;

        public ExpressionConditionBinding(BindingExpressionBase expression, object? value)
        {
            _expression = expression;
            _value = value;
            _expression.ValueChanged += ValueChanged;
        }

        private void ValueChanged(object? sender, EventArgs e)
        {
            EnsureEquailty();
        }

        public override bool IsEquality => _isEquality;

        public override void Dispose()
        {
            if (_disposed)
                return;
            _disposed = true;
            _expression.ValueChanged -= ValueChanged;
            _expression.Detach();
        }

        public override void EnsureEquailty()
        {
            var expressionValue = _expression.Value;
            bool isEquality;
            if (expressionValue != null && _value != null)
            {
                var valueType = _value.GetType();
                var expressionType = expressionValue.GetType();
                if (expressionType.IsAssignableFrom(valueType))
                {
                    isEquality = Equals(expressionValue, _value);
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
                    isEquality = Equals(expressionValue, _convertedValue);
                }

            }
            else
                isEquality = Equals(expressionValue, _value);
            if (!_isEquality && isEquality)
            {
                _isEquality = true;
                EqualityChanged(true);
            }
            else if (_isEquality && !isEquality)
            {
                _isEquality = false;
                EqualityChanged(false);
            }
        }
    }
}
