using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Controls;
using Wodsoft.UI.Threading;

namespace Wodsoft.UI
{
    public abstract class Window : ContentControl
    {
        static Window()
        {
            WidthProperty.OverrideMetadata(typeof(Window), new PropertyMetadata(OnWidthChanged));
            HeightProperty.OverrideMetadata(typeof(Window), new PropertyMetadata(OnHeightChanged));
        }

        private static void OnWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Window w = (Window)d;
            if (w._context != null && !w._context.IsInputProcessing)
                w._context.Width = (int)(float)e.NewValue!;
        }

        private static void OnHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Window w = (Window)d;
            if (w._context != null && !w._context.IsInputProcessing)
                w._context.Height = (int)(float)e.NewValue!;
        }

        #region Properties

        public static readonly DependencyProperty TitleProperty =
                DependencyProperty.Register("Title", typeof(String), typeof(Window),
                        new FrameworkPropertyMetadata(String.Empty,
                                new PropertyChangedCallback(OnTitleChanged)),
                        new ValidateValueCallback(ValidateText));
        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Window w = (Window)d;
            if (w._context != null)
                w._context.Title = (string)e.NewValue!;
        }
        private static bool ValidateText(object? value)
        {
            return (value != null);
        }
        public string Title { get { return (string)GetValue(TitleProperty)!; } set { SetValue(TitleProperty, value); } }

        public static readonly DependencyProperty LeftProperty = Canvas.LeftProperty.AddOwner(typeof(Window),
                        new FrameworkPropertyMetadata(
                                float.NaN,
                                new PropertyChangedCallback(OnLeftChanged),
                                new CoerceValueCallback(CoerceLeft)));
        private static void OnLeftChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Window w = (Window)d;
            if (w._context != null && !w._context.IsInputProcessing)
                w._context.X = (int)(float)e.NewValue!;
        }
        private static object? CoerceLeft(DependencyObject d, object? value)
        {
            Window w = (Window)d;

            if (value is float left)
            {
                if (float.IsNaN(left) || float.IsInfinity(left))
                {
                    return w._context?.X ?? 0;
                }

                if (w.WindowState != WindowState.Normal)
                {
                    return value;
                }
            }
            return value;
        }
        public float Left { get { return (float)GetValue(LeftProperty)!; } set { SetValue(LeftProperty, value); } }

        public static readonly DependencyProperty TopProperty = Canvas.TopProperty.AddOwner(typeof(Window),
                new FrameworkPropertyMetadata(
                        float.NaN,
                        new PropertyChangedCallback(OnTopChanged),
                        new CoerceValueCallback(CoerceTop)));
        private static void OnTopChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Window w = (Window)d;
            if (w._context != null && !w._context.IsInputProcessing)
                w._context.Y = (int)(float)e.NewValue!;
        }
        private static object? CoerceTop(DependencyObject d, object? value)
        {
            Window w = (Window)d;

            if (value is float top)
            {
                if (float.IsNaN(top) || float.IsInfinity(top))
                {
                    return w._context?.Y ?? 0;
                }

                if (w.WindowState != WindowState.Normal)
                {
                    return value;
                }
            }
            return value;
        }
        public float Top { get { return (float)GetValue(TopProperty)!; } set { SetValue(TopProperty, value); } }

        public static readonly DependencyProperty WindowStateProperty = DependencyProperty.Register("WindowState", typeof(WindowState), typeof(Window),
                new FrameworkPropertyMetadata(
                        WindowState.Normal,
                        FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                        new PropertyChangedCallback(OnWindowStateChanged)));
        private static void OnWindowStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Window w = (Window)d;
            if (w._context != null)
                w._context.State = (WindowState)e.NewValue!;
        }
        public WindowState WindowState { get { return (WindowState)GetValue(WindowStateProperty)!; } set { SetValue(WindowStateProperty, value); } }

        public WindowStartupLocation WindowStartupLocation { get; set; }

        public static readonly DependencyProperty WindowStyleProperty =
        DependencyProperty.Register("WindowStyle", typeof(WindowStyle), typeof(Window),
                new FrameworkPropertyMetadata(
                        WindowStyle.SingleBorderWindow,
                        new PropertyChangedCallback(OnWindowStyleChanged)));
        private static void OnWindowStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Window w = (Window)d;
            if (w._context != null)
                w._context.Style = (WindowStyle)e.NewValue!;
        }
        public WindowStyle WindowStyle { get { return (WindowStyle)GetValue(WindowStyleProperty)!; } set { SetValue(WindowStyleProperty, value); } }

        public static readonly DependencyProperty AllowsTransparencyProperty = DependencyProperty.Register(
                "AllowsTransparency",
                typeof(bool),
                typeof(Window),
                new FrameworkPropertyMetadata(false));
        public bool AllowsTransparency { get { return (bool)GetValue(AllowsTransparencyProperty)!; } set { SetValue(AllowsTransparencyProperty, value); } }

        private static readonly DependencyPropertyKey _IsActivePropertyKey = DependencyProperty.RegisterReadOnly("IsActive", typeof(bool), typeof(Window), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsActiveProperty = _IsActivePropertyKey.DependencyProperty;
        public bool IsActive { get { return (bool)GetValue(IsActiveProperty)!; } private set { SetValue(_IsActivePropertyKey, value); } }

        #endregion

        #region Events

        public event CancelEventHandler? Closing;
        public event EventHandler? Closed;
        public event EventHandler? Activated;
        public event EventHandler? Deactivated;
        public event EventHandler? LocationChanged;
        public event EventHandler? StateChanged;

        #endregion

        #region Operator

        private object _lock = new object();
        private IWindowContext? _context;

        public void Show()
        {
            if (Application.Current == null || !Application.Current.IsRunning)
                throw new InvalidOperationException("Application is not running.");
            lock (_lock)
            {
                if (_context == null)
                {
                    _context = Application.Current!.WindowProvider!.CreateContext(this);
                    _context.Closed += Context_Closed;
                    _context.Closing += Context_Closing;
                    _context.LocationChanged += Context_LocationChanged;
                    _context.StateChanged += Context_StateChanged;
                    _context.IsActivateChanged += Context_IsActivatedChanged;
                    _context.DpiChanged += Context_DpiChanged;
                    _context.SizeChanged += Context_SizeChanged;
                }
                _context.Show();
            }
        }

        private void Context_SizeChanged(IWindowContext context)
        {
            Width = context.Width;
            Height = context.Height;
            _visualSize = new Size(context.ClientWidth / _dpiScale.DpiScaleX, context.ClientHeight / _dpiScale.DpiScaleY);
        }

        private void Context_DpiChanged(IWindowContext context, DpiScale e)
        {
            _dpiScale = e;
        }


        private void Context_IsActivatedChanged(IWindowContext context)
        {
            IsActive = context.IsActivated;
            Activated?.Invoke(this, EventArgs.Empty);
        }

        private void Context_StateChanged(IWindowContext context)
        {
            WindowState = context.State;
            StateChanged?.Invoke(this, EventArgs.Empty);
        }

        private void Context_LocationChanged(IWindowContext context)
        {
            Left = context.X;
            Top = context.Y;
            LocationChanged?.Invoke(this, EventArgs.Empty);
        }

        private void Context_Closing(object? sender, CancelEventArgs e)
        {
            Closing?.Invoke(this, e);
        }

        private void Context_Closed(IWindowContext context)
        {
            lock (_lock)
            {
                context.Closed -= Context_Closed;
                context.Closing -= Context_Closing;
                context.LocationChanged -= Context_LocationChanged;
                context.StateChanged -= Context_StateChanged;
                context.IsActivateChanged -= Context_IsActivatedChanged;
                context.DpiChanged -= Context_DpiChanged;
                context.SizeChanged -= Context_SizeChanged;
                context.Dispose();
                _context = null;
            }
        }

        public void Hide()
        {
            if (Application.Current == null || !Application.Current.IsRunning)
                throw new InvalidOperationException("Application is not running.");
            lock (_lock)
            {
                if (_context == null)
                    return;
                _context.Hide();
            }
        }

        public void Close()
        {
            if (Application.Current == null || !Application.Current.IsRunning)
                throw new InvalidOperationException("Application is not running.");
            lock (_lock)
            {
                if (_context == null)
                    return;
                _context.Close();
            }
        }

        public bool? ShowDialog()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Render

        private Size _visualSize;
        public override Size GetVisualSize()
        {
            return _visualSize;
        }

        private DpiScale _dpiScale = DpiScale.Default;
        public override DpiScale GetDpi()
        {
            return _dpiScale;
        }

        #endregion

        #region Dispatcher

        public override Dispatcher Dispatcher => _context?.Dispatcher ?? base.Dispatcher;

        #endregion
    }
}
