using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class SingleAnimationUsingKeyFrames : StructAnimationUsingKeyFrames<float, SingleKeyFrame, SingleKeyFrameCollection>
    {
        /// <summary>
        /// Creates a copy of this KeyFrameSingleAnimation.
        /// </summary>
        /// <returns>The copy</returns>
        public new SingleAnimationUsingKeyFrames Clone()
        {
            return (SingleAnimationUsingKeyFrames)base.Clone();
        }


        /// <summary>
        /// Returns a version of this class with all its base property values
        /// set to the current animated values and removes the animations.
        /// </summary>
        /// <returns>
        /// Since this class isn't animated, this method will always just return
        /// this instance of the class.
        /// </returns>
        public new SingleAnimationUsingKeyFrames CloneCurrentValue()
        {
            return (SingleAnimationUsingKeyFrames)base.CloneCurrentValue();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new SingleAnimationUsingKeyFrames();
        }

        protected override float GetSegmentLength(float from, float to) => AnimatedTypeHelpers.GetSegmentLengthSingle(from, to);

        protected override float Add(float value1, float value2) => AnimatedTypeHelpers.AddSingle(value1, value2);

        protected override float GetZeroValue(float value) => AnimatedTypeHelpers.GetZeroValueSingle(value);

        protected override float Scale(float value, float factor) => AnimatedTypeHelpers.ScaleSingle(value, factor);
    }
}
