using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    /// <summary>
    /// This property determines how text is trimmed when it overflows the edge of its
    /// containing box.
    /// </summary>
    public enum TextTrimming
    {
        /// <summary>
        /// Default no trimming
        /// </summary>
        None,

        /// <summary>
        /// Text is trimmed at character boundary. Ellipsis is drawn in place of invisible part.
        /// </summary>
        CharacterEllipsis,

        /// <summary>
        /// Text is trimmed at word boundary. Ellipsis is drawn in place of invisible part.
        /// </summary>
        WordEllipsis,
    }
}
