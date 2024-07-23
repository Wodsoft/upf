using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    internal static class Parsers
    {
        public static Brush ParseBrush(string brush, IFormatProvider? formatProvider, ITypeDescriptorContext? context)
        {
            bool isPossibleKnownColor;
            bool isNumericColor;
            bool isScRgbColor;
            string trimmedColor = KnownColors.MatchColor(brush, out isPossibleKnownColor, out isNumericColor, out isScRgbColor);

            // Note that because trimmedColor is exactly brush.Trim() we don't have to worry about
            // extra tokens as we do with TokenizerHelper.  If we return one of the solid color
            // brushes then the ParseColor routine (or ColorStringToKnownColor) matched the entire
            // input.
            if (isNumericColor)
            {
                return (new SolidColorBrush(ParseHexColor(trimmedColor)));
            }

            if (isScRgbColor)
            {
                return (new SolidColorBrush(ParseScRgbColor(trimmedColor, formatProvider)));
            }

            if (isPossibleKnownColor)
            {
                SolidColorBrush scp = KnownColors.ColorStringToKnownBrush(trimmedColor);

                if (scp != null)
                {
                    return scp;
                }
            }

            // If it's not a color, so the content is illegal.
            throw new FormatException($"Unknown brush \"{brush}\".");
        }

        public static Color ParseHexColor(string trimmedColor)
        {
            int a, r, g, b;
            a = 255;

            if (trimmedColor.Length > 7)
            {
                a = ParseHexChar(trimmedColor[1]) * 16 + ParseHexChar(trimmedColor[2]);
                r = ParseHexChar(trimmedColor[3]) * 16 + ParseHexChar(trimmedColor[4]);
                g = ParseHexChar(trimmedColor[5]) * 16 + ParseHexChar(trimmedColor[6]);
                b = ParseHexChar(trimmedColor[7]) * 16 + ParseHexChar(trimmedColor[8]);
            }
            else if (trimmedColor.Length > 5)
            {
                r = ParseHexChar(trimmedColor[1]) * 16 + ParseHexChar(trimmedColor[2]);
                g = ParseHexChar(trimmedColor[3]) * 16 + ParseHexChar(trimmedColor[4]);
                b = ParseHexChar(trimmedColor[5]) * 16 + ParseHexChar(trimmedColor[6]);
            }
            else if (trimmedColor.Length > 4)
            {
                a = ParseHexChar(trimmedColor[1]);
                a = a + a * 16;
                r = ParseHexChar(trimmedColor[2]);
                r = r + r * 16;
                g = ParseHexChar(trimmedColor[3]);
                g = g + g * 16;
                b = ParseHexChar(trimmedColor[4]);
                b = b + b * 16;
            }
            else
            {
                r = ParseHexChar(trimmedColor[1]);
                r = r + r * 16;
                g = ParseHexChar(trimmedColor[2]);
                g = g + g * 16;
                b = ParseHexChar(trimmedColor[3]);
                b = b + b * 16;
            }

            return (Color.FromArgb((byte)a, (byte)r, (byte)g, (byte)b));
        }


        private const int _zeroChar = (int)'0';
        private const int _aLower = (int)'a';
        private const int _aUpper = (int)'A';
        public static int ParseHexChar(char c)
        {
            int intChar = (int)c;

            if ((intChar >= _zeroChar) && (intChar <= (_zeroChar + 9)))
            {
                return (intChar - _zeroChar);
            }

            if ((intChar >= _aLower) && (intChar <= (_aLower + 5)))
            {
                return (intChar - _aLower + 10);
            }

            if ((intChar >= _aUpper) && (intChar <= (_aUpper + 5)))
            {
                return (intChar - _aUpper + 10);
            }
            throw new FormatException($"Invalid hex char \"{c}\".");
        }

        public static Color ParseScRgbColor(string trimmedColor, IFormatProvider? formatProvider)
        {
            string tokens = trimmedColor.Substring(3, trimmedColor.Length - 3);

            // The tokenizer helper will tokenize a list based on the IFormatProvider.
            TokenizerHelper th = new TokenizerHelper(tokens, formatProvider);
            float[] values = new float[4];

            for (int i = 0; i < 3; i++)
            {
                values[i] = Convert.ToSingle(th.NextTokenRequired(), formatProvider);
            }

            if (th.NextToken())
            {
                values[3] = Convert.ToSingle(th.GetCurrentToken(), formatProvider);

                // We should be out of tokens at this point
                if (th.NextToken())
                {
                    throw new FormatException($"Invalid sc rgb color \"{trimmedColor}\".");
                }

                return Color.FromScRgb(values[0], values[1], values[2], values[3]);
            }
            else
            {
                return Color.FromScRgb(1.0f, values[0], values[1], values[2]);
            }
        }
        public static Color ParseColor(string color, IFormatProvider formatProvider)
        {
            return ParseColor(color, formatProvider, null);
        }

        /// <summary>
        /// ParseColor
        /// <param name="color"> string with color description </param>
        /// <param name="formatProvider">IFormatProvider for processing string</param>
        /// <param name="context">ITypeDescriptorContext</param>
        /// </summary>
        public static Color ParseColor(string color, IFormatProvider formatProvider, ITypeDescriptorContext? context)
        {
            bool isPossibleKnowColor;
            bool isNumericColor;
            bool isScRgbColor;
            string trimmedColor = KnownColors.MatchColor(color, out isPossibleKnowColor, out isNumericColor, out isScRgbColor);

            if ((isPossibleKnowColor == false) &&
                (isNumericColor == false) &&
                (isScRgbColor == false))
            {
                throw new FormatException("Illegal color token.");
            }

            //Is it a number?
            if (isNumericColor)
            {
                return ParseHexColor(trimmedColor);
            }
            else if (isScRgbColor)
            {
                return ParseScRgbColor(trimmedColor, formatProvider);
            }
            else
            {
                KnownColor kc = KnownColors.ColorStringToKnownColor(trimmedColor);

                if (kc == KnownColor.UnknownColor)
                {
                    throw new FormatException("Illegal color token.");
                }

                return Color.FromUInt32((uint)kc);
            }
        }
    }
}
