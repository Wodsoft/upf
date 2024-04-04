using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media.Animation;
using Wodsoft.UI.Threading;

namespace Wodsoft.UI.Providers
{
    public class FrameworkClockProvider : ClockProvider
    {
        private readonly List<ClockItem> _clocks = new List<ClockItem>();
        private readonly List<ClockItem> _addClocks = new List<ClockItem>();
        private readonly List<Clock> _needToRemove = new List<Clock>();

        protected override void OnRegisterClock(DependencyObject d, Clock clock)
        {
            lock (_addClocks)
            {
                _addClocks.Add(new ClockItem
                {
                    Clock = clock,
                    Target = d
                });
            }
        }

        protected override void OnUnregisterClock(Clock clock)
        {
            lock (_needToRemove)
            {
                _needToRemove.Add(clock);
            }
        }

        public override void ApplyTick(TimeSpan tick)
        {
            lock (_addClocks)
            {
                if (_addClocks.Count!=0)
                {
                    _clocks.AddRange(_addClocks);
                    _addClocks.Clear();
                }
            }
            Clock[] needToRemove;
            lock(_needToRemove)
            {
                if (_needToRemove.Count!=0)
                {
                    needToRemove = _needToRemove.ToArray();
                    _needToRemove.Clear();
                }
                else
                    needToRemove = Array.Empty<Clock>();
            }
            List<int> needToRemoveIndex = new List<int>();
            var clocks = CollectionsMarshal.AsSpan(_clocks);
            var count = clocks.Length;
            bool hasNeedToRemove = needToRemove.Length != 0;
            for (int i = 0; i < count; i++)
            {
                ref var clock = ref clocks[i];
                if (hasNeedToRemove && needToRemove.Contains(clock.Clock))
                    needToRemoveIndex.Add(i);
                else
                {
                    if (clock.Target.Dispatcher is not FrameworkDispatcher frameworkDispatcher || frameworkDispatcher.IsActived)
                        OnApplyTick(tick, clock.Clock);
                }
            }
            if (hasNeedToRemove)
            {
                for (int i = needToRemoveIndex.Count - 1; i >= 0; i--)
                    _clocks.RemoveAt(needToRemoveIndex[i]);
            }
        }

        private struct ClockItem
        {
            public Clock Clock;
            public DependencyObject Target;
        }
    }
}
