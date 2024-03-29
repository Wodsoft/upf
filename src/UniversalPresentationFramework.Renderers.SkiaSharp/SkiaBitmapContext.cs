﻿using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Renderers
{
    public class SkiaBitmapContext : IBitmapContext
    {
        private readonly SKBitmap _bitmap;
        private readonly PixelFormat _pixelFormat;
        private SKPixmap? _pixmap;
        private object _lock = new object();

        public SkiaBitmapContext(SKBitmap bitmap)
        {
            _bitmap = bitmap;
            _pixelFormat = SkiaHelper.GetPixelFormat(bitmap.ColorType, bitmap.AlphaType, bitmap.ColorSpace);
        }

        public SKBitmap Bitmap => _bitmap;

        public int Width => _bitmap.Width;

        public int Height => _bitmap.Height;

        public PixelFormat PixelFormat => _pixelFormat;

        public nint BackBuffer => _bitmap.GetPixels();

        public int BackBufferStride => _bitmap.RowBytes;

        public void Lock()
        {
            lock (_lock)
            {
                if (_pixmap != null)
                    return;
                _pixmap = _bitmap.PeekPixels();
            }
        }

        public void Unlock()
        {
            lock (_lock)
            {
                if (_pixmap == null)
                    return;
                _pixmap.Dispose();
            }
        }

        public void CopyPixels(Int32Rect sourceRect, nint buffer, int bufferSize, int stride)
        {
            if (sourceRect.X < 0)
                throw new ArgumentOutOfRangeException("Source rectangle x can not be negative.");
            if (sourceRect.X >= _bitmap.Width)
                throw new ArgumentOutOfRangeException("Source rectangle x can not large or equal than bitmap width.");
            if (sourceRect.Y < 0)
                throw new ArgumentOutOfRangeException("Source rectangle y can not be negative.");
            if (sourceRect.Y >= _bitmap.Height)
                throw new ArgumentOutOfRangeException("Source rectangle y can not large or equal than bitmap width.");

            if (stride * (sourceRect.Height - 1) + sourceRect.Width * _bitmap.BytesPerPixel > bufferSize)
                throw new ArgumentOutOfRangeException(nameof(bufferSize), "Buffer size less than pxiels length.");
            var imageInfo = new SKImageInfo(sourceRect.Width, sourceRect.Height);
            imageInfo.ColorSpace = _bitmap.ColorSpace;
            imageInfo.AlphaType = _bitmap.AlphaType;
            imageInfo.ColorType = _bitmap.ColorType;
            using (var pixmap = new SKPixmap(imageInfo, buffer, stride))
            {
                _bitmap.PeekPixels().ReadPixels(pixmap, sourceRect.X, sourceRect.Y);
            }
        }

        public unsafe void WritePixels(Int32Rect sourceRect, nint sourceBuffer, int sourceBufferSize, int sourceBufferStride, int destinationX, int destinationY)
        {
            lock (_lock)
            {
                if (_pixmap == null)
                    throw new InvalidOperationException("Pixels is not lock.");
                if (destinationX + sourceRect.Width > _bitmap.Width)
                    throw new ArgumentOutOfRangeException(nameof(destinationX), "Source rect can not write to bitmap because destination x is out of range.");
                if (destinationY + sourceRect.Height > _bitmap.Height)
                    throw new ArgumentOutOfRangeException(nameof(destinationX), "Source rect can not write to bitmap because destination x is out of range.");
                SKPixmap pixmap;
                if (destinationX == 0 && destinationY == 0 && sourceRect.Width == _bitmap.Width && sourceRect.Height == _bitmap.Height)
                    pixmap = _pixmap;
                else
                    pixmap = _pixmap.ExtractSubset(new SKRectI(destinationX, destinationY, destinationX + sourceRect.Width, destinationY + sourceRect.Height));

                var availableLength = sourceBufferStride * (sourceRect.Height - 1) + sourceRect.Width * _bitmap.BytesPerPixel;
                if (availableLength > sourceBufferSize)
                    throw new ArgumentOutOfRangeException(nameof(sourceBufferSize), "Source buffer size less than pxiels length.");
                var dest = pixmap.GetPixels();
                var availableBytesPerRow = (uint)(sourceRect.Width * _bitmap.BytesPerPixel);
                if (pixmap.RowBytes == sourceBufferStride && availableBytesPerRow == sourceBufferStride)
                {
                    Unsafe.CopyBlock((void*)dest, (void*)sourceBuffer, (uint)availableLength);
                }
                else
                {
                    var destStride = pixmap.RowBytes;
                    uint bytesPerPixel = (uint)pixmap.Info.BytesPerPixel;
                    uint width = (uint)sourceRect.Width;
                    uint height = (uint)sourceRect.Height;
                    for (int y = 0; y < height; y++)
                    {
                        Unsafe.CopyBlock((void*)(dest + y * destStride), (void*)(sourceBuffer + y * sourceBufferStride), availableBytesPerRow);
                    }
                }
                pixmap.Dispose();
            }
        }

        public void CopyPixels(IImageContext imageContext)
        {
            if (imageContext is not SkiaImageContext skiaImageContext)
                throw new NotSupportedException("Only support copy pixels from skia image context.");
            var sourcePixmap = skiaImageContext.Image.PeekPixels();
            var destPixmap = _bitmap.PeekPixels();
            destPixmap.ReadPixels(sourcePixmap);
            destPixmap.Dispose();
        }
    }
}
