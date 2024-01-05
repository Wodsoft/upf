using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using Wodsoft.UI.Controls;

namespace Wodsoft.UI
{
    public class DynamicResourceExtension : MarkupExtension
    {
        private object? _resourceKey;

        /// <summary>
        ///  Constructor that takes no parameters
        /// </summary>
        public DynamicResourceExtension()
        {
        }

        /// <summary>
        ///  Constructor that takes the resource key that this is a static reference to.
        /// </summary>
        public DynamicResourceExtension(
            object resourceKey)
        {
            if (resourceKey == null)
            {
                throw new ArgumentNullException("resourceKey");
            }
            _resourceKey = resourceKey;
        }


        /// <summary>
        ///  Return an object that should be set on the targetObject's targetProperty
        ///  for this markup extension.  For DynamicResourceExtension, this is the object found in
        ///  a resource dictionary in the current parent chain that is keyed by ResourceKey
        /// </summary>
        /// <returns>
        ///  The object to set on this property.
        /// </returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_resourceKey == null)
                throw new Exception($"DynamicResource must have a resource key.");

            var provideValueTarget = (IProvideValueTarget?)serviceProvider.GetService(typeof(IProvideValueTarget));
            if (provideValueTarget == null)
                throw new InvalidOperationException("Markup extension context not found.");
            if (provideValueTarget.TargetObject is not DependencyObject)
                throw new Exception("DynamicResource can only set to a dependency object.");
            if (provideValueTarget.TargetProperty is not DependencyProperty)
                throw new Exception("DynamicResource can only set to a dependency property.");

            return new ResourceReferenceExpression(_resourceKey);
        }



        /// <summary>
        ///  The key in a Resource Dictionary used to find the object refered to by this
        ///  Markup Extension.
        /// </summary>
        [ConstructorArgument("resourceKey")] // Uses an instance descriptor
        public object? ResourceKey
        {
            get { return _resourceKey; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                _resourceKey = value;
            }
        }
    }
}
