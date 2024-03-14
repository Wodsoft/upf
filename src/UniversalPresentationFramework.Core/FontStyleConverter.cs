using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public sealed class FontStyleConverter : TypeConverter
    {
        /// <summary>
        /// CanConvertFrom
        /// </summary>
        public override bool CanConvertFrom(ITypeDescriptorContext? td, Type? t)
        {
            if (t == typeof(string))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// TypeConverter method override.
        /// </summary>
        /// <param name="context">ITypeDescriptorContext</param>
        /// <param name="destinationType">Type to convert to</param>
        /// <returns>true if conversion is possible</returns>
        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor) || destinationType == typeof(string))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// ConvertFrom - attempt to convert to a FontStyle from the given object
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// A NotSupportedException is thrown if the example object is null or is not a valid type
        /// which can be converted to a FontStyle.
        /// </exception>
        public override object ConvertFrom(ITypeDescriptorContext? td, CultureInfo? ci, object value)
        {
            if (null == value)
            {
                throw GetConvertFromException(value);
            }

            if (value is not string s)
            {
                throw new ArgumentException("FontStyle can only convert from string.", "value");
            }

            FontStyle fontStyle = new FontStyle();
            if (!FontStyles.FontStyleStringToKnownStyle(s, ref fontStyle))
                throw new FormatException($"Invalid FontStyle value \"{s}\".");

            return fontStyle;
        }

        /// <summary>
        /// TypeConverter method implementation.
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// An NotSupportedException is thrown if the example object is null or is not a FontStyle,
        /// or if the destinationType isn't one of the valid destination types.
        /// </exception>
        /// <param name="context">ITypeDescriptorContext</param>
        /// <param name="culture">current culture (see CLR specs)</param>
        /// <param name="value">value to convert from</param>
        /// <param name="destinationType">Type to convert to</param>
        /// <returns>converted value</returns>
        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (value is FontStyle)
            {
                if (destinationType == typeof(InstanceDescriptor))
                {
                    ConstructorInfo ci = typeof(FontStyle).GetConstructor([typeof(int)])!;
                    int c = ((FontStyle)value).GetStyleForInternalConstruction();
                    return new InstanceDescriptor(ci, new object[] { c });
                }
                else if (destinationType == typeof(string))
                {
                    FontStyle c = (FontStyle)value;
                    return ((IFormattable)c).ToString(null, culture);
                }
            }

            // Pass unhandled cases to base class (which will throw exceptions for null value or destinationType.)
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
