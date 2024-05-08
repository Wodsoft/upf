using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media.Imaging;

namespace Wodsoft.UI.Media
{
    public interface IImageContext
    {
        int Width { get; }

        int Height { get; }

        PixelFormat PixelFormat { get; }

        Rotation Rotation { get; set; }

        void CopyPixels(Int32Rect sourceRect, IntPtr buffer, int bufferSize, int stride);

        void CopyPixels(IImageContext imageContext);
    }
}
