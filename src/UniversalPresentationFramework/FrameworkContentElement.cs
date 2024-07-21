using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using Wodsoft.UI.Data;
using Wodsoft.UI.Media;
using Wodsoft.UI.Threading;

namespace Wodsoft.UI
{
    public class FrameworkContentElement : ContentElement, IHaveTriggerValue
    {
        #region NameScope

        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register(
                        "Name",
                        typeof(string),
                        typeof(FrameworkContentElement),
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

        #region Resource

        private ResourceDictionary? _resources;
        [Ambient]
        public ResourceDictionary Resources
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

        #region Dispatcher

        public override Dispatcher Dispatcher
        {
            get
            {
                if (LogicalRoot == this)
                    return base.Dispatcher;
                return LogicalRoot.Dispatcher;
            }
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
            if (obj is not FrameworkContentElement fce || fce._needRetryBind)
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

        #region Style

        public static readonly DependencyProperty StyleProperty =
                DependencyProperty.Register(
                        "Style",
                        typeof(Style),
                        typeof(FrameworkContentElement),
                        new FrameworkPropertyMetadata(
                                null,   // default value
                                FrameworkPropertyMetadataOptions.AffectsMeasure,
                                new PropertyChangedCallback(OnStyleChanged)));
        private static void OnStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkContentElement fce = (FrameworkContentElement)d;
            fce._style = (Style?)e.NewValue;
            if (fce._style != null)
                fce._style.Seal();
            Style.ApplyStyle(fce, (Style?)e.OldValue, fce._style);
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
            = DependencyProperty.Register("DefaultStyleKey", typeof(object), typeof(FrameworkContentElement),
                                            new FrameworkPropertyMetadata(
                                                        null,
                                                        FrameworkPropertyMetadataOptions.AffectsMeasure,
                                                        new PropertyChangedCallback(OnThemeStyleKeyChanged)));
        private static void OnThemeStyleKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Re-evaluate ThemeStyle because it is
            // a factor of the ThemeStyleKey property
            ((FrameworkContentElement)d).UpdateThemeStyleProperty();
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
            = DependencyProperty.Register("OverridesDefaultStyle", typeof(bool), typeof(FrameworkContentElement),
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

        void IHaveTriggerValue.AddTriggerBinding(TriggerBase trigger, TriggerBinding triggerBinding)
        {
            _triggerBindings.Add(trigger, triggerBinding);
        }

        void IHaveTriggerValue.RemoveTriggerBinding(TriggerBase trigger)
        {
            if (_triggerBindings.TryGetValue(trigger, out var binding))
            {
                _triggerBindings.Remove(trigger);
                binding.Dispose();
            }
        }

        void IHaveTriggerValue.AddTriggerValue(DependencyProperty dp, byte layer, TriggerValue triggerValue)
        {
            if (!_triggerStorages.TryGetValue(dp.GlobalIndex, out var storage))
            {
                storage = new TriggerStorage();
                _triggerStorages.Add(dp.GlobalIndex, storage);
            }
            storage.AddValue(layer, triggerValue);
        }

        void IHaveTriggerValue.RemoveTriggerValue(DependencyProperty dp, byte layer, TriggerValue triggerValue)
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
    }
}
