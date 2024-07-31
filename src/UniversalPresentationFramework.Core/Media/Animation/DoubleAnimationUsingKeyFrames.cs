using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class DoubleAnimationUsingKeyFrames : StructAnimationUsingKeyFrames<double, DoubleKeyFrame, DoubleKeyFrameCollection>
    {
        /// <summary>
        /// Creates a copy of this KeyFrameDoubleAnimation.
        /// </summary>
        /// <returns>The copy</returns>
        public new DoubleAnimationUsingKeyFrames Clone()
        {
            return (DoubleAnimationUsingKeyFrames)base.Clone();
        }


        /// <summary>
        /// Returns a version of this class with all its base property values
        /// set to the current animated values and removes the animations.
        /// </summary>
        /// <returns>
        /// Since this class isn't animated, this method will always just return
        /// this instance of the class.
        /// </returns>
        public new DoubleAnimationUsingKeyFrames CloneCurrentValue()
        {
            return (DoubleAnimationUsingKeyFrames)base.CloneCurrentValue();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new DoubleAnimationUsingKeyFrames();
        }

        protected override float GetSegmentLength(double from, double to) => AnimatedTypeHelpers.GetSegmentLengthDouble(from, to);

        protected override double Add(double value1, double value2) => AnimatedTypeHelpers.AddDouble(value1, value2);

        protected override double GetZeroValue(double value) => AnimatedTypeHelpers.GetZeroValueDouble(value);

        protected override double Scale(double value, float factor) => AnimatedTypeHelpers.ScaleDouble(value, factor);
    }
}
