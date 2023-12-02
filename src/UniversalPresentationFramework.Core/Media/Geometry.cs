using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    public abstract class Geometry : DependencyObject
    {
        public virtual Rect Bounds { get; set; }

        protected abstract IEnumerable<PathFigure> GetPathGeometryData();
    }
}
