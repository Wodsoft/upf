using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Input;
using Wodsoft.UI.Threading;

namespace Wodsoft.UI.Platforms.Win32
{
    public sealed class Win32Dispatcher : FrameworkDispatcher
    {
        private readonly WindowContext _windowContext;
        private FrameworkElement? _mouseOver;

        public Win32Dispatcher(WindowContext windowContext, Thread thread)
        {
            _windowContext = windowContext;
            Thread = thread;
            MouseDevice = new Win32MouseDevice(windowContext);
        }

        public override Thread Thread { get; }

        protected override FrameworkElement RootElement => _windowContext.Window;

        protected override void OnSendOperationQueued()
        {

        }

        protected override void RunRenderCore()
        {
            _windowContext.Render();
        }

        protected override void RunInputCore(bool hasMouseInput)
        {
            if (hasMouseInput)
            {
                if (MouseDevice.Target == null && _mouseOver != null)
                {
                    _mouseOver = null;
                    _windowContext.ProcessInWindowThread(() => InputProvider.SetCursor(CursorType.Arrow));
                }
                else if (MouseDevice.Target is FrameworkElement fe && _mouseOver != fe)
                {
                    MouseDevice.UpdateCursor();
                }
            }
            _windowContext.ProcessInput();
        }

        protected override void UpdateLayoutCore()
        {
            RootElement.Arrange(new Rect(0, 0, RootElement.Width, RootElement.Height));
        }

        protected override MouseDevice MouseDevice { get; }

        protected override bool IsActived => _windowContext.State != WindowState.Minimized;
    }
}
