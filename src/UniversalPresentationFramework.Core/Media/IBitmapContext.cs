using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    public interface IBitmapContext : IImageContext
    {
        IntPtr BackBuffer { get; }

        int BackBufferStride { get; }

        void Lock();

        void Unlock();

        void WritePixels(Int32Rect sourceRect, IntPtr sourceBuffer, int sourceBufferSize, int sourceBufferStride, int destinationX, int destinationY);

        void CopyPixels(IImageContext imageContext);
    }
}
