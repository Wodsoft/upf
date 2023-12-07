using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Controls;

namespace Wodsoft.UI.Markup
{
    /// <summary>
    ///     XamlGridLengthSerializer is used to persist a GridLength structure in Baml files
    /// </summary>
    internal class XamlGridLengthSerializer //: XamlSerializer
    {
        #region Construction

        /// <summary>
        ///     Constructor for XamlGridLengthSerializer
        /// </summary>
        /// <remarks>
        ///     This constructor will be used under 
        ///     the following two scenarios
        ///     1. Convert a string to a custom binary representation stored in BAML
        ///     2. Convert a custom binary representation back into a GridLength
        /// </remarks>
        private XamlGridLengthSerializer()
        {
        }


        #endregion Construction

        #region Conversions

        ///<summary>
        /// Serializes this object using the passed writer.
        ///</summary>
        /// <remarks>
        /// This is called ONLY from the Parser and is not a general public method. 
        /// </remarks>
        //
        //  Format of serialized data:
        //  first byte   other bytes      format
        //  0AAAAAAA     none             Amount [0 - 127] in AAAAAAA, Pixel GridUnitType
        //  100XXUUU     one byte         Amount in byte [0 - 255], GridUnitType in UUU
        //  110XXUUU     two bytes        Amount in int16 , GridUnitType in UUU
        //  101XXUUU     four bytes       Amount in int32 , GridUnitType in UUU
        //  111XXUUU     eight bytes      Amount in double, GridUnitType in UUU
        //
        public bool ConvertStringToCustomBinary(
            BinaryWriter writer,           // Writer into the baml stream
            string stringValue)      // String to convert
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            GridUnitType gridUnitType;
            float value;
            FromString(stringValue, CultureInfo.InvariantCulture,
                        out value, out gridUnitType);

            byte unitAndFlags = (byte)gridUnitType;
            int intAmount = (int)value;

            if ((float)intAmount == value)
            {
                //
                //  0 - 127 and Pixel
                //
                if (intAmount <= 127
                    && intAmount >= 0
                    && gridUnitType == GridUnitType.Pixel)
                {
                    writer.Write((byte)intAmount);
                }
                //
                //  unsigned byte
                //
                else if (intAmount <= 255
                        && intAmount >= 0)
                {
                    writer.Write((byte)(0x80 | unitAndFlags));
                    writer.Write((byte)intAmount);
                }
                //
                //  signed short integer
                //
                else if (intAmount <= 32767
                        && intAmount >= -32768)
                {
                    writer.Write((byte)(0xC0 | unitAndFlags));
                    writer.Write((Int16)intAmount);
                }
                //
                //  signed integer
                //
                else
                {
                    writer.Write((byte)(0xA0 | unitAndFlags));
                    writer.Write(intAmount);
                }
            }
            //
            //  double
            //
            else
            {
                writer.Write((byte)(0xE0 | unitAndFlags));
                writer.Write(value);
            }

            return true;
        }

        /// <summary>
        ///   Convert a compact binary representation of a GridLength into and instance
        ///   of GridLength.  The reader must be left pointing immediately after the object 
        ///   data in the underlying stream.
        /// </summary>
        /// <remarks>
        /// This is called ONLY from the Parser and is not a general public method. 
        /// </remarks>
        public object ConvertCustomBinaryToObject(
            BinaryReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            GridUnitType unitType;
            float unitValue;
            byte unitAndFlags = reader.ReadByte();

            if ((unitAndFlags & 0x80) == 0)
            {
                unitType = GridUnitType.Pixel;
                unitValue = unitAndFlags;
            }
            else
            {
                unitType = (GridUnitType)(unitAndFlags & 0x1F);
                byte flags = (byte)(unitAndFlags & 0xE0);

                if (flags == 0x80)
                {
                    unitValue = reader.ReadByte();
                }
                else if (flags == 0xC0)
                {
                    unitValue = reader.ReadInt16();
                }
                else if (flags == 0xA0)
                {
                    unitValue = reader.ReadInt32();
                }
                else
                {
                    unitValue = reader.ReadSingle();
                }
            }
            return new GridLength(unitValue, unitType);
        }



        // Parse a GridLength from a string given the CultureInfo.
        static internal void FromString(
                string s,
                CultureInfo? cultureInfo,
            out float value,
            out GridUnitType unit)
        {
            string goodString = s.Trim().ToLowerInvariant();

            value = 0.0f;
            unit = GridUnitType.Pixel;

            int i;
            int strLen = goodString.Length;
            int strLenUnit = 0;
            float unitFactor = 1.0f;

            //  this is where we would handle trailing whitespace on the input string.
            //  peel [unit] off the end of the string
            i = 0;

            if (goodString == _UnitStrings[i])
            {
                strLenUnit = _UnitStrings[i].Length;
                unit = (GridUnitType)i;
            }
            else
            {
                for (i = 1; i < _UnitStrings.Length; ++i)
                {
                    //  Note: this is NOT a culture specific comparison.
                    //  this is by design: we want the same unit string table to work across all cultures.
                    if (goodString.EndsWith(_UnitStrings[i], StringComparison.Ordinal))
                    {
                        strLenUnit = _UnitStrings[i].Length;
                        unit = (GridUnitType)i;
                        break;
                    }
                }
            }

            //  we couldn't match a real unit from GridUnitTypes.
            //  try again with a converter-only unit (a pixel equivalent).
            if (i >= _UnitStrings.Length)
            {
                for (i = 0; i < _PixelUnitStrings.Length; ++i)
                {
                    //  Note: this is NOT a culture specific comparison.
                    //  this is by design: we want the same unit string table to work across all cultures.
                    if (goodString.EndsWith(_PixelUnitStrings[i], StringComparison.Ordinal))
                    {
                        strLenUnit = _PixelUnitStrings[i].Length;
                        unitFactor = _PixelUnitFactors[i];
                        break;
                    }
                }
            }

            //  this is where we would handle leading whitespace on the input string.
            //  this is also where we would handle whitespace between [value] and [unit].
            //  check if we don't have a [value].  This is acceptable for certain UnitTypes.
            if (strLen == strLenUnit
                && (unit == GridUnitType.Auto
                    || unit == GridUnitType.Star))
            {
                value = 1;
            }
            //  we have a value to parse.
            else
            {
                ReadOnlySpan<char> valueString = goodString.AsSpan(0, strLen - strLenUnit);
                value = float.Parse(valueString, provider: cultureInfo) * unitFactor;
            }
        }


        #endregion Conversions

        #region Fields

        //  Note: keep this array in sync with the GridUnitType enum
        static private string[] _UnitStrings = { "auto", "px", "*" };

        //  this array contains strings for unit types that are not present in the GridUnitType enum
        static private string[] _PixelUnitStrings = { "in", "cm", "pt" };
        static private float[] _PixelUnitFactors =
        {
            96.0f,             // Pixels per Inch
            96.0f / 2.54f,      // Pixels per Centimeter
            96.0f / 72.0f,      // Pixels per Point
        };

        #endregion Fields
    }
}
