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
        private bool _modified;
        private Expression? _expression;
        private object? _value;

        public DependencyEffectiveValue(Expression expression)
        {
            _expression = expression;
            _source = DependencyEffectiveSource.Expression;
        }

        public DependencyEffectiveValue(object? value, DependencyEffectiveSource source)
        {
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
                else if (!_modified && _source == DependencyEffectiveSource.Expression && _expression!.Value != Expression.NoValue)
                    return _expression!.Value;
                else
                    return _value;
            }
        }

        public Expression? Expression => _expression;

        public bool IsValueModified => _modified;

        public void ModifyValue(object? value)
        {
            _modified = true;
            _value = value;
        }
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
