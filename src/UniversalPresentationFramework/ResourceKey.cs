using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;

namespace Wodsoft.UI
{
    [MarkupExtensionReturnType(typeof(ResourceKey))]
    public abstract class ResourceKey : MarkupExtension
    {
        /// <summary>
        ///     Used to determine where to look for the resource dictionary that holds this resource.
        /// </summary>
        public abstract Assembly Assembly
        {
            get;
        }

        /// <summary>
        ///  Return this object.  ResourceKeys are typically used as a key in a dictionary.
        /// </summary>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public virtual bool HasResource { get; } = false;

        public virtual object? Resource { get; } = null;
    }
}
