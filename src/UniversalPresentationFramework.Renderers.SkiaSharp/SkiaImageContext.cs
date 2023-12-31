﻿using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;
using Wodsoft.UI.Media.Imaging;

namespace Wodsoft.UI.Renderers
{
    public class SkiaImageContext : IImageContext
    {
        private readonly SKImage _image;
        private readonly PixelFormat _pixelFormat;

        public SkiaImageContext(SKImage image, Rotation rotation)
        {
            _image = image ?? throw new ArgumentNullException(nameof(image));
            Rotation = rotation;
            _pixelFormat = SkiaHelper.GetPixelFormat(image.ColorType, image.AlphaType, image.ColorSpace);
        }

        public SKImage Image => _image;

        public int Width => _image.Width;

        public int Height => _image.Height;

        public Rotation Rotation { get; }

        public PixelFormat PixelFormat { get; private set; }

        public void CopyPixels(Int32Rect sourceRect, nint buffer, int bufferSize, int stride)
        {
            if (sourceRect.X < 0)
                throw new ArgumentOutOfRangeException("Source rectangle x can not be negative.");
            if (sourceRect.X >= _image.Width)
                throw new ArgumentOutOfRangeException("Source rectangle x can not large or equal than image width.");
            if (sourceRect.Y < 0)
                throw new ArgumentOutOfRangeException("Source rectangle y can not be negative.");
            if (sourceRect.Y >= _image.Height)
                throw new ArgumentOutOfRangeException("Source rectangle y can not large or equal than image width.");

            if (stride * (sourceRect.Height -1) + sourceRect.Width * _image.Info.BytesPerPixel > bufferSize)
                throw new ArgumentOutOfRangeException(nameof(bufferSize), "Buffer size less than pxiels length.");
            var imageInfo = new SKImageInfo(sourceRect.Width, sourceRect.Height);
            imageInfo.ColorSpace = _image.ColorSpace;
            imageInfo.AlphaType = _image.AlphaType;
            imageInfo.ColorType = _image.ColorType;
            using (var pixmap = new SKPixmap(imageInfo, buffer, stride))
            {
                _image.ReadPixels(pixmap, sourceRect.X, sourceRect.Y);
            }
        }
    }
}
