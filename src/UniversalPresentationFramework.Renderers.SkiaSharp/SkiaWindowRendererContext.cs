using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Renderers
{
    public abstract class SkiaWindowRendererContext : SkiaBackendRendererContext
    {
        private int _width, _height;
        private SKSurface[]? _surfaces;
        private SKSurface? _currentSurface;

        protected abstract int CurrentBufferIndex { get; }

        public abstract ISkiaWindowContext WindowContext { get; }

        public override SKAlphaType AlphaType => WindowContext.AlphaType;

        public override SKColorType ColorType => WindowContext.ColorType;

        public override SKColorSpace ColorSpace => WindowContext.ColorSpace;

        protected SKSurface? CurrentSurface => _currentSurface;

        protected abstract GRBackendRenderTarget[] CreateRenderTargets(int width, int height);

        protected override SKSurface GetSurface()
        {
            var width = WindowContext.Width;
            var height = WindowContext.Height;
            if (_surfaces == null || _width != width || _height != height)
            {
                if (_surfaces != null)
                {
                    for (int i = 0; i < _surfaces.Length; i++)
                    {
                        _surfaces[i].Dispose();
                    }
                }
                _surfaces = CreateSurfaces(width, height);
                _width = width;
                _height = height;
            }
            _currentSurface = _surfaces[CurrentBufferIndex];
            return _currentSurface;
        }

        protected virtual SKSurface[] CreateSurfaces(int width, int height)
        {
            var renderTargets = CreateRenderTargets(width, height);
            var surfaces = new SKSurface[renderTargets.Length];
            for (int i = 0; i < renderTargets.Length; i++)
            {
                surfaces[i] = SKSurface.Create(GRContext, renderTargets[i], SurfaceOrigin, ColorType, ColorSpace);
            }
            return surfaces;
        }

        protected override void DisposeCore(bool disposing)
        {
            if (disposing && _surfaces != null)
            {
                for (int i = 0; i < _surfaces.Length; i++)
                {
                    _surfaces[i].Dispose();
                }
            }
        }
    }
}
