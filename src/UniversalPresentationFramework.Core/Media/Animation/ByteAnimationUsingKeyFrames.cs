using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class ByteAnimationUsingKeyFrames : StructAnimationUsingKeyFrames<byte, ByteKeyFrame, ByteKeyFrameCollection>
    {
        /// <summary>
        /// Creates a copy of this KeyFrameByteAnimation.
        /// </summary>
        /// <returns>The copy</returns>
        public new ByteAnimationUsingKeyFrames Clone()
        {
            return (ByteAnimationUsingKeyFrames)base.Clone();
        }

        /// <summary>
        /// Returns a version of this class with all its base property values
        /// set to the current animated values and removes the animations.
        /// </summary>
        /// <returns>
        /// Since this class isn't animated, this method will always just return
        /// this instance of the class.
        /// </returns>
        public new ByteAnimationUsingKeyFrames CloneCurrentValue()
        {
            return (ByteAnimationUsingKeyFrames)base.CloneCurrentValue();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new ByteAnimationUsingKeyFrames();
        }

        protected override float GetSegmentLength(byte from, byte to) => AnimatedTypeHelpers.GetSegmentLengthByte(from, to);

        protected override byte Add(byte value1, byte value2) => AnimatedTypeHelpers.AddByte(value1, value2);

        protected override byte GetZeroValue(byte value) => AnimatedTypeHelpers.GetZeroValueByte(value);

        protected override byte Scale(byte value, float factor) => AnimatedTypeHelpers.ScaleByte(value, factor);
    }
}
