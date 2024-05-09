using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Vulkan;

namespace Wodsoft.UI.Renderers
{
    public class SkiaWindowRendererVulkanContext : SkiaWindowRendererContext
    {
        private readonly ISkiaVulkanContext _context;
        private readonly ISkiaWindowVulkanContext _windowContext;
        private GRContext? _grContext;
        private VkDevice? _device;
        private VkPhysicalDevice? _physicalDevice;
        private QueueFamilyIndices _queueFamilies;
        private GRBackendRenderTarget[]? _renderTargets;
        private VkSemaphore _renderFinishedSemaphore;
        private VkSwapchainKHR? _swapchain;
        private VkImage[]? _images;
        private VkQueue? _graphicsQueue, _presentQueue;
        //private VkFence[]? _fences;
        private uint _currentBufferIndex;

        public SkiaWindowRendererVulkanContext(ISkiaVulkanContext context, ISkiaWindowVulkanContext windowContext)
        {
            _context = context;
            _windowContext = windowContext;
        }

        public override ISkiaWindowContext WindowContext => _windowContext;

        public unsafe override GRContext GRContext
        {
            get
            {
                if (_grContext == null)
                {
                    var instance = _context.Instance;
                    var physicalDevice = PhysicalDevice;
                    var device = Device;
                    Vulkan.vkGetDeviceQueue(device, _queueFamilies.GraphicsFamily!.Value, 0, out var graphicsQueue);
                    _graphicsQueue = graphicsQueue;
                    Vulkan.vkGetDeviceQueue(device, _queueFamilies.PresentFamily!.Value, 0, out var presentQueue);
                    _presentQueue = presentQueue;
                    //var transferQueue = device.GetQueue(queueFamilies.TransferFamily!.Value, 0);

                    List<string> emptyAddress = new List<string>();
                    GRVkGetProcedureAddressDelegate getProc = (name, instanceHandle, deviceHandle) =>
                    {
                        nint address;
                        if (deviceHandle != default)
                            address = new nint(Vulkan.vkGetDeviceProcAddr(device, name));
                        else
                            address = new nint(Vulkan.vkGetInstanceProcAddr(instance, name));
                        if (address == default)
                            emptyAddress.Add(name);
                        return address;
                    };
                    var extensions = Vulkan.vkEnumerateDeviceExtensionProperties(physicalDevice).Select(t => t.GetExtensionName()).ToArray();
                    Vulkan.vkGetPhysicalDeviceFeatures(physicalDevice, out var features);
                    var backendContext = new GRVkBackendContext
                    {
                        VkInstance = instance,
                        VkPhysicalDevice = physicalDevice,
                        VkDevice = device,
                        VkQueue = graphicsQueue,
                        VkPhysicalDeviceFeatures = new nint(&features),
                        Extensions = GRVkExtensions.Create(getProc, instance, physicalDevice, null, extensions),
                        GraphicsQueueIndex = _queueFamilies.GraphicsFamily!.Value,
                        GetProcedureAddress = (name, innerInstance, innerDevice) =>
                        {
                            nint address;
                            if (innerInstance != default)
                                address = new nint(Vulkan.vkGetInstanceProcAddr(innerInstance, name));
                            else if (innerDevice != default)
                                address = new nint(Vulkan.vkGetDeviceProcAddr(innerDevice, name));
                            else
                                address = new nint(Vulkan.vkGetInstanceProcAddr(instance, name));
                            if (address == default)
                                emptyAddress.Add(name);
                            return address;
                        },
                        //ProtectedContext = false,
                        //MaxAPIVersion = 1 << 22
                    };
                    _grContext = GRContext.CreateVulkan(backendContext);
                    if (_grContext == null)
                    {
                        backendContext.MaxAPIVersion = 1 << 22;
                        _grContext = GRContext.CreateVulkan(backendContext);
                        if (_grContext == null)
                            throw new NotSupportedException($"Create Vulkan GRContext failed because missing functions. {string.Join(',', emptyAddress)}");
                    }
                }
                return _grContext;
            }
        }

        public override SKColorType ColorType => _windowContext.ColorType;

        public override SKColorSpace ColorSpace => _windowContext.ColorSpace;

