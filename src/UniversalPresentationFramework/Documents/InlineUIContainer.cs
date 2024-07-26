using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Documents
{
    [ContentProperty("Child")]
    public class InlineUIContainer : Inline
    {
        private UIElement? _child;

        #region Constructor

        public InlineUIContainer() { }

        public InlineUIContainer(UIElement child)
        {
            _child = child;
        }

        #endregion

        #region Properties

        protected internal override int ContentCount => _child == null ? 0 : 1;

        public UIElement? Child
        {
            get => _child;
            set
            {
                _child = value;
            }
        }


        #endregion

        #region Layout

        private InlineUIContainerLayout? _layout;
        public override IInlineLayout Layout => _layout ??= new InlineUIContainerLayout(this);

        private class InlineUIContainerLayout : IInlineLayout
        {
            private readonly InlineUIContainer _container;
            private float _width;

            public InlineUIContainerLayout(InlineUIContainer container)
            {
                _container = container;
            }

            public bool IsFloat => false;

            public void Draw(DrawingContext drawingContext, in Rect origin, in Rect clip, in float lineHeight, ReadOnlySpan<float> widths, TextPointer start, TextPointer end)
            {
                var child = _container._child;
                if (child != null)
                {
                    drawingContext.PushTransform(new TranslateTransform(origin.X, origin.Y));
                    child.OnRenderInternal(drawingContext);
                    drawingContext.Pop();
                }
            }

            public TextPointer GetCharacterAtPoint(in Point point, TextPointer start, TextPointer end)
            {
                if (point.X < _width / 2)
                    return _container.ContentStart;
                else
                    return _container.ContentEnd;
            }

            public InlineLayoutMeasureResult Measure(TextPointer start, TextPointer end, float availableWidth, bool isFullLine, TextWrapping textWrapping, TextTrimming textTrimming)
            {
                var container = _container;
                if (start != container.ContentStart && start != container.ElementStart)
                    throw new ArgumentOutOfRangeException("Start  position is not equal to container.");
                if (end != container.ContentEnd && end != container.ElementEnd)
                    throw new ArgumentOutOfRangeException("End position is not equal to container.");
                var result = new InlineLayoutMeasureResult();
                result.Start = start;
                result.End = end;
                var child = container._child;
                if (child == null)
                {
                    result.Widths = Array.Empty<float>();
                    result.Rect = Rect.Empty;
                    return result;
                }
                child.Arrange(new Rect());
                var desiredSize = child.DesiredSize;
                if (desiredSize.Width > availableWidth && !isFullLine)
                {
                    result.Widths = Array.Empty<float>();
                    result.Overflow = new TextRange(start, end);
                    result.Rect = Rect.Empty;
                    return result;
                }
                if (start == container.ElementStart && end == container.ElementEnd)
                    result.Widths = [0, desiredSize.Width, 0];
                else if (start == container.ElementStart)
                    result.Widths = [0, desiredSize.Width];
                else if (end == container.ElementEnd)
                    result.Widths = [desiredSize.Width, 0];
                else
                    result.Widths = [desiredSize.Width];
                if (desiredSize.Width > availableWidth)
                    _width = availableWidth;
                else
                    _width = desiredSize.Width;
                result.Rect = new Rect(desiredSize);
                return result;
            }
        }

        #endregion
    }
}
