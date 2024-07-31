using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class QuaternionAnimationUsingKeyFrames : StructAnimationUsingKeyFrames<Quaternion, QuaternionKeyFrame, QuaternionKeyFrameCollection>
    {
        /// <summary>
        /// Creates a copy of this KeyFrameQuaternionAnimation.
        /// </summary>
        /// <returns>The copy</returns>
        public new QuaternionAnimationUsingKeyFrames Clone()
        {
            return (QuaternionAnimationUsingKeyFrames)base.Clone();
        }


        /// <summary>
        /// Returns a version of this class with all its base property values
        /// set to the current animated values and removes the animations.
        /// </summary>
        /// <returns>
        /// Since this class isn't animated, this method will always just return
        /// this instance of the class.
        /// </returns>
        public new QuaternionAnimationUsingKeyFrames CloneCurrentValue()
        {
            return (QuaternionAnimationUsingKeyFrames)base.CloneCurrentValue();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new QuaternionAnimationUsingKeyFrames();
        }

        protected override float GetSegmentLength(Quaternion from, Quaternion to) => AnimatedTypeHelpers.GetSegmentLengthQuaternion(from, to);

        protected override Quaternion Add(Quaternion value1, Quaternion value2) => AnimatedTypeHelpers.AddQuaternion(value1, value2);

        protected override Quaternion GetZeroValue(Quaternion value) => AnimatedTypeHelpers.GetZeroValueQuaternion(value);

        protected override Quaternion Scale(Quaternion value, float factor) => AnimatedTypeHelpers.ScaleQuaternion(value, factor);
    }
}
