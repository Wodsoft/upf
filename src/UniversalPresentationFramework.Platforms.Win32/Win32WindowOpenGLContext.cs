using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.Graphics.OpenGL;
using Wodsoft.UI.Renderers;

namespace Wodsoft.UI.Platforms.Win32
{
    public class Win32WindowOpenGLContext : Win32WindowContext, ISkiaWindowOpenGLContext
    {
        private readonly HWND _hwnd;
        private HDC _hdc;
        private HGLRC _glContext;
        private GRContext? _grContext;
        public unsafe Win32WindowOpenGLContext(WindowContext windowContext) : base(windowContext)
        {
            _hwnd = windowContext.Hwnd;
            _hdc = PInvoke.GetDC(_hwnd);
            PIXELFORMATDESCRIPTOR formatDescriptor = new PIXELFORMATDESCRIPTOR();
            formatDescriptor.nSize = (ushort)sizeof(PIXELFORMATDESCRIPTOR);
            formatDescriptor.nVersion = 1;
            formatDescriptor.dwFlags = PFD_FLAGS.PFD_DRAW_TO_WINDOW | PFD_FLAGS.PFD_SUPPORT_OPENGL | PFD_FLAGS.PFD_DOUBLEBUFFER;
            formatDescriptor.dwLayerMask = (uint)PFD_LAYER_TYPE.PFD_MAIN_PLANE;
            formatDescriptor.iPixelType = PFD_PIXEL_TYPE.PFD_TYPE_RGBA;
            formatDescriptor.cColorBits = 32;
            formatDescriptor.cDepthBits = 24;
            formatDescriptor.iLayerType = PFD_LAYER_TYPE.PFD_OVERLAY_PLANE;
            var format = PInvoke.ChoosePixelFormat(_hdc, formatDescriptor);
            if (format == 0)
            {
                PInvoke.ReleaseDC(_hwnd, _hdc);
                throw new NotSupportedException("OpenGL choose pixel format error.", new Win32Exception(Marshal.GetLastWin32Error()));
            }
            if (!PInvoke.SetPixelFormat(_hdc, format, formatDescriptor))
            {
                PInvoke.ReleaseDC(_hwnd, _hdc);
                throw new NotSupportedException("OpenGL set pixel format error.", new Win32Exception(Marshal.GetLastWin32Error()));
            }
            _glContext = PInvoke.wglCreateContext(_hdc);
            if (_glContext == IntPtr.Zero)
            {
                PInvoke.ReleaseDC(_hwnd, _hdc);
                throw new NotSupportedException("OpenGL create context error.", new Win32Exception(Marshal.GetLastWin32Error()));
            }
        }

        public GRContext GRContext
        {
            get
            {
                if (_grContext == null)
                {
                    PInvoke.wglMakeCurrent(_hdc, _glContext);
                    var glInterface = GRGlInterface.Create();
                    _grContext = GRContext.CreateGl(glInterface);
                }
                return _grContext;
            }
        }

        public int GetInteger(uint index)
        {
            int result = 0;
            PInvoke.glGetIntegerv(index, ref result);
            return result;
        }

        public void MakeCurrent()
        {
            PInvoke.wglMakeCurrent(_hdc, _glContext);
        }

        public void SwapBuffers()
        {
            PInvoke.SwapBuffers(_hdc);
        }

        public override void Dispose()
        {
            if (_glContext != default)
            {
                PInvoke.wglDeleteContext(_glContext);
                _glContext = default;
            }
            if (_hdc != default)
            {
                PInvoke.ReleaseDC(_hwnd, _hdc);
                _hdc = default;
            }
        }
    }
}
