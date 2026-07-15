using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Threading
{
    public class DispatcherTimer
    {

        /// <summary>
        ///     Creates a timer that uses the current thread's Dispatcher to
        ///     process the timer event at background priority.
        /// </summary>
        public DispatcherTimer() : this(DispatcherPriority.Background)  // NOTE: should be Priority Dispatcher.BackgroundPriority
        {
        }

        /// <summary>
        ///     Creates a timer that uses the current thread's Dispatcher to
        ///     process the timer event at the specified priority.
        /// </summary>
        /// <param name="priority">
        ///     The priority to process the timer at.
        /// </param>
        public DispatcherTimer(DispatcherPriority priority) // NOTE: should be Priority
        {
            Initialize(Dispatcher.CurrentDispatcher, priority, TimeSpan.FromMilliseconds(0));
        }

        /// <summary>
        ///     Creates a timer that uses the specified Dispatcher to
        ///     process the timer event at the specified priority.
        /// </summary>
        /// <param name="priority">
        ///     The priority to process the timer at.
        /// </param>
        /// <param name="dispatcher">
        ///     The dispatcher to use to process the timer.
        /// </param>
        public DispatcherTimer(DispatcherPriority priority, Dispatcher dispatcher)  // NOTE: should be Priority
        {
            if (dispatcher == null)
            {
                throw new ArgumentNullException("dispatcher");
            }

            Initialize(dispatcher, priority, TimeSpan.FromMilliseconds(0));
        }

        /// <summary>
        ///     Creates a timer that is bound to the specified dispatcher and
        ///     will be processed at the specified priority, after the
        ///     specified timeout.
        /// </summary>
        /// <param name="interval">
        ///     The interval to tick the timer after.
        /// </param>
        /// <param name="priority">
        ///     The priority to process the timer at.
        /// </param>
        /// <param name="callback">
        ///     The callback to call when the timer ticks.
        /// </param>
        /// <param name="dispatcher">
        ///     The dispatcher to use to process the timer.
        /// </param>
        public DispatcherTimer(TimeSpan interval, DispatcherPriority priority, EventHandler callback, Dispatcher dispatcher) // NOTE: should be Priority
        {
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }
            if (dispatcher == null)
            {
                throw new ArgumentNullException("dispatcher");
            }

            if (interval.TotalMilliseconds < 0)
                throw new ArgumentOutOfRangeException("interval", "Interval can't less than zero.");

            if (interval.TotalMilliseconds > int.MaxValue)
                throw new ArgumentOutOfRangeException("interval", "Interval too large.");

            Initialize(dispatcher, priority, interval);

            Tick += callback;
            Start();
        }

        /// <summary>
        ///     Gets the dispatcher this timer is associated with.
        /// </summary>
        public Dispatcher? Dispatcher
        {
            get
            {
                return _dispatcher;
            }
        }

        /// <summary>
        ///     Gets or sets whether the timer is running.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }

            set
            {
                lock (_instanceLock)
                {
                    if (!value && _isEnabled)
                    {
                        Stop();
                    }
                    else if (value && !_isEnabled)
                    {
                        Start();
                    }
                }
            }
        }

        /// <summary>
        ///     Gets or sets the time between timer ticks.
        /// </summary>
        public TimeSpan Interval
        {
            get
            {
                return _interval;
            }

            set
            {
                bool updateWin32Timer = false;

                if (value.TotalMilliseconds < 0)
                    throw new ArgumentOutOfRangeException("value", "Interval can't less than zero.");

                if (value.TotalMilliseconds > int.MaxValue)
                    throw new ArgumentOutOfRangeException("value", "Interval too large.");

                lock (_instanceLock)
                {
                    _interval = value;

                    if (_isEnabled)
                    {
                        _dueTimeInTicks = Environment.TickCount + (int)_interval.TotalMilliseconds;
                        updateWin32Timer = true;
                    }
                }

                if (updateWin32Timer)
                {
                    _dispatcher.UpdateTimer(this);
                }
            }
        }

        /// <summary>
        ///     Starts the timer.
        /// </summary>
        public void Start()
        {
            if (_interval.TotalMilliseconds == 0)
                return;
            lock (_instanceLock)
            {
                if (!_isEnabled)
                {
                    _isEnabled = true;

                    Restart();
                }
            }
        }

        /// <summary>
        ///     Stops the timer.
        /// </summary>
        public void Stop()
        {
            bool updateWin32Timer = false;

            lock (_instanceLock)
            {
                if (_isEnabled)
                {
                    _isEnabled = false;
                    updateWin32Timer = true;

                    // If the operation is in the queue, abort it.
                    if (_operation != null)
                    {
                        _operation.Abort();
                        _operation = null;
                    }
                }
            }

            if (updateWin32Timer)
            {
                _dispatcher.RemoveTimer(this);
            }
        }

        /// <summary>
        ///     Occurs when the specified timer interval has elapsed and the
        ///     timer is enabled.
        /// </summary>
        public event EventHandler? Tick;

        /// <summary>
        ///     Any data that the caller wants to pass along with the timer.
        /// </summary>
        public object? Tag { get; set; }

        [MemberNotNull(nameof(_dispatcher))]
        private void Initialize(Dispatcher dispatcher, DispatcherPriority priority, TimeSpan interval)
        {
            // Note: all callers of this have a "priority" parameter.
            Dispatcher.ValidatePriority(priority, "priority");
            if (priority == DispatcherPriority.Inactive)
            {
                throw new ArgumentException("Invalid priority.", "priority");
            }

            _dispatcher = dispatcher;
            _priority = priority;
            _interval = interval;
        }

        private void Restart()
        {
            lock (_instanceLock)
            {
                if (_operation != null)
                {
                    // Timer has already been restarted, e.g. Start was called form the Tick handler.
                    return;
                }

                // BeginInvoke a new operation.
                _operation = _dispatcher.InvokeAsync(FireTick, DispatcherPriority.Inactive);

                _dueTimeInTicks = Environment.TickCount + (int)_interval.TotalMilliseconds;

                if (_interval.TotalMilliseconds == 0 && _dispatcher.CheckAccess())
                {
                    // shortcut - just promote the item now
                    Promote();
                }
                else
                {
                    _dispatcher.AddTimer(this);
                }
            }
        }

        internal void Promote() // called from Dispatcher
        {
            lock (_instanceLock)
            {
                // Simply promote the operation to it's desired priority.
                if (_operation != null)
                {
                    _operation.Priority = _priority;
                }
            }
        }

        private void FireTick()
        {
            // The operation has been invoked, so forget about it.
            _operation = null;

            // The dispatcher thread is calling us because item's priority
            // was changed from inactive to something else.
            if (Tick != null)
            {
                Tick(this, EventArgs.Empty);
            }

            // If we are still enabled, start the timer again.
            if (_isEnabled)
            {
                Restart();
            }
        }

        // This is the object we use to synchronize access.
        private readonly object _instanceLock = new object();

        // Note: We cannot BE a dispatcher-affinity object because we can be
        // created by a worker thread.  We are still associated with a
        // dispatcher (where we post the item) but we can be accessed
        // by any thread.
        private Dispatcher _dispatcher;

        private DispatcherPriority _priority;  // NOTE: should be Priority
        private TimeSpan _interval;
        private DispatcherOperation? _operation;
        private bool _isEnabled;

        internal int _dueTimeInTicks; // used by Dispatcher
    }
}
