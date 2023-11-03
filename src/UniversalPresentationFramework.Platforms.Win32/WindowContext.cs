using SharpVk;
using SharpVk.Khronos;
using SharpVk.Spirv;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.WindowsAndMessaging;
using Wodsoft.UI.Renderers;

namespace Wodsoft.UI.Platforms.Win32
{
    public class WindowContext : IWindowContext, IDisposable
    {
        private FreeLibrarySafeHandle _instance;
        private HWND _hwnd;

        private string _className;
        private bool _disposed, _topMost, _inputProcessing, _isActivated;
        private string _title = string.Empty;
        private Thread _windowThread, _uiThread, _inputThread;
        private int _x, _y, _width, _height;
        private WindowState _state;
        private WindowStyle _style;
        private readonly Window _window;

        public WindowContext(Window window)
        {
            if (window == null)
                throw new ArgumentNullException(nameof(window));
            Title = window.Title;
            X = (int)window.Left;
            Y = (int)window.Top;
            Width = (int)window.Width;
            Height = (int)window.Height;
            State = window.WindowState;
            Style = window.WindowStyle;
            StartupLocation = window.WindowStartupLocation;
            AllowsTransparency = window.AllowsTransparency;
            _instance = PInvoke.GetModuleHandle((string?)null);
            _className = "upfwindow_" + Guid.NewGuid().ToString().Replace("-", "");
            _windowThread = new Thread(ProcessWindow);
            _uiThread = new Thread(ProcessUI);
            _inputThread = new Thread(ProcessInput);
            _window = window;
            //_thread.SetApartmentState(ApartmentState.STA);
        }

        public bool IsClosing { get; private set; }

        #region Window Properties

        public string Title
        {
            get => _title;
            set
            {
                if (value == null)
                    value = string.Empty;
                _title = value;
            }
        }

        public bool TopMost
        {
            get => _topMost;
            set
            {
                _topMost = value;
            }
        }

        public int X
        {
            get => _x;
            set
            {
                _x = value;
            }
        }

        public int Y
        {
            get => _y;
            set
            {
                _y = value;
            }
        }

        public int Width
        {
            get => _width;
            set
            {
                if (value <= 0)
                    value = 1;
                _width = value;
            }
        }

        public int Height
        {
            get => _height;
            set
            {
                if (value <= 0)
                    value = 1;
                _height = value;
            }
        }

        public WindowStartupLocation StartupLocation { get; set; }

        public WindowState State
        {
            get => _state;
            set
            {
                _state = value;
            }
        }

        public WindowStyle Style
        {
            get => _style;
            set
            {
                _style = value;
            }
        }

        public bool AllowsTransparency { get; set; }

        public bool IsInputProcessing => _inputProcessing;

        public bool IsActivated => _isActivated;

        #endregion

        #region Window Operations

