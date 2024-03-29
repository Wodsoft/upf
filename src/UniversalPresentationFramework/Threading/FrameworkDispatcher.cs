using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Threading
{
    public abstract class FrameworkDispatcher : UIDispatcher
    {
        public override bool CheckAccess()
        {
            return Thread == Thread.CurrentThread;
        }

        public override void VerifyAccess()
        {
            if (!CheckAccess())
                throw new InvalidOperationException("Verify access failed.");
        }

        #region Invoke

        [Obsolete("Use InvokeAsync instead of BeginInvoke.")]
        public override DispatcherOperation<object?> BeginInvoke(Delegate method, DispatcherPriority priority, params object[] args)
        {
            if (priority == DispatcherPriority.Invalid || priority == DispatcherPriority.Inactive)
                throw new ArgumentException("Invalid priority.", "priority");
            return BuildOperation(priority, () => method.DynamicInvoke(args));
        }

        public override void Invoke(Action callback, DispatcherPriority priority, CancellationToken cancellationToken, TimeSpan timeout)
        {
            if (priority == DispatcherPriority.Invalid || priority == DispatcherPriority.Inactive)
                throw new ArgumentException("Invalid priority.", "priority");
            var operation = BuildOperation(priority, callback);
            operation.Wait();
        }

        public override TResult Invoke<TResult>(Func<TResult> callback, DispatcherPriority priority, CancellationToken cancellationToken, TimeSpan timeout)
        {
            if (priority == DispatcherPriority.Invalid || priority == DispatcherPriority.Inactive)
                throw new ArgumentException("Invalid priority.", "priority");
            if (priority == DispatcherPriority.Send && CheckAccess())
                return callback();
            var operation = BuildOperation(priority, callback);
            operation.Wait();
            return operation.Result;
        }

        [Obsolete("Use Invoke<TResult> instead of Invoke.")]
        public override object? Invoke(Delegate method, TimeSpan timeout, DispatcherPriority priority, params object[] args)
        {
            if (priority == DispatcherPriority.Invalid || priority == DispatcherPriority.Inactive)
                throw new ArgumentException("Invalid priority.", "priority");
            if (priority == DispatcherPriority.Send && CheckAccess())
                return method.DynamicInvoke(args);
            var operation = BuildOperation(priority, () => method.DynamicInvoke(args));
            operation.Wait();
            return operation.Result;
        }

        public override DispatcherOperation InvokeAsync(Action callback, DispatcherPriority priority, CancellationToken cancellationToken)
        {
            if (priority == DispatcherPriority.Invalid || priority == DispatcherPriority.Inactive)
                throw new ArgumentException("Invalid priority.", "priority");
            return BuildOperation(priority, callback);
        }

        public override DispatcherOperation<TResult> InvokeAsync<TResult>(Func<TResult> callback, DispatcherPriority priority, CancellationToken cancellationToken)
        {
            if (priority == DispatcherPriority.Invalid || priority == DispatcherPriority.Inactive)
                throw new ArgumentException("Invalid priority.", "priority");
            return BuildOperation(priority, callback);
        }

        private DispatcherOperation BuildOperation(DispatcherPriority priority, Action callback)
        {
            var operation = new FrameworkDispatcherOperation(this, priority, callback);
            QueueOperation(operation, priority);
            return operation;
        }

        private DispatcherOperation<TResult> BuildOperation<TResult>(DispatcherPriority priority, Func<TResult> callback)
        {
            var operation = new FrameworkDispatcherOperation<TResult>(this, priority, callback);
            QueueOperation(operation, priority);
            return operation;
        }

        private void QueueOperation(DispatcherOperation operation, DispatcherPriority priority)
        {
            switch (priority)
            {
                case DispatcherPriority.ApplicationIdle:
                case DispatcherPriority.SystemIdle:
                    throw new NotImplementedException();
                case DispatcherPriority.Normal:
                    lock (_normalLock)
                    {
                        if (_normalTasks == null)
                            _normalTasks = new List<DispatcherOperation>();
                        _normalTasks.Add(operation);
                    }
                    break;
                case DispatcherPriority.DataBind:
                    lock (_bindLock)
                    {
                        if (_bindTasks == null)
                            _bindTasks = new List<DispatcherOperation>();
                        _bindTasks.Add(operation);
                    }
                    break;
                case DispatcherPriority.Render:
                    lock (_renderLock)
                    {
                        if (_renderTasks == null)
                            _renderTasks = new List<DispatcherOperation>();
                        _renderTasks.Add(operation);
                    }
                    break;
                case DispatcherPriority.Loaded:
                    lock (_loadedLock)
                    {
                        if (_loadedTasks == null)
                            _loadedTasks = new List<DispatcherOperation>();
                        _loadedTasks.Add(operation);
                    }
                    break;
                case DispatcherPriority.Input:
                    lock (_inputLock)
                    {
                        if (_inputTasks == null)
                            _inputTasks = new List<DispatcherOperation>();
                        _inputTasks.Add(operation);
                    }
                    break;
                case DispatcherPriority.Background:
                    lock (_backgroundLock)
                    {
                        if (_backgroundTasks == null)
                            _backgroundTasks = new List<DispatcherOperation>();
                        _backgroundTasks.Add(operation);
                    }
                    break;
                case DispatcherPriority.Send:
                    _sendTasks.Push(operation);
                    OnSendOperationQueued();
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        protected abstract void OnSendOperationQueued();

        internal bool SetPriority(DispatcherOperation operation, DispatcherPriority priority)
        {
            if (operation.Status != DispatcherOperationStatus.Pending)
                return false;
            switch (operation.Priority)
            {
                case DispatcherPriority.ApplicationIdle:
                case DispatcherPriority.SystemIdle:
                    throw new NotImplementedException();
                case DispatcherPriority.Normal:
                    lock (_normalLock)
                    {
                        if (!_normalTasks!.Remove(operation))
                            return false;
                    }
                    break;
                case DispatcherPriority.DataBind:
                    lock (_bindLock)
                    {
                        if (!_bindTasks!.Remove(operation))
                            return false;
                    }
                    break;
                case DispatcherPriority.Render:
                    lock (_renderLock)
                    {
                        if (!_renderTasks!.Remove(operation))
                            return false;
                    }
                    break;
                case DispatcherPriority.Loaded:
                    lock (_loadedLock)
                    {
                        if (!_loadedTasks!.Remove(operation))
                            return false;
                    }
                    break;
                case DispatcherPriority.Input:
                    lock (_inputLock)
                    {
                        if (!_inputTasks!.Remove(operation))
                            return false;
                    }
                    break;
                case DispatcherPriority.Background:
                    lock (_backgroundLock)
                    {
                        if (!_backgroundTasks!.Remove(operation))
                            return false;
                    }
                    break;
                case DispatcherPriority.Send:
                    return false;
                default:
                    throw new NotSupportedException();
            }
            QueueOperation(operation, priority);
            return true;
        }

        #endregion

        #region Operation

        private readonly object
            _normalLock = new object(),
            _bindLock = new object(),
            _renderLock = new object(),
            _loadedLock = new object(),
            _inputLock = new object(),
            _backgroundLock = new object(),
            _executeLock = new object();
        private List<DispatcherOperation>? _normalTasks, _bindTasks, _renderTasks, _loadedTasks, _inputTasks, _backgroundTasks;
        private readonly ConcurrentStack<DispatcherOperation> _sendTasks = new ConcurrentStack<DispatcherOperation>();
        private readonly List<DispatcherOperation?> _executeTasks = new List<DispatcherOperation?>();
        private int _executeIndex;

        public void RunFrame()
        {
            RunSendOperations();
            lock (_normalLock)
            {
                if (_normalTasks != null && _normalTasks.Count != 0)
                {
                    _executeTasks.AddRange(_normalTasks);
                    _normalTasks.Clear();
                }
            }
            if (_executeTasks.Count != 0)
                RunOperations(_executeTasks);
            lock (_bindLock)
            {
                if (_bindTasks != null && _bindTasks.Count != 0)
                {
                    _executeTasks.AddRange(_bindTasks);
                    _bindTasks.Clear();
                }
            }
            if (_executeTasks.Count != 0)
                RunOperations(_executeTasks);
            RunBind();
            lock (_renderLock)
            {
                if (_renderTasks != null && _renderTasks.Count != 0)
                {
                    _executeTasks.AddRange(_renderTasks);
                    _renderTasks.Clear();
                }
            }
            if (_executeTasks.Count != 0)
                RunOperations(_executeTasks);
            RunRender();
            lock (_loadedLock)
            {
                if (_loadedTasks != null && _loadedTasks.Count != 0)
                {
                    _executeTasks.AddRange(_loadedTasks);
                    _loadedTasks.Clear();
                }
            }
            if (_executeTasks.Count != 0)
                RunOperations(_executeTasks);
            lock (_inputLock)
            {
                if (_inputTasks != null && _inputTasks.Count != 0)
                {
                    _executeTasks.AddRange(_inputTasks);
                    _inputTasks.Clear();
                }
            }
            if (_executeTasks.Count != 0)
                RunOperations(_executeTasks);
            RunInput();
            lock (_backgroundLock)
            {
                if (_backgroundTasks != null && _backgroundTasks.Count != 0)
                {
                    _executeTasks.AddRange(_backgroundTasks);
                    _backgroundTasks.Clear();
                }
            }
            if (_executeTasks.Count != 0)
                RunOperations(_executeTasks);
            RunSendOperations();
        }

        protected void RunSendOperations()
        {
            if (_sendTasks.Count != 0)
            {
                lock (_executeLock)
                {
                    while (_sendTasks.TryPop(out var operation))
                    {
                        ((IFrameworkDispatcherOperation)operation).Invoke();
                    }
                }
            }
        }

        protected void RunBind()
        {

        }

        private void RunOperations(List<DispatcherOperation?> operations)
        {
            int c = operations.Count;
            for (_executeIndex = 0; _executeIndex < c; _executeIndex++)
            {
                var operation = operations[_executeIndex];
                if (operation == null)
                    continue;
                RunSendOperations();
                lock (_executeLock)
                {
                    ((IFrameworkDispatcherOperation)operation).Invoke();
                }
            }
            operations.Clear();
        }

        internal bool Abort(DispatcherOperation operation)
        {
            lock (_executeLock)
            {
                if (_executeTasks.Count != 0)
                {
                    var i = _executeTasks.IndexOf(operation);
                    if (i != -1)
                    {
                        if (_executeIndex < i)
                        {
                            _executeTasks[i] = null;
                            return true;
                        }
                        else
                            return false;
                    }
                }
            }
            switch (operation.Priority)
            {
                case DispatcherPriority.Normal:
                    lock (_normalLock)
                    {
                        var i = _normalTasks!.IndexOf(operation);
                        if (i != -1)
                        {
                            _normalTasks.RemoveAt(i);
                            return true;
                        }
                    }
                    break;
                case DispatcherPriority.DataBind:
                    lock (_bindLock)
                    {
                        var i = _bindTasks!.IndexOf(operation);
                        if (i != -1)
                        {
                            _bindTasks.RemoveAt(i);
                            return true;
                        }
                    }
                    break;
                case DispatcherPriority.Render:
                    lock (_renderLock)
                    {
                        var i = _renderTasks!.IndexOf(operation);
                        if (i != -1)
                        {
                            _renderTasks.RemoveAt(i);
                            return true;
                        }
                    }
                    break;
                case DispatcherPriority.Loaded:
                    lock (_loadedLock)
                    {
                        var i = _loadedTasks!.IndexOf(operation);
                        if (i != -1)
                        {
                            _loadedTasks.RemoveAt(i);
                            return true;
                        }
                    }
                    break;
                case DispatcherPriority.Input:
                    lock (_inputLock)
                    {
                        var i = _inputTasks!.IndexOf(operation);
                        if (i != -1)
                        {
                            _inputTasks.RemoveAt(i);
                            return true;
                        }
                    }
                    break;
                case DispatcherPriority.Background:
                    lock (_backgroundLock)
                    {
                        var i = _backgroundTasks!.IndexOf(operation);
                        if (i != -1)
                        {
                            _backgroundTasks.RemoveAt(i);
                            return true;
                        }
                    }
                    break;

            }
            return false;
        }

        #endregion
    }
}
