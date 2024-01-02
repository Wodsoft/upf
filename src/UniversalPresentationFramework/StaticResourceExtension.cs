using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;
using System.Xaml.Markup;
using Wodsoft.UI.Controls;

namespace Wodsoft.UI
{
    public class StaticResourceExtension : MarkupExtension
    {
        private object? _resourceKey;

        /// <summary>
        ///  Constructor that takes no parameters
        /// </summary>
        public StaticResourceExtension()
        {
        }

        /// <summary>
        ///  Constructor that takes the resource key that this is a static reference to.
        /// </summary>
        public StaticResourceExtension(
            object resourceKey)
        {
            if (resourceKey == null)
                throw new ArgumentNullException("resourceKey");
            _resourceKey = resourceKey;
        }

        /// <summary>
        ///  The key in a Resource Dictionary used to find the object refered to by this
        ///  Markup Extension.
        /// </summary>
        [ConstructorArgument("resourceKey")]
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

        public override object? ProvideValue(IServiceProvider serviceProvider)
        {
            if (_resourceKey == null)
                throw new Exception($"StaticResource must have a resource key.");
            var value = TryFindValue(serviceProvider);
            if (value == DependencyProperty.UnsetValue)
                throw new Exception($"No resource of key \"{_resourceKey}\" found.");
            return value;
        }

        protected internal object? TryFindValue(IServiceProvider serviceProvider)
        {
            var resourceDictionary = FindResourceDictionary(serviceProvider);
            if (resourceDictionary != null)
                return resourceDictionary[_resourceKey!];
            return ResourceHelper.FindResourceInApplication(_resourceKey!);
        }

        private ResourceDictionary? FindResourceDictionary(IServiceProvider serviceProvider)
        {
            var schemaContextProvider = (IXamlSchemaContextProvider?)serviceProvider.GetService(typeof(IXamlSchemaContextProvider));
            if (schemaContextProvider == null)
                throw new InvalidOperationException("Markup extension context not found.");

            var ambientProvider = (IAmbientProvider?)serviceProvider.GetService(typeof(IAmbientProvider));
            if (ambientProvider == null)
                throw new InvalidOperationException("Markup extension context not found.");

            XamlSchemaContext schemaContext = schemaContextProvider.SchemaContext;

            // This seems like a lot of work to do on every Provide Value
            // but that types and properties are cached in the schema.
            //
            XamlType feXType = schemaContext.GetXamlType(typeof(FrameworkElement));
            XamlType styleXType = schemaContext.GetXamlType(typeof(Style));
            XamlType templateXType = schemaContext.GetXamlType(typeof(FrameworkTemplate));
            XamlType appXType = schemaContext.GetXamlType(typeof(Application));
            XamlType fceXType = schemaContext.GetXamlType(typeof(FrameworkContentElement));

            XamlMember fceResourcesProperty = fceXType.GetMember("Resources");
            XamlMember feResourcesProperty = feXType.GetMember("Resources");
            XamlMember styleResourcesProperty = styleXType.GetMember("Resources");
            XamlMember styleBasedOnProperty = styleXType.GetMember("BasedOn");
            XamlMember templateResourcesProperty = templateXType.GetMember("Resources");
            XamlMember appResourcesProperty = appXType.GetMember("Resources");

            XamlType[] types = new XamlType[1] { schemaContext.GetXamlType(typeof(ResourceDictionary)) };

            IEnumerable<AmbientPropertyValue> ambientValues = ambientProvider.GetAllAmbientValues(null,    // ceilingTypes
                                                                false,
                                                                types,
                                                                fceResourcesProperty,
                                                                feResourcesProperty,
                                                                styleResourcesProperty,
                                                                styleBasedOnProperty,
                                                                templateResourcesProperty,
                                                                appResourcesProperty);

            List<AmbientPropertyValue> ambientList = ambientValues.ToList();
            foreach (var ambientValue in ambientList)
            {
                if (ambientValue.Value is ResourceDictionary)
                {
                    var resourceDictionary = (ResourceDictionary)ambientValue.Value;
                    if (resourceDictionary.Contains(_resourceKey!))
                    {
                        return resourceDictionary;
                    }
                }
                if (ambientValue.Value is Style)
                {
                    var style = (Style)ambientValue.Value;
                    var resourceDictionary = style.FindResourceDictionary(_resourceKey!);
                    if (resourceDictionary != null)
                    {
                        return resourceDictionary;
                    }
                }
            }
            return null;

        }
    }
}
