using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    public class EllipseGeometry : Geometry
    {
        #region Properties


        public static readonly DependencyProperty RadiusXProperty =
                  DependencyProperty.Register("RadiusX",
                                   typeof(float),
                                   typeof(EllipseGeometry),
                                   new PropertyMetadata(0.0f));
        public float RadiusX
        {
            get { return (float)GetValue(RadiusXProperty)!; }
            set { SetValue(RadiusXProperty, value); }
        }

        public static readonly DependencyProperty RadiusYProperty =
                  DependencyProperty.Register("RadiusY",
                                   typeof(float),
                                   typeof(EllipseGeometry),
                                   new PropertyMetadata(0.0f));
        public float RadiusY
        {
            get { return (float)GetValue(RadiusYProperty)!; }
            set { SetValue(RadiusYProperty, value); }
        }

        public static readonly DependencyProperty CenterProperty =
                  DependencyProperty.Register("Center",
                                   typeof(Point),
                                   typeof(EllipseGeometry),
                                   new PropertyMetadata(new Point()));
        public Point Center
        {
            get { return (Point)GetValue(CenterProperty)!; }
            set { SetValue(CenterProperty, value); }
        }

        #endregion

        #region Methods

        public override PathGeometryData GetPathGeometryData()
        {
            Point[] points = GetPointList();

            var ctx = CreateStreamGeometryContext();

            ctx.BeginFigure(points[0], true /* is filled */, true /* is closed */);

            // i == 0, 3, 6, 9
            for (int i = 0; i < 12; i += 3)
            {
                ctx.BezierTo(points[i + 1], points[i + 2], points[i + 3], true /* is stroked */, true /* is smooth join */);
            }
            return ctx.GetGeometryData();
        }

        public override bool IsEmpty()
        {
            return false;
        }
        private Point[] GetPointList()
        {
            Point[] points = new Point[_PointCount];
            GetPointList(points, _PointCount, Center, RadiusX, RadiusY);
            return points;
        }

        private static void GetPointList(Point[] points, uint pointsCount, Point center, float radiusX, float radiusY)
        {
            radiusX = Math.Abs(radiusX);
            radiusY = Math.Abs(radiusY);

            // Set the X coordinates
            float mid = radiusX * ArcAsBezier;

            points[0].X = points[1].X = points[11].X = points[12].X = center.X + radiusX;
            points[2].X = points[10].X = center.X + mid;
            points[3].X = points[9].X = center.X;
            points[4].X = points[8].X = center.X - mid;
            points[5].X = points[6].X = points[7].X = center.X - radiusX;

            // Set the Y coordinates
            mid = radiusY * ArcAsBezier;

            points[2].Y = points[3].Y = points[4].Y = center.Y + radiusY;
            points[1].Y = points[5].Y = center.Y + mid;
            points[0].Y = points[6].Y = points[12].Y = center.Y;
            points[7].Y = points[11].Y = center.Y - mid;
            points[8].Y = points[9].Y = points[10].Y = center.Y - radiusY;
        }

        internal const float ArcAsBezier = 0.5522847498307933984f; // =( \/2 - 1)*4/3
        private const uint _PointCount = 13;

        #endregion

        #region Clone

        protected override Freezable CreateInstanceCore()
        {
            return new EllipseGeometry();
        }

        #endregion
    }
}
