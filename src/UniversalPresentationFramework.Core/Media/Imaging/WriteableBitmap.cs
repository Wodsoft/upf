using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Wodsoft.UI.Media.Imaging
{
    public sealed class WriteableBitmap : BitmapSource
    {
        private IBitmapContext _context;

        private WriteableBitmap(IBitmapContext context)
        {
            _context = context;
        }

        public WriteableBitmap(BitmapSource source)
        {
            if (FrameworkCoreProvider.RendererProvider == null)
                throw new InvalidOperationException("Framework not initialized.");
            if (source.Context == null)
                throw new ArgumentException("Bitmap source not initialized.");
            _context = FrameworkCoreProvider.RendererProvider.CreateBitmapContext(source.Context);
        }

        public WriteableBitmap(int pixelWidth, int pixelHeight, double dpiX, double dpiY, PixelFormat pixelFormat, BitmapPalette palette)
        {
            if (FrameworkCoreProvider.RendererProvider == null)
                throw new InvalidOperationException("Framework not initialized.");
            _context = FrameworkCoreProvider.RendererProvider.CreateBitmapContext(pixelWidth, pixelHeight, dpiX, dpiY, pixelFormat, palette);
        }

        public override int PixelWidth => _context.Width;

        public override int PixelHeight => _context.Height;

        public override IImageContext Context => _context;

        public override PixelFormat Format => _context.PixelFormat;

        protected override bool DelayCreation => false;

        public void Lock()
        {
            WritePreamble();
            _context.Lock();
        }

        public void Unlock()
        {
            _context.Unlock();
        }

        public void WritePixels(Int32Rect sourceRect, Array pixels, int stride, int offset)
        {
            WritePreamble();
            if (pixels == null)
                throw new ArgumentNullException(nameof(pixels));
            if (pixels.Length == 0)
                throw new ArgumentException("Pixels array is empty.");
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), "Offset can not be negative.");
            var elementSize = Marshal.SizeOf(pixels.GetValue(0)!);
            if (offset >= pixels.Length)
                throw new ArgumentOutOfRangeException(nameof(offset), "Offset is large or equal than pixels array.");
            int bufferSize = (pixels.Length - offset) * elementSize;
            GCHandle arrayHandle = GCHandle.Alloc(pixels, GCHandleType.Pinned);
            try
            {
                IntPtr buffer = arrayHandle.AddrOfPinnedObject();
                buffer = new IntPtr(((long)buffer) + (long)offset * elementSize);
                WritePixels(sourceRect, buffer, bufferSize, stride);
            }
            finally
            {
                arrayHandle.Free();
            }
        }

        public void WritePixels(Int32Rect sourceRect, Array sourceBuffer, int sourceBufferStride, int destinationX, int destinationY)
        {
            WritePreamble();
            if (sourceBuffer == null)
                throw new ArgumentNullException(nameof(sourceBuffer));
            if (sourceBuffer.Length == 0)
                throw new ArgumentException("Pixels array is empty.");
            var elementSize = Marshal.SizeOf(sourceBuffer.GetValue(0)!);
            int bufferSize = sourceBuffer.Length * elementSize;
            GCHandle arrayHandle = GCHandle.Alloc(sourceBuffer, GCHandleType.Pinned);
            try
            {
                IntPtr buffer = arrayHandle.AddrOfPinnedObject();
                WritePixels(sourceRect, buffer, bufferSize, sourceBufferStride);
            }
            finally
            {
                arrayHandle.Free();
            }
        }

        public void WritePixels(Int32Rect sourceRect, IntPtr buffer, int bufferSize, int stride)
        {
            WritePreamble();
            _context.WritePixels(sourceRect, buffer, bufferSize, stride, 0, 0);
        }

        public void WritePixels(Int32Rect sourceRect, IntPtr sourceBuffer, int sourceBufferSize, int sourceBufferStride, int destinationX, int destinationY)
        {
            WritePreamble();
            _context.WritePixels(sourceRect, sourceBuffer, sourceBufferSize, sourceBufferStride, destinationX, destinationY);
        }

        #region Clone

        public new WriteableBitmap Clone()
        {
            return (WriteableBitmap)base.Clone();
        }

        public new WriteableBitmap CloneCurrentValue()
        {
            return (WriteableBitmap)base.CloneCurrentValue();
        }

        protected override Freezable CreateInstanceCore()
        {
            if (IsFrozen)
                return new WriteableBitmap(_context);
            else
                return new WriteableBitmap(FrameworkCoreProvider.RendererProvider!.CreateBitmapContext(_context));
        }

        #endregion
    }
}
