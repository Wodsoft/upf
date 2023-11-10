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
    /// <summary>
    /// LengthConverter - Converter class for converting instances of other types to and from float representing length.
    /// </summary> 
    public class LengthConverter : TypeConverter
    {
        //-------------------------------------------------------------------
        //
        //  Public Methods
        //
        //-------------------------------------------------------------------

        #region Public Methods

        /// <summary>
        /// CanConvertFrom - Returns whether or not this class can convert from a given type.
        /// </summary>
        /// <returns>
        /// bool - True if thie converter can convert from the provided type, false if not.
        /// </returns>
        /// <param name="typeDescriptorContext"> The ITypeDescriptorContext for this call. </param>
        /// <param name="sourceType"> The Type being queried for support. </param>
        public override bool CanConvertFrom(ITypeDescriptorContext? typeDescriptorContext, Type sourceType)
        {
            // We can only handle strings, integral and floating types
            TypeCode tc = Type.GetTypeCode(sourceType);
            switch (tc)
            {
                case TypeCode.String:
                case TypeCode.Decimal:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// CanConvertTo - Returns whether or not this class can convert to a given type.
        /// </summary>
        /// <returns>
        /// bool - True if this converter can convert to the provided type, false if not.
        /// </returns>
        /// <param name="typeDescriptorContext"> The ITypeDescriptorContext for this call. </param>
        /// <param name="destinationType"> The Type being queried for support. </param>
        public override bool CanConvertTo(ITypeDescriptorContext? typeDescriptorContext, Type? destinationType)
        {
            // We can convert to an InstanceDescriptor or to a string.
            if (destinationType == typeof(InstanceDescriptor) ||
                destinationType == typeof(string))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// ConvertFrom - Attempt to convert to a length from the given object
        /// </summary>
        /// <returns>
        /// The float representing the size in 1/96th of an inch.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// An ArgumentNullException is thrown if the example object is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// An ArgumentException is thrown if the example object is not null and is not a valid type
        /// which can be converted to a float.
        /// </exception>
        /// <param name="typeDescriptorContext"> The ITypeDescriptorContext for this call. </param>
        /// <param name="cultureInfo"> The CultureInfo which is respected when converting. </param>
        /// <param name="source"> The object to convert to a float. </param>
        public override object ConvertFrom(ITypeDescriptorContext? typeDescriptorContext,
                                           CultureInfo? cultureInfo,
                                           object source)
        {
            if (source != null)
            {
                if (source is string) { return FromString((string)source, cultureInfo); }
                else { return Convert.ToSingle(source, cultureInfo); }
            }

            throw GetConvertFromException(source);
        }

        /// <summary>
        /// ConvertTo - Attempt to convert a float to the given type
        /// </summary>
        /// <returns>
        /// The object which was constructed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// An ArgumentNullException is thrown if the example object is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// An ArgumentException is thrown if the object is not null,
        /// or if the destinationType isn't one of the valid destination types.
        /// </exception>
        /// <param name="typeDescriptorContext"> The ITypeDescriptorContext for this call. </param>
        /// <param name="cultureInfo"> The CultureInfo which is respected when converting. </param>
        /// <param name="value"> The float to convert. </param>
        /// <param name="destinationType">The type to which to convert the float. </param>
        public override object ConvertTo(ITypeDescriptorContext? typeDescriptorContext,
                                         CultureInfo? cultureInfo,
                                         object? value,
                                         Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }

            if (value != null
                && value is float l)
            {
                if (destinationType == typeof(string))
                {
                    if (float.IsNaN(l))
                        return "Auto";
                    else
                        return Convert.ToString(l, cultureInfo);
                }
                else if (destinationType == typeof(InstanceDescriptor))
                {
                    ConstructorInfo ci = typeof(float).GetConstructor(new Type[] { typeof(float) })!;
                    return new InstanceDescriptor(ci, new object[] { l });
                }
            }
            throw GetConvertToException(value, destinationType);
        }
        #endregion 

        //-------------------------------------------------------------------
        //
        //  Internal Methods
        //
        //-------------------------------------------------------------------

        #region Internal Methods


        // Parse a Length from a string given the CultureInfo.
        // Formats: 
        //"[value][unit]"
        //   [value] is a float
        //   [unit] is a string specifying the unit, like 'in' or 'px', or nothing (means pixels)
        // NOTE - This code is called from FontSizeConverter, so changes will affect both.
        static internal float FromString(string s, CultureInfo? cultureInfo)
        {
            string valueString = s.Trim();
            string goodString = valueString.ToLowerInvariant();
            int strLen = goodString.Length;
            int strLenUnit = 0;
            float unitFactor = 1.0f;

            //Auto is represented and float.NaN
            //properties that do not want Auto and NaN to be in their ligit values,
            //should disallow NaN in validation callbacks (same goes for negative values)
            if (goodString == "auto") return float.NaN;

            for (int i = 0; i < _PixelUnitStrings.Length; i++)
            {
                // NOTE: This is NOT a culture specific comparison.
                // This is by design: we want the same unit string table to work across all cultures.
                if (goodString.EndsWith(_PixelUnitStrings[i], StringComparison.Ordinal))
                {
                    strLenUnit = _PixelUnitStrings[i].Length;
                    unitFactor = _PixelUnitFactors[i];
                    break;
                }
            }

            //  important to substring original non-lowered string 
            //  this allows case sensitive ToSingle below handle "NaN" and "Infinity" correctly. 
            //  this addresses windows bug 1177408
            valueString = valueString.Substring(0, strLen - strLenUnit);

            // FormatException errors thrown by Convert.ToSingle are pretty uninformative.
            // Throw a more meaningful error in this case that tells that we were attempting
            // to create a Length instance from a string.  This addresses windows bug 968884
            try
            {
                var result = Convert.ToSingle(valueString, cultureInfo) * unitFactor;
                return result;
            }
            catch (FormatException)
            {
                throw new FormatException("Length format error.");
            }
        }

        // This array contains strings for unit types 
        // These are effectively "TypeConverter only" units.
        // They are all expressable in terms of the Pixel unit type and a conversion factor.
        static private string[] _PixelUnitStrings = { "px", "in", "cm", "pt" };
        static private float[] _PixelUnitFactors =
        {
            1.0f,              // Pixel itself
            96.0f,             // Pixels per Inch
            96.0f / 2.54f,      // Pixels per Centimeter
            96.0f / 72.0f,      // Pixels per Point
        };

        static internal string ToString(float l, CultureInfo? cultureInfo)
        {
            if (float.IsNaN(l)) return "Auto";
            return Convert.ToString(l, cultureInfo);
        }

        #endregion

    }
}
