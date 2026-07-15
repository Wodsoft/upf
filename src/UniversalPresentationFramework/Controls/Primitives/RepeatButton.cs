using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Wodsoft.UI.Input;
using Wodsoft.UI.Threading;

namespace Wodsoft.UI.Controls.Primitives
{
    public class RepeatButton : ButtonBase
    {
        private DispatcherTimer? _timer;

        #region Constructors

        static RepeatButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RepeatButton), new FrameworkPropertyMetadata(typeof(RepeatButton)));
            ClickModeProperty.OverrideMetadata(typeof(RepeatButton), new FrameworkPropertyMetadata(ClickMode.Press));
        }

        #endregion

        #region Properties

        public static readonly DependencyProperty DelayProperty
            = DependencyProperty.Register("Delay", typeof(int), typeof(RepeatButton),
                                          new FrameworkPropertyMetadata(GetKeyboardDelay()),
                                          new ValidateValueCallback(IsDelayValid));
        private static bool IsDelayValid(object? value) { return (int)value! >= 0; }
        internal static int GetKeyboardDelay()
        {
            int delay = SystemParameters.KeyboardDelay;
            // SPI_GETKEYBOARDDELAY 0,1,2,3 correspond to 250,500,750,1000ms
            if (delay < 0 || delay > 3)
                delay = 0;
            return (delay + 1) * 250;
        }
        public int Delay
        {
            get { return (int)GetValue(DelayProperty)!; }
            set { SetValue(DelayProperty, value); }
        }

        public static readonly DependencyProperty IntervalProperty
            = DependencyProperty.Register("Interval", typeof(int), typeof(RepeatButton),
                                          new FrameworkPropertyMetadata(GetKeyboardSpeed()),
                                          new ValidateValueCallback(IsIntervalValid));
        internal static int GetKeyboardSpeed()
        {
            int speed = SystemParameters.KeyboardSpeed;
            // SPI_GETKEYBOARDSPEED 0,...,31 correspond to 1000/2.5=400,...,1000/30 ms
            if (speed < 0 || speed > 31)
                speed = 31;
            return (31 - speed) * (400 - 1000 / 30) / 31 + 1000 / 30;
        }
        private static bool IsIntervalValid(object? value) { return (int)value! > 0; }
        public int Interval
        {
            get { return (int)GetValue(IntervalProperty)!; }
            set { SetValue(IntervalProperty, value); }
        }

        #endregion

        #region Methods

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (IsPressed && (ClickMode != ClickMode.Hover))
            {
                StartTimer();
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            if (ClickMode != ClickMode.Hover)
            {
                StopTimer();
            }
        }

        protected override void OnLostMouseCapture(MouseEventArgs e)
        {
            base.OnLostMouseCapture(e);
            StopTimer();
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (HandleIsMouseOverChanged())
            {
                e.Handled = true;
            }
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (HandleIsMouseOverChanged())
            {
                e.Handled = true;
            }
        }

        private bool HandleIsMouseOverChanged()
        {
            if (ClickMode == ClickMode.Hover)
            {
                if (IsMouseOver)
                {
                    StartTimer();
                }
                else
                {
                    StopTimer();
                }

                return true;
            }

            return false;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if ((e.Key == Key.Space) && (ClickMode != ClickMode.Hover))
            {
                StartTimer();
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if ((e.Key == Key.Space) && (ClickMode != ClickMode.Hover))
            {
                StopTimer();
            }
            base.OnKeyUp(e);
        }
        protected override int EffectiveValuesInitialSize
        {
            get { return 28; }
        }

        private void StartTimer()
        {
            if (_timer == null)
            {
                _timer = new DispatcherTimer();
                _timer.Tick += new EventHandler(OnTimeout);
            }
            else if (_timer.IsEnabled)
                return;

            _timer.Interval = TimeSpan.FromMilliseconds(Delay);
            _timer.Start();
        }

        private void StopTimer()
        {
            if (_timer != null)
            {
                _timer.Stop();
            }
        }

        private void OnTimeout(object? sender, EventArgs e)
        {
            TimeSpan interval = TimeSpan.FromMilliseconds(Interval);
            if (_timer!.Interval != interval)
                _timer.Interval = interval;

            if (IsPressed)
            {
                OnClick();
            }
        }

        #endregion
    }
}
