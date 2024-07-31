using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public abstract class GenericAnimationBase<T> : AnimationTimeline
    {
        public override sealed object? GetCurrentValue(object? defaultOriginValue, object? defaultDestinationValue, AnimationClock animationClock)
        {
            // Verify that object arguments are non-null since we are a value type
            if (defaultOriginValue == null)
            {
                throw new ArgumentNullException("defaultOriginValue");
            }
            if (defaultDestinationValue == null)
            {
                throw new ArgumentNullException("defaultDestinationValue");
            }
            return GetCurrentValue((T)defaultOriginValue, (T)defaultDestinationValue, animationClock);
        }

        public override sealed Type TargetPropertyType
        {
            get
            {
                ReadPreamble();

                return typeof(T);
            }
        }

        public T GetCurrentValue(T defaultOriginValue, T defaultDestinationValue, AnimationClock animationClock)
        {
            ReadPreamble();

            if (animationClock == null)
            {
                throw new ArgumentNullException("animationClock");
            }

            // We check for null above but presharp doesn't notice so we suppress the 
            // warning here.

            //if (animationClock.CurrentState == ClockState.Stopped)
            //{
            //    return defaultDestinationValue;
            //}

            /*
            if (!IsValidAnimationValue(defaultDestinationValue))
            {
                throw new ArgumentException(
                    SR.Get(
                        SRID.Animation_InvalidBaseValue,
                        defaultDestinationValue, 
                        defaultDestinationValue.GetType(), 
                        GetType()),
                        "defaultDestinationValue");
            }
            */

            return GetCurrentValueCore(defaultOriginValue, defaultDestinationValue, animationClock);
        }

        protected abstract T GetCurrentValueCore(T defaultOriginValue, T defaultDestinationValue, AnimationClock animationClock);
    }
}
