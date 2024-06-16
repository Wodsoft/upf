using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Threading
{
    internal sealed class EmptyDispatcher : Dispatcher
    {
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
            return new EmptyDispatcherOperation<object?>(priority, Task.Run(() => method.DynamicInvoke(args)));
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
            return new EmptyDispatcherOperation(priority, Task.Run(callback));
        }

        public override DispatcherOperation<TResult> InvokeAsync<TResult>(Func<TResult> callback, DispatcherPriority priority, CancellationToken cancellationToken)
        {
            return new EmptyDispatcherOperation<TResult>(priority, Task.Run(callback));
        }

        [Obsolete("Use Invoke<TResult> instead of Invoke.")]
        public override object? Invoke(Delegate method, TimeSpan timeout, DispatcherPriority priority, params object[] args)
        {
            return method.DynamicInvoke(args);
        }

        public static readonly EmptyDispatcher Default = new EmptyDispatcher();
    }
}
