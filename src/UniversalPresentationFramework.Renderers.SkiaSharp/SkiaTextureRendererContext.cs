using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Renderers
{
    public abstract class SkiaTextureRendererContext : SkiaBackendRendererContext
    {
        private SKSurface? _surface;
        private SKImage? _image;

        public abstract GRBackendTexture Texture { get; }

        public virtual SKImage Image
        {
            get
            {
                if (_image == null)
                    _image = SKImage.FromTexture(GRContext, Texture, SurfaceOrigin, ColorType, AlphaType, ColorSpace);
                return _image;
            }
        }

        public abstract int SampleCount { get; }

        public abstract int Width { get; }

        public abstract int Height { get; }

        protected override SKSurface GetSurface()
        {
            if (_surface == null)
            {
                _surface = SKSurface.Create(GRContext, Texture, SurfaceOrigin, SampleCount, ColorType, ColorSpace);
                if (_surface == null)
                    throw new NotSupportedException("Create SKSurface failed.");
            }
            return _surface;
        }

        protected override void AfterRender()
        {
            GRContext.Submit(true);
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
