using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public interface IHaveTriggerValue : IInputElement
    {
        void AddTriggerBinding(TriggerBase trigger, TriggerBinding triggerBinding);
        void RemoveTriggerBinding(TriggerBase trigger);
        void AddTriggerValue(DependencyProperty dp, byte layer, TriggerValue triggerValue);
        void RemoveTriggerValue(DependencyProperty dp, byte layer, TriggerValue triggerValue);
        void InvalidateProperty(DependencyProperty dp);
    }
}
