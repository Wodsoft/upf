using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Providers;

namespace Wodsoft.UI.Platforms.Win32
{
    public class WindowProvider : IWindowProvider
    {
        private readonly Win32Platform _platform;
        private readonly List<IWindowContext> _contexts = new List<IWindowContext>();
        private readonly object _lock = new object();
        private Exception? _exception;

        public WindowProvider(Win32Platform platform)
        {
            _platform = platform;
        }

        public IWindowContext CreateContext(Window window)
        {
            var context = new WindowContext(window, _platform.RendererProvider, _platform.RendererContextType, _platform.ThemeProvider.OnThemeChanged);
            context.Disposed += Context_Disposed;
            context.Opened += Context_CreateOpened;
            context.Closed += Context_Closed;
            return context;
        }

        public event EventHandler? WindowEmpty;

        internal Exception? Exception => _exception;

        private void Context_Closed(IWindowContext context)
        {
            lock (_lock)
            {
                var ex = ((WindowContext)context).Exception;
                _contexts.Remove(context);
                if (ex == null)
                {
                    if (_contexts.Count == 0)
                        WindowEmpty?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    _exception = ex;
                    for (int i = _contexts.Count - 1; i >= 0; --i)
                    {
                        var c = _contexts[i];
                        Context_Disposed(c);
                        ((WindowContext)c).DestoryWindow();
                        _contexts.Remove(c);
                    }
                    WindowEmpty?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private void Context_CreateOpened(IWindowContext context)
        {
            lock (_lock)
            {
                _contexts.Add(context);
            }
        }

        private void Context_Disposed(IWindowContext context)
        {
            WindowContext windowContext = (WindowContext)context;
            windowContext.Disposed -= Context_Disposed;
            windowContext.Opened -= Context_CreateOpened;
            windowContext.Closed -= Context_Closed;
        }

        public void CloseAll()
        {
            lock (_lock)
            {
                if (_contexts.Count == 0)
                    return;
                for (int i = _contexts.Count - 1; i >= 0; --i)
                {
                    var context = _contexts[i];
                    Context_Disposed(context);
                    ((WindowContext)context).DestoryWindow();
                    _contexts.Remove(context);
                }
            }
            WindowEmpty?.Invoke(this, EventArgs.Empty);
        }
    }
}
