using SharpVk;
using SharpVk.Khronos;
using SharpVk.Multivendor;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using PhysicalDevice = SharpVk.PhysicalDevice;
using Semaphore = SharpVk.Semaphore;
using Version = SharpVk.Version;

namespace Wodsoft.UI.Renderers
{
    public class SkiaRendererVulkanContext : SkiaRendererContext
    {
        private readonly GRSharpVkBackendContext _backendContext;
        private readonly Instance _instance;
        private readonly Surface _surface;
        private readonly PhysicalDevice _physicalDevice;
        private readonly Device _device;
        private readonly QueueFamilyIndices _queueFamilies;
        private readonly Queue _presentQueue;
        private readonly Semaphore _renderFinishedSemaphore, _imageAvailableSemaphore;
        private GRBackendRenderTarget? _renderTarget;
        private Image? _image;
        private Swapchain? _swapchain;

        private SkiaRendererVulkanContext(GRSharpVkBackendContext backendContext, Instance instance, Surface surface, PhysicalDevice physicalDevice, Device device,
            QueueFamilyIndices queueFamilies, Queue presentQueue)
            : base(GRContext.CreateVulkan(backendContext))
        {
            _backendContext = backendContext;
            _instance = instance;
            _surface = surface;
            _physicalDevice = physicalDevice;
            _device = device;
            _queueFamilies = queueFamilies;
            _presentQueue = presentQueue;
            _renderFinishedSemaphore = device.CreateSemaphore();
            _imageAvailableSemaphore = device.CreateSemaphore();
        }

        #region CreateContext

        public static SkiaRendererVulkanContext CreateFromWindowHandle(IntPtr hinstance, IntPtr hwnd)
        {
            var enabledLayers = Instance.EnumerateLayerProperties().Any(x => x.LayerName == "VK_LAYER_LUNARG_standard_validation")
                ? new string[] { "VK_LAYER_LUNARG_standard_validation" } : null;
            var instance = Instance.Create(enabledLayers, new[]
                {
                    KhrExtensions.Surface,
                    KhrExtensions.Win32Surface,
                    ExtExtensions.DebugReport,
                }, applicationInfo: new ApplicationInfo
                {
                    ApplicationName = "UPF Application",
                    ApplicationVersion = new Version(1, 0, 0),
                    EngineName = "UPF",
                    EngineVersion = new Version(0, 4, 0),
                    ApiVersion = new Version(1, 0, 0)
                });
            var surface = instance.CreateWin32Surface(hinstance, hwnd);
            instance.EnumeratePhysicalDevices();
            var physicalDevice = instance.EnumeratePhysicalDevices().First(t => IsSuitableDevice(t, surface));
            QueueFamilyIndices queueFamilies = FindQueueFamilies(physicalDevice, surface);
            var device = physicalDevice.CreateDevice(queueFamilies.Indices
                                                        .Select(index => new DeviceQueueCreateInfo
                                                        {
                                                            QueueFamilyIndex = index,
                                                            QueuePriorities = new[] { 1f }
                                                        }).ToArray(),
                                                        null,
                                                        KhrExtensions.Swapchain);

            var graphicsQueue = device.GetQueue(queueFamilies.GraphicsFamily!.Value, 0);
            var presentQueue = device.GetQueue(queueFamilies.PresentFamily!.Value, 0);
            //var transferQueue = device.GetQueue(queueFamilies.TransferFamily!.Value, 0);

            GRVkGetProcedureAddressDelegate getProc = (name, instanceHandle, deviceHandle) =>
            {
                return instance.GetProcedureAddress(name);
            };
            var extensions = physicalDevice.EnumerateDeviceExtensionProperties(null).Select(t => t.ExtensionName).ToArray();
            using var backendContext = new GRSharpVkBackendContext
            {
                VkInstance = instance,
                VkPhysicalDevice = physicalDevice,
                VkDevice = device,
                VkQueue = graphicsQueue,
                VkPhysicalDeviceFeatures = physicalDevice.GetFeatures(),
                Extensions = GRVkExtensions.Create(getProc, (IntPtr)instance.RawHandle.ToUInt64(), (IntPtr)physicalDevice.RawHandle.ToUInt64(), null, extensions),
                GraphicsQueueIndex = queueFamilies.GraphicsFamily!.Value,
                GetProcedureAddress = (name, innerInstance, innerDevice) =>
                {
                    if (innerInstance != null)
                        return innerInstance.GetProcedureAddress(name);
                    if (innerDevice != null)
                        return innerDevice.GetProcedureAddress(name);
                    return instance.GetProcedureAddress(name);
                }
            };
            return new SkiaRendererVulkanContext(backendContext, instance, surface, physicalDevice, device, queueFamilies, presentQueue);
        }

        private static bool IsSuitableDevice(PhysicalDevice device, Surface surface)
        {
            return device.EnumerateDeviceExtensionProperties(null).Any(extension => extension.ExtensionName == KhrExtensions.Swapchain)
                    && FindQueueFamilies(device, surface).IsComplete;
        }

        #endregion

