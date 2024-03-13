using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public abstract class Animatable : Freezable, IAnimatable
    {
        protected override bool FreezeCore(bool isChecking)
        {
            if (AnimationStorage.HasAnimation(this))
                return false;
            return base.FreezeCore(isChecking);
        }

        public void ApplyAnimationClock(DependencyProperty dp, AnimationClock clock, HandoffBehavior handoffBehavior)
        {
            AnimationStorage.ApplyClock(this, dp, clock, handoffBehavior);
        }

        public void BeginAnimation(DependencyProperty dp, AnimationTimeline animation)
        {
            var clock = animation.CreateClock();
            ApplyAnimationClock(dp, clock, HandoffBehavior.SnapshotAndReplace);
        }
    }
}
