using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Controls.Primitives;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Controls
{
    public class TextBoxHostLine : ITextHostLine, ITextHostRun
    {
        private readonly TextBox _textBox;
        private readonly int _start, _length;
        private float _width, _height, _baseline;
        private ReadOnlyMemory<float> _widths;
        private bool _isMeasured;
        private GlyphTypeface? _typeface;

        public TextBoxHostLine(TextBox textBox, int start, int length)
        {
            _textBox = textBox;
            _start = start;
            _length = length;
        }

        public IReadOnlyList<ITextHostRun> Runs => [this];

        public float LineHeight => float.NaN;

        public float Baseline => _baseline;

        public bool IsMeasured => _isMeasured;

        public float Width => _width;

        public float Height => _height;

        public int Position => _start;

        public int Length => _length;

        public void Draw(DrawingContext drawingContext, in Point origin)
        {
            if (!_isMeasured || _typeface == null)
                return;
            var foreground = _textBox.Foreground;
            var selectionLength = _textBox.SelectionLength;
            var selectionStart = _textBox.SelectionStart;
            var text = _textBox.GetText().Slice(_start, _length);
            if (selectionLength != 0 &&
                ((selectionStart >= _start && selectionStart < _start + _length) ||
                (selectionStart + selectionLength > _start && selectionStart + selectionLength <= _start + _length)))
            {
                var selectionOffset = _typeface.Ascent * _textBox.FontSize;
                var selectionBrush = _textBox.SelectionBrush;
                var selectionTextBrush = _textBox.SelectionTextBrush;
                if (selectionStart <= _start && selectionStart + selectionLength >= _start + _length)
                {
                    if (selectionBrush != null)
                        drawingContext.DrawRectangle(selectionBrush, null, new Rect(origin.X, origin.Y - selectionOffset, _width, _height));
                    if (selectionTextBrush != null)
                        drawingContext.DrawText(text, _typeface, _textBox.FontSize, selectionTextBrush, origin);
                }
                else
                {
                    int offsetChar = 0;
                    int offsetLength = selectionLength;
                    if (selectionStart > _start)
                    {
                        var leftText = text[..(selectionStart - _start)];
                        if (foreground != null)
                            drawingContext.DrawText(leftText, _typeface, _textBox.FontSize, foreground, origin);
                        offsetChar = selectionStart - _start;
                        offsetLength = Math.Min(offsetChar + selectionLength, _length - leftText.Length);
                    }
                    if (selectionStart + selectionLength < _start + _length)
                    {
                        var rightText = text[^(_start + _length - selectionStart - selectionLength)..];
                        var rightTextOffset = _width - _widths.Span[^rightText.Length..].Sum();
                        if (foreground != null)
                            drawingContext.DrawText(rightText, _typeface, _textBox.FontSize, foreground, new Point(origin.X + rightTextOffset, origin.Y));
                        offsetChar = Math.Max(_start, selectionStart) - _start;
                        offsetLength = selectionStart + selectionLength - _start - offsetChar;
                    }

                    float offsetWidth = _widths.Span[offsetChar..(offsetLength + offsetChar)].Sum();
                    float offsetX = _widths.Span[..offsetChar].Sum();
                    if (selectionBrush != null)
                        drawingContext.DrawRectangle(selectionBrush, null, new Rect(origin.X + offsetX, origin.Y - selectionOffset, offsetWidth, _height));
                    if (selectionTextBrush != null)
                        drawingContext.DrawText(text.Slice(offsetChar, offsetLength), _typeface, _textBox.FontSize, selectionTextBrush, new Point(origin.X + offsetX, origin.Y));
                }
            }
            else
            {
                if (foreground != null)
                    drawingContext.DrawText(text, _typeface, _textBox.FontSize, foreground, origin);
            }
        }

        public int GetCharPosition(in float x)
        {
            if (!_isMeasured)
                return _start;
            var length = 0f;
            for (int i = 0; i < _length; i++)
            {
                var width = _widths.Span[i];
                if (length + width > x)
                {
                    if (x - length <= width / 2)
                        return _start + i;
                    else
                        return _start + i + 1;
                }
                length += width;
            }
            return _start + _length;
        }

        public void Measure()
        {
            _isMeasured = true;
            var font = _textBox.FontFamily;
            if (font != null)
            {
                _typeface = font.GetGlyphTypeface(_textBox.FontStyle, _textBox.FontWeight, _textBox.FontStretch);
                if (_typeface != null)
                {
                    var size = _textBox.FontSize;
                    _widths = _typeface.GetTextWidths(_textBox.GetText().Slice(_start, _length), size);
                    _width = _widths.Span.Sum();
                    _height = _typeface.Height * size;
                    _baseline = _typeface.Ascent * size;
                }
                return;
            }
            _width = 0;
            _height = 0;
            _baseline = 0;
        }

        public void Wrap(TextTrimming trimming, float width, bool overflow, out ITextHostRun? left, out ITextHostRun? right)
        {
            if (trimming == TextTrimming.None)
            {
                left = this;
                right = null;
                return;
            }
            if (_widths.Length == 0 || _widths.Span[0] > width)
            {
                left = null;
                right = this;
                return;
            }
            var currentWidth = _width;
            int i;
            for (i = _widths.Length - 1; i >= 0; i--)
            {
                currentWidth -= _widths.Span[i];
                if (currentWidth <= width)
                    break;
            }
            left = new TextBoxHostLine(_textBox, _start, i);
            right = new TextBoxHostLine(_textBox, _start + i, _length - i);
        }
    }
}