        protected override SKSurface CreateSurface(int width, int height)
        {
            if (_renderTarget != null)
                _renderTarget.Dispose();
            if (_image != null)
                _image.Dispose();
            var swapChainSupport = new SwapChainSupportDetails
            {
                Capabilities = _physicalDevice.GetSurfaceCapabilities(_surface),
                Formats = _physicalDevice.GetSurfaceFormats(_surface),
                PresentModes = _physicalDevice.GetSurfacePresentModes(_surface)
            };
            var imageUsageFlags = ImageUsageFlags.ColorAttachment | ImageUsageFlags.TransferSource | ImageUsageFlags.TransferDestination;
            if (swapChainSupport.Capabilities.SupportedUsageFlags.HasFlag(ImageUsageFlags.InputAttachment))
                imageUsageFlags |= ImageUsageFlags.InputAttachment;
            if (swapChainSupport.Capabilities.SupportedUsageFlags.HasFlag(ImageUsageFlags.Sampled))
                imageUsageFlags |= ImageUsageFlags.Sampled;
            var alphaFlags = swapChainSupport.Capabilities.SupportedCompositeAlpha.HasFlag(CompositeAlphaFlags.Inherit) ? CompositeAlphaFlags.Inherit : CompositeAlphaFlags.Opaque;
            //uint imageCount = swapChainSupport.Capabilities.MinImageCount + 1;
            //if (swapChainSupport.Capabilities.MaxImageCount > 0 && imageCount > swapChainSupport.Capabilities.MaxImageCount)
            //{
            //    imageCount = swapChainSupport.Capabilities.MaxImageCount;
            //}
            var indices = _queueFamilies.Indices.ToArray();
            Extent2D extent = new Extent2D((uint)width, (uint)height);
            var sharingMode = indices.Length == 1 ? SharingMode.Exclusive : SharingMode.Concurrent;
            _swapchain = _device.CreateSwapchain(_surface,
                                                    1,
                                                    Format.R8G8B8A8UNorm,
                                                    ColorSpace.SrgbNonlinear,
                                                    extent,
                                                    1,
                                                    imageUsageFlags,
                                                    sharingMode,
                                                    indices,
                                                    swapChainSupport.Capabilities.CurrentTransform,
                                                    alphaFlags,
                                                    ChooseSwapPresentMode(swapChainSupport.PresentModes),
                                                    true,
                                                    _swapchain);

            _image = _swapchain.GetImages()[0];
            _renderTarget = new GRBackendRenderTarget(width, height, 1, new GRVkImageInfo
            {
                CurrentQueueFamily = _queueFamilies.PresentFamily!.Value,
                Format = (uint)SharpVk.Format.R8G8B8A8UNorm,
                Image = _image.RawHandle.ToUInt64(),
                ImageLayout = (uint)ImageLayout.Undefined,
                ImageTiling = (uint)ImageTiling.Optimal,
                LevelCount = 1,
                Protected = false,
                Alloc = new GRVkAlloc(),
                ImageUsageFlags = (uint)imageUsageFlags,
                SharingMode = (uint)sharingMode
            });
            var surface = SKSurface.Create(GRContext, _renderTarget, GRSurfaceOrigin.TopLeft, SKColorType.Rgba8888);
            return surface;
        }

        protected override void AfterRender()
        {
            _presentQueue!.Present(_renderFinishedSemaphore, _swapchain, 0, null);
        }

        protected override void DisposeCore(bool disposing)
        {
            if (disposing)
            {
                if (_renderTarget != null)
                    _renderTarget.Dispose();
                if (_image != null)
                    _image.Dispose();
                if (_swapchain != null)
                    _swapchain.Dispose();
                _renderFinishedSemaphore.Dispose();
                _renderFinishedSemaphore.Dispose();
                _imageAvailableSemaphore.Dispose();
                _backendContext.Dispose();
                _device.Dispose();
                _surface.Dispose();
                _instance.Dispose();
            }
        }

        private struct SwapChainSupportDetails
        {
            public SurfaceCapabilities Capabilities;
            public SurfaceFormat[] Formats;
            public PresentMode[] PresentModes;
        }

        //private SurfaceFormat ChooseSwapSurfaceFormat(SurfaceFormat[] availableFormats)
        //{
        //    if (availableFormats.Length == 1 && availableFormats[0].Format == Format.Undefined)
        //    {
        //        return new SurfaceFormat
        //        {
        //            Format = Format.R8G8B8A8UNorm,
        //            ColorSpace = ColorSpace.SrgbNonlinear
        //        };
        //    }

        //    foreach (var format in availableFormats)
        //    {
        //        if (format.Format == Format.R8G8B8A8UNorm && format.ColorSpace == ColorSpace.SrgbNonlinear)
        //        {
        //            return format;
        //        }
        //    }

        //    return availableFormats[0];
        //}

        private static QueueFamilyIndices FindQueueFamilies(PhysicalDevice device, Surface surface)
        {
            QueueFamilyIndices indices = new QueueFamilyIndices();

            var queueFamilies = device.GetQueueFamilyProperties();

            for (uint index = 0; index < queueFamilies.Length && !indices.IsComplete; index++)
            {
                if (queueFamilies[index].QueueFlags.HasFlag(QueueFlags.Graphics))
                {
                    indices.GraphicsFamily = index;
                }

                if (device.GetSurfaceSupport(index, surface))
                {
                    indices.PresentFamily = index;
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
                    return this.GraphicsFamily.HasValue
                        && this.PresentFamily.HasValue;
                }
            }
        }

        private PresentMode ChooseSwapPresentMode(PresentMode[] availablePresentModes)
        {
            return availablePresentModes.Contains(PresentMode.Mailbox)
                    ? PresentMode.Mailbox
                    : PresentMode.Fifo;
        }
    }
}