        public override GRSurfaceOrigin SurfaceOrigin => GRSurfaceOrigin.TopLeft;

        protected override int CurrentBufferIndex
        {
            get
            {
                var result = Vulkan.vkAcquireNextImageKHR(_device!.Value, _swapchain!.Value, uint.MaxValue, default, default, out _currentBufferIndex);
                if (result != VkResult.SuboptimalKHR)
                    result.CheckResult();
                return (int)_currentBufferIndex;
            }
        }

        protected VkPhysicalDevice PhysicalDevice
        {
            get
            {
                if (_physicalDevice == null)
                {
                    var surface = _windowContext.GetSurface(_context.Instance);
                    _physicalDevice = _context.PhysicalDevices.First(t => IsSuitableDevice(t, surface));
                    _queueFamilies = FindQueueFamilies(_physicalDevice.Value, surface);
                }
                return _physicalDevice.Value;
            }
        }

        protected unsafe VkDevice Device
        {
            get
            {
                if (_device == null)
                {
                    var physicalDevice = PhysicalDevice;
                    var extensions = Vulkan.vkEnumerateDeviceExtensionProperties(physicalDevice).Select(t => t.GetExtensionName()).ToArray();
                    var layers = Array.Empty<string>();
                    using var vkLayers = new VkStringArray(layers);
                    using var vkExtensions = new VkStringArray(extensions);
                    float queuePriority = 1f;
                    int queueCount = _queueFamilies.Indices.Count();
                    VkDeviceQueueCreateInfo* queueCreateInfos = stackalloc VkDeviceQueueCreateInfo[queueCount];
                    int i = 0;
                    foreach (var index in _queueFamilies.Indices)
                    {
                        queueCreateInfos[i] = new VkDeviceQueueCreateInfo
                        {
                            queueFamilyIndex = index,
                            pQueuePriorities = &queuePriority,
                            queueCount = 1
                        };
                        i++;
                    }
                    var deviceCreateInfo = new VkDeviceCreateInfo
                    {
                        enabledExtensionCount = vkExtensions.Length,
                        enabledLayerCount = 0,
                        ppEnabledExtensionNames = vkExtensions,
                        ppEnabledLayerNames = vkLayers,
                        pQueueCreateInfos = queueCreateInfos,
                        queueCreateInfoCount = (uint)queueCount
                    };
                    var result = Vulkan.vkCreateDevice(physicalDevice, &deviceCreateInfo, null, out var device);
                    result.CheckResult();
                    _device = device;
                }
                Vulkan.vkLoadDevice(_device.Value);
                return _device.Value;
            }
        }

