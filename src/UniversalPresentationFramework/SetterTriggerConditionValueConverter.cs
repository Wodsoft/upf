using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;
using Wodsoft.UI.Controls;

namespace Wodsoft.UI
{
    /// <summary>
    ///     Class for converting a given DependencyProperty to and from a string
    /// </summary>
    public sealed class SetterTriggerConditionValueConverter : TypeConverter
    {
        #region Public Methods

        /// <summary>
        ///     CanConvertFrom()
        /// </summary>
        /// <param name="context">ITypeDescriptorContext</param>
        /// <param name="sourceType">type to convert from</param>
        /// <returns>true if the given type can be converted, false otherwise</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            // We can only convert from a string and that too only if we have all the contextual information
            // Note: Sometimes even the serializer calls CanConvertFrom in order 
            // to determine if it is a valid converter to use for serialization.
            if (sourceType == typeof(string) || sourceType == typeof(byte[]))
                return true;

            return false;
        }

        /// <summary>
        ///     TypeConverter method override. 
        /// </summary>
        /// <param name="context">ITypeDescriptorContext</param>
        /// <param name="destinationType">Type to convert to</param>
        /// <returns>true if conversion is possible</returns>
        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        {
            return false;
        }

        /// <summary>
        ///     ConvertFrom() -TypeConverter method override. using the givein name to return DependencyProperty
        /// </summary>
        /// <param name="context">ITypeDescriptorContext</param>
        /// <param name="culture">CultureInfo</param>
        /// <param name="source">Object to convert from</param>
        /// <returns>instance of Command</returns>
        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? source)
        {
            return ResolveValue(context, null, culture, source);
        }
        //    

        /// <summary>
        ///     ConvertTo() - Serialization purposes, returns the string from Command.Name by adding ownerType.FullName
        /// </summary>
        /// <param name="context">ITypeDescriptorContext</param>
        /// <param name="culture">CultureInfo</param>
        /// <param name="value">the	object to convert from</param>
        /// <param name="destinationType">the type to convert to</param>
        /// <returns>string object, if the destination type is string</returns>
        public override object ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            throw GetConvertToException(value, destinationType);
        }

        #endregion Public Methods

        internal static object? ResolveValue(ITypeDescriptorContext? serviceProvider,
            DependencyProperty? property, CultureInfo? culture, object? source)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException("serviceProvider");
            if (source == null)
                throw new ArgumentNullException("source");

            // Only need to type convert strings and byte[]
            if (!(source is byte[] || source is String || source is Stream))
            {
                return source;
            }

            IXamlSchemaContextProvider? ixsc = (IXamlSchemaContextProvider?)serviceProvider.GetService(typeof(IXamlSchemaContextProvider));
            if (ixsc == null)
                throw new NotSupportedException("Can not convert value property.");
            XamlSchemaContext schemaContext = ixsc.SchemaContext;

            if (property != null)
            {
                //Get XamlMember from dp
                System.Xaml.XamlMember xamlProperty =
                    schemaContext.GetXamlType(property.OwnerType).GetMember(property.Name);
                if (xamlProperty == null)
                    xamlProperty =
                        schemaContext.GetXamlType(property.OwnerType).GetAttachableMember(property.Name);

                System.Xaml.Schema.XamlValueConverter<TypeConverter>? typeConverter = null;

                if (xamlProperty != null)
                {
                    // If we have a Baml2006SchemaContext and the property is of type Enum, we already know that the 
                    // type converter must be the EnumConverter.
                    //if (xamlProperty.Type.UnderlyingType.IsEnum && schemaContext is Baml2006.Baml2006SchemaContext)
                    //{
                    //    typeConverter = XamlReader.BamlSharedSchemaContext.GetTypeConverter(xamlProperty.Type.UnderlyingType);
                    //}
                    //else
                    //{
                    typeConverter = xamlProperty.TypeConverter;

                    if (typeConverter == null)
                    {
                        typeConverter = xamlProperty.Type.TypeConverter;
                    }
                    //}
                }
                else
                {
                    typeConverter = schemaContext.GetXamlType(property.PropertyType).TypeConverter;
                }


                // No Type converter case...
                if (typeConverter.ConverterType == null)
                {
                    return source;
                }

                TypeConverter? converter = null;

                if (xamlProperty != null && xamlProperty.Type.UnderlyingType == typeof(bool))
                {
                    if (source is string)
                    {
                        converter = new BooleanConverter();
                    }
                    else if (source is byte[])
                    {
                        if (source is byte[] bytes && bytes.Length == 1)
                        {
                            return (bytes[0] != 0);
                        }
                        else
                        {
                            throw new NotSupportedException("Can not convert value property.");
                        }
                    }
                    else
                    {
                        throw new NotSupportedException("Can not convert value property.");
                    }
                }
                else
                {
                    converter = (TypeConverter)typeConverter.ConverterInstance;
                }

                return converter.ConvertFrom(serviceProvider, culture, source);
            }
            else
            {
                return source;
            }
        }
    }
}
