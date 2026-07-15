using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Input;
using Wodsoft.UI.Shapes;

namespace Wodsoft.UI.Controls.Primitives
{
    [TemplatePart(Name = "PART_Track", Type = typeof(Track))]
    public class ScrollBar : RangeBase
    {
        private Track? _track;
        private Vector2 _thumbOffset;
        private Point _latestRightButtonClickPoint = new Point(-1, -1);
        private bool _canScroll = true;

        #region Events

        public static readonly RoutedEvent ScrollEvent = EventManager.RegisterRoutedEvent("Scroll", RoutingStrategy.Bubble, typeof(ScrollEventHandler), typeof(ScrollBar));
        public event ScrollEventHandler Scroll { add { AddHandler(ScrollEvent, value); } remove { RemoveHandler(ScrollEvent, value); } }

        #endregion

        #region Properties

        public static readonly DependencyProperty OrientationProperty
            = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(ScrollBar),
                                          new FrameworkPropertyMetadata(Orientation.Vertical),
                                          new ValidateValueCallback(IsValidOrientation));
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty)!; }
            set { SetValue(OrientationProperty, value); }
        }


        public static readonly DependencyProperty ViewportSizeProperty
            = DependencyProperty.Register("ViewportSize", typeof(float), typeof(ScrollBar),
                                          new FrameworkPropertyMetadata(0.0f),
                                          new ValidateValueCallback(IsFloatFiniteNonNegative));
        internal static bool IsFloatFiniteNonNegative(object? o)
        {
            if (o is float d)
                return !(float.IsInfinity(d) || float.IsNaN(d) || d < 0.0);
            return false;
        }
        public float ViewportSize
        {
            get { return (float)GetValue(ViewportSizeProperty)!; }
            set { SetValue(ViewportSizeProperty, value); }
        }


        public Track? Track => _track;

        protected override bool IsEnabledCore => base.IsEnabledCore && _canScroll;

        public bool IsStandalone { get; set; }

        private IInputElement CommandTarget
        {
            get
            {
                IInputElement? target = TemplatedParent;
                if (target == null)
                    target = this;
                return target;
            }
        }

        #endregion

        #region Methods

        internal static bool IsValidOrientation(object? o)
        {
            if (o is Orientation value)
                return value == Orientation.Horizontal
                    || value == Orientation.Vertical;
            return false;
        }

        private void ChangeValue(float newValue, bool defer)
        {
            newValue = Math.Min(Math.Max(newValue, Minimum), Maximum);
            if (IsStandalone) { Value = newValue; }
            else
            {
                IInputElement target = CommandTarget;
                RoutedCommand? command = null;
                bool horizontal = (Orientation == Orientation.Horizontal);

                // Fire the deferred (drag) version of the command
                if (defer)
                {
                    command = horizontal ? DeferScrollToHorizontalOffsetCommand : DeferScrollToVerticalOffsetCommand;
                    if (command.CanExecute(newValue, target))
                    {
                        // The defer version of the command is enabled, fire this command and not the scroll version
                        command.Execute(newValue, target);
                    }
                    else
                    {
                        // The defer version of the command is not enabled, reset and try the scroll version
                        command = null;
                    }
                }

                if (command == null)
                {
                    // Either we're not dragging or the drag command is not enabled, try the scroll version
                    command = horizontal ? ScrollToHorizontalOffsetCommand : ScrollToVerticalOffsetCommand;
                    if (command.CanExecute(newValue, target))
                    {
                        command.Execute(newValue, target);
                    }
                }
            }
        }

        protected override void OnApplyTemplate()
        {
            _track = GetTemplateChild("PART_Track") as Track;
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            _thumbOffset = new Vector2();
            if (Track != null && Track.IsMouseOver && Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
            {
                // Move Thumb to the Mouse location
                Point pt = e.MouseDevice.GetPosition(Track);
                float newValue = Track.ValueFromPoint(pt);
                if (Shape.IsFloatFinite(newValue))
                {
                    ChangeValue(newValue, false /* defer */);
                }

                if (Track.Thumb != null && Track.Thumb.IsMouseOver)
                {
                    Point thumbPoint = e.MouseDevice.GetPosition(Track.Thumb);
                    _thumbOffset = thumbPoint - new Point(Track.Thumb.ActualWidth * 0.5f, Track.Thumb.ActualHeight * 0.5f);
                }
                else
                {
                    e.Handled = true;
                }
            }
        }

        protected override void OnPreviewMouseRightButtonUp(MouseButtonEventArgs e)
        {
            if (Track != null)
            {
                // Remember the mouse point (relative to Track's co-ordinate).
                _latestRightButtonClickPoint = e.MouseDevice.GetPosition(Track);
            }
            else
            {
                // Clear the mouse point
                _latestRightButtonClickPoint = new Point(-1, -1);
            }
        }


        #endregion

        #region Commands

        /// <summary>
        /// Scroll content by one line to the top.
        /// </summary>
        public static readonly RoutedCommand LineUpCommand = new RoutedCommand("LineUp", typeof(ScrollBar));
        /// <summary>
        /// Scroll content by one line to the bottom.
        /// </summary>
        public static readonly RoutedCommand LineDownCommand = new RoutedCommand("LineDown", typeof(ScrollBar));
        /// <summary>
        /// Scroll content by one line to the left.
        /// </summary>
        public static readonly RoutedCommand LineLeftCommand = new RoutedCommand("LineLeft", typeof(ScrollBar));
        /// <summary>
        /// Scroll content by one line to the right.
        /// </summary>
        public static readonly RoutedCommand LineRightCommand = new RoutedCommand("LineRight", typeof(ScrollBar));
        /// <summary>
        /// Scroll content by one page to the top.
        /// </summary>
        public static readonly RoutedCommand PageUpCommand = new RoutedCommand("PageUp", typeof(ScrollBar));
        /// <summary>
        /// Scroll content by one page to the bottom.
        /// </summary>
        public static readonly RoutedCommand PageDownCommand = new RoutedCommand("PageDown", typeof(ScrollBar));
        /// <summary>
        /// Scroll content by one page to the left.
        /// </summary>
        public static readonly RoutedCommand PageLeftCommand = new RoutedCommand("PageLeft", typeof(ScrollBar));
        /// <summary>
        /// Scroll content by one page to the right.
        /// </summary>
        public static readonly RoutedCommand PageRightCommand = new RoutedCommand("PageRight", typeof(ScrollBar));
        /// <summary>
        /// Horizontally scroll to the beginning of the content.
        /// </summary>
        public static readonly RoutedCommand ScrollToEndCommand = new RoutedCommand("ScrollToEnd", typeof(ScrollBar));
        /// <summary>
        /// Horizontally scroll to the end of the content.
        /// </summary>
        public static readonly RoutedCommand ScrollToHomeCommand = new RoutedCommand("ScrollToHome", typeof(ScrollBar));
        /// <summary>
        /// Horizontally scroll to the beginning of the content.
        /// </summary>
        public static readonly RoutedCommand ScrollToRightEndCommand = new RoutedCommand("ScrollToRightEnd", typeof(ScrollBar));
        /// <summary>
        /// Horizontally scroll to the end of the content.
        /// </summary>
        public static readonly RoutedCommand ScrollToLeftEndCommand = new RoutedCommand("ScrollToLeftEnd", typeof(ScrollBar));
        /// <summary>
        /// Vertically scroll to the beginning of the content.
        /// </summary>
        public static readonly RoutedCommand ScrollToTopCommand = new RoutedCommand("ScrollToTop", typeof(ScrollBar));
        /// <summary>
        /// Vertically scroll to the end of the content.
        /// </summary>
        public static readonly RoutedCommand ScrollToBottomCommand = new RoutedCommand("ScrollToBottom", typeof(ScrollBar));
        /// <summary>
        /// Scrolls horizontally to the double value provided in <see cref="System.Windows.Input.ExecutedRoutedEventArgs.Parameter" />.
        /// </summary>
        public static readonly RoutedCommand ScrollToHorizontalOffsetCommand = new RoutedCommand("ScrollToHorizontalOffset", typeof(ScrollBar));
        /// <summary>
        /// Scrolls vertically to the double value provided in <see cref="System.Windows.Input.ExecutedRoutedEventArgs.Parameter" />.
        /// </summary>
        public static readonly RoutedCommand ScrollToVerticalOffsetCommand = new RoutedCommand("ScrollToVerticalOffset", typeof(ScrollBar));
        /// <summary>
        /// Scrolls horizontally by dragging to the double value provided in <see cref="System.Windows.Input.ExecutedRoutedEventArgs.Parameter" />.
        /// </summary>
        public static readonly RoutedCommand DeferScrollToHorizontalOffsetCommand = new RoutedCommand("DeferScrollToToHorizontalOffset", typeof(ScrollBar));
        /// <summary>
        /// Scrolls vertically by dragging to the double value provided in <see cref="System.Windows.Input.ExecutedRoutedEventArgs.Parameter" />.
        /// </summary>
        public static readonly RoutedCommand DeferScrollToVerticalOffsetCommand = new RoutedCommand("DeferScrollToVerticalOffset", typeof(ScrollBar));

        /// <summary>
        /// Scroll to the point where user invoke ScrollBar ContextMenu.  This command is always handled by ScrollBar.
        /// </summary>
        public static readonly RoutedCommand ScrollHereCommand = new RoutedCommand("ScrollHere", typeof(ScrollBar));

        #endregion
    }
}
