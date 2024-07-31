using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class DecimalAnimationUsingKeyFrames : StructAnimationUsingKeyFrames<decimal, DecimalKeyFrame, DecimalKeyFrameCollection>
    {
        /// <summary>
        /// Creates a copy of this KeyFrameDecimalAnimation.
        /// </summary>
        /// <returns>The copy</returns>
        public new DecimalAnimationUsingKeyFrames Clone()
        {
            return (DecimalAnimationUsingKeyFrames)base.Clone();
        }

        /// <summary>
        /// Returns a version of this class with all its base property values
        /// set to the current animated values and removes the animations.
        /// </summary>
        /// <returns>
        /// Since this class isn't animated, this method will always just return
        /// this instance of the class.
        /// </returns>
        public new DecimalAnimationUsingKeyFrames CloneCurrentValue()
        {
            return (DecimalAnimationUsingKeyFrames)base.CloneCurrentValue();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new DecimalAnimationUsingKeyFrames();
        }

        protected override float GetSegmentLength(decimal from, decimal to) => AnimatedTypeHelpers.GetSegmentLengthDecimal(from, to);

        protected override decimal Add(decimal value1, decimal value2) => AnimatedTypeHelpers.AddDecimal(value1, value2);

        protected override decimal GetZeroValue(decimal value) => AnimatedTypeHelpers.GetZeroValueDecimal(value);

        protected override decimal Scale(decimal value, float factor) => AnimatedTypeHelpers.ScaleDecimal(value, factor);
    }
}
