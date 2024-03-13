using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Providers;

namespace Wodsoft.UI.Platforms.Win32
{
    public class LifecycleProvider : ILifecycleProvider
    {
        private bool _isRunning;
        private object _lock = new object();
        private ManualResetEvent? _eventEvent;
        private readonly WindowProvider _windowProvider;

        public LifecycleProvider(WindowProvider windowProvider)
        {
            windowProvider.WindowEmpty += WindowEmpty;
            _windowProvider = windowProvider;
        }

        private void WindowEmpty(object? sender, EventArgs e)
        {
            _eventEvent?.Set();
        }

        public void Start()
        {
            lock (_lock)
            {
                if (_isRunning)
                    throw new InvalidOperationException("Application is running already.");
                _eventEvent = new ManualResetEvent(false);
                _isRunning = true;
            }
        }

        public void Stop()
        {
            lock (_lock)
            {
                if (!_isRunning)
                    throw new InvalidOperationException("Application is not running.");
                _windowProvider.CloseAll();
            }
        }

        public void WaitForEnd()
        {
            if (!_isRunning)
                return;
            _eventEvent?.WaitOne();
        }
    }
}
