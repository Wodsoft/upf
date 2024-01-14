using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Renderers
{
    public class SkiaRenderBitmapContext : SkiaBitmapContext, IRenderBitmapContext
    {
        private readonly SkiaRendererSoftwareContext _rendererContext;

        public SkiaRenderBitmapContext(SKBitmap bitmap) : base(bitmap)
        {
            var pixels = bitmap.PeekPixels();
            _rendererContext = new SkiaRendererSoftwareContext(pixels);
        }

        public void Render(Visual visual)
        {
            _rendererContext.Render(visual);
        }

        
    }
}
