using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    public abstract class VisualDrawingContext : DrawingContext
    {
        public abstract IDrawingContent? Close();
    }
}
