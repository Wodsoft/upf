﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    /// <summary>
    /// Provides a type converter to convert Duration to and from other representations.
    /// </summary>
    public class DurationConverter : TypeConverter
    {
        /// <summary>
        /// CanConvertFrom - Returns whether or not this class can convert from a given type
        /// </summary>
        /// <ExternalAPI/>
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
        /// <ExternalAPI/>
        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        {
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
        /// ConvertFrom
        /// </summary>
        /// <ExternalAPI/>
        public override object ConvertFrom(
            ITypeDescriptorContext? td,
            CultureInfo? cultureInfo,
            object value)
        {
            string? stringValue = value as string;

            // Override the converter for sentinel values
            if (stringValue != null)
            {
                stringValue = stringValue.Trim();
                if (stringValue == "Automatic")
                {
                    return Duration.Automatic;
                }
                else if (stringValue == "Forever")
                {
                    return Duration.Forever;
                }
            }

            TimeSpan duration = TimeSpan.Zero;
            if (_TimeSpanConverter == null)
            {
                _TimeSpanConverter = new TimeSpanConverter();
            }
            duration = (TimeSpan)_TimeSpanConverter.ConvertFrom(td, cultureInfo, value)!;
            return new Duration(duration);
        }

        /// <summary>
        /// TypeConverter method implementation.
        /// </summary>
        /// <param name="context">ITypeDescriptorContext</param>
        /// <param name="cultureInfo">current culture (see CLR specs)</param>
        /// <param name="value">value to convert from</param>
        /// <param name="destinationType">Type to convert to</param>
        /// <returns>converted value</returns>
        /// <ExternalAPI/>
        public override object? ConvertTo(
            ITypeDescriptorContext? context,
            CultureInfo? cultureInfo,
            object? value,
            Type destinationType)
        {
            if (value is Duration durationValue)
            {
                if (destinationType == typeof(InstanceDescriptor))
                {
                    MemberInfo? mi;

                    if (durationValue.HasTimeSpan)
                    {
                        mi = typeof(Duration).GetConstructor(new Type[] { typeof(TimeSpan) });

                        return new InstanceDescriptor(mi, new object[] { durationValue.TimeSpan });
                    }
                    else if (durationValue == Duration.Forever)
                    {
                        mi = typeof(Duration).GetProperty("Forever");

                        return new InstanceDescriptor(mi, null);
                    }
                    else
                    {
                        Debug.Assert(durationValue == Duration.Automatic);  // Only other legal duration type

                        mi = typeof(Duration).GetProperty("Automatic");

                        return new InstanceDescriptor(mi, null);
                    }
                }
                else if (destinationType == typeof(string))
                {
                    return durationValue.ToString();
                }
            }

            // Pass unhandled cases to base class (which will throw exceptions for null value or destinationType.)
            return base.ConvertTo(context, cultureInfo, value, destinationType);
        }
        private static TimeSpanConverter? _TimeSpanConverter;
    }
}
