using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Controls
{
    /// <summary>
    /// StretchDirection - Enum which describes when scaling should be used on the content of a Viewbox. This
    /// enum restricts the scaling factors along various axes.
    /// </summary>
    /// <seealso cref="Viewbox" />
    public enum StretchDirection
    {
        /// <summary>
        /// Only scales the content upwards when the content is smaller than the Viewbox.
        /// If the content is larger, no scaling downwards is done.
        /// </summary>
        UpOnly,

        /// <summary>
        /// Only scales the content downwards when the content is larger than the Viewbox.
        /// If the content is smaller, no scaling upwards is done.
        /// </summary>
        DownOnly,

        /// <summary>
        /// Always stretches to fit the Viewbox according to the stretch mode.
        /// </summary>
        Both
    }
}
