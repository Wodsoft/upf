using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Renderers
{
    public interface ISkiaWindowDirect3DContext : ISkiaWindowContext
    {
        IntPtr WindowHandle { get; }

        int BufferCount { get; }

        int SampleCount { get; }
    }
}
