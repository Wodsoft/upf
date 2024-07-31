using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class Vector3DAnimation : StructAnimation<Vector3>
    {
        public Vector3DAnimation()
        {
        }

        public Vector3DAnimation(Vector3 toValue, Duration duration) : base(toValue, duration)
        {
        }

        public Vector3DAnimation(Vector3 toValue, Duration duration, FillBehavior fillBehavior) : base(toValue, duration, fillBehavior)
        {
        }

        public Vector3DAnimation(Vector3 fromValue, Vector3 toValue, Duration duration) : base(fromValue, toValue, duration)
        {
        }

        public Vector3DAnimation(Vector3 fromValue, Vector3 toValue, Duration duration, FillBehavior fillBehavior) : base(fromValue, toValue, duration, fillBehavior)
        {
        }

        #region Freezable

        /// <summary>
        /// Creates a copy of this Vector3DAnimation
        /// </summary>
        /// <returns>The copy</returns>
        public new Vector3DAnimation Clone()
        {
            return (Vector3DAnimation)base.Clone();
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
            return new Vector3DAnimation();
        }

        #endregion


        protected override Vector3 Add(Vector3 value1, Vector3 value2) => AnimatedTypeHelpers.AddVector3D(value1, value2);

        protected override bool IsValidAnimationValue(in Vector3 value) => AnimatedTypeHelpers.IsValidAnimationValueVector3D(value);

        protected override Vector3 Scale(Vector3 value, float factor) => AnimatedTypeHelpers.ScaleVector3D(value, factor);

        protected override Vector3 Subtract(Vector3 value1, Vector3 value2) => AnimatedTypeHelpers.SubtractVector3D(value1, value2);
    }
}
