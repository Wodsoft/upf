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
        private readonly SKPixmap? _pixmap;

        public SkiaRendererSoftwareContext() : base(null)
        {
        }

        public SkiaRendererSoftwareContext(SKPixmap pixmap) : base(null)
        {
            if (pixmap == null)
                throw new ArgumentNullException(nameof(pixmap));
            _pixmap = pixmap;
        }

        protected override bool ShouldCreateNewSurface(int width, int height)
        {
            return _pixmap == null;
        }

        protected override SKSurface CreateSurface(int width, int height)
        {
            if (_pixmap == null)
                return SKSurface.Create(new SKImageInfo
                {
                    Width = width,
                    Height = height,
                    ColorType = SKColorType.Rgba8888,
                    AlphaType = SKAlphaType.Premul
                });
            else
                return SKSurface.Create(_pixmap);
        }

        public SKImage GetImage()
        {
            return Surface!.Snapshot();
        }
    }
}
