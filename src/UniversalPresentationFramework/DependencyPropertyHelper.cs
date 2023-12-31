using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using System.Xaml;
using Wodsoft.UI.Controls;

namespace Wodsoft.UI
{
    internal static class DependencyPropertyHelper
    {
        internal static DependencyProperty ResolveProperty(IServiceProvider serviceProvider,
            string? targetName, object source)
        {
            Type? type = null;
            string? property = null;

            if (source is DependencyProperty dp)
                return dp;
            // If it's a byte[] we got it from BAML.  Let's resolve it using the schema context
            //else if (source is byte[] bytes)
            //{
            //    Baml2006SchemaContext schemaContext = (serviceProvider.GetService(typeof(IXamlSchemaContextProvider))
            //        as IXamlSchemaContextProvider).SchemaContext as Baml2006SchemaContext;

            //    // Array with length of 2 means its an ID for a
            //    // DependencyProperty in the Baml2006SchemaContext
            //    if (bytes.Length == 2)
            //    {
            //        short propId = (short)(bytes[0] | (bytes[1] << 8));

            //        return schemaContext.GetDependencyProperty(propId);
            //    }
            //    else
            //    {
            //        // Otherwise it's a string with a TypeId encoded in front
            //        using (BinaryReader reader = new BinaryReader(new MemoryStream(bytes)))
            //        {
            //            type = schemaContext.GetXamlType(reader.ReadInt16()).UnderlyingType;
            //            property = reader.ReadString();
            //        }
            //    }
            //}
            else if (source is string value)
            {
                value = value.Trim();
                // If it contains a . it means that it is a full name with type and property.
                if (value.Contains("."))
                {
                    // Prefixes could have .'s so we take the last one and do a type resolve against that
                    int lastIndex = value.LastIndexOf('.');
                    string typeName = value.Substring(0, lastIndex);
                    property = value.Substring(lastIndex + 1);

                    IXamlTypeResolver? resolver = (IXamlTypeResolver?)serviceProvider.GetService(typeof(IXamlTypeResolver));
                    if (resolver != null)
                        type = resolver.Resolve(typeName);
                }
                else
                {
                    // Only have the property name
                    // Strip prefixes if there are any, v3 essentially discards the prefix in this case
                    int lastIndex = value.LastIndexOf(':');
                    property = value.Substring(lastIndex + 1);
                }
            }
            else
            {
                throw new NotSupportedException("Can not convert property value.");
            }

            // We got additional info from either Trigger.SourceName or Setter.TargetName
            if (type == null && targetName != null)
            {
                IAmbientProvider? ambientProvider = (IAmbientProvider?)serviceProvider.GetService(typeof(IAmbientProvider));
                if (ambientProvider != null)
                {
                    var contextProvider = (IXamlSchemaContextProvider?)serviceProvider.GetService(typeof(IXamlSchemaContextProvider));
                    if (contextProvider != null)
                        type = GetTypeFromName(contextProvider.SchemaContext, ambientProvider, targetName);
                }
            }

            // Still don't have a Type so we need to loop up the chain and grab either Style.TargetType,
            // DataTemplate.DataType, or ControlTemplate.TargetType
            if (type == null)
            {
                IXamlSchemaContextProvider? ixscp = (IXamlSchemaContextProvider?)serviceProvider.GetService(typeof(IXamlSchemaContextProvider));
                if (ixscp == null)
                    throw new NotSupportedException("Can not convert property value.");
                XamlSchemaContext schemaContext = ixscp.SchemaContext;

                XamlType styleXType = schemaContext.GetXamlType(typeof(Style));
                XamlType frameworkTemplateXType = schemaContext.GetXamlType(typeof(FrameworkTemplate));
                XamlType dataTemplateXType = schemaContext.GetXamlType(typeof(DataTemplate));
                XamlType controlTemplateXType = schemaContext.GetXamlType(typeof(ControlTemplate));

                List<XamlType> ceilingTypes =
                [
                    styleXType,
                    frameworkTemplateXType,
                    dataTemplateXType,
                    controlTemplateXType,
                ];

                // We don't look for DataTemplate's DataType since we want to use the TargetTypeInternal instead
                XamlMember styleTargetType = styleXType.GetMember("TargetType");
                XamlMember templateProperty = frameworkTemplateXType.GetMember("Template");
                XamlMember controlTemplateTargetType = controlTemplateXType.GetMember("TargetType");

                IAmbientProvider? ambientProvider = (IAmbientProvider?)serviceProvider.GetService(typeof(IAmbientProvider));
                if (ambientProvider == null)
                    throw new NotSupportedException("Can not convert property value.");
                AmbientPropertyValue firstAmbientValue = ambientProvider.GetFirstAmbientValue(ceilingTypes, styleTargetType,
                    templateProperty, controlTemplateTargetType);

                if (firstAmbientValue != null)
                {
                    if (firstAmbientValue.Value is Type fType)
                    {
                        type = fType;
                    }
                    else if (firstAmbientValue.Value is TemplateContent tempContent)
                    {
                        type = tempContent.OwnerTemplate?.TargetTypeInternal;
                    }
                    else
                        throw new NotSupportedException("Can not convert property value.");
                }
            }

            if (type != null && property != null)
            {
                var dp2 = DependencyProperty.FromName(property, type);
                if (dp2 == null)
                    throw new ArgumentException($"Can not find dependency property for \"{type.FullName}.{property}\".");
                return dp2;
            }

            throw new NotSupportedException("Can not convert property value.");
        }
        private static Type? GetTypeFromName(XamlSchemaContext schemaContext, IAmbientProvider ambientProvider, string target)
        {
            XamlType frameworkTemplateXType = schemaContext.GetXamlType(typeof(FrameworkTemplate));
            XamlMember templateProperty = frameworkTemplateXType.GetMember("Template");

            AmbientPropertyValue ambientValue =
                ambientProvider.GetFirstAmbientValue(new XamlType[] { frameworkTemplateXType }, templateProperty);
            TemplateContent? templateHolder = ambientValue.Value as TemplateContent;

            if (templateHolder != null)
                return templateHolder.GetTypeForName(target);
            return null;
        }
    }
}
