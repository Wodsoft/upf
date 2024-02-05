using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using Wodsoft.UI.Data;

namespace Wodsoft.UI
{
    [ContentProperty("Setters")]
    public sealed class MultiTrigger : TriggerBase
    {
        #region Properties

        private ConditionCollection _conditions = new ConditionCollection();
        private SetterBaseCollection? _setters;
        private TriggerActionCollection? _enterActions, _exitActions;

        public ConditionCollection Conditions => _conditions;

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
            if (triggerBase is MultiTrigger trigger)
            {
                if (_conditions.Count != trigger.Conditions.Count)
                    return false;
                for (int i = 0; i < _conditions.Count; i++)
                {
                    var left = _conditions[i];
                    var right = trigger.Conditions[i];
                    if (left.Property != right.Property || left.SourceName != right.SourceName || left.Value != right.Value)
                        return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Merge this trigger.
        /// </summary>
        /// <param name="triggerBase">Merge into trigger.</param>
        /// <returns></returns>
        protected internal override TriggerBase Merge(TriggerBase triggerBase)
        {
            MultiTrigger oldTrigger = (MultiTrigger)triggerBase;
            MultiTrigger newTrigger = new MultiTrigger();
            newTrigger._conditions = _conditions;
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

        #region Seal

        protected override void OnSeal()
        {
            _conditions.Seal();
            _setters?.Seal();
        }

        #endregion


        #region Connect

        protected internal override void ConnectTrigger(object source, FrameworkElement container, INameScope? nameScope)
        {
            if (_conditions.Count == 0)
                return;
            TriggerBinding binding = new TriggerBinding(source, container, nameScope, _enterActions, _exitActions);
            foreach (var condition in _conditions)
            {
                if (condition.SourceName == null)
                {
                    binding.AddCondition(new DependencyConditionBinding(container, condition.Property!, condition.Value, condition.Logic));
                }
                else
                {
                    if (nameScope == null)
                        throw new NotSupportedException("There is no NameScope provide.");
                    var target = nameScope!.FindName(condition.SourceName);
                    if (target is DependencyObject d)
                    {
                        binding.AddCondition(new DependencyConditionBinding(d, condition.Property!, condition.Value, condition.Logic));
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            if (!binding.HasCondition)
            {
                binding.Dispose();
                return;
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
