using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public abstract class AnimationTimeline : Timeline
    {
        public static readonly DependencyProperty IsAdditiveProperty =
            DependencyProperty.Register(
                "IsAdditive",               // Property Name
                typeof(bool),               // Property Type
                typeof(AnimationTimeline),  // Owner Class
                new PropertyMetadata(false));

        public static readonly DependencyProperty IsCumulativeProperty =
            DependencyProperty.Register(
                "IsCumulative",             // Property Name
                typeof(bool),               // Property Type
                typeof(AnimationTimeline),  // Owner Class
                new PropertyMetadata(false));

        public abstract object? GetCurrentValue(object? defaultOriginValue, object? defaultDestinationValue, AnimationClock clock);

        public new AnimationClock CreateClock()
        {
            return (AnimationClock)base.CreateClock();
        }

        protected internal override Clock AllocateClock()
        {
            return new AnimationClock(this);
        }

        /// <summary>
        /// Returns the type of the animation.
        /// </summary>
        /// <value></value>
        public abstract Type TargetPropertyType { get; }
    }
}
