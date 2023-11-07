using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;

namespace Wodsoft.UI
{
    /// <summary>
    /// Framework Renderer Provider.
    /// </summary>
    public interface IRendererProvider
    {
        VisualDrawingContext GetDrawingContext(Visual visual);
    }
}
