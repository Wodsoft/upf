using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Data
{
    internal class ClrPropertyBindingContext : PropertyBindingContext
    {
        public ClrPropertyBindingContext(PropertyInfo property)
        {
            Property = property;
        }

        public PropertyInfo Property { get; }

        public override PropertyBinding CreateBinding(object source)
        {
            return new ClrPropertyBinding(source, Property);
        }

        public override object? GetValue(object source)
        {
            var cache = ClrPropertyBinding.GetCache(Property);
            if (cache.Get == null)
                return null;
            return cache.Get(source);
        }
    }
}
