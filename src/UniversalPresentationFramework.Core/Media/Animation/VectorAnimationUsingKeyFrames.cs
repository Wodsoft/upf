using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class VectorAnimationUsingKeyFrames : StructAnimationUsingKeyFrames<Vector2, VectorKeyFrame, VectorKeyFrameCollection>
    {
        /// <summary>
        /// Creates a copy of this KeyFrameVectorAnimation.
        /// </summary>
        /// <returns>The copy</returns>
        public new VectorAnimationUsingKeyFrames Clone()
        {
            return (VectorAnimationUsingKeyFrames)base.Clone();
        }


        /// <summary>
        /// Returns a version of this class with all its base property values
        /// set to the current animated values and removes the animations.
        /// </summary>
        /// <returns>
        /// Since this class isn't animated, this method will always just return
        /// this instance of the class.
        /// </returns>
        public new VectorAnimationUsingKeyFrames CloneCurrentValue()
        {
            return (VectorAnimationUsingKeyFrames)base.CloneCurrentValue();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new VectorAnimationUsingKeyFrames();
        }

        protected override float GetSegmentLength(Vector2 from, Vector2 to) => AnimatedTypeHelpers.GetSegmentLengthVector(from, to);

        protected override Vector2 Add(Vector2 value1, Vector2 value2) => AnimatedTypeHelpers.AddVector(value1, value2);

        protected override Vector2 GetZeroValue(Vector2 value) => AnimatedTypeHelpers.GetZeroValueVector(value);

        protected override Vector2 Scale(Vector2 value, float factor) => AnimatedTypeHelpers.ScaleVector(value, factor);
    }
}
