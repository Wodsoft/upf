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
        public virtual IBitmapContext CreateBitmapContext(int pixelWidth, int pixelHeight, float dpiX, float dpiY, PixelFormat pixelFormat, BitmapPalette? palette)
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

        public virtual IBitmapContext CreateBitmapContext(IImageContext context)
        {
            if (context is SkiaImageContext skiaImageContext)
                return new SkiaBitmapContext(SKBitmap.FromImage(skiaImageContext.Image));
            else if (context is SkiaBitmapContext skiaBitmapContext)
                return new SkiaBitmapContext(skiaBitmapContext.Bitmap.Copy());
            else
                throw new NotSupportedException("Only support skia image context.");
        }

        public virtual VisualDrawingContext CreateDrawingContext(Visual visual)
        {
            return new SkiaDrawingContext(0, 0);
        }

        public virtual StreamGeometryContext CreateGeometryContext()
        {
            return new SkiaGeometryContext();
        }

        public virtual GlyphTypeface? CreateGlyphTypeface(string familyName, FontStyle style, FontWeight weight, FontStretch stretch)
        {
            return SkiaGlyphTypeface.Create(familyName, style, weight, stretch);
        }

        public virtual IImageContext CreateImageContext(Stream stream, int newWidth, int newHeight, Rotation rotation)
        {
            var image = SKImage.FromEncodedData(stream);
            if (newWidth != 0 && newWidth != image.Width || newHeight != 0 && newHeight != image.Height)
            {
                if (newWidth == 0)
                    newWidth = newHeight * image.Width / image.Height;
                if (newHeight == 0)
                    newHeight = newWidth * image.Height / image.Width;
                var bitmap = SKBitmap.FromImage(image);
                image.Dispose();
                var resizedBitmap = bitmap.Resize(new SKSizeI { Width = newWidth, Height = newHeight }, new SKSamplingOptions());
                bitmap.Dispose();
                image = SKImage.FromBitmap(resizedBitmap);
            }
            return new SkiaImageContext(image, rotation);
        }

        public virtual IRenderBitmapContext CreateRenderBitmapContext(int pixelWidth, int pixelHeight, float dpiX, float dpiY, PixelFormat pixelFormat)
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
            var rendererContext = new SkiaTextureRendererSoftwareContext(info);
            return new SkiaRenderBitmapContext(rendererContext, pixelFormat);
        }
    }
}
