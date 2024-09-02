using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Controls
{
    public abstract class RangeBase : Control
    {
        #region Properties


        public static readonly DependencyProperty MinimumProperty =
                DependencyProperty.Register(
                        "Minimum",
                        typeof(float),
                        typeof(RangeBase),
                        new FrameworkPropertyMetadata(
                                0f,
                                new PropertyChangedCallback(OnMinimumChanged)),
                        new ValidateValueCallback(IsValidFloatValue));
        private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RangeBase ctrl = (RangeBase)d;
            //RangeBaseAutomationPeer peer = UIElementAutomationPeer.FromElement(ctrl) as RangeBaseAutomationPeer;
            //if (peer != null)
            //{
            //    peer.RaiseMinimumPropertyChangedEvent((float)e.OldValue, (float)e.NewValue);
            //}
            ctrl.CoerceValue(MaximumProperty);
            ctrl.CoerceValue(ValueProperty);
            ctrl.OnMinimumChanged((float)e.OldValue!, (float)e.NewValue!);
        }
        public float Minimum
        {
            get { return (float)GetValue(MinimumProperty)!; }
            set { SetValue(MinimumProperty, value); }
        }

        public static readonly DependencyProperty MaximumProperty =
                DependencyProperty.Register(
                        "Maximum",
                        typeof(float),
                        typeof(RangeBase),
                        new FrameworkPropertyMetadata(
                                1f,
                                new PropertyChangedCallback(OnMaximumChanged),
                                new CoerceValueCallback(CoerceMaximum)),
                        new ValidateValueCallback(IsValidFloatValue));
        private static object CoerceMaximum(DependencyObject d, object? value)
        {
            RangeBase ctrl = (RangeBase)d;
            float min = ctrl.Minimum;
            if ((float)value! < min)
                return min;
            return value;
        }
        private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RangeBase ctrl = (RangeBase)d;
            //RangeBaseAutomationPeer peer = UIElementAutomationPeer.FromElement(ctrl) as RangeBaseAutomationPeer;
            //if (peer != null)
            //{
            //    peer.RaiseMaximumPropertyChangedEvent((float)e.OldValue, (float)e.NewValue);
            //}
            ctrl.CoerceValue(ValueProperty);
            ctrl.OnMaximumChanged((float)e.OldValue!, (float)e.NewValue!);
        }
        public float Maximum
        {
            get { return (float)GetValue(MaximumProperty)!; }
            set { SetValue(MaximumProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
                DependencyProperty.Register(
                        "Value",
                        typeof(float),
                        typeof(RangeBase),
                        new FrameworkPropertyMetadata(
                                0f,
                                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal,
                                new PropertyChangedCallback(OnValueChanged),
                                new CoerceValueCallback(ConstrainToRange)),
                        new ValidateValueCallback(IsValidFloatValue));
        internal static object ConstrainToRange(DependencyObject d, object? value)
        {
            RangeBase ctrl = (RangeBase)d;
            float min = ctrl.Minimum;
            float v = (float)value!;
            if (v < min)
            {
                return min;
            }
            float max = ctrl.Maximum;
            if (v > max)
            {
                return max;
            }
            return value;
        }
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RangeBase ctrl = (RangeBase)d;
            //RangeBaseAutomationPeer peer = UIElementAutomationPeer.FromElement(ctrl) as RangeBaseAutomationPeer;
            //if (peer != null)
            //{
            //    peer.RaiseValuePropertyChangedEvent((float)e.OldValue, (float)e.NewValue);
            //}
            ctrl.OnValueChanged((float)e.OldValue!, (float)e.NewValue!);
        }
        public float Value
        {
            get { return (float)GetValue(ValueProperty)!; }
            set { SetValue(ValueProperty, value); }
        }

        #endregion

        #region Events

        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<float>), typeof(RangeBase));
        public event RoutedPropertyChangedEventHandler<float> ValueChanged { add { AddHandler(ValueChangedEvent, value); } remove { RemoveHandler(ValueChangedEvent, value); } }

        #endregion

        #region Methods

        protected virtual void OnMinimumChanged(float oldMinimum, float newMinimum)
        {
        }

        protected virtual void OnMaximumChanged(float oldMaximum, float newMaximum)
        {
        }

        protected virtual void OnValueChanged(float oldValue, float newValue)
        {
            RoutedPropertyChangedEventArgs<float> args = new RoutedPropertyChangedEventArgs<float>(oldValue, newValue);
            args.RoutedEvent = ValueChangedEvent;
            RaiseEvent(args);
        }

        private static bool IsValidFloatValue(object? value)
        {
            if (value is float d)
                return !(float.IsNaN(d) || float.IsInfinity(d));
            return false;
        }

        #endregion
    }
}
