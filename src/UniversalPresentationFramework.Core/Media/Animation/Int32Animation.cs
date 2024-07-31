using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class Int32Animation : NumberAnimation<int>
    {
        public Int32Animation()
        {
        }

        public Int32Animation(int toValue, Duration duration) : base(toValue, duration)
        {
        }

        public Int32Animation(int toValue, Duration duration, FillBehavior fillBehavior) : base(toValue, duration, fillBehavior)
        {
        }

        public Int32Animation(int fromValue, int toValue, Duration duration) : base(fromValue, toValue, duration)
        {
        }

        public Int32Animation(int fromValue, int toValue, Duration duration, FillBehavior fillBehavior) : base(fromValue, toValue, duration, fillBehavior)
        {
        }

        #region Freezable

        /// <summary>
        /// Creates a copy of this Int32Animation
        /// </summary>
        /// <returns>The copy</returns>
        public new Int32Animation Clone()
        {
            return (Int32Animation)base.Clone();
        }

        //
        // Note that we don't override the Clone virtuals (CloneCore, CloneCurrentValueCore,
        // GetAsFrozenCore, and GetCurrentValueAsFrozenCore) even though this class has state
        // not stored in a DP.
        // 
        // We don't need to clone _animationType and _keyValues because they are the the cached 
        // results of animation function validation, which can be recomputed.  The other remaining
        // field, isAnimationFunctionValid, defaults to false, which causes this recomputation to happen.
        //

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new Int32Animation();
        }

        #endregion

        protected override int Scale(int value, float factor) => AnimatedTypeHelpers.ScaleInt32(value, factor);
    }
}
