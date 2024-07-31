using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class StringAnimationUsingKeyFrames : GenericAnimationUsingKeyFrames<string, StringKeyFrame, StringKeyFrameCollection>
    {
        /// <summary>
        /// Creates a copy of this KeyFrameStringAnimation.
        /// </summary>
        /// <returns>The copy</returns>
        public new StringAnimationUsingKeyFrames Clone()
        {
            return (StringAnimationUsingKeyFrames)base.Clone();
        }


        /// <summary>
        /// Returns a version of this class with all its base property values
        /// set to the current animated values and removes the animations.
        /// </summary>
        /// <returns>
        /// Since this class isn't animated, this method will always just return
        /// this instance of the class.
        /// </returns>
        public new StringAnimationUsingKeyFrames CloneCurrentValue()
        {
            return (StringAnimationUsingKeyFrames)base.CloneCurrentValue();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new StringAnimationUsingKeyFrames();
        }

        protected override float GetSegmentLength(string from, string to) => AnimatedTypeHelpers.GetSegmentLengthString(from, to);
    }
}
