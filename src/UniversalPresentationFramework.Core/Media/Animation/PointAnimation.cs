using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class PointAnimation : StructAnimation<Point>
    {
        public PointAnimation()
        {
        }

        public PointAnimation(Point toValue, Duration duration) : base(toValue, duration)
        {
        }

        public PointAnimation(Point toValue, Duration duration, FillBehavior fillBehavior) : base(toValue, duration, fillBehavior)
        {
        }

        public PointAnimation(Point fromValue, Point toValue, Duration duration) : base(fromValue, toValue, duration)
        {
        }

        public PointAnimation(Point fromValue, Point toValue, Duration duration, FillBehavior fillBehavior) : base(fromValue, toValue, duration, fillBehavior)
        {
        }

        #region Freezable

        /// <summary>
        /// Creates a copy of this PointAnimation
        /// </summary>
        /// <returns>The copy</returns>
        public new PointAnimation Clone()
        {
            return (PointAnimation)base.Clone();
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
            return new PointAnimation();
        }

        #endregion

        protected override Point Add(Point value1, Point value2) => AnimatedTypeHelpers.AddPoint(value1, value2);

        protected override bool IsValidAnimationValue(in Point value) => AnimatedTypeHelpers.IsValidAnimationValuePoint(value);

        protected override Point Scale(Point value, float factor) => AnimatedTypeHelpers.ScalePoint(value, factor);

        protected override Point Subtract(Point value1, Point value2) => AnimatedTypeHelpers.SubtractPoint(value1, value2);
    }
}
