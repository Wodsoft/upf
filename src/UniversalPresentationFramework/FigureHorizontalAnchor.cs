using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public enum FigureHorizontalAnchor
    {
        /// <summary>
        /// Anchor the figure to the left of the page area.
        /// </summary>
        PageLeft = 0,

        /// <summary>
        /// Anchor the figure to the center of the page area.
        /// </summary>
        PageCenter = 1,

        /// <summary>
        /// Anchor the figure to the right of the page area.
        /// </summary>
        PageRight = 2,

        /// <summary>
        /// Anchor the figure to the left of the page content area.
        /// </summary>
        ContentLeft = 3,

        /// <summary>
        /// Anchor the figure to the center of the page content area.
        /// </summary>
        ContentCenter = 4,

        /// <summary>
        /// Anchor the figure to the right of the page content area.
        /// </summary>
        ContentRight = 5,

        /// <summary>
        /// Anchor the figure to the left of the current column
        /// </summary>
        ColumnLeft = 6,

        /// <summary>
        /// Anchor the figure to the center of the current column
        /// </summary>
        ColumnCenter = 7,

        /// <summary>
        /// Anchor the figure to the right of the current column
        /// </summary>
        ColumnRight = 8,

        // Disabled
        // Anchor the figure to the left of the current character position.
        //CharacterLeft   = 9,
        // Anchor the figure to the center of the current character position.
        //CharacterCenter = 10,
        // Anchor the figure to the right of the current character position.
        //CharacterRight  = 11,
    }
}
