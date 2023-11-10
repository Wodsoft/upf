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
    public class ThicknessConverter : TypeConverter
    {
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
            if (destinationType == typeof(InstanceDescriptor)
                || destinationType == typeof(string))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// ConvertFrom - Attempt to convert to a Thickness from the given object
        /// </summary>
        /// <returns>
        /// The Thickness which was constructed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// An ArgumentNullException is thrown if the example object is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// An ArgumentException is thrown if the example object is not null and is not a valid type
        /// which can be converted to a Thickness.
        /// </exception>
        /// <param name="typeDescriptorContext"> The ITypeDescriptorContext for this call. </param>
        /// <param name="cultureInfo"> The CultureInfo which is respected when converting. </param>
        /// <param name="source"> The object to convert to a Thickness. </param>
        public override object ConvertFrom(ITypeDescriptorContext? typeDescriptorContext, CultureInfo? cultureInfo, object? source)
        {
            if (source != null)
            {
                if (source is string stringValue) { return FromString(stringValue, cultureInfo); }
                else if (source is float floatValue) { return new Thickness(floatValue); }
                else { return new Thickness(Convert.ToSingle(source, cultureInfo)); }
            }
            throw GetConvertFromException(source);
        }

        /// <summary>
        /// ConvertTo - Attempt to convert a Thickness to the given type
        /// </summary>
        /// <returns>
        /// The object which was constructoed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// An ArgumentNullException is thrown if the example object is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// An ArgumentException is thrown if the object is not null and is not a Thickness,
        /// or if the destinationType isn't one of the valid destination types.
        /// </exception>
        /// <param name="typeDescriptorContext"> The ITypeDescriptorContext for this call. </param>
        /// <param name="cultureInfo"> The CultureInfo which is respected when converting. </param>
        /// <param name="value"> The Thickness to convert. </param>
        /// <param name="destinationType">The type to which to convert the Thickness instance. </param>
        public override object ConvertTo(ITypeDescriptorContext? typeDescriptorContext, CultureInfo? cultureInfo, object? value, Type destinationType)
        {
            if (null == value)
            {
                throw new ArgumentNullException("value");
            }

            if (null == destinationType)
            {
                throw new ArgumentNullException("destinationType");
            }

            if (!(value is Thickness))
            {
                throw new ArgumentException("Unexpected parameter type.");
            }

            Thickness th = (Thickness)value;
            if (destinationType == typeof(string)) { return ToString(th, cultureInfo); }
            if (destinationType == typeof(InstanceDescriptor))
            {
                ConstructorInfo ci = typeof(Thickness).GetConstructor(new Type[] { typeof(float), typeof(float), typeof(float), typeof(float) })!;
                return new InstanceDescriptor(ci, new object[] { th.Left, th.Top, th.Right, th.Bottom });
            }

            throw new ArgumentException($"Can not convert to type \"{destinationType.FullName}\".");

        }


        #endregion Public Methods

        //-------------------------------------------------------------------
        //
        //  Internal Methods
        //
        //-------------------------------------------------------------------

        #region Internal Methods

        static internal string ToString(Thickness th, CultureInfo? cultureInfo)
        {
            char listSeparator = TokenizerHelper.GetNumericListSeparator(cultureInfo);

            // Initial capacity [64] is an estimate based on a sum of:
            // 48 = 4x double (twelve digits is generous for the range of values likely)
            //  8 = 4x Unit Type string (approx two characters)
            //  4 = 4x separator characters
            StringBuilder sb = new StringBuilder(64);

            sb.Append(LengthConverter.ToString(th.Left, cultureInfo));
            sb.Append(listSeparator);
            sb.Append(LengthConverter.ToString(th.Top, cultureInfo));
            sb.Append(listSeparator);
            sb.Append(LengthConverter.ToString(th.Right, cultureInfo));
            sb.Append(listSeparator);
            sb.Append(LengthConverter.ToString(th.Bottom, cultureInfo));
            return sb.ToString();
        }

        static internal Thickness FromString(string s, CultureInfo? cultureInfo)
        {
            TokenizerHelper th = new TokenizerHelper(s, cultureInfo);
            float[] lengths = new float[4];
            int i = 0;

            // Peel off each double in the delimited list.
            while (th.NextToken())
            {
                if (i >= 4)
                {
                    i = 5;    // Set i to a bad value. 
                    break;
                }

                lengths[i] = LengthConverter.FromString(th.GetCurrentToken()!, cultureInfo);
                i++;
            }

            // We have a reasonable interpreation for one value (all four edges), two values (horizontal, vertical),
            // and four values (left, top, right, bottom).
            switch (i)
            {
                case 1:
                    return new Thickness(lengths[0]);

                case 2:
                    return new Thickness(lengths[0], lengths[1], lengths[0], lengths[1]);

                case 4:
                    return new Thickness(lengths[0], lengths[1], lengths[2], lengths[3]);
            }

            throw new FormatException("Invalid thickness string.");
        }

        #endregion

    }
}
