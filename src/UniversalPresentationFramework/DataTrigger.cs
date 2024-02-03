using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using Wodsoft.UI.Data;

namespace Wodsoft.UI
{
    [ContentProperty("Setters")]
    [XamlSetMarkupExtension("ReceiveMarkupExtension")]
    public sealed class DataTrigger : TriggerBase
    {
        #region Properties

        private BindingBase? _binding;
        private object? _value;
        private SetterBaseCollection? _setters;
        private TriggerActionCollection? _enterActions, _exitActions;

        public BindingBase? Binding
        {
            get => _binding;
            set
            {
                CheckSealed();                
                _binding = value;
            }
        }

        [DependsOn("Binding")]
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
            if (triggerBase is DataTrigger trigger)
                return _binding!.IsEqual(trigger.Binding!) && trigger.Value == _value;
            return false;
        }

        /// <summary>
        /// Merge this trigger.
        /// </summary>
        /// <param name="triggerBase">Merge into trigger.</param>
        /// <returns></returns>
        protected internal override TriggerBase Merge(TriggerBase triggerBase)
        {
            DataTrigger oldTrigger = (DataTrigger)triggerBase;
            DataTrigger newTrigger = new DataTrigger();
            newTrigger._binding = _binding;
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

        public static void ReceiveMarkupExtension(object targetObject, XamlSetMarkupExtensionEventArgs eventArgs)
        {
            if (targetObject == null)
            {
                throw new ArgumentNullException("targetObject");
            }
            if (eventArgs == null)
            {
                throw new ArgumentNullException("eventArgs");
            }

            DataTrigger? trigger = targetObject as DataTrigger;
            if (trigger != null && eventArgs.Member.Name == "Binding" && eventArgs.MarkupExtension is BindingBase)
            {
                trigger.Binding = eventArgs.MarkupExtension as BindingBase;
                eventArgs.Handled = true;
            }
            else
            {
                eventArgs.CallBase();
            }
        }

        #endregion

        #region Seal

        protected override void OnSeal()
        {
            if (_binding == null)
                throw new InvalidOperationException("Binding can't be null.");

            // Freeze the condition for the trigger
            if (_value is ISealable sealable)
                sealable.Seal();

            _setters?.Seal();
        }

        #endregion

        #region Connect

        protected internal override void ConnectTrigger(object? source, FrameworkElement container, INameScope? nameScope)
        {
            if (_binding == null)
                return;
            if (_setters == null || _setters.Count == 0)
                return;
            var bindingExpression = _binding!.CreateBindingExpression(container, BindingExpressionBase.NoTargetProperty);
            if (!bindingExpression.CanUpdateTarget)
                return;
            byte layer;
            if (source is FrameworkTemplate)
                layer = TriggerLayer.ControlTemplate;
            else if (source is Style)
                layer = TriggerLayer.Style;
            else
                layer = TriggerLayer.ThemeStyle;
            TriggerBinding binding = new TriggerBinding(container, _enterActions, _exitActions);
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
            if (!binding.HasSetter)
                return;
            bindingExpression.Attach(container, BindingExpressionBase.NoTargetProperty);
            binding.AddCondition(new ExpressionConditionBinding(bindingExpression, _value));
            binding.EnsureState();
            container.AddTriggerBinding(this, binding);
        }

        protected internal override void DisconnectTrigger(object source, FrameworkElement container, INameScope? nameScope)
        {
            container.RemoveTriggerBinding(this);
        }

        #endregion
    }
}
