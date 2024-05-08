using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Platforms.Win32
{
    public enum Win32RendererContextType : byte
    {
        Software = 0,
        Direct3D12 = 1,
        Vulkan = 2,
        OpenGL = 3
    }
}
