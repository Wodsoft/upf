using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    public enum Stretch
    {
        /// <summary>
        ///     None - Preserve original size
        /// </summary>
        None = 0,

        /// <summary>
        ///     Fill - Aspect ratio is not preserved, source rect fills destination rect.
        /// </summary>
        Fill = 1,

        /// <summary>
        ///     Uniform - Aspect ratio is preserved, Source rect is uniformly scaled as large as 
        ///     possible such that both width and height fit within destination rect.  This will 
        ///     not cause source clipping, but it may result in unfilled areas of the destination 
        ///     rect, if the aspect ratio of source and destination are different.
        /// </summary>
        Uniform = 2,

        /// <summary>
        ///     UniformToFill - Aspect ratio is preserved, Source rect is uniformly scaled as small 
        ///     as possible such that the entire destination rect is filled.  This can cause source 
        ///     clipping, if the aspect ratio of source and destination are different.
        /// </summary>
        UniformToFill = 3,
    }
}
