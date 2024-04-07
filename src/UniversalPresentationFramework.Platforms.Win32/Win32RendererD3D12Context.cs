using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D;
using Vortice.Direct3D12;
using Vortice.DXGI;
using Windows.Win32.Foundation;
using Wodsoft.UI.Renderers;

namespace Wodsoft.UI.Platforms.Win32
{
    public class Win32RendererD3D12Context : SkiaRendererContext
    {
        private readonly HWND _hwnd;
        private IDXGIFactory4 _factory;
        private readonly ID3D12Device2 _device;
        private readonly ID3D12CommandQueue _queue;
        private readonly ID3D12Fence _fence;
        //private readonly ID3D12CommandAllocator[] _commandAllocators;
        //private readonly ID3D12GraphicsCommandList4 _commandList;
        private IDXGISwapChain3? _swapChain;
        private SKSurface[]? _surfaces;
        private ID3D12Resource[] _resources;
        private GRBackendRenderTarget[] _renderTargets;
        private ulong[] _surfaceFrameCounts;
        private ulong _frameCount;
        private readonly AutoResetEvent _fenceEvent;


        private Win32RendererD3D12Context(GRContext context, HWND hwnd, IDXGIFactory4 factory, ID3D12Device2 device, ID3D12CommandQueue queue, ID3D12Fence fence) : base(context)
        {
            _hwnd = hwnd;
            _factory = factory;
            _device = device;
            _queue = queue;
            _fence = fence;
            _surfaceFrameCounts = new ulong[_BufferCount];
            _resources = new ID3D12Resource[_BufferCount];
            _renderTargets = new GRBackendRenderTarget[_BufferCount];
            _fenceEvent = new AutoResetEvent(false);
        }

        internal static Win32RendererD3D12Context? Create(HWND hwnd)
        {
            if (!D3D12.IsSupported(Vortice.Direct3D.FeatureLevel.Level_11_0))
                return null;

            if (D3D12.D3D12GetDebugInterface(out Vortice.Direct3D12.Debug.ID3D12Debug? debug).Success)
            {
                debug!.EnableDebugLayer();
                debug.Dispose();
            }

            if (D3D12.D3D12GetDebugInterface(out ID3D12DeviceRemovedExtendedDataSettings1? dredSettings).Success)
            {
                // Turn on auto-breadcrumbs and page fault reporting.
                dredSettings!.SetAutoBreadcrumbsEnablement(DredEnablement.ForcedOn);
                dredSettings.SetPageFaultEnablement(DredEnablement.ForcedOn);
                dredSettings.SetBreadcrumbContextEnablement(DredEnablement.ForcedOn);

                dredSettings.Dispose();
            }

            var factory = DXGI.CreateDXGIFactory2<IDXGIFactory4>(true);

            ID3D12Device2? device = default;
            IDXGIAdapter1? adapter1 = null;
            using (IDXGIFactory6? factory6 = factory.QueryInterfaceOrNull<IDXGIFactory6>())
            {
                if (factory6 != null)
                {
                    for (int adapterIndex = 0; factory6.EnumAdapterByGpuPreference(adapterIndex, GpuPreference.HighPerformance, out IDXGIAdapter1? adapter).Success; adapterIndex++)
                    {
                        AdapterDescription1 desc = adapter!.Description1;

                        // Don't select the Basic Render Driver adapter.
                        if ((desc.Flags & AdapterFlags.Software) != AdapterFlags.None)
                        {
                            adapter.Dispose();
                            continue;
                        }

                        if (D3D12.D3D12CreateDevice(adapter, FeatureLevel.Level_11_0, out device).Success)
                        {
                            adapter1 = adapter;
                            break;
                        }
                    }
                }
                else
                {
                    for (int adapterIndex = 0;
                        factory.EnumAdapters1(adapterIndex, out IDXGIAdapter1? adapter).Success;
                        adapterIndex++)
                    {
                        AdapterDescription1 desc = adapter.Description1;

                        // Don't select the Basic Render Driver adapter.
                        if ((desc.Flags & AdapterFlags.Software) != AdapterFlags.None)
                        {
                            adapter1 = adapter;
                            continue;
                        }

                        if (D3D12.D3D12CreateDevice(adapter, FeatureLevel.Level_11_0, out device).Success)
                        {
                            break;
                        }
                    }
                }
            }

            if (device == null)
                return null;

            var queue = device.CreateCommandQueue(new CommandQueueDescription
            {
                Flags = CommandQueueFlags.None,
                Type = CommandListType.Direct
            });

            var grContextPtr = gr_direct_context_make_direct3d(new GRD3D12BackendContext
            {
                fAdapter = adapter1!.NativePointer,
                fDevice = device.NativePointer,
                fQueue = queue.NativePointer,
                fProtectedContext = false
            });

            if (grContextPtr == default)
                return null;

            var grContext = (GRContext)typeof(GRContext).GetMethod("GetObject", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static, [typeof(nint), typeof(bool)])!
                .Invoke(null, [grContextPtr, true])!;


            factory.MakeWindowAssociation(hwnd.Value, WindowAssociationFlags.IgnoreAltEnter);

            var fence = device.CreateFence(0);

            return new Win32RendererD3D12Context(grContext, hwnd, factory, device, queue, fence);
        }

        private const int _BufferCount = 2;

