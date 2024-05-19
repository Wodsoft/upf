﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Input;
using Wodsoft.UI.Media;
using Wodsoft.UI.Media.Animation;
using Wodsoft.UI.Threading;

namespace Wodsoft.UI
{
    public class UIElement : Visual, IAnimatable, IInputElement
    {
        #region Constructor

        static UIElement()
        {
            RegisterEvents(typeof(UIElement));
        }

        #endregion

        #region Layout

        private Size _previousAvailableSize;
        private bool _isMeasuring, _isMeasuringDuringArrange;
        public void Measure(Size availableSize)
        {
            if (float.IsNaN(availableSize.Width) || float.IsNaN(availableSize.Height))
                throw new InvalidOperationException("Width and height can not be NaN.");

            if (Visibility == Visibility.Collapsed)
                return;

            if (IsMeasureValid)
            {
                bool isCloseToPreviousMeasure = FloatUtil.AreClose(availableSize, _previousAvailableSize);
                if (isCloseToPreviousMeasure)
                    return;
            }

            _isMeasuring = true;
            var previousSize = _desiredSize;
            Size desiredSize;
            try
            {
                desiredSize = MeasureCore(availableSize);
            }
            finally
            {
                _isMeasuring = false;
            }
            if (float.IsPositiveInfinity(desiredSize.Width) || float.IsPositiveInfinity(desiredSize.Height))
                throw new InvalidOperationException("Measure size can not be positive infinity.");
            if (float.IsNaN(desiredSize.Width) || float.IsNaN(desiredSize.Height))
                throw new InvalidOperationException("Measure size can not be NaN.");
            _previousAvailableSize = availableSize;
            _desiredSize = desiredSize;
            IsMeasureValid = true;

            if (!_isMeasuringDuringArrange && !FloatUtil.AreClose(_desiredSize, previousSize))
            {
                var parent = VisualParent as UIElement;
                if (parent != null && !parent._isMeasuring)
                    parent.OnChildDesiredSizeChanged(this);
            }
        }

        private Rect _previousFinalRect;
        private bool _isArranging, _isRenderValid;
        public void Arrange(Rect finalRect)
        {
            if (float.IsPositiveInfinity(finalRect.Width)
                || float.IsPositiveInfinity(finalRect.Height)
                || float.IsNaN(finalRect.Width)
                || float.IsNaN(finalRect.Height))
                throw new InvalidOperationException("Width and height can not be positive infinity and NaN.");

            if (Visibility == Visibility.Collapsed)
                return;

            if (!IsMeasureValid)
            {
                _isMeasuringDuringArrange = true;
                try
                {
                    Measure(finalRect.Size);
                }
                finally
                {
                    _isMeasuringDuringArrange = false;
                }
            }

            if (!IsArrangeValid || !FloatUtil.AreClose(finalRect, _previousFinalRect))
            {
                _isArranging = true;
                var oldRenderSize = RenderSize;
                try
                {
                    ArrangeCore(finalRect);
                }
                finally
                {
                    _isArranging = false;
                }
                _previousFinalRect = finalRect;
                IsArrangeValid = true;

                if (IsArrangeValid && IsMeasureValid && (!FloatUtil.AreClose(oldRenderSize, _renderSize) || !_isRenderValid))
                {
                    if (FrameworkCoreProvider.RendererProvider != null)
                    {
                        var drawingContext = FrameworkCoreProvider.RendererProvider.CreateDrawingContext(this);
                        try
                        {
                            OnRender(drawingContext);
                        }
                        finally
                        {
                            _drawingContent = drawingContext.Close();
                            _isRenderValid = true;
                        }
                        if (Dispatcher is UIDispatcher uiDispatcher)
                            uiDispatcher.UpdateRender();
                    }
                }
            }
        }

        protected virtual Size MeasureCore(Size availableSize)
        {
            return new Size();
        }

        protected virtual void ArrangeCore(Rect finalRect)
        {
            RenderSize = finalRect.Size;
        }

        public void InvalidateMeasure()
        {
            if (!_isMeasuring)
            {
                IsMeasureValid = false;
            }
        }

        public void InvalidateArrange()
        {
            if (!_isArranging)
            {
                IsArrangeValid = false;
            }
        }

        public void InvalidateVisual()
        {
            InvalidateArrange();
            _isRenderValid = false;
            if (Dispatcher is UIDispatcher uiDispatcher)
                uiDispatcher.UpdateRender();
        }

        protected virtual void OnChildDesiredSizeChanged(UIElement child)
        {
            if (IsMeasureValid)
            {
                InvalidateMeasure();
            }
        }

        private Size _desiredSize;
        public Size DesiredSize
        {
            get
            {
                if (Visibility == Visibility.Collapsed)
                    return new Size(0, 0);
                else
                    return _desiredSize;
            }
        }

        private Size _renderSize;
        public Size RenderSize
        {
            get
            {
                if (this.Visibility == Visibility.Collapsed)
                    return new Size();
                else
                    return _renderSize;
            }
            set
            {
                _renderSize = value;
            }
        }

        public override Size GetVisualSize()
        {
            return _renderSize;
        }

