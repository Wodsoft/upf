using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32;
using Windows.Win32.Foundation;
using Wodsoft.UI.Input;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Platforms.Win32
{
    public class Win32MouseDevice : MouseDevice
    {
        private readonly WindowContext _windowContext;

        internal Win32MouseDevice(WindowContext windowContext)
        {
            _windowContext = windowContext;
        }

        public override Point GetPosition(IInputElement relativeTo)
        {
            Visual? visual = relativeTo as Visual;
            if (visual == null)
            {
                LogicalObject? logicalObject = relativeTo as LogicalObject;
                if (logicalObject == null)
                    return new Point(0, 0);
                while (visual == null)
                {
                    logicalObject = LogicalTreeHelper.GetParent(logicalObject!);
                    if (logicalObject == null)
                        return new Point(0, 0);
                    visual = logicalObject as Visual;
                }
            }

            var point = MousePoint;
            point.X -= -_windowContext.X;
            point.Y -= -_windowContext.Y;
            point.X /= _windowContext.DpiX;
            point.Y /= _windowContext.DpiY;
            point.X -= visual.VisualOffset.X;
            point.Y -= visual.VisualOffset.Y;
            return point;
        }

        protected override MouseButtonState GetButtonState(MouseButton mouseButton)
        {
            throw new NotImplementedException();
        }

        protected override IInputElement? GetMouseOver(in int x, in int y)
        {
            var visual = GetMouseOver(_windowContext.Window, GetPointRelateToClientRect(x, y));
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

        protected override IInputElement? GetMouseOver(in IInputElement element, in int x, in int y)
        {
            if (element is not Visual)
                return null;
            var visual = GetMouseOver((Visual)element, GetPointRelateToClientRect(x, y));
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

        private Visual? GetMouseOver(in Visual visual, in Point point)
        {
            if (visual.HitTest(point))
                return visual;
            var childrenCount = VisualTreeHelper.GetChildrenCount(visual);
            for (int i = 0; i < childrenCount; i++)
            {
                var childTest = GetMouseOver(VisualTreeHelper.GetChild(visual, i), point);
                if (childTest != null)
                    return childTest;
            }
            return null;
        }

        private Point GetPointRelateToClientRect(in int x, in int y)
        {
            float px = x - _windowContext.X;
            float py = y - _windowContext.Y;
            px /= _windowContext.DpiX;
            py /= _windowContext.DpiY;
            return new Point(px, py);
        }

        protected override bool CaptureMouse()
        {
            return !PInvoke.SetCapture(_windowContext.Hwnd).IsNull;
        }

        protected override void ReleaseMouse()
        {
            _windowContext.ProcessInWindowThread(() => PInvoke.ReleaseCapture());
        }
    }
}
