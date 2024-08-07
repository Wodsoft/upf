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

namespace Wodsoft.UI.Media.Animation
{
    public sealed class RepeatBehaviorConverter : TypeConverter
    {
        #region Data

        private const char _IterationCharacter = 'x';

        #endregion

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
            if (value is string stringValue)
            {
                stringValue = stringValue.Trim();

                if (stringValue == "Forever")
                {
                    return RepeatBehavior.Forever;
                }
                else if (stringValue.Length > 0
                         && stringValue[stringValue.Length - 1] == _IterationCharacter)
                {
                    string stringFloatValue = stringValue.TrimEnd(_IterationCharacter);

                    float floatValue = (float)TypeDescriptor.GetConverter(typeof(float)).ConvertFrom(td, cultureInfo, stringFloatValue)!;

                    return new RepeatBehavior(floatValue);
                }
            }

            // The value is not Forever or an iteration count so it's either a TimeSpan
            // or we'll let the TimeSpanConverter raise the appropriate exception.

            TimeSpan timeSpanValue = (TimeSpan)TypeDescriptor.GetConverter(typeof(TimeSpan)).ConvertFrom(td, cultureInfo, value)!;

            return new RepeatBehavior(timeSpanValue);
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
            if (value is RepeatBehavior repeatBehavior)
            {
                if (destinationType == typeof(InstanceDescriptor))
                {
                    MemberInfo? mi;

                    if (repeatBehavior == RepeatBehavior.Forever)
                    {
                        mi = typeof(RepeatBehavior).GetProperty("Forever");

                        return new InstanceDescriptor(mi, null);
                    }
                    else if (repeatBehavior.HasCount)
                    {
                        mi = typeof(RepeatBehavior).GetConstructor(new Type[] { typeof(double) });

                        return new InstanceDescriptor(mi, new object[] { repeatBehavior.Count });
                    }
                    else if (repeatBehavior.HasDuration)
                    {
                        mi = typeof(RepeatBehavior).GetConstructor(new Type[] { typeof(TimeSpan) });

                        return new InstanceDescriptor(mi, new object[] { repeatBehavior.Duration });
                    }
                    else
                    {
                        Debug.Fail("Unknown type of RepeatBehavior passed to RepeatBehaviorConverter.");
                    }
                }
                else if (destinationType == typeof(string))
                {
                    return repeatBehavior.InternalToString(null, cultureInfo);
                }
            }

            // We can't do the conversion, let the base class raise the
            // appropriate exception.

            return base.ConvertTo(context, cultureInfo, value, destinationType);
        }
    }
}
