using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    public interface ITypefaceMetrics
    {
        /// <summary>
        /// (Western) x-height relative to em size.
        /// </summary>
        float XHeight { get; }


        /// <summary>
        /// Distance from baseline to top of English capital, relative to em size.
        /// </summary>
        float CapsHeight { get; }


        /// <summary>
        /// Distance from baseline to underline position
        /// </summary>
        float UnderlinePosition { get; }


        /// <summary>
        /// Underline thickness
        /// </summary>
        float UnderlineThickness { get; }


        /// <summary>
        /// Distance from baseline to strike-through position
        /// </summary>
        float StrikethroughPosition { get; }


        /// <summary>
        /// strike-through thickness
        /// </summary>
        float StrikethroughThickness { get; }


        float Descent { get; }

        float Ascent { get; }
    }
}
