using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media.Animation;

namespace Wodsoft.UI.Media
{
    public abstract class Geometry : Animatable
    {
        public virtual Rect Bounds { get; set; }

        public abstract PathGeometryData GetPathGeometryData();

        public abstract bool IsEmpty();

        protected StreamGeometryContext CreateStreamGeometryContext()
        {
            return FrameworkCoreProvider.GetRendererProvider().CreateGeometryContext();
        }
    }
}
