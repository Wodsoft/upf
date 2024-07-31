using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class SingleAnimation : NumberAnimation<float>
    {
        public SingleAnimation()
        {
        }

        public SingleAnimation(float toValue, Duration duration) : base(toValue, duration)
        {
        }

        public SingleAnimation(float toValue, Duration duration, FillBehavior fillBehavior) : base(toValue, duration, fillBehavior)
        {
        }

        public SingleAnimation(float fromValue, float toValue, Duration duration) : base(fromValue, toValue, duration)
        {
        }

        public SingleAnimation(float fromValue, float toValue, Duration duration, FillBehavior fillBehavior) : base(fromValue, toValue, duration, fillBehavior)
        {
        }

        /// <summary>
        /// Creates a copy of this SingleAnimation
        /// </summary>
        /// <returns>The copy</returns>
        public new SingleAnimation Clone()
        {
            return (SingleAnimation)base.Clone();
        }

        protected override Freezable CreateInstanceCore()
        {
            return new SingleAnimation();
        }

        protected override float Scale(float value, float factor) => AnimatedTypeHelpers.ScaleSingle(value, factor);
    }
}
