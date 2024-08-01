using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Input;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Controls.Primitives
{
    public abstract class ButtonBase : ContentControl
    {
        private bool _isSpaceKeyDown;

        #region Constructors

        #endregion

        #region Methods

        protected virtual void OnClick()
        {
            RoutedEventArgs newEvent = new RoutedEventArgs(ClickEvent, this);
            RaiseEvent(newEvent);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (ClickMode != ClickMode.Hover)
            {
                e.Handled = true;

                // Always set focus on itself
                // In case ButtonBase is inside a nested focus scope we should restore the focus OnLostMouseCapture
                Focus();

                // It is possible that the mouse state could have changed during all of
                // the call-outs that have happened so far.
                if (e.ButtonState == MouseButtonState.Pressed)
                {
                    // Capture the mouse, and make sure we got it.
                    // WARNING: callout
                    CaptureMouse();
                    if (IsMouseCaptured)
                    {
                        // Though we have already checked this state, our call to CaptureMouse
                        // could also end up changing the state, so we check it again.
                        if (e.ButtonState == MouseButtonState.Pressed)
                        {
                            IsPressed = true;
                        }
                        else
                        {
                            // Release capture since we decided not to press the button.
                            ReleaseMouseCapture();
                        }
                    }
                }

                if (ClickMode == ClickMode.Press)
                {
                    bool exceptionThrown = true;
                    try
                    {
                        OnClick();
                        exceptionThrown = false;
                    }
                    finally
                    {
                        if (exceptionThrown)
                        {
                            // Cleanup the buttonbase state
                            IsPressed = false;
                            ReleaseMouseCapture();
                        }
                    }
                }
            }

            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            // Ignore when in hover-click mode.
            if (ClickMode != ClickMode.Hover)
            {
                e.Handled = true;
                bool shouldClick = !_isSpaceKeyDown && IsPressed && ClickMode == ClickMode.Release;

                if (IsMouseCaptured && !_isSpaceKeyDown)
                {
                    ReleaseMouseCapture();
                }

                if (shouldClick)
                {
                    OnClick();
                }
            }

            base.OnMouseLeftButtonUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if ((ClickMode != ClickMode.Hover) &&
                ((IsMouseCaptured && (Mouse.PrimaryDevice.LeftButton == MouseButtonState.Pressed) && !_isSpaceKeyDown)))
            {
                UpdateIsPressed();

                e.Handled = true;
            }
        }

        protected override void OnLostMouseCapture(MouseEventArgs e)
        {
            base.OnLostMouseCapture(e);

            if ((e.OriginalSource == this) && (ClickMode != ClickMode.Hover) && !_isSpaceKeyDown)
            {
                // If we are inside a nested focus scope - we should restore the focus to the main focus scope
                // This will cover the scenarios like ToolBar buttons
                if (IsKeyboardFocused)
                    Keyboard.Focus(null);

                // When we lose capture, the button should not look pressed anymore
                // -- unless the spacebar is still down, in which case we are still pressed.
                IsPressed = false;
            }
        }

        private void UpdateIsPressed()
        {
            Point pos = Mouse.PrimaryDevice.GetPosition(this);

            if ((pos.X >= 0) && (pos.X <= ActualWidth) && (pos.Y >= 0) && (pos.Y <= ActualHeight))
            {
                if (!IsPressed)
                {
                    IsPressed = true;
                }
            }
            else if (IsPressed)
            {
                IsPressed = false;
            }
        }

        protected internal override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            // *** Workaround ***
            // We need OnMouseRealyOver Property here
            //
            // There is a problem when Button is capturing the Mouse and resizing untill the mouse fall of the Button
            // During that time, Button and Mouse didn't really move. However, we need to update the IsPressed property
            // because mouse is no longer over the button.
            // We migth need a new property called *** IsMouseReallyOver *** property, so we can update IsPressed when
            // it's changed. (Can't use IsMouseOver or IsMouseDirectlyOver 'coz once Mouse is captured, they're alway 'true'.
            //

            // Update IsPressed property
            if (IsMouseCaptured && (Mouse.PrimaryDevice.LeftButton == MouseButtonState.Pressed) && !_isSpaceKeyDown)
            {
                // At this point, RenderSize is not updated. We must use finalSize instead.
                UpdateIsPressed();
            }
        }

        protected override void ChangeVisualState(bool useTransitions)
        {
            if (!IsEnabled)
            {
                VisualStateManager.GoToState(this, VisualStates.StateDisabled, useTransitions);
            }
            else if (IsPressed)
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

        #endregion

        #region Properties

        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ButtonBase));
        public event RoutedEventHandler Click { add { AddHandler(ClickEvent, value); } remove { RemoveHandler(ClickEvent, value); } }

        private static readonly DependencyPropertyKey _IsPressedPropertyKey =
                DependencyProperty.RegisterReadOnly(
                        "IsPressed",
                        typeof(bool),
                        typeof(ButtonBase),
                        new FrameworkPropertyMetadata(
                                false,
                                new PropertyChangedCallback(OnIsPressedChanged)));
        private static void OnIsPressedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ButtonBase ctrl = (ButtonBase)d;
            //ctrl.OnIsPressedChanged(e);
        }
        public static readonly DependencyProperty IsPressedProperty = _IsPressedPropertyKey.DependencyProperty;
        public bool IsPressed
        {
            get { return (bool)GetValue(IsPressedProperty)!; }
            protected set { SetValue(_IsPressedPropertyKey, value); }
        }

        public static readonly DependencyProperty ClickModeProperty =
                DependencyProperty.Register(
                        "ClickMode",
                        typeof(ClickMode),
                        typeof(ButtonBase),
                        new FrameworkPropertyMetadata(ClickMode.Release));
        public ClickMode ClickMode { get { return (ClickMode)GetValue(ClickModeProperty)!; } set { SetValue(ClickModeProperty, value); } }

        #endregion
    }
}
