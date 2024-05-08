using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Renderers
{
    public class SkiaTextureRendererSoftwareContext : SkiaTextureRendererContext
    {
        private readonly SKImageInfo _imageInfo;
        private SKImage? _image;
        private SKSurface? _surface;

        public SkiaTextureRendererSoftwareContext(SKImageInfo imageInfo)
        {
            _imageInfo = imageInfo;
        }

        public override GRBackendTexture Texture => throw new NotSupportedException();

        public override int SampleCount => throw new NotImplementedException();

        public override int Width => _imageInfo.Width;

        public override int Height => _imageInfo.Height;

        public override GRContext GRContext => throw new NotSupportedException();

        public override SKAlphaType AlphaType => _imageInfo.AlphaType;

        public override SKColorType ColorType => _imageInfo.ColorType;

        public override SKColorSpace ColorSpace => _imageInfo.ColorSpace;

        public override GRSurfaceOrigin SurfaceOrigin => GRSurfaceOrigin.TopLeft;

        public override SKImage Image
        {
            get
            {
                if (_image == null)
                    _image = SKImage.Create(_imageInfo);
                return _image;
            }
        }

        protected override SKSurface GetSurface()
        {
            if (_surface == null)
                _surface = SKSurface.Create(Image.PeekPixels());
            return _surface;
        }

        protected override void DisposeCore(bool disposing)
        {
            if (disposing)
            {
                if (_surface != null)
                {
                    _surface.Dispose();
                    _surface = null;
                }
                if (_image != null)
                {
                    _image.Dispose();
                    _image = null;
                }
            }
        }
    }
}
