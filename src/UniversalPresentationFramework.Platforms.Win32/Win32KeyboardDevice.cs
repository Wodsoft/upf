using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D12;
using Windows.Win32;
using Wodsoft.UI.Input;
using Wodsoft.UI.Threading;

namespace Wodsoft.UI.Platforms.Win32
{
    public class Win32KeyboardDevice : KeyboardDevice
    {
        internal WindowContext? CurrentContext { get; set; }

        public override PresentationSource? ActiveSource => CurrentContext?.Source;

        public override ModifierKeys Modifiers
        {
            get
            {

                ModifierKeys modifiers = ModifierKeys.None;
                if (GetIsKeyDown(Key.LeftAlt) || GetIsKeyDown(Key.RightAlt))
                    modifiers |= ModifierKeys.Alt;
                if (GetIsKeyDown(Key.LeftCtrl) || GetIsKeyDown(Key.RightCtrl))
                    modifiers |= ModifierKeys.Control;
                if (GetIsKeyDown(Key.LeftShift) || GetIsKeyDown(Key.RightShift))
                    modifiers |= ModifierKeys.Shift;
                return modifiers;
            }
        }

        private bool GetIsKeyDown(Key key)
        {
            var state = PInvoke.GetKeyState(KeyInterop.VirtualKeyFromKey(key));
            return (state & 0x00008000) == 0x00008000;
        }

        public override KeyStates GetKeyStates(Key key)
        {
            var state = PInvoke.GetKeyState(KeyInterop.VirtualKeyFromKey(key));
            KeyStates keyStates = KeyStates.None;
            if ((state & 0x00008000) == 0x00008000)
                keyStates |= KeyStates.Down;
            if ((state & 0x00000001) == 0x00000001)
                keyStates |= KeyStates.Toggled;
            return keyStates;
        }

        public override bool IsKeyDown(Key key)
        {
            var state = PInvoke.GetKeyState(KeyInterop.VirtualKeyFromKey(key));
            return (state & 0x00008000) == 0x00008000;
        }

        public override bool IsKeyToggled(Key key)
        {
            var state = PInvoke.GetKeyState(KeyInterop.VirtualKeyFromKey(key));
            return (state & 0x00000001) == 0x00008000;
        }

        public override bool IsKeyUp(Key key)
        {
            var state = PInvoke.GetKeyState(KeyInterop.VirtualKeyFromKey(key));
            return state == 0;
        }

        protected override bool ChangeFocus(IInputElement? oldElement, IInputElement? newElement)
        {
            Dispatcher? oldDispatch, newDispatcher;
            if (oldElement == null)
                oldDispatch = null;
            else
                oldDispatch = ((DependencyObject)oldElement).Dispatcher;
            if (newElement == null)
                newDispatcher = null;
            else
                newDispatcher = ((DependencyObject)newElement).Dispatcher;
            if (oldDispatch != newDispatcher)
            {
                if (newDispatcher == null)
                    return PInvoke.SetFocus(default) != default;
                else
                {
                    if (newDispatcher is not Win32Dispatcher win32Dispatcher)
                        return false;
                    return PInvoke.SetFocus(win32Dispatcher.WindowContext.Hwnd) != default;
                }
            }
            return true;
        }
    }
}
