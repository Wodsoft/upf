using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using Wodsoft.UI.Controls;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Documents
{
    [ContentProperty("Text")]
    public class Run : Inline, IDocumentText
    {
        private string _text = string.Empty;
        private GlyphTypeface? _typeface;
        private bool _isMeasured;
        private ReadOnlyMemory<float>? _widths;
        private float _width, _height, _lineHeight;

        #region Constructor

        public Run() { }

        public Run(string text)
        {
            Text = text;
        }

        #endregion

        #region Properties

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(Run),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnTextPropertyChanged), new CoerceValueCallback(CoerceText)));
        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Run)d)._text = (string)e.NewValue!;
        }
        private static object CoerceText(DependencyObject d, object? value)
        {
            if (value == null)
                value = string.Empty;
            return value;
        }
        public string Text
        {
            get { return (string)GetValue(TextProperty)!; }
            set { SetValue(TextProperty, value); }
        }

        protected internal override int ContentCount => _text.Length;

        #endregion

        #region Methods

        private void Measure()
        {
            if (_isMeasured)
                return;
            _isMeasured = true;
            if (_typeface == null)
                _typeface = FontFamily.GetGlyphTypeface(FontStyle, FontWeight, FontStretch);
            if (_typeface == null)
                return;
            var fontSize = FontSize;
            var widths = _typeface.GetTextWidths(_text, fontSize);
            _widths = widths;
            _width = widths.Span.Sum();
            _height = (_typeface.Ascent + _typeface.Descent) * fontSize;
            _lineHeight = _typeface.Ascent * fontSize;
        }

        #endregion

        #region Layout

        private RunLayout? _layout;
        public override IInlineLayout Layout => _layout ??= new RunLayout(this);

        private class RunLayout : IInlineLayout
        {
            private readonly Run _run;

            public RunLayout(Run run)
            {
                _run = run;
            }

            public bool IsFloat => false;

            public void Draw(DrawingContext drawingContext, in Rect origin, in Rect clip, in float lineHeight, ReadOnlySpan<float> widths, TextPointer start, TextPointer end)
            {
                var run = _run;
                var runWidths = run._widths;
                if (runWidths == null)
                    return;
                var foreground = run.Foreground;
                var startIndex = run.ElementStart.GetOffsetToPosition(start) - 1;
                var text = run.Text.AsSpan();
                if (text.Length == 0)
                    return;
                if (startIndex < 0 || startIndex >= text.Length)
                    return;
                var length = start.GetOffsetToPosition(end);
                if (start == run.ElementStart)
                {
                    length -= 1;
                    start = run.ContentStart;
                }
                if (end == run.ElementEnd)
                {
                    length -= 1;
                    end = run.ContentEnd;
                }
                if (length == 0)
                    return;
                if (startIndex + length > text.Length)
                    return;
                var brush = run.Background;
                if (brush != null)
                {
                    drawingContext.DrawRectangle(brush, null, origin);
                }
                var container = start.TextContainer;

                var selectionStart = container.SelectionStart;
                var selectionEnd = container.SelectionEnd;

                text = text.Slice(startIndex, length);
                //var widths = runWidths.Value.Span.Slice(startIndex, length);
                var point = origin.TopLeft;
                point.Y += lineHeight;
                if (point.X < clip.X)
                {
                    int i = 0;
                    for (; i < widths.Length; i++)
                    {
                        if (point.X + widths[i] >= clip.X)
                            break;
                        point.X += widths[i];
                    }
                    text = text.Slice(i);
                    widths = widths.Slice(i);
                    start = start.GetPositionAtOffset(i)!;
                }
                var width = widths.Sum();
                if (width > clip.Width)
                {
                    int i = widths.Length - 1;
                    for (; i >= 0; i--)
                    {
                        if (width - widths[i] < clip.Width)
                            break;
                        width -= widths[i];
                    }
                    text = text.Slice(0, i + 1);
                    widths = widths.Slice(0, i + 1);
                    end = start.GetPositionAtOffset(i + 1)!;
                }
                var typeface = run._typeface!;
                var fontSize = run.FontSize;
                var decorations = run.TextDecorations;
                float[]? decorationOffsets;
                if (decorations != null)
                {
                    decorationOffsets = new float[decorations.Count];
                    for (int i = 0; i < decorations.Count; i++)
                    {
                        var decoration = decorations[i];
                        float decorationOffset;
                        switch (decoration.Location)
                        {
                            case TextDecorationLocation.Underline:
                                decorationOffset = typeface.Descent * fontSize;
                                break;
                            case TextDecorationLocation.Strikethrough:
                                decorationOffset = -(typeface.Ascent * fontSize / 2f);
                                break;
                            case TextDecorationLocation.OverLine:
                                decorationOffset = -typeface.Ascent * fontSize;
                                break;
                            default:
                                decorationOffset = 0f;
                                break;
                        }
                        switch (decoration.PenOffsetUnit)
                        {
                            case TextDecorationUnit.Pixel:
                                decorationOffset += decoration.PenOffset;
                                break;
                            case TextDecorationUnit.FontRenderingEmSize:
                                decorationOffset += decoration.PenOffset * fontSize;
                                break;
                        }
                        decorationOffsets[i] = decorationOffset;
                    }
                }
                else
                    decorationOffsets = null;
                if (selectionStart != selectionEnd &&
                    ((selectionStart <= start && selectionEnd >= end) ||
                    (selectionStart >= start && selectionStart < end) ||
                    (selectionEnd > start && selectionEnd <= end)))
                {
                    var selectionOffset = typeface.Ascent * fontSize;
                    var selectionBrush = container.SelectionBrush;
                    var selectionTextBrush = container.SelectionTextBrush;
                    if (selectionStart <= start && selectionEnd >= end)
                    {
                        if (selectionBrush != null)
                            drawingContext.DrawRectangle(selectionBrush, null, new Rect(point.X, point.Y - selectionOffset, width, origin.Height));
                        if (selectionTextBrush != null)
                        {
                            drawingContext.DrawText(text, typeface, fontSize, selectionTextBrush, point);
                            if (decorations != null)
                            {
                                for (int i = 0; i < decorations.Count; i++)
                                {
                                    var decoration = decorations[i];
                                    drawingContext.DrawLine(decoration.Pen ?? new Pen(selectionTextBrush, 1f), new Point(point.X, point.Y + decorationOffsets![i]), new Point(point.X + width, point.Y + decorationOffsets![i]));
                                }
                            }
                        }
                    }
                    else
                    {
                        int leftOffsetChar = 0, rightOffsetChar = 0;
                        if (selectionStart > start)
                        {
                            leftOffsetChar = start.GetOffsetToPosition(selectionStart);
                            var leftText = text[..leftOffsetChar];
                            if (foreground != null)
                            {
                                drawingContext.DrawText(leftText, typeface, fontSize, foreground, point);
                                if (decorations != null)
                                {
                                    for (int i = 0; i < decorations.Count; i++)
                                    {
                                        var decoration = decorations[i];
                                        drawingContext.DrawLine(decoration.Pen ?? new Pen(foreground, 1f), new Point(point.X, point.Y + decorationOffsets![i]), new Point(point.X + leftOffsetChar, point.Y + decorationOffsets![i]));
                                    }
                                }
                            }
                        }
                        if (selectionEnd < end)
                        {
                            rightOffsetChar = selectionEnd.GetOffsetToPosition(end);
                            var rightText = text[^rightOffsetChar..];
                            var rightTextOffset = width - widths[^rightText.Length..].Sum();
                            if (foreground != null)
                            {
                                drawingContext.DrawText(rightText, typeface, fontSize, foreground, new Point(point.X + rightTextOffset, point.Y));
                                if (decorations != null)
                                {
                                    for (int i = 0; i < decorations.Count; i++)
                                    {
                                        var decoration = decorations[i];
                                        drawingContext.DrawLine(decoration.Pen ?? new Pen(foreground, 1f), new Point(point.X + rightTextOffset , point.Y + decorationOffsets![i]), new Point(point.X + width , point.Y + decorationOffsets![i]));
                                    }
                                }
                            }
                        }
                        float offsetWidth = widths[leftOffsetChar..^rightOffsetChar].Sum();
                        float offsetX = widths[..leftOffsetChar].Sum();
                        if (selectionBrush != null)
                            drawingContext.DrawRectangle(selectionBrush, null, new Rect(point.X + offsetX, point.Y - selectionOffset, offsetWidth, origin.Height));
                        if (selectionTextBrush != null)
                        {
                            drawingContext.DrawText(text[leftOffsetChar..^rightOffsetChar], typeface, fontSize, selectionTextBrush, new Point(point.X + offsetX, point.Y));
                            if (decorations != null)
                            {
                                for (int i = 0; i < decorations.Count; i++)
                                {
                                    var decoration = decorations[i];
                                    drawingContext.DrawLine(decoration.Pen ?? new Pen(selectionTextBrush, 1f), new Point(point.X + offsetX, point.Y + decorationOffsets![i]), new Point(point.X + offsetX + offsetWidth, point.Y + decorationOffsets![i]));
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (foreground != null)
                    {
                        drawingContext.DrawText(text, run._typeface!, run.FontSize, foreground, point);
                        if (decorations != null)
                        {
                            for (int i = 0; i < decorations.Count; i++)
                            {
                                var decoration = decorations[i];
                                drawingContext.DrawLine(decoration.Pen ?? new Pen(foreground, 1f), new Point(point.X, point.Y + decorationOffsets![i]), new Point(point.X + width, point.Y + decorationOffsets![i]));
                            }
                        }
                    }
                }
            }

            public TextPointer GetCharacterAtPoint(in Point point, TextPointer start, TextPointer end)
            {
                var run = _run;
                if (start == run.ElementStart)
                    start = run.ContentStart;
                if (end == run.ElementEnd)
                    end = run.ContentEnd;
                var widths = _run._widths;
                if (widths == null)
                    throw new InvalidOperationException("Run doesn't measure yet.");
                var startIndex = run.ContentStart.GetOffsetToPosition(start);
                var length = start.GetOffsetToPosition(end);
                var span = widths.Value.Span.Slice(startIndex, length);
                int offset = 0;
                float calcLength = 0f;
                for (int j = 0; j < span.Length; j++)
                {
                    var width = span[j];
                    if (width == 0)
                        continue;
                    if (calcLength + width > point.X)
                    {
                        if (point.X - calcLength <= width / 2)
                            offset = j;
                        else
                            offset = j + 1;
                        break;
                    }
                    calcLength += width;
                }
                if (offset == 0)
                    return start;
                return start.GetPositionAtOffset(offset, LogicalDirection.Forward)!;
            }

            public InlineLayoutMeasureResult Measure(TextPointer start, TextPointer end, float availableWidth, bool isFullLine, TextWrapping textWrapping, TextTrimming textTrimming)
            {
                var run = _run;
                var result = new InlineLayoutMeasureResult();
                if (run._text == string.Empty)
                {
                    result.Widths = Array.Empty<float>();
                    result.Rect = Rect.Empty;
                    return result;
                }
                run.Measure();
                if (run._widths == null)
                {
                    result.Widths = Array.Empty<float>();
                    result.Rect = Rect.Empty;
                    return result;
                }
                var index = run.ElementStart.GetOffsetToPosition(start);
                if (start != run.ElementStart)
                    index -= 1;
                var length = start.GetOffsetToPosition(end);
                if (start == run.ElementStart)
                    length -= 1;
                if (end == run.ElementEnd)
                    length -= 1;
                //end with new line
                if (length == 0)
                {
                    result.Widths = Array.Empty<float>();
                    result.Rect = new Rect(0, 0, 0, run._height);
                    return result;
                }
                var text = run._text.AsSpan().Slice(index, length);
                result.Start = start;
                var newLine = text.IndexOf('\n');
                if (newLine != -1)
                    length = newLine;
                result.LineHeight = run._lineHeight;
                var measuredWidths = run._widths.Value.Span.Slice(index, length);
                var currentWidth = measuredWidths.Sum();
                if (currentWidth <= availableWidth || textWrapping == TextWrapping.NoWrap || textTrimming == TextTrimming.None)
                {
                    if (newLine != -1)
                    {
                        result.End = start.GetPositionAtOffset(newLine, LogicalDirection.Forward)!;
                        result.Overflow = new TextRange(result.End, end);
                    }
                    else
                        result.End = end;
                    if (start == run.ElementStart && result.End == run.ElementEnd)
                        result.Widths = [0, .. measuredWidths.ToArray(), 0];
                    else if (start == run.ElementStart)
                        result.Widths = [0, .. measuredWidths.ToArray()];
                    else if (result.End == run.ElementEnd)
                        result.Widths = [.. measuredWidths.ToArray(), 0];
                    else
                        result.Widths = measuredWidths.ToArray();
                    result.Rect = new Rect(0, 0, currentWidth, run._height);
                    return result;
                }
                int i;
                for (i = measuredWidths.Length - 1; i >= 1; i--)
                {
                    currentWidth -= measuredWidths[i];
                    if (currentWidth <= availableWidth)
                        break;
                }
                if (isFullLine && currentWidth > availableWidth)
                {
                    if (start == run.ElementStart)
                        result.Widths = [0, measuredWidths[0]];
                    else
                        result.Widths = [measuredWidths[0]];
                    result.End = run.ContentStart.GetPositionAtOffset(1, LogicalDirection.Forward)!;
                    result.Overflow = new TextRange(result.End, end);
                    result.Rect = new Rect(0, 0, currentWidth, run._height);
                }
                else
                {
                    if (i == 0)
                    {
                        result.Widths = Array.Empty<float>();
                        result.Overflow = new TextRange(start, end);
                        result.Rect = Rect.Empty;
                    }
                    else
                    {
                        if (start == run.ElementStart)
                        {
                            result.Widths = [0, .. measuredWidths.Slice(0, i)];
                            result.End = run.ContentStart.GetPositionAtOffset(i, LogicalDirection.Forward)!;
                        }
                        else
                        {
                            result.Widths = measuredWidths.Slice(0, i).ToArray();
                            result.End = start.GetPositionAtOffset(i, LogicalDirection.Forward)!;
                        }
                        result.Overflow = new TextRange(result.End, end);
                        result.Rect = new Rect(0, 0, currentWidth, run._height);
                    }
                }
                return result;
            }
        }

        #endregion
    }
}
