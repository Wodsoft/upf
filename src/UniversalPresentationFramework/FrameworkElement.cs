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
using System.Xml.Linq;
using Wodsoft.UI.Controls;
using Wodsoft.UI.Data;
using Wodsoft.UI.Input;
using Wodsoft.UI.Media;
using Wodsoft.UI.Threading;

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
            Initialize();
            EndInit();
            IsInitPending = false;
        }

        protected virtual void BeginInit()
        {
        }

        protected virtual void EndInit()
        {
        }

        private void Initialize()
        {
            if (!_isThemeStyleLoaded)
                UpdateThemeStyleProperty();
            if (_style == null)
                InvalidateProperty(StyleProperty);
        }

        #endregion

        #region Logical

        public LogicalObject? Parent => LogicalParent;

        protected override void OnLogicalRootChanged(LogicalObject oldRoot, LogicalObject newRoot)
        {
            base.OnLogicalRootChanged(oldRoot, newRoot);
            _hasRetryBind = false;
            Initialize();
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
            if (_needRetryBind && !_hasRetryBind)
            {
                UpdateBindingSelf();
                _hasRetryBind = true;
            }

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

            bool needClip = false;

            if (FloatUtil.LessThan(arrangeSize.Width, unclippedDesiredSize.Width))
            {
                needClip = true;
                arrangeSize.Width = unclippedDesiredSize.Width;
            }
            if (FloatUtil.LessThan(arrangeSize.Height, unclippedDesiredSize.Height))
            {
                needClip = true;
                arrangeSize.Height = unclippedDesiredSize.Height;
            }

            if (HorizontalAlignment != HorizontalAlignment.Stretch)
            {
                arrangeSize.Width = unclippedDesiredSize.Width;
            }
            if (VerticalAlignment != VerticalAlignment.Stretch)
            {
                arrangeSize.Height = unclippedDesiredSize.Height;
            }

            //if (HorizontalAlignment == HorizontalAlignment.Stretch)
            //    arrangeSize.Width = MathF.Max(arrangeSize.Width, unclippedDesiredSize.Width);
            //else
            //    arrangeSize.Width = unclippedDesiredSize.Width;
            //if (VerticalAlignment == VerticalAlignment.Stretch)
            //    arrangeSize.Height = MathF.Max(arrangeSize.Height, unclippedDesiredSize.Height);
            //else
            //    arrangeSize.Height = unclippedDesiredSize.Height;

            MinMax mm = new MinMax(this);
            float effectiveMaxWidth = Math.Max(unclippedDesiredSize.Width, mm.maxWidth);
            if (FloatUtil.LessThan(effectiveMaxWidth, arrangeSize.Width))
            {
                needClip = true;
                arrangeSize.Width = effectiveMaxWidth;
            }
            float effectiveMaxHeight = Math.Max(unclippedDesiredSize.Height, mm.maxHeight);
            if (FloatUtil.LessThan(effectiveMaxHeight, arrangeSize.Height))
            {
                needClip = true;
                arrangeSize.Height = effectiveMaxHeight;
            }
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



        public static readonly DependencyProperty UseLayoutRoundingProperty =
                DependencyProperty.Register(
                        "UseLayoutRounding",
                        typeof(bool),
                        typeof(FrameworkElement),
                        new FrameworkPropertyMetadata(
                            false,
                            FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public bool UseLayoutRounding
        {
            get { return (bool)GetValue(UseLayoutRoundingProperty)!; }
            set { SetValue(UseLayoutRoundingProperty, value); }
        }

        #endregion

        #region PropertyChanged

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == BindingExpression.BindingRetryProperty)
            {
                _needRetryBind = e.NewValue != null;
            }
            if (e.Metadata is FrameworkPropertyMetadata metadata)
            {
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
                if (metadata.Flags.HasFlag(FrameworkPropertyMetadataOptions.AffectsMeasure))
                    InvalidateMeasure();
                if (metadata.Flags.HasFlag(FrameworkPropertyMetadataOptions.AffectsArrange))
                    InvalidateArrange();
                if (metadata.Flags.HasFlag(FrameworkPropertyMetadataOptions.AffectsRender))
                    InvalidateVisual();
            }
        }

        #endregion

        #region Template

        private bool _templateGenerated;
        private FrameworkTemplate? _lastTemplate;
        private TriggerCollection? _appliedTemplateTriggers;
        private FrameworkElement? _templatedContent, _templatedParent;

        public FrameworkElement? TemplatedParent => _templatedParent;

        public FrameworkElement? TemplatedChild => _templatedContent;

        public event EventHandler? TemplatedParentChanged;

        internal FrameworkTemplate? LastTemplate => _lastTemplate;

        public virtual bool ApplyTemplate()
        {
            OnPreApplyTemplate();

            var result = false;

            if (!_templateGenerated)
            {
                if (_templatedContent != null)
                {
                    if (_appliedTemplateTriggers != null)
                    {
                        foreach (var trigger in _appliedTemplateTriggers)
                        {
                            trigger.DisconnectTrigger(_lastTemplate!, this, _templatedContent.FindScope());
                        }
                    }
                    _templatedContent._templatedParent = null;
                    _templatedContent.TemplatedParentChanged?.Invoke(_templatedContent, EventArgs.Empty);
                    //RemoveLogicalChild(_templatedContent);
                    RemoveVisualChild(_templatedContent);
                }
                var template = GetTemplate();
                if (template != null)
                {
                    _templatedContent = template.LoadContent(this);
                    if (_templatedContent != null)
                    {
                        if (template.TriggersInternal != null)
                        {
                            _appliedTemplateTriggers = template.TriggersInternal;
                            foreach (var trigger in _appliedTemplateTriggers)
                            {
                                trigger.ConnectTrigger(template, this, _templatedContent.FindScope());
                            }
                        }
                        _templatedContent._templatedParent = this;
                        _templatedContent._hasRetryBind = false;
                        _templatedContent.TemplatedParentChanged?.Invoke(_templatedContent, EventArgs.Empty);
                        //AddLogicalChild(_templatedContent);
                        AddVisualChild(_templatedContent);
                        _templateGenerated = true;
                    }
                }
                _lastTemplate = template;
                OnApplyTemplate();
            }

            OnPostApplyTemplate();

            return result;
        }

        protected virtual FrameworkTemplate? GetTemplate() => null;

        internal FrameworkTemplate? GetTemplateInternal() => GetTemplate();

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

        private bool _needRetryBind, _hasRetryBind;

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
            if (obj is not FrameworkElement fe || fe._needRetryBind)
            {
                var list = (List<BindingExpressionBase>?)obj.GetValue(BindingExpressionBase.BindingRetryProperty);
                if (list != null)
                {
                    foreach (var binding in list.ToArray())
                    {
                        binding.RetryAttach();
                    }
                }
            }
            var children = LogicalTreeHelper.GetChildren(obj);
            if (children != null)
            {
                foreach (var child in children)
                    UpdateBinding(child);
            }
        }

        private void UpdateBindingSelf()
        {
            var list = (List<BindingExpressionBase>?)GetValue(BindingExpressionBase.BindingRetryProperty);
            if (list != null)
            {
                foreach (var binding in list.ToArray())
                {
                    binding.RetryAttach();
                }
            }
        }

        protected sealed override void EvaluateBaseValue(DependencyProperty dp, PropertyMetadata metadata, ref DependencyEffectiveValue effectiveValue)
        {
            TriggerModifiedValue? triggerModifiedValue = null;
            if ((effectiveValue.Source == DependencyEffectiveSource.Local || effectiveValue.Source == DependencyEffectiveSource.Expression) && effectiveValue.ModifiedValue is TriggerModifiedValue modifiedValue)
            {
                triggerModifiedValue = modifiedValue;
                effectiveValue.ModifyValue(null);
            }
            base.EvaluateBaseValue(dp, metadata, ref effectiveValue);
            TriggerStorage? triggerStorage = null;
            if ((effectiveValue.Source == DependencyEffectiveSource.Local || effectiveValue.Source == DependencyEffectiveSource.Expression))
            {
                if (!_triggerStorages.TryGetValue(dp.GlobalIndex, out triggerStorage))
                    return;
                if (triggerStorage.TryGetValue(TriggerLayer.ParentTemplate, out var value))
                {
                    if (!dp.IsValidValue(value))
                        value = metadata.DefaultValue;
                    if (triggerModifiedValue == null)
                        triggerModifiedValue = new TriggerModifiedValue(value);
                    else
                        triggerModifiedValue.Value = value;
                    effectiveValue.ModifyValue(triggerModifiedValue);
                }
                return;
            }
            if (dp == StyleProperty)
            {
                InitializeStyle(ref effectiveValue);
                return;
            }
            else
            {
                if (_style != null && _style.TryApplyProperty(this, dp, ref effectiveValue))
                    return;
                if (triggerStorage == null && _triggerStorages.TryGetValue(dp.GlobalIndex, out triggerStorage))
                {
                    object? value;
                    if (triggerStorage.TryGetValue(TriggerLayer.Style, out value))
                    { }
                    else if (triggerStorage.TryGetValue(TriggerLayer.ControlTemplate, out value))
                    { }
                    else if (triggerStorage.TryGetValue(TriggerLayer.ThemeStyle, out value))
                    { }
                    else
                        return;
                    if (!dp.IsValidValue(value))
                        value = metadata.DefaultValue;
                    effectiveValue = new DependencyEffectiveValue(value, DependencyEffectiveSource.Internal);
                }
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

        #endregion

        #region Resources

        private ResourceDictionary? _resources;
        [Ambient]
        public ResourceDictionary Resources
        {
            get
            {
                if (_resources == null)
                    _resources = new ResourceDictionary(this);
                return _resources;
            }
            set
            {
                if (_resources == value)
                    return;
                _resources?.RemoveOwner(this);
                _resources = value;
                value?.AddOwner(this);
            }
        }

        public object? FindResource(object resourceKey)
        {
            if (resourceKey == null)
                throw new ArgumentNullException("resourceKey");
            var value = ResourceHelper.FindResource(this, resourceKey);
            if (value == DependencyProperty.UnsetValue)
                throw new InvalidOperationException("Resource key not found.");
            return value;
        }

        public object? TryFindResource(object resourceKey)
        {
            if (resourceKey == null)
                throw new ArgumentNullException("resourceKey");
            var value = ResourceHelper.FindResource(this, resourceKey);
            if (value == DependencyProperty.UnsetValue)
                return null;
            return value;
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
            else if (_themeStyle != null)
                effectiveValue = new DependencyEffectiveValue(_themeStyle, DependencyEffectiveSource.Internal);
        }

        protected internal static readonly DependencyProperty DefaultStyleKeyProperty
            = DependencyProperty.Register("DefaultStyleKey", typeof(object), typeof(FrameworkElement),
                                            new FrameworkPropertyMetadata(
                                                        null,
                                                        FrameworkPropertyMetadataOptions.AffectsMeasure,
                                                        new PropertyChangedCallback(OnThemeStyleKeyChanged)));
        private static void OnThemeStyleKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Re-evaluate ThemeStyle because it is
            // a factor of the ThemeStyleKey property
            ((FrameworkElement)d).UpdateThemeStyleProperty();
        }
        protected internal object? DefaultStyleKey
        {
            get { return GetValue(DefaultStyleKeyProperty); }
            set { SetValue(DefaultStyleKeyProperty, value); }
        }

        internal Style? ThemeStyle => _themeStyle;

        private Style? _themeStyle;
        private bool _isThemeStyleUpdateInProgress, _isThemeStyleLoaded;
        internal void UpdateThemeStyleProperty()
        {
            if (_isThemeStyleUpdateInProgress == false)
            {
                _isThemeStyleLoaded = true;
                _isThemeStyleUpdateInProgress = true;
                try
                {
                    Style? oldTheme = _themeStyle, newTheme;
                    var key = DefaultStyleKey;
                    if (key == null || OverridesDefaultStyle)
                        newTheme = null;
                    else
                    {
                        if (FrameworkProvider.ResourceProvider != null)
                            newTheme = FrameworkProvider.ResourceProvider.FindSystemResource(key) as Style;
                        else
                            newTheme = null;
                    }

                    if (oldTheme != newTheme)
                    {
                        if (newTheme != null)
                        {
                            newTheme.CheckTargetType(this);
                            newTheme.Seal();
                        }
                        _themeStyle = newTheme;

                        if (_style == null)
                            Style.ApplyStyle(this, oldTheme, newTheme);
                    }

                    // Update the ContextMenu and ToolTips separately because they aren't in the tree
                    //ContextMenu contextMenu =
                    //        GetValueEntry(
                    //                LookupEntry(ContextMenuProperty.GlobalIndex),
                    //                ContextMenuProperty,
                    //                null,
                    //                RequestFlags.DeferredReferences).Value as ContextMenu;
                    //if (contextMenu != null)
                    //{
                    //    TreeWalkHelper.InvalidateOnResourcesChange(contextMenu, null, ResourcesChangeInfo.ThemeChangeInfo);
                    //}

                    //DependencyObject toolTip =
                    //        GetValueEntry(
                    //                LookupEntry(ToolTipProperty.GlobalIndex),
                    //                ToolTipProperty,
                    //                null,
                    //                RequestFlags.DeferredReferences).Value as DependencyObject;

                    //if (toolTip != null)
                    //{
                    //    FrameworkObject toolTipFO = new FrameworkObject(toolTip);
                    //    if (toolTipFO.IsValid)
                    //    {
                    //        TreeWalkHelper.InvalidateOnResourcesChange(toolTipFO.FE, toolTipFO.FCE, ResourcesChangeInfo.ThemeChangeInfo);
                    //    }
                    //}

                    //OnThemeChanged();
                }
                finally
                {
                    _isThemeStyleUpdateInProgress = false;
                }
            }
            else
            {
                throw new InvalidOperationException("Cyclic theme style reference detected.");
            }
        }


        public static readonly DependencyProperty OverridesDefaultStyleProperty
            = DependencyProperty.Register("OverridesDefaultStyle", typeof(bool), typeof(FrameworkElement),
                                            new FrameworkPropertyMetadata(
                                                        false,
                                                        FrameworkPropertyMetadataOptions.AffectsMeasure,
                                                        new PropertyChangedCallback(OnThemeStyleKeyChanged)));
        public bool OverridesDefaultStyle
        {
            get { return (bool)GetValue(OverridesDefaultStyleProperty)!; }
            set { SetValue(OverridesDefaultStyleProperty, value); }
        }

        #endregion

        #region Triggers

        private readonly Dictionary<TriggerBase, TriggerBinding> _triggerBindings = new Dictionary<TriggerBase, TriggerBinding>();
        private readonly Dictionary<int, TriggerStorage> _triggerStorages = new Dictionary<int, TriggerStorage>();

        internal void AddTriggerBinding(TriggerBase trigger, TriggerBinding triggerBinding)
        {
            _triggerBindings.Add(trigger, triggerBinding);
        }

        internal void RemoveTriggerBinding(TriggerBase trigger)
        {
            if (_triggerBindings.TryGetValue(trigger, out var binding))
            {
                _triggerBindings.Remove(trigger);
                binding.Dispose();
            }
        }

        internal void AddTriggerValue(DependencyProperty dp, byte layer, TriggerValue triggerValue)
        {
            if (!_triggerStorages.TryGetValue(dp.GlobalIndex, out var storage))
            {
                storage = new TriggerStorage();
                _triggerStorages.Add(dp.GlobalIndex, storage);
            }
            storage.AddValue(layer, triggerValue);
        }

        internal void RemoveTriggerValue(DependencyProperty dp, byte layer, TriggerValue triggerValue)
        {
            if (_triggerStorages.TryGetValue(dp.GlobalIndex, out var storage))
            {
                storage.RemoveValue(layer, triggerValue);
            }
        }

        private TriggerCollection? _triggers;
        public TriggerCollection Triggers
        {
            get
            {
                if (_triggers == null)
                {
                    _triggers = new TriggerCollection();
                }
                return _triggers;
            }
        }

        #endregion

        #region Visual

        protected internal override int VisualChildrenCount => TemplatedChild == null ? 0 : 1;

        protected internal override Visual GetVisualChild(int index)
        {
            if (TemplatedChild == null)
                throw new ArgumentOutOfRangeException("index");
            if (index != 0)
                throw new ArgumentOutOfRangeException("index");
            return TemplatedChild;
        }

        #endregion

        #region Dispatcher

        public override Dispatcher Dispatcher
        {
            get
            {
                if (LogicalRoot == this)
                {
                    if (TemplatedParent != null)
                        return TemplatedParent.Dispatcher;
                    return base.Dispatcher;
                }
                return LogicalRoot.Dispatcher;
            }
        }

        #endregion

        #region Text

        public static readonly DependencyProperty FlowDirectionProperty =
                    DependencyProperty.RegisterAttached(
                                "FlowDirection",
                                typeof(FlowDirection),
                                typeof(FrameworkElement),
                                new FrameworkPropertyMetadata(
                                            FlowDirection.LeftToRight, // default value
                                            FrameworkPropertyMetadataOptions.Inherits
                                          | FrameworkPropertyMetadataOptions.AffectsParentArrange,
                                            new PropertyChangedCallback(OnFlowDirectionChanged),
                                            new CoerceValueCallback(CoerceFlowDirectionProperty)),
                                new ValidateValueCallback(IsValidFlowDirection));
        private static object? CoerceFlowDirectionProperty(DependencyObject d, object? value)
        {
            if (d is FrameworkElement fe)
            {
                fe.InvalidateArrange();
                fe.InvalidateVisual();
                //fe.AreTransformsClean = false;
            }
            return value;
        }
        private static void OnFlowDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Check that d is a FrameworkElement since the property inherits and this can be called
            // on non-FEs.
            if (d is FrameworkElement fe)
            {
                // Cache the new value as a bit to optimize accessing the FlowDirection property's CLR accessor
                //fe.IsRightToLeft = ((FlowDirection)e.NewValue!) == FlowDirection.RightToLeft;
                //fe.AreTransformsClean = false;
            }
        }
        public FlowDirection FlowDirection
        {
            get { return IsRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight; }
            set { SetValue(FlowDirectionProperty, value); }
        }
        /// <summary>
        /// Queries the attached property FlowDirection from the given element.
        /// </summary>
        /// <seealso cref="DockPanel.DockProperty" />
        public static FlowDirection GetFlowDirection(DependencyObject element)
        {
            if (element == null) { throw new ArgumentNullException("element"); }
            return (FlowDirection)element.GetValue(FlowDirectionProperty)!;
        }
        /// <summary>
        /// Writes the attached property FlowDirection to the given element.
        /// </summary>
        /// <seealso cref="DockPanel.DockProperty" />
        public static void SetFlowDirection(DependencyObject element, FlowDirection value)
        {
            if (element == null) { throw new ArgumentNullException("element"); }
            element.SetValue(FlowDirectionProperty, value);
        }
        internal bool IsRightToLeft;
        private static bool IsValidFlowDirection(object o)
        {
            FlowDirection value = (FlowDirection)o;
            return value == FlowDirection.LeftToRight || value == FlowDirection.RightToLeft;
        }

        #endregion

        #region Mouse

        public static readonly DependencyProperty CursorProperty =
                    DependencyProperty.Register(
                                "Cursor",
                                typeof(Cursor),
                                typeof(FrameworkElement),
                                new FrameworkPropertyMetadata(
                                            null, // default value
                                            0,
                                            new PropertyChangedCallback(OnCursorChanged)));
        static private void OnCursorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement fe = ((FrameworkElement)d);
            if (fe.IsMouseOver && fe.Dispatcher is UIDispatcher dispatcher)
                dispatcher.MouseDevice.UpdateCursor();
        }
        public Cursor? Cursor
        {
            get { return (Cursor?)GetValue(CursorProperty); }
            set { SetValue(CursorProperty, value); }
        }

        protected override void OnQueryCursor(QueryCursorEventArgs e)
        {
            var cursor = Cursor;
            if (cursor != null)
            {
                e.Cursor = cursor;
                e.Handled = true;
            }
        }

        #endregion
    }
}
