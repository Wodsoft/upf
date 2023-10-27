using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public enum Visibility : byte
    {
        /// <summary>
        /// Normally visible.
        /// </summary>
        Visible = 0,

        /// <summary>
        /// Occupies space in the layout, but is not visible (completely transparent).
        /// </summary>
        Hidden,

        /// <summary>
        /// Not visible and does not occupy any space in layout, as if it doesn't exist.
        /// </summary>
        Collapsed
    }
}
