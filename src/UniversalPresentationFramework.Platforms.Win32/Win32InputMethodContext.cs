using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Input.KeyboardAndMouse;
using Wodsoft.UI.Input;
using Wodsoft.UI.Threading;

namespace Wodsoft.UI.Platforms.Win32
{
    public class Win32InputMethodContext : IInputMethodContext
    {
        private readonly Win32InputMethod _inputMethod;
        private readonly WindowContext _windowContext;
        private readonly IInputMethodSource _source;

        internal Win32InputMethodContext(Win32InputMethod inputMethod, WindowContext windowContext, IInputMethodSource source)
        {
            _inputMethod = inputMethod;
            _windowContext = windowContext;
            _source = source;
        }

        #region Input Method Context

        public IInputMethodSource Source => _source;

        public WindowContext WindowContext => _windowContext;

        public void Focus()
        {
            _inputMethod.Focus(this);
        }

        public void Unfocus()
        {
            _inputMethod.Unfocus(this);
        }

        #endregion

    }
}
