using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;
using Wodsoft.UI.Media.Imaging;
using Wodsoft.UI.Providers;

namespace Wodsoft.UI.Renderers
{
    public class SkiaRendererProvider : IRendererProvider
    {
        public IBitmapContext CreateBitmapContext(int pixelWidth, int pixelHeight, double dpiX, double dpiY, PixelFormat pixelFormat, BitmapPalette? palette)
        {
            if (pixelFormat.IsPalettized)
                throw new NotSupportedException("Skia do not support palettized pixel format.");
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
            if (context is SkiaImageContext skiaImageContext)
                return new SkiaBitmapContext(SKBitmap.FromImage(skiaImageContext.Image));
            else if (context is SkiaBitmapContext skiaBitmapContext)
                return new SkiaBitmapContext(skiaBitmapContext.Bitmap.Copy());
            else
                throw new NotSupportedException("Only support skia image context.");
        }

        public VisualDrawingContext CreateDrawingContext(Visual visual)
        {
            return new SkiaDrawingContext(0, 0);
        }

        public StreamGeometryContext CreateGeometryContext()
        {
            return new SkiaGeometryContext();
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

        public IRenderBitmapContext CreateRenderBitmapContext(int pixelWidth, int pixelHeight, double dpiX, double dpiY, PixelFormat pixelFormat)
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
            return new SkiaRenderBitmapContext(new SKBitmap(info));
        }
    }
}
