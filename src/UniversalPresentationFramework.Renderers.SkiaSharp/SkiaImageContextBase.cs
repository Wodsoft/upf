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
    public abstract class SkiaImageContextBase : IImageContext, ISkiaImageContext
    {
        public virtual int Width => Image.Width;

        public virtual int Height => Image.Height;

        public abstract PixelFormat PixelFormat { get; }

        public virtual Rotation Rotation { get; set; }

        public abstract SKImage Image { get; }

        public virtual void CopyPixels(Int32Rect sourceRect, nint buffer, int bufferSize, int stride)
        {
            if (sourceRect.X < 0)
                throw new ArgumentOutOfRangeException("Source rectangle x can not be negative.");
            if (sourceRect.X >= Image.Width)
                throw new ArgumentOutOfRangeException("Source rectangle x can not large or equal than image width.");
            if (sourceRect.Y < 0)
                throw new ArgumentOutOfRangeException("Source rectangle y can not be negative.");
            if (sourceRect.Y >= Image.Height)
                throw new ArgumentOutOfRangeException("Source rectangle y can not large or equal than image width.");

            if (stride * (sourceRect.Height - 1) + sourceRect.Width * Image.Info.BytesPerPixel > bufferSize)
                throw new ArgumentOutOfRangeException(nameof(bufferSize), "Buffer size less than pxiels length.");
            var imageInfo = new SKImageInfo(sourceRect.Width, sourceRect.Height);
            imageInfo.ColorSpace = Image.ColorSpace;
            imageInfo.AlphaType = Image.AlphaType;
            imageInfo.ColorType = Image.ColorType;
            using (var pixmap = new SKPixmap(imageInfo, buffer, stride))
            {
                Image.ReadPixels(pixmap, sourceRect.X, sourceRect.Y);
            }
        }

        public virtual void CopyPixels(IImageContext imageContext)
        {
            if (imageContext is not ISkiaImageContext skiaImageContext)
                throw new NotSupportedException("Only support copy pixels from skia image context.");
            var sourcePixmap = skiaImageContext.Image.PeekPixels();
            var destPixmap = Image.PeekPixels();
            destPixmap.ReadPixels(sourcePixmap);
            destPixmap.Dispose();
        }
    }
}
