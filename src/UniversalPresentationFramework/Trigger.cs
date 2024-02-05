using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using Wodsoft.UI.Controls;

namespace Wodsoft.UI
{
    [ContentProperty("Setters")]
    [XamlSetTypeConverterAttribute("ReceiveTypeConverter")]
    public sealed class Trigger : TriggerBase, ISupportInitialize
    {
        #region Properties

        private DependencyProperty? _property;
        private object? _value;
        private string? _sourceName;
        private ConditionLogic _logic;
        private SetterBaseCollection? _setters;
        private TriggerActionCollection? _enterActions, _exitActions;

        [Ambient]
        public DependencyProperty? Property
        {
            get
            {
                return _property;
            }
            set
            {
                CheckSealed();
                _property = value;
            }
        }

        [DependsOn("Property")]
        [DependsOn("SourceName")]
        [TypeConverter(typeof(SetterTriggerConditionValueConverter))]
        public object? Value
        {
            get
            {
                return _value;
            }
            set
            {
                CheckSealed();
                if (value is NullExtension)
                    value = null;
                if (value is MarkupExtension)
                    throw new ArgumentException("Trigger condition value doesn't support markup extension.");
                if (value is Expression)
                    throw new ArgumentException("Trigger condition value doesn't support expression.");
                _value = value;
            }
        }

        [Ambient]
        public string? SourceName
        {
            get
            {
                return _sourceName;
            }
            set
            {
                CheckSealed();
                _sourceName = value;
            }
        }

        public ConditionLogic Logic
        {
            get { return _logic; }
            set
            {
                CheckSealed();
                _logic = value;
            }
        }

        public SetterBaseCollection Setters
        {
            get
            {
                if (_setters == null)
                    _setters = new SetterBaseCollection();
                return _setters;
            }
        }

        public TriggerActionCollection EnterActions
        {
            get
            {
                if (_enterActions == null)
                {
                    if (IsSealed)
                        return TriggerActionCollection.ReadOnly;
                    _enterActions = new TriggerActionCollection(this);
                }
                return _enterActions;
            }
        }

        public TriggerActionCollection ExitActions
        {
            get
            {
                if (_exitActions == null)
                {
                    if (IsSealed)
                        return TriggerActionCollection.ReadOnly;
                    _exitActions = new TriggerActionCollection(this);
                }
                return _exitActions;
            }
        }

        #endregion

        #region Merge

        protected internal override bool CanMerge(TriggerBase triggerBase)
        {
            if (triggerBase is Trigger trigger)
                return trigger.Property == _property && trigger.SourceName == _sourceName && trigger.Value == _value;
            return false;
        }

        /// <summary>
        /// Merge this trigger.
        /// </summary>
        /// <param name="triggerBase">Merge into trigger.</param>
        /// <returns></returns>
        protected internal override TriggerBase Merge(TriggerBase triggerBase)
        {
            Trigger oldTrigger = (Trigger)triggerBase;
            Trigger newTrigger = new Trigger();
            newTrigger._property = _property;
            newTrigger._sourceName = _sourceName;
            newTrigger._value = _value;
            if (_setters == null || _setters.Count == 0)
                newTrigger._setters = oldTrigger._setters;
            else if (oldTrigger._setters == null || oldTrigger._setters.Count == 0)
                newTrigger._setters = _setters;
            else
            {
                var setters = new List<SetterBase>(_setters.Count + oldTrigger._setters.Count);
                setters.AddRange(_setters);
                setters.AddRange(oldTrigger._setters);
                newTrigger._setters = new SetterBaseCollection(setters);
            }
            if ((_enterActions == null || _enterActions.Count == 0) && oldTrigger._enterActions != null)
                newTrigger._enterActions = new TriggerActionCollection(this, new List<TriggerAction>(oldTrigger._enterActions));
            else if ((oldTrigger._enterActions == null || oldTrigger._enterActions.Count == 0) && _enterActions != null)
                newTrigger._enterActions = new TriggerActionCollection(this, new List<TriggerAction>(_enterActions));
            else if (oldTrigger._enterActions != null && _enterActions != null)
            {
                var actions = new List<TriggerAction>();
                actions.AddRange(_enterActions);
                actions.AddRange(oldTrigger._enterActions);
                newTrigger._enterActions = new TriggerActionCollection(this, actions);
            }
            if ((_exitActions == null || _exitActions.Count == 0) && oldTrigger._exitActions != null)
                newTrigger._exitActions = new TriggerActionCollection(this, new List<TriggerAction>(oldTrigger._exitActions));
            else if ((oldTrigger._exitActions == null || oldTrigger._exitActions.Count == 0) && _exitActions != null)
                newTrigger._exitActions = new TriggerActionCollection(this, new List<TriggerAction>(_exitActions));
            else if (oldTrigger._exitActions != null && _exitActions != null)
            {
                var actions = new List<TriggerAction>();
                actions.AddRange(_exitActions);
                actions.AddRange(oldTrigger._exitActions);
                newTrigger._exitActions = new TriggerActionCollection(this, actions);
            }
            return newTrigger;
        }

