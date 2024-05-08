using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.WindowsAndMessaging;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Platforms.Win32
{
    internal unsafe class Win32BackendWindow : IDisposable
    {
        private ushort _classRegistration;

        public string WindowClassName { get; }

        public FreeLibrarySafeHandle Instance { get; }

        public HWND WindowHandle { get; private set; }

        public HDC DeviceContextHandle { get; private set; }

        public Win32BackendWindow(string className, int width, int height)
        {
            WindowClassName = className;
            Instance = PInvoke.GetModuleHandle((string?)null);

            fixed (char* classNamePtr = className)
            {
                WNDCLASSEXW wcx;
                // Fill in the window class structure with parameters 
                // that describe the main window. 
#pragma warning disable CS8500 // 这会获取托管类型的地址、获取其大小或声明指向它的指针
                wcx.cbSize = (uint)sizeof(WNDCLASSEXW);          // size of structure 
#pragma warning restore CS8500 // 这会获取托管类型的地址、获取其大小或声明指向它的指针
                wcx.style = WNDCLASS_STYLES.CS_HREDRAW | WNDCLASS_STYLES.CS_VREDRAW | WNDCLASS_STYLES.CS_OWNDC;// redraw if size changes 
                wcx.lpfnWndProc = PInvoke.DefWindowProc;// points to window procedure 
                wcx.cbClsExtra = 0;// no extra class memory 
                wcx.cbWndExtra = 0;// no extra window memory 
                wcx.hInstance = new HINSTANCE(Instance.DangerousGetHandle());// handle to instance 
                wcx.hIcon = HICON.Null;// predefined app. icon 
                wcx.hCursor = HCURSOR.Null;// predefined arrow 
                wcx.hbrBackground = Windows.Win32.Graphics.Gdi.HBRUSH.Null;// white background brush 
                wcx.lpszMenuName = null;// name of menu resource 
                wcx.lpszClassName = new PCWSTR(classNamePtr);  // name of window class 
                wcx.hIconSm = HICON.Null; // small class icon
                _classRegistration = PInvoke.RegisterClassEx(wcx);
            }

            if (_classRegistration == 0)
                throw new Exception($"Could not register window class: {className}");

            WindowHandle = PInvoke.CreateWindowEx(WINDOW_EX_STYLE.WS_EX_NOACTIVATE | WINDOW_EX_STYLE.WS_EX_TRANSPARENT,
                WindowClassName,
                $"UPF Backend Window",
                WINDOW_STYLE.WS_OVERLAPPED,
                0, 0,
                width, height,
                HWND.Null, null, Instance, null);
            if (WindowHandle == IntPtr.Zero)
                throw new Exception($"Could not create window: {className}");

            DeviceContextHandle = PInvoke.GetDC(WindowHandle);
            if (DeviceContextHandle == IntPtr.Zero)
            {
                Dispose();
                throw new Exception($"Could not get device context: {className}");
            }
        }

        public void Dispose()
        {
            if (!WindowHandle.IsNull)
            {
                if (DeviceContextHandle != IntPtr.Zero)
                {
                    PInvoke.ReleaseDC(WindowHandle, DeviceContextHandle);
                    DeviceContextHandle = default;
                }

                PInvoke.DestroyWindow(WindowHandle);
                WindowHandle = default;
            }

            if (_classRegistration != 0)
            {
                PInvoke.UnregisterClass(WindowClassName, Instance);
                _classRegistration = 0;
            }
        }
    }
}
