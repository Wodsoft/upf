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
            Focus(null);
        }

        public IInputElement? Focus(IInputElement? element)
        {
            //if (element == null)
            //{
            //    if (ActiveSource == null)
            //        return _focusedElement;
            //    element = ActiveSource.RootVisual as IInputElement;
            //    if (element == null)
            //        return _focusedElement;
            //}
            //else 
            if (element != null)
            {
                if (element is not DependencyObject)
                    return _focusedElement;
                if (!element.Focusable)
                    return _focusedElement;
                if (element == _focusedElement)
                    return element;
            }
            var oldElement = _focusedElement;
            if (oldElement != null)
            {
                KeyboardFocusChangedEventArgs previewLostFocus = new KeyboardFocusChangedEventArgs(this, Environment.TickCount, oldElement, element);
                previewLostFocus.RoutedEvent = Keyboard.PreviewLostKeyboardFocusEvent;
                previewLostFocus.Source = oldElement;
                oldElement.RaiseEvent(previewLostFocus);
                if (previewLostFocus.Handled)
                    return oldElement;
            }
            if (element != null)
            {
                KeyboardFocusChangedEventArgs previewGotFocus = new KeyboardFocusChangedEventArgs(this, Environment.TickCount, oldElement, element);
                previewGotFocus.RoutedEvent = Keyboard.PreviewGotKeyboardFocusEvent;
                previewGotFocus.Source = element;
                element.RaiseEvent(previewGotFocus);
                if (previewGotFocus.Handled)
                    return oldElement;
            }
            //if (oldElement != null && element == null)
            //{
            //    if (oldElement.Dispatcher is UIDispatcher dispatcher)
            //        dispatcher.InputMethod.Disable(oldElement);
            //}
            //else if (element != null)
            //{
            //    if (element.Dispatcher is UIDispatcher dispatcher)
            //        dispatcher.InputMethod.Enable(element);
            //}
            if (ChangeFocus(oldElement, element))
            {
                _focusedElement = element;
                if (oldElement != null)
                {
                    ((DependencyObject)oldElement).SetValue(UIElement.IsKeyboardFocusedPropertyKey, false);
                    KeyboardFocusChangedEventArgs lostFocus = new KeyboardFocusChangedEventArgs(this, Environment.TickCount, oldElement, element);
                    lostFocus.RoutedEvent = Keyboard.LostKeyboardFocusEvent;
                    lostFocus.Source = oldElement;
                    oldElement.RaiseEvent(lostFocus);
                }
                if (element != null)
                {
                    var newElement = (DependencyObject)element;
                    newElement.SetValue(UIElement.IsKeyboardFocusedPropertyKey, true);
                    KeyboardFocusChangedEventArgs gotFocus = new KeyboardFocusChangedEventArgs(this, Environment.TickCount, oldElement, element);
                    gotFocus.RoutedEvent = Keyboard.GotKeyboardFocusEvent;
                    gotFocus.Source = element;
                    element.RaiseEvent(gotFocus);
                    if (newElement.Dispatcher is UIDispatcher dispatcher)
                    {
                        dispatcher.InputMethod.GotKeyboardFocus(newElement);
                    }
                }
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
            var element = _focusedElement;
            if (element == null)
                return;
            if (input.IsCharCode)
            {
                
            }
            else
            {
                var e = new KeyEventArgs(this, input.MessageTime, input.Key, input.KeyStates);
                switch (input.KeyStates)
                {
                    case KeyStates.None:
                        e.RoutedEvent = Keyboard.PreviewKeyUpEvent;
                        break;
                    case KeyStates.Down:
                        e.RoutedEvent = Keyboard.PreviewKeyDownEvent;
                        break;
                    default:
                        return;
                }
                element.RaiseEvent(e);
                if (!e.Handled)
                {
                    switch (input.KeyStates)
                    {
                        case KeyStates.None:
                            e.RoutedEvent = Keyboard.KeyUpEvent;
                            break;
                        case KeyStates.Down:
                            e.RoutedEvent = Keyboard.KeyDownEvent;
                            break;
                    }
                    element.RaiseEvent(e);
                }
            }
        }

        #endregion
    }
}
