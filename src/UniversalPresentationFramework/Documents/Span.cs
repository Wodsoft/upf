using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using Wodsoft.UI.Markup;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Documents
{
    [ContentProperty("Inlines")]
    public class Span : Inline, IAddChild
    {
        private readonly InlineCollection _inlines;
        private ParagraphLayout _inlineLayout;

        #region Constructors

        static Span()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Span), new FrameworkPropertyMetadata(typeof(Span)));
        }

        public Span()
        {
            _inlines = new InlineCollection(this, TextElementNode);
            _inlineLayout = new ParagraphLayout(this, _inlines);
        }

        public Span(Inline inline) : this()
        {
            _inlines.Add(inline);
        }

        #endregion

        #region IAddChild

        void IAddChild.AddChild(object value)
        {
            if (value is Inline inline)
                _inlines.Add(inline);
            else if (value is string text)
                _inlines.Add(new Run(text));
            else if (value is UIElement ue)
                _inlines.Add(new InlineUIContainer(ue));
            else
                throw new ArgumentException($"Invalid child type \"{value.GetType().FullName}\".");
        }

        void IAddChild.AddText(string text)
        {
            _inlines.Add(new Run(text));
        }

        #endregion

        #region Properties

        public InlineCollection Inlines => _inlines;

        #endregion

        #region Layout

        private SpanLayout? _layout;
        public override IInlineLayout Layout => _layout ??= new SpanLayout(this, _inlines);

        private class SpanLayout : IInlineLayout
        {
            private readonly Span _span;
            private readonly InlineCollection _inlines;
            private int _lastMeasuredInline, _lastDrawInline;

            public SpanLayout(Span span, InlineCollection inlines)
            {
                _span = span;
                _inlines = inlines;
            }

            public bool IsFloat => false;

            public void Draw(DrawingContext drawingContext, in Rect origin, in Rect clip, in float lineHeight, ReadOnlySpan<float> widths, TextPointer start, TextPointer end)
            {
                var inlines = _inlines;
                if (inlines.Count == 0)
                    return;
                var span = _span;
                var brush = span.Background;
                if (brush != null)
                    drawingContext.DrawRectangle(brush, null, origin);
                if (end < span.ElementStart)
                    throw new ArgumentOutOfRangeException("End is less than Span content start.");
                if (start > span.ElementEnd)
                    throw new ArgumentOutOfRangeException("Start is greater than Span content end.");
                var i = _lastMeasuredInline;
                if (i >= inlines.Count)
                    i = 0;
                Inline inline;
                if (start < inlines[i].ElementStart)
                    i = 0;
                inline = inlines[i];
                while (start > inline.ElementEnd)
                {
                    i++;
                    inline = inlines[i];
                }
                var inlineOrigin = origin;
                var inlineClip = clip;
                while (true)
                {
                    if (start < inline.ContentStart)
                        start = inline.ContentStart;
                    if (end > inline.ContentEnd)
                    {
                        var chars = start.GetOffsetToPosition(inline.ContentEnd);
                        var inlineWidths = widths.Slice(inline.TextElementNode.StartSymbolCount, chars);
                        inline.Layout.Draw(drawingContext, inlineOrigin, inlineClip, lineHeight, inlineWidths, start, inline.ContentEnd);
                        widths = widths.Slice(chars);
                        var inlineWidth = inlineWidths.Sum();
                        inlineOrigin.X += inlineWidth;
                        inlineClip.X += inlineWidth;
                        i++;
                        if (i < inlines.Count)
                            inline = inlines[i];
                        else
                            break;
                    }
                    else
                    {
                        inline.Layout.Draw(drawingContext, origin, clip, lineHeight, widths, start, end);
                        break;
                    }
                }
            }

            public TextPointer GetCharacterAtPoint(in Point point, TextPointer start, TextPointer end)
            {
                var inlines = _inlines;
                var span = _span;
                if (end < span.ElementStart)
                    throw new ArgumentOutOfRangeException("End is less than Span content start.");
                if (start > span.ElementEnd)
                    throw new ArgumentOutOfRangeException("Start is greater than Span content end.");
                var i = _lastMeasuredInline;
                if (i >= inlines.Count)
                    i = 0;
                Inline inline;
                if (start < inlines[i].ElementStart)
                    i = 0;
                inline = inlines[i];
                while (start > inline.ElementEnd)
                {
                    i++;
                    inline = inlines[i];
                }
                if (start < inline.ContentStart)
                    start = inline.ContentStart;
                if (end > inline.ContentEnd)
                    end = inline.ContentEnd;
                return inline.Layout.GetCharacterAtPoint(point, start, end);
            }

            public InlineLayoutMeasureResult Measure(TextPointer start, TextPointer end, float availableWidth, bool isFullLine, TextWrapping textWrapping, TextTrimming textTrimming)
            {
                var result = new InlineLayoutMeasureResult();
                result.Start = start;
                var inlines = _inlines;
                if (inlines.Count == 0)
                {
                    result.Widths = Array.Empty<float>();
                    result.Rect = Rect.Empty;
                    return result;
                }
                var span = _span;
                if (end < span.ElementStart)
                    throw new ArgumentOutOfRangeException("End is less than Span content start.");
                if (start > span.ElementEnd)
                    throw new ArgumentOutOfRangeException("Start is greater than Span content end.");
                List<float> allWidths = new List<float>();
                if (start == span.ElementStart)
                    allWidths.Add(0f);
                var i = _lastMeasuredInline;
                if (i >= inlines.Count)
                    i = 0;
                Inline inline;
                if (start < inlines[i].ElementStart)
                    i = 0;
                inline = inlines[i];
                if (start < inline.ElementStart)
                    start = inline.ElementStart;
                else
                {
                    //fetch inline in range
                    while (start > inline.ElementEnd)
                    {
                        i++;
                        inline = inlines[i];
                    }
                }
                Rect rect = new Rect();
                //measure inlines in range
                while (true)
                {
                    InlineLayoutMeasureResult inlineMeasure;
                    if (end > inline.ElementEnd)
                        inlineMeasure = inline.Layout.Measure(start, inline.ElementEnd, availableWidth, isFullLine, textWrapping, textTrimming);
                    else
                        inlineMeasure = inline.Layout.Measure(start, end, availableWidth, isFullLine, textWrapping, textTrimming);
                    //handle float inline
                    if (inline.Layout.IsFloat)
                    {
                        //float inline only accept standalone
                        if (allWidths.Count == 0)
                        {
                            _lastMeasuredInline = i;
                            return inlineMeasure;
                        }
                        else
                        {
                            start = inline.ElementStart;
                            break;
                        }
                    }
                    if (inlineMeasure.Widths.Length != 0)
                    {
                        rect.Width += inlineMeasure.Rect.Width;
                        if (inlineMeasure.Rect.Height > rect.Height)
                        {
                            rect.Height = inlineMeasure.Rect.Height;
                            if (inlineMeasure.LineHeight != 0f)
                                result.LineHeight = inlineMeasure.LineHeight;
                        }
                        allWidths.AddRange(inlineMeasure.Widths);
                    }
                    if (inlineMeasure.Overflow == null)
                    {
                        i++;
                        if (i < inlines.Count)
                        {
                            inline = inlines[i];
                            start = inline.ContentStart;
                        }
                        else
                        {
                            start = end;
                            break;
                        }
                        isFullLine = false;
                    }
                    else
                    {
                        start = inlineMeasure.Overflow.Start;
                        break;
                    }
                }
                if (start == end)
                {
                    result.End = end;
                    if (end == span.ElementEnd)
                        allWidths.Add(0);
                }
                else
                {
                    result.End = start;
                    result.Overflow = new TextRange(start, end);
                }
                result.Widths = allWidths.ToArray();
                _lastMeasuredInline = i;
                result.Rect = rect;
                return result;
            }
        }

        #endregion
    }
}
