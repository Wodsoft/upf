using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class QuaternionAnimation : StructAnimation<Quaternion>
    {
        public QuaternionAnimation()
        {
        }

        public QuaternionAnimation(Quaternion toValue, Duration duration) : base(toValue, duration)
        {
        }

        public QuaternionAnimation(Quaternion toValue, Duration duration, FillBehavior fillBehavior) : base(toValue, duration, fillBehavior)
        {
        }

        public QuaternionAnimation(Quaternion fromValue, Quaternion toValue, Duration duration) : base(fromValue, toValue, duration)
        {
        }

        public QuaternionAnimation(Quaternion fromValue, Quaternion toValue, Duration duration, FillBehavior fillBehavior) : base(fromValue, toValue, duration, fillBehavior)
        {
        }

        #region Freezable

        /// <summary>
        /// Creates a copy of this QuaternionAnimation
        /// </summary>
        /// <returns>The copy</returns>
        public new QuaternionAnimation Clone()
        {
            return (QuaternionAnimation)base.Clone();
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
            return new QuaternionAnimation();
        }

        #endregion

        protected override Quaternion Add(Quaternion value1, Quaternion value2) => AnimatedTypeHelpers.AddQuaternion(value1, value2);

        protected override bool IsValidAnimationValue(in Quaternion value) => AnimatedTypeHelpers.IsValidAnimationValueQuaternion(value);

        protected override Quaternion Scale(Quaternion value, float factor) => AnimatedTypeHelpers.ScaleQuaternion(value, factor);

        protected override Quaternion Subtract(Quaternion value1, Quaternion value2) => AnimatedTypeHelpers.SubtractQuaternion(value1, value2);
    }
}
