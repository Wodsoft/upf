using SharpVk;
using SharpVk.Glfw;
using SharpVk.Khronos;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using PhysicalDevice = SharpVk.PhysicalDevice;

namespace Wodsoft.UI.Renderers
{
    public class SkiaRendererVulkanContext : SkiaRendererContext
    {
        private readonly GRSharpVkBackendContext _backendContext;
        private GRBackendRenderTarget? _renderTarget;
        private SharpVk.Image? _image;
        private SharpVk.DeviceMemory? _memory;
        private Surface _surface;

        public SkiaRendererVulkanContext(GRSharpVkBackendContext backendContext, IntPtr hinstance, IntPtr hwnd)
            : base(GRContext.CreateVulkan(backendContext))
        {
            _backendContext = backendContext;          
            _surface = backendContext.VkInstance.CreateWin32Surface(hinstance, hwnd);
            //var swapChainSupport = new SwapChainSupportDetails
            //{
            //    Capabilities = backendContext.VkPhysicalDevice.GetSurfaceCapabilities(_surface),
            //    Formats = backendContext.VkPhysicalDevice.GetSurfaceFormats(_surface),
            //    PresentModes = backendContext.VkPhysicalDevice.GetSurfacePresentModes(_surface)
            //};
            //SurfaceFormat surfaceFormat = ChooseSwapSurfaceFormat(swapChainSupport.Formats);

            //uint imageCount = swapChainSupport.Capabilities.MinImageCount + 1;
            //if (swapChainSupport.Capabilities.MaxImageCount > 0 && imageCount > swapChainSupport.Capabilities.MaxImageCount)
            //{
            //    imageCount = swapChainSupport.Capabilities.MaxImageCount;
            //}
            //QueueFamilyIndices queueFamilies = FindQueueFamilies(backendContext.VkPhysicalDevice);
            //var indices = queueFamilies.Indices.ToArray();
            //Extent2D extent = ChooseSwapExtent(swapChainSupport.Capabilities);

            //this.swapChain = device.CreateSwapchain(surface,
            //                                        imageCount,
            //                                        surfaceFormat.Format,
            //                                        surfaceFormat.ColorSpace,
            //                                        extent,
            //                                        1,
            //                                        ImageUsageFlags.ColorAttachment,
            //                                        indices.Length == 1
            //                                            ? SharingMode.Exclusive
            //                                            : SharingMode.Concurrent,
            //                                        indices,
            //                                        swapChainSupport.Capabilities.CurrentTransform,
            //                                        CompositeAlphaFlags.Opaque,
            //                                        this.ChooseSwapPresentMode(swapChainSupport.PresentModes),
            //                                        true,
            //                                        this.swapChain);
        }

        protected override SKSurface CreateSurface(int width, int height)
        {
            if (_renderTarget != null)
                _renderTarget.Dispose();
            if (_image != null)
                _image.Dispose();

            //SurfaceFormat surfaceFormat = ChooseSwapSurfaceFormat(_backendContext.VkPhysicalDevice.GetSurfaceFormats(_surface));
            _image = _backendContext.VkDevice.CreateImage(
                SharpVk.ImageType.Image2d,
                SharpVk.Format.R8G8B8A8UNorm,
                new Extent3D((uint)width, (uint)height, 1),
                9,
                1,
                SampleCountFlags.SampleCount1,
                ImageTiling.Optimal,
                ImageUsageFlags.ColorAttachment | ImageUsageFlags.TransferDestination | ImageUsageFlags.Sampled,
                SharingMode.Exclusive,
                new[] { _backendContext.GraphicsQueueIndex },
                ImageLayout.Undefined);
            var memoryRequirements = _image.GetMemoryRequirements();
           _memory = _backendContext.VkDevice.AllocateMemory(memoryRequirements.Size,
                    FindMemoryType(_backendContext.VkPhysicalDevice, memoryRequirements.MemoryTypeBits, MemoryPropertyFlags.DeviceLocal));
            _image.BindMemory(_memory, 0);
            var commandPool = _backendContext.VkDevice.CreateCommandPool(_backendContext.GraphicsQueueIndex);
            var commandBuffer = _backendContext.VkDevice.AllocateCommandBuffer(commandPool, CommandBufferLevel.Primary);
            commandBuffer.Begin(CommandBufferUsageFlags.OneTimeSubmit);
            var subresourceRange = new ImageSubresourceRange(ImageAspectFlags.Color, 0, 9, 0, 1);
            var barrier = new ImageMemoryBarrier()
            {
                SourceAccessMask = 0,
                DestinationAccessMask = 0,
                OldLayout = ImageLayout.Undefined,
                NewLayout = ImageLayout.ColorAttachmentOptimal,
                SourceQueueFamilyIndex = SharpVk.Constants.QueueFamilyIgnored,
                DestinationQueueFamilyIndex = SharpVk.Constants.QueueFamilyIgnored,
                Image = _image,
                SubresourceRange = subresourceRange
            };
            commandBuffer.PipelineBarrier(PipelineStageFlags.TopOfPipe, PipelineStageFlags.AllCommands, null, null, new[] { barrier });
            commandBuffer.End();            
            _backendContext.VkQueue.Submit(new[] { new SubmitInfo() { CommandBuffers = new[] { commandBuffer }, } }, null);
            _backendContext.VkQueue.WaitIdle();
            commandPool.FreeCommandBuffers(new[] { commandBuffer });            
            _renderTarget = new GRBackendRenderTarget(width, height, 1, new GRVkImageInfo
            {
                CurrentQueueFamily = _backendContext.GraphicsQueueIndex,
                Format = (uint)SharpVk.Format.R8G8B8A8UNorm,
                Image = _image.RawHandle.ToUInt64(),
                ImageLayout = (uint)ImageLayout.ColorAttachmentOptimal,
                ImageTiling = (uint)ImageTiling.Optimal,
                LevelCount = 9,
                Protected = false,
                Alloc = new GRVkAlloc()
                {
                    Memory = _memory.RawHandle.ToUInt64(),
                    Flags = 0,
                    Offset = 0,
                    Size = memoryRequirements.Size
                }
            });
            return SKSurface.Create(GRContext, _renderTarget, GRSurfaceOrigin.TopLeft, SKColorType.Rgba8888);
        }

