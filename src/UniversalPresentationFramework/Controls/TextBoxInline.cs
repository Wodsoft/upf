using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Controls.Primitives;
using Wodsoft.UI.Media;
using static System.Net.Mime.MediaTypeNames;

namespace Wodsoft.UI.Controls
{
    public partial class TextBox
    {
        private class TextBoxInline : ITextOwnerInline
        {
            private readonly TextBoxBlock _block;
            private readonly int _start, _length;
            private float _width, _height, _baseline;
            private ReadOnlyMemory<float> _widths;
            private bool _isMeasured, _isEndOfNewLine;
            private GlyphTypeface? _typeface;
            private TextBoxInline? _previousInline, _nextInline;

            public TextBoxInline(TextBoxBlock block, int start, int length, bool isEndOfNewLine)
            {
                _block = block;
                _start = start;
                _length = length;
                _isEndOfNewLine = isEndOfNewLine;
            }
            private TextBoxInline(TextBoxBlock block, int start, int length, bool isEndOfNewLine, ReadOnlyMemory<float> widths, float width, float height, float baseline)
                : this(block, start, length, isEndOfNewLine)
            {
                _widths = widths;
                _width = width;
                _height = height;
                _baseline = baseline;
                _isMeasured = true;
            }

            public bool IsMeasured => _isMeasured;

            public bool IsEndOfNewLine => _isEndOfNewLine;

            public float Width => _width;

            public float Height => _height;

            public float Baseline => _baseline;

            public int Position => _start;

            public int Length => _length;

            public ReadOnlySpan<float> Widths => _widths.Span;

            public ITextOwnerBlock Line => _block;

            public ITextOwnerInline? PreviousInline
            {
                get
                {
                    if (_previousInline == null)
                    {
                        if (_start == 0)
                            return null;
                        var text = _block.TextBox._text.AsSpan();
                        if (_block.Length != text.Length)
                            throw new InvalidDataException("Text has been changed.");
                        text = text.Slice(0, _start - 1);
                        var index = text.LastIndexOf('\n');
                        if (index == -1)
                            _previousInline = new TextBoxInline(_block, 0, text.Length, false);
                        else
                            _previousInline = new TextBoxInline(_block, index + 1, text.Length - index - 1, true);
                    }
                    return _previousInline;
                }
            }

            public ITextOwnerInline? NextInline
            {
                get
                {
                    if (_nextInline == null)
                    {
                        if (!_isEndOfNewLine)
                            return null;
                        var text = _block.TextBox._text.AsSpan();
                        if (_block.Length != text.Length)
                            throw new InvalidDataException("Text has been changed.");
                        text = text.Slice(_start + _length + 1);
                        var index = text.IndexOf('\n');
                        if (index == -1)
                            return new TextBoxInline(_block, _start + _length + 1, text.Length, false);
                        return new TextBoxInline(_block, _start + _length + 1, index, true);
                    }
                    return _nextInline;

                }
            }

