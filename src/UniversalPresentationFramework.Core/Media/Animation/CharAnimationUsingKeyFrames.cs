using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class CharAnimationUsingKeyFrames : GenericAnimationUsingKeyFrames<char, CharKeyFrame, CharKeyFrameCollection>
    {
        /// <summary>
        /// Creates a copy of this KeyFrameCharAnimation.
        /// </summary>
        /// <returns>The copy</returns>
        public new CharAnimationUsingKeyFrames Clone()
        {
            return (CharAnimationUsingKeyFrames)base.Clone();
        }

        /// <summary>
        /// Returns a version of this class with all its base property values
        /// set to the current animated values and removes the animations.
        /// </summary>
        /// <returns>
        /// Since this class isn't animated, this method will always just return
        /// this instance of the class.
        /// </returns>
        public new CharAnimationUsingKeyFrames CloneCurrentValue()
        {
            return (CharAnimationUsingKeyFrames)base.CloneCurrentValue();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new CharAnimationUsingKeyFrames();
        }

        protected override float GetSegmentLength(char from, char to) => AnimatedTypeHelpers.GetSegmentLengthChar(from, to);
    }
}
