using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Renderers
{
    public abstract class SkiaBackendRendererContext : SkiaRendererContext
    {
        public abstract GRContext GRContext { get; }

        public abstract SKAlphaType AlphaType { get; }

        public abstract SKColorType ColorType { get; }

        public abstract SKColorSpace ColorSpace { get; }

        public abstract GRSurfaceOrigin SurfaceOrigin { get; }
    }
}
