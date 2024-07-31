using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class DoubleAnimation : NumberAnimation<double>
    {
        public DoubleAnimation()
        {
        }

        public DoubleAnimation(double toValue, Duration duration) : base(toValue, duration)
        {
        }

        public DoubleAnimation(double toValue, Duration duration, FillBehavior fillBehavior) : base(toValue, duration, fillBehavior)
        {
        }

        public DoubleAnimation(double fromValue, double toValue, Duration duration) : base(fromValue, toValue, duration)
        {
        }

        public DoubleAnimation(double fromValue, double toValue, Duration duration, FillBehavior fillBehavior) : base(fromValue, toValue, duration, fillBehavior)
        {
        }

        #region Freezable

        /// <summary>
        /// Creates a copy of this DoubleAnimation
        /// </summary>
        /// <returns>The copy</returns>
        public new DoubleAnimation Clone()
        {
            return (DoubleAnimation)base.Clone();
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
            return new DoubleAnimation();
        }

        #endregion

        protected override double Scale(double value, float factor) => AnimatedTypeHelpers.ScaleDouble(value, factor);
    }
}