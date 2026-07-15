using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Threading
{
    public abstract class Dispatcher
    {

        #region Constructors

        protected Dispatcher()
        {
            if (CanFromThread)
                lock (_GlobalLock)
                {
                    _Dispatchers.Add(new WeakReference(this));
                }
        }

        #endregion

        #region Properties

        public abstract Thread Thread { get; }

        public static Dispatcher CurrentDispatcher => FromThread(Thread.CurrentThread);

        protected virtual bool CanFromThread => true;

        #endregion

        #region Methods

        public abstract bool CheckAccess();

        public abstract void VerifyAccess();

        private static PriorityRange _ForegroundPriorityRange = new PriorityRange(DispatcherPriority.Loaded, true, DispatcherPriority.Send, true);
        private static PriorityRange _BackgroundPriorityRange = new PriorityRange(DispatcherPriority.Background, true, DispatcherPriority.Input, true);
        private static PriorityRange _IdlePriorityRange = new PriorityRange(DispatcherPriority.SystemIdle, true, DispatcherPriority.ContextIdle, true);
        /// <summary>
        ///     Validates that a priority is suitable for use by the dispatcher.
        /// </summary>
        /// <param name="priority">
        ///     The priority to validate.
        /// </param>
        /// <param name="parameterName">
        ///     The name if the argument to report in the ArgumentException
        ///     that is raised if the priority is not suitable for use by
        ///     the dispatcher.
        /// </param>
        public static void ValidatePriority(DispatcherPriority priority, string parameterName) // NOTE: should be Priority
        {
            // First make sure the Priority is valid.
            // Priority.ValidatePriority(priority, paramName);

            // Second, make sure the priority is in a range recognized by
            // the dispatcher.
            if (!_ForegroundPriorityRange.Contains(priority) &&
               !_BackgroundPriorityRange.Contains(priority) &&
               !_IdlePriorityRange.Contains(priority) &&
               DispatcherPriority.Inactive != priority)  // NOTE: should be Priority.Min
            {
                // If we move to a Priority class, this exception will have to change too.
                throw new System.ComponentModel.InvalidEnumArgumentException(parameterName, (int)priority, typeof(DispatcherPriority));
            }
        }

        #endregion

        #region Invoke

        [Obsolete("Use InvokeAsync instead of BeginInvoke.")]
        public DispatcherOperation<object?> BeginInvoke(Delegate method, params object[] args) => BeginInvoke(method, DispatcherPriority.Normal, args);

        [Obsolete("Use InvokeAsync instead of BeginInvoke.")]
        public abstract DispatcherOperation<object?> BeginInvoke(Delegate method, DispatcherPriority priority, params object[] args);

        public void Invoke(Action callback) => Invoke(callback, DispatcherPriority.Send, CancellationToken.None, TimeSpan.FromMilliseconds(-1));

        public void Invoke(Action callback, DispatcherPriority priority) => Invoke(callback, priority, CancellationToken.None, TimeSpan.FromMilliseconds(-1));

        public void Invoke(Action callback, DispatcherPriority priority, CancellationToken cancellationToken) => Invoke(callback, priority, cancellationToken, TimeSpan.FromMilliseconds(-1));

        public abstract void Invoke(Action callback, DispatcherPriority priority, CancellationToken cancellationToken, TimeSpan timeout);

        public TResult Invoke<TResult>(Func<TResult> callback) => Invoke(callback, DispatcherPriority.Send, CancellationToken.None, TimeSpan.FromMilliseconds(-1));

        public TResult Invoke<TResult>(Func<TResult> callback, DispatcherPriority priority) => Invoke(callback, priority, CancellationToken.None, TimeSpan.FromMilliseconds(-1));

        public TResult Invoke<TResult>(Func<TResult> callback, DispatcherPriority priority, CancellationToken cancellationToken) => Invoke(callback, priority, cancellationToken, TimeSpan.FromMilliseconds(-1));

        public abstract TResult Invoke<TResult>(Func<TResult> callback, DispatcherPriority priority, CancellationToken cancellationToken, TimeSpan timeout);

        public DispatcherOperation InvokeAsync(Action callback) => InvokeAsync(callback, DispatcherPriority.Normal, CancellationToken.None);

        public DispatcherOperation InvokeAsync(Action callback, DispatcherPriority priority) => InvokeAsync(callback, priority, CancellationToken.None);

        public abstract DispatcherOperation InvokeAsync(Action callback, DispatcherPriority priority, CancellationToken cancellationToken);

        public DispatcherOperation<TResult> InvokeAsync<TResult>(Func<TResult> callback) => InvokeAsync(callback, DispatcherPriority.Normal, CancellationToken.None);

        public DispatcherOperation<TResult> InvokeAsync<TResult>(Func<TResult> callback, DispatcherPriority priority) => InvokeAsync(callback, priority, CancellationToken.None);

        public abstract DispatcherOperation<TResult> InvokeAsync<TResult>(Func<TResult> callback, DispatcherPriority priority, CancellationToken cancellationToken);

        [Obsolete("Use Invoke<TResult> instead of Invoke.")]
        public object? Invoke(Delegate method, params object[] args) => Invoke(method, TimeSpan.FromMilliseconds(-1), DispatcherPriority.Normal, args);

        [Obsolete("Use Invoke<TResult> instead of Invoke.")]
        public object? Invoke(Delegate method, DispatcherPriority priority, params object[] args) => Invoke(method, TimeSpan.FromMilliseconds(-1), priority, args);

        [Obsolete("Use Invoke<TResult> instead of Invoke.")]
        public object? Invoke(Delegate method, TimeSpan timeout, params object[] args) => Invoke(method, timeout, DispatcherPriority.Normal, args);

        [Obsolete("Use Invoke<TResult> instead of Invoke.")]
        public abstract object? Invoke(Delegate method, TimeSpan timeout, DispatcherPriority priority, params object[] args);

        #endregion

        #region Dispatchers

        private static readonly object _GlobalLock = new object();
        private static readonly List<WeakReference> _Dispatchers = new List<WeakReference>();

        public static Dispatcher FromThread(Thread thread)
        {
            lock (_GlobalLock)
            {
                for (int i = 0; i < _Dispatchers.Count; i++)
                {
                    Dispatcher? d = _Dispatchers[i].Target as Dispatcher;
                    if (d != null)
                    {
                        // Note: we compare the thread objects themselves to protect
                        // against threads reusing old thread IDs.
                        Thread dispatcherThread = d.Thread;
                        if (dispatcherThread == thread)
                            return d;
                    }
                    else
                    {
                        // We found a dead reference, so remove it from
                        // the list, and adjust the index so we account
                        // for it.
                        _Dispatchers.RemoveAt(i);
                        i--;
                    }
                }
            }
            return EmptyDispatcher.Default;
        }

        #endregion

        #region Timer

        private readonly List<DispatcherTimer> _timers = new List<DispatcherTimer>();
        private readonly object _timerLock = new object();
        private int _minTimerTick = 0;

        internal void AddTimer(DispatcherTimer timer)
        {
            lock (_timerLock)
            {
                _timers.Add(timer);
                UpdateTimerCore(timer);
            }
        }

        internal void RemoveTimer(DispatcherTimer timer)
        {
            lock (_timerLock)
            {
                _timers.Remove(timer);
                if (_timers.Count == 0)
                {
                    if (CheckAccess())
                    {
                        RemoveTimerTick();
                    }
                    else
                    {
                        Invoke(RemoveTimerTick);
                    }
                }
                else if (timer._dueTimeInTicks == _minTimerTick)
                {
                    for (int i = 0; i < _timers.Count; i++)
                    {
                        if (UpdateTimerCore(_timers[i]))
                            break;
                    }
                }
            }
        }

        internal void UpdateTimer(DispatcherTimer timer)
        {
            lock (_timerLock)
            {
                UpdateTimerCore(timer);
            }
        }

        private bool UpdateTimerCore(DispatcherTimer timer)
        {
            if (_minTimerTick == 0 || timer._dueTimeInTicks < _minTimerTick)
            {
                _minTimerTick = timer._dueTimeInTicks;
                if (CheckAccess())
                {
                    PreUpdateTimer(timer._dueTimeInTicks);
                }
                else
                {
                    Invoke(() => PreUpdateTimer(timer._dueTimeInTicks));
                }
                return true;
            }
            return false;
        }

        private void PreUpdateTimer(int targetTick)
        {
            if (Environment.TickCount < targetTick)
            {
                SetTimerTick(targetTick);
            }
        }

        protected abstract void SetTimerTick(int targetTick);

        protected abstract void RemoveTimerTick();

        protected void ApplyTimerTick(int currentTick)
        {
            lock (_timerLock)
            {
                _minTimerTick = 0;
                int minDueTime = int.MaxValue;
                var timerMaxIndex = _timers.Count - 1;
                for (int i = timerMaxIndex; i >= 0; i--)
                {
                    var timer = _timers[i];
                    if (timer._dueTimeInTicks <= currentTick)
                    {
                        if (i == timerMaxIndex)
                        {
                            _timers.RemoveAt(i);
                        }
                        else
                        {
                            _timers[i] = _timers[timerMaxIndex];
                            _timers.RemoveAt(timerMaxIndex);
                        }
                        timerMaxIndex--;
                        timer.Promote();
                    }
                    else if (timer._dueTimeInTicks < minDueTime)
                        minDueTime = timer._dueTimeInTicks;
                }
                if (minDueTime == int.MaxValue)
                    RemoveTimerTick();
                else
                    SetTimerTick(minDueTime);
            }
        }

        #endregion
    }
}
