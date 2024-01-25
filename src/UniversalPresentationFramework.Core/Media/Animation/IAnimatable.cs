using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public interface IAnimatable
    {
        void ApplyAnimationClock(DependencyProperty dp, AnimationClock clock, HandoffBehavior handoffBehavior);

        void BeginAnimation(DependencyProperty dp, AnimationTimeline animation);
    }
}
