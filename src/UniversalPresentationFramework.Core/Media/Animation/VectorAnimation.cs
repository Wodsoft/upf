using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class VectorAnimation : StructAnimation<Vector2>
    {
        public VectorAnimation()
        {
        }

        public VectorAnimation(Vector2 toValue, Duration duration) : base(toValue, duration)
        {
        }

        public VectorAnimation(Vector2 toValue, Duration duration, FillBehavior fillBehavior) : base(toValue, duration, fillBehavior)
        {
        }

        public VectorAnimation(Vector2 fromValue, Vector2 toValue, Duration duration) : base(fromValue, toValue, duration)
        {
        }

        public VectorAnimation(Vector2 fromValue, Vector2 toValue, Duration duration, FillBehavior fillBehavior) : base(fromValue, toValue, duration, fillBehavior)
        {
        }

        #region Freezable

        /// <summary>
        /// Creates a copy of this VectorAnimation
        /// </summary>
        /// <returns>The copy</returns>
        public new VectorAnimation Clone()
        {
            return (VectorAnimation)base.Clone();
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
            return new VectorAnimation();
        }

        #endregion

        protected override Vector2 Add(Vector2 value1, Vector2 value2) => AnimatedTypeHelpers.AddVector(value1, value2);

        protected override bool IsValidAnimationValue(in Vector2 value) => AnimatedTypeHelpers.IsValidAnimationValueVector(value);

        protected override Vector2 Scale(Vector2 value, float factor) => AnimatedTypeHelpers.ScaleVector(value, factor);

        protected override Vector2 Subtract(Vector2 value1, Vector2 value2) => AnimatedTypeHelpers.SubtractVector(value1, value2);
    }
}
