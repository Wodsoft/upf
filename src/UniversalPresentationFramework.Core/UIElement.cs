using System;
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
            UIElement uie = (UIElement)d;
            Visibility newVisibility = (Visibility)e.NewValue!;
            uie._visibility = newVisibility;
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

        private readonly Dictionary<int, List<RoutedEventHandlerInfo>> _eventHandlers = new Dictionary<int, List<RoutedEventHandlerInfo>>();

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

            if (!_eventHandlers.TryGetValue(routedEvent.GlobalIndex, out var list))
            {
                list = new List<RoutedEventHandlerInfo>();
                _eventHandlers.Add(routedEvent.GlobalIndex, list);
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

            if (_eventHandlers.TryGetValue(routedEvent.GlobalIndex, out var list))
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

            List<Visual> list = new List<Visual>();
            List<List<RoutedEventHandlerInfo>?> handlers = new List<List<RoutedEventHandlerInfo>?>();
            Visual? visual = this;
            while (visual != null)
            {
                list.Add(visual);
                if (visual is UIElement element)
                {
                    handlers.Add(e.RoutedEvent.GetClassHandlers(visual.GetType()));
                    if (element._eventHandlers.TryGetValue(e.RoutedEvent.GlobalIndex, out var delegates))
                        handlers.Add(delegates);
                    else
                        handlers.Add(null);
                }
                if (e.RoutedEvent.RoutingStrategy == RoutingStrategy.Direct)
                    break;
                visual = visual.VisualParent;
                if (list.Count > 4096)
                    throw new InvalidOperationException("Routed event have more than 4096 levels.");
            }

            if (e.RoutedEvent.RoutingStrategy == RoutingStrategy.Bubble || e.RoutedEvent.RoutingStrategy == RoutingStrategy.Direct)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    e.Source = list[i];
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
                    e.Source = list[i];
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

            e.Source = e.OriginalSource;
            e.ClearUserInitiated();
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
                //ContentElement contentElement = d as ContentElement;
                //if (contentElement != null)
                //{
                //    contentElement.AddHandler(routedEvent, handler);
                //}
                //else
                //{
                //    UIElement3D uiElement3D = d as UIElement3D;
                //    if (uiElement3D != null)
                //    {
                //        uiElement3D.AddHandler(routedEvent, handler);
                //    }
                //    else
                //    {
                throw new ArgumentException($"Invalid input element \"{d.GetType().FullName}\".");
                //    }
                //}
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
                //ContentElement contentElement = d as ContentElement;
                //if (contentElement != null)
                //{
                //    contentElement.RemoveHandler(routedEvent, handler);
                //}
                //else
                //{
                //    UIElement3D uiElement3D = d as UIElement3D;
                //    if (uiElement3D != null)
                //    {
                //        uiElement3D.RemoveHandler(routedEvent, handler);
                //    }
                //    else
                //    {
                throw new ArgumentException($"Invalid input element \"{d.GetType().FullName}\".");
                //    }
                //}
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

        public bool IsMouseOver => throw new NotImplementedException();

        public bool IsMouseDirectlyOver => throw new NotImplementedException();

        public bool IsMouseCaptured => throw new NotImplementedException();

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

        #endregion
    }
}
