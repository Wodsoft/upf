using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Renderers
{
    public interface ISkiaWindowOpenGLContext : ISkiaWindowContext
    {
        GRContext GRContext { get; }
        void MakeCurrent();
        void SwapBuffers();
        int GetInteger(uint index);
    }
}
