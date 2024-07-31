using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class RectAnimationUsingKeyFrames : StructAnimationUsingKeyFrames<Rect, RectKeyFrame, RectKeyFrameCollection>
    {
        /// <summary>
        /// Creates a copy of this KeyFrameRectAnimation.
        /// </summary>
        /// <returns>The copy</returns>
        public new RectAnimationUsingKeyFrames Clone()
        {
            return (RectAnimationUsingKeyFrames)base.Clone();
        }


        /// <summary>
        /// Returns a version of this class with all its base property values
        /// set to the current animated values and removes the animations.
        /// </summary>
        /// <returns>
        /// Since this class isn't animated, this method will always just return
        /// this instance of the class.
        /// </returns>
        public new RectAnimationUsingKeyFrames CloneCurrentValue()
        {
            return (RectAnimationUsingKeyFrames)base.CloneCurrentValue();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new RectAnimationUsingKeyFrames();
        }

        protected override float GetSegmentLength(Rect from, Rect to) => AnimatedTypeHelpers.GetSegmentLengthRect(from, to);

        protected override Rect Add(Rect value1, Rect value2) => AnimatedTypeHelpers.AddRect(value1, value2);

        protected override Rect GetZeroValue(Rect value) => AnimatedTypeHelpers.GetZeroValueRect(value);

        protected override Rect Scale(Rect value, float factor) => AnimatedTypeHelpers.ScaleRect(value, factor);
    }
}
