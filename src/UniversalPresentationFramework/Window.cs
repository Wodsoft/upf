using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Controls;

namespace Wodsoft.UI
{
    public class Window : ContentControl
    {
        #region Properties

        public static readonly DependencyProperty TitleProperty =
                DependencyProperty.Register("Title", typeof(String), typeof(Window),
                        new FrameworkPropertyMetadata(String.Empty,
                                new PropertyChangedCallback(OnTitleChanged)),
                        new ValidateValueCallback(ValidateText));
        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Window w = (Window)d;
            lock (w._lock)
            {
                if (w._context != null)
                    w._context.Title = (string)e.NewValue!;
            }
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
            if (w._context != null)
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
            if (w._context != null)
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
                _context = Application.Current!.WindowProvider!.CreateContext();
                _context.Title = Title;
                _context.X = (int)Left;
                _context.Y = (int)Top;
                _context.Width = (int)Width;
                _context.Height = (int)Height;
                _context.State = WindowState;
                _context.Style = WindowStyle;
                _context.StartupLocation = WindowStartupLocation;
                _context.AllowsTransparency = AllowsTransparency;
                _context.Show();
            }
        }

        public void Hide()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public bool? ShowDialog()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
