using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.TextFormatting
{
    public class TextBounds
    {
        public TextBounds(TextRun textRun, ReadOnlyMemory<float> textWidths, float textRunWidth, GlyphTypeface glyphTypeface)
        {
            if (textRun == null)
                throw new ArgumentNullException(nameof(textRun));
            if (textRun.Length != textWidths.Length)
                throw new ArgumentException("Text run length not equal to text widths count.");
            TextRun = textRun;
            TextWidths = textWidths;
            TextRunWidth = textRunWidth;
            GlyphTypeface = glyphTypeface;
        }

        public TextRun TextRun { get; }

        public ReadOnlyMemory<float> TextWidths { get; }

        public float TextRunWidth { get; }

        public GlyphTypeface GlyphTypeface { get; }
    }
}
