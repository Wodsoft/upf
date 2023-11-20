using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Imaging
{
    public abstract class BitmapSource : ImageSource
    {
        public abstract int PixelWidth { get; }

        public abstract int PixelHeight { get; }

        public override float Width => PixelWidth;

        public override float Height => PixelHeight;

        public virtual bool IsDownloading { get; } = false;

        protected abstract bool DelayCreation { get; }

        public abstract PixelFormat Format { get; }

        public event EventHandler? DownloadCompleted;

        public event EventHandler<ExceptionEventArgs>? DecodeFailed;

        public event EventHandler<ExceptionEventArgs>? DownloadFailed;

        public event EventHandler<DownloadProgressEventArgs>? DownloadProgress;

        protected void OnDownloadCompleted()
        {
            DownloadCompleted?.Invoke(this, EventArgs.Empty);
        }

        protected void OnDecodeFailed(Exception exception)
        {
            DecodeFailed?.Invoke(this, new ExceptionEventArgs(exception));
        }

        protected void OnDownloadFailed(Exception exception)
        {
            DownloadFailed?.Invoke(this, new ExceptionEventArgs(exception));
        }

        protected void OnDownloadProgress(int progress)
        {
            if (progress < 0)
                progress = 0;
            if (progress > 100)
                progress = 100;
            DownloadProgress?.Invoke(this, new DownloadProgressEventArgs(progress));
        }

        internal virtual void EnsureCreation()
        {

        }

        public virtual void CopyPixels(Int32Rect sourceRect, IntPtr buffer, int bufferSize, int stride)
        {
            if (sourceRect == Int32Rect.Empty)
                sourceRect = new Int32Rect(0, 0, PixelWidth, PixelHeight);
            Context.CopyPixels(sourceRect, buffer, bufferSize, stride);
        }

        public virtual void CopyPixels(Int32Rect sourceRect, Array pixels, int stride, int offset)
        {
            var elementSize = Marshal.SizeOf(pixels.GetValue(0)!);
            if (offset >= pixels.Length)
                throw new ArgumentOutOfRangeException(nameof(offset), "Offset is large or equal than pixels array.");
            int bufferSize = (pixels.Length - offset) * elementSize;
            GCHandle arrayHandle = GCHandle.Alloc(pixels, GCHandleType.Pinned);
            try
            {
                IntPtr buffer = arrayHandle.AddrOfPinnedObject();
                buffer = new IntPtr(((long)buffer) + (long)offset * elementSize);
                CopyPixels(sourceRect, buffer, bufferSize, stride);
            }
            finally
            {
                arrayHandle.Free();
            }
        }

        public virtual void CopyPixels(Array pixels, int stride, int offset)
        {
            var elementSize = Marshal.SizeOf(pixels.GetValue(0)!);
            if (offset >= pixels.Length)
                throw new ArgumentOutOfRangeException(nameof(offset), "Offset is large or equal than pixels array.");
            int bufferSize = (pixels.Length - offset) * elementSize;
            GCHandle arrayHandle = GCHandle.Alloc(pixels, GCHandleType.Pinned);
            try
            {
                IntPtr buffer = arrayHandle.AddrOfPinnedObject();
                buffer = new IntPtr(((long)buffer) + (long)offset * elementSize);
                CopyPixels(new Int32Rect(0, 0, PixelWidth, PixelHeight), buffer, bufferSize, stride);
            }
            finally
            {
                arrayHandle.Free();
            }

        }
    }
}
