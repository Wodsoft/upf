using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public struct DependencyEffectiveValue
    {
        private DependencyEffectiveSource _source;
        private Expression? _expression;
        private object? _value, _coercedValue;
        private bool _hasValue, _isCoerced;
        private IDependencyModifiedValue? _modifiedValue;

        public DependencyEffectiveValue(Expression expression) : this(expression, DependencyEffectiveSource.Expression)
        {
        }

        public DependencyEffectiveValue(Expression expression, DependencyEffectiveSource source)
        {
            _expression = expression;
            _source = DependencyEffectiveSource.Expression;
            _value = DependencyProperty.UnsetValue;
        }

        public DependencyEffectiveValue(DependencyEffectiveSource source)
        {
            //if (source == DependencyEffectiveSource.None)
            //    throw new ArgumentException("Could not create none source effective value.", "source");
            if (source == DependencyEffectiveSource.Expression)
                throw new InvalidOperationException("Could not create expression source effective value without expression.");
            _source = source;
        }

        public DependencyEffectiveValue(object? value, DependencyEffectiveSource source)
        {
            if (source == DependencyEffectiveSource.None)
                throw new ArgumentException("Could not create none source effective value.", "source");
            if (source == DependencyEffectiveSource.Expression)
                throw new InvalidOperationException("Could not create expression source effective value without expression.");
            _source = source;
            _value = value;
        }

        public DependencyEffectiveSource Source => _source;

        public object? Value
        {
            get
            {
                if (_source == DependencyEffectiveSource.None)
                    return DependencyProperty.UnsetValue;
                else if (_isCoerced)
                    return _coercedValue;
                else if (_modifiedValue != null)
                    return _modifiedValue.GetValue(ref this);
                //else if (!_hasValue && _expression != null && _expression!.Value != Expression.NoValue)
                //    return _expression.Value;
                //else
                return _value;
            }
        }

        public object? BaseValue
        {
            get
            {
                if (_source == DependencyEffectiveSource.None)
                    return DependencyProperty.UnsetValue;
                else if (_modifiedValue != null)
                    return _modifiedValue.GetValue(ref this);
                //else if (!_hasValue && _expression != null && _expression!.Value != Expression.NoValue)
                //    return _expression.Value;
                //else
                return _value;
            }
        }

        public Expression? Expression => _expression;

        public bool HasValue => _hasValue;

        public IDependencyModifiedValue? ModifiedValue => _modifiedValue;

        public bool HasModifiedValue => _modifiedValue != null;

        public void ModifyValue(IDependencyModifiedValue? modifyedValue)
        {
            _modifiedValue = modifyedValue;
        }

        public void UpdateValue(object? value)
        {
            _hasValue = true;
            _value = value;
        }

        public bool IsCoerced => _isCoerced;

        internal void SetCoercedValue(object? value)
        {
            _isCoerced = true;
            _coercedValue = value;
        }

        internal void ClearCoercedValue()
        {
            _isCoerced = false;
            _coercedValue = null;
        }

        internal static DependencyEffectiveValue Default;
    }

    public enum DependencyEffectiveSource : byte
    {
        None = 0,
        Local = 1,
        Expression = 2,
        Internal = 3,
        Inherited = 4
    }
}
