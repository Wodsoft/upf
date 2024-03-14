using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    /// <summary>
    ///     TextDecorationUnit - The unit type of text decoration value
    /// </summary>
    public enum TextDecorationUnit
    {
        /// <summary>
        ///     FontRecommended - The unit is the calculated value by layout system
        /// </summary>
        FontRecommended = 0,

        /// <summary>
        ///     FontRenderingEmSize - The unit is the rendering Em size
        /// </summary>
        FontRenderingEmSize = 1,

        /// <summary>
        ///     Pixel - The unit is one pixel
        /// </summary>
        Pixel = 2,
    }
}
