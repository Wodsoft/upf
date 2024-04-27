using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.TextFormatting
{
    public class GenericTextLine : TextLine
    {
        private readonly IReadOnlyList<TextBounds> _textBounds;
        private readonly bool _hasBreakText, _hasCollapsed;
        private readonly TextParagraphProperties _paragraphProperties;
        private readonly GlyphTypeface _defaultGlyphTypeface;
        private readonly float _width, _height, _textHeight, _emSize;
        private TextRunCollection? _textRuns;

        public GenericTextLine(IReadOnlyList<TextBounds> textBounds, float width, bool hasBreakText, bool hasCollapsed, TextParagraphProperties paragraphProperties, GlyphTypeface defaultGlyphTypeface)
        {
            _textBounds = textBounds;
            _width = width;
            _hasBreakText = hasBreakText;
            _hasCollapsed = hasCollapsed;
            _paragraphProperties = paragraphProperties;
            _defaultGlyphTypeface = defaultGlyphTypeface;
            _emSize = paragraphProperties.DefaultTextRunProperties.FontRenderingEmSize;
            if (_paragraphProperties.LineHeight > 0)
                _height = _paragraphProperties.LineHeight;
            else
                _height = _defaultGlyphTypeface.Height * _emSize;
        }

        public override bool HasOverflowed => _hasBreakText;

        public override bool HasCollapsed => _hasCollapsed;

        public override int Length
        {
            get
            {
                var firstRun = _textBounds[0].TextRun;
                var lastRun = _textBounds[_textBounds.Count - 1].TextRun;
                return lastRun.Start + lastRun.Length - firstRun.Start;
            }
        }

        public override int TrailingWhitespaceLength => 0;

        public override float Start => 0;

        public override float Width => _width;

        public override float WidthIncludingTrailingWhitespace => _width;

        public override float Height => _height;

        public override float TextHeight => (_defaultGlyphTypeface.Ascent + _defaultGlyphTypeface.Descent) * _emSize;

        public override float Extent => 0f;

        public override float Baseline => _defaultGlyphTypeface.Baseline * _emSize;

        public override float TextBaseline => _defaultGlyphTypeface.Ascent * _emSize;

        public override float MarkerBaseline => Baseline;

        public override float MarkerHeight => Height;

        public override float OverhangLeading => 0f;

        public override float OverhangTrailing => 0f;

        public override float OverhangAfter => 0f;

        public override TextLine Collapse(TextTrimming trimming, float maxLineLength)
        {
            Collapse(trimming, maxLineLength, out var boundIndex, out var boundLength, out var boundWidth);
            if (boundIndex == -1)
                return Empty;
            float leftWidth = 0f;
            var leftBounds = new TextBounds[boundIndex + 1];
            var splitBound = _textBounds[boundIndex];
            for (int i = 0; i < boundIndex; i++)
            {
                var bound = _textBounds[i];
                leftBounds[i] = bound;
                leftWidth += bound.TextRunWidth;
            }
            if (splitBound.TextRun.Length == boundLength)
            {
                leftBounds[boundIndex] = splitBound;
                leftWidth += splitBound.TextRunWidth;
            }
            else
            {
                leftBounds[boundIndex] = new TextBounds(splitBound.TextRun.Slice(0, boundLength), splitBound.TextWidths.Slice(0, boundLength), boundWidth, splitBound.GlyphTypeface);
                leftWidth += boundWidth;
            }
            return new GenericTextLine(leftBounds, leftWidth, false, true, _paragraphProperties, _defaultGlyphTypeface);
        }

        public override TextLine Collapse(TextTrimming trimming, float maxLineLength, out TextLine? collapsedLine)
        {
            Collapse(trimming, maxLineLength, out var boundIndex, out var boundLength, out var boundWidth);
            if (boundIndex == -1)
            {
                collapsedLine = this;
                return Empty;
            }
            float leftWidth = 0f, rightWidth = 0f;
            var leftBounds = new TextBounds[boundIndex + 1];
            var splitBound = _textBounds[boundIndex];
            for (int i = 0; i < boundIndex; i++)
            {
                var bound = _textBounds[i];
                leftBounds[i] = bound;
                leftWidth += bound.TextRunWidth;
            }
            TextBounds[] rightBounds;
            if (splitBound.TextRun.Length == boundLength)
            {
                leftBounds[boundIndex] = splitBound;
                leftWidth += splitBound.TextRunWidth;
                rightBounds = new TextBounds[_textBounds.Count - boundIndex - 1];
                for (int i = boundIndex + 1; i < _textBounds.Count; i++)
                {
                    var bound = _textBounds[i];
                    rightBounds[i - boundIndex - 1] = bound;
                    rightWidth += bound.TextRunWidth;
                }
            }
            else
            {
                leftBounds[boundIndex] = new TextBounds(splitBound.TextRun.Slice(0, boundLength), splitBound.TextWidths.Slice(0, boundLength), boundWidth, splitBound.GlyphTypeface);
                leftWidth += boundWidth;
                rightBounds = new TextBounds[_textBounds.Count - boundIndex];
                var rightRun = splitBound.TextRun.Slice(boundLength);
                var trimmedText = rightRun.Characters.TrimStart();
                float rightRunWidth = splitBound.TextRunWidth - boundWidth;
                if (trimmedText.Length != rightRun.Length)
                {
                    var trimmedLength = rightRun.Length - trimmedText.Length;
                    rightRun = rightRun.Slice(trimmedLength);
                    for (int i = 0; i < trimmedLength; i++)
                        rightRunWidth -= splitBound.TextWidths.Span[boundLength + i];
                }
                rightBounds[0] = new TextBounds(rightRun, splitBound.TextWidths.Slice(splitBound.TextWidths.Length-rightRun.Length), rightRunWidth, splitBound.GlyphTypeface);
                rightWidth += rightBounds[0].TextRunWidth;
                for (int i = boundIndex + 1; i < _textBounds.Count; i++)
                {
                    var bound = _textBounds[i];
                    rightBounds[i - boundIndex] = bound;
                    rightWidth += bound.TextRunWidth;
                }
            }
            collapsedLine = new GenericTextLine(rightBounds, rightWidth, rightWidth > maxLineLength, false, _paragraphProperties, _defaultGlyphTypeface);
            return new GenericTextLine(leftBounds, leftWidth, false, true, _paragraphProperties, _defaultGlyphTypeface);
        }

        private void Collapse(TextTrimming trimming, float maxLineLength, out int boundIndex, out int boundLength, out float boundWidth)
        {
            if (!_hasBreakText)
            {
                boundIndex = _textBounds.Count - 1;
                boundLength = 0;
                boundWidth = 0f;
                return;
            }
            var currentBound = _textBounds.Count;
            float currentWidth = _width;
            while (currentWidth > maxLineLength)
            {
                currentBound--;
                var bound = _textBounds[currentBound];
                currentWidth -= bound.TextRunWidth;
                if (currentWidth > maxLineLength)
                    continue;

                var maxWidth = maxLineLength - currentWidth;
                if (trimming == TextTrimming.CharacterEllipsis)
                {
                    int length = 0;
                    float widths = 0;
                    while (true)
                    {
                        var width = bound.TextWidths.Span[length];
                        if (widths + width > maxWidth)
                            break;
                        widths += width;
                        length++;
                    }
                    if (length == 0)
                    {
                        boundIndex = currentBound - 1;
                        boundLength = 0;
                        boundWidth = 0f;
                    }
                    else
                    {
                        boundIndex = currentBound;
                        boundLength = length;
                        boundWidth = widths;
                    }
                    return;
                }
                else
                {
                    var length = bound.GlyphTypeface.BreakText(bound.TextRun.Characters, bound.TextRun.Properties.FontRenderingEmSize, maxWidth, out boundWidth);
                    if (length == 0)
                    {
                        boundIndex = currentBound - 1;
                        boundLength = 0;
                    }
                    else
                    {
                        boundIndex = currentBound;
                        boundLength = length;
                    }
                    return;
                }
            }
            boundIndex = -1;
            boundLength = 0;
            boundWidth = 0f;
        }

        public override void Dispose()
        {

        }

        public override void Draw(DrawingContext drawingContext, Point origin, InvertAxes inversion)
        {
            float offset = 0f;
            for (int i = 0; i < _textBounds.Count; i++)
            {
                var bound = _textBounds[i];
                if (bound.TextRun.Properties.ForegroundBrush != null)
                    drawingContext.DrawText(bound.TextRun.Characters, bound.GlyphTypeface, bound.TextRun.Properties.FontRenderingEmSize, bound.TextRun.Properties.ForegroundBrush, new Point(origin.X + offset, origin.Y));
            }
        }

        public override IReadOnlyList<TextRun> GetTextRunSpans()
        {
            if (_textRuns == null)
                _textRuns = new TextRunCollection(_textBounds);
            return _textRuns;
        }

        private class TextRunCollection : IReadOnlyList<TextRun>
        {
            private readonly IReadOnlyList<TextBounds> _textBounds;

            public TextRunCollection(IReadOnlyList<TextBounds> textBounds)
            {
                _textBounds = textBounds;
            }

            public TextRun this[int index] => _textBounds[index].TextRun;

            public int Count => _textBounds.Count;

            public IEnumerator<TextRun> GetEnumerator()
            {
                return new Enumerator(_textBounds);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            private class Enumerator : IEnumerator<TextRun>
            {
                private readonly IReadOnlyList<TextBounds> _textBounds;
                private int _index;

                public Enumerator(IReadOnlyList<TextBounds> textBounds)
                {
                    _textBounds = textBounds;
                }

                public TextRun Current => _textBounds[_index].TextRun;

                object IEnumerator.Current => Current;

                public void Dispose()
                {

                }

                public bool MoveNext()
                {
                    _index++;
                    if (_index >= _textBounds.Count)
                        return false;
                    return true;
                }

                public void Reset()
                {
                    _index = -1;
                }
            }
        }
    }
}
