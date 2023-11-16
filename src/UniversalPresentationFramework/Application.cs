using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public class Application
    {
        private bool _isRunning;
        public static Application? Current { get; private set; }

        public bool IsRunning => _isRunning;

        #region Providers

        private IRendererProvider? _rendererProvider;
        public IRendererProvider? RendererProvider
        {
            get => _rendererProvider;
            set
            {
                if (_isRunning)
                    throw new InvalidOperationException("Could not set renderer provider while application is running.");
                _rendererProvider = value;
            }
        }

        private IWindowProvider? _windowProvider;
        public IWindowProvider? WindowProvider
        {
            get => _windowProvider;
            set
            {
                if (_isRunning)
                    throw new InvalidOperationException("Could not set window provider while application is running.");
                _windowProvider = value;
            }
        }

        private ILifecycleProvider? _lifecycleProvider;
        public ILifecycleProvider? LifecycleProvider
        {
            get => _lifecycleProvider;
            set
            {
                if (_isRunning)
                    throw new InvalidOperationException("Could not set lifecycle provider while application is running.");
                _lifecycleProvider = value;
            }
        }

        #endregion

        public int Run()
        {
            return Run(null);
        }

        public int Run(Window? window)
        {
            if (_windowProvider == null)
                throw new InvalidOperationException("Can not start application because window provider is not set.");
            if (_lifecycleProvider == null)
                throw new InvalidOperationException("Can not start application because lifecycle provider is not set.");
            if (_rendererProvider == null)
                throw new InvalidOperationException("Can not start application because renderer provider is not set.");
            if (_isRunning)
                throw new InvalidOperationException("Application running already.");
            _isRunning = true;
            Current = this;
            _lifecycleProvider.Start();
            FrameworkProvider.RendererProvider = _rendererProvider;
            if (window == null)
            {

            }
            else
            {
                window.Show();
            }
            _lifecycleProvider.WaitForEnd();
            return 0;
        }
    }
}
