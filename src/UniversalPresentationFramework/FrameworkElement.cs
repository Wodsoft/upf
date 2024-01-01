using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using Wodsoft.UI.Controls;
using Wodsoft.UI.Data;
using Wodsoft.UI.Media;

namespace Wodsoft.UI
{
    [RuntimeNameProperty("Name")]
    public class FrameworkElement : UIElement, ISupportInitialize
    {
        #region Initialize

        public bool IsInitPending { get; private set; }

        void ISupportInitialize.BeginInit()
        {
            if (IsInitPending)
                throw new InvalidOperationException("Element is initializing.");
            IsInitPending = true;
            BeginInit();
        }

        void ISupportInitialize.EndInit()
        {
            if (!IsInitPending)
                throw new InvalidOperationException("Element is not initializing.");
            if (_style == null)
                InvalidateProperty(StyleProperty);
            EndInit();
            IsInitPending = false;
        }

        protected virtual void BeginInit()
        {
        }

        protected virtual void EndInit()
        {
        }

        #endregion

        #region Logical

        public LogicalObject? Parent => LogicalParent;

        internal new void AddLogicalChild(LogicalObject child)
        {
            base.AddLogicalChild(child);
        }

        internal new void RemoveLogicalChild(LogicalObject child)
        {
            base.RemoveLogicalChild(child);
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

        public static readonly DependencyProperty MarginProperty = DependencyProperty.Register("Margin", typeof(Thickness), typeof(FrameworkElement),
                                  new FrameworkPropertyMetadata(
                                        new Thickness(),
                                        FrameworkPropertyMetadataOptions.AffectsMeasure),
                                  new ValidateValueCallback(IsMarginValid));

        private static bool IsMarginValid(object? value)
        {
            if (value is Thickness m)
                return m.IsValid(true, false, true, false);
            return false;
        }

        /// <summary>
        /// Margin Property
        /// </summary>
        public Thickness Margin
        {
            get { return (Thickness)GetValue(MarginProperty)!; }
            set { SetValue(MarginProperty, value); }
        }

        public static readonly DependencyProperty HorizontalAlignmentProperty =
            DependencyProperty.Register(
                        "HorizontalAlignment",
                        typeof(HorizontalAlignment),
                        typeof(FrameworkElement),
                        new FrameworkPropertyMetadata(
                                    HorizontalAlignment.Stretch,
                                    FrameworkPropertyMetadataOptions.AffectsArrange));
        public HorizontalAlignment HorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(HorizontalAlignmentProperty)!; }
            set { SetValue(HorizontalAlignmentProperty, value); }
        }

