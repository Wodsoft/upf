using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Threading
{
    public abstract class DispatcherOperation
    {
        private DispatcherPriority _priority;
        public DispatcherOperation(Dispatcher dispatcher, DispatcherPriority priority)
        {
            Dispatcher = dispatcher;
            Priority = priority;
        }

        public Dispatcher Dispatcher { get; }

        public DispatcherPriority Priority
        {
            get => _priority;
            set
            {
                if (value != _priority && SetPriority(value))
                {
                    _priority = value;
                }
            }
        }

        public virtual DispatcherOperationStatus Status
        {
            get
            {
                switch (Task.Status)
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

        public abstract Task Task { get; }

        public TaskAwaiter GetAwaiter() => Task.GetAwaiter();

        public DispatcherOperationStatus Wait() => Wait(TimeSpan.FromMicroseconds(-1));

        public abstract DispatcherOperationStatus Wait(TimeSpan timeout);

        public abstract bool Abort();

        public abstract event EventHandler? Aborted;

        public abstract event EventHandler? Completed;

        protected abstract bool SetPriority(DispatcherPriority priority);

    }

    public abstract class DispatcherOperation<TResult> : DispatcherOperation
    {
        public DispatcherOperation(Dispatcher dispatcher, DispatcherPriority priority) : base(dispatcher, priority) { }

        public abstract TResult Result { get; }
    }
}
