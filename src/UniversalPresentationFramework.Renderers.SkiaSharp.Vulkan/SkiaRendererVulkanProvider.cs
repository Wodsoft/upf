using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Vulkan;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Renderers
{
    public abstract class SkiaRendererVulkanProvider : SkiaRendererProvider, ISkiaVulkanContext, IComparer<(ulong, VkPhysicalDevice)>
    {
        protected abstract string[] GetInstanceExtensions();

        protected virtual string ApplicationName => "UPF Application";

        protected virtual VkVersion ApplicationVersion => new VkVersion(1, 0, 0);

        protected virtual string EngineName => "UPF";

        protected virtual VkVersion EngineVersion => new VkVersion(1, 0, 0);

        protected virtual VkVersion ApiVersion => new VkVersion(1, 0, 0);

        private VkInstance? _instance;
        public unsafe VkInstance Instance
        {
            get
            {
                if (_instance == null)
                {
                    var layerProperties = Vulkan.vkEnumerateInstanceLayerProperties();
                    string[] enabledLayers = layerProperties.Any(t => t.GetLayerName() == "VK_LAYER_LUNARG_standard_validation") ? ["VK_LAYER_LUNARG_standard_validation"] : [];
                    using VkString vkApplicationName = new VkString(ApplicationName);
                    using VkString vkEngineName = new VkString(EngineName);
                    VkApplicationInfo applicationInfo = new VkApplicationInfo
                    {
                        apiVersion = ApiVersion,
                        applicationVersion = ApplicationVersion,
                        engineVersion = EngineVersion,
                        pApplicationName = vkApplicationName,
                        pEngineName = vkEngineName
                    };
                    var instanceExtensions = GetInstanceExtensions();
                    using VkStringArray vkLayerNames = new(enabledLayers);
                    using VkStringArray vkInstanceExtensions = new VkStringArray(instanceExtensions);
                    VkInstanceCreateInfo createInfo = new VkInstanceCreateInfo
                    {
                        enabledExtensionCount = (uint)instanceExtensions.Length,
                        enabledLayerCount = (uint)enabledLayers.Length,
                        ppEnabledLayerNames = vkLayerNames,
                        pApplicationInfo = &applicationInfo,
                        ppEnabledExtensionNames = vkInstanceExtensions
                    };
                    var result = Vulkan.vkCreateInstance(&createInfo, null, out var instance);
                    result.CheckResult();
                    _instance = instance;
                    Vulkan.vkLoadInstanceOnly(instance);
                }
                return _instance.Value;
            }
        }

        private VkPhysicalDevice[]? _physicalDevices;
        public unsafe VkPhysicalDevice[] PhysicalDevices
        {
            get
            {
                if (_physicalDevices == null)
                {
                    var instance = Instance;
                    var physicalDevices = Vulkan.vkEnumeratePhysicalDevices(instance);
                    _physicalDevices = physicalDevices.ToArray();
                    //var orderedList = new SortedSet<(ulong, VkPhysicalDevice)>(this);
                    //VkResult result;
                    //foreach (var physicalDevice in physicalDevices)
                    //{
                    //    var extensionProperties = Vulkan.vkEnumerateDeviceExtensionProperties(physicalDevice);
                    //    if (extensionProperties.Any(t => t.GetExtensionName() == Vulkan.VK_KHR_PERFORMANCE_QUERY_EXTENSION_NAME))
                    //    {
                    //        var queues = Vulkan.vkGetPhysicalDeviceQueueFamilyProperties(physicalDevice);
                    //        var queueIndex = queues.FirstIndex(t => t.queueFlags.HasFlag(VkQueueFlags.Graphics));
                    //        if (queueIndex == -1)
                    //        {
                    //            orderedList.Add((0, physicalDevice));
                    //            continue;
                    //        }
                    //        var counterCount = 0u;
                    //        result = Vulkan.vkEnumeratePhysicalDeviceQueueFamilyPerformanceQueryCountersKHR(physicalDevice, (uint)queueIndex, &counterCount, null, null);
                    //        result.CheckResult();
                    //        if (counterCount == 0)
                    //        {
                    //            orderedList.Add((0, physicalDevice));
                    //            continue;
                    //        }
                    //        VkPerformanceCounterKHR[] counters = new VkPerformanceCounterKHR[counterCount];
                    //        VkPerformanceCounterDescriptionKHR[] counterDescriptions = new VkPerformanceCounterDescriptionKHR[counterCount];
                    //        fixed (VkPerformanceCounterKHR* countersPtr = &counters[0])
                    //        fixed (VkPerformanceCounterDescriptionKHR* descriptionPtr = &counterDescriptions[0])
                    //        {
                    //            result = Vulkan.vkEnumeratePhysicalDeviceQueueFamilyPerformanceQueryCountersKHR(physicalDevice, (uint)queueIndex, &counterCount, countersPtr, descriptionPtr);
                    //            result.CheckResult();
                    //        }
                    //        var counterIndex = new ReadOnlySpan<VkPerformanceCounterDescriptionKHR>(counterDescriptions).FirstIndex(t => !t.flags.HasFlag(Vulkan.VK_PERFORMANCE_COUNTER_DESCRIPTION_PERFORMANCE_IMPACTING_BIT_KHR));
                    //        if (counterIndex == -1)
                    //        {
                    //            counterIndex = 0;
                    //            //orderedList.Add((0, physicalDevice));
                    //            //continue;
                    //        }
                    //        //uint[] counterIndices = [(uint)counterIndex];
                    //        //counterCount = 1;
                    //        uint[] counterIndices = new uint[counterCount];
                    //        for (uint i = 1; i < counterCount; i++)
                    //            counterIndices[i] = i;
                    //        fixed (uint* counterIndicesPtr = &counterIndices[0])
                    //        {
                    //            var queryPoolPerformanceCreateInfo = new VkQueryPoolPerformanceCreateInfoKHR
                    //            {
                    //                counterIndexCount = (uint)counterIndices.Length,
                    //                queueFamilyIndex = (uint)queueIndex,
                    //                pCounterIndices = counterIndicesPtr
                    //            };
                    //            uint numPass = 0;
                    //            Vulkan.vkGetPhysicalDeviceQueueFamilyPerformanceQueryPassesKHR(physicalDevice, &queryPoolPerformanceCreateInfo, &numPass);
                    //            var queryPoolCreateInfo = new VkQueryPoolCreateInfo
                    //            {
                    //                pNext = &queryPoolPerformanceCreateInfo,
                    //                queryType = VkQueryType.PerformanceQueryKHR,
                    //                queryCount = 1
                    //            };
                    //            var priority = 1f;
                    //            var deviceQueueCreateInfo = new VkDeviceQueueCreateInfo
                    //            {
                    //                queueCount = 1,
                    //                queueFamilyIndex = (uint)queueIndex,
                    //                pQueuePriorities = &priority
                    //            };
                    //            var extensions = Vulkan.vkEnumerateDeviceExtensionProperties(physicalDevice).Select(t => t.GetExtensionName()).ToArray();
                    //            using var vkExtensions = new VkStringArray(extensions);
                    //            var deviceCreateInfo = new VkDeviceCreateInfo
                    //            {
                    //                pQueueCreateInfos = &deviceQueueCreateInfo,
                    //                queueCreateInfoCount = 1,
                    //                enabledExtensionCount = vkExtensions.Length,
                    //                ppEnabledExtensionNames = vkExtensions
                    //            };
                    //            result = Vulkan.vkCreateDevice(physicalDevice, &deviceCreateInfo, null, out var device);
                    //            result.CheckResult();
                    //            Vulkan.vkLoadDevice(device);
                    //            Vulkan.vkGetDeviceQueue(device, (uint)queueIndex, 0, out var queue);
                    //            result = Vulkan.vkCreateCommandPool(device, (uint)queueIndex, out var commandPool);
                    //            result.CheckResult();
                    //            var commandBufferAllocateInfo = new VkCommandBufferAllocateInfo
                    //            {
                    //                commandBufferCount = 1,
                    //                commandPool = commandPool,
                    //                level = VkCommandBufferLevel.Primary
                    //            };
                    //            result = Vulkan.vkAllocateCommandBuffer(device, &commandBufferAllocateInfo, out var commandBuffer);
                    //            result.CheckResult();
                    //            result = Vulkan.vkCreateQueryPool(device, &queryPoolCreateInfo, null, out var queryPool);
                    //            result.CheckResult();
                    //            var lockInfo = new VkAcquireProfilingLockInfoKHR
                    //            {
                    //                timeout = ulong.MaxValue
                    //            };
                    //            result = Vulkan.vkAcquireProfilingLockKHR(device, &lockInfo);
                    //            result.CheckResult();
                    //            result = Vulkan.vkBeginCommandBuffer(commandBuffer, VkCommandBufferUsageFlags.None);
                    //            result.CheckResult();
                    //            Vulkan.vkCmdBeginQuery(commandBuffer, queryPool, 0, VkQueryControlFlags.None);
                    //            Vulkan.vkCmdPipelineBarrier(commandBuffer, VkPipelineStageFlags.BottomOfPipe, VkPipelineStageFlags.BottomOfPipe, VkDependencyFlags.None, 0, null, 0, null, 0, null);
                    //            Vulkan.vkCmdEndQuery(commandBuffer, queryPool, 0);
                    //            Vulkan.vkEndCommandBuffer(commandBuffer);
                    //            VkPerformanceQuerySubmitInfoKHR[] performanceSubmitInfo = new VkPerformanceQuerySubmitInfoKHR[numPass];
                    //            fixed (VkPerformanceQuerySubmitInfoKHR* performanceSubmitInfoPtr = &performanceSubmitInfo[0])
                    //            {
                    //                for (int i = 0; i < numPass; i++)
                    //                {
                    //                    performanceSubmitInfo[i].counterPassIndex = (uint)i;
                    //                    if (i == numPass - 1)
                    //                        break;
                    //                    performanceSubmitInfo[i].pNext = performanceSubmitInfoPtr + i + 1;
                    //                }
                    //                var submitInfo = new VkSubmitInfo
                    //                {
                    //                    pCommandBuffers = &commandBuffer,
                    //                    commandBufferCount = 1,
                    //                    pNext = performanceSubmitInfoPtr
                    //                    //signalSemaphoreCount = 1,
                    //                    //pSignalSemaphores = &semaphore
                    //                };
                    //                Vulkan.vkQueueSubmit(queue, submitInfo, default);
                    //            }
                    //            Vulkan.vkQueueWaitIdle(queue);
                    //            Vulkan.vkReleaseProfilingLockKHR(device);
                    //            VkPerformanceCounterResultKHR[] counterResults = new VkPerformanceCounterResultKHR[counterCount];
                    //            fixed (VkPerformanceCounterResultKHR* counterResultPtr = &counterResults[0])
                    //            {
                    //                result = Vulkan.vkGetQueryPoolResults(device, queryPool, 0, 1, (nuint)sizeof(VkPerformanceCounterResultKHR) * counterCount, counterResultPtr, (nuint)sizeof(VkPerformanceCounterResultKHR) * counterCount, VkQueryResultFlags.Wait);
                    //                result.CheckResult();
                    //            }
                    //            Vulkan.vkDestroyQueryPool(device, queryPool);
                    //            Vulkan.vkFreeCommandBuffers(device, commandPool, commandBuffer);
                    //            Vulkan.vkDestroyCommandPool(device, commandPool);
                    //            Vulkan.vkDestroyDevice(device);
                    //            //ulong count;
                    //            //switch (counters[counterIndex].storage)
                    //            //{
                    //            //    case VkPerformanceCounterStorageKHR.Int32:
                    //            //        count = counterResult.int32 < 0 ? 0 : (ulong)counterResult.int32;
                    //            //        break;
                    //            //    case VkPerformanceCounterStorageKHR.Int64:
                    //            //        count = counterResult.int64 < 0 ? 0 : (ulong)counterResult.int64;
                    //            //        break;
                    //            //    case VkPerformanceCounterStorageKHR.Uint32:
                    //            //        count = counterResult.uint32;
                    //            //        break;
                    //            //    case VkPerformanceCounterStorageKHR.Uint64:
                    //            //        count = counterResult.uint64;
                    //            //        break;
                    //            //    case VkPerformanceCounterStorageKHR.Float32:
                    //            //        count = counterResult.float32 < 0 ? 0 : (ulong)counterResult.float32;
                    //            //        break;
                    //            //    case VkPerformanceCounterStorageKHR.Float64:
                    //            //        count = counterResult.float64 < 0 ? 0 : (ulong)counterResult.float64;
                    //            //        break;
                    //            //}
                    //        }
                    //    }
                    //}
                    //_physicalDevices = orderedList.Select(t => t.Item2).ToArray();
                }
                return _physicalDevices;
            }
        }

        int IComparer<(ulong, VkPhysicalDevice)>.Compare((ulong, VkPhysicalDevice) x, (ulong, VkPhysicalDevice) y)
        {
            if (x.Item1 == y.Item1)
                return 0;
            return x.Item1 > y.Item1 ? -1 : 1;
        }

        public override IRenderBitmapContext CreateRenderBitmapContext(int pixelWidth, int pixelHeight, double dpiX, double dpiY, PixelFormat pixelFormat)
        {
            SKColorSpace colorSpace;
            switch (pixelFormat.ColorSpace)
            {
                case PixelFormatColorSpace.IsSRGB:
                case PixelFormatColorSpace.IsScRGB:
                    colorSpace = pixelFormat.ColorSpace == PixelFormatColorSpace.IsSRGB ? SKColorSpace.CreateSrgb() : SKColorSpace.CreateSrgbLinear();
                    break;
                default:
                    colorSpace = SKColorSpace.CreateSrgb();
                    break;
            }
            var renderer = new SkiaTextureRendererVulkanContext(this, pixelWidth, pixelHeight, SkiaHelper.GetColorType(pixelFormat), pixelFormat.IsPremultiplied ? SKAlphaType.Premul : SKAlphaType.Opaque, colorSpace);
            return new SkiaRenderBitmapContext(renderer, pixelFormat);
        }
    }
}
