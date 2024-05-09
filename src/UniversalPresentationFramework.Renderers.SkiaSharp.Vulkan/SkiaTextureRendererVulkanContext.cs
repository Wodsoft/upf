using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vortice.Vulkan;

namespace Wodsoft.UI.Renderers
{
    public class SkiaTextureRendererVulkanContext : SkiaTextureRendererContext
    {
        private readonly ISkiaVulkanContext _context;
        private readonly int _width;
        private readonly int _height;
        private readonly SKColorType _colorType;
        private readonly SKColorSpace _colorSpace;
        private readonly SKAlphaType _alphaType;
        private readonly VkPhysicalDevice _physicalDevice;
        private GRContext? _grContext;
        private VkDevice? _device;
        private uint _queueIndex;
        private VkQueue? _graphicsQueue;
        private VkImage? _image;
        private GRBackendTexture? _texture;
        private VkDeviceMemory? _memory;
        private VkFormat _format;

        public SkiaTextureRendererVulkanContext(ISkiaVulkanContext context, int width, int height, SKColorType colorType, SKAlphaType alphaType, SKColorSpace colorSpace, int sampleCount = 4)
        {
            _physicalDevice = context.PhysicalDevices[0];
            _format = VulkanHelper.GetFormat(colorType);
            _context = context;
            _width = width;
            _height = height;
            _colorType = colorType;
            _colorSpace = colorSpace;
            SampleCount = sampleCount;
            _alphaType = alphaType;
            var queueFamilies = Vulkan.vkGetPhysicalDeviceQueueFamilyProperties(_physicalDevice);
            _queueIndex = (uint)queueFamilies.FirstIndex(t => t.queueFlags.HasFlag(VkQueueFlags.Graphics));
        }

        public unsafe override GRBackendTexture Texture
        {
            get
            {
                if (_texture == null)
                {
                    var device = Device;
                    var imageUsageFlags = VkImageUsageFlags.ColorAttachment | VkImageUsageFlags.TransferSrc | VkImageUsageFlags.TransferDst | VkImageUsageFlags.Sampled;
                    fixed (uint* queuePtr = &_queueIndex)
                    {
                        var imageCreateInfo = new VkImageCreateInfo
                        {
                            arrayLayers = 1,
                            extent = new VkExtent3D
                            {
                                depth = 1,
                                width = (uint)_width,
                                height = (uint)_height
                            },
                            format = _format,
                            imageType = VkImageType.Image2D,
                            initialLayout = VkImageLayout.Undefined,
                            mipLevels = 1,
                            //pQueueFamilyIndices = queuePtr,
                            //queueFamilyIndexCount = 1,
                            samples = VkSampleCountFlags.Count1,
                            sharingMode = VkSharingMode.Exclusive,
                            tiling = VkImageTiling.Optimal,
                            usage = imageUsageFlags,
                        };
                        var result = Vulkan.vkCreateImage(device, &imageCreateInfo, null, out var image);
                        result.CheckResult();
                        Vulkan.vkGetImageMemoryRequirements(device, image, out var memRequirements);
                        Vulkan.vkGetPhysicalDeviceMemoryProperties(_physicalDevice, out var memoryProperties);
                        int memoryTypeIndex = -1;
                        for (int i = 0; i < memoryProperties.memoryTypeCount; i++)
                        {
                            if ((memRequirements.memoryTypeBits & (1 << i)) != 0 && memoryProperties.memoryTypes[i].propertyFlags.HasFlag(VkMemoryPropertyFlags.DeviceLocal))
                            {
                                memoryTypeIndex = i;
                                break;
                            }
                        }
                        if (memoryTypeIndex == -1)
                            throw new NotSupportedException("Unable to find suitable memory type.");
                        VkMemoryAllocateInfo allocInfo = new VkMemoryAllocateInfo
                        {
                            allocationSize = memRequirements.size,
                            memoryTypeIndex = (uint)memoryTypeIndex
                        };
                        result = Vulkan.vkAllocateMemory(device, &allocInfo, null, out var memory);
                        result.CheckResult();
                        _memory = memory;
                        result = Vulkan.vkBindImageMemory(device, image, memory, 0);
                        result.CheckResult();
                        _image = image;
                    }
                    _texture = new GRBackendTexture(_width, _height, new GRVkImageInfo
                    {
                        CurrentQueueFamily = _queueIndex,
                        Format = (uint)_format,
                        Image = _image.Value,
                        ImageLayout = (uint)VkImageLayout.Undefined,
                        ImageTiling = (uint)VkImageTiling.Optimal,
                        LevelCount = 1,
                        Protected = false,
                        Alloc = new GRVkAlloc(),
                        ImageUsageFlags = (uint)imageUsageFlags,
                        SharingMode = (uint)VkSharingMode.Exclusive,
                        SampleCount = (uint)VkSampleCountFlags.Count1
                    });
                }
                return _texture;
            }
        }

