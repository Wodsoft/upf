using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Input
{
    /// <summary>
    ///     Provides the base class for all input devices.
    /// </summary>
    public abstract class InputDevice
    {
        /// <summary>
        ///     Constructs an instance of the InputDevice class.
        /// </summary>
        protected InputDevice()
        {
            // Only we can create these.
            // But perhaps HID devices can create these too? 
        }

        ///// <summary>
        /////     Returns the element that input from this device is sent to.
        ///// </summary>
        //public abstract IInputElement Target { get; }

        ///// <summary>
        /////     Returns the PresentationSource that is reporting input for this device.
        ///// </summary>
        //public abstract PresentationSource ActiveSource { get; }
    }
}
