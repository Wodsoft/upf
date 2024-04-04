using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.WindowsAndMessaging;
using Wodsoft.UI.Renderers;
using Wodsoft.UI.Threading;

namespace Wodsoft.UI.Platforms.Win32
{
    public class WindowContext : IWindowContext, IDisposable
    {
        private FreeLibrarySafeHandle _instance;
        private HWND _hwnd;

        private string _className;
        private bool _disposed, _topMost, _inputProcessing, _isActivated, _isDestoryed;
        private string _title = string.Empty;
        private Thread _windowThread, _dispatcherThread;
        private int _x, _y, _width, _height, _clientWidth, _clientHeight, _clientX, _clientY;
        private float _dpiX, _dpiY;
        private WindowState _state;
        private WindowStyle _style;
        private readonly Window _window;
        private readonly Action _themeChanged;
        private readonly Win32Dispatcher _dispatcher;
        private SkiaRendererContext? _rendererContext;

        public WindowContext(Window window, Action themeChanged)
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
            _windowThread.Name = "Window HWND Loop";
            _dispatcherThread = new Thread(ProcessDispatcher);
            _dispatcherThread.Name = "Window Dispatcher";
            _dispatcher = new Win32Dispatcher(this, _dispatcherThread);
            _window = window;
            _themeChanged = themeChanged;
            //_thread.SetApartmentState(ApartmentState.STA);
        }

        public bool IsClosing { get; private set; }

        public Dispatcher Dispatcher => _dispatcher;

        internal Window Window => _window;

        internal HWND Hwnd => _hwnd;

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

        public float DpiX => _dpiX;

        public float DpiY => _dpiY;

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

        public int ClientX => _clientX;

        public int ClientY => _clientY;

        public int ClientWidth => _clientWidth;

        public int ClientHeight => _clientHeight;

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
        public event WindowContextEventHandler? IsActivateChanged;
        public event WindowContextEventHandler? LocationChanged;
        public event WindowContextEventHandler? StateChanged;
        public event WindowContextEventHandler? Disposed;
        public event WindowContextEventHandler? Opened;
        public event WindowContextEventHandler<DpiScale>? DpiChanged;
        public event WindowContextEventHandler? SizeChanged;

        #endregion

        #region Window Process

        [MemberNotNull(nameof(_rendererContext))]
        private void EnsureRendererContext()
        {
            //if (_rendererContext == null)
            //    _rendererContext = Win32RendererD3D12Context.Create();
            if (_rendererContext == null)
                _rendererContext = SkiaRendererVulkanContext.CreateFromWindowHandle(_instance.DangerousGetHandle(), _hwnd);
            if (_rendererContext == null)
                _rendererContext = Win32RendererOpenGLContext.Create(_hwnd);
            if (_rendererContext == null)
                _rendererContext = new Win32RendererSoftwareContext(_hwnd);
        }

