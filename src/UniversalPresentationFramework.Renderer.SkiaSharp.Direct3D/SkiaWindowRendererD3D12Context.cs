using SkiaSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D12;
using Vortice.DXGI;

namespace Wodsoft.UI.Renderers
{
    public class SkiaWindowRendererD3D12Context : SkiaWindowRendererContext
    {
        private readonly ISkiaDirect3DContext _context;
        private readonly ISkiaWindowDirect3DContext _windowContext;
        private readonly ulong[] _surfaceFrameCounts;
        private GRContext? _grContext;
        private ID3D12CommandQueue? _queue;
        private IDXGISwapChain3? _swapChain;
        private ID3D12Resource[]? _resources;
        private GRBackendRenderTarget[]? _renderTargets;
        private ID3D12Fence? _fence;
        private ulong _frameCount;
        private AutoResetEvent? _fenceEvent;

        public SkiaWindowRendererD3D12Context(ISkiaDirect3DContext context, ISkiaWindowDirect3DContext windowContext)
        {
            _context = context;
            _windowContext = windowContext;
            _surfaceFrameCounts = new ulong[windowContext.BufferCount];
        }

        public override GRContext GRContext
        {
            get
            {
                if (_grContext == null)
                {
                    var grContextPtr = D3D12Helper.gr_direct_context_make_direct3d(new GRD3D12BackendContext
                    {
                        fAdapter = _context.Adapter.NativePointer,
                        fDevice = _context.Device.NativePointer,
                        fQueue = Queue.NativePointer,
                        fProtectedContext = false
                    });

                    if (grContextPtr == default)
                        throw new NotSupportedException("Create Direct3D GRContext failed.");

                    _grContext = (GRContext)typeof(GRContext).GetMethod("GetObject", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static, [typeof(nint), typeof(bool)])!
                        .Invoke(null, [grContextPtr, true])!;
                }
                return _grContext;
            }
        }

        protected ID3D12CommandQueue Queue
        {
            get
            {
                if (_queue == null)
                {
                    _queue = _context.Device.CreateCommandQueue(new CommandQueueDescription
                    {
                        Flags = CommandQueueFlags.None,
                        Type = CommandListType.Direct
                    });
                }
                return _queue;
            }
        }

        public override SKColorType ColorType => _windowContext.ColorType;

        public override SKColorSpace ColorSpace => _windowContext.ColorSpace;

        public override ISkiaWindowContext WindowContext => _windowContext;

        public override GRSurfaceOrigin SurfaceOrigin => GRSurfaceOrigin.TopLeft;

        protected override int CurrentBufferIndex => _swapChain?.CurrentBackBufferIndex ?? 0;

