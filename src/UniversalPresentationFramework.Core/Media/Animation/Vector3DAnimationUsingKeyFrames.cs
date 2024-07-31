using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class Vector3DAnimationUsingKeyFrames : StructAnimationUsingKeyFrames<Vector3, Vector3DKeyFrame, Vector3DKeyFrameCollection>
    {
        /// <summary>
        /// Creates a copy of this KeyFrameVector3DAnimation.
        /// </summary>
        /// <returns>The copy</returns>
        public new Vector3DAnimationUsingKeyFrames Clone()
        {
            return (Vector3DAnimationUsingKeyFrames)base.Clone();
        }

        /// <summary>
        /// Returns a version of this class with all its base property values
        /// set to the current animated values and removes the animations.
        /// </summary>
        /// <returns>
        /// Since this class isn't animated, this method will always just return
        /// this instance of the class.
        /// </returns>
        public new Vector3DAnimationUsingKeyFrames CloneCurrentValue()
        {
            return (Vector3DAnimationUsingKeyFrames)base.CloneCurrentValue();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new Vector3DAnimationUsingKeyFrames();
        }

        protected override float GetSegmentLength(Vector3 from, Vector3 to) => AnimatedTypeHelpers.GetSegmentLengthVector3D(from, to);

        protected override Vector3 Add(Vector3 value1, Vector3 value2) => AnimatedTypeHelpers.AddVector3D(value1, value2);

        protected override Vector3 GetZeroValue(Vector3 value) => AnimatedTypeHelpers.GetZeroValueVector3D(value);

        protected override Vector3 Scale(Vector3 value, float factor) => AnimatedTypeHelpers.ScaleVector3D(value, factor);
    }
}
