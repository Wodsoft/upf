using SharpGen.Runtime;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D12;
using Vortice.DXGI;
using Vortice.Mathematics;

namespace Wodsoft.UI.Renderers
{
    internal class D3D12Helper
    {
        internal static Format GetFormat(SKColorType colorType)
        {
            switch (colorType)
            {
                case SKColorType.Alpha8:
                    return Format.A8_UNorm;
                case SKColorType.Rgba8888:
                    return Format.R8G8B8A8_UNorm;
                case SKColorType.Rgb888x:
                    return Format.R8G8_B8G8_UNorm;
                case SKColorType.Bgra8888:
                    return Format.B8G8R8A8_UNorm;
                case SKColorType.Rgba1010102:
                    return Format.R10G10B10A2_UNorm;
                case SKColorType.RgbaF16:
                    return Format.R16G16B16A16_Float;
                case SKColorType.RgbaF32:
                    return Format.R32G32B32A32_Float;
                case SKColorType.Rg88:
                    return Format.R8G8_UNorm;                    
                case SKColorType.RgF16:
                    return Format.R16G16_Float;
                case SKColorType.Rg1616:
                    return Format.R16G16_UNorm;
                case SKColorType.Rgba16161616:
                    return Format.R16G16B16A16_UNorm;
                case SKColorType.Bgra1010102:
                    return Format.R10G10B10A2_UNorm;
                case SKColorType.Srgba8888:
                    return Format.R8G8B8A8_UNorm_SRgb;
                case SKColorType.R8Unorm:
                    return Format.R8_UNorm;
                case SKColorType.Unknown:
                case SKColorType.Rgb565:
                case SKColorType.Argb4444:
                case SKColorType.Rgb101010x:
                case SKColorType.Gray8:
                case SKColorType.RgbaF16Clamped:
                case SKColorType.AlphaF16:
                case SKColorType.Alpha16:
                case SKColorType.Bgr101010x:
                case SKColorType.Bgr101010xXR:
                default:
                    throw new NotSupportedException($"Direct3D does not support color type \"{colorType}\".");
            }
        }

        [DllImport("libSkiaSharp", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern IntPtr gr_direct_context_make_direct3d(GRD3D12BackendContext vkBackendContext);

        [DllImport("libSkiaSharp", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern IntPtr gr_backendtexture_new_direct3d(int width, int height, GRD3D12TextureInfo* vkInfo);

        [DllImport("libSkiaSharp", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern IntPtr gr_backendrendertarget_new_direct3d(int width, int height, GRD3D12TextureInfo* vkInfo);
    }

    internal struct GRD3D12BackendContext
    {
        public nint fAdapter;
        public nint fDevice;
        public nint fQueue;
        public nint fMemoryAllocator;
        public bool fProtectedContext;
    }

    internal struct GRD3D12TextureInfo
    {
        public nint fResource;
        public nint fAlloc;
        public ResourceStates fResourceState;
        public Format fFormat;
        public uint fSampleCount;
        public uint fLevelCount;
        public uint fSampleQualityPattern;
        public bool fProtected;
    }
}