        protected unsafe override GRBackendRenderTarget[] CreateRenderTargets(int width, int height)
        {
            var format = D3D12Helper.GetFormat(ColorType);
            if (_swapChain == null)
            {
                SwapChainDescription1 swapChainDescription = new SwapChainDescription1
                {
                    BufferCount = _windowContext.BufferCount,
                    Width = width,
                    Height = height,
                    Format = format,
                    BufferUsage = Usage.RenderTargetOutput,
                    SwapEffect = SwapEffect.FlipDiscard,
                    Scaling = Scaling.None,
                    SampleDescription = new SampleDescription
                    {
                        Count = _windowContext.SampleCount
                    }
                };
                var swapChain = _context.Factory.CreateSwapChainForHwnd(Queue, _windowContext.WindowHandle, swapChainDescription);
                _swapChain = swapChain.QueryInterface<IDXGISwapChain3>();

                _context.Factory.MakeWindowAssociation(_windowContext.WindowHandle, WindowAssociationFlags.IgnoreAltEnter);

                _resources = new ID3D12Resource[_windowContext.BufferCount];
                _renderTargets = new GRBackendRenderTarget[_windowContext.BufferCount];

                _fence = _context.Device.CreateFence(0);
                _fenceEvent = new AutoResetEvent(false);
            }
            else
            {
                GRContext.Flush();
                GRContext.Submit(true);
                WaitIdle();
                for (int i = 0; i < _resources!.Length; i++)
                {
                    _resources[i].Release();
                    //_resources[i].Dispose();
                    _renderTargets![i].Dispose();
                    _surfaceFrameCounts[i] = 0;
                }
                _frameCount = 0;
                var result = _swapChain.ResizeBuffers(0, _windowContext.Width, _windowContext.Height, format, SwapChainFlags.None);
                if (!result.Success)
                {

                }
            }
            for (int i = 0; i < _windowContext.BufferCount; i++)
            {
                var textureInfo = new GRD3D12TextureInfo();
                var resource = _swapChain.GetBuffer<ID3D12Resource>(i);
                textureInfo.fResource = resource.NativePointer;
                textureInfo.fResourceState = ResourceStates.Present;
                textureInfo.fFormat = format;
                textureInfo.fSampleCount = (uint)_windowContext.SampleCount;
                textureInfo.fLevelCount = 1;
                textureInfo.fSampleQualityPattern = 0;
                textureInfo.fProtected = false;

                //var backendTexturePtr = gr_backendtexture_new_direct3d(width, height, &textureInfo);
                //var backendTexture = (GRBackendTexture)typeof(GRBackendTexture).GetConstructor(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, [typeof(nint), typeof(bool)])!
                //    .Invoke([backendTexturePtr, true]);

                var renderTargetPtr = D3D12Helper.gr_backendrendertarget_new_direct3d(_windowContext.Width, _windowContext.Height, &textureInfo);
                var renderTarget = (GRBackendRenderTarget)typeof(GRBackendRenderTarget).GetConstructor(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, [typeof(nint), typeof(bool)])!
                    .Invoke([renderTargetPtr, true]);

                _resources[i] = resource;
                _renderTargets![i] = renderTarget;
            }
            return _renderTargets!;
        }

        protected override void BeforeRender()
        {
            var index = _swapChain!.CurrentBackBufferIndex;
            var frameCount = _surfaceFrameCounts[index];
            if (_fence!.CompletedValue < frameCount)
            {
                _fence.SetEventOnCompletion(frameCount, _fenceEvent);
                _fenceEvent!.WaitOne();
            }
        }

        protected override void AfterRender()
        {
            GRContext.Submit();
            _frameCount++;
            _surfaceFrameCounts[_swapChain!.CurrentBackBufferIndex] = _frameCount;
            var result = _swapChain!.Present(1, PresentFlags.None);
            if (result.Failure)
            {
                if (result.Code == Vortice.DXGI.ResultCode.DeviceRemoved.Code || result.Code == Vortice.DXGI.ResultCode.DeviceReset.Code)
                    return;
            }
            _queue!.Signal(_fence, _frameCount);
            if (!_context.Factory.IsCurrent)
            {
                //_context.Factory.Dispose();
                //_context.Factory = DXGI.CreateDXGIFactory2<IDXGIFactory4>(true);
            }
        }

        private void WaitIdle()
        {
            for (int i = 0; i < _windowContext.BufferCount; i++)
            {
                var frameCount = _surfaceFrameCounts[i];
                _queue!.Signal(_fence, frameCount);
                if (_fence!.CompletedValue < frameCount)
                {
                    _fence.SetEventOnCompletion(frameCount, _fenceEvent);
                    _fenceEvent!.WaitOne();
                }
            }
        }

        protected override void DisposeCore(bool disposing)
        {
            base.DisposeCore(disposing);
            if (disposing)
            {
                if (_swapChain != null)
                {
                    WaitIdle();
                    _fence!.Dispose();
                    _fence = null;
                    _swapChain.Dispose();
                    _swapChain = null;
                }
                if (_grContext != null)
                {
                    _grContext.Dispose();
                    _grContext = null;
                }
                if (_queue != null)
                {
                    _queue!.Dispose();
                    _queue = null;
                }
            }
        }
    }
}
