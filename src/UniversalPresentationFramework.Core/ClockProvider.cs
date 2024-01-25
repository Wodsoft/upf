using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media.Animation;

namespace Wodsoft.UI
{
    public abstract class ClockProvider : IClockProvider
    {
        public void RegisterClock(DependencyObject d, Clock clock)
        {
            if (d == null)
                throw new ArgumentNullException(nameof(d));
            if (clock == null)
                throw new ArgumentNullException(nameof(clock));
            clock.Completed += Clock_Completed;
            OnRegisterClock(d, clock);
        }

        private void Clock_Completed(object? sender, EventArgs e)
        {
            var clock = (Clock?)sender;
            if (clock != null)
            {
                clock.Completed -= Clock_Completed;
                OnUnregisterClock(clock);
            }
        }

        protected abstract void OnRegisterClock(DependencyObject d, Clock clock);

        protected abstract void OnUnregisterClock(Clock clock);

        protected void OnApplyTick(TimeSpan tick, Clock clock)
        {
            clock.ApplyTick(tick);
        }
    }
}
