using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Input
{
    public class InputScopeConverter : TypeConverter
    {
        ///<summary>
        /// Returns whether this converter can convert an object of one type to InputScope type
        /// InputScopeConverter only supports string type to convert from
        ///</summary>
        ///<param name="context">
        /// The conversion context.
        ///</param>
        ///<param name="sourceType">
        /// The type to convert from.
        ///</param>
        ///<returns>
        /// True if conversion is possible, false otherwise.
        ///</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            // We can only handle string.
            return sourceType == typeof(string);
        }

        ///<summary>
        /// Returns whether this converter can convert the object to the specified type. 
        /// InputScopeConverter only supports string type to convert to
        ///</summary>
        ///<param name="context">
        /// The conversion context.
        ///</param>
        ///<param name="destinationType">
        /// The type to convert to.
        ///</param>
        ///<returns>
        /// True if conversion is possible, false otherwise.
        ///</returns>

        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        {
            return destinationType == typeof(string);
        }

        ///<summary>
        /// Converts the given value to InputScope type
        ///</summary>
        /// <param name="context">
        /// The conversion context.
        /// </param>
        /// <param name="culture">
        /// The current culture that applies to the conversion.
        /// </param>
        /// <param name="source">
        /// The source object to convert from.
        /// </param>
        /// <returns>
        /// InputScope object with a specified scope name, otherwise InputScope with Default scope.
        /// </returns>

        public override object ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object source)
        {
            InputScopeNameValue sn = InputScopeNameValue.Default;
            InputScope inputScope;

            if (source is string stringSource)
            {
                ReadOnlySpan<char> spanSource = stringSource;
                spanSource = spanSource.Trim();

                int periodPos = spanSource.LastIndexOf('.');
                if (periodPos != -1)
                {
                    spanSource = spanSource.Slice(periodPos + 1);
                }

                if (!spanSource.IsEmpty)
                {
                    sn = Enum.Parse<InputScopeNameValue>(spanSource);
                }
            }

            inputScope = new InputScope();
            inputScope.Names.Add(new InputScopeName(sn));
            return inputScope;
        }

        ///<summary>
        /// Converts the given value as InputScope object to the specified type. 
        /// This converter only supports string type as a type to convert to.
        ///</summary>
        /// <param name="context">
        /// The conversion context.
        /// </param>
        /// <param name="culture">
        /// The current culture that applies to the conversion.
        /// </param>
        /// <param name="value">
        /// The value to convert.
        /// </param>
        /// <param name="destinationType">
        /// The type to convert to.
        /// </param>
        /// <returns>
        /// A new object of the specified type (string) converted from the given InputScope object.
        /// </returns>
        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (value is InputScope inputScope && destinationType == typeof(string))
            {
                return Enum.GetName(typeof(InputScopeNameValue), inputScope.Names[0].NameValue);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
