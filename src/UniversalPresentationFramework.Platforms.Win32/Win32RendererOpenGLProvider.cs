using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.Graphics.OpenGL;
using Wodsoft.UI.Renderers;

namespace Wodsoft.UI.Platforms.Win32
{
    public class Win32RendererOpenGLProvider : SkiaRendererOpenGLProvider
    {
        protected Win32RendererOpenGLProvider()
        {

        }

        public unsafe static bool TryCreate([NotNullWhen(true)] out Win32RendererOpenGLProvider? provider)
        {
            PInvoke.glGetString(0);
            using Win32BackendWindow window = new Win32BackendWindow("OpenGL Test", 1, 1);
            var hwnd = window.WindowHandle;
            var hdc = window.DeviceContextHandle;
            PIXELFORMATDESCRIPTOR formatDescriptor = new PIXELFORMATDESCRIPTOR();
            formatDescriptor.nSize = (ushort)sizeof(PIXELFORMATDESCRIPTOR);
            formatDescriptor.nVersion = 1;
            formatDescriptor.dwFlags = PFD_FLAGS.PFD_DRAW_TO_WINDOW | PFD_FLAGS.PFD_SUPPORT_OPENGL;
            formatDescriptor.dwLayerMask = (uint)PFD_LAYER_TYPE.PFD_MAIN_PLANE;
            formatDescriptor.iPixelType = PFD_PIXEL_TYPE.PFD_TYPE_RGBA;
            formatDescriptor.cColorBits = 32;
            formatDescriptor.cDepthBits = 24;
            formatDescriptor.iLayerType = PFD_LAYER_TYPE.PFD_MAIN_PLANE;
            var format = PInvoke.ChoosePixelFormat(hdc, formatDescriptor);
            if (format == 0)
            {
                provider = null;
                return false;
            }
            if (!PInvoke.SetPixelFormat(hdc, format, formatDescriptor))
            {
                provider = null;
                return false;
            }
            var glContext = PInvoke.wglCreateContext(hdc);
            if (glContext == default)
            {
                var error = Marshal.GetLastPInvokeErrorMessage();
                provider = null;
                return false;
            }
            if (!PInvoke.wglMakeCurrent(hdc, glContext))
            {
                var error = Marshal.GetLastPInvokeErrorMessage();
                PInvoke.wglDeleteContext(glContext);
                provider = null;
                return false;
            }
            var versionPtr = PInvoke.glGetString(PInvoke.GL_VERSION);
            if (versionPtr == default)
            {
                provider = null;
                return false;
            }
            var versionStr = Marshal.PtrToStringAnsi(new nint(versionPtr))!;
            var versionStrs = versionStr.Split(' ');
            var version = new Version(versionStrs[0]);
            PInvoke.wglDeleteContext(glContext);
            if (version.Major < 2)
            {
                provider = null;
                return false;
            }
            provider = new Win32RendererOpenGLProvider();
            return true;
        }
    }
}
