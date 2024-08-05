using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    /// <summary>
    ///     TileMode - Enum which descibes the drawing of the ends of a line.
    /// </summary>
    public enum TileMode
    {
        /// <summary>
        ///     None - Do not tile only the base tile is drawn, the remaining area is left as 
        ///     transparent
        /// </summary>
        None = 0,

        /// <summary>
        ///     Tile - The basic tile mode  the base tile is drawn and the remaining area is filled 
        ///     by repeating the base tile such that the right edge of one tile is adjacent to the left edge 
        ///     of the next, and similarly for bottom and top
        /// </summary>
        Tile = 4,

        /// <summary>
        ///     FlipX - The same as tile, but alternate columns of tiles are flipped horizontally.  
        ///     The base tile is drawn untransformed.
        /// </summary>
        FlipX = 1,

        /// <summary>
        ///     FlipY - The same as tile, but alternate rows of tiles are flipped vertically.  The 
        ///     base tile is drawn untransformed.
        /// </summary>
        FlipY = 2,

        /// <summary>
        ///     FlipXY - The combination of FlipX and FlipY.  The base tile is drawn untransformed.
        /// </summary>
        FlipXY = 3,
    }
}
