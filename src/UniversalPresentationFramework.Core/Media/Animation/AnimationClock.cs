using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class AnimationClock : Clock
    {
        protected internal AnimationClock(AnimationTimeline timeline) : base(timeline)
        {
        }

        public new AnimationTimeline Timeline
        {
            get
            {
                return (AnimationTimeline)base.Timeline;
            }
        }

        public object? GetCurrentValue(object? defaultOriginValue, object? defaultDestinationValue)
        {
            return Timeline.GetCurrentValue(defaultOriginValue, defaultDestinationValue, this);
        }
    }
}
