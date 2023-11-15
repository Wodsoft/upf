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
            return constraint;
        }

        #endregion

        #region Render

        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawRoundedRectangle(Fill,
                new Pen(Stroke, StrokeThickness, StrokeStartLineCap, StrokeEndLineCap, StrokeDashCap, StrokeLineJoin, StrokeMiterLimit, new DashStyle(StrokeDashArray, StrokeDashOffset)),
                new Rect(VisualOffset, RenderSize),
                RadiusX,
                RadiusY);
        }

        #endregion
    }
}
