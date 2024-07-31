using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class Int16AnimationUsingKeyFrames : StructAnimationUsingKeyFrames<short, Int16KeyFrame, Int16KeyFrameCollection>
    {
        /// <summary>
        /// Creates a copy of this KeyFrameInt16Animation.
        /// </summary>
        /// <returns>The copy</returns>
        public new Int16AnimationUsingKeyFrames Clone()
        {
            return (Int16AnimationUsingKeyFrames)base.Clone();
        }

        /// <summary>
        /// Returns a version of this class with all its base property values
        /// set to the current animated values and removes the animations.
        /// </summary>
        /// <returns>
        /// Since this class isn't animated, this method will always just return
        /// this instance of the class.
        /// </returns>
        public new Int16AnimationUsingKeyFrames CloneCurrentValue()
        {
            return (Int16AnimationUsingKeyFrames)base.CloneCurrentValue();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new Int16AnimationUsingKeyFrames();
        }

        protected override float GetSegmentLength(short from, short to) => AnimatedTypeHelpers.GetSegmentLengthInt16(from, to);

        protected override short Add(short value1, short value2) => AnimatedTypeHelpers.AddInt16(value1, value2);

        protected override short GetZeroValue(short value) => AnimatedTypeHelpers.GetZeroValueInt16(value);

        protected override short Scale(short value, float factor) => AnimatedTypeHelpers.ScaleInt16(value, factor);
    }
}
