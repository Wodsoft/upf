using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class PointAnimationUsingKeyFrames : StructAnimationUsingKeyFrames<Point, PointKeyFrame, PointKeyFrameCollection>
    {
        /// <summary>
        /// Creates a copy of this KeyFramePointAnimation.
        /// </summary>
        /// <returns>The copy</returns>
        public new PointAnimationUsingKeyFrames Clone()
        {
            return (PointAnimationUsingKeyFrames)base.Clone();
        }


        /// <summary>
        /// Returns a version of this class with all its base property values
        /// set to the current animated values and removes the animations.
        /// </summary>
        /// <returns>
        /// Since this class isn't animated, this method will always just return
        /// this instance of the class.
        /// </returns>
        public new PointAnimationUsingKeyFrames CloneCurrentValue()
        {
            return (PointAnimationUsingKeyFrames)base.CloneCurrentValue();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new PointAnimationUsingKeyFrames();
        }

        protected override float GetSegmentLength(Point from, Point to) => AnimatedTypeHelpers.GetSegmentLengthPoint(from, to);

        protected override Point Add(Point value1, Point value2) => AnimatedTypeHelpers.AddPoint(value1, value2);

        protected override Point GetZeroValue(Point value) => AnimatedTypeHelpers.GetZeroValuePoint(value);

        protected override Point Scale(Point value, float factor) => AnimatedTypeHelpers.ScalePoint(value, factor);
    }
}
