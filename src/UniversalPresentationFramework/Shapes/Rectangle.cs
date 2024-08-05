using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Shapes
{
    public class Rectangle : Shape
    {
        private Rect _rect;

        static Rectangle()
        {
            StretchProperty.OverrideMetadata(typeof(Rectangle), new FrameworkPropertyMetadata(Stretch.Fill));
        }

        #region Properties

        public static readonly DependencyProperty RadiusXProperty =
            DependencyProperty.Register("RadiusX", typeof(float), typeof(Rectangle),
                new FrameworkPropertyMetadata(0f, FrameworkPropertyMetadataOptions.AffectsRender));
        [TypeConverter(typeof(LengthConverter))]
        public float RadiusX { get { return (float)GetValue(RadiusXProperty)!; } set { SetValue(RadiusXProperty, value); } }

        public static readonly DependencyProperty RadiusYProperty =
            DependencyProperty.Register("RadiusY", typeof(float), typeof(Rectangle),
                new FrameworkPropertyMetadata(0f, FrameworkPropertyMetadataOptions.AffectsRender));
        [TypeConverter(typeof(LengthConverter))]
        public float RadiusY { get { return (float)GetValue(RadiusYProperty)!; } set { SetValue(RadiusYProperty, value); } }

        #endregion

        #region Layout

        protected override Size MeasureOverride(Size constraint)
        {
            if (Stretch == Stretch.UniformToFill)
            {
                float width = constraint.Width;
                float height = constraint.Height;

                if (float.IsInfinity(width) && float.IsInfinity(height))
                {
                    return GetNaturalSize();
                }
                else if (float.IsInfinity(width) || float.IsInfinity(height))
                {
                    width = Math.Min(width, height);
                }
                else
                {
                    width = Math.Max(width, height);
                }

                return new Size(width, width);
            }

            return GetNaturalSize();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            float penThickness = GetStrokeThickness();
            float margin = penThickness / 2;

            _rect = new Rect(
                margin, // X
                margin, // Y
                Math.Max(0, finalSize.Width - penThickness),    // Width
                Math.Max(0, finalSize.Height - penThickness));  // Height

            switch (Stretch)
            {
                case Stretch.None:
                    // A 0 Rect.Width and Rect.Height rectangle
                    _rect.Width = _rect.Height = 0;
                    break;

                case Stretch.Fill:
                    // The most common case: a rectangle that fills the box.
                    // _rect has already been initialized for that.
                    break;

                case Stretch.Uniform:
                    // The maximal square that fits in the final box
                    if (_rect.Width > _rect.Height)
                    {
                        _rect.Width = _rect.Height;
                    }
                    else  // _rect.Width <= _rect.Height
                    {
                        _rect.Height = _rect.Width;
                    }
                    break;

                case Stretch.UniformToFill:

                    // The minimal square that fills the final box
                    if (_rect.Width < _rect.Height)
                    {
                        _rect.Width = _rect.Height;
                    }
                    else  // _rect.Width >= _rect.Height
                    {
                        _rect.Height = _rect.Width;
                    }
                    break;
            }


            //ResetRenderedGeometry();

            return finalSize;
        }

        #endregion

        #region Render

        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawRoundedRectangle(Fill,
                new Pen(Stroke, StrokeThickness, StrokeStartLineCap, StrokeEndLineCap, StrokeDashCap, StrokeLineJoin, StrokeMiterLimit, new DashStyle(StrokeDashArray, StrokeDashOffset)),
                _rect,
                RadiusX,
                RadiusY);
        }

        #endregion

        #region Shape

        protected override Rect GetDefiningGeometryBounds()
        {
            return _rect;
        }

        protected override Size GetNaturalSize()
        {
            float strokeThickness = GetStrokeThickness();
            return new Size(strokeThickness, strokeThickness);
        }

        #endregion
    }
}
