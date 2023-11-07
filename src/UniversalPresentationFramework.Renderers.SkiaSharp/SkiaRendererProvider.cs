using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Renderers
{
    public class SkiaRendererProvider : IRendererProvider
    {
        public VisualDrawingContext GetDrawingContext(Visual visual)
        {
            return new SkiaDrawingContext(0, 0);
        }
    }
}
