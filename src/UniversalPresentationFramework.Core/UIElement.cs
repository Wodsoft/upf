using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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
            EventManager.RegisterClassHandler(type, GotFocusEvent, new RoutedEventHandler(OnGotFocusThunk), false);
            EventManager.RegisterClassHandler(type, LostFocusEvent, new RoutedEventHandler(OnLostFocusThunk), false);
            EventManager.RegisterClassHandler(type, Keyboard.PreviewKeyDownEvent, new KeyEventHandler(OnPreviewKeyDownThunk), false);
            EventManager.RegisterClassHandler(type, Keyboard.KeyDownEvent, new KeyEventHandler(OnKeyDownThunk), false);
            EventManager.RegisterClassHandler(type, Keyboard.PreviewKeyUpEvent, new KeyEventHandler(OnPreviewKeyUpThunk), false);
            EventManager.RegisterClassHandler(type, Keyboard.KeyUpEvent, new KeyEventHandler(OnKeyUpThunk), false);
            EventManager.RegisterClassHandler(type, Keyboard.PreviewGotKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(OnPreviewGotKeyboardFocusThunk), false);
            EventManager.RegisterClassHandler(type, Keyboard.GotKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(OnGotKeyboardFocusThunk), false);
            EventManager.RegisterClassHandler(type, Keyboard.PreviewLostKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(OnPreviewLostKeyboardFocusThunk), false);
            EventManager.RegisterClassHandler(type, Keyboard.LostKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(OnLostKeyboardFocusThunk), false);
            EventManager.RegisterClassHandler(type, TextCompositionManager.PreviewTextInputEvent, new TextCompositionEventHandler(UIElement.OnPreviewTextInputThunk), false);
            EventManager.RegisterClassHandler(type, TextCompositionManager.TextInputEvent, new TextCompositionEventHandler(UIElement.OnTextInputThunk), false);
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

            if (Dispatcher is UIDispatcher uiDispatcher)
                uiDispatcher.RemoveMeasure(this);
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

        internal Size PreviousAvailableSize => _previousAvailableSize;

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

            var uiDispatcher = Dispatcher as UIDispatcher;
            if (!IsArrangeValid || !FloatUtil.AreClose(finalRect, _previousFinalRect))
            {
                if (uiDispatcher != null)
                    uiDispatcher.RemoveArrange(this);
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
                        if (uiDispatcher != null)
                            uiDispatcher.UpdateRender();
                    }
                }
            }
        }

        internal Rect PreviousArrangeRect => _previousFinalRect;

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
                if (Dispatcher is UIDispatcher uiDispatcher)
                    uiDispatcher.UpdateMeasure(this);
            }
        }

        public void InvalidateArrange()
        {
            if (!_isArranging)
            {
                IsArrangeValid = false;
                if (Dispatcher is UIDispatcher uiDispatcher)
                    uiDispatcher.UpdateArrange(this);
            }
        }

        public void InvalidateVisual()
        {
            InvalidateArrange();
            _isRenderValid = false;
            //if (Dispatcher is UIDispatcher uiDispatcher)
            //    uiDispatcher.UpdateRender();
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

        public override void RenderContext(RenderContext renderContext)
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


        public static readonly DependencyProperty OpacityProperty =
                    DependencyProperty.Register(
                                "Opacity",
                                typeof(float),
                                typeof(UIElement),
                                new UIPropertyMetadata(1.0f));
        public float Opacity { get { return (float)GetValue(OpacityProperty)!; } set { SetValue(OpacityProperty, value); } }

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
            List<(IInputElement Element, List<RoutedEventHandlerInfo>? Delegates)> handlers = new List<(IInputElement, List<RoutedEventHandlerInfo>?)>();
            LogicalObject? element = this;
            while (element != null)
            {
                list.Add(element);
                if (element is UIElement ue)
                {
                    handlers.Add((ue, e.RoutedEvent!.GetClassHandlers(ue.GetType())));
                    if (ue.EventHandlers.TryGetValue(e.RoutedEvent.GlobalIndex, out var delegates))
                        handlers.Add((ue, delegates));
                    else
                        handlers.Add((ue, null));
                    element = ue.VisualParent;
                }
                else if (element is ContentElement ce)
                {
                    handlers.Add((ce, e.RoutedEvent!.GetClassHandlers(ce.GetType())));
                    if (ce.EventHandlers.TryGetValue(e.RoutedEvent.GlobalIndex, out var delegates))
                        handlers.Add((ce, delegates));
                    else
                        handlers.Add((ce, null));
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
                    if (classHandlers.Delegates != null)
                    {
                        for (int ii = 0; ii < classHandlers.Delegates.Count; ii++)
                        {
                            var handler = classHandlers.Delegates[ii];
                            if (!e.Handled || handler.HandledEventsToo)
                            {
                                if (handler.Handler is RoutedEventHandler reh)
                                    reh(classHandlers.Element, e);
                                else
                                    e.InvokeHandler(handler.Handler, classHandlers.Element);
                            }
                        }
                    }
                    var routedHandlers = handlers[i * 2 + 1];
                    if (routedHandlers.Delegates != null)
                    {
                        for (int ii = 0; ii < routedHandlers.Delegates.Count; ii++)
                        {
                            var handler = routedHandlers.Delegates[ii];
                            if (!e.Handled || handler.HandledEventsToo)
                            {
                                if (handler.Handler is RoutedEventHandler reh)
                                    reh(routedHandlers.Element, e);
                                else
                                    e.InvokeHandler(handler.Handler, routedHandlers.Element);
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
                    if (classHandlers.Delegates != null)
                    {
                        for (int ii = 0; ii < classHandlers.Delegates.Count; ii++)
                        {
                            var handler = classHandlers.Delegates[ii];
                            if (!e.Handled || handler.HandledEventsToo)
                            {
                                if (handler.Handler is RoutedEventHandler reh)
                                    reh(classHandlers.Element, e);
                                else
                                    e.InvokeHandler(handler.Handler, classHandlers.Element);
                            }
                        }
                    }
                    var routedHandlers = handlers[i * 2 + 1];
                    if (routedHandlers.Delegates != null)
                    {
                        for (int ii = 0; ii < routedHandlers.Delegates.Count; ii++)
                        {
                            var handler = routedHandlers.Delegates[ii];
                            if (!e.Handled || handler.HandledEventsToo)
                            {
                                if (handler.Handler is RoutedEventHandler reh)
                                    reh(routedHandlers.Element, e);
                                else
                                    e.InvokeHandler(handler.Handler, routedHandlers.Element);
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
            return _drawingContent.HitTest(point);
        }

        #endregion

        #region Mouse

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
            CrackMouseButtonEventAndReRaiseEvent((DependencyObject)sender, e);
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
            CrackMouseButtonEventAndReRaiseEvent((DependencyObject)sender, e);
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
            CrackMouseButtonEventAndReRaiseEvent((DependencyObject)sender, e);
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
            CrackMouseButtonEventAndReRaiseEvent((DependencyObject)sender, e);
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
                        newEvent = PreviewMouseLeftButtonDownEvent;
                    else if (e.RoutedEvent == Mouse.MouseDownEvent)
                        newEvent = MouseLeftButtonDownEvent;
                    else if (e.RoutedEvent == Mouse.PreviewMouseUpEvent)
                        newEvent = PreviewMouseLeftButtonUpEvent;
                    else
                        newEvent = MouseLeftButtonUpEvent;
                    break;
                case MouseButton.Right:
                    if (e.RoutedEvent == Mouse.PreviewMouseDownEvent)
                        newEvent = PreviewMouseRightButtonDownEvent;
                    else if (e.RoutedEvent == Mouse.MouseDownEvent)
                        newEvent = MouseRightButtonDownEvent;
                    else if (e.RoutedEvent == Mouse.PreviewMouseUpEvent)
                        newEvent = PreviewMouseRightButtonUpEvent;
                    else
                        newEvent = MouseRightButtonUpEvent;
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

        #region Keyboard


        internal static readonly DependencyPropertyKey IsKeyboardFocusedPropertyKey =
                    DependencyProperty.RegisterReadOnly(
                                "IsKeyboardFocused",
                                typeof(bool),
                                typeof(UIElement),
                                new PropertyMetadata(
                                            false, // default value
                                            new PropertyChangedCallback(IsKeyboardFocused_Changed)));
        public static readonly DependencyProperty IsKeyboardFocusedProperty = IsKeyboardFocusedPropertyKey.DependencyProperty;
        private static void IsKeyboardFocused_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIElement uie = (UIElement)d;
            uie.OnIsKeyboardFocusedChanged(e);
            uie.IsKeyboardFocusedChanged?.Invoke(d, e);
        }
        public event DependencyPropertyChangedEventHandler? IsKeyboardFocusedChanged;
        public bool IsKeyboardFocused => Keyboard.FocusedElement == this;

        public static readonly RoutedEvent PreviewKeyDownEvent = Keyboard.PreviewKeyDownEvent.AddOwner(typeof(UIElement));
        public static readonly RoutedEvent KeyDownEvent = Keyboard.KeyDownEvent.AddOwner(typeof(UIElement));
        public static readonly RoutedEvent PreviewKeyUpEvent = Keyboard.PreviewKeyUpEvent.AddOwner(typeof(UIElement));
        public static readonly RoutedEvent KeyUpEvent = Keyboard.KeyUpEvent.AddOwner(typeof(UIElement));
        public static readonly RoutedEvent PreviewGotKeyboardFocusEvent = Keyboard.PreviewGotKeyboardFocusEvent.AddOwner(typeof(UIElement));
        public static readonly RoutedEvent GotKeyboardFocusEvent = Keyboard.GotKeyboardFocusEvent.AddOwner(typeof(UIElement));
        public static readonly RoutedEvent PreviewLostKeyboardFocusEvent = Keyboard.PreviewLostKeyboardFocusEvent.AddOwner(typeof(UIElement));
        public static readonly RoutedEvent LostKeyboardFocusEvent = Keyboard.LostKeyboardFocusEvent.AddOwner(typeof(UIElement));
        public static readonly RoutedEvent PreviewTextInputEvent = TextCompositionManager.PreviewTextInputEvent.AddOwner(typeof(UIElement));
        public static readonly RoutedEvent TextInputEvent = TextCompositionManager.TextInputEvent.AddOwner(typeof(UIElement));

        public event KeyEventHandler PreviewKeyDown { add { AddHandler(Keyboard.PreviewKeyDownEvent, value, false); } remove { RemoveHandler(Keyboard.PreviewKeyDownEvent, value); } }
        public event KeyEventHandler KeyDown { add { AddHandler(Keyboard.KeyDownEvent, value, false); } remove { RemoveHandler(Keyboard.KeyDownEvent, value); } }
        public event KeyEventHandler PreviewKeyUp { add { AddHandler(Keyboard.PreviewKeyUpEvent, value, false); } remove { RemoveHandler(Keyboard.PreviewKeyUpEvent, value); } }
        public event KeyEventHandler KeyUp { add { AddHandler(Keyboard.KeyUpEvent, value, false); } remove { RemoveHandler(Keyboard.KeyUpEvent, value); } }
        public event KeyboardFocusChangedEventHandler PreviewGotKeyboardFocus { add { AddHandler(Keyboard.PreviewGotKeyboardFocusEvent, value, false); } remove { RemoveHandler(Keyboard.PreviewGotKeyboardFocusEvent, value); } }
        public event KeyboardFocusChangedEventHandler GotKeyboardFocus { add { AddHandler(Keyboard.GotKeyboardFocusEvent, value, false); } remove { RemoveHandler(Keyboard.GotKeyboardFocusEvent, value); } }
        public event KeyboardFocusChangedEventHandler PreviewLostKeyboardFocus { add { AddHandler(Keyboard.PreviewLostKeyboardFocusEvent, value, false); } remove { RemoveHandler(Keyboard.PreviewLostKeyboardFocusEvent, value); } }
        public event KeyboardFocusChangedEventHandler LostKeyboardFocus { add { AddHandler(Keyboard.LostKeyboardFocusEvent, value, false); } remove { RemoveHandler(Keyboard.LostKeyboardFocusEvent, value); } }
        public event TextCompositionEventHandler PreviewTextInput { add { AddHandler(TextCompositionManager.PreviewTextInputEvent, value, false); } remove { RemoveHandler(TextCompositionManager.PreviewTextInputEvent, value); } }
        public event TextCompositionEventHandler TextInput { add { AddHandler(TextCompositionManager.TextInputEvent, value, false); } remove { RemoveHandler(TextCompositionManager.TextInputEvent, value); } }

        private static void OnPreviewKeyDownThunk(object sender, KeyEventArgs e)
        {
            if (sender is UIElement uie)
            {
                uie.OnPreviewKeyDown(e);
            }
            else if (sender is ContentElement ce)
            {
                ce.OnPreviewKeyDown(e);
            }
        }

        private static void OnKeyDownThunk(object sender, KeyEventArgs e)
        {
            if (sender is UIElement uie)
            {
                uie.OnKeyDown(e);
            }
            else if (sender is ContentElement ce)
            {
                ce.OnKeyDown(e);
            }
        }

        private static void OnPreviewKeyUpThunk(object sender, KeyEventArgs e)
        {
            if (sender is UIElement uie)
            {
                uie.OnPreviewKeyUp(e);
            }
            else if (sender is ContentElement ce)
            {
                ce.OnPreviewKeyUp(e);
            }
        }

        private static void OnKeyUpThunk(object sender, KeyEventArgs e)
        {
            if (sender is UIElement uie)
            {
                uie.OnKeyUp(e);
            }
            else if (sender is ContentElement ce)
            {
                ce.OnKeyUp(e);
            }
        }

        private static void OnPreviewGotKeyboardFocusThunk(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is UIElement uie)
            {
                uie.OnPreviewGotKeyboardFocus(e);
            }
            else if (sender is ContentElement ce)
            {
                ce.OnPreviewGotKeyboardFocus(e);
            }
        }

        private static void OnGotKeyboardFocusThunk(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is UIElement uie)
            {
                uie.OnGotKeyboardFocus(e);
            }
            else if (sender is ContentElement ce)
            {
                ce.OnGotKeyboardFocus(e);
            }
        }

        private static void OnPreviewLostKeyboardFocusThunk(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is UIElement uie)
            {
                uie.OnPreviewLostKeyboardFocus(e);
            }
            else if (sender is ContentElement ce)
            {
                ce.OnPreviewLostKeyboardFocus(e);
            }
        }

        private static void OnLostKeyboardFocusThunk(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is UIElement uie)
            {
                uie.OnLostKeyboardFocus(e);
            }
            else if (sender is ContentElement ce)
            {
                ce.OnLostKeyboardFocus(e);
            }
        }


        private static void OnPreviewTextInputThunk(object sender, TextCompositionEventArgs e)
        {
            if (sender is UIElement uie)
            {
                uie.OnPreviewTextInput(e);
            }
            else if (sender is ContentElement ce)
            {
                ce.OnPreviewTextInput(e);
            }
        }

        private static void OnTextInputThunk(object sender, TextCompositionEventArgs e)
        {
            if (sender is UIElement uie)
            {
                uie.OnTextInput(e);
            }
            else if (sender is ContentElement ce)
            {
                ce.OnTextInput(e);
            }
        }

        protected virtual void OnPreviewKeyDown(KeyEventArgs e) { }
        protected virtual void OnKeyDown(KeyEventArgs e) { }
        protected virtual void OnPreviewKeyUp(KeyEventArgs e) { }
        protected virtual void OnKeyUp(KeyEventArgs e) { }
        protected virtual void OnPreviewGotKeyboardFocus(KeyboardFocusChangedEventArgs e) { }
        protected virtual void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e) { }
        protected virtual void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e) { }
        protected virtual void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e) { }
        protected virtual void OnIsKeyboardFocusedChanged(DependencyPropertyChangedEventArgs e) { }
        protected virtual void OnPreviewTextInput(TextCompositionEventArgs e) { }
        protected virtual void OnTextInput(TextCompositionEventArgs e) { }

        #endregion

        #region Common

        public static readonly DependencyProperty IsEnabledProperty =
                    DependencyProperty.Register(
                                "IsEnabled",
                                typeof(bool),
                                typeof(UIElement),
                                new UIPropertyMetadata(
                                            true, // default value
                                            new PropertyChangedCallback(OnIsEnabledChanged),
                                            new CoerceValueCallback(CoerceIsEnabled)));
        private static object CoerceIsEnabled(DependencyObject d, object? value)
        {
            UIElement uie = (UIElement)d;

            // We must be false if our parent is false, but we can be
            // either true or false if our parent is true.
            //
            // Another way of saying this is that we can only be true
            // if our parent is true, but we can always be false.
            if ((bool)value!)
            {
                // Our parent can constrain us.  We can be plugged into either
                // a "visual" or "content" tree.  If we are plugged into a
                // "content" tree, the visual tree is just considered a
                // visual representation, and is normally composed of raw
                // visuals, not UIElements, so we prefer the content tree.
                //
                // The content tree uses the "logical" links.  But not all
                // "logical" links lead to a content tree.
                //
                DependencyObject? parent = LogicalTreeHelper.GetParent(uie) as ContentElement;
                if (parent == null)
                {
                    parent = InputElement.GetContainingUIElement(LogicalTreeHelper.GetParent(uie));
                }

                if (parent == null || (bool)parent.GetValue(IsEnabledProperty)!)
                {
                    return uie.IsEnabledCore;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIElement uie = (UIElement)d;
            if (!(bool)e.NewValue! && uie.IsFocused && uie.Dispatcher is UIDispatcher dispatcher)
                dispatcher.SetFocus(null);
            uie.IsEnabledChanged?.Invoke(d, e);
            //// Raise the public changed event.
            //uie.RaiseDependencyPropertyChanged(IsEnabledChangedKey, e);

            //// Invalidate the children so that they will inherit the new value.
            uie.InvalidateInheritPropertyOnChildren(e.Property);

            //// The input manager needs to re-hittest because something changed
            //// that is involved in the hit-testing we do, so a different result
            //// could be returned.
            //InputManager.SafeCurrentNotifyHitTestInvalidated();

            ////Notify Automation in case it is interested.
            //AutomationPeer peer = uie.GetAutomationPeer();
            //if (peer != null)
            //    peer.InvalidatePeer();

        }
        public bool IsEnabled
        {
            get => (bool)GetValue(IsEnabledProperty)!;
            set => SetValue(IsEnabledProperty, value);
        }
        public event DependencyPropertyChangedEventHandler? IsEnabledChanged;

        protected virtual bool IsEnabledCore => true;

        #endregion

        #region Focus

        public static readonly DependencyProperty FocusableProperty =
                DependencyProperty.Register(
                        "Focusable",
                        typeof(bool),
                        typeof(UIElement),
                        new UIPropertyMetadata(
                                false, // default value
                                new PropertyChangedCallback(OnFocusableChanged)));
        private static void OnFocusableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIElement uie = (UIElement)d;

            // Raise the public changed event.
            uie.FocusableChanged?.Invoke(d, e);
        }
        public bool Focusable { get => (bool)GetValue(FocusableProperty)!; set => SetValue(FocusableProperty, value); }
        public event DependencyPropertyChangedEventHandler? FocusableChanged;

        public bool Focus()
        {
            if (Keyboard.PrimaryDevice.Focus(this) == this)
            {
                return true;
            }
            if (Focusable && IsEnabled && Dispatcher is UIDispatcher dispatcher)
            {
                dispatcher.SetFocus(this);
                return true;
            }
            return false;
        }

        internal static readonly DependencyPropertyKey IsFocusedPropertyKey =
                    DependencyProperty.RegisterReadOnly(
                                "IsFocused",
                                typeof(bool),
                                typeof(UIElement),
                                new PropertyMetadata(
                                            false, // default value
                                            new PropertyChangedCallback(IsFocused_Changed)));
        private static void IsFocused_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIElement uiElement = ((UIElement)d);
            if ((bool)e.NewValue!)
                uiElement.RaiseEvent(new RoutedEventArgs(GotFocusEvent, uiElement));
            else
                uiElement.RaiseEvent(new RoutedEventArgs(LostFocusEvent, uiElement));
        }
        public static readonly DependencyProperty IsFocusedProperty = IsFocusedPropertyKey.DependencyProperty;
        public bool IsFocused => (bool)GetValue(IsFocusedProperty)!;

        public static readonly RoutedEvent GotFocusEvent = FocusManager.GotFocusEvent.AddOwner(typeof(UIElement));
        public static readonly RoutedEvent LostFocusEvent = FocusManager.LostFocusEvent.AddOwner(typeof(UIElement));

        public event RoutedEventHandler GotFocus { add { AddHandler(GotFocusEvent, value); } remove { RemoveHandler(GotFocusEvent, value); } }
        public event RoutedEventHandler LostFocus { add { AddHandler(LostFocusEvent, value); } remove { RemoveHandler(LostFocusEvent, value); } }

        private static void OnLostFocusThunk(object sender, RoutedEventArgs e)
        {
            if (!e.Handled)
            {
                if (sender is UIElement ue)
                {
                    ue.OnLostFocus(e);
                }
                else if (sender is ContentElement ce)
                {
                    ce.OnLostFocus(e);
                }
            }
        }
        private static void OnGotFocusThunk(object sender, RoutedEventArgs e)
        {
            if (!e.Handled)
            {
                if (sender is UIElement ue)
                {
                    ue.OnGotFocus(e);
                }
                else if (sender is ContentElement ce)
                {
                    ce.OnGotFocus(e);
                }
            }
        }

        protected virtual void OnGotFocus(RoutedEventArgs e) { }
        protected virtual void OnLostFocus(RoutedEventArgs e) { }

        #endregion

        #region Logical

        protected override void OnLogicalRootChanged(LogicalObject oldRoot, LogicalObject newRoot)
        {
            //reset focused element
            if (IsFocused && oldRoot.Dispatcher is UIDispatcher dispatcher && newRoot.Dispatcher is not UIDispatcher)
                dispatcher.SetFocus(null);
        }

        #endregion

        #region DependencyValue

        protected override bool HandleInvalidateInheritProperty(DependencyProperty dp)
        {
            //if (dp == IsVisibleProperty)
            //{
            //    // For Read-Only force-inherited properties, use
            //    // a private update method.
            //    UpdateIsVisibleCache();
            //}
            //else
            //{
            // For Read/Write force-inherited properties, use
            // the standard coersion pattern.
            CoerceValue(dp);
            //}
            return true;
        }

        #endregion
    }
}
