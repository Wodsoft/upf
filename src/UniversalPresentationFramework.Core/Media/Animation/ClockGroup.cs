using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class ClockGroup : Clock
    {
        private readonly Clock[] _clocks;
        private readonly IReadOnlyList<Clock> _readonlyClocks;

        public ClockGroup(TimelineGroup timelineGroup) : base(timelineGroup)
        {
            _clocks = new Clock[timelineGroup.Children.Count];
            for (int i = 0; i < timelineGroup.Children.Count; i++)
            {
                _clocks[i] = timelineGroup.Children[i].AllocateClock();
            }
            _readonlyClocks = _clocks.AsReadOnly();
        }

        public IReadOnlyList<Clock> Children => _readonlyClocks;

        public override void Measure()
        {
            for (int i = 0; i < _clocks.Length; i++)
            {
                _clocks[i].Measure();
            }
            base.Measure();
        }

        protected override Duration MeasureDuration()
        {
            var duration = Timeline.Duration;
            if (duration == Duration.Forever)
                return duration;
            if (duration == Duration.Automatic)
            {
                return _clocks.Max(t => t.TotalDuration);
            }
            else
                return duration;
        }

        protected override ClockState ComputeState(ref TimeSpan tick, ref TimeSpan totalTime, ref TimeSpan currentTime, ref int iteration, ref bool isReverse)
        {
            var state = base.ComputeState(ref tick, ref totalTime, ref currentTime, ref iteration, ref isReverse);
            for (int i = 0; i < _clocks.Length; i++)
            {
                if (Duration == Duration.Forever)
                {
                    _clocks[i].ApplyTick(tick);
                }
                else
                {
                    if (isReverse)
                        _clocks[i].ApplyTimeSpan(Duration.TimeSpan - currentTime, IsExpired);
                    else
                        _clocks[i].ApplyTimeSpan(currentTime, IsExpired);
                }
            }
            return state;
        }

        protected override ClockState ComputeState(TimeSpan totalTime, out TimeSpan currentTime, out int iteration, out bool isReverse)
        {
            var state = base.ComputeState(totalTime, out currentTime, out iteration, out isReverse);
            for (int i = 0; i < _clocks.Length; i++)
            {
                if (isReverse)
                    _clocks[i].ApplyTimeSpan(Duration.TimeSpan - currentTime, IsExpired);
                else
                    _clocks[i].ApplyTimeSpan(currentTime, IsExpired);
            }
            return state;
        }

        protected internal override void OnRootRemoveRequest()
        {
            base.OnRootRemoveRequest();
            for (int i = 0; i < _clocks.Length; i++)
            {
                _clocks[i].OnRootRemoveRequest();
            }
        }

        protected internal override void OnRootStopRequest()
        {
            base.OnRootStopRequest();
            for (int i = 0; i < _clocks.Length; i++)
            {
                _clocks[i].OnRootStopRequest();
            }
        }
    }
}
