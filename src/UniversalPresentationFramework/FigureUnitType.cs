using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public enum FigureUnitType
    {
        /// <summary>
        /// The value indicates that content should be calculated without constraints. 
        /// </summary>
        Auto = 0,
        /// <summary>
        /// The value is expressed as a pixel.
        /// </summary>
        Pixel,
        /// <summary>
        /// The value is expressed as fraction of column width.
        /// </summary>
        Column,

        /// <summary>
        /// The value is expressed as a fraction of content width.
        /// </summary>
        Content,

        /// <summary>
        /// The value is expressed as a fraction of page width.
        /// </summary>
        Page,
    }
}
