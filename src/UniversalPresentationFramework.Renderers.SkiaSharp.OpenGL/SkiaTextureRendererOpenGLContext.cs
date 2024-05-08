//using SkiaSharp;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection.PortableExecutable;
//using System.Text;
//using System.Threading.Tasks;

//namespace Wodsoft.UI.Renderers
//{
//    public class SkiaTextureRendererOpenGLContext : SkiaTextureRendererContext
//    {
//        private readonly ISkiaOpenGLContext _context;
//        private readonly int _width, _height;
//        private readonly SKAlphaType _alphaType;
//        private readonly SKColorType _colorType;
//        private readonly SKColorSpace _colorSpace;
//        private GRContext? _grContext;
//        private GRBackendTexture? _texture;
//        private GRGlTextureInfo _textureInfo;

//        public SkiaTextureRendererOpenGLContext(ISkiaOpenGLContext context, int width, int height, SKColorType colorType, SKAlphaType alphaType, SKColorSpace colorSpace, int sampleCount = 4) : base()
//        {
//            _context = context;
//            _width = width;
//            _height = height;
//            _colorType = colorType;
//            _alphaType = alphaType;
//            _colorSpace = colorSpace;
//            SampleCount = sampleCount;
//        }

//        public override GRBackendTexture Texture
//        {
//            get
//            {
//                if (_texture == null)
//                {
//                    _textureInfo = _context.CreateTexture(_width, _height);
//                    _texture = new GRBackendTexture(_width, _height, false, _textureInfo);
//                }
//                return _texture;
//            }
//        }

//        public override int SampleCount { get; }

//        public override int Width => _width;

//        public override int Height => _height;

//        public override GRContext GRContext
//        {
//            get
//            {
//                if (_grContext == null)
//                    _grContext = _context.CreateGRContext();
//                return _grContext;
//            }
//        }

//        public override SKAlphaType AlphaType => _alphaType;

//        public override SKColorType ColorType => _colorType;

//        public override SKColorSpace ColorSpace => _colorSpace;

//        //protected override SKSurface GetSurface()
//        //{
//        //    if (_surface == null)
//        //    {
//        //        var frameBuffer = GetInteger(0X8ca6);
//        //        var stencil = GetInteger(0x0D57);
//        //        var samples = GetInteger(0X80a9);
//        //        var maxSamples = _grContext.GetMaxSurfaceSampleCount(SKColorType.Rgba8888);
//        //        if (samples > maxSamples)
//        //            samples = maxSamples;
//        //        var glInfo = new GRGlTextureInfo()
//        //        _texture = new GRBackendTexture(_imageInfo.Width, _imageInfo.Height, false, glInfo);
//        //        _renderTarget = new GRBackendRenderTarget(width, height, samples, stencil, glInfo);
//        //        _surface = SKSurface.Create(_grContext, _renderTarget, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888);
//        //    }
//        //}

//        protected override void BeforeRender()
//        {
//            _context.MakeCurrent();
//        }

//        protected override void DisposeCore(bool disposing)
//        {
//            base.DisposeCore(disposing);
//            if (disposing)
//            {
//                if (_texture != null)
//                {
//                    _texture.Dispose();
//                    _texture = null;
//                    _context.DestroyTexture(_textureInfo.Target);
//                }
//                if (_grContext != null)
//                {
//                    _grContext.Dispose();
//                    _grContext = null;
//                }
//            }
//        }
//    }
//}
