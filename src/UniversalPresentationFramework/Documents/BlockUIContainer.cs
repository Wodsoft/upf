using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Documents
{
    [ContentProperty("Child")]
    public class BlockUIContainer : Block
    {
        private UIElement? _child;

        #region Constructor

        public BlockUIContainer() { }

        public BlockUIContainer(UIElement child)
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

        private BlockUIContainerLayout? _layout;
        public override IBlockLayout Layout => _layout ??= new BlockUIContainerLayout(this);

        private class BlockUIContainerLayout : IBlockLayout
        {
            private readonly BlockUIContainer _container;
            private float _width, _height;

            public BlockUIContainerLayout(BlockUIContainer container)
            {
                _container = container;
            }

            public float Width => _width;

            public float Height => _height;

            public void Draw(DrawingContext drawingContext, in Point origin, in Rect clip)
            {
                var container = _container;
                var margin = container.Margin;
                var padding = container.Padding;
                var brush = container.Background;
                if (brush != null)
                    drawingContext.DrawRectangle(brush, null, new Rect(origin.X + margin.Left, origin.Y + margin.Top, _width - margin.Left - margin.Right, _height - margin.Top - margin.Bottom));
                var child = _container._child;
                if (child != null)
                {
                    drawingContext.PushTransform(new TranslateTransform(origin.X + margin.Left + padding.Left, origin.Y + margin.Top + padding.Top));
                    child.OnRenderInternal(drawingContext);
                    drawingContext.Pop();
                }
            }

            public TextPointer GetCharacterAtPoint(in Point point)
            {
                throw new NotImplementedException();
            }

            public bool GetCharacterRelateToCharacter(TextPointer pointer, in LogicalDirection direction, [NotNullWhen(true)] out TextPointer? position)
            {
                throw new NotImplementedException();
            }

            public Rect GetRectAtCharacter(TextPointer pointer)
            {
                throw new NotImplementedException();
            }

            public void Measure(Size availableSize)
            {
                var container = _container;
                var margin = container.Margin;
                var padding = container.Padding;
                var child = container._child;
                _width = availableSize.Width;
                _height = margin.Top + padding.Top + margin.Bottom + padding.Bottom;
                if (child == null)
                    return;
                child.Arrange(new Rect(0, 0, availableSize.Width, 0));
                var desiredSize = child.DesiredSize;
                _height += desiredSize.Height;
            }
        }

        #endregion

    }
}
