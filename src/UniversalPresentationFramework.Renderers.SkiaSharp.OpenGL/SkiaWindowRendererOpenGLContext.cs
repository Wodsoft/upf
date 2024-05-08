using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Renderers
{
    public class SkiaWindowRendererOpenGLContext : SkiaWindowRendererContext
    {
        private readonly ISkiaWindowOpenGLContext _windowContext;
        private GRBackendRenderTarget? _renderTarget;

        public SkiaWindowRendererOpenGLContext(ISkiaWindowOpenGLContext windowContext)
        {
            _windowContext = windowContext;
        }

        public override ISkiaWindowContext WindowContext => _windowContext;

        public override GRContext GRContext => _windowContext.GRContext;

        protected override int CurrentBufferIndex => 0;

        public override GRSurfaceOrigin SurfaceOrigin => GRSurfaceOrigin.BottomLeft;

        protected override GRBackendRenderTarget[] CreateRenderTargets(int width, int height)
        {
            if (_renderTarget != null)
                _renderTarget.Dispose();
            var frameBuffer = _windowContext.GetInteger(0X8ca6);
            var stencil = _windowContext.GetInteger(0x0D57);
            var samples = _windowContext.GetInteger(0X80a9);
            var maxSamples = _windowContext.GRContext.GetMaxSurfaceSampleCount(_windowContext.ColorType);
            if (samples > maxSamples)
                samples = maxSamples;
            var glInfo = new GRGlFramebufferInfo((uint)frameBuffer, _windowContext.ColorType.ToGlSizedFormat());
            _renderTarget = new GRBackendRenderTarget(width, height, samples, stencil, glInfo);
            return [_renderTarget];
        }

        protected override void BeforeRender()
        {
            _windowContext.MakeCurrent();
        }

        protected override void AfterRender()
        {
            _windowContext.GRContext.Submit();
            _windowContext.SwapBuffers();
        }
    }
}
