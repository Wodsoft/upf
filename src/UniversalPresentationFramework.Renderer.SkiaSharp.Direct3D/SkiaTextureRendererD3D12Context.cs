using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D12;
using Vortice.DXGI;

namespace Wodsoft.UI.Renderers
{
    public class SkiaTextureRendererD3D12Context : SkiaTextureRendererContext
    {
        private readonly ISkiaDirect3DContext _context;
        private readonly int _width;
        private readonly int _height;
        private readonly SKColorType _colorType;
        private readonly SKAlphaType _alphaType;
        private readonly SKColorSpace _colorSpace;
        private ID3D12CommandQueue? _queue;
        private GRBackendTexture? _texture;
        private ID3D12Resource? _resource;
        private GRContext? _grContext;

        public SkiaTextureRendererD3D12Context(ISkiaDirect3DContext context, int width, int height, SKColorType colorType, SKAlphaType alphaType, SKColorSpace colorSpace, int sampleCount = 4)
        {
            _context = context;
            _width = width;
            _height = height;
            _colorType = colorType;
            _alphaType = alphaType;
            _colorSpace = colorSpace;
            SampleCount = sampleCount;
        }

        public unsafe override GRBackendTexture Texture
        {
            get
            {
                if (_texture == null)
                {
                    var format = D3D12Helper.GetFormat(_colorType);

                    //ID3D12Resource resource = new ID3D12Resource
                    var resourceDescription = ResourceDescription.Texture2D(format, (uint)_width, (uint)_height, 1, 1, 1, 0, ResourceFlags.AllowRenderTarget);

                    _resource = _context.Device.CreateCommittedResource(HeapProperties.DefaultHeapProperties, HeapFlags.AllowAllBuffersAndTextures, resourceDescription, ResourceStates.RenderTarget);

                    var textureInfo = new GRD3D12TextureInfo();
                    textureInfo.fResource = _resource.NativePointer;
                    textureInfo.fResourceState = ResourceStates.RenderTarget;
                    textureInfo.fFormat = format;
                    textureInfo.fSampleCount = 1;
                    textureInfo.fLevelCount = 1;
                    textureInfo.fSampleQualityPattern = 0;
                    textureInfo.fProtected = false;

                    var texturePtr = D3D12Helper.gr_backendtexture_new_direct3d(_width, _height, &textureInfo);
                    _texture = (GRBackendTexture)typeof(GRBackendTexture).GetConstructor(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, [typeof(nint), typeof(bool)])!
                        .Invoke([texturePtr, true]);
                }
                return _texture;
            }
        }

        public override int SampleCount { get; }

        public override SKAlphaType AlphaType => _alphaType;

        public override SKColorType ColorType => _colorType;

        public override SKColorSpace ColorSpace => _colorSpace;

        public override GRSurfaceOrigin SurfaceOrigin => GRSurfaceOrigin.TopLeft;

        public override GRContext GRContext
        {
            get
            {
                if (_grContext == null)
                {
                    _queue = _context.Device.CreateCommandQueue(new CommandQueueDescription
                    {
                        Flags = CommandQueueFlags.None,
                        Type = CommandListType.Direct
                    });

                    var grContextPtr = D3D12Helper.gr_direct_context_make_direct3d(new GRD3D12BackendContext
                    {
                        fAdapter = _context.Adapter.NativePointer,
                        fDevice = _context.Device.NativePointer,
                        fQueue = _queue.NativePointer,
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

        public override int Width => _width;

        public override int Height => _height;

        protected override void DisposeCore(bool disposing)
        {
            base.DisposeCore(disposing);
            if (disposing)
            {
                if (_grContext != null)
                {
                    _grContext.Dispose();
                    _grContext = null;
                    _queue!.Dispose();
                    _queue = null;
                }
                if (_texture != null)
                {
                    _texture.Dispose();
                    _texture = null;
                    _resource!.Dispose();
                    _resource = null;
                }
            }
        }
    }
}
