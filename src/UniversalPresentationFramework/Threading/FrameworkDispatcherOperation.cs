using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Threading
{
    internal interface IFrameworkDispatcherOperation
    {
        void Invoke();
    }

    public class FrameworkDispatcherOperation : DispatcherOperation, IFrameworkDispatcherOperation
    {
        private readonly FrameworkDispatcher _dispatcher;
        private readonly Action _callback;
        private readonly TaskCompletionSource _abortTask, _callbackTask;
        private DispatcherOperationStatus _status;

        public FrameworkDispatcherOperation(FrameworkDispatcher dispatcher, DispatcherPriority priority, Action callback) : base(dispatcher, priority)
        {
            _dispatcher = dispatcher;
            _callback = callback;
            _callbackTask = new TaskCompletionSource();
            _abortTask = new TaskCompletionSource();
            _status = DispatcherOperationStatus.Pending;
        }

        public override Task Task => _callbackTask.Task;

        public override DispatcherOperationStatus Status => _status;

        public override event EventHandler? Aborted;
        public override event EventHandler? Completed;

        public override bool Abort()
        {
            if (_status != DispatcherOperationStatus.Aborted && _status != DispatcherOperationStatus.Completed && _dispatcher.Abort(this))
            {
                _status = DispatcherOperationStatus.Aborted;
                _abortTask.SetResult();
                Aborted?.Invoke(this, EventArgs.Empty);
                return true;
            }
            return false;
        }

        public override DispatcherOperationStatus Wait(TimeSpan timeout)
        {
            if (_status == DispatcherOperationStatus.Pending || _status == DispatcherOperationStatus.Executing)
            {
                ManualResetEvent resetEvent = new ManualResetEvent(false);
                Task.WhenAny(_callbackTask.Task, Task.Delay(timeout), _abortTask.Task).ContinueWith(_ => resetEvent.Set());
                resetEvent.WaitOne();
            }
            return Status;
        }

        public void Invoke()
        {
            _status = DispatcherOperationStatus.Executing;
            try
            {
                _callback();
            }
            finally
            {
                _status = DispatcherOperationStatus.Completed;
                _callbackTask.SetResult();
            }
        }

        protected override bool SetPriority(DispatcherPriority priority)
        {
            return _dispatcher.SetPriority(this, priority);
        }
    }

    public class FrameworkDispatcherOperation<TResult> : DispatcherOperation<TResult>, IFrameworkDispatcherOperation
    {
        private readonly FrameworkDispatcher _dispatcher;
        private readonly Func<TResult> _callback;
        private readonly TaskCompletionSource _abortTask;
        private readonly TaskCompletionSource<TResult> _callbackTask;
        private DispatcherOperationStatus _status;
        private TResult _result;

        public FrameworkDispatcherOperation(FrameworkDispatcher dispatcher, DispatcherPriority priority, Func<TResult> callback) : base(dispatcher, priority)
        {
            _dispatcher = dispatcher;
            _callback = callback;
            _callbackTask = new TaskCompletionSource<TResult>();
            _abortTask = new TaskCompletionSource();
            _result = default!;
        }

        public override Task Task => _callbackTask.Task;

        public override DispatcherOperationStatus Status => _status;

        public override TResult Result => _result!;

        public override event EventHandler? Aborted;
        public override event EventHandler? Completed;

        public override bool Abort()
        {
            if (_status != DispatcherOperationStatus.Aborted && _status != DispatcherOperationStatus.Completed && _dispatcher.Abort(this))
            {
                _status = DispatcherOperationStatus.Aborted;
                _abortTask.SetResult();
                Aborted?.Invoke(this, EventArgs.Empty);
                return true;
            }
            return false;
        }

        public override DispatcherOperationStatus Wait(TimeSpan timeout)
        {
            if (_status == DispatcherOperationStatus.Pending || _status == DispatcherOperationStatus.Executing)
            {
                ManualResetEvent resetEvent = new ManualResetEvent(false);
                Task.WhenAny(_callbackTask.Task, Task.Delay(timeout), _abortTask.Task).ContinueWith(_ => resetEvent.Set());
                resetEvent.WaitOne();
            }
            return Status;
        }

        public void Invoke()
        {
            _status = DispatcherOperationStatus.Executing;
            try
            {
                _result = _callback();
            }
            finally
            {
                _status = DispatcherOperationStatus.Completed;
                _callbackTask.SetResult(_result);
            }
        }

        protected override bool SetPriority(DispatcherPriority priority)
        {
            return _dispatcher.SetPriority(this, priority);
        }
    }
}
