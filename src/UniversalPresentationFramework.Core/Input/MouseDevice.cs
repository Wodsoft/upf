using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Input
{
    public abstract class MouseDevice : InputDevice
    {
        #region Properties 

        /// <summary>
        ///     The state of the left button.
        /// </summary>
        public MouseButtonState LeftButton
        {
            get
            {
                return GetButtonState(MouseButton.Left);
            }
        }

        /// <summary>
        ///     The state of the right button.
        /// </summary>
        public MouseButtonState RightButton
        {
            get
            {
                return GetButtonState(MouseButton.Right);
            }
        }

        /// <summary>
        ///     The state of the middle button.
        /// </summary>
        public MouseButtonState MiddleButton
        {
            get
            {
                return GetButtonState(MouseButton.Middle);
            }
        }

        /// <summary>
        ///     The state of the first extended button.
        /// </summary>
        public MouseButtonState XButton1
        {
            get
            {
                return GetButtonState(MouseButton.XButton1);
            }
        }

        /// <summary>
        ///     The state of the second extended button.
        /// </summary>
        public MouseButtonState XButton2
        {
            get
            {
                return GetButtonState(MouseButton.XButton2);
            }
        }

        #endregion

        #region Methods

        protected abstract MouseButtonState GetButtonState(MouseButton mouseButton);

        public abstract Point GetPosition(IInputElement relativeTo);

        protected abstract IInputElement? GetMouseOver(in Int32Point point);

        protected abstract IInputElement? GetMouseOver(in IInputElement element, in Int32Point point);

        public bool Capture(IInputElement? element)
        {
            return Capture(element, CaptureMode.Element);
        }

        public bool Capture(IInputElement? element, CaptureMode captureMode)
        {
            if (captureMode == CaptureMode.None)
                element = null;
            if (element == null)
            {
                if (_captureMode != CaptureMode.None)
                {
                    var capturedElement = _capturedElement!;
                    ReleaseMouse();
                    ChangeElementMouseCaptured(capturedElement, false);
                }
                return true;
            }
            else
            {
                if (CaptureMouse())
                {
                    var capturedElement = _capturedElement;
                    if (capturedElement != null)
                        ChangeElementMouseCaptured(capturedElement, false);
                    _capturedElement = element;
                    ChangeElementMouseCaptured(element, true);
                    return true;
                }
                return false;
            }
        }

        protected abstract bool CaptureMouse();

        protected abstract void ReleaseMouse();

        private void ChangeElementMouseCaptured(IInputElement inputElement, bool isCaptured)
        {
            var e = new MouseEventArgs(this, 0);
            e.RoutedEvent = isCaptured ? Mouse.GotMouseCaptureEvent : Mouse.LostMouseCaptureEvent;
            inputElement.RaiseEvent(e);
        }

        #endregion

        #region Input Handle

        private Int32Point _lastPoint;
        private IInputElement? _capturedElement;
        private CaptureMode _captureMode;

        public IInputElement? Captured => _capturedElement;

        protected Int32Point MousePoint => _lastPoint;

        internal void HandleInput(in MouseInput input)
        {
            switch (input.Actions)
            {
                case MouseActions.Move:
                    {
                        if (_lastPoint == input.Point)
                            return;
                        _lastPoint = input.Point;
                        var targetElement = GetTargetElement(_lastPoint);
                        if (targetElement != null)
                            HandleMouseMove(targetElement, input);
                        break;
                    }
                case MouseActions.Press:
                case MouseActions.Release:
                    {
                        if (input.Button == null)
                            return;
                        IInputElement? targetElement;
                        if (_lastPoint != input.Point)
                        {
                            _lastPoint = input.Point;
                            targetElement = GetTargetElement(_lastPoint);
                            if (targetElement != null)
                                HandleMouseMove(targetElement, input);
                        }
                        else
                            targetElement = GetTargetElement(_lastPoint);
                        if (targetElement != null)
                            HandleMouseButton(targetElement, input);
                        break;
                    }
                case MouseActions.Wheel:
                    {
                        IInputElement? targetElement;
                        if (_lastPoint != input.Point)
                        {
                            _lastPoint = input.Point;
                            targetElement = GetTargetElement(_lastPoint);
                            if (targetElement != null)
                                HandleMouseMove(targetElement, input);
                        }
                        else
                            targetElement = GetTargetElement(_lastPoint);
                        if (targetElement != null)
                            HandleMouseWheel(targetElement, input);
                        break;
                    }
                case MouseActions.CancelCapture:
                    {
                        var oldCaptureElement = _capturedElement;
                        if (oldCaptureElement == null)
                            return;
                        var e = new MouseEventArgs(this, input.MessageTime);
                        e.RoutedEvent = Mouse.LostMouseCaptureEvent;
                        oldCaptureElement.RaiseEvent(e);
                        _captureMode = CaptureMode.None;
                        _capturedElement = null;
                        break;
                    }
            }
        }

        private IInputElement? GetTargetElement(in Int32Point point)
        {
            switch (_captureMode)
            {
                case CaptureMode.None:
                    return GetMouseOver(point);
                case CaptureMode.Element:
                    return _capturedElement;
                case CaptureMode.SubTree:
                    return GetMouseOver(_capturedElement!, point);
                default:
                    throw new NotSupportedException();
            }
        }

        private void HandleMouseMove(IInputElement targetElement, in MouseInput input)
        {
            var e = new MouseEventArgs(this, input.MessageTime);
            e.RoutedEvent = Mouse.PreviewMouseMoveEvent;
            targetElement.RaiseEvent(e);
            if (!e.Handled)
            {
                e.RoutedEvent = Mouse.MouseMoveEvent;
                targetElement.RaiseEvent(e);
            }
        }

        private void HandleMouseButton(IInputElement targetElement, in MouseInput input)
        {
            var e = new MouseButtonEventArgs(this, input.MessageTime, input.Button!.Value);
            e.RoutedEvent = input.Actions == MouseActions.Press ? Mouse.PreviewMouseDownEvent : Mouse.PreviewMouseUpEvent;
            targetElement.RaiseEvent(e);
            if (!e.Handled)
            {
                e.RoutedEvent = input.Actions == MouseActions.Press ? Mouse.MouseDownEvent : Mouse.MouseUpEvent;
                targetElement.RaiseEvent(e);
            }
        }

        private void HandleMouseWheel(IInputElement targetElement, in MouseInput input)
        {
            var e = new MouseWheelEventArgs(this, input.MessageTime, input.Wheel);
            e.RoutedEvent = Mouse.PreviewMouseWheelEvent;
            targetElement.RaiseEvent(e);
            if (!e.Handled)
            {
                e.RoutedEvent = Mouse.MouseWheelEvent;
                targetElement.RaiseEvent(e);
            }
        }

        #endregion
    }
}