        public override int SampleCount { get; }

        public unsafe override GRContext GRContext
        {
            get
            {
                if (_grContext == null)
                {
                    var instance = _context.Instance;
                    var device = Device;
                    Vulkan.vkGetDeviceQueue(device, _queueIndex, 0, out var graphicsQueue);
                    _graphicsQueue = graphicsQueue;

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
                    var extensions = Vulkan.vkEnumerateDeviceExtensionProperties(_physicalDevice).Select(t => t.GetExtensionName()).ToArray();
                    Vulkan.vkGetPhysicalDeviceFeatures(_physicalDevice, out var features);
                    var backendContext = new GRVkBackendContext
                    {
                        VkInstance = instance,
                        VkPhysicalDevice = _physicalDevice,
                        VkDevice = device,
                        VkQueue = graphicsQueue,
                        VkPhysicalDeviceFeatures = new nint(&features),
                        Extensions = GRVkExtensions.Create(getProc, instance, _physicalDevice, null, extensions),
                        GraphicsQueueIndex = _queueIndex,
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

        protected unsafe VkDevice Device
        {
            get
            {
                if (_device == null)
                {
                    var extensions = Vulkan.vkEnumerateDeviceExtensionProperties(_physicalDevice).Select(t => t.GetExtensionName()).ToArray();
                    var layers = Array.Empty<string>();
                    using var vkLayers = new VkStringArray(layers);
                    using var vkExtensions = new VkStringArray(extensions);
                    float queuePriority = 1f;
                    VkDeviceQueueCreateInfo queueCreateInfo = new VkDeviceQueueCreateInfo
                    {
                        queueFamilyIndex = _queueIndex,
                        pQueuePriorities = &queuePriority,
                        queueCount = 1
                    };
                    var deviceCreateInfo = new VkDeviceCreateInfo
                    {
                        enabledExtensionCount = vkExtensions.Length,
                        enabledLayerCount = 0,
                        ppEnabledExtensionNames = vkExtensions,
                        ppEnabledLayerNames = vkLayers,
                        pQueueCreateInfos = &queueCreateInfo,
                        queueCreateInfoCount = 1
                    };
                    var result = Vulkan.vkCreateDevice(_physicalDevice, &deviceCreateInfo, null, out var device);
                    result.CheckResult();
                    _device = device;
                }
                Vulkan.vkLoadDevice(_device.Value);
                return _device.Value;
            }
        }

        public override SKColorType ColorType => _colorType;

        public override SKColorSpace ColorSpace => _colorSpace;

        public override int Width => _width;

        public override int Height => _height;

        public override SKAlphaType AlphaType => _alphaType;

        public override GRSurfaceOrigin SurfaceOrigin => GRSurfaceOrigin.TopLeft;

        protected unsafe override void DisposeCore(bool disposing)
        {
            base.DisposeCore(disposing);
            if (disposing)
            {
                if (_texture != null)
                {
                    _texture.Dispose();
                    _texture = null;
                }
                if (_image != null)
                {
                    Vulkan.vkDestroyImage(_device!.Value, _image.Value);
                    _image = null;
                }
                if (_memory != null)
                {
                    Vulkan.vkFreeMemory(_device!.Value, _memory.Value);
                    _memory = null;
                }
                if (_grContext != null)
                {
                    _grContext.Dispose();
                    _grContext = null;
                }
                if (_device != null)
                {
                    Vulkan.vkDestroyDevice(_device.Value);
                    _device = null;
                }                
            }
        }
    }
}
