using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Renderers;
using Windows.Win32;
using Windows.Win32.Graphics.Gdi;
using SkiaSharp;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Wodsoft.UI.Media;
using Windows.Win32.Foundation;

namespace Wodsoft.UI.Platforms.Win32
{
    public sealed class Win32RendererSoftwareContext : SkiaRendererSoftwareContext
    {
        private readonly HWND _hwnd;
        private nint _buffer;
        private int _width, _height;

        internal Win32RendererSoftwareContext(HWND hwnd)
        {
            _hwnd = hwnd;
        }

        private static unsafe int _HeaderSize = sizeof(BITMAPINFOHEADER);

        protected override bool ShouldCreateNewSurface(int width, int height)
        {
            return width != _width || height != _height;
        }

        protected override unsafe SKSurface CreateSurface(int width, int height)
        {
            if (_buffer != default)
                Marshal.FreeHGlobal(_buffer);
            _width = width;
            _height = height;
            var size = _HeaderSize + width * height * sizeof(uint);
            _buffer = Marshal.AllocHGlobal(size);
            ref BITMAPINFO bitmapInfo = ref Unsafe.AsRef<BITMAPINFO>(_buffer.ToPointer());
            bitmapInfo.bmiHeader.biSize = (uint)_HeaderSize;
            bitmapInfo.bmiHeader.biPlanes = 1;
            bitmapInfo.bmiHeader.biWidth = width;
            bitmapInfo.bmiHeader.biHeight = -height;
            bitmapInfo.bmiHeader.biBitCount = 32;
            bitmapInfo.bmiHeader.biCompression = 0;
            return SKSurface.Create(new SKImageInfo
            {
                Width = width,
                Height = height,
                ColorType = SKColorType.Bgra8888,
                AlphaType = SKAlphaType.Premul
            }, _buffer + _HeaderSize);
        }

        public unsafe override void Render(Visual visual)
        {
            base.Render(visual);
            var width = _width;
            var height = _height;
            var hdc = PInvoke.GetDC(_hwnd);
            PInvoke.StretchDIBits(hdc, 0, 0, width, height, 0, 0, width, height, (_buffer + _HeaderSize).ToPointer(), in Unsafe.AsRef<BITMAPINFO>(_buffer.ToPointer()), 0, ROP_CODE.SRCCOPY);
            PInvoke.ReleaseDC(_hwnd, hdc);
        }

        protected override void DisposeCore(bool disposing)
        {
            if (disposing)
            {
                if (_buffer != default)
                {
                    Marshal.FreeHGlobal(_buffer);
                    _buffer = default;
                }
            }
        }
    }
}
