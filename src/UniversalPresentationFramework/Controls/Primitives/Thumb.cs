using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Input;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Controls.Primitives
{
    public class Thumb : Control
    {
        /// <summary>
        /// The point where the mouse was clicked down (Thumb's co-ordinate).
        /// </summary>
        private Point _originThumbPoint;

        /// <summary>
        /// The position of the mouse (screen co-ordinate) where the mouse was clicked down.
        /// </summary>
        private Point _originScreenCoordPosition;

        /// <summary>
        /// The position of the mouse (screen co-ordinate) when the previous DragDelta event was fired
        /// </summary>
        private Point _previousScreenCoordPosition;

        static Thumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Thumb), new FrameworkPropertyMetadata(typeof(Thumb)));
            IsEnabledProperty.OverrideMetadata(typeof(Thumb), new UIPropertyMetadata(new PropertyChangedCallback(OnVisualStatePropertyChanged)));
            IsMouseOverPropertyKey.OverrideMetadata(typeof(Thumb), new UIPropertyMetadata(new PropertyChangedCallback(OnVisualStatePropertyChanged)));
        }

        #region Events

        /// <summary>
        ///     Event fires when user press mouse's left button on the thumb.
        /// </summary>
        public static readonly RoutedEvent DragStartedEvent = EventManager.RegisterRoutedEvent("DragStarted", RoutingStrategy.Bubble, typeof(DragStartedEventHandler), typeof(Thumb));

        /// <summary>
        ///     Event fires when the thumb is in a mouse capture state and the user moves the mouse around.
        /// </summary>
        public static readonly RoutedEvent DragDeltaEvent = EventManager.RegisterRoutedEvent("DragDelta", RoutingStrategy.Bubble, typeof(DragDeltaEventHandler), typeof(Thumb));

        /// <summary>
        ///     Event fires when user released mouse's left button or when CancelDrag method is called.
        /// </summary>
        public static readonly RoutedEvent DragCompletedEvent = EventManager.RegisterRoutedEvent("DragCompleted", RoutingStrategy.Bubble, typeof(DragCompletedEventHandler), typeof(Thumb));


        #endregion

        #region Properties

        /// <summary>
        /// Add / Remove DragStartedEvent handler
        /// </summary>
        public event DragStartedEventHandler DragStarted { add { AddHandler(DragStartedEvent, value); } remove { RemoveHandler(DragStartedEvent, value); } }

        /// <summary>
        /// Add / Remove DragDeltaEvent handler
        /// </summary>
        public event DragDeltaEventHandler DragDelta { add { AddHandler(DragDeltaEvent, value); } remove { RemoveHandler(DragDeltaEvent, value); } }

        /// <summary>
        /// Add / Remove DragCompletedEvent handler
        /// </summary>
        public event DragCompletedEventHandler DragCompleted { add { AddHandler(DragCompletedEvent, value); } remove { RemoveHandler(DragCompletedEvent, value); } }

        private static readonly DependencyPropertyKey _IsDraggingPropertyKey =
                DependencyProperty.RegisterReadOnly(
                        "IsDragging",
                        typeof(bool),
                        typeof(Thumb),
                        new FrameworkPropertyMetadata(
                                false,
                                new PropertyChangedCallback(OnIsDraggingPropertyChanged)));
        public static readonly DependencyProperty IsDraggingProperty = _IsDraggingPropertyKey.DependencyProperty;
        public bool IsDragging
        {
            get { return (bool)GetValue(IsDraggingProperty)!; }
            protected set { SetValue(_IsDraggingPropertyKey, value); }
        }
        private static void OnIsDraggingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var thumb = (Thumb)d;
            thumb.OnDraggingChanged(e);
            thumb.UpdateVisualState();
        }

        protected override int EffectiveValuesInitialSize => 19;

        #endregion

        #region Methods

        /// <summary>
        ///     This method cancels the dragging operation.
        /// </summary>
        public void CancelDrag()
        {
            if (IsDragging)
            {
                if (IsMouseCaptured)
                {
                    ReleaseMouseCapture();
                }
                ClearValue(_IsDraggingPropertyKey);
                RaiseEvent(new DragCompletedEventArgs(_previousScreenCoordPosition.X - _originScreenCoordPosition.X, _previousScreenCoordPosition.Y - _originScreenCoordPosition.Y, true));
            }
        }

        /// <summary>
        ///     This method is invoked when the IsDragging property changes.
        /// </summary>
        /// <param name="e">DependencyPropertyChangedEventArgs for IsDragging property.</param>
        protected virtual void OnDraggingChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        protected override void ChangeVisualState(bool useTransitions)
        {
            // See ButtonBase.ChangeVisualState.
            // This method should be exactly like it, except we use IsDragging instead of IsPressed for the pressed state
            if (!IsEnabled)
            {
                VisualStateManager.GoToState(this, VisualStates.StateDisabled, useTransitions);
            }
            else if (IsDragging)
            {
                VisualStateManager.GoToState(this, VisualStates.StatePressed, useTransitions);
            }
            else if (IsMouseOver)
            {
                VisualStateManager.GoToState(this, VisualStates.StateMouseOver, useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, VisualStates.StateNormal, useTransitions);
            }

            if (IsKeyboardFocused)
            {
                VisualStateManager.GoToState(this, VisualStates.StateFocused, useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, VisualStates.StateUnfocused, useTransitions);
            }

            base.ChangeVisualState(useTransitions);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (!IsDragging)
            {
                e.Handled = true;
                Focus();
                CaptureMouse();
                SetValue(_IsDraggingPropertyKey, true);
                _originThumbPoint = e.GetPosition(this);
                _previousScreenCoordPosition = _originScreenCoordPosition = ClientToScreen(_originThumbPoint);
                bool exceptionThrown = true;
                try
                {
                    RaiseEvent(new DragStartedEventArgs(_originThumbPoint.X, _originThumbPoint.Y));
                    exceptionThrown = false;
                }
                finally
                {
                    if (exceptionThrown)
                    {
                        CancelDrag();
                    }
                }
            }
            else
            {
                // This is weird, Thumb shouldn't get MouseLeftButtonDown event while dragging.
                // This may be the case that something ate MouseLeftButtonUp event, so Thumb never had a chance to
                // reset IsDragging property
                Debug.Assert(false, "Got MouseLeftButtonDown event while dragging!");
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (IsMouseCaptured && IsDragging)
            {
                e.Handled = true;
                ClearValue(_IsDraggingPropertyKey);
                ReleaseMouseCapture();
                Point pt = ClientToScreen(e.MouseDevice.GetPosition(this));
                RaiseEvent(new DragCompletedEventArgs(pt.X - _originScreenCoordPosition.X, pt.Y - _originScreenCoordPosition.Y, false));
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (IsDragging)
            {
                if (e.MouseDevice.LeftButton == MouseButtonState.Pressed)
                {
                    Point thumbCoordPosition = e.GetPosition(this);
                    // Get client point then convert to screen point
                    Point screenCoordPosition = ClientToScreen(thumbCoordPosition);

                    // We will fire DragDelta event only when the mouse is really moved
                    if (screenCoordPosition != _previousScreenCoordPosition)
                    {
                        _previousScreenCoordPosition = screenCoordPosition;
                        e.Handled = true;
                        RaiseEvent(new DragDeltaEventArgs(thumbCoordPosition.X - _originThumbPoint.X,
                                                          thumbCoordPosition.Y - _originThumbPoint.Y));
                    }
                }
                else
                {
                    if (e.MouseDevice.Captured == this)
                        ReleaseMouseCapture();
                    ClearValue(_IsDraggingPropertyKey);
                    _originThumbPoint.X = 0;
                    _originThumbPoint.Y = 0;
                }
            }
        }

        protected override void OnLostMouseCapture(MouseEventArgs e)
        {
            if (Mouse.Captured != this)
            {
                CancelDrag();
            }
        }

        private Point ClientToScreen(in Point point)
        {
            Visual visual = this;
            Vector2 visualOffset = VisualOffset;
            while (visual.VisualParent != null)
            {
                visual = visual.VisualParent;
                visualOffset += visual.VisualOffset;
            }
            if (visual is Window window)
            {
                visualOffset.X += window.Left;
                visualOffset.Y += window.Top;
            }
            return new Point(visualOffset.X + point.X, visualOffset.Y + point.Y);
        }

        #endregion
    }
}
