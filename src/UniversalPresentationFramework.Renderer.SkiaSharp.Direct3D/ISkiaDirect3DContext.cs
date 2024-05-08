using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D12;
using Vortice.DXGI;

namespace Wodsoft.UI.Renderers
{
    public interface ISkiaDirect3DContext
    {
        IDXGIAdapter1 Adapter { get; }

        ID3D12Device2 Device { get; }

        IDXGIFactory4 Factory { get; }
    }
}
