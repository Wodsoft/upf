using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;
using Wodsoft.UI.Media.Imaging;

namespace Wodsoft.UI.Renderers
{
    public class SkiaRenderBitmapContext : SkiaImageContextBase, IRenderBitmapContext
    {
        private readonly SkiaTextureRendererContext _rendererContext;
        private SKImage? _image;

        public SkiaRenderBitmapContext(SkiaTextureRendererContext rendererContext) : this(rendererContext, SkiaHelper.GetPixelFormat(rendererContext.ColorType, rendererContext.AlphaType, rendererContext.ColorSpace))
        {
        }

        public SkiaRenderBitmapContext(SkiaTextureRendererContext rendererContext, PixelFormat pixelFormat)
        {
            _rendererContext = rendererContext;
            _rendererContext.IsShowFPS = false;
            PixelFormat = pixelFormat;
        }

        public override int Width => _rendererContext.Width;

        public override int Height => _rendererContext.Height;

        public override PixelFormat PixelFormat { get; }

        public override SKImage Image
        {
            get
            {
                if (_image == null)
                {
                    _image = SKImage.FromTexture(_rendererContext.GRContext, _rendererContext.Texture, GRSurfaceOrigin.TopLeft, _rendererContext.ColorType);
                }
                return _image;
            }
        }

        public void Render(Visual visual)
        {
            _rendererContext.Render(visual);
        }
    }
}
