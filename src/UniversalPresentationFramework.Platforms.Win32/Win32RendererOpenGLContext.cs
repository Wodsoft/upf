using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.Graphics.OpenGL;
using Wodsoft.UI.Media;
using Wodsoft.UI.Renderers;

namespace Wodsoft.UI.Platforms.Win32
{
    public sealed class Win32RendererOpenGLContext : SkiaRendererOpenGLContext
    {
        private readonly HWND _hwnd;
        private readonly HDC _hdc;
        private readonly HGLRC _glContext;

        private Win32RendererOpenGLContext(HWND hwnd, HDC hdc, HGLRC glContext, GRContext? grContext) : base(grContext)
        {
            _hwnd = hwnd;
            _hdc = hdc;
            _glContext = glContext;
        }

        internal static unsafe Win32RendererOpenGLContext? Create(HWND hwnd)
        {
            var hdc = PInvoke.GetDC(hwnd);
            PIXELFORMATDESCRIPTOR formatDescriptor = new Windows.Win32.Graphics.OpenGL.PIXELFORMATDESCRIPTOR();
            formatDescriptor.nSize = (ushort)sizeof(PIXELFORMATDESCRIPTOR);
            formatDescriptor.nVersion = 1;
            formatDescriptor.dwFlags = PFD_FLAGS.PFD_DRAW_TO_WINDOW | PFD_FLAGS.PFD_SUPPORT_OPENGL | PFD_FLAGS.PFD_DOUBLEBUFFER;
            formatDescriptor.dwLayerMask = (uint)PFD_LAYER_TYPE.PFD_MAIN_PLANE;
            formatDescriptor.iPixelType = PFD_PIXEL_TYPE.PFD_TYPE_RGBA;
            formatDescriptor.cColorBits = 32;
            formatDescriptor.cDepthBits = 32;
            formatDescriptor.iLayerType = PFD_LAYER_TYPE.PFD_OVERLAY_PLANE;
            var format = PInvoke.ChoosePixelFormat(hdc, formatDescriptor);
            if (format == 0)
            {
                PInvoke.ReleaseDC(hwnd, hdc);
                return null;
            }
            if (!PInvoke.SetPixelFormat(hdc, format, formatDescriptor))
            {
                PInvoke.ReleaseDC(hwnd, hdc);
                return null;
            }
            var glContext = PInvoke.wglCreateContext(hdc);
            if (glContext == IntPtr.Zero)
            {
                PInvoke.ReleaseDC(hwnd, hdc);
                return null;
            }
            if (!PInvoke.wglMakeCurrent(hdc, glContext))
            {
                PInvoke.wglDeleteContext(glContext);
                PInvoke.ReleaseDC(hwnd, hdc);
                return null;
            }
            var glInterface = GRGlInterface.Create();
            if (glInterface == null)
            {
                PInvoke.wglDeleteContext(glContext);
                PInvoke.ReleaseDC(hwnd, hdc);
                return null;
            }
            var grContext = GRContext.CreateGl(glInterface);
            if (grContext == null)
            {
                PInvoke.wglDeleteContext(glContext);
                PInvoke.ReleaseDC(hwnd, hdc);
                return null;
            }
            return new Win32RendererOpenGLContext(hwnd, hdc, glContext, grContext);
        }

        public override void Render(Visual visual)
        {
            PInvoke.wglMakeCurrent(_hdc, _glContext);
            base.Render(visual);
            PInvoke.SwapBuffers(_hdc);
        }

        protected override void DisposeCore(bool disposing)
        {
            base.DisposeCore(disposing);
            if (disposing)
            {                
                PInvoke.wglDeleteContext(_glContext);
                PInvoke.ReleaseDC(_hwnd, _hdc);
            }
        }

        protected override int GetInteger(int code)
        {
            int value = default;
            PInvoke.glGetIntegerv(0, ref value);
            return value;
        }
    }
}
