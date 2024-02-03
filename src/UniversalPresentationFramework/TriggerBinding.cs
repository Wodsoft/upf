using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    internal class TriggerBinding : IDisposable
    {
        private readonly DependencyObject _container;
        private readonly TriggerActionCollection? _enterActions, _exitActions;
        private readonly List<(FrameworkElement, DependencyProperty, TriggerValue, byte)> _setters = new List<(FrameworkElement, DependencyProperty, TriggerValue, byte)>();
        private readonly List<ConditionBinding> _conditions = new List<ConditionBinding>();
        private bool _disposed, _isEquality;

        public TriggerBinding(DependencyObject container, TriggerActionCollection? enterActions, TriggerActionCollection? exitActions)
        {
            _container = container;
            _enterActions = enterActions;
            _exitActions = exitActions;
        }

        public bool HasSetter => _setters.Count != 0;

        public bool HasCondition => _conditions.Count != 0;

        public void AddSetter(FrameworkElement target, DependencyProperty property, TriggerValue value, byte layer)
        {            
            target.AddTriggerValue(property, layer, value);
            _setters.Add((target, property, value, layer));
        }

        public void AddCondition(ConditionBinding conditionBinding)
        {
            _conditions.Add(conditionBinding);
            conditionBinding.IsEqualityChanged += ConditionBinding_IsEqualityChanged;
        }

        public void EnsureState()
        {
            foreach (var condition in _conditions)
                condition.EnsureEquailty();
        }

        private void ConditionBinding_IsEqualityChanged(ConditionBinding conditionBinding, bool isEquality)
        {
            if (_isEquality && !isEquality)
            {
                _isEquality = false;
                foreach (var (target, property, value, _) in _setters)
                {
                    value.IsEnabled = false;
                    target.InvalidateProperty(property);
                }
                if (_exitActions != null)
                {
                    foreach (var action in _exitActions)
                    {
                        action.Invoke(_container);
                    }
                }
            }
            else if (!_isEquality && isEquality)
            {
                _isEquality = _conditions.All(t => t.IsEquality);
                if (_isEquality)
                {
                    foreach (var (target, property, value, _) in _setters)
                    {
                        value.IsEnabled = true;
                        target.InvalidateProperty(property);
                    }
                }
                if (_enterActions != null)
                {
                    foreach (var action in _enterActions)
                    {
                        action.Invoke(_container);
                    }
                }
            }
        }

        public void Dispose()
        {
            if (_disposed)
                return;
            _disposed = true;
            foreach (var condition in _conditions)
            {
                condition.IsEqualityChanged -= ConditionBinding_IsEqualityChanged;
                condition.Dispose();
            }
            _conditions.Clear();
            foreach (var (target, property, value, layer) in _setters)
            {
                target.RemoveTriggerValue(property, layer, value);
                if (value.IsEnabled)
                    target.InvalidateProperty(property);
            }
            _setters.Clear();
        }
    }
}
