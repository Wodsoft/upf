using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    internal enum KnownColor : uint
    {
        // We've reserved the value "1" as unknown.  If for some odd reason "1" is added to the
        // list, redefined UnknownColor

        AliceBlue = 0xFFF0F8FF,
        AntiqueWhite = 0xFFFAEBD7,
        Aqua = 0xFF00FFFF,
        Aquamarine = 0xFF7FFFD4,
        Azure = 0xFFF0FFFF,
        Beige = 0xFFF5F5DC,
        Bisque = 0xFFFFE4C4,
        Black = 0xFF000000,
        BlanchedAlmond = 0xFFFFEBCD,
        Blue = 0xFF0000FF,
        BlueViolet = 0xFF8A2BE2,
        Brown = 0xFFA52A2A,
        BurlyWood = 0xFFDEB887,
        CadetBlue = 0xFF5F9EA0,
        Chartreuse = 0xFF7FFF00,
        Chocolate = 0xFFD2691E,
        Coral = 0xFFFF7F50,
        CornflowerBlue = 0xFF6495ED,
        Cornsilk = 0xFFFFF8DC,
        Crimson = 0xFFDC143C,
        Cyan = 0xFF00FFFF,
        DarkBlue = 0xFF00008B,
        DarkCyan = 0xFF008B8B,
        DarkGoldenrod = 0xFFB8860B,
        DarkGray = 0xFFA9A9A9,
        DarkGreen = 0xFF006400,
        DarkKhaki = 0xFFBDB76B,
        DarkMagenta = 0xFF8B008B,
        DarkOliveGreen = 0xFF556B2F,
        DarkOrange = 0xFFFF8C00,
        DarkOrchid = 0xFF9932CC,
        DarkRed = 0xFF8B0000,
        DarkSalmon = 0xFFE9967A,
        DarkSeaGreen = 0xFF8FBC8F,
        DarkSlateBlue = 0xFF483D8B,
        DarkSlateGray = 0xFF2F4F4F,
        DarkTurquoise = 0xFF00CED1,
        DarkViolet = 0xFF9400D3,
        DeepPink = 0xFFFF1493,
        DeepSkyBlue = 0xFF00BFFF,
        DimGray = 0xFF696969,
        DodgerBlue = 0xFF1E90FF,
        Firebrick = 0xFFB22222,
        FloralWhite = 0xFFFFFAF0,
        ForestGreen = 0xFF228B22,
        Fuchsia = 0xFFFF00FF,
        Gainsboro = 0xFFDCDCDC,
        GhostWhite = 0xFFF8F8FF,
        Gold = 0xFFFFD700,
        Goldenrod = 0xFFDAA520,
        Gray = 0xFF808080,
        Green = 0xFF008000,
        GreenYellow = 0xFFADFF2F,
        Honeydew = 0xFFF0FFF0,
        HotPink = 0xFFFF69B4,
        IndianRed = 0xFFCD5C5C,
        Indigo = 0xFF4B0082,
        Ivory = 0xFFFFFFF0,
        Khaki = 0xFFF0E68C,
        Lavender = 0xFFE6E6FA,
        LavenderBlush = 0xFFFFF0F5,
        LawnGreen = 0xFF7CFC00,
        LemonChiffon = 0xFFFFFACD,
        LightBlue = 0xFFADD8E6,
        LightCoral = 0xFFF08080,
        LightCyan = 0xFFE0FFFF,
        LightGoldenrodYellow = 0xFFFAFAD2,
        LightGreen = 0xFF90EE90,
        LightGray = 0xFFD3D3D3,
        LightPink = 0xFFFFB6C1,
        LightSalmon = 0xFFFFA07A,
        LightSeaGreen = 0xFF20B2AA,
        LightSkyBlue = 0xFF87CEFA,
        LightSlateGray = 0xFF778899,
        LightSteelBlue = 0xFFB0C4DE,
        LightYellow = 0xFFFFFFE0,
        Lime = 0xFF00FF00,
        LimeGreen = 0xFF32CD32,
        Linen = 0xFFFAF0E6,
        Magenta = 0xFFFF00FF,
        Maroon = 0xFF800000,
        MediumAquamarine = 0xFF66CDAA,
        MediumBlue = 0xFF0000CD,
        MediumOrchid = 0xFFBA55D3,
        MediumPurple = 0xFF9370DB,
        MediumSeaGreen = 0xFF3CB371,
        MediumSlateBlue = 0xFF7B68EE,
        MediumSpringGreen = 0xFF00FA9A,
        MediumTurquoise = 0xFF48D1CC,
        MediumVioletRed = 0xFFC71585,
        MidnightBlue = 0xFF191970,
        MintCream = 0xFFF5FFFA,
        MistyRose = 0xFFFFE4E1,
        Moccasin = 0xFFFFE4B5,
        NavajoWhite = 0xFFFFDEAD,
        Navy = 0xFF000080,
        OldLace = 0xFFFDF5E6,
        Olive = 0xFF808000,
        OliveDrab = 0xFF6B8E23,
        Orange = 0xFFFFA500,
        OrangeRed = 0xFFFF4500,
        Orchid = 0xFFDA70D6,
        PaleGoldenrod = 0xFFEEE8AA,
        PaleGreen = 0xFF98FB98,
        PaleTurquoise = 0xFFAFEEEE,
        PaleVioletRed = 0xFFDB7093,
        PapayaWhip = 0xFFFFEFD5,
        PeachPuff = 0xFFFFDAB9,
        Peru = 0xFFCD853F,
        Pink = 0xFFFFC0CB,
        Plum = 0xFFDDA0DD,
        PowderBlue = 0xFFB0E0E6,
        Purple = 0xFF800080,
        Red = 0xFFFF0000,
        RosyBrown = 0xFFBC8F8F,
        RoyalBlue = 0xFF4169E1,
        SaddleBrown = 0xFF8B4513,
        Salmon = 0xFFFA8072,
        SandyBrown = 0xFFF4A460,
        SeaGreen = 0xFF2E8B57,
        SeaShell = 0xFFFFF5EE,
        Sienna = 0xFFA0522D,
        Silver = 0xFFC0C0C0,
        SkyBlue = 0xFF87CEEB,
        SlateBlue = 0xFF6A5ACD,
        SlateGray = 0xFF708090,
        Snow = 0xFFFFFAFA,
        SpringGreen = 0xFF00FF7F,
        SteelBlue = 0xFF4682B4,
        Tan = 0xFFD2B48C,
        Teal = 0xFF008080,
        Thistle = 0xFFD8BFD8,
        Tomato = 0xFFFF6347,
        Transparent = 0x00FFFFFF,
        Turquoise = 0xFF40E0D0,
        Violet = 0xFFEE82EE,
        Wheat = 0xFFF5DEB3,
        White = 0xFFFFFFFF,
        WhiteSmoke = 0xFFF5F5F5,
        Yellow = 0xFFFFFF00,
        YellowGreen = 0xFF9ACD32,
        UnknownColor = 0x00000001
    }

    internal class KnownColors
    {
        public static ConcurrentDictionary<uint, SolidColorBrush> _SolidColorBrushCache = new ConcurrentDictionary<uint, SolidColorBrush>();

        static internal string MatchColor(string colorString, out bool isKnownColor, out bool isNumericColor, out bool isScRgbColor)
        {
            string trimmedString = colorString.Trim();

            if (((trimmedString.Length == 4) ||
                (trimmedString.Length == 5) ||
                (trimmedString.Length == 7) ||
                (trimmedString.Length == 9)) &&
                (trimmedString[0] == '#'))
            {
                isNumericColor = true;
                isScRgbColor = false;
                isKnownColor = false;
                return trimmedString;
            }
            else
                isNumericColor = false;

            if ((trimmedString.StartsWith("sc#", StringComparison.Ordinal) == true))
            {
                isNumericColor = false;
                isScRgbColor = true;
                isKnownColor = false;
            }
            else
            {
                isScRgbColor = false;
            }

            isKnownColor = true;
            return trimmedString;
        }

        public static SolidColorBrush ColorStringToKnownBrush(string s)
        {
            if (null != s)
            {
                KnownColor result = ColorStringToKnownColor(s);

                // If the result is UnknownColor, that means this string wasn't found
                if (result != KnownColor.UnknownColor)
                {
                    // Otherwise, return the appropriate SolidColorBrush
                    return SolidColorBrushFromUint((uint)result);
                }
            }
            return null;
        }

        public static SolidColorBrush SolidColorBrushFromUint(uint argb)
        {
            return _SolidColorBrushCache.GetOrAdd(argb, key =>
            {
                var brush = new SolidColorBrush(Color.FromUInt32(argb));
                brush.Freeze();
                return brush;
            });
        }

        internal static KnownColor ColorStringToKnownColor(string colorString)
        {
            if (null != colorString)
            {
                // We use invariant culture because we don't globalize our color names
                string colorUpper = colorString.ToUpper(System.Globalization.CultureInfo.InvariantCulture);

                // Use String.Equals because it does explicit equality
                // StartsWith/EndsWith are culture sensitive and are 4-7 times slower than Equals

                switch (colorUpper.Length)
                {
                    case 3:
                        if (colorUpper.Equals("RED")) return KnownColor.Red;
                        if (colorUpper.Equals("TAN")) return KnownColor.Tan;
                        break;
                    case 4:
                        switch (colorUpper[0])
                        {
                            case 'A':
                                if (colorUpper.Equals("AQUA")) return KnownColor.Aqua;
                                break;
                            case 'B':
                                if (colorUpper.Equals("BLUE")) return KnownColor.Blue;
                                break;
                            case 'C':
                                if (colorUpper.Equals("CYAN")) return KnownColor.Cyan;
                                break;
                            case 'G':
                                if (colorUpper.Equals("GOLD")) return KnownColor.Gold;
                                if (colorUpper.Equals("GRAY")) return KnownColor.Gray;
                                break;
                            case 'L':
                                if (colorUpper.Equals("LIME")) return KnownColor.Lime;
                                break;
                            case 'N':
                                if (colorUpper.Equals("NAVY")) return KnownColor.Navy;
                                break;
                            case 'P':
                                if (colorUpper.Equals("PERU")) return KnownColor.Peru;
                                if (colorUpper.Equals("PINK")) return KnownColor.Pink;
                                if (colorUpper.Equals("PLUM")) return KnownColor.Plum;
                                break;
                            case 'S':
                                if (colorUpper.Equals("SNOW")) return KnownColor.Snow;
                                break;
                            case 'T':
                                if (colorUpper.Equals("TEAL")) return KnownColor.Teal;
                                break;
                        }
                        break;
                    case 5:
                        switch (colorUpper[0])
                        {
                            case 'A':
                                if (colorUpper.Equals("AZURE")) return KnownColor.Azure;
                                break;
                            case 'B':
                                if (colorUpper.Equals("BEIGE")) return KnownColor.Beige;
                                if (colorUpper.Equals("BLACK")) return KnownColor.Black;
                                if (colorUpper.Equals("BROWN")) return KnownColor.Brown;
                                break;
                            case 'C':
                                if (colorUpper.Equals("CORAL")) return KnownColor.Coral;
                                break;
                            case 'G':
                                if (colorUpper.Equals("GREEN")) return KnownColor.Green;
                                break;
                            case 'I':
                                if (colorUpper.Equals("IVORY")) return KnownColor.Ivory;
                                break;
                            case 'K':
                                if (colorUpper.Equals("KHAKI")) return KnownColor.Khaki;
                                break;
                            case 'L':
                                if (colorUpper.Equals("LINEN")) return KnownColor.Linen;
                                break;
                            case 'O':
                                if (colorUpper.Equals("OLIVE")) return KnownColor.Olive;
                                break;
                            case 'W':
                                if (colorUpper.Equals("WHEAT")) return KnownColor.Wheat;
                                if (colorUpper.Equals("WHITE")) return KnownColor.White;
                                break;
                        }
                        break;
                    case 6:
                        switch (colorUpper[0])
                        {
                            case 'B':
                                if (colorUpper.Equals("BISQUE")) return KnownColor.Bisque;
                                break;
                            case 'I':
                                if (colorUpper.Equals("INDIGO")) return KnownColor.Indigo;
                                break;
                            case 'M':
                                if (colorUpper.Equals("MAROON")) return KnownColor.Maroon;
                                break;
                            case 'O':
                                if (colorUpper.Equals("ORANGE")) return KnownColor.Orange;
                                if (colorUpper.Equals("ORCHID")) return KnownColor.Orchid;
                                break;
                            case 'P':
                                if (colorUpper.Equals("PURPLE")) return KnownColor.Purple;
                                break;
                            case 'S':
                                if (colorUpper.Equals("SALMON")) return KnownColor.Salmon;
                                if (colorUpper.Equals("SIENNA")) return KnownColor.Sienna;
                                if (colorUpper.Equals("SILVER")) return KnownColor.Silver;
                                break;
                            case 'T':
                                if (colorUpper.Equals("TOMATO")) return KnownColor.Tomato;
                                break;
                            case 'V':
                                if (colorUpper.Equals("VIOLET")) return KnownColor.Violet;
                                break;
                            case 'Y':
                                if (colorUpper.Equals("YELLOW")) return KnownColor.Yellow;
                                break;
                        }
                        break;
                    case 7:
                        switch (colorUpper[0])
                        {
                            case 'C':
                                if (colorUpper.Equals("CRIMSON")) return KnownColor.Crimson;
                                break;
                            case 'D':
                                if (colorUpper.Equals("DARKRED")) return KnownColor.DarkRed;
                                if (colorUpper.Equals("DIMGRAY")) return KnownColor.DimGray;
                                break;
                            case 'F':
                                if (colorUpper.Equals("FUCHSIA")) return KnownColor.Fuchsia;
                                break;
                            case 'H':
                                if (colorUpper.Equals("HOTPINK")) return KnownColor.HotPink;
                                break;
                            case 'M':
                                if (colorUpper.Equals("MAGENTA")) return KnownColor.Magenta;
                                break;
                            case 'O':
                                if (colorUpper.Equals("OLDLACE")) return KnownColor.OldLace;
                                break;
                            case 'S':
                                if (colorUpper.Equals("SKYBLUE")) return KnownColor.SkyBlue;
                                break;
                            case 'T':
                                if (colorUpper.Equals("THISTLE")) return KnownColor.Thistle;
                                break;
                        }
                        break;
                    case 8:
                        switch (colorUpper[0])
                        {
                            case 'C':
                                if (colorUpper.Equals("CORNSILK")) return KnownColor.Cornsilk;
                                break;
                            case 'D':
                                if (colorUpper.Equals("DARKBLUE")) return KnownColor.DarkBlue;
                                if (colorUpper.Equals("DARKCYAN")) return KnownColor.DarkCyan;
                                if (colorUpper.Equals("DARKGRAY")) return KnownColor.DarkGray;
                                if (colorUpper.Equals("DEEPPINK")) return KnownColor.DeepPink;
                                break;
                            case 'H':
                                if (colorUpper.Equals("HONEYDEW")) return KnownColor.Honeydew;
                                break;
                            case 'L':
                                if (colorUpper.Equals("LAVENDER")) return KnownColor.Lavender;
                                break;
                            case 'M':
                                if (colorUpper.Equals("MOCCASIN")) return KnownColor.Moccasin;
                                break;
                            case 'S':
                                if (colorUpper.Equals("SEAGREEN")) return KnownColor.SeaGreen;
                                if (colorUpper.Equals("SEASHELL")) return KnownColor.SeaShell;
                                break;
                        }
                        break;
                    case 9:
                        switch (colorUpper[0])
                        {
                            case 'A':
                                if (colorUpper.Equals("ALICEBLUE")) return KnownColor.AliceBlue;
                                break;
                            case 'B':
                                if (colorUpper.Equals("BURLYWOOD")) return KnownColor.BurlyWood;
                                break;
                            case 'C':
                                if (colorUpper.Equals("CADETBLUE")) return KnownColor.CadetBlue;
                                if (colorUpper.Equals("CHOCOLATE")) return KnownColor.Chocolate;
                                break;
                            case 'D':
                                if (colorUpper.Equals("DARKGREEN")) return KnownColor.DarkGreen;
                                if (colorUpper.Equals("DARKKHAKI")) return KnownColor.DarkKhaki;
                                break;
                            case 'F':
                                if (colorUpper.Equals("FIREBRICK")) return KnownColor.Firebrick;
                                break;
                            case 'G':
                                if (colorUpper.Equals("GAINSBORO")) return KnownColor.Gainsboro;
                                if (colorUpper.Equals("GOLDENROD")) return KnownColor.Goldenrod;
                                break;
                            case 'I':
                                if (colorUpper.Equals("INDIANRED")) return KnownColor.IndianRed;
                                break;
                            case 'L':
                                if (colorUpper.Equals("LAWNGREEN")) return KnownColor.LawnGreen;
                                if (colorUpper.Equals("LIGHTBLUE")) return KnownColor.LightBlue;
                                if (colorUpper.Equals("LIGHTCYAN")) return KnownColor.LightCyan;
                                if (colorUpper.Equals("LIGHTGRAY")) return KnownColor.LightGray;
                                if (colorUpper.Equals("LIGHTPINK")) return KnownColor.LightPink;
                                if (colorUpper.Equals("LIMEGREEN")) return KnownColor.LimeGreen;
                                break;
                            case 'M':
                                if (colorUpper.Equals("MINTCREAM")) return KnownColor.MintCream;
                                if (colorUpper.Equals("MISTYROSE")) return KnownColor.MistyRose;
                                break;
                            case 'O':
                                if (colorUpper.Equals("OLIVEDRAB")) return KnownColor.OliveDrab;
                                if (colorUpper.Equals("ORANGERED")) return KnownColor.OrangeRed;
                                break;
                            case 'P':
                                if (colorUpper.Equals("PALEGREEN")) return KnownColor.PaleGreen;
                                if (colorUpper.Equals("PEACHPUFF")) return KnownColor.PeachPuff;
                                break;
                            case 'R':
                                if (colorUpper.Equals("ROSYBROWN")) return KnownColor.RosyBrown;
                                if (colorUpper.Equals("ROYALBLUE")) return KnownColor.RoyalBlue;
                                break;
                            case 'S':
                                if (colorUpper.Equals("SLATEBLUE")) return KnownColor.SlateBlue;
                                if (colorUpper.Equals("SLATEGRAY")) return KnownColor.SlateGray;
                                if (colorUpper.Equals("STEELBLUE")) return KnownColor.SteelBlue;
                                break;
                            case 'T':
                                if (colorUpper.Equals("TURQUOISE")) return KnownColor.Turquoise;
                                break;
                        }
                        break;
                    case 10:
                        switch (colorUpper[0])
                        {
                            case 'A':
                                if (colorUpper.Equals("AQUAMARINE")) return KnownColor.Aquamarine;
                                break;
                            case 'B':
                                if (colorUpper.Equals("BLUEVIOLET")) return KnownColor.BlueViolet;
                                break;
                            case 'C':
                                if (colorUpper.Equals("CHARTREUSE")) return KnownColor.Chartreuse;
                                break;
                            case 'D':
                                if (colorUpper.Equals("DARKORANGE")) return KnownColor.DarkOrange;
                                if (colorUpper.Equals("DARKORCHID")) return KnownColor.DarkOrchid;
                                if (colorUpper.Equals("DARKSALMON")) return KnownColor.DarkSalmon;
                                if (colorUpper.Equals("DARKVIOLET")) return KnownColor.DarkViolet;
                                if (colorUpper.Equals("DODGERBLUE")) return KnownColor.DodgerBlue;
                                break;
                            case 'G':
                                if (colorUpper.Equals("GHOSTWHITE")) return KnownColor.GhostWhite;
                                break;
                            case 'L':
                                if (colorUpper.Equals("LIGHTCORAL")) return KnownColor.LightCoral;
                                if (colorUpper.Equals("LIGHTGREEN")) return KnownColor.LightGreen;
                                break;
                            case 'M':
                                if (colorUpper.Equals("MEDIUMBLUE")) return KnownColor.MediumBlue;
                                break;
                            case 'P':
                                if (colorUpper.Equals("PAPAYAWHIP")) return KnownColor.PapayaWhip;
                                if (colorUpper.Equals("POWDERBLUE")) return KnownColor.PowderBlue;
                                break;
                            case 'S':
                                if (colorUpper.Equals("SANDYBROWN")) return KnownColor.SandyBrown;
                                break;
                            case 'W':
                                if (colorUpper.Equals("WHITESMOKE")) return KnownColor.WhiteSmoke;
                                break;
                        }
                        break;
                    case 11:
                        switch (colorUpper[0])
                        {
                            case 'D':
                                if (colorUpper.Equals("DARKMAGENTA")) return KnownColor.DarkMagenta;
                                if (colorUpper.Equals("DEEPSKYBLUE")) return KnownColor.DeepSkyBlue;
                                break;
                            case 'F':
                                if (colorUpper.Equals("FLORALWHITE")) return KnownColor.FloralWhite;
                                if (colorUpper.Equals("FORESTGREEN")) return KnownColor.ForestGreen;
                                break;
                            case 'G':
                                if (colorUpper.Equals("GREENYELLOW")) return KnownColor.GreenYellow;
                                break;
                            case 'L':
                                if (colorUpper.Equals("LIGHTSALMON")) return KnownColor.LightSalmon;
                                if (colorUpper.Equals("LIGHTYELLOW")) return KnownColor.LightYellow;
                                break;
                            case 'N':
                                if (colorUpper.Equals("NAVAJOWHITE")) return KnownColor.NavajoWhite;
                                break;
                            case 'S':
                                if (colorUpper.Equals("SADDLEBROWN")) return KnownColor.SaddleBrown;
                                if (colorUpper.Equals("SPRINGGREEN")) return KnownColor.SpringGreen;
                                break;
                            case 'T':
                                if (colorUpper.Equals("TRANSPARENT")) return KnownColor.Transparent;
                                break;
                            case 'Y':
                                if (colorUpper.Equals("YELLOWGREEN")) return KnownColor.YellowGreen;
                                break;
                        }
                        break;
                    case 12:
                        switch (colorUpper[0])
                        {
                            case 'A':
                                if (colorUpper.Equals("ANTIQUEWHITE")) return KnownColor.AntiqueWhite;
                                break;
                            case 'D':
                                if (colorUpper.Equals("DARKSEAGREEN")) return KnownColor.DarkSeaGreen;
                                break;
                            case 'L':
                                if (colorUpper.Equals("LIGHTSKYBLUE")) return KnownColor.LightSkyBlue;
                                if (colorUpper.Equals("LEMONCHIFFON")) return KnownColor.LemonChiffon;
                                break;
                            case 'M':
                                if (colorUpper.Equals("MEDIUMORCHID")) return KnownColor.MediumOrchid;
                                if (colorUpper.Equals("MEDIUMPURPLE")) return KnownColor.MediumPurple;
                                if (colorUpper.Equals("MIDNIGHTBLUE")) return KnownColor.MidnightBlue;
                                break;
                        }
                        break;
                    case 13:
                        switch (colorUpper[0])
                        {
                            case 'D':
                                if (colorUpper.Equals("DARKSLATEBLUE")) return KnownColor.DarkSlateBlue;
                                if (colorUpper.Equals("DARKSLATEGRAY")) return KnownColor.DarkSlateGray;
                                if (colorUpper.Equals("DARKGOLDENROD")) return KnownColor.DarkGoldenrod;
                                if (colorUpper.Equals("DARKTURQUOISE")) return KnownColor.DarkTurquoise;
                                break;
                            case 'L':
                                if (colorUpper.Equals("LIGHTSEAGREEN")) return KnownColor.LightSeaGreen;
                                if (colorUpper.Equals("LAVENDERBLUSH")) return KnownColor.LavenderBlush;
                                break;
                            case 'P':
                                if (colorUpper.Equals("PALEGOLDENROD")) return KnownColor.PaleGoldenrod;
                                if (colorUpper.Equals("PALETURQUOISE")) return KnownColor.PaleTurquoise;
                                if (colorUpper.Equals("PALEVIOLETRED")) return KnownColor.PaleVioletRed;
                                break;
                        }
                        break;
                    case 14:
                        switch (colorUpper[0])
                        {
                            case 'B':
                                if (colorUpper.Equals("BLANCHEDALMOND")) return KnownColor.BlanchedAlmond;
                                break;
                            case 'C':
                                if (colorUpper.Equals("CORNFLOWERBLUE")) return KnownColor.CornflowerBlue;
                                break;
                            case 'D':
                                if (colorUpper.Equals("DARKOLIVEGREEN")) return KnownColor.DarkOliveGreen;
                                break;
                            case 'L':
                                if (colorUpper.Equals("LIGHTSLATEGRAY")) return KnownColor.LightSlateGray;
                                if (colorUpper.Equals("LIGHTSTEELBLUE")) return KnownColor.LightSteelBlue;
                                break;
                            case 'M':
                                if (colorUpper.Equals("MEDIUMSEAGREEN")) return KnownColor.MediumSeaGreen;
                                break;
                        }
                        break;
                    case 15:
                        if (colorUpper.Equals("MEDIUMSLATEBLUE")) return KnownColor.MediumSlateBlue;
                        if (colorUpper.Equals("MEDIUMTURQUOISE")) return KnownColor.MediumTurquoise;
                        if (colorUpper.Equals("MEDIUMVIOLETRED")) return KnownColor.MediumVioletRed;
                        break;
                    case 16:
                        if (colorUpper.Equals("MEDIUMAQUAMARINE")) return KnownColor.MediumAquamarine;
                        break;
                    case 17:
                        if (colorUpper.Equals("MEDIUMSPRINGGREEN")) return KnownColor.MediumSpringGreen;
                        break;
                    case 20:
                        if (colorUpper.Equals("LIGHTGOLDENRODYELLOW")) return KnownColor.LightGoldenrodYellow;
                        break;
                }
            }
            // colorString was null or not found
            return KnownColor.UnknownColor;
        }
    }
}
