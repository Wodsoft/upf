using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Vulkan;

namespace Wodsoft.UI.Renderers
{
    internal static class VulkanHelper
    {
        public static VkFormat GetFormat(SKColorType colorType)
        {
            switch (colorType)
            {
                case SKColorType.Rgb565:
                    return VkFormat.R5G6B5UnormPack16;
                case SKColorType.Argb4444:
                    return VkFormat.R4G4B4A4UnormPack16;
                case SKColorType.Rgba8888:
                    return VkFormat.R8G8B8A8Unorm;
                case SKColorType.Rgb888x:
                    return VkFormat.R8G8B8Unorm;
                case SKColorType.Bgra8888:
                    return VkFormat.B8G8R8A8Unorm;
                case SKColorType.Rgba1010102:
                    return VkFormat.A2R10G10B10UnormPack32;
                case SKColorType.RgbaF16:
                    return VkFormat.R16G16B16A16Sfloat;
                case SKColorType.RgbaF32:
                    return VkFormat.R32G32B32A32Sfloat;
                case SKColorType.Rg88:
                    return VkFormat.R8G8Unorm;
                case SKColorType.RgF16:
                    return VkFormat.R16G16Sfloat;
                case SKColorType.Rg1616:
                    return VkFormat.R16G16Sfloat;
                case SKColorType.Rgba16161616:
                    return VkFormat.R16G16B16A16Unorm;
                case SKColorType.Bgra1010102:
                    return VkFormat.A2B10G10R10UnormPack32;
                case SKColorType.Srgba8888:
                    return VkFormat.R8G8B8A8Snorm;
                case SKColorType.R8Unorm:
                    return VkFormat.R8Unorm;
                case SKColorType.Unknown:
                case SKColorType.Rgb101010x:
                case SKColorType.Gray8:
                case SKColorType.RgbaF16Clamped:
                case SKColorType.AlphaF16:
                case SKColorType.Alpha16:
                case SKColorType.Bgr101010x:
                case SKColorType.Bgr101010xXR:
                case SKColorType.Alpha8:
                default:
                    throw new NotSupportedException($"Vulkan does not support color type \"{colorType}\".");
            }
        }

        public static VkColorSpaceKHR GetColorSpace(SKColorSpace colorSpace)
        {
            if (colorSpace.ToColorSpaceXyz(out var xyz))
            {
                if (xyz == SKColorSpaceXyz.Srgb)
                    if (colorSpace.GammaIsLinear)
                        return VkColorSpaceKHR.ExtendedSrgbLinearEXT;
                    else
                        return VkColorSpaceKHR.ExtendedSrgbNonLinearEXT;
                else if (xyz == SKColorSpaceXyz.AdobeRgb)
                    if (colorSpace.GammaIsLinear)
                        return VkColorSpaceKHR.AdobeRgbLinearEXT;
                    else
                        return VkColorSpaceKHR.AdobeRgbNonLinearEXT;
                else if (xyz == SKColorSpaceXyz.DisplayP3)
                {
                    if (colorSpace.GammaIsLinear)
                        return VkColorSpaceKHR.DisplayP3LinearEXT;
                    else
                        return VkColorSpaceKHR.DisplayP3NonLinearEXT;
                }
                else if (xyz == SKColorSpaceXyz.Rec2020)
                    if (colorSpace.GammaIsLinear)
                        return VkColorSpaceKHR.Bt2020LinearEXT;
            }
            else if (colorSpace.IsSrgb)
            {
                if (colorSpace.GammaIsLinear)
                    return VkColorSpaceKHR.ExtendedSrgbLinearEXT;
                else
                    return VkColorSpaceKHR.ExtendedSrgbNonLinearEXT;
            }
            throw new NotSupportedException($"Vulkan not support color space \"{colorSpace}\"");
        }

        public static bool Any<T>(this ReadOnlySpan<T> span, Func<T, bool> predicate)
        {
            for (int i = 0; i < span.Length; i++)
            {
                if (predicate(span[i]))
                    return true;
            }
            return false;
        }

        public static int FirstIndex<T>(this ReadOnlySpan<T> span, Func<T, bool> predicate)
        {
            for (int i = 0; i < span.Length; i++)
            {
                if (predicate(span[i]))
                    return i;
            }
            return -1;
        }

        public static bool Contains<T>(this ReadOnlySpan<T> span, T value)
        {
            for (int i = 0; i < span.Length; i++)
            {
                if (Equals(span[i], value))
                    return true;
            }
            return false;
        }

        public static IEnumerable<TResult> Select<T, TResult>(this ReadOnlySpan<T> span, Func<T, TResult> selector)
        {
            List<TResult> results = new List<TResult>();
            for (int i = 0; i < span.Length; i++)
            {
                results.Add(selector(span[i]));
            }
            return results;
        }

        public static bool IsSwapChainSupport(this VkPhysicalDevice physicalDevice, uint queueFamilyIndex, VkSurfaceKHR surface)
        {
            var result = Vulkan.vkGetPhysicalDeviceSurfaceSupportKHR(physicalDevice, queueFamilyIndex, surface, out var isSupport);
            result.CheckResult();
            return isSupport;
        }
    }
}
