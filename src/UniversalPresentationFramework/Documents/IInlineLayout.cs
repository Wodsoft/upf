using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Documents
{
    public interface IInlineLayout
    {
        bool IsFloat { get; }

        InlineLayoutMeasureResult Measure(TextPointer start, TextPointer end, float availableWidth, bool isFullLine, TextWrapping textWrapping, TextTrimming textTrimming);

        void Draw(DrawingContext drawingContext, in Rect origin, in Rect clip, in float lineHeight, ReadOnlySpan<float> widths, TextPointer start, TextPointer end);

        TextPointer GetCharacterAtPoint(in Point point, TextPointer start, TextPointer end);
    }

    public struct InlineLayoutMeasureResult
    {
        public Rect Rect;
        public float LineHeight;
        public float[] Widths;
        public TextRange? Overflow;
        public TextPointer Start;
        public TextPointer End;
    }
}
