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
    public class FigureLengthConverter : TypeConverter
    {
        //-------------------------------------------------------------------
        //
        //  Public Methods
        //
        //-------------------------------------------------------------------

        #region Public Methods

        /// <summary>
        /// Checks whether or not this class can convert from a given type.
        /// </summary>
        /// <param name="typeDescriptorContext">The ITypeDescriptorContext 
        /// for this call.</param>
        /// <param name="sourceType">The Type being queried for support.</param>
        /// <returns>
        /// <c>true</c> if thie converter can convert from the provided type, 
        /// <c>false</c> otherwise.
        /// </returns>
        public override bool CanConvertFrom(
            ITypeDescriptorContext? typeDescriptorContext,
            Type sourceType)
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
        /// Checks whether or not this class can convert to a given type.
        /// </summary>
        /// <param name="typeDescriptorContext">The ITypeDescriptorContext 
        /// for this call.</param>
        /// <param name="destinationType">The Type being queried for support.</param>
        /// <returns>
        /// <c>true</c> if this converter can convert to the provided type, 
        /// <c>false</c> otherwise.
        /// </returns>
        public override bool CanConvertTo(
            ITypeDescriptorContext? typeDescriptorContext,
            Type? destinationType)
        {
            return (destinationType == typeof(InstanceDescriptor)
                    || destinationType == typeof(string));
        }

        /// <summary>
        /// Attempts to convert to a FigureLength from the given object.
        /// </summary>
        /// <param name="typeDescriptorContext">The ITypeDescriptorContext for this call.</param>
        /// <param name="cultureInfo">The CultureInfo which is respected when converting.</param>
        /// <param name="source">The object to convert to a FigureLength.</param>
        /// <returns>
        /// The FigureLength instance which was constructed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// An ArgumentNullException is thrown if the example object is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// An ArgumentException is thrown if the example object is not null 
        /// and is not a valid type which can be converted to a FigureLength.
        /// </exception>
        public override object ConvertFrom(
            ITypeDescriptorContext? typeDescriptorContext,
            CultureInfo? cultureInfo,
            object source)
        {
            if (source != null)
            {
                if (source is string)
                {
                    return FromString((string)source, cultureInfo);
                }
                else
                {
                    return new FigureLength(Convert.ToSingle(source, cultureInfo)); //conversion from numeric type
                }
            }
            throw GetConvertFromException(source);
        }

        /// <summary>
        /// Attempts to convert a FigureLength instance to the given type.
        /// </summary>
        /// <param name="typeDescriptorContext">The ITypeDescriptorContext for this call.</param>
        /// <param name="cultureInfo">The CultureInfo which is respected when converting.</param>
        /// <param name="value">The FigureLength to convert.</param>
        /// <param name="destinationType">The type to which to convert the FigureLength instance.</param>
        /// <returns>
        /// The object which was constructed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// An ArgumentNullException is thrown if the example object is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// An ArgumentException is thrown if the object is not null and is not a FigureLength,
        /// or if the destinationType isn't one of the valid destination types.
        /// </exception>
        public override object ConvertTo(
            ITypeDescriptorContext? typeDescriptorContext,
            CultureInfo? cultureInfo,
            object? value,
            Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }

            if (value != null
                && value is FigureLength)
            {
                FigureLength fl = (FigureLength)value;

                if (destinationType == typeof(string))
                {
                    return (ToString(fl, cultureInfo));
                }

                if (destinationType == typeof(InstanceDescriptor))
                {
                    ConstructorInfo ci = typeof(FigureLength).GetConstructor(new Type[] { typeof(double), typeof(FigureUnitType) })!;
                    return (new InstanceDescriptor(ci, new object[] { fl.Value, fl.FigureUnitType }));
                }
            }
            throw GetConvertToException(value, destinationType);
        }

        #endregion Public Methods

        //-------------------------------------------------------------------
        //
        //  Internal Methods
        //
        //-------------------------------------------------------------------

        #region Internal Methods

        /// <summary>
        /// Converts a FigureLength instance to a String given the CultureInfo.
        /// </summary>
        /// <param name="fl">FigureLength instance to convert.</param>
        /// <param name="cultureInfo">Culture Info.</param>
        /// <returns>String representation of the object.</returns>
        static internal string ToString(FigureLength fl, CultureInfo? cultureInfo)
        {
            switch (fl.FigureUnitType)
            {
                //  for Auto print out "Auto". value is always "1.0"
                case FigureUnitType.Auto:
                    return ("Auto");

                case FigureUnitType.Pixel:
                    return Convert.ToString(fl.Value, cultureInfo);

                default:
                    return Convert.ToString(fl.Value, cultureInfo) + " " + fl.FigureUnitType.ToString();
            }
        }

        /// <summary>
        /// Parses a FigureLength from a string given the CultureInfo.
        /// </summary>
        /// <param name="s">String to parse from.</param>
        /// <param name="cultureInfo">Culture Info.</param>
        /// <returns>Newly created FigureLength instance.</returns>
        /// <remarks>
        /// Formats: 
        /// "[value][unit]"
        ///     [value] is a double
        ///     [unit] is a string in FigureLength._unitTypes connected to a FigureUnitType
        /// "[value]"
        ///     As above, but the FigureUnitType is assumed to be FigureUnitType.Pixel
        /// "[unit]"
        ///     As above, but the value is assumed to be 1.0
        ///     This is only acceptable for a subset of FigureUnitType: Auto
        /// </remarks>
        static internal FigureLength FromString(string s, CultureInfo? cultureInfo)
        {
            s = s.Trim().ToLowerInvariant();
            FigureUnitType unit;
            float value;
            if (s == "auto")
                return new FigureLength(1f, FigureUnitType.Auto);
            else if (s.EndsWith("px"))
            {
                unit = FigureUnitType.Pixel;
                s = s.Substring(0, s.Length - 2);
                value = Convert.ToSingle(s);
            }
            else if (s.EndsWith("column"))
            {
                unit = FigureUnitType.Column;
                s = s.Substring(0, s.Length - 6);
                value = Convert.ToSingle(s);
            }
            else if (s.EndsWith("columns"))
            {
                unit = FigureUnitType.Column;
                s = s.Substring(0, s.Length - 7);
                value = Convert.ToSingle(s);
            }
            else if (s.EndsWith("content"))
            {
                unit = FigureUnitType.Content;
                s = s.Substring(0, s.Length - 6);
                value = Convert.ToSingle(s);
            }
            else if (s.EndsWith("page"))
            {
                unit = FigureUnitType.Page;
                s = s.Substring(0, s.Length - 4);
                value = Convert.ToSingle(s);
            }
            else
            {
                unit = FigureUnitType.Pixel;
                if (s.EndsWith("in"))
                {
                    s = s.Substring(0, s.Length - 2);
                    value = Convert.ToSingle(s) * _In;
                }
                else if (s.EndsWith("cm"))
                {
                    s = s.Substring(0, s.Length - 2);
                    value = Convert.ToSingle(s) * _Cm;
                }
                else if (s.EndsWith("pt"))
                {
                    s = s.Substring(0, s.Length - 2);
                    value = Convert.ToSingle(s) * _Pt;
                }
                else
                    value = Convert.ToSingle(s);
            }
            return new FigureLength(value, unit);
        }

        private const float _In = 96f, _Cm = 96f / 2.54f, _Pt = 96f / 72f;

        #endregion Internal Methods
    }
}
