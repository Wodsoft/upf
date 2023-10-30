using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public enum WindowStartupLocation
    {
        /// <summary>
        /// Uses the values specified by Left and Top properties to position the Window
        /// </summary>
        Manual = 0,

        /// <summary>
        /// Centers the Window on the screen.  If there are more than one monitors, then
        /// the Window is centered on the monitor that has the mouse on it
        /// </summary>
        CenterScreen = 1,

        /// <summary>
        /// Centers the Window on its owner.  If there is no owner window defined or if
        /// it is not possible to center it on the owner, then defaults to Manual
        /// </summary>
        CenterOwner = 2
    }
}
