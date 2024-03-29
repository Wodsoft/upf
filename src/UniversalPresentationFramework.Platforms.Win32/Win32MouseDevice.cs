using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32;
using Windows.Win32.Foundation;
using Wodsoft.UI.Input;

namespace Wodsoft.UI.Platforms.Win32
{
    public class Win32MouseDevice : MouseDevice
    {
        private Point _currentPoint;
        private IInputElement? _currentCaptured;

        internal Win32MouseDevice(HWND hwnd)
        {

        }

        public override Point GetPosition(IInputElement relativeTo)
        {
            throw new NotImplementedException();
        }

        protected override MouseButtonState GetButtonState(MouseButton mouseButton)
        {            
            throw new NotImplementedException();
        }
    }
}