            public void Draw(DrawingContext drawingContext, in Point origin)
            {
                if (!_isMeasured || _typeface == null)
                    return;
                if (_length == 0)
                    return;
                var textBox = _block.TextBox;
                var foreground = _block.TextBox.Foreground;
                var selectionLength = textBox.SelectionLength;
                var selectionStart = textBox.SelectionStart;
                var text = textBox._text.AsSpan().Slice(_start, _length);
                if (selectionLength != 0 &&
                    ((selectionStart <= _start && selectionStart + selectionLength >= _start + _length) ||
                    (selectionStart >= _start && selectionStart < _start + _length) ||
                    (selectionStart + selectionLength > _start && selectionStart + selectionLength <= _start + _length)))
                {
                    var selectionOffset = _typeface.Ascent * textBox.FontSize;
                    var selectionBrush = textBox.SelectionBrush;
                    var selectionTextBrush = textBox.SelectionTextBrush;
                    if (selectionStart <= _start && selectionStart + selectionLength >= _start + _length)
                    {
                        if (selectionBrush != null)
                            drawingContext.DrawRectangle(selectionBrush, null, new Rect(origin.X, origin.Y - selectionOffset, _width, _height));
                        if (selectionTextBrush != null)
                            drawingContext.DrawText(text, _typeface, textBox.FontSize, selectionTextBrush, origin);
                    }
                    else
                    {
                        int leftOffsetChar = 0, rightOffsetChar = 0;
                        if (selectionStart > _start)
                        {
                            leftOffsetChar = selectionStart - _start;
                            var leftText = text[..leftOffsetChar];
                            if (foreground != null)
                                drawingContext.DrawText(leftText, _typeface, textBox.FontSize, foreground, origin);
                        }
                        if (selectionStart + selectionLength < _start + _length)
                        {
                            rightOffsetChar = _start + _length - selectionStart - selectionLength;
                            var rightText = text[^rightOffsetChar..];
                            var rightTextOffset = _width - _widths.Span[^rightText.Length..].Sum();
                            if (foreground != null)
                                drawingContext.DrawText(rightText, _typeface, textBox.FontSize, foreground, new Point(origin.X + rightTextOffset, origin.Y));
                        }

                        float offsetWidth = _widths.Span[leftOffsetChar..^rightOffsetChar].Sum();
                        float offsetX = _widths.Span[..leftOffsetChar].Sum();
                        if (selectionBrush != null)
                            drawingContext.DrawRectangle(selectionBrush, null, new Rect(origin.X + offsetX, origin.Y - selectionOffset, offsetWidth, _height));
                        if (selectionTextBrush != null)
                            drawingContext.DrawText(text[leftOffsetChar..^rightOffsetChar], _typeface, textBox.FontSize, selectionTextBrush, new Point(origin.X + offsetX, origin.Y));
                    }
                }
                else
                {
                    if (foreground != null)
                        drawingContext.DrawText(text, _typeface, textBox.FontSize, foreground, origin);
                }
                var compositionLength = textBox.CompositionLength;
                if (compositionLength != 0 && foreground != null)
                {
                    selectionStart -= textBox.ComposttionStart;
                    selectionLength = compositionLength;
                    if ((selectionStart >= _start && selectionStart < _start + _length) ||
                        (selectionStart + selectionLength > _start && selectionStart + selectionLength <= _start + _length))
                    {
                        var pen = new Pen(foreground, 1, PenLineCap.Flat, PenLineCap.Flat, PenLineCap.Flat, PenLineJoin.Miter, 1f, DashStyles.DashDot);
                        if (selectionStart <= _start && selectionStart + selectionLength >= _start + _length)
                        {
                            drawingContext.DrawLine(pen, new Point(origin.X, origin.Y), new Point(origin.X + _width, origin.Y));
                        }
                        else
                        {
                            int left = 0, right = 0;
                            if (selectionStart > _start)
                                left = selectionStart - _start;
                            if (selectionStart + selectionLength < _start + _length)
                                right = _start + _length - selectionStart - selectionLength;
                            var offsetX = origin.X;
                            if (left != 0)
                                offsetX += _widths.Span[..left].Sum();
                            var width = _widths.Span[left..^right].Sum();
                            var offsetY = origin.Y + _typeface.Descent * textBox.FontSize;
                            drawingContext.DrawLine(pen, new Point(offsetX, offsetY), new Point(offsetX + width, offsetY));
                        }
                    }
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
                if (_isMeasured)
                    return;
                if (_block.Length != _block.TextBox._text.Length)
                    throw new InvalidDataException("Text has been changed.");
                _isMeasured = true;
                _typeface = _block.GetTypeface();
                if (_typeface != null)
                {
                    var textBox = _block.TextBox;
                    var size = textBox.FontSize;
                    if (_length == 0)
                    {
                        _widths = Array.Empty<float>();
                        _width = 0;
                    }
                    else
                    {
                        _widths = _typeface.GetTextWidths(textBox._text.AsSpan().Slice(_start, _length), size);
                        _width = _widths.Span.Sum();
                    }
                    _height = _typeface.Height * size;
                    _baseline = _typeface.Ascent * size;
                }
            }

            public void Wrap(TextTrimming trimming, float width, bool overflow, out ITextOwnerInline? left, out ITextOwnerInline? right)
            {
                if (!_isMeasured || trimming == TextTrimming.None)
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
                var newLeft = new TextBoxInline(_block, _start, i, false, _widths.Slice(0, i), currentWidth, _height, _baseline);
                var newRight = new TextBoxInline(_block, _start + i, _length - i, _isEndOfNewLine, _widths.Slice(i), _width - currentWidth, _height, _baseline);
                newLeft._previousInline = _previousInline;
                newLeft._nextInline = newRight;
                newRight._previousInline = newLeft;
                newRight._nextInline = _nextInline;
                left = newLeft;
                right = newRight;
            }
        }
    }
}
