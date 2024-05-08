using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Renderers;

namespace Wodsoft.UI.Platforms.Win32
{
    public class Win32WindowContext : ISkiaWindowContext
    {
        private readonly WindowContext _windowContext;

        public Win32WindowContext(WindowContext windowContext)
        {
            _windowContext = windowContext;
        }

        protected WindowContext WindowContext => _windowContext;

        public int Width => _windowContext.ClientWidth;

        public int Height => _windowContext.ClientHeight;

        public SKAlphaType AlphaType => SKAlphaType.Premul;

        public SKColorType ColorType => SKColorType.Rgba8888;

        public SKColorSpace ColorSpace => SKColorSpace.CreateSrgb();

        public virtual void Dispose()
        {

        }
    }
}
