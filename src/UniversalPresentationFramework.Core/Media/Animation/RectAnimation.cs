using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class RectAnimation : StructAnimation<Rect>
    {
        public RectAnimation()
        {
        }

        public RectAnimation(Rect toValue, Duration duration) : base(toValue, duration)
        {
        }

        public RectAnimation(Rect toValue, Duration duration, FillBehavior fillBehavior) : base(toValue, duration, fillBehavior)
        {
        }

        public RectAnimation(Rect fromValue, Rect toValue, Duration duration) : base(fromValue, toValue, duration)
        {
        }

        public RectAnimation(Rect fromValue, Rect toValue, Duration duration, FillBehavior fillBehavior) : base(fromValue, toValue, duration, fillBehavior)
        {
        }

        #region Freezable

        /// <summary>
        /// Creates a copy of this RectAnimation
        /// </summary>
        /// <returns>The copy</returns>
        public new RectAnimation Clone()
        {
            return (RectAnimation)base.Clone();
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
            return new RectAnimation();
        }

        #endregion

        protected override Rect Add(Rect value1, Rect value2) => AnimatedTypeHelpers.AddRect(value1, value2);

        protected override bool IsValidAnimationValue(in Rect value) => AnimatedTypeHelpers.IsValidAnimationValueRect(value);

        protected override Rect Scale(Rect value, float factor) => AnimatedTypeHelpers.ScaleRect(value, factor);

        protected override Rect Subtract(Rect value1, Rect value2) => AnimatedTypeHelpers.SubtractRect(value1, value2);
    }
}
