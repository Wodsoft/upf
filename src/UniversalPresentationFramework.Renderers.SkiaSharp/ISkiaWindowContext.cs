using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Renderers
{
    public interface ISkiaWindowContext : IDisposable
    {
        int Width { get; }

        int Height { get; }

        SKAlphaType AlphaType { get; }

        SKColorType ColorType { get; }

        SKColorSpace ColorSpace { get; }
    }
}
