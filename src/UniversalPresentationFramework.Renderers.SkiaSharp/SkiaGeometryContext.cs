using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Renderers
{
    public class SkiaGeometryContext : StreamGeometryContext
    {
        private readonly SKPath _path;

        public SkiaGeometryContext()
        {
            _path = new SKPath();
        }

        private SkiaGeometryContext(SKPath path)
        {
            _path = new SKPath(path);
        }

        public override void ArcTo(Point point, Size size, float rotationAngle, bool isLargeArc, SweepDirection sweepDirection, bool isStroked, bool isSmoothJoin)
        {
            _path.ArcTo(point.X, point.Y, rotationAngle, isLargeArc ? SKPathArcSize.Large : SKPathArcSize.Small,
                sweepDirection == SweepDirection.Counterclockwise ? SKPathDirection.CounterClockwise : SKPathDirection.Clockwise, point.X + size.Width, point.Y + size.Height);
        }

        public override void BeginFigure(Point startPoint, bool isFilled, bool isClosed)
        {
            if (isClosed)
                _path.Close();
            _path.FillType = SKPathFillType.EvenOdd;
            _path.MoveTo(startPoint.X, startPoint.Y);
        }

        public override void BezierTo(Point point1, Point point2, Point point3, bool isStroked, bool isSmoothJoin)
        {
            _path.CubicTo(Unsafe.As<Point, SKPoint>(ref point1), Unsafe.As<Point, SKPoint>(ref point2), Unsafe.As<Point, SKPoint>(ref point3));
        }

        public override void LineTo(Point point, bool isStroked, bool isSmoothJoin)
        {
            _path.LineTo(Unsafe.As<Point, SKPoint>(ref point));
        }

        public override void PolyBezierTo(IList<Point> points, bool isStroked, bool isSmoothJoin)
        {
            _path.AddPoly(points.Select(t => new SKPoint(t.X, t.Y)).ToArray());
        }

        public override void PolyLineTo(IList<Point> points, bool isStroked, bool isSmoothJoin)
        {
            foreach (var point in points)
                _path.LineTo(point.X, point.Y);
        }

        public override void PolyQuadraticBezierTo(IList<Point> points, bool isStroked, bool isSmoothJoin)
        {
            for (int i = 0; i < points.Count; i += 2)
            {
                _path.QuadTo(points[i].X, points[i].Y, points[i + 1].X, points[i + 1].Y);
            }
        }

        public override void QuadraticBezierTo(Point point1, Point point2, bool isStroked, bool isSmoothJoin)
        {
            _path.QuadTo(point1.X, point1.Y, point2.X, point2.Y);
        }

        private PathGeometryData? _data;
        public override PathGeometryData GetGeometryData()
        {
            if (_data == null)
                _data = new SkiaGeometryData(_path);
            return _data;
        }

        public override StreamGeometryContext Clone()
        {
            return new SkiaGeometryContext(_path);
        }
    }
}
