using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Threading;

namespace Wodsoft.UI.Input
{
    public abstract class KeyboardDevice : InputDevice
    {
        private IInputElement? _focusedElement;

        #region Properties

        public override IInputElement? Target => _focusedElement;

        public IInputElement? FocusedElement => _focusedElement;

        public abstract ModifierKeys Modifiers { get; }

        #endregion

        #region Methods

        public void ClearFocus()
        {

        }

        public IInputElement? Focus(IInputElement? element)
        {
            if (element == null)
            {
                if (ActiveSource == null)
                    return _focusedElement;
                element = ActiveSource.RootVisual as IInputElement;
                if (element == null)
                    return _focusedElement;
            }
            else if (element is not DependencyObject)
                return _focusedElement;
            if (!element.Focusable)
                return _focusedElement;
            if (element == _focusedElement)
                return element;

            if (_focusedElement != null)
            {
                KeyboardFocusChangedEventArgs previewLostFocus = new KeyboardFocusChangedEventArgs(this, Environment.TickCount, _focusedElement, element);
                previewLostFocus.RoutedEvent = Keyboard.PreviewLostKeyboardFocusEvent;
                previewLostFocus.Source = _focusedElement;
                _focusedElement.RaiseEvent(previewLostFocus);
                if (previewLostFocus.Handled)
                    return _focusedElement;
            }
            if (element != null)
            {
                KeyboardFocusChangedEventArgs previewGotFocus = new KeyboardFocusChangedEventArgs(this, Environment.TickCount, _focusedElement, element);
                previewGotFocus.RoutedEvent = Keyboard.PreviewGotKeyboardFocusEvent;
                previewGotFocus.Source = element;
                element.RaiseEvent(previewGotFocus);
                if (previewGotFocus.Handled)
                    return _focusedElement;
            }
            if (ChangeFocus(_focusedElement, element))
            {
                _focusedElement = element;
            }
            return element;
        }

        protected abstract bool ChangeFocus(IInputElement? oldElement, IInputElement? newElement);

        public abstract bool IsKeyDown(Key key);

        public abstract bool IsKeyUp(Key key);

        public abstract bool IsKeyToggled(Key key);

        public abstract KeyStates GetKeyStates(Key key);

        #endregion

        #region Handle Input

        internal void HandleInput(in KeyboardInput input)
        {

        }

        #endregion
    }
}
