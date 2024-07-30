using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static System.TimeZoneInfo;

namespace Wodsoft.UI.Media.Animation
{
    public class Clock
    {
        private readonly Timeline _timeline;
        private readonly bool _autoReverse;
        private readonly RepeatBehavior _repeatBehavior;
        private readonly TimeSpan _beginTime;
        private readonly FillBehavior _fillBehavior;
        private readonly double _acce, _dece, _transitionTime;
        private double _speedRatio;
        private Duration _duration, _totalDuration;
        private double _progress;
        private int _iteration;
        private bool _isReverse, _hasControllableRoot, _isStopped, _isPaused, _isExpired;
        private TimeSpan _currentTime, _totalTime;
        private ClockState _state;
        private ClockController? _controller;

        protected internal Clock(Timeline timeline)
        {
            _timeline = (Timeline)timeline.GetCurrentValueAsFrozen();
            _autoReverse = _timeline.AutoReverse;
            _repeatBehavior = _timeline.RepeatBehavior;
            _beginTime = _timeline.BeginTime ?? TimeSpan.Zero;
            _speedRatio = _timeline.SpeedRatio;
            _fillBehavior = _timeline.FillBehavior;
            _acce = _timeline.AccelerationRatio;
            _dece = _timeline.DecelerationRatio;
            _transitionTime = _acce + _dece;
        }

        #region Properties

        public Timeline Timeline => _timeline;

        private bool _isRoot;
        public bool IsRoot { get => _isRoot; internal set => _isRoot = value; }

        public bool AutoReverse => _autoReverse;

        public RepeatBehavior RepeatBehavior => _repeatBehavior;

        public TimeSpan BeginTime => _beginTime;

        public Duration Duration => _duration;

        public Duration TotalDuration => _totalDuration;

        public bool IsExpired => _isExpired;

        public double SpeedRatio { get => _speedRatio; internal set => _speedRatio = value; }

        public bool IsReverse => _isReverse;

        public int CurrentIteration => _iteration;

        public TimeSpan CurrentTime => _currentTime;

        public TimeSpan TotalTime => _totalTime;

        public double CurrentProgress => _progress;

        public bool HasControllableRoot { get => _hasControllableRoot; internal set => _hasControllableRoot = value; }

        public ClockController? Controller
        {
            get
            {
                if (_isRoot && _hasControllableRoot)
                {
                    if (_controller == null)
                        _controller = new ClockController(this);
                    return _controller;
                }
                return null;
            }
        }

        public ClockState CurrentState => _state;

        #endregion

        #region Methods

        public virtual void Measure()
        {
            var duration = MeasureDuration();
            if (duration == Duration.Automatic)
                throw new InvalidDataException("Measure duration result can't be automatic.");
            _duration = duration;
            duration = MeasureTotalDuration();
            if (duration == Duration.Automatic)
                throw new InvalidDataException("Measure total duration result can't be automatic.");
            _totalDuration = duration;
        }

        protected virtual Duration MeasureDuration()
        {
            var duration = _timeline.Duration;
            if (duration.HasTimeSpan)
                return duration;
            return new Duration(TimeSpan.Zero);
        }

        protected virtual Duration MeasureTotalDuration()
        {
            if (_duration == Duration.Forever)
                return _duration;
            if (_repeatBehavior == RepeatBehavior.Forever)
                return Duration.Forever;
            var time = _duration.TimeSpan;
            if (_autoReverse)
                time *= 2;
            if (_repeatBehavior.HasCount)
                time *= _repeatBehavior.Count;
            else
                time = _repeatBehavior.Duration;
            time += _beginTime;
            return new Duration(time);
        }

        protected internal void ApplyTick(TimeSpan tick)
        {
            if (_isPaused || _isStopped)
                return;
            var totalTime = _totalTime;
            tick *= _speedRatio;
            if (_totalTime + tick < _beginTime)
            {
                _totalTime += tick;
                return;
            }
            else if (_totalTime < _beginTime)
            {
                tick -= _beginTime - _totalTime;
                _totalTime = _beginTime;
            }
            if (!_isExpired)
            {
                _state = ComputeState(ref tick, ref _totalTime, ref _currentTime, ref _iteration, ref _isReverse);
                ComputeProgress();
            }
            if (totalTime != _totalTime)
                _timeline.RaiseCurrentTimeInvalidated(this);
            if (_state == ClockState.Stopped)
            {
                _timeline.RaiseCompleted(this);
                if (_isRoot)
                    OnRootRemoveRequest();
            }
        }

        protected internal virtual void OnRootRemoveRequest()
        {
            _timeline.RaiseRemoveRequested(this);
        }

        protected internal virtual void OnRootStopRequest()
        {
            _state = ClockState.Stopped;
        }

        protected internal void ApplyTimeSpan(TimeSpan time, bool isParentExpired)
        {
            if (time < TimeSpan.Zero)
                throw new InvalidOperationException();
            var totalTime = _totalTime;
            time *= _speedRatio;
            _totalTime = time;
            _isExpired = false;
            ComputeState();
            if (isParentExpired)
            {
                _state = _fillBehavior == FillBehavior.HoldEnd ? ClockState.Filling : ClockState.Stopped;
                _isExpired = true;
            }
            if (totalTime != _totalTime)
                _timeline.RaiseCurrentTimeInvalidated(this);
            if (_state == ClockState.Stopped)
            {
                _timeline.RaiseCompleted(this);
                if (_isRoot)
                    OnRootRemoveRequest();
            }
        }

