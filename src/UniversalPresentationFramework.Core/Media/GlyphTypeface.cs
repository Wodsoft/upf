using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    public abstract class GlyphTypeface : ITypefaceMetrics
    {
        public abstract FontStyle Style { get; }

        public abstract FontStretch Stretch { get; }

        public abstract FontWeight Weight { get; }

        public abstract float Baseline { get; }

        public abstract float CapsHeight { get; }

        public abstract float Height { get; }

        public abstract float XHeight { get; }

        /// <summary>
        /// Distance from baseline to underline position
        /// </summary>
        public abstract float UnderlinePosition { get; }

        /// <summary>
        /// Underline thickness
        /// </summary>
        public abstract float UnderlineThickness { get; }

        /// <summary>
        /// Distance from baseline to strike-through position
        /// </summary>
        public abstract float StrikethroughPosition { get; }

        /// <summary>
        /// strike-through thickness
        /// </summary>
        public abstract float StrikethroughThickness { get; }

        public abstract float Descent { get; }

        public abstract float Ascent { get; }

        public abstract int BreakText(ReadOnlySpan<char> text, float emSize, float maxWidth, out float measuredWidth);

        public abstract float MeasureText(ReadOnlySpan<char> text, float emSize);

        public abstract ReadOnlyMemory<float> GetTextWidths(ReadOnlySpan<char> text, float emSize);
    }
}
