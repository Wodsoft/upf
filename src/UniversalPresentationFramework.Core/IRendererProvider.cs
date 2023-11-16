using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;
using Wodsoft.UI.Media.Imaging;

namespace Wodsoft.UI
{
    /// <summary>
    /// Framework Renderer Provider.
    /// </summary>
    public interface IRendererProvider
    {
        VisualDrawingContext CreateDrawingContext(Visual visual);

        IImageContext CreateImageContext(Stream stream, int newWidth, int newHeight, Rotation rotation);
    }
}
