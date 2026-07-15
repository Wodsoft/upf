using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Documents;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Controls.Primitives
{
    public class Track : FrameworkElement
    {
        private RepeatButton? _increaseButton;
        private RepeatButton? _decreaseButton;
        private Thumb? _thumb;
        private float _density = float.NaN;
        private float _thumbCenterOffset = float.NaN;

        #region Properties

        /// <summary>
        /// The RepeatButton used to decrease the Value
        /// </summary>
        public RepeatButton? DecreaseRepeatButton
        {
            get
            {
                return _decreaseButton;
            }
            set
            {
                if (_decreaseButton == value)
                    return;
                if (_increaseButton == value)
                    throw new NotSupportedException("Could not use same button with increase repeat button.");
                if (_decreaseButton != null)
                    RemoveVisualChild(_decreaseButton);
                if (value != null)
                    AddVisualChild(value);
                _decreaseButton = value;
                //if (_decreaseButton != null)
                //{
                //    CommandManager.InvalidateRequerySuggested(); // Should post an idle queue item to update IsEnabled on button
                //}
            }
        }

        /// <summary>
        /// The Thumb in the Track
        /// </summary>
        public Thumb? Thumb
        {
            get
            {
                return _thumb;
            }
            set
            {
                if (_thumb == value)
                    return;
                if (_thumb != null)
                    RemoveVisualChild(_thumb);
                if (value != null)
                    AddVisualChild(value);
                _thumb = value;
            }
        }

        /// <summary>
        /// The RepeatButton used to increase the Value
        /// </summary>
        public RepeatButton? IncreaseRepeatButton
        {
            get
            {
                return _increaseButton;
            }
            set
            {
                if (_increaseButton == value)
                    return;
                if (_decreaseButton == value)
                    throw new NotSupportedException("Could not use same button with decrease repeat button.");
                if (_increaseButton != null)
                    RemoveVisualChild(_increaseButton);
                if (value != null)
                    AddVisualChild(value);
                _increaseButton = value;

                //if (_increaseButton != null)
                //{
                //    CommandManager.InvalidateRequerySuggested(); // Should post an idle queue item to update IsEnabled on button
                //}
            }
        }

        /// <summary>
        /// DependencyProperty for <see cref="Orientation" /> property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
                DependencyProperty.Register("Orientation", typeof(Orientation), typeof(Track),
                                          new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsMeasure),
                                          new ValidateValueCallback(ScrollBar.IsValidOrientation));

        /// <summary>
        /// This property represents the Track layout orientation: Vertical or Horizontal.
        /// On vertical ScrollBars, the thumb moves up and down.  On horizontal bars, the thumb moves left to right.
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty)!; }
            set { SetValue(OrientationProperty, value); }
        }


        /// <summary>
        /// DependencyProperty for <see cref="Minimum" /> property.
        /// </summary>
        public static readonly DependencyProperty MinimumProperty =
                RangeBase.MinimumProperty.AddOwner(typeof(Track),
                        new FrameworkPropertyMetadata(0f, FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// The Minimum value of the Slider or ScrollBar
        /// </summary>
        public float Minimum
        {
            get { return (float)GetValue(MinimumProperty)!; }
            set { SetValue(MinimumProperty, value); }
        }


        /// <summary>
        /// DependencyProperty for <see cref="Maximum" /> property.
        /// </summary>
        public static readonly DependencyProperty MaximumProperty =
                RangeBase.MaximumProperty.AddOwner(typeof(Track),
                        new FrameworkPropertyMetadata(1f, FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// The Maximum value of the Slider or ScrollBar
        /// </summary>
        public float Maximum
        {
            get { return (float)GetValue(MaximumProperty)!; }
            set { SetValue(MaximumProperty, value); }
        }


        /// <summary>
        /// DependencyProperty for <see cref="Value" /> property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
                RangeBase.ValueProperty.AddOwner(typeof(Track),
                        new FrameworkPropertyMetadata(0f, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// The current value of the Slider or ScrollBar
        /// </summary>
        public float Value
        {
            get { return (float)GetValue(ValueProperty)!; }
            set { SetValue(ValueProperty, value); }
        }


        /// <summary>
        /// DependencyProperty for <see cref="ViewportSize" /> property.
        /// </summary>
        public static readonly DependencyProperty ViewportSizeProperty =
                DependencyProperty.Register("ViewportSize",
                        typeof(float),
                        typeof(Track),
                        new FrameworkPropertyMetadata(float.NaN, FrameworkPropertyMetadataOptions.AffectsArrange),
                        new ValidateValueCallback(IsValidViewport));

        /// <summary>
        /// ViewportSize is the amount of the scrolled extent currently visible.  For most scrolled content, this value
        /// will be bound to one of <see cref="ScrollViewer" />'s ViewportSize properties.
        /// This property is in logical scrolling units.
        /// 
        /// Setting this value to NaN will turn off automatic sizing of the thumb
        /// </summary>
        public float ViewportSize
        {
            get { return (float)GetValue(ViewportSizeProperty)!; }
            set { SetValue(ViewportSizeProperty, value); }
        }

        private static bool IsValidViewport(object? o)
        {
            float d = (float)o!;
            return d >= 0.0f || float.IsNaN(d);
        }


        /// <summary>
        /// DependencyProperty for <see cref="IsDirectionReversed" /> property.
        /// </summary>
        public static readonly DependencyProperty IsDirectionReversedProperty =
                DependencyProperty.Register("IsDirectionReversed",
                                            typeof(bool),
                                            typeof(Track),
                                            new FrameworkPropertyMetadata(false));

        /// <summary>
        /// Indicates if the location of the DecreaseRepeatButton and IncreaseRepeatButton 
        /// should be swapped.
        /// </summary>
        public bool IsDirectionReversed
        {
            get { return (bool)GetValue(IsDirectionReversedProperty)!; }
            set { SetValue(IsDirectionReversedProperty, value); }
        }

        protected override int EffectiveValuesInitialSize => 28;

        #endregion

        #region Methods

        /// <summary>
        /// Calculate the value from given Point. The input point is relative to TopLeft conner of Track.
        /// </summary>
        /// <param name="pt">Point (in Track's co-ordinate).</param>        
        public virtual float ValueFromPoint(Point pt)
        {
            float val;
            // Find distance from center of thumb to given point.
            if (Orientation == Orientation.Horizontal)
            {
                val = Value + ValueFromDistance(pt.X - _thumbCenterOffset, pt.Y - (RenderSize.Height * 0.5f));
            }
            else
            {
                val = Value + ValueFromDistance(pt.X - (RenderSize.Width * 0.5f), pt.Y - _thumbCenterOffset);
            }
            return Math.Max(Minimum, Math.Min(Maximum, val));
        }

        /// <summary>
        /// This function returns the delta in value that would be caused by moving the thumb the given pixel distances.
        /// The returned delta value is not guaranteed to be inside the valid Value range.
        /// </summary>
        /// <param name="horizontal">Total horizontal distance that the Thumb has moved.</param>
        /// <param name="vertical">Total vertical distance that the Thumb has moved.</param>        
        public virtual float ValueFromDistance(float horizontal, float vertical)
        {
            float scale = IsDirectionReversed ? -1 : 1;
            //
            // Note: To implement 'Snap-Back' feature, we could check whether the point is far away from center of the track.
            // If so, just return current value (this should move the Thumb back to its original localtion).
            //
            if (Orientation == Orientation.Horizontal)
            {
                return scale * horizontal * _density;
            }
            else
            {
                // Increases in y cause decreases in Sliders value
                return -1 * scale * vertical * _density;
            }
        }

        protected internal override Visual GetVisualChild(int index)
        {
            switch (index)
            {
                case 0:
                    return (Visual?)_decreaseButton ?? (Visual?)_thumb ?? (Visual?)_increaseButton ?? throw new ArgumentOutOfRangeException("index");
                case 1:
                    return (Visual?)_thumb ?? (Visual?)_increaseButton ?? throw new ArgumentOutOfRangeException("index");
                case 2:
                    return (Visual?)_increaseButton ?? throw new ArgumentOutOfRangeException("index");
                default:
                    throw new ArgumentOutOfRangeException("index");
            }
        }

        protected internal override int VisualChildrenCount
        {
            get
            {
                int count = 0;
                if (_decreaseButton != null)
                    count++;
                if (_thumb != null)
                    count++;
                if (_increaseButton != null)
                    count++;
                return count;
            }
        }

        #endregion
    }
}
