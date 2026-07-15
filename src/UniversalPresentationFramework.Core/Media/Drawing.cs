using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media.Animation;

namespace Wodsoft.UI.Media
{
    public abstract class Drawing : Animatable
    {
        public abstract void Draw(DrawingContext drawingContext);
    }
}