        #endregion

        #region XAML

        private ITypeDescriptorContext? _serviceProvider;
        private object? _unresolvedProperty, _unresolvedValue;
        private CultureInfo? _cultureInfoForTypeConverter;

        public static void ReceiveTypeConverter(object targetObject, XamlSetTypeConverterEventArgs eventArgs)
        {
            Trigger? trigger = targetObject as Trigger;
            if (trigger == null)
                throw new ArgumentNullException("targetObject");
            if (eventArgs == null)
                throw new ArgumentNullException("eventArgs");
            if (eventArgs.Member.Name == "Property")
            {
                trigger._unresolvedProperty = eventArgs.Value;
                trigger._serviceProvider = eventArgs.ServiceProvider;
                trigger._cultureInfoForTypeConverter = eventArgs.CultureInfo;

                eventArgs.Handled = true;
            }
            else if (eventArgs.Member.Name == "Value")
            {
                trigger._unresolvedValue = eventArgs.Value;
                trigger._serviceProvider = eventArgs.ServiceProvider;
                trigger._cultureInfoForTypeConverter = eventArgs.CultureInfo;

                eventArgs.Handled = true;
            }
        }

        #endregion

        #region ISupportInitialize

        void ISupportInitialize.BeginInit()
        {
        }

        void ISupportInitialize.EndInit()
        {
            // Resolve all properties here
            if (_unresolvedProperty != null)
            {
                try
                {
                    Property = DependencyPropertyHelper.ResolveProperty(_serviceProvider!,
                        SourceName, _unresolvedProperty);
                }
                finally
                {
                    _unresolvedProperty = null;
                }
            }
            if (_unresolvedValue != null)
            {
                try
                {
                    Value = SetterTriggerConditionValueConverter.ResolveValue(_serviceProvider,
                        Property, _cultureInfoForTypeConverter, _unresolvedValue);
                }
                finally
                {
                    _unresolvedValue = null;
                }
            }
            _serviceProvider = null;
            _cultureInfoForTypeConverter = null;
        }

        #endregion

        #region Seal

        protected override void OnSeal()
        {
            if (_property != null)
            {
                // Ensure valid condition
                if (!_property.IsValidValue(_value))
                    throw new InvalidOperationException("Value is invalid to property.");
            }

            // Freeze the condition for the trigger
            if (_value is ISealable sealable)
                sealable.Seal();

            _setters?.Seal();
        }

        #endregion

        #region Connect

        protected internal override void ConnectTrigger(object source, FrameworkElement container, INameScope? nameScope)
        {
            TriggerBinding binding = new TriggerBinding(source, container, nameScope, _enterActions, _exitActions);
            if (_sourceName == null)
            {
                binding.AddCondition(new DependencyConditionBinding(container, _property!, _value, _logic));
            }
            else
            {
                if (nameScope == null)
                    throw new NotSupportedException("There is no NameScope provide.");
                var target = nameScope!.FindName(_sourceName);
                if (target is DependencyObject d)
                {
                    binding.AddCondition(new DependencyConditionBinding(d, _property!, _value, _logic));
                }
                else
                {
                    return;
                }
            }
            byte layer;
            if (source is FrameworkTemplate)
                layer = TriggerLayer.ControlTemplate;
            else if (source is Style)
                layer = TriggerLayer.Style;
            else
                layer = TriggerLayer.ThemeStyle;
            if (_setters != null)
            {
                foreach (var setterBase in _setters)
                {
                    if (setterBase is Setter setter)
                    {
                        FrameworkElement? target;
                        if (setter.TargetName == null)
                            target = container;
                        else
                        {
                            target = nameScope!.FindName(setter.TargetName) as FrameworkElement;
                            if (target == null)
                                continue;
                        }
                        if (setter.Property == null)
                            continue;
                        TriggerValue triggerValue = new TriggerValue(setter.Value);
                        binding.AddSetter(target, setter.Property, triggerValue, layer == TriggerLayer.ControlTemplate && setter.TargetName != null ? TriggerLayer.ParentTemplate : layer);
                    }
                }
            }
            if (!binding.HasContent)
            {
                binding.Dispose();
                return;
            }
            container.AddTriggerBinding(this, binding);
            binding.EnsureState();
        }

        protected internal override void DisconnectTrigger(object source, FrameworkElement container, INameScope? nameScope)
        {
            container.RemoveTriggerBinding(this);
        }

        #endregion
    }
}
