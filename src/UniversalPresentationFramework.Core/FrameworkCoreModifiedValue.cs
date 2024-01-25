using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public class FrameworkCoreModifiedValue : IDependencyModifiedValue
    {
        private bool _hasAnimationValue;
        private object? _animationValue;

        public object? AnimationValue => _animationValue;

        public bool HasAnimationValue => _hasAnimationValue;

        public Expression? Expression { get; set; }

        public object? BaseValue { get; set; }

        public bool IsEmpty => !HasAnimationValue;

        public void SetAnimationValue(object? animationValue)
        {
            _hasAnimationValue = true;
            _animationValue = animationValue;
        }

        public void CleanAnimationValue()
        {
            _hasAnimationValue = false;
            _animationValue = null;
        }

        public virtual object? GetValue(ref DependencyEffectiveValue effectiveValue)
        {
            if (_hasAnimationValue)
                return _animationValue;
            return effectiveValue.Value;
        }

        public virtual FrameworkCoreModifiedValue Clone()
        {
            return new FrameworkCoreModifiedValue
            {
                _hasAnimationValue = _hasAnimationValue,
                _animationValue = _animationValue,
                BaseValue = BaseValue,
                Expression = Expression
            };
        }
    }
}
