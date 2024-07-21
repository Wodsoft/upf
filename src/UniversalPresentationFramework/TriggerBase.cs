using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;

namespace Wodsoft.UI
{
    public abstract class TriggerBase : SealableDependencyObject
    {
        /// <summary>
        /// Can this trigger merge with target trigger.
        /// </summary>
        /// <param name="triggerBase">Target trigger.</param>
        /// <returns></returns>
        protected internal abstract bool CanMerge(TriggerBase triggerBase);

        /// <summary>
        /// Merge this trigger.
        /// </summary>
        /// <param name="triggerBase">Merge into trigger.</param>
        /// <returns>Return a new trigger.</returns>
        protected internal abstract TriggerBase Merge(TriggerBase triggerBase);

        protected internal abstract void ConnectTrigger<T>(object source, T container, INameScope? nameScope)
            where T : DependencyObject , IHaveTriggerValue;

        protected internal abstract void DisconnectTrigger<T>(object source, T container, INameScope? nameScope)
            where T : DependencyObject, IHaveTriggerValue;
    }
}
