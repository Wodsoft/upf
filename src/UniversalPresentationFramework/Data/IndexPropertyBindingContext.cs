using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Data
{
    internal class IndexPropertyBindingContext : PropertyBindingContext
    {
        public IndexPropertyBindingContext(object?[] parameters, PropertyInfo property)
        {
            Parameters = parameters;
            Property = property;
        }

        public object?[] Parameters { get; }

        public PropertyInfo Property { get; }

        public override PropertyBinding CreateBinding(object source)
        {
            return new IndexPropertyBinding(source, Parameters, Property);
        }

        public override object? GetValue(object source)
        {
            var cache = IndexPropertyBinding.GetCache(Property);
            if (cache.Get == null)
                return null;
            return cache.Get(source, Parameters);
        }
    }
}