        public bool IsMeasureValid { get; private set; }

        public bool IsArrangeValid { get; private set; }

        public static float RoundLayoutValue(float value, float dpiScale)
        {
            float newValue;

            // If DPI == 1, don't use DPI-aware rounding.
            if (!FloatUtil.AreClose(dpiScale, 1.0f))
            {
                newValue = MathF.Round(value * dpiScale) / dpiScale;
                // If rounding produces a value unacceptable to layout (NaN, Infinity or MaxValue), use the original value.
                if (float.IsNaN(newValue) ||
                    float.IsInfinity(newValue) ||
                    FloatUtil.AreClose(newValue, float.MaxValue))
                {
                    newValue = value;
                }
            }
            else
            {
                newValue = MathF.Round(value);
            }

            return newValue;
        }

        public void UpdateLayout()
        {
            if (Dispatcher is UIDispatcher uiDispatcher)
                uiDispatcher.UpdateLayout();
            else
            {
                InvalidateLayout(this);
            }
        }

        private void InvalidateLayout(Visual visual)
        {
            if (visual is UIElement element)
            {
                element.InvalidateMeasure();
                element.InvalidateArrange();
            }
            var childrenCount = visual.VisualChildrenCount;
            for (int i = 0; i < childrenCount; i++)
            {
                var child = visual.GetVisualChild(i);
                if (child != null)
                    InvalidateLayout(child);
            }
        }

        #endregion

        #region Render

        private IDrawingContent? _drawingContent;
        protected virtual void OnRender(DrawingContext drawingContext)
        {

        }

        public sealed override void RenderContext(RenderContext renderContext)
        {
            if (_drawingContent != null)
                renderContext.Render(_drawingContent);
        }

        public override bool HasRenderContent => _drawingContent != null;

        #endregion

        #region Visual

        public static readonly DependencyProperty VisibilityProperty =
        DependencyProperty.Register(
                "Visibility",
                typeof(Visibility),
                typeof(UIElement),
                new PropertyMetadata(
                        Visibility.Visible,
                        new PropertyChangedCallback(OnVisibilityChanged)),
                new ValidateValueCallback(ValidateVisibility));

        private static void OnVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIElement ue = (UIElement)d;
            Visibility newVisibility = (Visibility)e.NewValue!;
            ue._visibility = newVisibility;
        }

        private static bool ValidateVisibility(object? o)
        {
            if (o is Visibility value)
                return (value == Visibility.Visible) || (value == Visibility.Hidden) || (value == Visibility.Collapsed);
            return false;
        }

        private Visibility _visibility;
        public Visibility Visibility
        {
            get { return _visibility; }
            set { SetValue(VisibilityProperty, value); }
        }

        #endregion

        #region Event

        internal readonly Dictionary<int, List<RoutedEventHandlerInfo>> EventHandlers = new Dictionary<int, List<RoutedEventHandlerInfo>>();

        public void AddHandler(RoutedEvent routedEvent, Delegate handler)
        {
            AddHandler(routedEvent, handler, false);
        }

