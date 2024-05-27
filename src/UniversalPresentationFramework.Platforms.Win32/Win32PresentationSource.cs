using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;
using Wodsoft.UI.Threading;

namespace Wodsoft.UI.Platforms.Win32
{
    public sealed class Win32PresentationSource : PresentationSource
    {
        private readonly WindowContext _windowContext;

        public Win32PresentationSource(WindowContext windowContext)
        {
            _windowContext = windowContext;
        }

        public override Visual RootVisual => _windowContext.Window;

        public override bool IsDisposed => _windowContext.IsDisposed;

        public nint Handle => _windowContext.Hwnd;

        public override Dispatcher Dispatcher => _windowContext.Dispatcher;

        internal WindowContext WindowContext => _windowContext;

        internal void Start()
        {
            AddRootSource(RootVisual);
            AddSource();
        }

        internal void Stop()
        {
            RemoveRootSource(RootVisual);
            RemoveSource();
        }
    }
}
