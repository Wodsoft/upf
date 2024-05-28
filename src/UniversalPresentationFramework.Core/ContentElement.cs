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

            UIElement.IsFocusedPropertyKey.OverrideMetadata(
                typeof(ContentElement),
                new PropertyMetadata(
                    false, // default value
                    new PropertyChangedCallback(IsFocused_Changed)));
            UIElement.IsKeyboardFocusedPropertyKey.OverrideMetadata(
                typeof(ContentElement),
                new PropertyMetadata(false,
                    new PropertyChangedCallback(IsKeyboardFocused_Changed)));
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
            List<(IInputElement Element, List<RoutedEventHandlerInfo>? Delegates)> handlers = new List<(IInputElement Element, List<RoutedEventHandlerInfo>?)>();
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
                                    reh(classHandlers.Delegates, e);
                                else
                                    e.InvokeHandler(handler.Handler, classHandlers.Delegates);
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
                                    reh(classHandlers.Delegates, e);
                                else
                                    e.InvokeHandler(handler.Handler, classHandlers.Delegates);
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

        #endregion

        #region Mouse

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

        #region Keyboard

        private static void IsKeyboardFocused_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ContentElement ce = (ContentElement)d;
            ce.OnIsKeyboardFocusedChanged(e);
            ce.IsKeyboardFocusedChanged?.Invoke(d, e);
        }
        public event DependencyPropertyChangedEventHandler? IsKeyboardFocusedChanged;
        public bool IsKeyboardFocused => Keyboard.FocusedElement == this;

        public static readonly RoutedEvent PreviewKeyDownEvent = Keyboard.PreviewKeyDownEvent.AddOwner(typeof(ContentElement));
        public static readonly RoutedEvent KeyDownEvent = Keyboard.KeyDownEvent.AddOwner(typeof(ContentElement));
        public static readonly RoutedEvent PreviewKeyUpEvent = Keyboard.PreviewKeyUpEvent.AddOwner(typeof(ContentElement));
        public static readonly RoutedEvent KeyUpEvent = Keyboard.KeyUpEvent.AddOwner(typeof(ContentElement));
        public static readonly RoutedEvent PreviewGotKeyboardFocusEvent = Keyboard.PreviewGotKeyboardFocusEvent.AddOwner(typeof(ContentElement));
        public static readonly RoutedEvent GotKeyboardFocusEvent = Keyboard.GotKeyboardFocusEvent.AddOwner(typeof(ContentElement));
        public static readonly RoutedEvent PreviewLostKeyboardFocusEvent = Keyboard.PreviewLostKeyboardFocusEvent.AddOwner(typeof(ContentElement));
        public static readonly RoutedEvent LostKeyboardFocusEvent = Keyboard.LostKeyboardFocusEvent.AddOwner(typeof(ContentElement));

        public event KeyEventHandler PreviewKeyDown { add { AddHandler(Keyboard.PreviewKeyDownEvent, value, false); } remove { RemoveHandler(Keyboard.PreviewKeyDownEvent, value); } }
        public event KeyEventHandler KeyDown { add { AddHandler(Keyboard.KeyDownEvent, value, false); } remove { RemoveHandler(Keyboard.KeyDownEvent, value); } }
        public event KeyEventHandler PreviewKeyUp { add { AddHandler(Keyboard.PreviewKeyUpEvent, value, false); } remove { RemoveHandler(Keyboard.PreviewKeyUpEvent, value); } }
        public event KeyEventHandler KeyUp { add { AddHandler(Keyboard.KeyUpEvent, value, false); } remove { RemoveHandler(Keyboard.KeyUpEvent, value); } }
        public event KeyboardFocusChangedEventHandler PreviewGotKeyboardFocus { add { AddHandler(Keyboard.PreviewGotKeyboardFocusEvent, value, false); } remove { RemoveHandler(Keyboard.PreviewGotKeyboardFocusEvent, value); } }
        public event KeyboardFocusChangedEventHandler GotKeyboardFocus { add { AddHandler(Keyboard.GotKeyboardFocusEvent, value, false); } remove { RemoveHandler(Keyboard.GotKeyboardFocusEvent, value); } }
        public event KeyboardFocusChangedEventHandler PreviewLostKeyboardFocus { add { AddHandler(Keyboard.PreviewLostKeyboardFocusEvent, value, false); } remove { RemoveHandler(Keyboard.PreviewLostKeyboardFocusEvent, value); } }
        public event KeyboardFocusChangedEventHandler LostKeyboardFocus { add { AddHandler(Keyboard.LostKeyboardFocusEvent, value, false); } remove { RemoveHandler(Keyboard.LostKeyboardFocusEvent, value); } }
                
        protected internal virtual void OnPreviewKeyDown(KeyEventArgs e) { }
        protected internal virtual void OnKeyDown(KeyEventArgs e) { }
        protected internal virtual void OnPreviewKeyUp(KeyEventArgs e) { }
        protected internal virtual void OnKeyUp(KeyEventArgs e) { }
        protected internal virtual void OnPreviewGotKeyboardFocus(KeyboardFocusChangedEventArgs e) { }
        protected internal virtual void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e) { }
        protected internal virtual void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e) { }
        protected internal virtual void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e) { }
        protected virtual void OnIsKeyboardFocusedChanged(DependencyPropertyChangedEventArgs e) { }

        #endregion

        #region Common

        public static readonly DependencyProperty IsEnabledProperty = UIElement.IsEnabledProperty.AddOwner(typeof(ContentElement),
                                new UIPropertyMetadata(
                                            true, // default value
                                            new PropertyChangedCallback(OnIsEnabledChanged),
                                            new CoerceValueCallback(CoerceIsEnabled)));
        private static object CoerceIsEnabled(DependencyObject d, object? value)
        {
            ContentElement ce = (ContentElement)d;

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
                DependencyObject? parent = ce.LogicalParent as ContentElement;
                if (parent == null)
                {
                    parent = InputElement.GetContainingUIElement(ce.LogicalParent);
                }

                if (parent == null || (bool)parent.GetValue(IsEnabledProperty)!)
                {
                    return ce.IsEnabledCore;
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
            ContentElement ce = (ContentElement)d;
            if (!(bool)e.NewValue! && ce.IsFocused && ce.Dispatcher is UIDispatcher dispatcher)
                dispatcher.SetFocus(null);
            ce.IsEnabledChanged?.Invoke(d, e);
            //// Raise the public changed event.
            //uie.RaiseDependencyPropertyChanged(IsEnabledChangedKey, e);

            //// Invalidate the children so that they will inherit the new value.
            ce.InvalidateInheritPropertyOnChildren(e.Property);

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


        public static readonly DependencyProperty FocusableProperty = UIElement.FocusableProperty.AddOwner(typeof(ContentElement),
                        new UIPropertyMetadata(
                                false, // default value
                                new PropertyChangedCallback(OnFocusableChanged)));
        private static void OnFocusableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ContentElement ce = (ContentElement)d;

            // Raise the public changed event.
            ce.FocusableChanged?.Invoke(d, e);
        }
        public bool Focusable { get => (bool)GetValue(FocusableProperty)!; set => SetValue(FocusableProperty, value); }
        public event DependencyPropertyChangedEventHandler? FocusableChanged;

        public bool Focus()
        {
            if (Focusable && IsEnabled && Dispatcher is UIDispatcher dispatcher)
            {
                dispatcher.SetFocus(this);
                return true;
            }
            return false;
        }

        public static readonly DependencyProperty IsFocusedProperty =
                    UIElement.IsFocusedProperty.AddOwner(typeof(ContentElement));
        private static void IsFocused_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ContentElement ce = ((ContentElement)d);
            if ((bool)e.NewValue!)
                ce.OnGotFocus(new RoutedEventArgs(GotFocusEvent, ce));
            else
                ce.OnLostFocus(new RoutedEventArgs(LostFocusEvent, ce));
        }
        public bool IsFocused => (bool)GetValue(IsFocusedProperty)!;

        public static readonly RoutedEvent GotFocusEvent = FocusManager.GotFocusEvent.AddOwner(typeof(ContentElement));
        public static readonly RoutedEvent LostFocusEvent = FocusManager.LostFocusEvent.AddOwner(typeof(ContentElement));

        public event RoutedEventHandler GotFocus { add { AddHandler(GotFocusEvent, value); } remove { RemoveHandler(GotFocusEvent, value); } }
        public event RoutedEventHandler LostFocus { add { AddHandler(LostFocusEvent, value); } remove { RemoveHandler(LostFocusEvent, value); } }

        protected internal virtual void OnGotFocus(RoutedEventArgs e) { }
        protected internal virtual void OnLostFocus(RoutedEventArgs e) { }

        #endregion

        #region Logical

        protected override void OnLogicalRootChanged(LogicalObject oldRoot, LogicalObject newRoot)
        {
            //reset focused element
            if (IsFocused && oldRoot.Dispatcher is UIDispatcher dispatcher && newRoot.Dispatcher is not UIDispatcher)
                dispatcher.SetFocus(null);
        }

        #endregion
    }
}
