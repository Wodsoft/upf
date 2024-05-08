using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Vulkan;
using Wodsoft.UI.Renderers;

namespace Wodsoft.UI.Platforms.Win32
{
    public class Win32WindowVulkanContext : Win32WindowContext, ISkiaWindowVulkanContext
    {
        private VkSurfaceKHR? _vulkanSurface;

        public Win32WindowVulkanContext(WindowContext windowContext) : base(windowContext)
        {
        }

        public unsafe VkSurfaceKHR GetSurface(in VkInstance instance)
        {
            if (_vulkanSurface == null)
            {
                var surfaceCreateInfo = new VkWin32SurfaceCreateInfoKHR
                {
                    hinstance = WindowContext.Instance.DangerousGetHandle(),
                    hwnd = WindowContext.Hwnd
                };
                VkSurfaceKHR surface = default;
                var result = Vulkan.vkCreateWin32SurfaceKHR(instance, &surfaceCreateInfo, null, &surface);
                result.CheckResult();
                _vulkanSurface = surface;
            }
            return _vulkanSurface.Value;
        }

        public override void Dispose()
        {
            
        }
    }
}
