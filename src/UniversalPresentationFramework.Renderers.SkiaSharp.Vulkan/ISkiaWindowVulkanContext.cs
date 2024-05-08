using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Vulkan;

namespace Wodsoft.UI.Renderers
{
    public interface ISkiaWindowVulkanContext : ISkiaWindowContext
    {
        VkSurfaceKHR GetSurface(in VkInstance instance);
    }
}