        private void ProcessDispatcher()
        {
            EnsureRendererContext();
            try
            {
                //PAINTSTRUCT paint;
                ulong startTick, endTick;
                var frameTime = 1000d / 60d * 1000;
                TimeSpan waitTime;
                while (!_disposed && !_hwnd.IsNull && !_isDestoryed)
                {
                    PInvoke.QueryUnbiasedInterruptTime(out startTick);

                    _dispatcher.RunFrame();

                    PInvoke.QueryUnbiasedInterruptTime(out endTick);
                    waitTime = TimeSpan.FromMicroseconds(frameTime - (endTick - startTick) / 10);
                    if (waitTime.TotalMilliseconds > 0)
                        Thread.Sleep(waitTime);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine(ex);
#endif
            }
            finally
            {
                _rendererContext.Dispose();
            }
        }

        internal void Render()
        {
            _rendererContext!.Render(_window);
        }

        private unsafe void ProcessWindow()
        {
            if (PInvoke.RegisterClassEx(GetWindowClass()) == 0)
                throw new Win32Exception(Marshal.GetLastPInvokeError());
            PInvoke.SetProcessDpiAwareness(Windows.Win32.UI.HiDpi.PROCESS_DPI_AWARENESS.PROCESS_PER_MONITOR_DPI_AWARE);
            int x = -1, y = -1, width = _width, height = _height;
            System.Drawing.Point point;
            var startupLocation = _window.WindowStartupLocation;
            switch (startupLocation)
            {
                case WindowStartupLocation.Manual:
                    {
                        x = _x;
                        y = _y;
                        if (x < 0 || y < 0)
                            point = new System.Drawing.Point();
                        else
                            point = new System.Drawing.Point(x, y);
                        break;
                    }
                case WindowStartupLocation.CenterScreen:
                    PInvoke.GetCursorPos(out point);
                    break;
                case WindowStartupLocation.CenterOwner:
                default:
                    throw new NotImplementedException();
            }
            var monitor = PInvoke.MonitorFromPoint(point, MONITOR_FROM_FLAGS.MONITOR_DEFAULTTONEAREST);
            if (monitor.Value != default)
            {
                MONITORINFO monitorInfo = new MONITORINFO();
                monitorInfo.cbSize = (uint)sizeof(MONITORINFO);
                if (PInvoke.GetMonitorInfo(monitor, ref monitorInfo))
                {
                    float scaleX, scaleY;
                    if (PInvoke.GetDpiForMonitor(monitor, Windows.Win32.UI.HiDpi.MONITOR_DPI_TYPE.MDT_DEFAULT, out var dpiX, out var dpiY).Succeeded)
                    {
                        scaleX = dpiX / 96f;
                        scaleY = dpiY / 96f;
                    }
                    else
                        scaleX = scaleY = 1f;
                    width = (int)(width * scaleX);
                    height = (int)(height * scaleY);
                    if (startupLocation != WindowStartupLocation.Manual)
                    {
                        x = monitorInfo.rcWork.X + (monitorInfo.rcWork.Width - width) / 2;
                        y = monitorInfo.rcWork.Y + (monitorInfo.rcWork.Height - height) / 2;
                    }
                }
            }
            var windowPtr = PInvoke.CreateWindowEx(
                GetExStyle(),
                _className,
                _title,
                GetStyle(),
                x, y, width, height,
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

        private bool _locationChanged, _sizeChanged, _isActivateChanged, _stateChanged;
        internal void ProcessInput()
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
                SizeChanged?.Invoke(this);
            }
            if (_isActivateChanged)
            {
                _isActivateChanged = false;
                IsActivateChanged?.Invoke(this);
            }
            if (_stateChanged)
            {
                _stateChanged = false;
                StateChanged?.Invoke(this);
            }
            _inputProcessing = false;
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
                        _isDestoryed = true;
                        _dispatcherThread.Join();
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
                        _dpiX = x / 96f;
                        _dpiY = y / 96f;
                        DpiChanged?.Invoke(this, new DpiScale(_dpiX, _dpiY));
                        //if (!PInvoke.GetClientRect(hwnd, out var rect))
                        //    throw new Win32Exception(Marshal.GetLastPInvokeError());
                        //SizeChanged?.Invoke(this, new Size(rect.Width, rect.Height));
                        Opened?.Invoke(this);
                        //new Thread(ProcessUI).Start();
                        //Task.Run(ProcessUI);
                        break;
                    }
                case PInvoke.WM_DPICHANGED:
                    {
                        var x = wParam.Value & 0xffff;
                        var y = wParam.Value >> 16;
                        _dpiX = x / 96f;
                        _dpiY = y / 96f;
                        DpiChanged?.Invoke(this, new DpiScale(_dpiX, _dpiY));
                        break;
                    }
                case PInvoke.WM_PAINT:
                    {
                        if (_dispatcherThread.ThreadState == ThreadState.Unstarted)
                        {
                            SizeChanged?.Invoke(this);
                            _dispatcherThread.Start();
                        }
                        _dispatcher.UpdateRender();
                        break;
                    }
                case PInvoke.WM_MOVING:
                    {
                        ref RECT rect = ref Unsafe.AsRef<RECT>((void*)lParam.Value);
                        _x = rect.X;
                        _y = rect.Y;
                        PInvoke.GetClientRect(hwnd, out var clientRect);
                        _clientX = clientRect.X;
                        _clientY = clientRect.Y;
                        _locationChanged = true;
                        break;
                    }
                case PInvoke.WM_SIZE:
                    {
                        switch (wParam.Value)
                        {
                            case 2:
                                if (_state != WindowState.Maximized)
                                {
                                    _state = WindowState.Maximized;
                                    _stateChanged = true;
                                }
                                break;
                            case 1:
                                if (_state != WindowState.Minimized)
                                {
                                    _state = WindowState.Minimized;
                                    _stateChanged = true;
                                }
                                break;
                            case 0:
                                if (_state != WindowState.Normal)
                                {
                                    _state = WindowState.Normal;
                                    _stateChanged = true;
                                }
                                break;
                        }
                        if (_isActivated && lParam.Value != 0)
                        {
                            PInvoke.GetWindowRect(hwnd, out var rect);
                            if (_x != rect.X)
                            {
                                _x = rect.X;
                                _locationChanged = true;
                            }
                            if (_y != rect.Y)
                            {
                                _y = rect.Y;
                                _locationChanged = true;
                            }
                            PInvoke.GetClientRect(hwnd, out var clientRect);
                            _clientX = clientRect.X;
                            _clientY = clientRect.Y;
                            _clientWidth = (int)(lParam.Value & 0xffff);
                            _clientHeight = (int)(lParam.Value >> 16);
                            _width = (int)(_clientWidth / _dpiX);
                            _height = (int)(_clientHeight / DpiY);
                            _sizeChanged = true;
                            _window.Width = _width;
                            _window.Height = _height;
                            _window.UpdateLayout();
                            //_window.Arrange(new Rect(0, 0, _clientWidth, _clientHeight));
                        }
                        break;
                    }
                case PInvoke.WM_NCACTIVATE:
                    {
                        _isActivated = wParam == 1;
                        _isActivateChanged = true;
                        break;
                    }
                case PInvoke.WM_THEMECHANGED:
                    {
                        _themeChanged();
                        break;
                    }
                case PInvoke.WM_MOUSEMOVE:
                    {
                        int x = lParam.Value.ToInt32() & 0xffff;
                        int y = lParam.Value.ToInt32() >> 16;
                        var time = PInvoke.GetMessageTime();
                        _dispatcher.PushMouseInput(time, Input.MouseActions.Move, null, x, y, 0);
                        break;
                    }
                case PInvoke.WM_MOUSEWHEEL:
                    {
                        int x = lParam.Value.ToInt32() & 0xffff;
                        int y = lParam.Value.ToInt32() >> 16;
                        int wheel = unchecked((int)wParam.Value.ToUInt32()) >> 16;
                        var time = PInvoke.GetMessageTime();
                        _dispatcher.PushMouseInput(time, Input.MouseActions.Wheel, null, x, y, wheel);
                        break;
                    }
                case PInvoke.WM_LBUTTONDBLCLK:
                case PInvoke.WM_LBUTTONDOWN:
                    {
                        int x = lParam.Value.ToInt32() & 0xffff;
                        int y = lParam.Value.ToInt32() >> 16;
                        var time = PInvoke.GetMessageTime();
                        _dispatcher.PushMouseInput(time, Input.MouseActions.Press, Input.MouseButton.Left, x, y, 0);
                        break;
                    }
                case PInvoke.WM_LBUTTONUP:
                    {
                        int x = lParam.Value.ToInt32() & 0xffff;
                        int y = lParam.Value.ToInt32() >> 16;
                        var time = PInvoke.GetMessageTime();
                        _dispatcher.PushMouseInput(time, Input.MouseActions.Release, Input.MouseButton.Left, x, y, 0);
                        break;
                    }
                case PInvoke.WM_RBUTTONDBLCLK:
                case PInvoke.WM_RBUTTONDOWN:
                    {
                        int x = lParam.Value.ToInt32() & 0xffff;
                        int y = lParam.Value.ToInt32() >> 16;
                        var time = PInvoke.GetMessageTime();
                        _dispatcher.PushMouseInput(time, Input.MouseActions.Press, Input.MouseButton.Right, x, y, 0);
                        break;
                    }
                case PInvoke.WM_RBUTTONUP:
                    {
                        int x = lParam.Value.ToInt32() & 0xffff;
                        int y = lParam.Value.ToInt32() >> 16;
                        var time = PInvoke.GetMessageTime();
                        _dispatcher.PushMouseInput(time, Input.MouseActions.Release, Input.MouseButton.Right, x, y, 0);
                        break;
                    }
                case PInvoke.WM_MBUTTONDBLCLK:
                case PInvoke.WM_MBUTTONDOWN:
                    {
                        int x = lParam.Value.ToInt32() & 0xffff;
                        int y = lParam.Value.ToInt32() >> 16;
                        var time = PInvoke.GetMessageTime();
                        _dispatcher.PushMouseInput(time, Input.MouseActions.Press, Input.MouseButton.Middle, x, y, 0);
                        break;
                    }
                case PInvoke.WM_MBUTTONUP:
                    {
                        int x = lParam.Value.ToInt32() & 0xffff;
                        int y = lParam.Value.ToInt32() >> 16;
                        var time = PInvoke.GetMessageTime();
                        _dispatcher.PushMouseInput(time, Input.MouseActions.Release, Input.MouseButton.Middle, x, y, 0);
                        break;
                    }
                case PInvoke.WM_XBUTTONDBLCLK:
                case PInvoke.WM_XBUTTONDOWN:
                    {
                        int x = lParam.Value.ToInt32() & 0xffff;
                        int y = lParam.Value.ToInt32() >> 16;
                        var time = PInvoke.GetMessageTime();
                        _dispatcher.PushMouseInput(time, Input.MouseActions.Press, Input.MouseButton.XButton1, x, y, 0);
                        break;
                    }
                case PInvoke.WM_XBUTTONUP:
                    {
                        int x = lParam.Value.ToInt32() & 0xffff;
                        int y = lParam.Value.ToInt32() >> 16;
                        var time = PInvoke.GetMessageTime();
                        _dispatcher.PushMouseInput(time, Input.MouseActions.Release, Input.MouseButton.XButton1, x, y, 0);
                        break;
                    }
                case PInvoke.WM_CAPTURECHANGED:
                    {
                        var time = PInvoke.GetMessageTime();
                        _dispatcher.PushMouseInput(time, Input.MouseActions.CancelCapture, null, 0, 0, 0);
                        break;
                    }
                case _InsertMessage:
                    while (_insertMessages.TryPop(out var action))
                        action();
                    break;
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

        private const uint _InsertMessage = 0xffff0000;
        private ConcurrentStack<Action> _insertMessages = new ConcurrentStack<Action>();

        internal void ProcessInWindowThread(Action callback)
        {
            _insertMessages.Push(callback);
            PInvoke.SendMessage(_hwnd, _InsertMessage, default, default);
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
