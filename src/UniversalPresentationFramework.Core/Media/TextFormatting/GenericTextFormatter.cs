using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.TextFormatting
{
    public class GenericTextFormatter : TextFormatter
    {
        public override TextLine FormatLine(TextSource textSource, int firstCharIndex, float paragraphWidth, TextParagraphProperties paragraphProperties)
        {
            var defaultGlyphTypeface = paragraphProperties.DefaultTextRunProperties.Typeface.GetGlyphTypeface();
            if (defaultGlyphTypeface == null)
                return TextLine.Empty;
            List<TextBounds> bounds = new List<TextBounds>();
            var currentPosition = firstCharIndex;
            float currentWidth = 0f;
            bool hasBreakText = false;
            while (currentPosition < textSource.Length - 1)
            {
                var textRun = textSource.GetTextRun(currentPosition);
                var glyphTypeface = textRun.Properties.Typeface.GetGlyphTypeface();
                if (glyphTypeface == null)
                    continue;
                var nextLineBreak = textRun.GetNextLineBreakPosition();
                ReadOnlySpan<char> text = textRun.Characters;
                if (nextLineBreak != -1)
                    text = text.Slice(0, nextLineBreak);

                var textWidths = glyphTypeface.GetTextWidths(text, textRun.Properties.FontRenderingEmSize);
                float width = 0;
                if (Vector.IsHardwareAccelerated)
                {
                    var vectorSize = Vector<float>.Count;
                    int i = 0;
                    var widthSpan = textWidths.Span;
                    for (; i < textWidths.Length - vectorSize; i += vectorSize)
                    {
                        var vectors = MemoryMarshal.Cast<float, Vector<float>>(widthSpan.Slice(i, vectorSize));
                        width += Vector.Sum(vectors[0]);
                    }
                    for (; i < textWidths.Length; i++)
                        width += widthSpan[i];
                }
                else
                {
                    for (int i = 0; i < textWidths.Length; i++)
                        width += textWidths.Span[i];
                }
                currentWidth += width;                
                bounds.Add(new TextBounds(textRun, textWidths, width, glyphTypeface.Height * textRun.Properties.FontRenderingEmSize, glyphTypeface));
                if (textRun.IsEndOfNewLine)
                    break;
                if (currentWidth >= paragraphWidth)
                {
                    hasBreakText = true;
                    break;
                }
                currentPosition += textRun.Length;
            }
            if (bounds.Count == 0)
                return TextLine.Empty;
            return new GenericTextLine(bounds.AsReadOnly(), currentWidth, hasBreakText, false, paragraphProperties, defaultGlyphTypeface);
        }

        public static GenericTextFormatter Default { get; } = new GenericTextFormatter();
    }
}
