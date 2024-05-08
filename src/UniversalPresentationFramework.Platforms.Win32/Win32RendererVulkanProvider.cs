using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Vulkan;
using Wodsoft.UI.Renderers;

namespace Wodsoft.UI.Platforms.Win32
{
    public class Win32RendererVulkanProvider : SkiaRendererVulkanProvider
    {
        protected Win32RendererVulkanProvider() { }

        public static bool TryCreate([NotNullWhen(true)] out Win32RendererVulkanProvider? provider)
        {
            try
            {
                Vulkan.vkInitialize();
                var result = Vulkan.vkEnumerateInstanceVersion();
            }
            catch
            {
                provider = null;
                return false;
            }
            provider = new Win32RendererVulkanProvider();
            return true;
        }

        protected override string[] GetInstanceExtensions()
        {
            return [Vulkan.VK_KHR_SURFACE_EXTENSION_NAME, Vulkan.VK_KHR_WIN32_SURFACE_EXTENSION_NAME, Vulkan.VK_EXT_DEBUG_REPORT_EXTENSION_NAME];
        }
    }
}
