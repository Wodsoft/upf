using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Renderers;

namespace Wodsoft.UI.Platforms.Win32
{
    public class Win32WindowDirect3DContext : Win32WindowContext, ISkiaWindowDirect3DContext
    {

        public Win32WindowDirect3DContext(WindowContext windowContext) : base(windowContext) { }

        public nint WindowHandle => WindowContext.Hwnd;

        public int BufferCount => 2;

        public int SampleCount => 1;
    }
}
