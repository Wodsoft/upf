using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Input;
using Wodsoft.UI.Media;
using Wodsoft.UI.Media.Animation;
using Wodsoft.UI.Threading;

namespace Wodsoft.UI
{
    public class ContentElement : LogicalObject, IInputElement, IAnimatable
    {
        #region Constructor

        static ContentElement()
        {
            UIElement.RegisterEvents(typeof(ContentElement));
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

        #endregion

        #region InputElement

        public static readonly RoutedEvent PreviewMouseLeftButtonDownEvent = EventManager.RegisterRoutedEvent("PreviewMouseLeftButtonDown", RoutingStrategy.Direct, typeof(MouseButtonEventHandler), typeof(ContentElement));
        public static readonly RoutedEvent MouseLeftButtonDownEvent = EventManager.RegisterRoutedEvent("MouseLeftButtonDown", RoutingStrategy.Direct, typeof(MouseButtonEventHandler), typeof(ContentElement));
        public static readonly RoutedEvent PreviewMouseLeftButtonUpEvent = EventManager.RegisterRoutedEvent("PreviewMouseLeftButtonUp", RoutingStrategy.Direct, typeof(MouseButtonEventHandler), typeof(ContentElement));
        public static readonly RoutedEvent MouseLeftButtonUpEvent = EventManager.RegisterRoutedEvent("MouseLeftButtonUp", RoutingStrategy.Direct, typeof(MouseButtonEventHandler), typeof(ContentElement));
        public static readonly RoutedEvent PreviewMouseRightButtonDownEvent = EventManager.RegisterRoutedEvent("PreviewMouseRightButtonDown", RoutingStrategy.Direct, typeof(MouseButtonEventHandler), typeof(ContentElement));
        public static readonly RoutedEvent MouseRightButtonDownEvent = EventManager.RegisterRoutedEvent("MouseRightButtonDown", RoutingStrategy.Direct, typeof(MouseButtonEventHandler), typeof(ContentElement));
        public static readonly RoutedEvent PreviewMouseRightButtonUpEvent = EventManager.RegisterRoutedEvent("PreviewMouseRightButtonUp", RoutingStrategy.Direct, typeof(MouseButtonEventHandler), typeof(ContentElement));
        public static readonly RoutedEvent MouseRightButtonUpEvent = EventManager.RegisterRoutedEvent("MouseRightButtonUp", RoutingStrategy.Direct, typeof(MouseButtonEventHandler), typeof(ContentElement));

        public static readonly RoutedEvent PreviewMouseMoveEvent = Mouse.PreviewMouseMoveEvent.AddOwner(typeof(ContentElement));
        public static readonly RoutedEvent MouseMoveEvent = Mouse.MouseMoveEvent.AddOwner(typeof(ContentElement));
        public static readonly RoutedEvent PreviewMouseWheelEvent = Mouse.PreviewMouseWheelEvent.AddOwner(typeof(ContentElement));
        public static readonly RoutedEvent MouseWheelEvent = Mouse.MouseWheelEvent.AddOwner(typeof(ContentElement));
        public static readonly RoutedEvent MouseEnterEvent = Mouse.MouseEnterEvent.AddOwner(typeof(ContentElement));
        public static readonly RoutedEvent MouseLeaveEvent = Mouse.MouseLeaveEvent.AddOwner(typeof(ContentElement));
        public static readonly RoutedEvent GotMouseCaptureEvent = Mouse.GotMouseCaptureEvent.AddOwner(typeof(ContentElement));
        public static readonly RoutedEvent LostMouseCaptureEvent = Mouse.LostMouseCaptureEvent.AddOwner(typeof(ContentElement));

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

        public static readonly DependencyProperty IsMouseOverProperty = UIElement.IsMouseOverProperty.AddOwner(typeof(ContentElement));
        public bool IsMouseOver { get; internal set; }

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

        protected internal virtual void OnPreviewMouseDown(MouseButtonEventArgs e) { }

        protected internal virtual void OnMouseDown(MouseButtonEventArgs e) { }

        protected internal virtual void OnPreviewMouseUp(MouseButtonEventArgs e) { }

        protected internal virtual void OnMouseUp(MouseButtonEventArgs e) { }

        protected internal virtual void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e) { }

        protected internal virtual void OnMouseLeftButtonDown(MouseButtonEventArgs e) { }

        protected internal virtual void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e) { }

        protected internal virtual void OnMouseLeftButtonUp(MouseButtonEventArgs e) { }

        protected internal virtual void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e) { }

        protected internal virtual void OnMouseRightButtonDown(MouseButtonEventArgs e) { }

        protected internal virtual void OnPreviewMouseRightButtonUp(MouseButtonEventArgs e) { }

        protected internal virtual void OnMouseRightButtonUp(MouseButtonEventArgs e) { }

        protected internal virtual void OnPreviewMouseMove(MouseEventArgs e) { }

        protected internal virtual void OnMouseMove(MouseEventArgs e) { }

        protected internal virtual void OnPreviewMouseWheel(MouseWheelEventArgs e) { }

        protected internal virtual void OnMouseWheel(MouseWheelEventArgs e) { }

        protected internal virtual void OnMouseEnter(MouseEventArgs e) { }

        protected internal virtual void OnMouseLeave(MouseEventArgs e) { }

        protected internal virtual void OnGotMouseCapture(MouseEventArgs e) { }

        protected internal virtual void OnLostMouseCapture(MouseEventArgs e) { }

        protected internal virtual void OnQueryCursor(QueryCursorEventArgs e) { }

        #endregion
    }
}
