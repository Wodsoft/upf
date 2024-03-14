using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Input
{
    /// <summary>
    ///     The MouseButton enumeration describes the buttons available on
    ///     the mouse device.
    /// </summary>
    /// <remarks>
    ///     You must update MouseButtonUtilities.Validate if any changes are made to this type
    /// </remarks>
    public enum MouseButton
    {
        /// <summary>
        ///    The left mouse button.
        /// </summary>
        Left,

        /// <summary>
        ///    The middle mouse button.
        /// </summary>
        Middle,

        /// <summary>
        ///    The right mouse button.
        /// </summary>
        Right,

        /// <summary>
        ///    The fourth mouse button.
        /// </summary>
        XButton1,

        /// <summary>
        ///    The fifth mouse button.
        /// </summary>
        XButton2
    }
}
