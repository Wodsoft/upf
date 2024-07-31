using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;

namespace Wodsoft.UI
{
    internal class VisualStateTrigger : TriggerBase
    {
        private readonly Setter _setter;
        private readonly TriggerValue _value;

        public VisualStateTrigger(Setter setter)
        {
            _setter = setter;
            _value = new TriggerValue(_setter.Value) { IsEnabled = true };
        }

        protected internal override bool CanMerge(TriggerBase triggerBase) => false;

        protected internal override void ConnectTrigger<T>(object source, T container, INameScope? nameScope)
        {
            var setter = _setter;
            IHaveTriggerValue? target;
            if (setter.TargetName == null)
                target = (FrameworkElement)source;
            else
            {
                target = ((FrameworkElement)(object)container).FindName(setter.TargetName) as IHaveTriggerValue;
                if (target == null)
                    return;
            }
            target.AddTriggerValue(setter.Property!, TriggerLayer.VisualState, _value);
            target.InvalidateProperty(setter.Property!);
        }

        protected internal override void DisconnectTrigger<T>(object source, T container, INameScope? nameScope)
        {
            var setter = _setter;
            IHaveTriggerValue? target;
            if (setter.TargetName == null)
                target = (FrameworkElement)source;
            else
            {
                target = ((FrameworkElement)(object)container).FindName(setter.TargetName) as IHaveTriggerValue;
                if (target == null)
                    return;
            }
            target.RemoveTriggerValue(setter.Property!, TriggerLayer.VisualState, _value);
            target.InvalidateProperty(setter.Property!);
        }

        protected internal override TriggerBase Merge(TriggerBase triggerBase)
        {
            throw new NotSupportedException();
        }
    }
}
