using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Platforms.Win32
{
    public class WindowProvider : IWindowProvider
    {
        private List<IWindowContext> _contexts = new List<IWindowContext>();
        private object _lock = new object();

        public IWindowContext CreateContext(Window window)
        {
            var context = new WindowContext(window);
            context.Disposed += Context_Disposed;
            context.Opened += Context_CreateOpened;
            context.Closed += Context_Closed;
            return context;
        }

        public event EventHandler? WindowEmpty;

        private void Context_Closed(IWindowContext context)
        {
            lock (_lock)
            {
                _contexts.Remove(context);
                if (_contexts.Count == 0)
                    WindowEmpty?.Invoke(this, EventArgs.Empty);
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
    }
}
