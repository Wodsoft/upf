using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Imaging
{
    /// <summary>
    ///     Rotation - The rotation to be applied; only multiples of 90 degrees is supported.
    /// </summary>
    public enum Rotation
    {
        /// <summary>
        ///     Rotate0 - Do not rotate
        /// </summary>
        Rotate0 = 0,

        /// <summary>
        ///     Rotate90 - Rotate 90 degress
        /// </summary>
        Rotate90 = 1,

        /// <summary>
        ///     Rotate180 - Rotate 180 degrees
        /// </summary>
        Rotate180 = 2,

        /// <summary>
        ///     Rotate270 - Rotate 270 degrees
        /// </summary>
        Rotate270 = 3,
    }
}
