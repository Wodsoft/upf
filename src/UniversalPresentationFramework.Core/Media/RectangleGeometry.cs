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

        protected override IEnumerable<PathFigure> GetPathGeometryData()
        {
            throw new NotImplementedException();
        }
    }
}