        protected unsafe override GRBackendRenderTarget[] CreateRenderTargets(int width, int height)
        {
            if (_renderTargets != null)
                for (int i = 0; i < _renderTargets.Length; i++)
                    _renderTargets[i].Dispose();
            var physicalDevice = PhysicalDevice;
            var device = Device;
            var result = Vulkan.vkDeviceWaitIdle(device);
            result.CheckResult();

            var surface = _windowContext.GetSurface(_context.Instance);
            result = Vulkan.vkGetPhysicalDeviceSurfaceCapabilitiesKHR(physicalDevice, surface, out var capabilities);
            result.CheckResult();
            //var formats = _physicalDevice.GetSurfaceFormats(surface);
            var presentModes = Vulkan.vkGetPhysicalDeviceSurfacePresentModesKHR(physicalDevice, surface);

            var imageUsageFlags = VkImageUsageFlags.ColorAttachment | VkImageUsageFlags.TransferSrc | VkImageUsageFlags.TransferDst;
            if (capabilities.supportedUsageFlags.HasFlag(VkImageUsageFlags.InputAttachment))
                imageUsageFlags |= VkImageUsageFlags.InputAttachment;
            if (capabilities.supportedUsageFlags.HasFlag(VkImageUsageFlags.Sampled))
                imageUsageFlags |= VkImageUsageFlags.Sampled;
            var alphaFlags = capabilities.supportedCompositeAlpha.HasFlag(VkCompositeAlphaFlagsKHR.Inherit) ? VkCompositeAlphaFlagsKHR.Inherit : VkCompositeAlphaFlagsKHR.Opaque;
            uint imageCount = capabilities.minImageCount + 1;
            if (capabilities.maxImageCount > 0 && imageCount > capabilities.maxImageCount)
            {
                imageCount = capabilities.maxImageCount;
            }
            var queueIndexCount = _queueFamilies.Indices.Count();
            uint* indices = stackalloc uint[queueIndexCount];
            VkSharingMode sharingMode;
            if (_queueFamilies.GraphicsFamily != _queueFamilies.PresentFamily)
            {
                sharingMode = VkSharingMode.Concurrent;
            }
            else
            {
                sharingMode = VkSharingMode.Exclusive;
                indices[0] = _queueFamilies.GraphicsFamily!.Value;
                indices[1] = _queueFamilies.PresentFamily!.Value;
            }
            VkExtent2D extent = new VkExtent2D((uint)_windowContext.Width, (uint)_windowContext.Height);
            var oldSwapchain = _swapchain;
            var format = VulkanHelper.GetFormat(_windowContext.ColorType);
            var colorSpace = VulkanHelper.GetColorSpace(_windowContext.ColorSpace);
            var swapchainCreateInfo = new VkSwapchainCreateInfoKHR
            {
                surface = surface,
                minImageCount = imageCount,
                imageFormat = format,
                imageColorSpace = colorSpace,
                imageExtent = extent,
                imageArrayLayers = 1,
                imageUsage = imageUsageFlags,
                imageSharingMode = sharingMode,
                pQueueFamilyIndices = indices,
                preTransform = capabilities.currentTransform,
                queueFamilyIndexCount = (uint)queueIndexCount,
                compositeAlpha = alphaFlags,
                presentMode = ChooseSwapPresentMode(presentModes),
                oldSwapchain = oldSwapchain ?? default
            };
            result = Vulkan.vkCreateSwapchainKHR(device, &swapchainCreateInfo, null, out var swapchain);
            result.CheckResult();
            _swapchain = swapchain;
            if (oldSwapchain != null)
            {
                result = Vulkan.vkDeviceWaitIdle(device);
                result.CheckResult();
                Vulkan.vkDestroySwapchainKHR(device, oldSwapchain.Value);
                //for (int i = 0; i < imageCount; i++)
                //{
                //    Vulkan.vkResetFences(device, _fences![i]);
                //}
            }
            else
            {
                //result = Vulkan.vkCreateSemaphore(device, out var imageSemaphore);
                //result.CheckResult();
                //_imageAvailableSemaphore = imageSemaphore;
                result = Vulkan.vkCreateSemaphore(device, out var renderSemaphore);
                result.CheckResult();
                _renderFinishedSemaphore = renderSemaphore;
                //_fences = new VkFence[imageCount];
                //for (int i = 0; i < imageCount; i++)
                //{
                //    Vulkan.vkCreateFence(device, VkFenceCreateFlags.Signaled, out _fences[i]);
                //}
            }
            _images = Vulkan.vkGetSwapchainImagesKHR(device, swapchain).ToArray();
            _renderTargets = new GRBackendRenderTarget[imageCount];
            for (int i = 0; i < imageCount; i++)
            {
                _renderTargets[i] = new GRBackendRenderTarget(_windowContext.Width, _windowContext.Height, 1, new GRVkImageInfo
                {
                    CurrentQueueFamily = _queueFamilies.PresentFamily!.Value,
                    Format = (uint)format,
                    Image = _images[i],
                    ImageLayout = (uint)VkImageLayout.Undefined,
                    ImageTiling = (uint)VkImageTiling.Optimal,
                    LevelCount = 1,
                    Protected = false,
                    Alloc = new GRVkAlloc(),
                    ImageUsageFlags = (uint)imageUsageFlags,
                    SharingMode = (uint)sharingMode
                });
            }
            return _renderTargets;
        }

        //protected override void BeforeRender()
        //{
        //    var result = Vulkan.vkAcquireNextImageKHR(_device!.Value, _swapchain!.Value, uint.MaxValue, _imageAvailableSemaphore, default, out _currentBufferIndex);
        //    if (result != VkResult.SuboptimalKHR)
        //        result.CheckResult();
        //}