        public void AddHandler(RoutedEvent routedEvent, Delegate handler, bool handledEventsToo)
        {
            if (routedEvent == null)
                throw new ArgumentNullException(nameof(routedEvent));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));
            if (!routedEvent.IsLegalHandler(handler))
                throw new ArgumentException("Event handler type invalid.");

            if (!EventHandlers.TryGetValue(routedEvent.GlobalIndex, out var list))
            {
                list = new List<RoutedEventHandlerInfo>();
                EventHandlers.Add(routedEvent.GlobalIndex, list);
            }
            list.Add(new RoutedEventHandlerInfo(handler, handledEventsToo));
        }

        public void RemoveHandler(RoutedEvent routedEvent, Delegate handler)
        {
            if (routedEvent == null)
                throw new ArgumentNullException(nameof(routedEvent));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));
            if (!routedEvent.IsLegalHandler(handler))
                throw new ArgumentException("Event handler type invalid.");

            if (EventHandlers.TryGetValue(routedEvent.GlobalIndex, out var list))
            {
                var i = list.FindIndex(t => t.Handler == handler);
                if (i != -1)
                    list.RemoveAt(i);
            }
        }

        public void RaiseEvent(RoutedEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));
            if (e.RoutedEvent == null)
                throw new InvalidOperationException("RoutedEventArgs must have RoutedEvent.");
            e.MarkAsUserInitiated();
            e.Source = this;
            RaiseEventCore(e);
            e.Source = e.OriginalSource;
            e.ClearUserInitiated();
        }

        internal void RaiseEventCore(RoutedEventArgs e)
        {
            List<LogicalObject> list = new List<LogicalObject>();
            List<List<RoutedEventHandlerInfo>?> handlers = new List<List<RoutedEventHandlerInfo>?>();
            LogicalObject? element = this;
            while (element != null)
            {
                list.Add(element);
                if (element is UIElement ue)
                {
                    handlers.Add(e.RoutedEvent!.GetClassHandlers(ue.GetType()));
                    if (ue.EventHandlers.TryGetValue(e.RoutedEvent.GlobalIndex, out var delegates))
                        handlers.Add(delegates);
                    else
                        handlers.Add(null);
                    element = ue.VisualParent;
                }
                else if (element is ContentElement ce)
                {
                    handlers.Add(e.RoutedEvent!.GetClassHandlers(ce.GetType()));
                    if (ce.EventHandlers.TryGetValue(e.RoutedEvent.GlobalIndex, out var delegates))
                        handlers.Add(delegates);
                    else
                        handlers.Add(null);
                    element = ce.LogicalParent;
                }
                else
                    element = null;
                if (e.RoutedEvent!.RoutingStrategy == RoutingStrategy.Direct)
                    break;
                if (list.Count > 4096)
                    throw new InvalidOperationException("Routed event have more than 4096 levels.");
            }

            if (e.RoutedEvent!.RoutingStrategy == RoutingStrategy.Bubble || e.RoutedEvent.RoutingStrategy == RoutingStrategy.Direct)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    e.OverrideSource(list[i]);
                    var classHandlers = handlers[i * 2];
                    if (classHandlers != null)
                    {
                        for (int ii = 0; ii < classHandlers.Count; ii++)
                        {
                            var handler = classHandlers[ii];
                            if (!e.Handled || handler.HandledEventsToo)
                            {
                                if (handler.Handler is RoutedEventHandler reh)
                                    reh(this, e);
                                else
                                    e.InvokeHandler(handler.Handler, this);
                            }
                        }
                    }
                    var routedHandlers = handlers[i * 2 + 1];
                    if (routedHandlers != null)
                    {
                        for (int ii = 0; ii < routedHandlers.Count; ii++)
                        {
                            var handler = routedHandlers[ii];
                            if (!e.Handled || handler.HandledEventsToo)
                            {
                                if (handler.Handler is RoutedEventHandler reh)
                                    reh(this, e);
                                else
                                    e.InvokeHandler(handler.Handler, this);
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    e.OverrideSource(list[i]);
                    var classHandlers = handlers[i * 2];
                    if (classHandlers != null)
                    {
                        for (int ii = 0; ii < classHandlers.Count; ii++)
                        {
                            var handler = classHandlers[ii];
                            if (!e.Handled || handler.HandledEventsToo)
                            {
                                if (handler.Handler is RoutedEventHandler reh)
                                    reh(this, e);
                                else
                                    e.InvokeHandler(handler.Handler, this);
                            }
                        }
                    }
                    var routedHandlers = handlers[i * 2 + 1];
                    if (routedHandlers != null)
                    {
                        for (int ii = 0; ii < routedHandlers.Count; ii++)
                        {
                            var handler = routedHandlers[ii];
                            if (!e.Handled || handler.HandledEventsToo)
                            {
                                if (handler.Handler is RoutedEventHandler reh)
                                    reh(this, e);
                                else
                                    e.InvokeHandler(handler.Handler, this);
                            }
                        }
                    }
                }
            }
        }

        internal static void AddHandler(DependencyObject d, RoutedEvent routedEvent, Delegate handler)
        {
            if (d == null)
                throw new ArgumentNullException("d");

            if (d is UIElement uiElement)
            {
                uiElement.AddHandler(routedEvent, handler);
            }
            else
            {
                if (d is ContentElement contentElement)
                {
                    contentElement.AddHandler(routedEvent, handler);
                }
                else
                {
                    //UIElement3D uiElement3D = d as UIElement3D;
                    //if (uiElement3D != null)
                    //{
                    //    uiElement3D.AddHandler(routedEvent, handler);
                    //}
                    //else
                    //{
                    throw new ArgumentException($"Invalid input element \"{d.GetType().FullName}\".");
                    //}
                }
            }
        }

        internal static void RemoveHandler(DependencyObject d, RoutedEvent routedEvent, Delegate handler)
        {
            if (d == null)
            {
                throw new ArgumentNullException("d");
            }

            if (d is UIElement uiElement)
            {
                uiElement.RemoveHandler(routedEvent, handler);
            }
            else
            {
                if (d is ContentElement contentElement)
                {
                    contentElement.AddHandler(routedEvent, handler);
                }
                else
                {
                    //UIElement3D uiElement3D = d as UIElement3D;
                    //if (uiElement3D != null)
                    //{
                    //    uiElement3D.RemoveHandler(routedEvent, handler);
                    //}
                    //else
                    //{
                    throw new ArgumentException($"Invalid input element \"{d.GetType().FullName}\".");
                    //}
                }
            }
        }

        #endregion

        #region Animatable

        public void BeginAnimation(DependencyProperty dp, AnimationTimeline animation)
        {
            var clock = animation.CreateClock();
            ApplyAnimationClock(dp, clock, HandoffBehavior.SnapshotAndReplace);
        }

        public void ApplyAnimationClock(DependencyProperty dp, AnimationClock clock, HandoffBehavior handoffBehavior)
        {
            AnimationStorage.ApplyClock(this, dp, clock, handoffBehavior);
        }

        protected override void EvaluateBaseValue(DependencyProperty dp, PropertyMetadata metadata, ref DependencyEffectiveValue effectiveValue)
        {
            FrameworkCoreModifiedValue? modifiedValue = effectiveValue.ModifiedValue as FrameworkCoreModifiedValue;
            if (AnimationStorage.TryGetStorage(this, dp, out var storage))
            {
                object? value;
                if (modifiedValue == null)
                    value = effectiveValue.Value;
                else
                    value = modifiedValue.BaseValue;
                var hasValue = storage.TryGetValue(ref value);
                var isValidValue = dp.IsValidValue(value);
                if (hasValue && isValidValue)
                {
                    if (modifiedValue == null)
                    {
                        modifiedValue = CreateModifiedValue();
                        modifiedValue.BaseValue = effectiveValue.Value;
                        if (effectiveValue.Source != DependencyEffectiveSource.Local)
                        {
                            var newEffectiveValue = new DependencyEffectiveValue(DependencyEffectiveSource.Local);
                            if (effectiveValue.Expression != null)
                                modifiedValue.Expression = effectiveValue.Expression;
                            if (effectiveValue.HasValue)
                                newEffectiveValue.UpdateValue(effectiveValue.Value);
                            effectiveValue = newEffectiveValue;
                        }
                    }
                    modifiedValue = modifiedValue.Clone();
                    modifiedValue.SetAnimationValue(value);
                    effectiveValue.ModifyValue(modifiedValue);
                    return;
                }
            }

            //Animation completed, may need to reset effective value
            //If fill behavior is stop and there is no animation value, nothing to do
            if (modifiedValue == null)
                return;
            modifiedValue = modifiedValue.Clone();
            //Clean animation value
            modifiedValue.CleanAnimationValue();
            //Reset effective value if modified value is empty
            if (modifiedValue.IsEmpty)
            {
                if (modifiedValue.Expression == null)
                {
                    effectiveValue = new DependencyEffectiveValue(modifiedValue.BaseValue, DependencyEffectiveSource.Local);
                }
                else
                {
                    //Rebuild expression effective value
                    effectiveValue = new DependencyEffectiveValue(modifiedValue.Expression);
                }
            }
            else
                effectiveValue.ModifyValue(modifiedValue);
        }

        protected virtual FrameworkCoreModifiedValue CreateModifiedValue()
        {
            return new FrameworkCoreModifiedValue();
        }

        #endregion

        #region HitTest

        protected override bool HitTestCore(in Point point)
        {
            if (_drawingContent == null)
                return false;
            return _drawingContent.HitTest(point - VisualOffset);
        }

        #endregion

        #region InputElement

        public static readonly RoutedEvent PreviewMouseLeftButtonDownEvent = EventManager.RegisterRoutedEvent("PreviewMouseLeftButtonDown", RoutingStrategy.Direct, typeof(MouseButtonEventHandler), typeof(UIElement));
        public static readonly RoutedEvent MouseLeftButtonDownEvent = EventManager.RegisterRoutedEvent("MouseLeftButtonDown", RoutingStrategy.Direct, typeof(MouseButtonEventHandler), typeof(UIElement));
        public static readonly RoutedEvent PreviewMouseLeftButtonUpEvent = EventManager.RegisterRoutedEvent("PreviewMouseLeftButtonUp", RoutingStrategy.Direct, typeof(MouseButtonEventHandler), typeof(UIElement));
        public static readonly RoutedEvent MouseLeftButtonUpEvent = EventManager.RegisterRoutedEvent("MouseLeftButtonUp", RoutingStrategy.Direct, typeof(MouseButtonEventHandler), typeof(UIElement));
        public static readonly RoutedEvent PreviewMouseRightButtonDownEvent = EventManager.RegisterRoutedEvent("PreviewMouseRightButtonDown", RoutingStrategy.Direct, typeof(MouseButtonEventHandler), typeof(UIElement));
        public static readonly RoutedEvent MouseRightButtonDownEvent = EventManager.RegisterRoutedEvent("MouseRightButtonDown", RoutingStrategy.Direct, typeof(MouseButtonEventHandler), typeof(UIElement));
        public static readonly RoutedEvent PreviewMouseRightButtonUpEvent = EventManager.RegisterRoutedEvent("PreviewMouseRightButtonUp", RoutingStrategy.Direct, typeof(MouseButtonEventHandler), typeof(UIElement));
        public static readonly RoutedEvent MouseRightButtonUpEvent = EventManager.RegisterRoutedEvent("MouseRightButtonUp", RoutingStrategy.Direct, typeof(MouseButtonEventHandler), typeof(UIElement));

        public static readonly RoutedEvent PreviewMouseMoveEvent = Mouse.PreviewMouseMoveEvent.AddOwner(typeof(UIElement));
        public static readonly RoutedEvent MouseMoveEvent = Mouse.MouseMoveEvent.AddOwner(typeof(UIElement));
        public static readonly RoutedEvent PreviewMouseWheelEvent = Mouse.PreviewMouseWheelEvent.AddOwner(typeof(UIElement));
        public static readonly RoutedEvent MouseWheelEvent = Mouse.MouseWheelEvent.AddOwner(typeof(UIElement));
        public static readonly RoutedEvent MouseEnterEvent = Mouse.MouseEnterEvent.AddOwner(typeof(UIElement));
        public static readonly RoutedEvent MouseLeaveEvent = Mouse.MouseLeaveEvent.AddOwner(typeof(UIElement));
        public static readonly RoutedEvent GotMouseCaptureEvent = Mouse.GotMouseCaptureEvent.AddOwner(typeof(UIElement));
        public static readonly RoutedEvent LostMouseCaptureEvent = Mouse.LostMouseCaptureEvent.AddOwner(typeof(UIElement));

        public event MouseButtonEventHandler PreviewMouseLeftButtonDown { add { AddHandler(PreviewMouseLeftButtonDownEvent, value); } remove { RemoveHandler(PreviewMouseLeftButtonDownEvent, value); } }
        public event MouseButtonEventHandler MouseLeftButtonDown { add { AddHandler(MouseLeftButtonDownEvent, value); } remove { RemoveHandler(MouseLeftButtonDownEvent, value); } }
        public event MouseButtonEventHandler PreviewMouseLeftButtonUp { add { AddHandler(PreviewMouseLeftButtonUpEvent, value); } remove { RemoveHandler(PreviewMouseLeftButtonUpEvent, value); } }
        public event MouseButtonEventHandler MouseLeftButtonUp { add { AddHandler(MouseLeftButtonUpEvent, value); } remove { RemoveHandler(MouseLeftButtonUpEvent, value); } }
        public event MouseButtonEventHandler PreviewMouseRightButtonDown { add { AddHandler(PreviewMouseRightButtonDownEvent, value); } remove { RemoveHandler(PreviewMouseRightButtonDownEvent, value); } }
        public event MouseButtonEventHandler MouseRightButtonDown { add { AddHandler(MouseRightButtonDownEvent, value); } remove { RemoveHandler(MouseRightButtonDownEvent, value); } }
        public event MouseButtonEventHandler PreviewMouseRightButtonUp { add { AddHandler(PreviewMouseRightButtonUpEvent, value); } remove { RemoveHandler(PreviewMouseRightButtonUpEvent, value); } }
        public event MouseButtonEventHandler MouseRightButtonUp { add { AddHandler(MouseRightButtonUpEvent, value); } remove { RemoveHandler(MouseRightButtonUpEvent, value); } }
        public event MouseEventHandler PreviewMouseMove { add { AddHandler(PreviewMouseMoveEvent, value); } remove { RemoveHandler(PreviewMouseMoveEvent, value); } }
        public event MouseEventHandler MouseMove { add { AddHandler(MouseMoveEvent, value); } remove { RemoveHandler(MouseMoveEvent, value); } }
        public event MouseWheelEventHandler PreviewMouseWheel { add { AddHandler(PreviewMouseWheelEvent, value); } remove { RemoveHandler(PreviewMouseWheelEvent, value); } }
        public event MouseWheelEventHandler MouseWheel { add { AddHandler(MouseWheelEvent, value); } remove { RemoveHandler(MouseWheelEvent, value); } }
        public event MouseEventHandler MouseEnter { add { AddHandler(MouseEnterEvent, value); } remove { RemoveHandler(MouseEnterEvent, value); } }
        public event MouseEventHandler MouseLeave { add { AddHandler(MouseLeaveEvent, value); } remove { RemoveHandler(MouseLeaveEvent, value); } }
        public event MouseEventHandler GotMouseCapture { add { AddHandler(GotMouseCaptureEvent, value); } remove { RemoveHandler(GotMouseCaptureEvent, value); } }
        public event MouseEventHandler LostMouseCapture { add { AddHandler(LostMouseCaptureEvent, value); } remove { RemoveHandler(LostMouseCaptureEvent, value); } }


        internal static readonly DependencyPropertyKey IsMouseOverPropertyKey =
                    DependencyProperty.RegisterReadOnly(
                                "IsMouseOver",
                                typeof(bool),
                                typeof(UIElement),
                                new PropertyMetadata(false));
        public static readonly DependencyProperty IsMouseOverProperty = IsMouseOverPropertyKey.DependencyProperty;

        public bool IsMouseOver { get; internal set; }

        public bool IsMouseDirectlyOver => IsMouseOver && Dispatcher is UIDispatcher uiDispatcher && uiDispatcher.MouseDevice.Target == this;


        internal static readonly DependencyPropertyKey IsMouseCapturedPropertyKey =
                    DependencyProperty.RegisterReadOnly(
                                "IsMouseCaptured",
                                typeof(bool),
                                typeof(UIElement),
                                new PropertyMetadata(false));
        public static readonly DependencyProperty IsMouseCapturedProperty = IsMouseCapturedPropertyKey.DependencyProperty;
        public bool IsMouseCaptured => (bool)GetValue(IsMouseCapturedProperty)!;

        public bool CaptureMouse()
        {
            if (Dispatcher is UIDispatcher uiDispatcher)
                return uiDispatcher.MouseDevice.Capture(this);
            return false;
        }

        public void ReleaseMouseCapture()
        {
            if (Dispatcher is UIDispatcher uiDispatcher && uiDispatcher.MouseDevice.Captured == this)
                uiDispatcher.MouseDevice.Capture(null);
        }

        internal static void RegisterEvents(Type type)
        {
            EventManager.RegisterClassHandler(type, Mouse.PreviewMouseDownEvent, new MouseButtonEventHandler(OnPreviewMouseDownThunk), true);
            EventManager.RegisterClassHandler(type, Mouse.MouseDownEvent, new MouseButtonEventHandler(OnMouseDownThunk), true);
            EventManager.RegisterClassHandler(type, Mouse.PreviewMouseUpEvent, new MouseButtonEventHandler(OnPreviewMouseUpThunk), true);
            EventManager.RegisterClassHandler(type, Mouse.MouseUpEvent, new MouseButtonEventHandler(OnMouseUpThunk), true);
            EventManager.RegisterClassHandler(type, PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(OnPreviewMouseLeftButtonDownThunk), false);
            EventManager.RegisterClassHandler(type, MouseLeftButtonDownEvent, new MouseButtonEventHandler(OnMouseLeftButtonDownThunk), false);
            EventManager.RegisterClassHandler(type, PreviewMouseLeftButtonUpEvent, new MouseButtonEventHandler(OnPreviewMouseLeftButtonUpThunk), false);
            EventManager.RegisterClassHandler(type, MouseLeftButtonUpEvent, new MouseButtonEventHandler(OnMouseLeftButtonUpThunk), false);
            EventManager.RegisterClassHandler(type, PreviewMouseRightButtonDownEvent, new MouseButtonEventHandler(OnPreviewMouseRightButtonDownThunk), false);
            EventManager.RegisterClassHandler(type, MouseRightButtonDownEvent, new MouseButtonEventHandler(OnMouseRightButtonDownThunk), false);
            EventManager.RegisterClassHandler(type, PreviewMouseRightButtonUpEvent, new MouseButtonEventHandler(OnPreviewMouseRightButtonUpThunk), false);
            EventManager.RegisterClassHandler(type, MouseRightButtonUpEvent, new MouseButtonEventHandler(OnMouseRightButtonUpThunk), false);
            EventManager.RegisterClassHandler(type, Mouse.PreviewMouseMoveEvent, new MouseEventHandler(OnPreviewMouseMoveThunk), false);
            EventManager.RegisterClassHandler(type, Mouse.MouseMoveEvent, new MouseEventHandler(OnMouseMoveThunk), false);
            EventManager.RegisterClassHandler(type, Mouse.PreviewMouseWheelEvent, new MouseWheelEventHandler(OnPreviewMouseWheelThunk), false);
            EventManager.RegisterClassHandler(type, Mouse.MouseWheelEvent, new MouseWheelEventHandler(OnMouseWheelThunk), false);
            EventManager.RegisterClassHandler(type, Mouse.MouseEnterEvent, new MouseEventHandler(OnMouseEnterThunk), false);
            EventManager.RegisterClassHandler(type, Mouse.MouseLeaveEvent, new MouseEventHandler(OnMouseLeaveThunk), false);
            EventManager.RegisterClassHandler(type, Mouse.GotMouseCaptureEvent, new MouseEventHandler(OnGotMouseCaptureThunk), false);
            EventManager.RegisterClassHandler(type, Mouse.LostMouseCaptureEvent, new MouseEventHandler(OnLostMouseCaptureThunk), false);
            EventManager.RegisterClassHandler(type, Mouse.QueryCursorEvent, new QueryCursorEventHandler(OnQueryCursorThunk), false);
        }

        private static void OnPreviewMouseDownThunk(object sender, MouseButtonEventArgs e)
        {
            if (!e.Handled)
            {
                if (sender is UIElement ue)
                {
                    ue.OnPreviewMouseDown(e);
                }
                else if (sender is ContentElement ce)
                {
                    ce.OnPreviewMouseDown(e);
                }
            }

            // Always raise this "sub-event", but we pass along the handledness.
            UIElement.CrackMouseButtonEventAndReRaiseEvent((DependencyObject)sender, e);
        }

        private static void OnMouseDownThunk(object sender, MouseButtonEventArgs e)
        {
            //if (!e.Handled)
            //{
            //    CommandManager.TranslateInput((IInputElement)sender, e);
            //}

            if (!e.Handled)
            {
                if (sender is UIElement ue)
                {
                    ue.OnMouseDown(e);
                }
                else if (sender is ContentElement ce)
                {
                    ce.OnMouseDown(e);
                }
            }

            // Always raise this "sub-event", but we pass along the handledness.
            UIElement.CrackMouseButtonEventAndReRaiseEvent((DependencyObject)sender, e);
        }

        private static void OnPreviewMouseUpThunk(object sender, MouseButtonEventArgs e)
        {
            if (!e.Handled)
            {
                if (sender is UIElement ue)
                {
                    ue.OnPreviewMouseUp(e);
                }
                else if (sender is ContentElement ce)
                {
                    ce.OnPreviewMouseUp(e);
                }
            }

            // Always raise this "sub-event", but we pass along the handledness.
            UIElement.CrackMouseButtonEventAndReRaiseEvent((DependencyObject)sender, e);
        }

        private static void OnMouseUpThunk(object sender, MouseButtonEventArgs e)
        {
            if (!e.Handled)
            {
                if (sender is UIElement ue)
                {
                    ue.OnMouseUp(e);
                }
                else if (sender is ContentElement ce)
                {
                    ce.OnMouseUp(e);
                }
            }

            // Always raise this "sub-event", but we pass along the handledness.
            UIElement.CrackMouseButtonEventAndReRaiseEvent((DependencyObject)sender, e);
        }

        private static void OnPreviewMouseLeftButtonDownThunk(object sender, MouseButtonEventArgs e)
        {
            if (sender is UIElement ue)
            {
                ue.OnPreviewMouseLeftButtonDown(e);
            }
            else if (sender is ContentElement ce)
            {
                ce.OnPreviewMouseLeftButtonDown(e);
            }
        }

        private static void OnMouseLeftButtonDownThunk(object sender, MouseButtonEventArgs e)
        {
            if (sender is UIElement ue)
            {
                ue.OnMouseLeftButtonDown(e);
            }
            else if (sender is ContentElement ce)
            {
                ce.OnMouseLeftButtonDown(e);
            }
        }

        private static void OnPreviewMouseLeftButtonUpThunk(object sender, MouseButtonEventArgs e)
        {
            if (sender is UIElement ue)
            {
                ue.OnPreviewMouseLeftButtonUp(e);
            }
            else if (sender is ContentElement ce)
            {
                ce.OnPreviewMouseLeftButtonUp(e);
            }
        }

        private static void OnMouseLeftButtonUpThunk(object sender, MouseButtonEventArgs e)
        {
            if (sender is UIElement ue)
            {
                ue.OnMouseLeftButtonUp(e);
            }
            else if (sender is ContentElement ce)
            {
                ce.OnMouseLeftButtonUp(e);
            }
        }

        private static void OnPreviewMouseRightButtonDownThunk(object sender, MouseButtonEventArgs e)
        {
            if (sender is UIElement ue)
            {
                ue.OnPreviewMouseRightButtonDown(e);
            }
            else if (sender is ContentElement ce)
            {
                ce.OnPreviewMouseRightButtonDown(e);
            }
        }

        private static void OnMouseRightButtonDownThunk(object sender, MouseButtonEventArgs e)
        {
            if (sender is UIElement ue)
            {
                ue.OnMouseRightButtonDown(e);
            }
            else if (sender is ContentElement ce)
            {
                ce.OnMouseRightButtonDown(e);
            }
        }

        private static void OnPreviewMouseRightButtonUpThunk(object sender, MouseButtonEventArgs e)
        {
            if (sender is UIElement ue)
            {
                ue.OnPreviewMouseRightButtonUp(e);
            }
            else if (sender is ContentElement ce)
            {
                ce.OnPreviewMouseRightButtonUp(e);
            }
        }

        private static void OnMouseRightButtonUpThunk(object sender, MouseButtonEventArgs e)
        {
            if (sender is UIElement ue)
            {
                ue.OnMouseRightButtonUp(e);
            }
            else if (sender is ContentElement ce)
            {
                ce.OnMouseRightButtonUp(e);
            }
        }

        private static void OnPreviewMouseMoveThunk(object sender, MouseEventArgs e)
        {
            if (sender is UIElement ue)
            {
                ue.OnPreviewMouseMove(e);
            }
            else if (sender is ContentElement ce)
            {
                ce.OnPreviewMouseMove(e);
            }
        }

        private static void OnMouseMoveThunk(object sender, MouseEventArgs e)
        {
            if (sender is UIElement ue)
            {
                ue.OnMouseMove(e);
            }
            else if (sender is ContentElement ce)
            {
                ce.OnMouseMove(e);
            }
        }

        private static void OnPreviewMouseWheelThunk(object sender, MouseWheelEventArgs e)
        {
            if (sender is UIElement ue)
            {
                ue.OnPreviewMouseWheel(e);
            }
            else if (sender is ContentElement ce)
            {
                ce.OnPreviewMouseWheel(e);
            }
        }

        private static void OnMouseWheelThunk(object sender, MouseWheelEventArgs e)
        {
            //CommandManager.TranslateInput((IInputElement)sender, e);

            if (!e.Handled)
            {


                if (sender is UIElement ue)
                {
                    ue.OnMouseWheel(e);
                }
                else if (sender is ContentElement ce)
                {
                    ce.OnMouseWheel(e);
                }
            }
        }

        private static void OnMouseLeaveThunk(object sender, MouseEventArgs e)
        {
            if (sender is UIElement ue)
            {
                ue.IsMouseOver = false;
                ue.SetValue(IsMouseOverPropertyKey, false);
            }
            else if (sender is ContentElement ce)
            {
                ce.IsMouseOver = false;
                ce.SetValue(IsMouseOverPropertyKey, false);
            }
        }

        private static void OnMouseEnterThunk(object sender, MouseEventArgs e)
        {
            if (sender is UIElement ue)
            {
                ue.IsMouseOver = true;
                ue.SetValue(IsMouseOverPropertyKey, true);
            }
            else if (sender is ContentElement ce)
            {
                ce.IsMouseOver = true;
                ce.SetValue(IsMouseOverPropertyKey, true);
            }
        }

        private static void OnGotMouseCaptureThunk(object sender, MouseEventArgs e)
        {
            if (sender is UIElement ue)
            {
                ue.OnGotMouseCapture(e);
            }
            else if (sender is ContentElement ce)
            {
                ce.OnGotMouseCapture(e);
            }
        }

        private static void OnLostMouseCaptureThunk(object sender, MouseEventArgs e)
        {
            if (sender is UIElement ue)
            {
                ue.OnLostMouseCapture(e);
            }
            else if (sender is ContentElement ce)
            {
                ce.OnLostMouseCapture(e);
            }
        }

        private static void OnQueryCursorThunk(object sender, QueryCursorEventArgs e)
        {
            if (sender is UIElement ue)
            {
                ue.OnQueryCursor(e);
            }
            else if (sender is ContentElement ce)
            {
                ce.OnQueryCursor(e);
            }
        }

        private static void CrackMouseButtonEventAndReRaiseEvent(DependencyObject sender, MouseButtonEventArgs e)
        {
            RoutedEvent? newEvent = CrackMouseButtonEvent(e);
            if (newEvent != null)
            {
                var previousSource = e.Source;
                var previousRoute = e.RoutedEvent;
                e.OverrideRoutedEvent(newEvent);
                e.OverrideSource(sender);
                if (sender is UIElement ue)
                    ue.RaiseEventCore(e);
                else if (sender is ContentElement ce)
                    ce.RaiseEventCore(e);
                e.OverrideSource(previousSource!);
                e.OverrideRoutedEvent(previousRoute!);
            }
        }

        private static RoutedEvent? CrackMouseButtonEvent(MouseButtonEventArgs e)
        {
            RoutedEvent? newEvent;

            switch (e.ChangedButton)
            {
                case MouseButton.Left:
                    if (e.RoutedEvent == Mouse.PreviewMouseDownEvent)
                        newEvent = UIElement.PreviewMouseLeftButtonDownEvent;
                    else if (e.RoutedEvent == Mouse.MouseDownEvent)
                        newEvent = UIElement.MouseLeftButtonDownEvent;
                    else if (e.RoutedEvent == Mouse.PreviewMouseUpEvent)
                        newEvent = UIElement.PreviewMouseLeftButtonUpEvent;
                    else
                        newEvent = UIElement.MouseLeftButtonUpEvent;
                    break;
                case MouseButton.Right:
                    if (e.RoutedEvent == Mouse.PreviewMouseDownEvent)
                        newEvent = UIElement.PreviewMouseRightButtonDownEvent;
                    else if (e.RoutedEvent == Mouse.MouseDownEvent)
                        newEvent = UIElement.MouseRightButtonDownEvent;
                    else if (e.RoutedEvent == Mouse.PreviewMouseUpEvent)
                        newEvent = UIElement.PreviewMouseRightButtonUpEvent;
                    else
                        newEvent = UIElement.MouseRightButtonUpEvent;
                    break;
                default:
                    // No wrappers exposed for the other buttons.
                    newEvent = null;
                    break;
            }
            return newEvent;
        }

        protected virtual void OnPreviewMouseDown(MouseButtonEventArgs e) { }

        protected virtual void OnMouseDown(MouseButtonEventArgs e) { }

        protected virtual void OnPreviewMouseUp(MouseButtonEventArgs e) { }

        protected virtual void OnMouseUp(MouseButtonEventArgs e) { }

        protected virtual void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e) { }

        protected virtual void OnMouseLeftButtonDown(MouseButtonEventArgs e) { }

        protected virtual void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e) { }

        protected virtual void OnMouseLeftButtonUp(MouseButtonEventArgs e) { }

        protected virtual void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e) { }

        protected virtual void OnMouseRightButtonDown(MouseButtonEventArgs e) { }

        protected virtual void OnPreviewMouseRightButtonUp(MouseButtonEventArgs e) { }

        protected virtual void OnMouseRightButtonUp(MouseButtonEventArgs e) { }

        protected virtual void OnPreviewMouseMove(MouseEventArgs e) { }

        protected virtual void OnMouseMove(MouseEventArgs e) { }

        protected virtual void OnPreviewMouseWheel(MouseWheelEventArgs e) { }

        protected virtual void OnMouseWheel(MouseWheelEventArgs e) { }

        protected virtual void OnMouseEnter(MouseEventArgs e) { }

        protected virtual void OnMouseLeave(MouseEventArgs e) { }

        protected virtual void OnGotMouseCapture(MouseEventArgs e) { }

        protected virtual void OnLostMouseCapture(MouseEventArgs e) { }

        protected virtual void OnQueryCursor(QueryCursorEventArgs e) { }

        #endregion
    }
}
