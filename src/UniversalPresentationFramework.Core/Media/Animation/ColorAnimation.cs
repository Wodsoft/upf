using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class ColorAnimation : StructAnimation<Color>
    {
        public ColorAnimation()
        {
        }

        public ColorAnimation(Color toValue, Duration duration) : base(toValue, duration)
        {
        }

        public ColorAnimation(Color toValue, Duration duration, FillBehavior fillBehavior) : base(toValue, duration, fillBehavior)
        {
        }

        public ColorAnimation(Color fromValue, Color toValue, Duration duration) : base(fromValue, toValue, duration)
        {
        }

        public ColorAnimation(Color fromValue, Color toValue, Duration duration, FillBehavior fillBehavior) : base(fromValue, toValue, duration, fillBehavior)
        {
        }

        #region Freezable

        /// <summary>
        /// Creates a copy of this ColorAnimation
        /// </summary>
        /// <returns>The copy</returns>
        public new ColorAnimation Clone()
        {
            return (ColorAnimation)base.Clone();
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
            return new ColorAnimation();
        }

        #endregion

        protected override Color Add(Color value1, Color value2) => AnimatedTypeHelpers.AddColor(value1, value2);

        protected override bool IsValidAnimationValue(in Color value) => true;

        protected override Color Scale(Color value, float factor) => AnimatedTypeHelpers.ScaleColor(value, factor);

        protected override Color Subtract(Color value1, Color value2) => AnimatedTypeHelpers.SubtractColor(value1, value2);
    }
}
