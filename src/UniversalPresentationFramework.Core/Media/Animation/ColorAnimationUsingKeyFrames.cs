using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class ColorAnimationUsingKeyFrames : StructAnimationUsingKeyFrames<Color, ColorKeyFrame, ColorKeyFrameCollection>
    {
        /// <summary>
        /// Creates a copy of this KeyFrameColorAnimation.
        /// </summary>
        /// <returns>The copy</returns>
        public new ColorAnimationUsingKeyFrames Clone()
        {
            return (ColorAnimationUsingKeyFrames)base.Clone();
        }

        /// <summary>
        /// Returns a version of this class with all its base property values
        /// set to the current animated values and removes the animations.
        /// </summary>
        /// <returns>
        /// Since this class isn't animated, this method will always just return
        /// this instance of the class.
        /// </returns>
        public new ColorAnimationUsingKeyFrames CloneCurrentValue()
        {
            return (ColorAnimationUsingKeyFrames)base.CloneCurrentValue();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new ColorAnimationUsingKeyFrames();
        }

        protected override float GetSegmentLength(Color from, Color to) => AnimatedTypeHelpers.GetSegmentLengthColor(from, to);

        protected override Color Add(Color value1, Color value2) => AnimatedTypeHelpers.AddColor(value1, value2);

        protected override Color GetZeroValue(Color value) => AnimatedTypeHelpers.GetZeroValueColor(value);

        protected override Color Scale(Color value, float factor) => AnimatedTypeHelpers.ScaleColor(value, factor);
    }
}
