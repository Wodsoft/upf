using System.ComponentModel.Design.Serialization;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Wodsoft.UI
{
    public sealed class FontStretchConverter : TypeConverter
    {
        /// <summary>
        /// CanConvertFrom
        /// </summary>
        public override bool CanConvertFrom(ITypeDescriptorContext? td, Type t)
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
        /// ConvertFrom - attempt to convert to a FontStretch from the given object
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// A NotSupportedException is thrown if the example object is null or is not a valid type
        /// which can be converted to a FontStretch.
        /// </exception>
        public override object ConvertFrom(ITypeDescriptorContext? td, CultureInfo? ci, object value)
        {
            if (null == value)
            {
                throw GetConvertFromException(value);
            }

            if (value is not string s)
            {
                throw new ArgumentException("FontStretch can only convert from string.", "value");
            }

            FontStretch fontStretch = new FontStretch();
            if (!FontStretches.FontStretchStringToKnownStretch(s, ci, ref fontStretch))
                throw new FormatException($"Invalid FontStretch value \"{s}\".");

            return fontStretch;
        }

        /// <summary>
        /// TypeConverter method implementation.
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// An NotSupportedException is thrown if the example object is null or is not a FontStretch,
        /// or if the destinationType isn't one of the valid destination types.
        /// </exception>
        /// <param name="context">ITypeDescriptorContext</param>
        /// <param name="culture">current culture (see CLR specs)</param>
        /// <param name="value">value to convert from</param>
        /// <param name="destinationType">Type to convert to</param>
        /// <returns>converted value</returns>
        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (value is FontStretch)
            {
                if (destinationType == typeof(InstanceDescriptor))
                {
                    MethodInfo mi = typeof(FontStretch).GetMethod("FromOpenTypeStretch", [typeof(int)])!;
                    FontStretch c = (FontStretch)value;
                    return new InstanceDescriptor(mi, new object[] { c.ToOpenTypeStretch() });
                }
                else if (destinationType == typeof(string))
                {
                    FontStretch c = (FontStretch)value;
                    return ((IFormattable)c).ToString(null, culture);
                }
            }

            // Pass unhandled cases to base class (which will throw exceptions for null value or destinationType.)
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}