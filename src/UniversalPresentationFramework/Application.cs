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

        #endregion

        public int Run()
        {
            if (_windowProvider == null)
                throw new InvalidOperationException("Can not start application because window provider is not set.");
            if (_isRunning)
                throw new InvalidOperationException("Application running already.");
            return 0;
        }
    }
}
