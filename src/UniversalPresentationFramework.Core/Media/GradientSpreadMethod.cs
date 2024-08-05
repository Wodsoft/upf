using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    /// <summary>
    ///     GradientSpreadMethod - This determines how a gradient fills the space outside its 
    ///     primary area.
    /// </summary>
    public enum GradientSpreadMethod
    {
        /// <summary>
        ///     Pad - Pad - The final color in the gradient is used to fill the remaining area.
        /// </summary>
        Pad = 0,

        /// <summary>
        ///     Reflect - Reflect - The gradient is mirrored and repeated, then mirrored again, 
        ///     etc.
        /// </summary>
        Reflect = 1,

        /// <summary>
        ///     Repeat - Repeat - The gradient is drawn again and again.
        /// </summary>
        Repeat = 2,
    }
}
