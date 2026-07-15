using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Threading
{
    internal sealed class EmptyDispatcher : Dispatcher
    {
        public EmptyDispatcher()
        {
            _timer = new Timer(TimerCallback, null, -1, Timeout.Infinite);
        }

        public override Thread Thread => Thread.CurrentThread;

        protected override bool CanFromThread => false;

        public override bool CheckAccess()
        {
            return true;
        }

        public override void VerifyAccess()
        {

        }

        [Obsolete("Use InvokeAsync instead of BeginInvoke.")]
        public override DispatcherOperation<object?> BeginInvoke(Delegate method, DispatcherPriority priority, params object[] args)
        {
            return new EmptyDispatcherOperation<object?>(priority, new Task<object?>(() => method.DynamicInvoke(args)));
        }

        public override void Invoke(Action callback, DispatcherPriority priority, CancellationToken cancellationToken, TimeSpan timeout)
        {
            callback();
        }

        public override TResult Invoke<TResult>(Func<TResult> callback, DispatcherPriority priority, CancellationToken cancellationToken, TimeSpan timeout)
        {
            return callback();
        }

        public override DispatcherOperation InvokeAsync(Action callback, DispatcherPriority priority, CancellationToken cancellationToken)
        {
            return new EmptyDispatcherOperation(priority, new Task(callback));
        }

        public override DispatcherOperation<TResult> InvokeAsync<TResult>(Func<TResult> callback, DispatcherPriority priority, CancellationToken cancellationToken)
        {
            return new EmptyDispatcherOperation<TResult>(priority, new Task<TResult>(callback));
        }

        [Obsolete("Use Invoke<TResult> instead of Invoke.")]
        public override object? Invoke(Delegate method, TimeSpan timeout, DispatcherPriority priority, params object[] args)
        {
            return method.DynamicInvoke(args);
        }

        public static readonly EmptyDispatcher Default = new EmptyDispatcher();

        #region Timer

        private readonly Timer _timer;
        private int _lastTimerTick;

        protected override void SetTimerTick(int targetTick)
        {
            var dueTime = targetTick - Environment.TickCount;
            if (dueTime >= 0)
                _timer.Change(dueTime, Timeout.Infinite);
            else
                _timer.Change(0, Timeout.Infinite);
        }

        protected override void RemoveTimerTick()
        {
            _timer.Change(-1, Timeout.Infinite);
        }

        private void TimerCallback(object? state)
        {
            ApplyTimerTick(Environment.TickCount);
        }

        #endregion
    }
}
