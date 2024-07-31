using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class Int64AnimationUsingKeyFrames : StructAnimationUsingKeyFrames<long, Int64KeyFrame, Int64KeyFrameCollection>
    {
        /// <summary>
        /// Creates a copy of this KeyFrameInt64Animation.
        /// </summary>
        /// <returns>The copy</returns>
        public new Int64AnimationUsingKeyFrames Clone()
        {
            return (Int64AnimationUsingKeyFrames)base.Clone();
        }


        /// <summary>
        /// Returns a version of this class with all its base property values
        /// set to the current animated values and removes the animations.
        /// </summary>
        /// <returns>
        /// Since this class isn't animated, this method will always just return
        /// this instance of the class.
        /// </returns>
        public new Int64AnimationUsingKeyFrames CloneCurrentValue()
        {
            return (Int64AnimationUsingKeyFrames)base.CloneCurrentValue();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new Int64AnimationUsingKeyFrames();
        }

        protected override float GetSegmentLength(long from, long to) => AnimatedTypeHelpers.GetSegmentLengthInt64(from, to);

        protected override long Add(long value1, long value2) => AnimatedTypeHelpers.AddInt64(value1, value2);

        protected override long GetZeroValue(long value) => AnimatedTypeHelpers.GetZeroValueInt64(value);

        protected override long Scale(long value, float factor) => AnimatedTypeHelpers.ScaleInt64(value, factor);
    }
}
