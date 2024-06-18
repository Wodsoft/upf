using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Input
{
    /// <summary>
    /// State of Ime
    /// </summary>
    public enum InputMethodState
    {
        /// <summary>
        /// InputMethod state is on.
        /// </summary>
        Off = 0,

        /// <summary>
        /// InputMethod state is on.
        /// </summary>
        On = 1,

        /// <summary>
        /// InputMethod state is not set. It does not care.
        /// </summary>
        DoNotCare = 2,
    }
}