        protected internal void SeekTimeSpan(TimeSpan time)
        {
            if (time < TimeSpan.Zero)
                throw new InvalidOperationException();
            _totalTime = time;
            ComputeState();
        }

        protected void ComputeState()
        {
            _state = ComputeState(_totalTime, out _currentTime, out _iteration, out _isReverse);
            ComputeProgress();
        }

        protected virtual ClockState ComputeState(ref TimeSpan tick, ref TimeSpan totalTime, ref TimeSpan currentTime, ref int iteration, ref bool isReverse)
        {
            _totalTime += tick;
            if (_totalTime < _beginTime)
                return ClockState.Stopped;
            if (_duration == Duration.Forever)
                return ClockState.Active;
            bool isCompleted = false;
            currentTime += tick;
            if (_totalDuration != Duration.Forever)
            {
                if (_totalTime > _totalDuration)
                {
                    var offset = _totalTime - _totalDuration.TimeSpan;
                    _isExpired = true;
                    tick -= offset;
                    currentTime -= offset;
                    _totalTime = _totalDuration.TimeSpan;
                    isCompleted = true;
                }
            }
            while (currentTime > _duration.TimeSpan)
            {
                currentTime -= _duration.TimeSpan;
                if (_autoReverse)
                {
                    isReverse = !isReverse;
                    if (!isReverse)
                        iteration++;
                }
                else
                {
                    iteration++;
                }
            }
            return isCompleted ? (_fillBehavior == FillBehavior.Stop ? ClockState.Stopped : ClockState.Filling) : ClockState.Active;
        }

        protected void ComputeProgress()
        {
            if (_duration == Duration.Forever)
                _progress = 0d;
            else if (_currentTime == _duration.TimeSpan)
                _progress = 1d;
            else
            {
                _progress = _currentTime / _duration.TimeSpan;
                if (_transitionTime != 0)
                {
                    double maxRate = 2 / (2 - _transitionTime);

                    if (_progress < _acce)
                    {
                        // Acceleration phase
                        _progress = maxRate * _progress * _progress / (2 * _acce);
                    }
                    else if (_progress <= (1 - _dece))
                    {
                        // Run-rate phase
                        _progress = maxRate * (_progress - _acce / 2);
                    }
                    else
                    {
                        // Deceleration phase
                        double tc = 1 - _progress;  // t's complement from 1
                        _progress = 1 - maxRate * tc * tc / (2 * _dece);
                    }
                }
            }
        }

        protected virtual ClockState ComputeState(TimeSpan totalTime, out TimeSpan currentTime, out int iteration, out bool isReverse)
        {
            isReverse = false;
            totalTime -= _beginTime;
            if (totalTime < TimeSpan.Zero)
            {
                currentTime = TimeSpan.Zero;
                iteration = 0;
                return ClockState.Stopped;
            }
            if (_duration == Duration.Forever)
            {
                currentTime = TimeSpan.Zero;
                iteration = 0;
                return ClockState.Active;
            }
            if (_repeatBehavior == RepeatBehavior.Forever)
            {
                var ticks = _duration.TimeSpan.Ticks;
                if (_autoReverse)
                    ticks *= 2;
                ticks = totalTime.Ticks % ticks;
                totalTime = new TimeSpan(ticks);
                if (_autoReverse && totalTime > _duration.TimeSpan)
                {
                    isReverse = true;
                    totalTime -= _duration.TimeSpan;
                }
                else
                    isReverse = false;
                currentTime = totalTime;
                iteration = 1;
                return ClockState.Active;
            }
            iteration = 1;
            bool isCompleted;
            if (totalTime > _totalDuration)
            {
                _isExpired = true;
                totalTime = _totalDuration.TimeSpan;
                isCompleted = true;
            }
            else
            {
                isCompleted = false;
            }
            while (totalTime > _duration.TimeSpan)
            {
                totalTime -= _duration.TimeSpan;
                if (_autoReverse)
                {
                    isReverse = !isReverse;
                    if (!isReverse)
                        iteration++;
                }
                else
                {
                    iteration++;
                }
            }
            currentTime = totalTime;
            return isCompleted ? (_fillBehavior == FillBehavior.Stop ? ClockState.Stopped : ClockState.Filling) : ClockState.Active;
        }

        internal void Begin()
        {
            _isStopped = false;
            _isPaused = false;
            _isExpired = false;
            SeekTimeSpan(TimeSpan.Zero);
        }

        internal void SkipToFill()
        {
            ApplyTimeSpan(_totalDuration.TimeSpan, false);
        }

        internal void Pause()
        {
            _isPaused = true;
        }

        internal void Resume()
        {
            _isPaused = false;
        }

        internal void Stop()
        {
            _isStopped = true;
            _isPaused = false;
            _isExpired = false;
            _currentTime = _totalTime = TimeSpan.Zero;
            _progress = 0d;
            OnRootStopRequest();
        }

        internal void Remove()
        {
            OnRootRemoveRequest();
        }

        #endregion

        #region Events

        public event EventHandler Completed { add => _timeline.Completed += value;remove => _timeline.Completed -= value; }

        public event EventHandler CurrentTimeInvalidated { add => _timeline.CurrentTimeInvalidated += value; remove => _timeline.CurrentTimeInvalidated -= value; }

        public event EventHandler CurrentStateInvalidated { add => _timeline.CurrentStateInvalidated += value; remove => _timeline.CurrentStateInvalidated -= value; }

        public event EventHandler RemoveRequested { add => _timeline.RemoveRequested += value; remove => _timeline.RemoveRequested -= value; }

        #endregion
    }
}
