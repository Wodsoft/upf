using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Input.KeyboardAndMouse;
using Wodsoft.UI.Input;
using Wodsoft.UI.Media;
using Wodsoft.UI.Threading;

namespace Wodsoft.UI.Platforms.Win32
{
    public class Win32MouseDevice : MouseDevice
    {
        internal WindowContext? CurrentContext { get; set; }

        public override PresentationSource? ActiveSource => CurrentContext?.Source;

        public override Point GetPosition(IInputElement relativeTo)
        {
            if (relativeTo is not LogicalObject current)
                return new Point();
            var point = new Point();
            Window? window;
            while (true)
            {
                if (current is Visual visual)
                {
                    point.X += visual.VisualOffset.X;
                    point.Y += visual.VisualOffset.Y;
                    if (visual.VisualParent == null)
                    {
                        window = visual as Window;
                        if (window == null)
                            return new Point();
                        break;
                    }
                    current = visual.VisualParent;
                }
                else
                {
                    var parent = LogicalTreeHelper.GetParent(current);
                    if (parent == null)
                        return new Point();
                    current = parent;
                }
            }
            if (window.Dispatcher is not Win32Dispatcher dispatcher)
                return new Point();
            var dpi = window.GetDpi();
            point.X *= dpi.DpiScaleX;
            point.Y *= dpi.DpiScaleY;
            point.X += dispatcher.WindowContext.ClientX;
            point.Y += dispatcher.WindowContext.ClientY;
            if (!PInvoke.GetCursorPos(out var mousePoint))
                return new Point();
            point.X = mousePoint.X - point.X;
            point.Y = mousePoint.Y - point.Y;
            point.X /= dpi.DpiScaleX;
            point.Y /= dpi.DpiScaleY;
            return point;
        }

        protected override Point GetPosition(IInputElement relativeTo, Int32Point mousePoint)
        {
            if (relativeTo is not LogicalObject current)
                return new Point();
            var point = new Point();
            Window? window;
            while (true)
            {
                if (current is Visual visual)
                {
                    point.X += visual.VisualOffset.X;
                    point.Y += visual.VisualOffset.Y;
                    if (visual.VisualParent == null)
                    {
                        window = visual as Window;
                        if (window == null)
                            return new Point();
                        break;
                    }
                    current = visual.VisualParent;
                }
                else
                {
                    var parent = LogicalTreeHelper.GetParent(current);
                    if (parent == null)
                        return new Point();
                    current = parent;
                }
            }
            if (window.Dispatcher != CurrentContext?.Dispatcher)
                return new Point();
            var dpi = window.GetDpi();
            point.X = mousePoint.X / dpi.DpiScaleX - point.X;
            point.Y = mousePoint.Y / dpi.DpiScaleY - point.Y;
            return point;
        }

        protected override MouseButtonState GetButtonState(MouseButton mouseButton)
        {
            int key;
            switch (mouseButton)
            {
                case MouseButton.Left:
                    key = (int)VIRTUAL_KEY.VK_LBUTTON;
                    break;
                case MouseButton.Right:
                    key = (int)VIRTUAL_KEY.VK_RBUTTON;
                    break;
                case MouseButton.Middle:
                    key = (int)VIRTUAL_KEY.VK_MBUTTON;
                    break;
                case MouseButton.XButton1:
                    key = (int)VIRTUAL_KEY.VK_XBUTTON1;
                    break;
                case MouseButton.XButton2:
                    key = (int)VIRTUAL_KEY.VK_XBUTTON2;
                    break;
                default:
                    return MouseButtonState.Released;
            }
            return (PInvoke.GetKeyState(key) & 0x8000) != 0 ? MouseButtonState.Pressed : MouseButtonState.Pressed;
        }

        protected override IInputElement? GetMouseOver(in Int32Point point)
        {
            var windowContext = CurrentContext;
            if (windowContext == null)
                return null;
            var visual = windowContext.Window.HitTest(GetPointRelateToClientRect(windowContext, point));
            if (visual == null)
                return windowContext.Window;
            do
            {
                if (visual is IInputElement inputElement)
                    return inputElement;
                visual = visual.VisualParent;
            }
            while (visual != null);
            return windowContext.Window;
        }

        protected override IInputElement? GetMouseOver(in IInputElement element, in Int32Point mousePoint)
        {
            var windowContext = CurrentContext;
            if (windowContext == null)
                return null;
            if (element is not Visual current)
                return null;
            Visual? visual = current;
            var point = GetPointRelateToClientRect(windowContext, mousePoint);
            while (true)
            {
                point -= current.VisualOffset;
                if (current.VisualParent == null)
                    break;
                current = current.VisualParent;
            }
            visual = visual.HitTest(point);
            if (visual == null)
                return null;
            do
            {
                if (visual is IInputElement inputElement)
                    return inputElement;
                visual = visual.VisualParent;
            }
            while (visual != null);
            return null;
        }

        private Point GetPointRelateToClientRect(WindowContext windowContext, in Int32Point point)
        {
            float px = point.X;
            float py = point.Y;
            px /= windowContext.DpiX;
            py /= windowContext.DpiY;
            return new Point(px, py);
        }

        protected override bool CaptureMouse()
        {
            var windowContext = CurrentContext;
            if (windowContext == null)
                return false;
            return !PInvoke.SetCapture(windowContext.Hwnd).IsNull;
        }

        protected override void ReleaseMouse()
        {
            var windowContext = CurrentContext;
            if (windowContext == null)
                return;
            windowContext.ProcessInWindowThread(() => PInvoke.ReleaseCapture());
        }

        protected override bool UpdateCursor(Cursor cursor)
        {
            var windowContext = CurrentContext;
            if (windowContext == null)
                return false;
            if (cursor.CursorType == CursorType.Custom)
            {
                if (cursor.Context is not Win32CursorContext context)
                    throw new InvalidOperationException("Invalid cursor context.");
                windowContext.ProcessInWindowThread(() => InputProvider.SetCursor(context));
            }
            else
                windowContext.ProcessInWindowThread(() => InputProvider.SetCursor(cursor.CursorType));
            return true;
        }
    }
}
