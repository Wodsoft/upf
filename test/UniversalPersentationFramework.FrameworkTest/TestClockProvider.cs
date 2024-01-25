using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media.Animation;

namespace Wodsoft.UI.Test
{
    public class TestClockProvider : ClockProvider
    {
        private readonly List<Clock> _clocks = new List<Clock>();
        private readonly List<Clock> _needToRemove = new List<Clock>();
        private bool _isTicking;

        protected override void OnRegisterClock(DependencyObject d, Clock clock)
        {
            _clocks.Add(clock);
        }

        protected override void OnUnregisterClock(Clock clock)
        {
            if (_isTicking)
                _needToRemove.Add(clock);
            else
                _clocks.Remove(clock);
        }

        public void ApplyTick(TimeSpan tick)
        {
            _isTicking = true;
            foreach (var clock in _clocks)
            {
                OnApplyTick(tick, clock);
            }
            _isTicking = false;
            if (_needToRemove.Count != 0)
            {
                foreach (var clock in _needToRemove)
                    _clocks.Remove(clock);
                _needToRemove.Clear();
            }
        }
    }
}
