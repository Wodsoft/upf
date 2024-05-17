using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.TextFormatting
{
    public class EmptyTextLine : TextLine
    {
        public override bool HasOverflowed => false;

        public override int Length => 0;

        public override bool HasCollapsed => false;

        public override int TrailingWhitespaceLength => 0;

        public override float Start => 0f;

        public override float Width => 0f;

        public override float WidthIncludingTrailingWhitespace => 0f;

        public override float Height => 0f;

        public override float TextHeight => 0f;

        public override float Extent => 0f;

        public override float Baseline => 0f;

        public override float TextBaseline => 0f;

        public override float MarkerBaseline => 0f;

        public override float MarkerHeight => 0f;

        public override float OverhangLeading => 0f;

        public override float OverhangTrailing => 0f;

        public override float OverhangAfter => 0f;

        public override TextLine Collapse(TextTrimming trimming, float maxLineLength, bool overflow)
        {
            return this;
        }

        public override TextLine Collapse(TextTrimming trimming, float maxLineLength, bool overflow, out TextLine? collapsedLine)
        {
            collapsedLine = null;
            return this;
        }

        public override void Dispose()
        {

        }

        public override void Draw(DrawingContext drawingContext, Point origin, InvertAxes inversion)
        {

        }

        public override IReadOnlyList<TextRun> GetTextRunSpans()
        {
            return Array.Empty<TextRun>();
        }
    }
}
