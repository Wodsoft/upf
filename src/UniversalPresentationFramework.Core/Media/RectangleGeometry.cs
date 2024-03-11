using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    public class RectangleGeometry : Geometry
    {
        public RectangleGeometry()
        {
        }

        /// <summary>
        /// Constructor - sets the rounded rectangle to equal the passed in parameters
        /// </summary>
        public RectangleGeometry(Rect rect)
        {
            Rect = rect;
        }

        /// <summary>
        /// Constructor - sets the rounded rectangle to equal the passed in parameters
        /// </summary>
        public RectangleGeometry(Rect rect,
            float radiusX,
            float radiusY) : this(rect)
        {
            RadiusX = radiusX;
            RadiusY = radiusY;
        }

        #region Properties

        public static readonly DependencyProperty RadiusXProperty =
                  DependencyProperty.Register("RadiusX",
                                   typeof(float),
                                   typeof(RectangleGeometry),
                                   new PropertyMetadata(0f));
        public float RadiusX { get { return (float)GetValue(RadiusXProperty)!; } set { SetValue(RadiusXProperty, value); } }

        public static readonly DependencyProperty RadiusYProperty =
                  DependencyProperty.Register("RadiusY",
                                   typeof(float),
                                   typeof(RectangleGeometry),
                                   new PropertyMetadata(0f));
        public float RadiusY { get { return (float)GetValue(RadiusYProperty)!; } set { SetValue(RadiusYProperty, value); } }

        public static readonly DependencyProperty RectProperty =
                  DependencyProperty.Register("Rect",
                                   typeof(Rect),
                                   typeof(RectangleGeometry),
                                   new PropertyMetadata(Rect.Empty));
        public Rect Rect { get { return (Rect)GetValue(RectProperty)!; } set { SetValue(RectProperty, value); } }

        #endregion

        #region Methods

        public override bool IsEmpty()
        {
            return Rect.IsEmpty;
        }

        public override PathGeometryData GetPathGeometryData()
        {
            var ctx = CreateStreamGeometryContext();
            if (IsEmpty())
                return ctx.GetGeometryData();

            //PathGeometryData data = new PathGeometryData();
            //data.FillRule = FillRule.EvenOdd;
            //data.Matrix = CompositionResourceManager.TransformToMilMatrix3x2D(Transform);

            float radiusX = RadiusX;
            float radiusY = RadiusY;
            Rect rect = Rect;

            //ByteStreamGeometryContext ctx = new ByteStreamGeometryContext();

            if (IsRounded(radiusX, radiusY))
            {
                Point[] points = GetPointList(rect, radiusX, radiusY);

                ctx.BeginFigure(points[0], true /* is filled */, true /* is closed */);
                ctx.BezierTo(points[1], points[2], points[3], true /* is stroked */, false /* is smooth join */);
                ctx.LineTo(points[4], true /* is stroked */, false /* is smooth join */);
                ctx.BezierTo(points[5], points[6], points[7], true /* is stroked */, false /* is smooth join */);
                ctx.LineTo(points[8], true /* is stroked */, false /* is smooth join */);
                ctx.BezierTo(points[9], points[10], points[11], true /* is stroked */, false /* is smooth join */);
                ctx.LineTo(points[12], true /* is stroked */, false /* is smooth join */);
                ctx.BezierTo(points[13], points[14], points[15], true /* is stroked */, false /* is smooth join */);
            }
            else
            {
                ctx.BeginFigure(rect.TopLeft, true /* is filled */, true /* is closed */);
                ctx.LineTo(rect.TopRight, true /* is stroked */, false /* is smooth join */);
                ctx.LineTo(rect.BottomRight, true /* is stroked */, false /* is smooth join */);
                ctx.LineTo(rect.BottomLeft, true /* is stroked */, false /* is smooth join */);
            }
            return ctx.GetGeometryData();
        }

        internal static bool IsRounded(float radiusX, float radiusY)
        {
            return (radiusX != 0.0) && (radiusY != 0.0);
        }

        private Point[] GetPointList(Rect rect, float radiusX, float radiusY)
        {
            uint pointCount = GetPointCount(rect, radiusX, radiusY);
            Point[] points = new Point[pointCount];
            GetPointList(points, pointCount, rect, radiusX, radiusY);
            return points;
        }

        const uint _RoundedPointCount = 17;
        const uint _SquaredPointCount = 5;
        private uint GetPointCount(Rect rect, float radiusX, float radiusY)
        {
            if (rect.IsEmpty)
            {
                return 0;
            }
            else if (IsRounded(radiusX, radiusY))
            {
                return _RoundedPointCount;
            }
            else
            {
                return _SquaredPointCount;
            }
        }

        private static void GetPointList(Point[] points, uint pointsCount, Rect rect, float radiusX, float radiusY)
        {
            if (IsRounded(radiusX, radiusY))
            {
                radiusX = Math.Min(rect.Width * (1.0f / 2.0f), Math.Abs(radiusX));
                radiusY = Math.Min(rect.Height * (1.0f / 2.0f), Math.Abs(radiusY));

                float bezierX = ((1.0f - EllipseGeometry.ArcAsBezier) * radiusX);
                float bezierY = ((1.0f - EllipseGeometry.ArcAsBezier) * radiusY);

                points[1].X = points[0].X = points[15].X = points[14].X = rect.X;
                points[2].X = points[13].X = rect.X + bezierX;
                points[3].X = points[12].X = rect.X + radiusX;
                points[4].X = points[11].X = rect.Right - radiusX;
                points[5].X = points[10].X = rect.Right - bezierX;
                points[6].X = points[7].X = points[8].X = points[9].X = rect.Right;

                points[2].Y = points[3].Y = points[4].Y = points[5].Y = rect.Y;
                points[1].Y = points[6].Y = rect.Y + bezierY;
                points[0].Y = points[7].Y = rect.Y + radiusY;
                points[15].Y = points[8].Y = rect.Bottom - radiusY;
                points[14].Y = points[9].Y = rect.Bottom - bezierY;
                points[13].Y = points[12].Y = points[11].Y = points[10].Y = rect.Bottom;

                points[16] = points[0];
            }
            else
            {
                points[0].X = points[3].X = points[4].X = rect.X;
                points[1].X = points[2].X = rect.Right;

                points[0].Y = points[1].Y = points[4].Y = rect.Y;
                points[2].Y = points[3].Y = rect.Bottom;
            }
        }

        #endregion

        #region Clone

        protected override Freezable CreateInstanceCore()
        {
            return new RectangleGeometry();
        }

        #endregion
    }
}
