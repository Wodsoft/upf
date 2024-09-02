using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D;
using Vortice.Direct3D12;
using Vortice.DXGI;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Renderers
{
    public class SkiaRendererD3D12Provider : SkiaRendererProvider, ISkiaDirect3DContext
    {
        private readonly IDXGIFactory4 _factory;
        private readonly IDXGIAdapter1 _adapter;
        private readonly ID3D12Device2 _device;

        protected SkiaRendererD3D12Provider(IDXGIFactory4 factory, IDXGIAdapter1 adapter, ID3D12Device2 device)
        {
            _factory = factory;
            _adapter = adapter;
            _device = device;
        }

        public IDXGIFactory4 Factory => _factory;

        public IDXGIAdapter1 Adapter => _adapter;

        public ID3D12Device2 Device => _device;

        public static bool TryCreate([NotNullWhen(true)] out SkiaRendererD3D12Provider? provider)
        {
            provider = null;
            if (!D3D12.IsSupported(Vortice.Direct3D.FeatureLevel.Level_11_0))
                return false;
#if DEBUG
            if (D3D12.D3D12GetDebugInterface(out ID3D12DeviceRemovedExtendedDataSettings1? dredSettings).Success)
            {
                // Turn on auto-breadcrumbs and page fault reporting.
                dredSettings!.SetAutoBreadcrumbsEnablement(DredEnablement.ForcedOn);
                dredSettings.SetPageFaultEnablement(DredEnablement.ForcedOn);
                dredSettings.SetBreadcrumbContextEnablement(DredEnablement.ForcedOn);

                dredSettings.Dispose();
            }
#endif
            var factory = DXGI.CreateDXGIFactory2<IDXGIFactory4>(true);

            ID3D12Device2? device = default;
            IDXGIAdapter1? adapter = null;
            using (IDXGIFactory6? factory6 = factory.QueryInterfaceOrNull<IDXGIFactory6>())
            {
                if (factory6 != null)
                {
                    for (int adapterIndex = 0; factory6.EnumAdapterByGpuPreference(adapterIndex, GpuPreference.HighPerformance, out adapter).Success; adapterIndex++)
                    {
                        AdapterDescription1 desc = adapter!.Description1;

                        // Don't select the Basic Render Driver adapter.
                        if ((desc.Flags & AdapterFlags.Software) != AdapterFlags.None)
                        {
                            adapter.Dispose();
                            continue;
                        }

                        if (D3D12.D3D12CreateDevice(adapter, FeatureLevel.Level_11_0, out device).Success)
                            break;
                    }
                }
                else
                {
                    for (int adapterIndex = 0;
                        factory.EnumAdapters1(adapterIndex, out adapter).Success;
                        adapterIndex++)
                    {
                        AdapterDescription1 desc = adapter.Description1;

                        // Don't select the Basic Render Driver adapter.
                        if ((desc.Flags & AdapterFlags.Software) != AdapterFlags.None)
                            continue;

                        if (D3D12.D3D12CreateDevice(adapter, FeatureLevel.Level_11_0, out device).Success)
                            break;
                    }
                }
            }

            if (device == null)
                return false;

            provider = new SkiaRendererD3D12Provider(factory, adapter!, device);
            return true;
        }

        public override IRenderBitmapContext CreateRenderBitmapContext(int pixelWidth, int pixelHeight, float dpiX, float dpiY, PixelFormat pixelFormat)
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
            var renderer = new SkiaTextureRendererD3D12Context(this, pixelWidth, pixelHeight, SkiaHelper.GetColorType(pixelFormat), pixelFormat.IsPremultiplied ? SKAlphaType.Premul : SKAlphaType.Opaque, colorSpace);
            return new SkiaRenderBitmapContext(renderer, pixelFormat);
        }
    }
}
