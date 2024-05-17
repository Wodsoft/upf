using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Input
{
    /// <summary>
    /// TypeConverter to convert CursorType to/from other types.
    /// Currently To/From string is only supported.
    /// </summary>
    public class CursorConverter : TypeConverter
    {
        /// <summary>
        /// TypeConverter method override.
        /// </summary>
        /// <param name="context">ITypeDescriptorContext</param>
        /// <param name="sourceType">Type to convert from</param>
        /// <returns>true if conversion is possible</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// TypeConverter method override.
        /// </summary>
        /// <param name="context">ITypeDescriptorContext</param>
        /// <param name="destinationType">Type to convert to</param>
        /// <returns>true if conversion is possible</returns>
        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        {
            if (destinationType == typeof(string))
                return true;
            return false;
        }

        /// <summary>
        ///     Gets the public/static properties of the Cursors class
        /// </summary>
        /// <returns>PropertyInfo array of the objects properties</returns>
        private PropertyInfo[] GetProperties()
        {
            return typeof(Cursors).GetProperties(BindingFlags.Public | BindingFlags.Static);
        }

        /// <summary>
        ///     StandardValuesCollection method override
        /// </summary>
        /// <param name="context">ITypeDescriptorContext</param>
        /// <returns>TypeConverter.StandardValuesCollection</returns>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext? context)
        {
            if (_standardValues == null)
            {
                ArrayList list1 = new ArrayList();
                PropertyInfo[] infoArray1 = GetProperties();
                for (int num1 = 0; num1 < infoArray1.Length; num1++)
                {
                    PropertyInfo info1 = infoArray1[num1];
                    object[]? objArray1 = null;
                    list1.Add(info1.GetValue(null, objArray1));
                }
                _standardValues = new StandardValuesCollection(list1.ToArray());
            }
            return _standardValues;
        }

        /// <summary>
        ///     Returns whether this object supports a standard set of values that can be
        ///     picked from a list, using the specified context.
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
        /// <returns>
        ///     true if GetStandardValues should be called to find a common set
        ///     of values the object supports; otherwise, false.
        /// </returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext? context)
        {
            return true;
        }

        /// <summary>
        /// TypeConverter method implementation.
        /// </summary>
        /// <param name="context">ITypeDescriptorContext</param>
        /// <param name="culture">current culture (see CLR specs)</param>
        /// <param name="value">value to convert from</param>
        /// <returns>value that is result of conversion</returns>
        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is string text)
            {
                text = text.Trim();

                if (text != string.Empty)
                {
                    if (Enum.TryParse<CursorType>(text, out CursorType cursorType))
                    {
                        switch (cursorType)
                        {
                            case CursorType.Arrow:
                                return Cursors.Arrow;
                            case CursorType.AppStarting:
                                return Cursors.AppStarting;
                            case CursorType.Cross:
                                return Cursors.Cross;
                            case CursorType.Help:
                                return Cursors.Help;
                            case CursorType.IBeam:
                                return Cursors.IBeam;
                            case CursorType.SizeAll:
                                return Cursors.SizeAll;
                            case CursorType.SizeNESW:
                                return Cursors.SizeNESW;
                            case CursorType.SizeNS:
                                return Cursors.SizeNS;
                            case CursorType.SizeNWSE:
                                return Cursors.SizeNWSE;
                            case CursorType.SizeWE:
                                return Cursors.SizeWE;
                            case CursorType.UpArrow:
                                return Cursors.UpArrow;
                            case CursorType.Wait:
                                return Cursors.Wait;
                            case CursorType.Hand:
                                return Cursors.Hand;
                            case CursorType.No:
                                return Cursors.No;
                            case CursorType.None:
                                return Cursors.None;
                            case CursorType.Pen:
                                return Cursors.Pen;
                            case CursorType.ScrollNS:
                                return Cursors.ScrollNS;
                            case CursorType.ScrollWE:
                                return Cursors.ScrollWE;
                            case CursorType.ScrollAll:
                                return Cursors.ScrollAll;
                            case CursorType.ScrollN:
                                return Cursors.ScrollN;
                            case CursorType.ScrollS:
                                return Cursors.ScrollS;
                            case CursorType.ScrollW:
                                return Cursors.ScrollW;
                            case CursorType.ScrollE:
                                return Cursors.ScrollE;
                            case CursorType.ScrollNW:
                                return Cursors.ScrollNW;
                            case CursorType.ScrollNE:
                                return Cursors.ScrollNE;
                            case CursorType.ScrollSW:
                                return Cursors.ScrollSW;
                            case CursorType.ScrollSE:
                                return Cursors.ScrollSE;
                            case CursorType.ArrowCD:
                                return Cursors.ArrowCD;
                            case CursorType.Custom:
                                throw new NotSupportedException("Can't use custom cursor directly.");
                            default:
                                throw new NotSupportedException("Not supported cursor type.");
                        }
                    }
                    return new Cursor(text);
                }
                else
                {
                    // An empty string means no cursor.
                    return null;
                }
            }
            throw GetConvertFromException(value);
        }

        /// <summary>
        /// TypeConverter method implementation.
        /// </summary>
        /// <param name="context">ITypeDescriptorContext</param>
        /// <param name="culture">current culture (see CLR specs)</param>
        /// <param name="value">value to convert from</param>
        /// <param name="destinationType">Type to convert to</param>
        /// <returns>converted value</returns>
        public override object ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }

            // If value is not a Cursor or null, it will throw GetConvertToException.
            if (destinationType == typeof(string))
            {
                if (value is Cursor cursor)
                {
                    return cursor.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            throw GetConvertToException(value, destinationType);
        }

        /// <summary>
        /// Cached value for GetStandardValues
        /// </summary>
        private StandardValuesCollection? _standardValues;
    }
}
