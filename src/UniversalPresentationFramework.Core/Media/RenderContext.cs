using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    public abstract class RenderContext
    {
        public abstract void Render(IDrawingContent drawingContent);
    }
}
