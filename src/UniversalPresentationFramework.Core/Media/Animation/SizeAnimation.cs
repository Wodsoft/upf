using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class SizeAnimation : StructAnimation<Size>
    {
        public SizeAnimation()
        {
        }

        public SizeAnimation(Size toValue, Duration duration) : base(toValue, duration)
        {
        }

        public SizeAnimation(Size toValue, Duration duration, FillBehavior fillBehavior) : base(toValue, duration, fillBehavior)
        {
        }

        public SizeAnimation(Size fromValue, Size toValue, Duration duration) : base(fromValue, toValue, duration)
        {
        }

        public SizeAnimation(Size fromValue, Size toValue, Duration duration, FillBehavior fillBehavior) : base(fromValue, toValue, duration, fillBehavior)
        {
        }

        #region Freezable

        /// <summary>
        /// Creates a copy of this SizeAnimation
        /// </summary>
        /// <returns>The copy</returns>
        public new SizeAnimation Clone()
        {
            return (SizeAnimation)base.Clone();
        }

        //
        // Note that we don't override the Clone virtuals (CloneCore, CloneCurrentValueCore,
        // GetAsFrozenCore, and GetCurrentValueAsFrozenCore) even though this class has state
        // not stored in a DP.
        // 
        // We don't need to clone _animationType and _keyValues because they are the the cached 
        // results of animation function validation, which can be recomputed.  The other remaining
        // field, isAnimationFunctionValid, defaults to false, which causes this recomputation to happen.
        //

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new SizeAnimation();
        }

        #endregion

        protected override Size Add(Size value1, Size value2) => AnimatedTypeHelpers.AddSize(value1, value2);

        protected override bool IsValidAnimationValue(in Size value) => AnimatedTypeHelpers.IsValidAnimationValueSize(value);

        protected override Size Scale(Size value, float factor) => AnimatedTypeHelpers.ScaleSize(value, factor);

        protected override Size Subtract(Size value1, Size value2) => AnimatedTypeHelpers.SubtractSize(value1, value2);
    }
}