        protected unsafe override void AfterRender()
        {
            _grContext!.Submit();
            fixed (VkSemaphore* renderSemaphorePtr = &_renderFinishedSemaphore)
            //fixed (VkSemaphore* imageSemaphorePtr = &_imageAvailableSemaphore)
            {
                var stageMask = VkPipelineStageFlags.ColorAttachmentOutput;
                var result = Vulkan.vkQueueSubmit(_graphicsQueue!.Value, new VkSubmitInfo
                {
                    signalSemaphoreCount = 1,
                    pSignalSemaphores = renderSemaphorePtr,
                    waitSemaphoreCount = 0,
                    pWaitSemaphores = default,
                    pWaitDstStageMask = &stageMask
                }, default);
                result.CheckResult();
                result = Vulkan.vkQueuePresentKHR(_presentQueue!.Value, _renderFinishedSemaphore, _swapchain!.Value, _currentBufferIndex);
                if (result != VkResult.SuboptimalKHR)
                    result.CheckResult();
            }
        }

        protected unsafe override void DisposeCore(bool disposing)
        {
            base.DisposeCore(disposing);
            if (disposing)
            {
                if (_renderTargets != null)
                {
                    for (int i = 0; i < _renderTargets.Length; i++)
                        _renderTargets[i].Dispose();
                    _renderTargets = null;
                }
                if (_images != null)
                    _images = null;
                if (_swapchain != null)
                {
                    Vulkan.vkDestroySwapchainKHR(_device!.Value, _swapchain.Value);
                    _swapchain = null;
                    Vulkan.vkDestroySemaphore(_device.Value, _renderFinishedSemaphore);
                    _renderFinishedSemaphore = default;
                }
                if (_device != null)
                {
                    Vulkan.vkDestroyDevice(_device.Value);
                    _device = null;
                }
                if (_grContext != null)
                {
                    _grContext.Dispose();
                    _grContext = null;
                }
                _physicalDevice = null;
            }
        }

        private static bool IsSuitableDevice(VkPhysicalDevice device, VkSurfaceKHR surface)
        {
            return Vulkan.vkEnumerateDeviceExtensionProperties(device).Any(extension => extension.GetExtensionName() == Vulkan.VK_KHR_SWAPCHAIN_EXTENSION_NAME)
                    && FindQueueFamilies(device, surface).IsComplete;
        }

        private static QueueFamilyIndices FindQueueFamilies(VkPhysicalDevice device, VkSurfaceKHR surface)
        {
            QueueFamilyIndices indices = new QueueFamilyIndices();

            var queueFamilies = Vulkan.vkGetPhysicalDeviceQueueFamilyProperties(device);

            for (int index = 0; index < queueFamilies.Length && !indices.IsComplete; index++)
            {
                if (queueFamilies[index].queueFlags.HasFlag(VkQueueFlags.Graphics))
                {
                    indices.GraphicsFamily = (uint)index;
                }

                if (device.IsSwapChainSupport((uint)index, surface))
                {
                    indices.PresentFamily = (uint)index;
                }
            }
            return indices;
        }

        private struct QueueFamilyIndices
        {
            public uint? GraphicsFamily;
            public uint? PresentFamily;

            public IEnumerable<uint> Indices
            {
                get
                {
                    if (this.GraphicsFamily.HasValue)
                    {
                        yield return this.GraphicsFamily.Value;
                    }

                    if (this.PresentFamily.HasValue && this.PresentFamily != this.GraphicsFamily)
                    {
                        yield return this.PresentFamily.Value;
                    }
                }
            }

            public bool IsComplete
            {
                get
                {
                    return GraphicsFamily.HasValue
                        && PresentFamily.HasValue;
                }
            }
        }

        private VkPresentModeKHR ChooseSwapPresentMode(ReadOnlySpan<VkPresentModeKHR> availablePresentModes)
        {
            return availablePresentModes.Contains(VkPresentModeKHR.Mailbox)
                    ? VkPresentModeKHR.Mailbox
                    : VkPresentModeKHR.Fifo;
        }
    }
}
