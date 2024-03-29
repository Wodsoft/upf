using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Threading
{
    internal class EmptyDispatcherOperation : DispatcherOperation
    {
        private readonly Task _task;

        public EmptyDispatcherOperation(DispatcherPriority priority, Task task) : base(EmptyDispatcher.Default, priority)
        {
            _task = task;
        }

        public override Task Task => _task;

        public override event EventHandler? Aborted;
        public override event EventHandler? Completed;

        public override bool Abort()
        {
            return false;
        }

        public override DispatcherOperationStatus Wait(TimeSpan timeout)
        {
            if (!_task.IsCompleted)
                _task.Wait(timeout);
            return Status;
        }

        protected override bool SetPriority(DispatcherPriority priority)
        {
            return false;
        }
    }

    internal class EmptyDispatcherOperation<TResult> : DispatcherOperation<TResult>
    {
        private readonly Task _task;
        private TResult? _result;

        public EmptyDispatcherOperation(DispatcherPriority priority, Task<TResult> task) : base(EmptyDispatcher.Default, priority)
        {
            _task = task.ContinueWith(task =>
            {
                _result = task.Result;
            });
        }

        public override DispatcherOperationStatus Status
        {
            get
            {
                switch (_task.Status)
                {
                    case TaskStatus.Running:
                    case TaskStatus.WaitingForChildrenToComplete:
                        return DispatcherOperationStatus.Executing;
                    case TaskStatus.RanToCompletion:
                    case TaskStatus.Faulted:
                        return DispatcherOperationStatus.Completed;
                    case TaskStatus.Canceled:
                        return DispatcherOperationStatus.Aborted;
                    default:
                        return DispatcherOperationStatus.Pending;
                }
            }
        }

        public override Task Task => _task;

        public override TResult Result => _result!;

        public override event EventHandler? Aborted;
        public override event EventHandler? Completed;

        public override bool Abort()
        {
            return false;
        }

        public override DispatcherOperationStatus Wait(TimeSpan timeout)
        {
            if (!_task.IsCompleted)
            {
                ManualResetEvent resetEvent = new ManualResetEvent(false);
                Task.WhenAny(_task, Task.Delay(timeout)).ContinueWith(task =>
                {
                    resetEvent.Set();
                });
                resetEvent.WaitOne();
            }
            return Status;
        }

        protected override bool SetPriority(DispatcherPriority priority)
        {
            return false;
        }
    }
}
