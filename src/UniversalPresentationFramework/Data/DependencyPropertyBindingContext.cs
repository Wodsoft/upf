using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Data
{
    internal class DependencyPropertyBindingContext : PropertyBindingContext
    {
        public DependencyPropertyBindingContext(DependencyProperty property)
        {
            Property = property;
        }

        public DependencyProperty Property { get; }

        public override PropertyBinding CreateBinding(object source)
        {
            return new DependencyPropertyBinding((DependencyObject)source, Property);
        }

        public override object? GetValue(object source)
        {
            return ((DependencyObject)source).GetValue(Property);
        }
    }
}
