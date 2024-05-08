using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Renderers
{
    public class SkiaWindowRendererSoftwareContext : SkiaWindowRendererContext
    {
        private readonly ISkiaWindowContext _windowContext;
        private SKImage? _image;
        public SkiaWindowRendererSoftwareContext(ISkiaWindowContext windowContext)
        {
            _windowContext = windowContext;
        }

        public override ISkiaWindowContext WindowContext => _windowContext;

        public override GRContext GRContext => throw new NotSupportedException();

        public override GRSurfaceOrigin SurfaceOrigin => GRSurfaceOrigin.TopLeft;

        protected override int CurrentBufferIndex => 0;

        protected SKImage Image => _image ?? throw new InvalidOperationException("Image not create yet.");

        protected override GRBackendRenderTarget[] CreateRenderTargets(int width, int height)
        {
            throw new NotSupportedException();
        }

        protected override SKSurface[] CreateSurfaces(int width, int height)
        {
            if (_image != null)
                _image.Dispose();
            SKImageInfo imageInfo = new SKImageInfo
            {
                AlphaType = AlphaType,
                ColorSpace = ColorSpace,
                ColorType = ColorType,
                Height = height,
                Width = width,
            };
            _image = SKImage.Create(imageInfo);
            return [SKSurface.Create(_image.PeekPixels())];
        }

        protected override void DisposeCore(bool disposing)
        {
            if (disposing && _image != null)
            {
                _image.Dispose();
                _image = null;
            }
        }
    }
}
