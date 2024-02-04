using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;

namespace Wodsoft.UI
{
    public abstract class TriggerAction : SealableDependencyObject
    {
        public abstract void Invoke(object source, DependencyObject container, INameScope? nameScope);
    }
}
