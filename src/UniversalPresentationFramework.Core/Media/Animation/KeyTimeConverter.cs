using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class KeyTimeConverter : TypeConverter
    {
        /// <summary>
        /// Returns whether or not this class can convert from a given type
        /// to an instance of a KeyTime.
        /// </summary>
        public override bool CanConvertFrom(
            ITypeDescriptorContext? typeDescriptorContext,
            Type type)
        {
            if (type == typeof(string))
            {
                return true;
            }
            else
            {
                return base.CanConvertFrom(
                    typeDescriptorContext,
                    type);
            }
        }

        /// <summary>
        /// Returns whether or not this class can convert from an instance of a
        /// KeyTime to a given type.
        /// </summary>
        public override bool CanConvertTo(
            ITypeDescriptorContext? typeDescriptorContext,
            Type? type)
        {
            if (type == typeof(InstanceDescriptor)
                || type == typeof(string))
            {
                return true;
            }
            else
            {
                return base.CanConvertTo(
                    typeDescriptorContext,
                    type);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override object? ConvertFrom(
            ITypeDescriptorContext? typeDescriptorContext,
            CultureInfo? cultureInfo,
            object value)
        {
            if (value is string stringValue)
            {
                stringValue = stringValue.Trim();

                if (stringValue == "Uniform")
                {
                    return KeyTime.Uniform;
                }
                else if (stringValue == "Paced")
                {
                    return KeyTime.Paced;
                }
                else if (stringValue[stringValue.Length - 1] == '%')
                {
                    stringValue = stringValue.TrimEnd('%');

                    float floatValue = (float)TypeDescriptor.GetConverter(
                        typeof(float)).ConvertFrom(
                            typeDescriptorContext,
                            cultureInfo,
                            stringValue)!;

                    if (floatValue == 0.0f)
                    {
                        return KeyTime.FromPercent(0.0f);
                    }
                    else if (floatValue == 100.0f)
                    {
                        return KeyTime.FromPercent(1.0f);
                    }
                    else
                    {
                        return KeyTime.FromPercent(floatValue / 100.0f);
                    }
                }
                else
                {
                    TimeSpan timeSpanValue = (TimeSpan)TypeDescriptor.GetConverter(
                        typeof(TimeSpan)).ConvertFrom(
                            typeDescriptorContext,
                            cultureInfo,
                            stringValue)!;
                    return KeyTime.FromTimeSpan(timeSpanValue);
                }
            }

            return base.ConvertFrom(
                typeDescriptorContext,
                cultureInfo,
                value);
        }

        /// <summary>
        /// 
        /// </summary>
        public override object? ConvertTo(
            ITypeDescriptorContext? typeDescriptorContext,
            CultureInfo? cultureInfo,
            object? value,
            Type destinationType)
        {
            if (value is KeyTime keyTime)
            {
                if (destinationType == typeof(InstanceDescriptor))
                {
                    MemberInfo mi;

                    switch (keyTime.Type)
                    {
                        case KeyTimeType.Percent:

                            mi = typeof(KeyTime).GetMethod("FromPercent", new Type[] { typeof(float) })!;

                            return new InstanceDescriptor(mi, new object[] { keyTime.Percent });

                        case KeyTimeType.TimeSpan:

                            mi = typeof(KeyTime).GetMethod("FromTimeSpan", new Type[] { typeof(TimeSpan) })!;

                            return new InstanceDescriptor(mi, new object[] { keyTime.TimeSpan });

                        case KeyTimeType.Uniform:

                            mi = typeof(KeyTime).GetProperty("Uniform")!;

                            return new InstanceDescriptor(mi, null);

                        case KeyTimeType.Paced:

                            mi = typeof(KeyTime).GetProperty("Paced")!;

                            return new InstanceDescriptor(mi, null);
                    }
                }
                else if (destinationType == typeof(String))
                {
                    switch (keyTime.Type)
                    {
                        case KeyTimeType.Uniform:

                            return "Uniform";

                        case KeyTimeType.Paced:

                            return "Paced";

                        case KeyTimeType.Percent:

                            string returnValue = (string)TypeDescriptor.GetConverter(
                                typeof(float)).ConvertTo(
                                    typeDescriptorContext,
                                    cultureInfo,
                                    keyTime.Percent * 100.0f,
                                    destinationType)!;

                            return string.Concat(returnValue, (ReadOnlySpan<char>)stackalloc char[] { '%' });

                        case KeyTimeType.TimeSpan:

                            return TypeDescriptor.GetConverter(
                                typeof(TimeSpan)).ConvertTo(
                                    typeDescriptorContext,
                                    cultureInfo,
                                    keyTime.TimeSpan,
                                    destinationType)!;
                    }
                }
            }

            return base.ConvertTo(
                typeDescriptorContext,
                cultureInfo,
                value,
                destinationType);
        }
    }
}
