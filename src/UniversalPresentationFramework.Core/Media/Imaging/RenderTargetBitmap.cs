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
        private readonly double _dpiX;
        private readonly double _dpiY;

        private RenderTargetBitmap(IRenderBitmapContext context, double dpiX, double dpiY)
        {
            _context = context;
            _dpiX = dpiX;
            _dpiY = dpiY;
        }

        public RenderTargetBitmap(int pixelWidth, int pixelHeight, double dpiX, double dpiY, PixelFormat pixelFormat)
        {
            if (FrameworkProvider.RendererProvider == null)
                throw new InvalidOperationException("Framework not initialized.");
            _context = FrameworkProvider.RendererProvider.CreateRenderBitmapContext(pixelWidth, pixelHeight, dpiX, dpiY, pixelFormat);
            _dpiX = dpiX;
            _dpiY = dpiY;
        }

        public override int PixelWidth => _context.Width;

        public override int PixelHeight => _context.Height;

        public override IImageContext Context => _context;

        public override PixelFormat Format => _context.PixelFormat;

        protected override bool DelayCreation => false;

        public void Render(Visual visual)
        {
            WritePreamble();
            _context.Render(visual);
        }

        #region Clone

        public new RenderTargetBitmap Clone()
        {
            return (RenderTargetBitmap)base.Clone();
        }

        public new RenderTargetBitmap CloneCurrentValue()
        {
            return (RenderTargetBitmap)base.CloneCurrentValue();
        }

        protected override Freezable CreateInstanceCore()
        {
            if (IsFrozen)
                return new RenderTargetBitmap(_context, _dpiX, _dpiY);
            var newBitmap = new RenderTargetBitmap(_context.Width, _context.Height, _dpiX, _dpiY, _context.PixelFormat);
            newBitmap._context.CopyPixels(_context);
            return newBitmap;
        }

        #endregion
    }
}
