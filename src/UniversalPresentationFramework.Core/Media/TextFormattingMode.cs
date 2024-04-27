using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    public enum TextFormattingMode
    {
        /// <summary>
        /// Text is measured using glyph ideal metrics whose values are independent to the current display resolution.
        /// </summary>
        Ideal = 0,

        /// <summary>
        /// Text is measured using glyph display compatible metrics whose values tuned for the current display resolution.
        /// </summary>
        Display = 1
    }
}
