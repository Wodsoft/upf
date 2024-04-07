using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Renderers
{
    public abstract class SkiaRendererOpenGLContext : SkiaRendererContext
    {
        private GRBackendRenderTarget? _renderTarget;
        private SKSurface? _surface;

        public SkiaRendererOpenGLContext(GRContext? grContext) : base(grContext)
        {
            if (grContext == null)
                throw new ArgumentNullException(nameof(grContext));
        }

        protected override void CreateSurfaces(int width, int height)
        {
            if (_renderTarget != null)
                _renderTarget.Dispose();
            var frameBuffer = GetInteger(0X8ca6);
            var stencil = GetInteger(0x0D57);
            var samples = GetInteger(0X80a9);
            var maxSamples = GRContext!.GetMaxSurfaceSampleCount(SKColorType.Rgba8888);
            if (samples > maxSamples)
                samples = maxSamples;
            var glInfo = new GRGlFramebufferInfo((uint)frameBuffer, SKColorType.Rgba8888.ToGlSizedFormat());
            _renderTarget = new GRBackendRenderTarget(width, height, samples, stencil, glInfo);
            _surface = SKSurface.Create(GRContext, _renderTarget, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888);
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

        protected abstract int GetInteger(int code);

        protected override void DisposeCore(bool disposing)
        {
            if (disposing)
            {
                if (_renderTarget != null)
                {
                    _renderTarget.Dispose();
                    _renderTarget = null;
                }
            }
        }
    }
}