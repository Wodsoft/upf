using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class MatrixAnimationUsingKeyFrames : GenericAnimationUsingKeyFrames<Matrix3x2, MatrixKeyFrame, MatrixKeyFrameCollection>
    {

        /// <summary>
        /// Creates a copy of this KeyFrameMatrixAnimation.
        /// </summary>
        /// <returns>The copy</returns>
        public new MatrixAnimationUsingKeyFrames Clone()
        {
            return (MatrixAnimationUsingKeyFrames)base.Clone();
        }


        /// <summary>
        /// Returns a version of this class with all its base property values
        /// set to the current animated values and removes the animations.
        /// </summary>
        /// <returns>
        /// Since this class isn't animated, this method will always just return
        /// this instance of the class.
        /// </returns>
        public new MatrixAnimationUsingKeyFrames CloneCurrentValue()
        {
            return (MatrixAnimationUsingKeyFrames)base.CloneCurrentValue();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new MatrixAnimationUsingKeyFrames();
        }

        protected override float GetSegmentLength(Matrix3x2 from, Matrix3x2 to) => AnimatedTypeHelpers.GetSegmentLengthMatrix(from, to);
    }
}
