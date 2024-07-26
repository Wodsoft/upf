using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public enum FigureVerticalAnchor
    {
        /// <summary>
        /// Anchor the figure to the top of the page area.
        /// </summary>
        PageTop = 0,

        /// <summary>
        /// Anchor the figure to the center of the page area.
        /// </summary>
        PageCenter = 1,

        /// <summary>
        /// Anchor the figure to the bottom of the page area.
        /// </summary>
        PageBottom = 2,

        /// <summary>
        /// Anchor the figure to the top of the page content area.
        /// </summary>
        ContentTop = 3,

        /// <summary>
        /// Anchor the figure to the center of the page content area.
        /// </summary>
        ContentCenter = 4,

        /// <summary>
        /// Anchor the figure to the bottom of the page content area.
        /// </summary>
        ContentBottom = 5,


        /// <summary>
        /// Anchor the figure to the top of the current paragraph.
        /// </summary>
        ParagraphTop = 6,

        //ParagraphCenter = 7,      Not supported by PTS
        //ParagraphBottom = 8,      Not supported by PTS

        // Disabled
        // Anchor the figure to the top of the current character position.
        //CharacterTop    = 9,
        // Anchor the figure to the center of the current character position.
        //CharacterCenter = 10,
        // Anchor the figure to the bottom of the current character position.
        //CharacterBottom = 11,
    }
}
