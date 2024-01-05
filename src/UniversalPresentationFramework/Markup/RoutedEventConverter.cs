using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using System.Xaml;

namespace Wodsoft.UI.Markup
{
    public sealed class RoutedEventConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? typeDescriptorContext, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            return false;
        }

        public override bool CanConvertTo(ITypeDescriptorContext? typeDescriptorContext, Type? destinationType)
        {
            return false;
        }

        public override object ConvertFrom(ITypeDescriptorContext? typeDescriptorContext,
                                           CultureInfo? cultureInfo,
                                           object? source)
        {
            string? routedEventName = source as string;
            RoutedEvent? routedEvent = null;

            if (routedEventName != null && typeDescriptorContext != null)
            {
                routedEventName = routedEventName.Trim();
                IXamlTypeResolver? resolver = (IXamlTypeResolver?)typeDescriptorContext.GetService(typeof(IXamlTypeResolver));
                Type? type = null;
                if (resolver != null)
                {
                    // Verify that there's at least one period.  (A simple
                    //  but not foolproof check for "[class].[event]")
                    int lastIndex = routedEventName.IndexOf('.');
                    if (lastIndex != -1)
                    {
                        string typeName = routedEventName.Substring(0, lastIndex);
                        routedEventName = routedEventName.Substring(lastIndex + 1);

                        type = resolver.Resolve(typeName);
                    }
                }
                if (type == null)
                {
                    IXamlSchemaContextProvider? schemaContextProvider = (IXamlSchemaContextProvider?)typeDescriptorContext.GetService(typeof(IXamlSchemaContextProvider));
                    IAmbientProvider? iapp = (IAmbientProvider?)typeDescriptorContext.GetService(typeof(IAmbientProvider));
                    if (schemaContextProvider != null && iapp != null)
                    {
                        XamlSchemaContext schemaContext = schemaContextProvider.SchemaContext;

                        XamlType styleXType = schemaContext.GetXamlType(typeof(Style));
                        List<XamlType> ceilingTypes = new List<XamlType>();
                        ceilingTypes.Add(styleXType);
                        XamlMember styleTargetType = styleXType.GetMember("TargetType");
                        AmbientPropertyValue firstAmbientValue = iapp.GetFirstAmbientValue(ceilingTypes, styleTargetType);
                        if (firstAmbientValue != null)
                            type = firstAmbientValue.Value as Type;
                        if (type == null)
                            type = typeof(FrameworkElement);
                    }
                }

                if (type != null)
                {
                    Type currentType = type;
                    // Force load the Statics by walking up the hierarchy and running class constructors
                    XamlReader.SchemaContext.GetXamlType(type);

                    routedEvent = EventManager.GetRoutedEventFromName(routedEventName, type, true);
                }
            }

            if (routedEvent == null)
            {
                // Falling through here means we are unable to perform the conversion.
                throw GetConvertFromException(source);
            }

            return routedEvent;
        }

        public override object ConvertTo(ITypeDescriptorContext? typeDescriptorContext,
                                         CultureInfo? cultureInfo,
                                         object? value,
                                         Type destinationType)
        {
            if (value is RoutedEvent routedEvent)
                return $"{routedEvent.OwnerType.Name}.{routedEvent.Name}";
            else
                throw GetConvertToException(value, destinationType);
        }
    }
}
