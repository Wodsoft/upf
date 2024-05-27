using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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

        public Win32Dispatcher(WindowContext windowContext, InputProvider inputProvider, Thread thread)
        {
            _windowContext = windowContext;
            Thread = thread;
            MouseDevice = inputProvider.MouseDevice;
            KeyboardDevice = inputProvider.KeyboardDevice;
        }

        public override Thread Thread { get; }

        internal WindowContext WindowContext => _windowContext;

        protected override FrameworkElement RootElement => _windowContext.Window;

        protected override void OnSendOperationQueued()
        {

        }

        protected override void RunRenderCore()
        {
            _windowContext.Render();
        }

        protected override void RunInputCore(bool hasMouseInput, bool hasKeyboardInput)
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
            RootElement.Arrange(new Rect(0, 0, _windowContext.ClientWidth / _windowContext.DpiX, _windowContext.ClientHeight / _windowContext.DpiY));
        }

        protected override MouseDevice MouseDevice { get; }

        protected override KeyboardDevice KeyboardDevice { get; }

        protected override bool IsActived => _windowContext.State != WindowState.Minimized;
    }
}
