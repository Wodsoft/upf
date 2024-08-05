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

        protected static DependencyProperty RegisterProperty(
            string name,
            Type propertyType,
            Type ownerType,
            object? defaultValue,
            PropertyChangedCallback? changed,
            ValidateValueCallback? validate,
            bool isIndependentlyAnimated,
            CoerceValueCallback? coerced)
        {
            // Override metadata for this particular object type. This defines
            // the methods that will be called when property actions (setting,
            // getting, invalidating) are taken for this specific object type.

            UIPropertyMetadata propertyMetadata;

            //// If this property is animated using a property resource, we create
            //// AnimatablePropertyMetadata instead of UIPropertyMetadata.

            propertyMetadata = new UIPropertyMetadata(defaultValue);

            propertyMetadata.PropertyChangedCallback = changed;

            if (coerced != null)
            {
                propertyMetadata.CoerceValueCallback = coerced;
            }

            // Register property with passed in default metadata.  The type of
            // defaultMetadata will determine whether this property is animatable.
            DependencyProperty dp = DependencyProperty.Register(
                name,
                propertyType,
                ownerType,
                propertyMetadata,
                validate);

            return dp;
        }
    }
}
