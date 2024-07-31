using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using Wodsoft.UI.Markup;

namespace Wodsoft.UI.Media.Animation
{
    /// <summary>
    /// This class is used to animate a Boolean property value along a set
    /// of key frames.
    /// </summary>
    public class BooleanAnimationUsingKeyFrames : GenericAnimationUsingKeyFrames<bool, BooleanKeyFrame, BooleanKeyFrameCollection>
    {
        #region Freezable

        /// <summary>
        /// Creates a copy of this KeyFrameBooleanAnimation.
        /// </summary>
        /// <returns>The copy</returns>
        public new BooleanAnimationUsingKeyFrames Clone()
        {
            return (BooleanAnimationUsingKeyFrames)base.Clone();
        }

        /// <summary>
        /// Returns a version of this class with all its base property values
        /// set to the current animated values and removes the animations.
        /// </summary>
        /// <returns>
        /// Since this class isn't animated, this method will always just return
        /// this instance of the class.
        /// </returns>
        public new BooleanAnimationUsingKeyFrames CloneCurrentValue()
        {
            return (BooleanAnimationUsingKeyFrames)base.CloneCurrentValue();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new BooleanAnimationUsingKeyFrames();
        }

        #endregion  // Freezable

        protected override float GetSegmentLength(bool from, bool to)
        {
            if (from != to)
            {
                return 1.0f;
            }
            else
            {
                return 0.0f;
            }
        }
    }
}
