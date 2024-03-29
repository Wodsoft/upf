using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Threading
{
    public abstract class Dispatcher
    {
        public abstract Thread Thread { get; }

        public abstract bool CheckAccess();

        public abstract void VerifyAccess();

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
    }
}
