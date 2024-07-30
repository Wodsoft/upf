using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    internal class AnimationStorage
    {
        private static readonly Dictionary<DependencyObject, Dictionary<int, AnimationStorage>> _Storages = new Dictionary<DependencyObject, Dictionary<int, AnimationStorage>>();
        private readonly List<AnimationClock> _clocks = new List<AnimationClock>();
        private readonly DependencyObject _d;
        private readonly DependencyProperty _dp;

        public AnimationStorage(DependencyObject d, DependencyProperty dp)
        {
            _d = d;
            _dp = dp;
        }

        #region Methods

        private void AttachClock(AnimationClock clock, HandoffBehavior handoffBehavior)
        {
            if (handoffBehavior == HandoffBehavior.SnapshotAndReplace)
            {
                ClearClocks();
            }
            clock.CurrentTimeInvalidated += Clock_CurrentTimeInvalidated;
            clock.RemoveRequested += Clock_RemoveRequested;
            _clocks.Add(clock);
        }

        private void ClearClocks()
        {
            foreach (var clock in _clocks)
            {
                DetachClock(clock);
            }
            _clocks.Clear();
        }

        private void DetachClock(AnimationClock clock)
        {
            clock.CurrentTimeInvalidated -= Clock_CurrentTimeInvalidated;
            clock.RemoveRequested -= Clock_RemoveRequested;
        }

        private void Clock_RemoveRequested(object? sender, EventArgs e)
        {
            var clock = (AnimationClock)sender!;
            DetachClock(clock);
            _clocks.Remove(clock);
            Check();
        }

        private void Clock_CurrentTimeInvalidated(object? sender, EventArgs e)
        {
            _d.InvalidateProperty(_dp);
        }

        private void Check()
        {
            if (_clocks.Count == 0)
            {
                _d.InvalidateProperty(_dp);
                var storages = _Storages[_d];
                if (storages.Count == 1)
                    _Storages.Remove(_d);
                storages.Remove(_dp.GlobalIndex);
            }
        }

        public bool TryGetValue(ref object? baseValue)
        {
            if (baseValue == DependencyProperty.UnsetValue)
                baseValue = _d.GetMetadata(_dp).GetDefaultValue(_d, _dp);
            object? defaultDestinationValue = baseValue;
            object? currentLayerValue = baseValue;
            bool hasClock = false;
            for (int i = 0; i < _clocks.Count; i++)
            {
                var clock = _clocks[i];
                if (clock.CurrentState != ClockState.Stopped)
                {
                    currentLayerValue = clock.GetCurrentValue(currentLayerValue, defaultDestinationValue);
                    hasClock = true;
                }
            }
            if (hasClock)
                baseValue = currentLayerValue;
            return hasClock;
        }

        #endregion

        #region Static Methods

        public static void ApplyClock(DependencyObject d, DependencyProperty dp, AnimationClock clock, HandoffBehavior handoffBehavior)
        {
            var storage = GetStorage(d, dp) ?? CreateStorage(d, dp);
            storage.AttachClock(clock, handoffBehavior);
        }

        public static bool HasAnimation(DependencyObject d)
        {
            return _Storages.ContainsKey(d);
        }

        public static AnimationStorage? GetStorage(DependencyObject d, DependencyProperty dp)
        {
            if (_Storages.TryGetValue(d, out var ds))
            {
                if (ds.TryGetValue(dp.GlobalIndex, out var ps))
                    return ps;
            }
            return null;
        }


        public static bool TryGetStorage(DependencyObject d, DependencyProperty dp, [NotNullWhen(true)] out AnimationStorage? storage)
        {
            if (_Storages.TryGetValue(d, out var ds))
            {
                if (ds.TryGetValue(dp.GlobalIndex, out storage))
                    return true;
            }
            storage = null;
            return false;
        }

        private static AnimationStorage CreateStorage(DependencyObject d, DependencyProperty dp)
        {
            if (!_Storages.TryGetValue(d, out var ds))
            {
                ds = new Dictionary<int, AnimationStorage>();
                _Storages.Add(d, ds);
            }
            if (!ds.TryGetValue(dp.GlobalIndex, out var ps))
            {
                ps = new AnimationStorage(d, dp);
                ds.Add(dp.GlobalIndex, ps);
            }
            return ps;
        }

        #endregion
    }
}
