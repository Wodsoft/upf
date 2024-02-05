using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using Wodsoft.UI.Controls;
using Wodsoft.UI.Data;

namespace Wodsoft.UI
{
    [ContentProperty("Actions")]
    public class EventTrigger : TriggerBase
    {
        #region Constructor

        public EventTrigger()
        {
        }

        public EventTrigger(RoutedEvent routedEvent)
        {
            _routedEvent = routedEvent;
        }

        #endregion

        #region Properties

        private RoutedEvent? _routedEvent;
        public RoutedEvent? RoutedEvent
        {
            get
            {
                return _routedEvent;
            }
            set
            {
                CheckSealed();
                if (value == null)
                    throw new ArgumentNullException("value");
                _routedEvent = value;
            }
        }

        private string? _sourceName;
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

        private TriggerActionCollection? _actions;
        public TriggerActionCollection Actions
        {
            get
            {
                if (_actions == null)
                {
                    if (IsSealed)
                        return TriggerActionCollection.ReadOnly;
                    _actions = new TriggerActionCollection(this);
                }
                return _actions;
            }
        }

        #endregion

        #region Merge

        protected internal override bool CanMerge(TriggerBase triggerBase)
        {
            if (triggerBase is EventTrigger trigger)
                return trigger._routedEvent == _routedEvent && trigger.SourceName == _sourceName;
            return false;
        }

        /// <summary>
        /// Merge this trigger.
        /// </summary>
        /// <param name="triggerBase">Merge into trigger.</param>
        /// <returns></returns>
        protected internal override TriggerBase Merge(TriggerBase triggerBase)
        {
            EventTrigger oldTrigger = (EventTrigger)triggerBase;
            EventTrigger newTrigger = new EventTrigger();
            newTrigger._routedEvent = _routedEvent;
            newTrigger._sourceName = _sourceName;
            if (_actions == null || _actions.Count == 0)
                newTrigger._actions = oldTrigger._actions;
            else if (oldTrigger._actions == null || oldTrigger._actions.Count == 0)
                newTrigger._actions = _actions;
            else
            {
                var actions = new List<TriggerAction>(_actions.Count + oldTrigger._actions.Count);
                actions.AddRange(_actions);
                actions.AddRange(oldTrigger._actions);
                newTrigger._actions = new TriggerActionCollection(newTrigger, actions);
            }
            return newTrigger;
        }

        #endregion

        #region Seal

        protected override void OnSeal()
        {
            if (_actions != null)
                _actions.Seal();
        }

        #endregion

        #region Connect

        private readonly Dictionary<FrameworkElement, (object Source, INameScope? NameScope)> _scopes = new Dictionary<FrameworkElement, (object, INameScope?)>();

        protected internal override void ConnectTrigger(object source, FrameworkElement container, INameScope? nameScope)
        {
            if (_routedEvent == null)
                return;
            if (_actions == null || _actions.Count == 0)
                return;
            container.AddHandler(_routedEvent, Handle, false);
            _scopes[container] = (source, nameScope);
        }

        protected internal override void DisconnectTrigger(object source, FrameworkElement container, INameScope? nameScope)
        {
            if (_routedEvent == null)
                return;
            if (_actions == null || _actions.Count == 0)
                return;
            container.RemoveHandler(_routedEvent, Handle);
            _scopes.Remove(container);
        }

        #endregion

        #region Handler

        private void Handle(object sender, RoutedEventArgs e)
        {
            _scopes.TryGetValue((FrameworkElement)sender, out var value);
            foreach (var action in _actions!)
            {
                action.Invoke(value.Source, (DependencyObject)sender, value.NameScope);
            }
        }

        #endregion
    }
}
