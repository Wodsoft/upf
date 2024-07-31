using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class Int32AnimationUsingKeyFrames : StructAnimationUsingKeyFrames<int, Int32KeyFrame, Int32KeyFrameCollection>
    {
        /// <summary>
        /// Creates a copy of this KeyFrameInt32Animation.
        /// </summary>
        /// <returns>The copy</returns>
        public new Int32AnimationUsingKeyFrames Clone()
        {
            return (Int32AnimationUsingKeyFrames)base.Clone();
        }


        /// <summary>
        /// Returns a version of this class with all its base property values
        /// set to the current animated values and removes the animations.
        /// </summary>
        /// <returns>
        /// Since this class isn't animated, this method will always just return
        /// this instance of the class.
        /// </returns>
        public new Int32AnimationUsingKeyFrames CloneCurrentValue()
        {
            return (Int32AnimationUsingKeyFrames)base.CloneCurrentValue();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new Int32AnimationUsingKeyFrames();
        }

        protected override float GetSegmentLength(int from, int to) => AnimatedTypeHelpers.GetSegmentLengthInt32(from, to);

        protected override int Add(int value1, int value2) => AnimatedTypeHelpers.AddInt32(value1, value2);

        protected override int GetZeroValue(int value) => AnimatedTypeHelpers.GetZeroValueInt32(value);

        protected override int Scale(int value, float factor) => AnimatedTypeHelpers.ScaleInt32(value, factor);
    }
}
