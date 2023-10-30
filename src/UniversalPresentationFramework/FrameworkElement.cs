using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public class FrameworkElement : UIElement
    {
        #region Logical

        public FrameworkElement? Parent { get; private set; }

        protected override DependencyObject? GetInheritanceParent()
        {
            return Parent;
        }

        protected internal void AddLogicalChild(FrameworkElement child)
        {
            if (child == null)
                throw new ArgumentNullException(nameof(child));
            if (child.Parent != null)
                throw new InvalidOperationException("Child has its parent already.");
            child.Parent = this;
        }

        protected internal void RemoveLogicalChild(FrameworkElement child)
        {
            if (child == null)
                throw new ArgumentNullException(nameof(child));
            if (child.Parent == null)
                throw new InvalidOperationException("Child has no parent.");
            if (child.Parent != this)
                throw new InvalidOperationException("This element is not the parent of child.");
            child.Parent = null;
        }

        #endregion

        #region Size

        /// <summary>
        /// Width Dependency Property
        /// </summary>
        public static readonly DependencyProperty WidthProperty =
                    DependencyProperty.Register(
                                "Width",
                                typeof(float),
                                typeof(FrameworkElement),
                                new FrameworkPropertyMetadata(
                                        float.NaN,
                                        FrameworkPropertyMetadataOptions.AffectsMeasure,
                                        new PropertyChangedCallback(OnTransformDirty)),
                                new ValidateValueCallback(IsWidthHeightValid));

        /// <summary>
        /// Width Property
        /// </summary>
        [TypeConverter(typeof(LengthConverter))]
        public float Width
        {
            get { return (float)GetValue(WidthProperty)!; }
            set { SetValue(WidthProperty, value); }
        }

        /// <summary>
        /// MinWidth Dependency Property
        /// </summary>
        public static readonly DependencyProperty MinWidthProperty =
                    DependencyProperty.Register(
                                "MinWidth",
                                typeof(float),
                                typeof(FrameworkElement),
                                new FrameworkPropertyMetadata(
                                        0f,
                                        FrameworkPropertyMetadataOptions.AffectsMeasure,
                                        new PropertyChangedCallback(OnTransformDirty)),
                                new ValidateValueCallback(IsMinWidthHeightValid));

        /// <summary>
        /// MinWidth Property
        /// </summary>
        [TypeConverter(typeof(LengthConverter))]
        public float MinWidth
        {
            get { return (float)GetValue(MinWidthProperty)!; }
            set { SetValue(MinWidthProperty, value); }
        }

        /// <summary>
        /// MaxWidth Dependency Property
        /// </summary>
        public static readonly DependencyProperty MaxWidthProperty =
                    DependencyProperty.Register(
                                "MaxWidth",
                                typeof(float),
                                typeof(FrameworkElement),
                                new FrameworkPropertyMetadata(
                                        float.PositiveInfinity,
                                        FrameworkPropertyMetadataOptions.AffectsMeasure,
                                        new PropertyChangedCallback(OnTransformDirty)),
                                new ValidateValueCallback(IsMaxWidthHeightValid));


        /// <summary>
        /// MaxWidth Property
        /// </summary>
        [TypeConverter(typeof(LengthConverter))]
        public float MaxWidth
        {
            get { return (float)GetValue(MaxWidthProperty)!; }
            set { SetValue(MaxWidthProperty, value); }
        }

        /// <summary>
        /// Height Dependency Property
        /// </summary>
        public static readonly DependencyProperty HeightProperty =
                    DependencyProperty.Register(
                                "Height",
                                typeof(float),
                                typeof(FrameworkElement),
                                new FrameworkPropertyMetadata(
                                    float.NaN,
                                    FrameworkPropertyMetadataOptions.AffectsMeasure,
                                    new PropertyChangedCallback(OnTransformDirty)),
                                new ValidateValueCallback(IsWidthHeightValid));

        /// <summary>
        /// Height Property
        /// </summary>
        [TypeConverter(typeof(LengthConverter))]
        public float Height
        {
            get { return (float)GetValue(HeightProperty)!; }
            set { SetValue(HeightProperty, value); }
        }

        /// <summary>
        /// MinHeight Dependency Property
        /// </summary>
        public static readonly DependencyProperty MinHeightProperty =
                    DependencyProperty.Register(
                                "MinHeight",
                                typeof(float),
                                typeof(FrameworkElement),
                                new FrameworkPropertyMetadata(
                                        0f,
                                        FrameworkPropertyMetadataOptions.AffectsMeasure,
                                        new PropertyChangedCallback(OnTransformDirty)),
                                new ValidateValueCallback(IsMinWidthHeightValid));

        /// <summary>
        /// MinHeight Property
        /// </summary>
        [TypeConverter(typeof(LengthConverter))]
        public float MinHeight
        {
            get { return (float)GetValue(MinHeightProperty)!; }
            set { SetValue(MinHeightProperty, value); }
        }

        /// <summary>
        /// MaxHeight Dependency Property
        /// </summary>
        public static readonly DependencyProperty MaxHeightProperty =
                    DependencyProperty.Register(
                                "MaxHeight",
                                typeof(float),
                                typeof(FrameworkElement),
                                new FrameworkPropertyMetadata(
                                        float.PositiveInfinity,
                                        FrameworkPropertyMetadataOptions.AffectsMeasure,
                                        new PropertyChangedCallback(OnTransformDirty)),
                                new ValidateValueCallback(IsMaxWidthHeightValid));

        /// <summary>
        /// MaxHeight Property
        /// </summary>
        [TypeConverter(typeof(LengthConverter))]
        public float MaxHeight
        {
            get { return (float)GetValue(MaxHeightProperty)!; }
            set { SetValue(MaxHeightProperty, value); }
        }

        private static bool IsWidthHeightValid(object? value)
        {
            if (value is float v)
                return (float.IsNaN(v)) || (v >= 0f && !float.IsPositiveInfinity(v));
            return false;
        }

        private static bool IsMinWidthHeightValid(object? value)
        {
            if (value is float v)
                return (!float.IsNaN(v) && v >= 0f && !float.IsPositiveInfinity(v));
            return false;
        }

        private static bool IsMaxWidthHeightValid(object? value)
        {
            if (value is float v)
                return (!float.IsNaN(v) && v >= 0f);
            return false;
        }

        private static void OnTransformDirty(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Callback for MinWidth, MaxWidth, Width, MinHeight, MaxHeight, Height, and RenderTransformOffset
            //FrameworkElement fe = (FrameworkElement)d;
            //fe.AreTransformsClean = false;
        }

        #endregion

        #region Layout

        protected sealed override Size MeasureCore(Size availableSize)
        {
            return base.MeasureCore(availableSize);
        }

        protected sealed override void ArrangeCore(Rect finalRect)
        {
            base.ArrangeCore(finalRect);
        }

        protected virtual Size MeasureOverride(Size availableSize)
        {
            return new Size(0, 0);
        }

        protected virtual Size ArrangeOverride(Size finalSize)
        {
            return finalSize;
        }

        #endregion
    }
}
