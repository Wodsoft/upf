using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class SizeAnimationUsingKeyFrames : StructAnimationUsingKeyFrames<Size, SizeKeyFrame, SizeKeyFrameCollection>
    {
        /// <summary>
        /// Creates a copy of this KeyFrameSizeAnimation.
        /// </summary>
        /// <returns>The copy</returns>
        public new SizeAnimationUsingKeyFrames Clone()
        {
            return (SizeAnimationUsingKeyFrames)base.Clone();
        }


        /// <summary>
        /// Returns a version of this class with all its base property values
        /// set to the current animated values and removes the animations.
        /// </summary>
        /// <returns>
        /// Since this class isn't animated, this method will always just return
        /// this instance of the class.
        /// </returns>
        public new SizeAnimationUsingKeyFrames CloneCurrentValue()
        {
            return (SizeAnimationUsingKeyFrames)base.CloneCurrentValue();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new SizeAnimationUsingKeyFrames();
        }

        protected override float GetSegmentLength(Size from, Size to) => AnimatedTypeHelpers.GetSegmentLengthSize(from, to);

        protected override Size Add(Size value1, Size value2) => AnimatedTypeHelpers.AddSize(value1, value2);

        protected override Size GetZeroValue(Size value) => AnimatedTypeHelpers.GetZeroValueSize(value);

        protected override Size Scale(Size value, float factor) => AnimatedTypeHelpers.ScaleSize(value, factor);
    }
}
