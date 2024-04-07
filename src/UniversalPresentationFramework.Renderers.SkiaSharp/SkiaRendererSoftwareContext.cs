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
        private SKSurface? _surface;

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

        protected override void CreateSurfaces(int width, int height)
        {
            _surface = CreateSurface(width, height);
        }

        protected virtual SKSurface CreateSurface(int width, int height)
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

        protected override void DeleteSurfaces()
        {
            if (_surface != null)
            {
                _surface.Dispose();
                _surface = null;
            }
        }

        protected override SKSurface? GetSurface() => _surface;

        public SKImage GetImage()
        {
            if (_surface == null)
                throw new InvalidOperationException("Surface not created.");
            return _surface!.Snapshot();
        }
    }
}