        protected unsafe override void CreateSurfaces(int width, int height)
        {
            if (_swapChain == null)
            {
                SwapChainDescription1 swapChainDescription = new SwapChainDescription1
                {
                    BufferCount = _BufferCount,
                    Width = width,
                    Height = height,
                    Format = Format.R8G8B8A8_UNorm,
                    BufferUsage = Usage.RenderTargetOutput,
                    SwapEffect = SwapEffect.FlipDiscard,
                    Scaling = Scaling.None,
                    SampleDescription = new SampleDescription
                    {
                        Count = 1
                    }
                };
                var swapChain = _factory.CreateSwapChainForHwnd(_queue, _hwnd.Value, swapChainDescription);
                _swapChain = swapChain.QueryInterface<IDXGISwapChain3>();

                _surfaces = new SKSurface[_BufferCount];
            }
            else
            {
                GRContext!.Flush();
                GRContext.Submit(true);
                WaitIdle();
                for (int i = 0; i < _BufferCount; i++)
                {
                    _surfaces![i].Dispose();
                    _resources[i].Release();
                    //_resources[i].Dispose();
                    _renderTargets[i].Dispose();
                }

                var result = _swapChain.ResizeBuffers(0, width, height, Format.R8G8B8A8_UNorm, SwapChainFlags.None);
                if (!result.Success)
                {

                }
            }
            for (int i = 0; i < _BufferCount; i++)
            {
                var textureInfo = new GRD3D12TextureInfo();
                var resource = _swapChain.GetBuffer<ID3D12Resource>(i);
                textureInfo.fResource = resource.NativePointer;
                textureInfo.fResourceState = ResourceStates.Present;
                textureInfo.fFormat = Format.R8G8B8A8_UNorm;
                textureInfo.fSampleCount = 1;
                textureInfo.fLevelCount = 1;
                textureInfo.fSampleQualityPattern = 0;
                textureInfo.fProtected = false;

                //var backendTexturePtr = gr_backendtexture_new_direct3d(width, height, &textureInfo);
                //var backendTexture = (GRBackendTexture)typeof(GRBackendTexture).GetConstructor(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, [typeof(nint), typeof(bool)])!
                //    .Invoke([backendTexturePtr, true]);

                var renderTargetPtr = gr_backendrendertarget_new_direct3d(width, height, &textureInfo);
                var renderTarget = (GRBackendRenderTarget)typeof(GRBackendRenderTarget).GetConstructor(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, [typeof(nint), typeof(bool)])!
                    .Invoke([renderTargetPtr, true]);

                _surfaces![i] = SKSurface.Create(GRContext, renderTarget, GRSurfaceOrigin.TopLeft, SKColorType.Rgba8888);
                _resources[i] = resource;
                _renderTargets[i] = renderTarget;
            }
        }

        protected override void DeleteSurfaces()
        {
            if (_surfaces != null)
            {
                for (int i = 0; i < _BufferCount; i++)
                {
                    _surfaces[i].Dispose();
                }
            }
        }

        protected override SKSurface? GetSurface()
        {
            if (_swapChain == null)
                return null;
            var index = _swapChain.CurrentBackBufferIndex;
            var frameCount = _surfaceFrameCounts[index];
            if (_fence.CompletedValue < frameCount)
            {
                _fence.SetEventOnCompletion(frameCount, _fenceEvent);
                _fenceEvent.WaitOne();
            }
            return _surfaces![index];
        }

        protected override void AfterRender()
        {
            GRContext!.Submit();
            var result = _swapChain!.Present(1, PresentFlags.None);
            if (result.Failure)
            {
                if (result.Code == Vortice.DXGI.ResultCode.DeviceRemoved.Code || result.Code == Vortice.DXGI.ResultCode.DeviceReset.Code)
                    return;
            }
            _queue.Signal(_fence, _frameCount);
            if (!_factory.IsCurrent)
            {
                _factory.Dispose();
                _factory = DXGI.CreateDXGIFactory2<IDXGIFactory4>(true);
            }
        }

        private void WaitIdle()
        {
            for (int i = 0; i < _BufferCount; i++)
            {
                var frameCount = _surfaceFrameCounts[i];
                //frameCount++;
                _queue.Signal(_fence, frameCount);
                if (_fence.CompletedValue < frameCount)
                {
                    _fence.SetEventOnCompletion(frameCount, _fenceEvent);
                    _fenceEvent.WaitOne();
                }
            }
        }

        protected override void DisposeCore(bool disposing)
        {
            if (disposing)
            {
                if (_swapChain != null)
                {
                    WaitIdle();
                    _fence.Dispose();
                    _swapChain.Dispose();
                    _queue.Dispose();
                    _device.Dispose();
                }
            }
        }

        [DllImport("libSkiaSharp", CallingConvention = CallingConvention.Cdecl)]
        private unsafe static extern IntPtr gr_direct_context_make_direct3d(GRD3D12BackendContext vkBackendContext);

        [DllImport("libSkiaSharp", CallingConvention = CallingConvention.Cdecl)]
        private unsafe static extern IntPtr gr_backendtexture_new_direct3d(int width, int height, GRD3D12TextureInfo* vkInfo);

        [DllImport("libSkiaSharp", CallingConvention = CallingConvention.Cdecl)]
        private unsafe static extern IntPtr gr_backendrendertarget_new_direct3d(int width, int height, GRD3D12TextureInfo* vkInfo);

        private struct GRD3D12BackendContext
        {
            public nint fAdapter;
            public nint fDevice;
            public nint fQueue;
            public nint fMemoryAllocator;
            public bool fProtectedContext;
        }

        private struct GRD3D12TextureInfo
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
}
