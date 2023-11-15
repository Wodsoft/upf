using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Renderers
{
    public class SkiaRendererSoftwareContext : SkiaRendererContext
    {
        public SkiaRendererSoftwareContext() : base(null)
        {
        }

        protected override SKSurface CreateSurface(int width, int height)
        {
            return SKSurface.Create(new SKImageInfo
            {
                Width = width,
                Height = height,
                ColorType = SKColorType.Rgba8888,
                AlphaType = SKAlphaType.Premul
            });
        }

        public SKImage GetImage()
        {
            return Surface!.Snapshot();
        }
    }
}