        public static readonly DependencyProperty VerticalAlignmentProperty =
            DependencyProperty.Register(
                        "VerticalAlignment",
                        typeof(VerticalAlignment),
                        typeof(FrameworkElement),
                        new FrameworkPropertyMetadata(
                                    VerticalAlignment.Stretch,
                                    FrameworkPropertyMetadataOptions.AffectsArrange));
        public VerticalAlignment VerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(VerticalAlignmentProperty)!; }
            set { SetValue(VerticalAlignmentProperty, value); }
        }

        #endregion

        #region Layout

        protected sealed override Size MeasureCore(Size availableSize)
        {
            ApplyTemplate();

            Thickness margin = Margin;
            float marginWidth = margin.Left + margin.Right;
            float marginHeight = margin.Top + margin.Bottom;

            float width = MathF.Max(availableSize.Width - marginWidth, 0);
            float height = MathF.Max(availableSize.Height - marginHeight, 0);
            MinMax mm = new MinMax(this);
            width = Math.Max(mm.minWidth, Math.Min(width, mm.maxWidth));
            height = Math.Max(mm.minHeight, Math.Min(height, mm.maxHeight));

            Size desiredSize = MeasureOverride(new Size(width, height));
            width = MathF.Min(Math.Max(desiredSize.Width, mm.minWidth), mm.maxWidth) + marginWidth;
            height = MathF.Min(Math.Max(desiredSize.Height, mm.minHeight), mm.maxHeight) + marginHeight;
            if (width < 0)
                width = 0;
            if (height < 0)
                height = 0;
            return new Size(width, height);
        }

        protected sealed override void ArrangeCore(Rect finalRect)
        {
            Size arrangeSize = finalRect.Size;

            var margin = Margin;
            float marginWidth = margin.Left + margin.Right;
            float marginHeight = margin.Top + margin.Bottom;
            //Available size
            arrangeSize.Width = Math.Max(0, arrangeSize.Width - marginWidth);
            arrangeSize.Height = Math.Max(0, arrangeSize.Height - marginHeight);

            var desiredSize = DesiredSize;
            //Really measure size
            Size unclippedDesiredSize = new Size(Math.Max(0, desiredSize.Width - marginWidth),
                                                    Math.Max(0, desiredSize.Height - marginHeight));
            if (unclippedDesiredSize.Width < arrangeSize.Width)
                arrangeSize.Width = unclippedDesiredSize.Width;
            if (unclippedDesiredSize.Height < arrangeSize.Height)
                arrangeSize.Height = unclippedDesiredSize.Height;

            //if (HorizontalAlignment == HorizontalAlignment.Stretch)
            //    arrangeSize.Width = MathF.Max(arrangeSize.Width, unclippedDesiredSize.Width);
            //else
            //    arrangeSize.Width = unclippedDesiredSize.Width;
            //if (VerticalAlignment == VerticalAlignment.Stretch)
            //    arrangeSize.Height = MathF.Max(arrangeSize.Height, unclippedDesiredSize.Height);
            //else
            //    arrangeSize.Height = unclippedDesiredSize.Height;

            //MinMax mm = new MinMax(this);
            //if (mm.minWidth > unclippedDesiredSize.Width)
            //    unclippedDesiredSize.Width = mm.minWidth;
            //if (mm.maxWidth < unclippedDesiredSize.Width)
            //    unclippedDesiredSize.Width = mm.maxWidth;
            //if (mm.minHeight > unclippedDesiredSize.Height)
            //    unclippedDesiredSize.Height = mm.minHeight;
            //if (mm.maxHeight < unclippedDesiredSize.Height)
            //    unclippedDesiredSize.Height = mm.maxHeight;

            Size renderSize = ArrangeOverride(arrangeSize);

            var hAlignment = HorizontalAlignment;
            var vAlignment = VerticalAlignment;
            float x, y;
            if (hAlignment == HorizontalAlignment.Left)
                x = margin.Left;
            else if (hAlignment == HorizontalAlignment.Right)
                x = finalRect.Width - renderSize.Width - margin.Right;
            else
                x = (finalRect.Width - renderSize.Width + margin.Left - margin.Right) / 2;
            if (vAlignment == VerticalAlignment.Top)
                y = margin.Top;
            else if (vAlignment == VerticalAlignment.Bottom)
                y = finalRect.Height - renderSize.Height - margin.Bottom;
            else
                y = (finalRect.Height - renderSize.Height + margin.Top - margin.Bottom) / 2;
            VisualOffset = new Vector2(finalRect.X + x, finalRect.Y + y);
            RenderSize = renderSize;
        }

        protected virtual Size MeasureOverride(Size availableSize)
        {
            return new Size(0, 0);
        }

        protected virtual Size ArrangeOverride(Size finalSize)
        {
            return finalSize;
        }

        private struct MinMax
        {
            internal MinMax(FrameworkElement e)
            {
                maxHeight = e.MaxHeight;
                minHeight = e.MinHeight;
                float l = e.Height;

                float height = (float.IsNaN(l) ? float.PositiveInfinity : l);
                maxHeight = MathF.Max(MathF.Min(height, maxHeight), minHeight);

                height = (float.IsNaN(l) ? 0 : l);
                minHeight = MathF.Max(MathF.Min(maxHeight, height), minHeight);

                maxWidth = e.MaxWidth;
                minWidth = e.MinWidth;
                l = e.Width;

                float width = (float.IsNaN(l) ? float.PositiveInfinity : l);
                maxWidth = MathF.Max(MathF.Min(width, maxWidth), minWidth);

                width = (float.IsNaN(l) ? 0 : l);
                minWidth = MathF.Max(MathF.Min(maxWidth, width), minWidth);
            }

            internal float minWidth;
            internal float maxWidth;
            internal float minHeight;
            internal float maxHeight;
        }

        #endregion

        #region PropertyChanged

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Metadata is FrameworkPropertyMetadata metadata)
            {
                if (metadata.Flags.HasFlag(FrameworkPropertyMetadataOptions.AffectsMeasure))
                    InvalidateMeasure();
                if (metadata.Flags.HasFlag(FrameworkPropertyMetadataOptions.AffectsArrange))
                    InvalidateArrange();
                if (metadata.Flags.HasFlag(FrameworkPropertyMetadataOptions.AffectsRender))
                    InvalidateVisual();
                var affectParentMeasure = metadata.Flags.HasFlag(FrameworkPropertyMetadataOptions.AffectsParentMeasure);
                var affectParentArrange = metadata.Flags.HasFlag(FrameworkPropertyMetadataOptions.AffectsParentArrange);
                if (affectParentArrange || affectParentMeasure)
                {
                    for (Visual? v = VisualTreeHelper.GetParent(this);
                         v != null;
                         v = VisualTreeHelper.GetParent(v))
                    {
                        if (v is UIElement element)
                        {
                            if (affectParentMeasure)
                                element.InvalidateMeasure();
                            if (affectParentArrange)
                                element.InvalidateArrange();
                        }
                    }
                }
            }
        }

        #endregion

        #region Template

        private bool _templateGenerated;
        private FrameworkElement? _templatedContent, _templatedParent;

        public FrameworkElement? TemplatedParent => _templatedParent;

        public FrameworkElement? TemplatedChild => _templatedContent;

        public virtual bool ApplyTemplate()
        {
            OnPreApplyTemplate();

            var result = false;

            if (!_templateGenerated)
            {
                if (_templatedContent != null)
                {
                    _templatedContent._templatedParent = null;
                    RemoveVisualChild(_templatedContent);
                }
                _templatedContent = LoadTemplate();
                if (_templatedContent != null)
                {
                    _templatedContent._templatedParent = this;
                    AddVisualChild(_templatedContent);
                }
                OnApplyTemplate();
            }

            OnPostApplyTemplate();

            return result;
        }

        protected virtual FrameworkElement? LoadTemplate() { return null; }

        protected virtual void OnPreApplyTemplate() { }

        protected virtual void OnApplyTemplate() { }

        protected virtual void OnPostApplyTemplate() { }

        protected DependencyObject? GetTemplateChild(string childName)
        {
            if (_templatedContent == null)
                return null;
            return _templatedContent.FindName(childName) as DependencyObject;
        }

        protected void OnTemplateChanged()
        {
            _templateGenerated = false;
        }

        #endregion

        #region NameScope

        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register(
                        "Name",
                        typeof(string),
                        typeof(FrameworkElement),
                        new FrameworkPropertyMetadata(
                            string.Empty,                           // defaultValue
                            FrameworkPropertyMetadataOptions.None,  // flags
                            null,                                   // propertyChangedCallback
                            null,                                   // coerceValueCallback
                            true),                                  // isAnimationProhibited
                        new ValidateValueCallback(NameValidationCallback));
        private static bool NameValidationCallback(object? candidateName)
        {
            string? name = candidateName as string;
            if (name != null)
            {
                // Non-null string, ask the XAML validation code for blessing.
                return NameScope.IsValidIdentifierName(name);
            }
            else if (candidateName == null)
            {
                // Null string is allowed
                return true;
            }
            else
            {
                // candiateName is not a string object.
                return false;
            }
        }
        public string? Name { get { return (string?)GetValue(NameProperty); } set { SetValue(NameProperty, value); } }

        /// <summary>
        /// Registers the name - element combination from the
        /// NameScope that the current element belongs to.
        /// </summary>
        /// <param name="name">Name of the element</param>
        /// <param name="scopedElement">Element where name is defined</param>
        public void RegisterName(string name, object scopedElement)
        {
            INameScope? nameScope = FindScope();
            if (nameScope == null)
                throw new InvalidOperationException("NameScope not found.");
            nameScope.RegisterName(name, scopedElement);
        }

        /// <summary>
        /// Unregisters the name - element combination from the
        /// NameScope that the current element belongs to.
        /// </summary>
        /// <param name="name">Name of the element</param>
        public void UnregisterName(string name)
        {
            INameScope? nameScope = FindScope();
            if (nameScope == null)
                throw new InvalidOperationException("NameScope not found.");
            nameScope.UnregisterName(name);
        }

        /// <summary>
        /// Find the object with given name in the
        /// NameScope that the current element belongs to.
        /// </summary>
        /// <param name="name">string name to index</param>
        /// <returns>context if found, else null</returns>
        public object? FindName(string name)
        {
            return FindScope()?.FindName(name);
        }

        private INameScope? FindScope()
        {
            LogicalObject? element = this;
            while (element != null)
            {
                INameScope? nameScope = NameScope.NameScopeFromObject(element);
                if (nameScope != null)
                    return nameScope;
                element = LogicalTreeHelper.GetParent(element);
            }
            return null;
        }

        #endregion

        #region DependencyValue

        public BindingExpressionBase? GetBindingExpression(DependencyProperty dp)
        {
            return GetExpression(dp) as BindingExpressionBase;
        }

        public BindingExpressionBase SetBinding(DependencyProperty dp, BindingBase binding)
        {
            if (dp == null)
                throw new ArgumentNullException(nameof(dp));
            if (binding == null)
                throw new ArgumentNullException(nameof(binding));
            var expression = binding.CreateBindingExpression(this, dp);
            SetValueCore(dp, expression);
            return expression;
        }

        public void UpdateBinding()
        {
            UpdateBinding(this);
        }

        private void UpdateBinding(LogicalObject obj)
        {
            var list = (List<BindingExpressionBase>?)obj.GetValue(BindingExpressionBase.BindingRetryProperty);
            if (list != null)
            {
                foreach (var binding in list.ToArray())
                {
                    binding.RetryAttach();
                }
            }
            var children = obj.LogicalChildren;
            if (children != null)
            {
                foreach (var child in children)
                    UpdateBinding(child);
            }
        }

        //protected override void OnLogicalRootChanged(LogicObject oldRoot, LogicObject newRoot)
        //{
        //    base.OnLogicalRootChanged(oldRoot, newRoot);
        //}

        protected sealed override void EvaluateBaseValue(DependencyProperty dp, PropertyMetadata metadata, ref DependencyEffectiveValue effectiveValue)
        {
            if (effectiveValue.Source == DependencyEffectiveSource.Local || effectiveValue.Source == DependencyEffectiveSource.Expression)
                return;
            if (dp == StyleProperty)
            {
                InitializeStyle(ref effectiveValue);
                return;
            }
            else
            {
                if (_style != null && _style.TryApplyProperty(dp, ref effectiveValue))
                    return;
            }
        }

        #endregion

        #region Data

        public static readonly DependencyProperty DataContextProperty =
            DependencyProperty.Register(
                        "DataContext",
                        typeof(object),
                        typeof(FrameworkElement),
                        new FrameworkPropertyMetadata(null,
                                FrameworkPropertyMetadataOptions.Inherits,
                                new PropertyChangedCallback(OnDataContextChanged)));
        private static void OnDataContextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //if (e.NewValue == BindingExpressionBase.DisconnectedItem)
            //    return;

            //((FrameworkElement)d).RaiseDependencyPropertyChanged(DataContextChangedKey, e);
        }
        public object? DataContext { get { return GetValue(DataContextProperty); } set { SetValue(DataContextProperty, value); } }

        private ResourceDictionary? _resources;
        public ResourceDictionary? Resources
        {
            get
            {
                if (_resources == null)
                    _resources = new ResourceDictionary();
                return _resources;
            }
            set
            {
                _resources = value;
            }
        }

        #endregion

        #region Style

        public static readonly DependencyProperty StyleProperty =
                DependencyProperty.Register(
                        "Style",
                        typeof(Style),
                        typeof(FrameworkElement),
                        new FrameworkPropertyMetadata(
                                null,   // default value
                                FrameworkPropertyMetadataOptions.AffectsMeasure,
                                new PropertyChangedCallback(OnStyleChanged)));
        private static void OnStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement fe = (FrameworkElement)d;
            fe._style = (Style?)e.NewValue;
            if (fe._style != null)
                fe._style.Seal();
            Style.ApplyStyle(fe, (Style?)e.OldValue, fe._style);
        }
        private Style? _style;
        public Style? Style { get { return _style; } set { SetValue(StyleProperty, value); } }

        private void InitializeStyle(ref DependencyEffectiveValue effectiveValue)
        {
            var style = ResourceHelper.FindResource(this, GetType()) as Style;
            if (style != null)
                effectiveValue = new DependencyEffectiveValue(style, DependencyEffectiveSource.Internal);
        }

        #endregion
    }
}