        public void Hide()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(WindowContext));
            if (!_hwnd.IsNull)
            {
                if (!PInvoke.ShowWindow(_hwnd, SHOW_WINDOW_CMD.SW_HIDE))
                    throw new Win32Exception(Marshal.GetLastPInvokeError());
            }
        }

        public void Show()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(WindowContext));
            if (_hwnd.IsNull)
            {
                _windowThread.Start();
            }
            else
            {
                if (!PInvoke.ShowWindow(_hwnd, SHOW_WINDOW_CMD.SW_SHOWNORMAL))
                    throw new Win32Exception(Marshal.GetLastPInvokeError());
            }
        }

        public void Close()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(WindowContext));
            if (_hwnd.IsNull)
                return;
            if (OnClosing())
                DestoryWindow();
        }

        private bool OnClosing()
        {
            if (Closing == null)
                return true;
            var e = new CancelEventArgs();
            Closing(this, e);
            return !e.Cancel;
        }

        private void DestoryWindow()
        {
            var hwnd = _hwnd;
            if (!hwnd.IsNull)
            {
                _hwnd = HWND.Null;
                PInvoke.DestroyWindow(hwnd);
                Closed?.Invoke(this);
            }
        }

        #endregion

        #region Window Events

        public event CancelEventHandler? Closing;
        public event WindowContextEventHandler? Closed;
        public event WindowContextEventHandler? IsActivatedChanged;
        public event WindowContextEventHandler? LocationChanged;
        public event WindowContextEventHandler? StateChanged;
        public event WindowContextEventHandler? Disposed;
        public event WindowContextEventHandler? Opened;
        public event WindowContextEventHandler<DpiScale>? DpiChanged;
        public event WindowContextEventHandler<Size>? SizeChanged;        

        #endregion

        #region Window Process

        private unsafe void ProcessWindow()
        {
            if (PInvoke.RegisterClassEx(GetWindowClass()) == 0)
                throw new Win32Exception(Marshal.GetLastPInvokeError());
            var windowPtr = PInvoke.CreateWindowEx(
                GetExStyle(),
                _className,
                _title,
                GetStyle(),
                _x, _y, _width, _height,
                HWND.Null, null, _instance, null);
            if (windowPtr == IntPtr.Zero)
                throw new Win32Exception(Marshal.GetLastPInvokeError());
            _hwnd = new HWND(windowPtr);
            if (!PInvoke.ShowWindow(_hwnd, SHOW_WINDOW_CMD.SW_SHOWNORMAL))
                throw new Win32Exception(Marshal.GetLastPInvokeError());
            if (!PInvoke.UpdateWindow(_hwnd))
                throw new Win32Exception(Marshal.GetLastPInvokeError());
            MSG msg;
            while (true)
            {
                if (_disposed || _hwnd.IsNull)
                    break;
                if (PInvoke.GetMessage(out msg, _hwnd, 0, 0))
                {
                    PInvoke.TranslateMessage(msg);
                    PInvoke.DispatchMessage(msg);
                }
            }
        }

        private void ProcessUI()
        {
            var rendererContext = SkiaRendererVulkanContext.CreateFromWindowHandle(_instance.DangerousGetHandle(), _hwnd);
            PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromMilliseconds(1000d / 60d));
            try
            {
                //PAINTSTRUCT paint;
                while (!_disposed && !_hwnd.IsNull)
                {
                    rendererContext.Render(_window);
                    //timer.WaitForNextTickAsync().AsTask().Wait();
                    Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine(ex);
#endif
            }
            rendererContext.Dispose();
        }

        private bool _locationChanged, _sizeChanged, _isActivatedChanged;
        private void ProcessInput()
        {
            while (!_disposed && !_hwnd.IsNull)
            {
                _inputProcessing = true;
                if (_locationChanged)
                {
                    _locationChanged = false;
                    LocationChanged?.Invoke(this);
                }
                if (_sizeChanged)
                {
                    _sizeChanged = false;
                    SizeChanged?.Invoke(this, new Size(_width, _height));
                }
                if (_isActivatedChanged)
                {
                    _isActivatedChanged = false;
                    if (_isActivated)
                        IsActivatedChanged?.Invoke(this);
                }
                _inputProcessing = false;
                Thread.Sleep(10);
            }
        }

        private unsafe WNDCLASSEXW GetWindowClass()
        {
            fixed (char* className = _className)
            {
                WNDCLASSEXW wcx;
                // Fill in the window class structure with parameters 
                // that describe the main window. 
#pragma warning disable CS8500 // 这会获取托管类型的地址、获取其大小或声明指向它的指针
                wcx.cbSize = (uint)sizeof(WNDCLASSEXW);          // size of structure 
#pragma warning restore CS8500 // 这会获取托管类型的地址、获取其大小或声明指向它的指针
                wcx.style = WNDCLASS_STYLES.CS_HREDRAW | WNDCLASS_STYLES.CS_VREDRAW | WNDCLASS_STYLES.CS_DBLCLKS | WNDCLASS_STYLES.CS_OWNDC;                    // redraw if size changes 
                wcx.lpfnWndProc = WndProc;     // points to window procedure 
                wcx.cbClsExtra = 0;                // no extra class memory 
                wcx.cbWndExtra = 0;                // no extra window memory 
                wcx.hInstance = new HINSTANCE(_instance.DangerousGetHandle());         // handle to instance 
                wcx.hIcon = HICON.Null;              // predefined app. icon 
                wcx.hCursor = HCURSOR.Null;                    // predefined arrow 
                wcx.hbrBackground = HBRUSH.Null;                  // white background brush 
                wcx.lpszMenuName = null;    // name of menu resource 
                wcx.lpszClassName = new PCWSTR(className);  // name of window class 
                wcx.hIconSm = HICON.Null; // small class icon
                return wcx;
            }
        }

        private unsafe LRESULT WndProc(HWND hwnd, uint msg, WPARAM wParam, LPARAM lParam)
        {
            switch (msg)
            {
                case PInvoke.WM_DESTROY:
                    {
                        DestoryWindow();
                        break;
                    }
                case PInvoke.WM_CREATE:
                    {
                        _hwnd = hwnd;
                        var dc = PInvoke.GetDC(hwnd);
                        var x = PInvoke.GetDeviceCaps(dc, GET_DEVICE_CAPS_INDEX.LOGPIXELSX);
                        var y = PInvoke.GetDeviceCaps(dc, GET_DEVICE_CAPS_INDEX.LOGPIXELSY);
                        PInvoke.ReleaseDC(hwnd, dc);
                        DpiChanged?.Invoke(this, new DpiScale(x / 96f, y / 96f));
                        //if (!PInvoke.GetClientRect(hwnd, out var rect))
                        //    throw new Win32Exception(Marshal.GetLastPInvokeError());
                        //SizeChanged?.Invoke(this, new Size(rect.Width, rect.Height));
                        Opened?.Invoke(this);
                        //new Thread(ProcessUI).Start();
                        //Task.Run(ProcessUI);
                        _inputThread.Start();
                        break;
                    }
                case PInvoke.WM_DPICHANGED:
                    {
                        var x = wParam.Value & 0xffff;
                        var y = wParam.Value >> 16;
                        DpiChanged?.Invoke(this, new DpiScale(x / 96f, y / 96f));
                        break;
                    }
                case PInvoke.WM_PAINT:
                    {
                        if (_uiThread.ThreadState == ThreadState.Unstarted)
                            _uiThread.Start();
                        //if (_rendererContext == null)
                        //    _rendererContext = SkiaRendererVulkanContext.CreateFromWindowHandle(_instance.DangerousGetHandle(), _hwnd);
                        //_rendererContext!.Render(_window);
                        break;
                    }
                case PInvoke.WM_MOVING:
                    {
                        ref RECT rect = ref Unsafe.AsRef<RECT>((void*)lParam.Value);
                        _x = rect.X;
                        _y = rect.Y;
                        _locationChanged = true;
                        break;
                    }
                case PInvoke.WM_SIZE:
                    {
                        if (_isActivated)
                        {
                            _width = (int)(lParam.Value & 0xffff);
                            _height = (int)(lParam.Value >> 16);
                            _sizeChanged = true;
                        }
                        break;
                    }
                case PInvoke.WM_NCACTIVATE:
                    {
                        _isActivated = wParam == 1;
                        _isActivatedChanged = true;
                        break;
                    }
            }
            return PInvoke.DefWindowProc(hwnd, msg, wParam, lParam);
        }

        private WINDOW_EX_STYLE GetExStyle()
        {
            WINDOW_EX_STYLE style;
            switch (Style)
            {
                case WindowStyle.ThreeDBorderWindow:
                    style = WINDOW_EX_STYLE.WS_EX_CLIENTEDGE;
                    break;
                case WindowStyle.ToolWindow:
                    style = WINDOW_EX_STYLE.WS_EX_TOOLWINDOW;
                    break;
                default:
                    style = WINDOW_EX_STYLE.WS_EX_LEFT;
                    break;
            }
            if (TopMost)
                style |= WINDOW_EX_STYLE.WS_EX_TOPMOST;
            if (AllowsTransparency)
                style |= WINDOW_EX_STYLE.WS_EX_TRANSPARENT;
            return style;
        }

        private WINDOW_STYLE GetStyle()
        {
            var style = WINDOW_STYLE.WS_VISIBLE | WINDOW_STYLE.WS_OVERLAPPEDWINDOW;
            if (Style != WindowStyle.None)
                style |= WINDOW_STYLE.WS_BORDER | WINDOW_STYLE.WS_CAPTION;
            return style;
        }

        #endregion

        #region Dispose

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                }

                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                if (!_hwnd.IsNull)
                {
                    DestoryWindow();
                }
                _instance.Dispose();
                _disposed = true;
                Disposed?.Invoke(this);
            }
        }

        // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        ~WindowContext()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
