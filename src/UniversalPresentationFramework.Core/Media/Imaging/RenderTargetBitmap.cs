using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Imaging
{
    public sealed class RenderTargetBitmap : BitmapSource
    {
        private readonly IRenderBitmapContext _context;

        public RenderTargetBitmap(int pixelWidth, int pixelHeight, double dpiX, double dpiY, PixelFormat pixelFormat)
        {
            if (FrameworkProvider.RendererProvider == null)
                throw new InvalidOperationException("Framework not initialized.");
            _context = FrameworkProvider.RendererProvider.CreateRenderBitmapContext(pixelWidth, pixelHeight, dpiX, dpiY, pixelFormat);
        }

        public override int PixelWidth => _context.Width;

        public override int PixelHeight => _context.Height;

        public override IImageContext Context => _context;

        public override PixelFormat Format => _context.PixelFormat;

        protected override bool DelayCreation => false;

        public void Render(Visual visual)
        {
            _context.Render(visual);
        }
    }
}
