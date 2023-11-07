using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;

namespace Wodsoft.UI
{
    public abstract class RendererProvider : IRendererProvider
    {
        internal static IRendererProvider? Current;

        public abstract VisualDrawingContext GetDrawingContext(Visual visual);
    }
}
