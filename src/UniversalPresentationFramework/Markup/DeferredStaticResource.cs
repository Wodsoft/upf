using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Markup
{
    internal class DeferredStaticResource : StaticResourceExtension
    {
        private readonly object? _value;

        public DeferredStaticResource(object resourceKey, object? value) : base(resourceKey)
        {
            _value = value;
        }

        public override object? ProvideValue(IServiceProvider serviceProvider)
        {
            var value = TryFindValue(serviceProvider);
            if (value == DependencyProperty.UnsetValue)
                value = _value;
            if (value == DependencyProperty.UnsetValue)
                throw new Exception($"No resource of key \"{ResourceKey}\" found.");
            return value;
        }
    }
}
