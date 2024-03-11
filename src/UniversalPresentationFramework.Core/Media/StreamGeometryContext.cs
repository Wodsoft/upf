using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    public abstract class StreamGeometryContext
    {
        /// <summary>
        /// BeginFigure - Start a new figure.
        /// </summary>
        public abstract void BeginFigure(Point startPoint, bool isFilled, bool isClosed);

        /// <summary>
        /// LineTo - append a LineTo to the current figure.
        /// </summary>
        public abstract void LineTo(Point point, bool isStroked, bool isSmoothJoin);

        /// <summary>
        /// QuadraticBezierTo - append a QuadraticBezierTo to the current figure.
        /// </summary>
        public abstract void QuadraticBezierTo(Point point1, Point point2, bool isStroked, bool isSmoothJoin);

        /// <summary>
        /// BezierTo - apply a BezierTo to the current figure.
        /// </summary>
        public abstract void BezierTo(Point point1, Point point2, Point point3, bool isStroked, bool isSmoothJoin);

        /// <summary>
        /// PolyLineTo - append a PolyLineTo to the current figure.
        /// </summary>
        public abstract void PolyLineTo(IList<Point> points, bool isStroked, bool isSmoothJoin);

        /// <summary>
        /// PolyQuadraticBezierTo - append a PolyQuadraticBezierTo to the current figure.
        /// </summary>
        public abstract void PolyQuadraticBezierTo(IList<Point> points, bool isStroked, bool isSmoothJoin);

        /// <summary>
        /// PolyBezierTo - append a PolyBezierTo to the current figure.
        /// </summary>
        public abstract void PolyBezierTo(IList<Point> points, bool isStroked, bool isSmoothJoin);

        public abstract void ArcTo(Point point, Size size, float rotationAngle, bool isLargeArc, SweepDirection sweepDirection, bool isStroked, bool isSmoothJoin);

        public abstract PathGeometryData GetGeometryData();

        public abstract StreamGeometryContext Clone();
    }
}
