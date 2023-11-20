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
    public class SkiaRendererProvider : IRendererProvider
    {
        public IBitmapContext CreateBitmapContext(int pixelWidth, int pixelHeight, double dpiX, double dpiY, PixelFormat pixelFormat, BitmapPalette palette)
        {
            SKImageInfo info = new SKImageInfo();
            info.Width = pixelWidth;
            info.Height = pixelHeight;
            info.ColorType = SkiaHelper.GetColorType(pixelFormat);
            info.AlphaType = pixelFormat.IsPremultiplied ? SKAlphaType.Premul : SKAlphaType.Opaque;
            switch (pixelFormat.ColorSpace)
            {
                case PixelFormatColorSpace.IsSRGB:
                case PixelFormatColorSpace.IsScRGB:
                    info.ColorSpace = pixelFormat.ColorSpace == PixelFormatColorSpace.IsSRGB ? SKColorSpace.CreateSrgb() : SKColorSpace.CreateSrgbLinear();
                    break;
            }
            return new SkiaBitmapContext(new SKBitmap(info));
        }

        public IBitmapContext CreateBitmapContext(IImageContext context)
        {
            throw new NotImplementedException();
        }

        public VisualDrawingContext CreateDrawingContext(Visual visual)
        {
            return new SkiaDrawingContext(0, 0);
        }

        public IImageContext CreateImageContext(Stream stream, int newWidth, int newHeight, Rotation rotation)
        {
            var image = SKImage.FromEncodedData(stream);
            if (newWidth != 0 || newHeight != 0)
            {
                if (newWidth == 0)
                    newWidth = newHeight * image.Width / image.Height;
                if (newHeight == 0)
                    newHeight = newWidth * image.Height / image.Width;
                var bitmap = SKBitmap.FromImage(image);
                image.Dispose();
                var resizedBitmap = bitmap.Resize(new SKSizeI { Width = newWidth, Height = newHeight }, SKFilterQuality.High);
                bitmap.Dispose();
                image = SKImage.FromBitmap(resizedBitmap);
            }
            return new SkiaImageContext(image, rotation);
        }
    }
}
