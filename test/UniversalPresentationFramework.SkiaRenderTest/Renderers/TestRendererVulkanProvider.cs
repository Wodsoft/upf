using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Vulkan;
using Wodsoft.UI.Renderers;

namespace Wodsoft.UI.Test.Renderers
{
    public class TestRendererVulkanProvider : SkiaRendererVulkanProvider
    {
        protected TestRendererVulkanProvider() { }

        public static bool TryCreate([NotNullWhen(true)] out TestRendererVulkanProvider? provider)
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
            provider = new TestRendererVulkanProvider();
            return true;
        }

        protected override string[] GetInstanceExtensions()
        {
            return [Vulkan.VK_EXT_DEBUG_REPORT_EXTENSION_NAME];
        }
    }
}