        private static uint FindMemoryType(PhysicalDevice physicalDevice, uint memoryTypeBits, MemoryPropertyFlags flags)
        {
            var props = physicalDevice.GetMemoryProperties();

            for (int i = 0; i < props.MemoryTypes.Length; i++)
            {
                var type = props.MemoryTypes[i];
                if ((memoryTypeBits & (1 << i)) != 0 && type.PropertyFlags.HasFlag(flags))
                {
                    return (uint)i;
                }
            }
            return 0;
        }

        protected override void DisposeCore(bool disposing)
        {
            if (disposing)
            {
                if (_renderTarget != null)
                    _renderTarget.Dispose();
            }
        }

        private struct SwapChainSupportDetails
        {
            public SurfaceCapabilities Capabilities;
            public SurfaceFormat[] Formats;
            public PresentMode[] PresentModes;
        }

        private SurfaceFormat ChooseSwapSurfaceFormat(SurfaceFormat[] availableFormats)
        {
            if (availableFormats.Length == 1 && availableFormats[0].Format == Format.Undefined)
            {
                return new SurfaceFormat
                {
                    Format = Format.R8G8B8A8UNorm,
                    ColorSpace = ColorSpace.SrgbNonlinear
                };
            }

            foreach (var format in availableFormats)
            {
                if (format.Format == Format.R8G8B8A8UNorm && format.ColorSpace == ColorSpace.SrgbNonlinear)
                {
                    return format;
                }
            }

            return availableFormats[0];
        }

        private QueueFamilyIndices FindQueueFamilies(PhysicalDevice device)
        {
            QueueFamilyIndices indices = new QueueFamilyIndices();

            var queueFamilies = device.GetQueueFamilyProperties();

            for (uint index = 0; index < queueFamilies.Length && !indices.IsComplete; index++)
            {
                if (queueFamilies[index].QueueFlags.HasFlag(QueueFlags.Graphics))
                {
                    indices.GraphicsFamily = index;
                }

                if (device.GetSurfaceSupport(index, _surface))
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

        //private Extent2D ChooseSwapExtent(SurfaceCapabilities capabilities)
        //{
        //    if (capabilities.CurrentExtent.Width != uint.MaxValue)
        //    {
        //        return capabilities.CurrentExtent;
        //    }
        //    else
        //    {
        //        return new Extent2D
        //        {
        //            Width = Math.Max(capabilities.MinImageExtent.Width, Math.Min(capabilities.MaxImageExtent.Width, SurfaceWidth)),
        //            Height = Math.Max(capabilities.MinImageExtent.Height, Math.Min(capabilities.MaxImageExtent.Height, SurfaceHeight))
        //        };
        //    }
        //}
    }
}
