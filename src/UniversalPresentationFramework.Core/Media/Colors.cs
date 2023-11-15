using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    public sealed class Colors
    {
        #region Constructors

        // Colors only has static members, so it shouldn't be constructable.
        private Colors()
        {
        }

        #endregion Constructors

        #region static Known Colors

        /// <summary>
        /// Well-known color: AliceBlue
        /// </summary>
        public static Color AliceBlue
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.AliceBlue);
            }
        }

        /// <summary>
        /// Well-known color: AntiqueWhite
        /// </summary>
        public static Color AntiqueWhite
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.AntiqueWhite);
            }
        }

        /// <summary>
        /// Well-known color: Aqua
        /// </summary>
        public static Color Aqua
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Aqua);
            }
        }

        /// <summary>
        /// Well-known color: Aquamarine
        /// </summary>
        public static Color Aquamarine
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Aquamarine);
            }
        }

        /// <summary>
        /// Well-known color: Azure
        /// </summary>
        public static Color Azure
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Azure);
            }
        }

        /// <summary>
        /// Well-known color: Beige
        /// </summary>
        public static Color Beige
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Beige);
            }
        }

        /// <summary>
        /// Well-known color: Bisque
        /// </summary>
        public static Color Bisque
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Bisque);
            }
        }

        /// <summary>
        /// Well-known color: Black
        /// </summary>
        public static Color Black
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Black);
            }
        }

        /// <summary>
        /// Well-known color: BlanchedAlmond
        /// </summary>
        public static Color BlanchedAlmond
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.BlanchedAlmond);
            }
        }

        /// <summary>
        /// Well-known color: Blue
        /// </summary>
        public static Color Blue
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Blue);
            }
        }

        /// <summary>
        /// Well-known color: BlueViolet
        /// </summary>
        public static Color BlueViolet
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.BlueViolet);
            }
        }

        /// <summary>
        /// Well-known color: Brown
        /// </summary>
        public static Color Brown
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Brown);
            }
        }

        /// <summary>
        /// Well-known color: BurlyWood
        /// </summary>
        public static Color BurlyWood
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.BurlyWood);
            }
        }

        /// <summary>
        /// Well-known color: CadetBlue
        /// </summary>
        public static Color CadetBlue
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.CadetBlue);
            }
        }

        /// <summary>
        /// Well-known color: Chartreuse
        /// </summary>
        public static Color Chartreuse
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Chartreuse);
            }
        }

        /// <summary>
        /// Well-known color: Chocolate
        /// </summary>
        public static Color Chocolate
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Chocolate);
            }
        }

        /// <summary>
        /// Well-known color: Coral
        /// </summary>
        public static Color Coral
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Coral);
            }
        }

        /// <summary>
        /// Well-known color: CornflowerBlue
        /// </summary>
        public static Color CornflowerBlue
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.CornflowerBlue);
            }
        }

        /// <summary>
        /// Well-known color: Cornsilk
        /// </summary>
        public static Color Cornsilk
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Cornsilk);
            }
        }

        /// <summary>
        /// Well-known color: Crimson
        /// </summary>
        public static Color Crimson
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Crimson);
            }
        }

        /// <summary>
        /// Well-known color: Cyan
        /// </summary>
        public static Color Cyan
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Cyan);
            }
        }

        /// <summary>
        /// Well-known color: DarkBlue
        /// </summary>
        public static Color DarkBlue
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.DarkBlue);
            }
        }

        /// <summary>
        /// Well-known color: DarkCyan
        /// </summary>
        public static Color DarkCyan
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.DarkCyan);
            }
        }

        /// <summary>
        /// Well-known color: DarkGoldenrod
        /// </summary>
        public static Color DarkGoldenrod
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.DarkGoldenrod);
            }
        }

        /// <summary>
        /// Well-known color: DarkGray
        /// </summary>
        public static Color DarkGray
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.DarkGray);
            }
        }

        /// <summary>
        /// Well-known color: DarkGreen
        /// </summary>
        public static Color DarkGreen
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.DarkGreen);
            }
        }

        /// <summary>
        /// Well-known color: DarkKhaki
        /// </summary>
        public static Color DarkKhaki
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.DarkKhaki);
            }
        }

        /// <summary>
        /// Well-known color: DarkMagenta
        /// </summary>
        public static Color DarkMagenta
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.DarkMagenta);
            }
        }

        /// <summary>
        /// Well-known color: DarkOliveGreen
        /// </summary>
        public static Color DarkOliveGreen
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.DarkOliveGreen);
            }
        }

        /// <summary>
        /// Well-known color: DarkOrange
        /// </summary>
        public static Color DarkOrange
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.DarkOrange);
            }
        }

        /// <summary>
        /// Well-known color: DarkOrchid
        /// </summary>
        public static Color DarkOrchid
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.DarkOrchid);
            }
        }

        /// <summary>
        /// Well-known color: DarkRed
        /// </summary>
        public static Color DarkRed
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.DarkRed);
            }
        }

        /// <summary>
        /// Well-known color: DarkSalmon
        /// </summary>
        public static Color DarkSalmon
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.DarkSalmon);
            }
        }

        /// <summary>
        /// Well-known color: DarkSeaGreen
        /// </summary>
        public static Color DarkSeaGreen
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.DarkSeaGreen);
            }
        }

        /// <summary>
        /// Well-known color: DarkSlateBlue
        /// </summary>
        public static Color DarkSlateBlue
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.DarkSlateBlue);
            }
        }

        /// <summary>
        /// Well-known color: DarkSlateGray
        /// </summary>
        public static Color DarkSlateGray
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.DarkSlateGray);
            }
        }

        /// <summary>
        /// Well-known color: DarkTurquoise
        /// </summary>
        public static Color DarkTurquoise
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.DarkTurquoise);
            }
        }

        /// <summary>
        /// Well-known color: DarkViolet
        /// </summary>
        public static Color DarkViolet
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.DarkViolet);
            }
        }

        /// <summary>
        /// Well-known color: DeepPink
        /// </summary>
        public static Color DeepPink
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.DeepPink);
            }
        }

        /// <summary>
        /// Well-known color: DeepSkyBlue
        /// </summary>
        public static Color DeepSkyBlue
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.DeepSkyBlue);
            }
        }

        /// <summary>
        /// Well-known color: DimGray
        /// </summary>
        public static Color DimGray
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.DimGray);
            }
        }

        /// <summary>
        /// Well-known color: DodgerBlue
        /// </summary>
        public static Color DodgerBlue
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.DodgerBlue);
            }
        }

        /// <summary>
        /// Well-known color: Firebrick
        /// </summary>
        public static Color Firebrick
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Firebrick);
            }
        }

        /// <summary>
        /// Well-known color: FloralWhite
        /// </summary>
        public static Color FloralWhite
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.FloralWhite);
            }
        }

        /// <summary>
        /// Well-known color: ForestGreen
        /// </summary>
        public static Color ForestGreen
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.ForestGreen);
            }
        }

        /// <summary>
        /// Well-known color: Fuchsia
        /// </summary>
        public static Color Fuchsia
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Fuchsia);
            }
        }

        /// <summary>
        /// Well-known color: Gainsboro
        /// </summary>
        public static Color Gainsboro
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Gainsboro);
            }
        }

        /// <summary>
        /// Well-known color: GhostWhite
        /// </summary>
        public static Color GhostWhite
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.GhostWhite);
            }
        }

        /// <summary>
        /// Well-known color: Gold
        /// </summary>
        public static Color Gold
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Gold);
            }
        }

        /// <summary>
        /// Well-known color: Goldenrod
        /// </summary>
        public static Color Goldenrod
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Goldenrod);
            }
        }

        /// <summary>
        /// Well-known color: Gray
        /// </summary>
        public static Color Gray
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Gray);
            }
        }

        /// <summary>
        /// Well-known color: Green
        /// </summary>
        public static Color Green
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Green);
            }
        }

        /// <summary>
        /// Well-known color: GreenYellow
        /// </summary>
        public static Color GreenYellow
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.GreenYellow);
            }
        }

        /// <summary>
        /// Well-known color: Honeydew
        /// </summary>
        public static Color Honeydew
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Honeydew);
            }
        }

        /// <summary>
        /// Well-known color: HotPink
        /// </summary>
        public static Color HotPink
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.HotPink);
            }
        }

        /// <summary>
        /// Well-known color: IndianRed
        /// </summary>
        public static Color IndianRed
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.IndianRed);
            }
        }

        /// <summary>
        /// Well-known color: Indigo
        /// </summary>
        public static Color Indigo
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Indigo);
            }
        }

        /// <summary>
        /// Well-known color: Ivory
        /// </summary>
        public static Color Ivory
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Ivory);
            }
        }

        /// <summary>
        /// Well-known color: Khaki
        /// </summary>
        public static Color Khaki
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Khaki);
            }
        }

        /// <summary>
        /// Well-known color: Lavender
        /// </summary>
        public static Color Lavender
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Lavender);
            }
        }

        /// <summary>
        /// Well-known color: LavenderBlush
        /// </summary>
        public static Color LavenderBlush
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.LavenderBlush);
            }
        }

        /// <summary>
        /// Well-known color: LawnGreen
        /// </summary>
        public static Color LawnGreen
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.LawnGreen);
            }
        }

        /// <summary>
        /// Well-known color: LemonChiffon
        /// </summary>
        public static Color LemonChiffon
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.LemonChiffon);
            }
        }

        /// <summary>
        /// Well-known color: LightBlue
        /// </summary>
        public static Color LightBlue
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.LightBlue);
            }
        }

        /// <summary>
        /// Well-known color: LightCoral
        /// </summary>
        public static Color LightCoral
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.LightCoral);
            }
        }

        /// <summary>
        /// Well-known color: LightCyan
        /// </summary>
        public static Color LightCyan
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.LightCyan);
            }
        }

        /// <summary>
        /// Well-known color: LightGoldenrodYellow
        /// </summary>
        public static Color LightGoldenrodYellow
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.LightGoldenrodYellow);
            }
        }

        /// <summary>
        /// Well-known color: LightGray
        /// </summary>
        public static Color LightGray
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.LightGray);
            }
        }

        /// <summary>
        /// Well-known color: LightGreen
        /// </summary>
        public static Color LightGreen
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.LightGreen);
            }
        }

        /// <summary>
        /// Well-known color: LightPink
        /// </summary>
        public static Color LightPink
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.LightPink);
            }
        }

        /// <summary>
        /// Well-known color: LightSalmon
        /// </summary>
        public static Color LightSalmon
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.LightSalmon);
            }
        }

        /// <summary>
        /// Well-known color: LightSeaGreen
        /// </summary>
        public static Color LightSeaGreen
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.LightSeaGreen);
            }
        }

        /// <summary>
        /// Well-known color: LightSkyBlue
        /// </summary>
        public static Color LightSkyBlue
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.LightSkyBlue);
            }
        }

        /// <summary>
        /// Well-known color: LightSlateGray
        /// </summary>
        public static Color LightSlateGray
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.LightSlateGray);
            }
        }

        /// <summary>
        /// Well-known color: LightSteelBlue
        /// </summary>
        public static Color LightSteelBlue
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.LightSteelBlue);
            }
        }

        /// <summary>
        /// Well-known color: LightYellow
        /// </summary>
        public static Color LightYellow
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.LightYellow);
            }
        }

        /// <summary>
        /// Well-known color: Lime
        /// </summary>
        public static Color Lime
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Lime);
            }
        }

        /// <summary>
        /// Well-known color: LimeGreen
        /// </summary>
        public static Color LimeGreen
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.LimeGreen);
            }
        }

        /// <summary>
        /// Well-known color: Linen
        /// </summary>
        public static Color Linen
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Linen);
            }
        }

        /// <summary>
        /// Well-known color: Magenta
        /// </summary>
        public static Color Magenta
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Magenta);
            }
        }

        /// <summary>
        /// Well-known color: Maroon
        /// </summary>
        public static Color Maroon
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Maroon);
            }
        }

        /// <summary>
        /// Well-known color: MediumAquamarine
        /// </summary>
        public static Color MediumAquamarine
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.MediumAquamarine);
            }
        }

        /// <summary>
        /// Well-known color: MediumBlue
        /// </summary>
        public static Color MediumBlue
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.MediumBlue);
            }
        }

        /// <summary>
        /// Well-known color: MediumOrchid
        /// </summary>
        public static Color MediumOrchid
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.MediumOrchid);
            }
        }

        /// <summary>
        /// Well-known color: MediumPurple
        /// </summary>
        public static Color MediumPurple
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.MediumPurple);
            }
        }

        /// <summary>
        /// Well-known color: MediumSeaGreen
        /// </summary>
        public static Color MediumSeaGreen
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.MediumSeaGreen);
            }
        }

        /// <summary>
        /// Well-known color: MediumSlateBlue
        /// </summary>
        public static Color MediumSlateBlue
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.MediumSlateBlue);
            }
        }

        /// <summary>
        /// Well-known color: MediumSpringGreen
        /// </summary>
        public static Color MediumSpringGreen
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.MediumSpringGreen);
            }
        }

        /// <summary>
        /// Well-known color: MediumTurquoise
        /// </summary>
        public static Color MediumTurquoise
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.MediumTurquoise);
            }
        }

        /// <summary>
        /// Well-known color: MediumVioletRed
        /// </summary>
        public static Color MediumVioletRed
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.MediumVioletRed);
            }
        }

        /// <summary>
        /// Well-known color: MidnightBlue
        /// </summary>
        public static Color MidnightBlue
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.MidnightBlue);
            }
        }

        /// <summary>
        /// Well-known color: MintCream
        /// </summary>
        public static Color MintCream
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.MintCream);
            }
        }

        /// <summary>
        /// Well-known color: MistyRose
        /// </summary>
        public static Color MistyRose
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.MistyRose);
            }
        }

        /// <summary>
        /// Well-known color: Moccasin
        /// </summary>
        public static Color Moccasin
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Moccasin);
            }
        }

        /// <summary>
        /// Well-known color: NavajoWhite
        /// </summary>
        public static Color NavajoWhite
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.NavajoWhite);
            }
        }

        /// <summary>
        /// Well-known color: Navy
        /// </summary>
        public static Color Navy
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Navy);
            }
        }

        /// <summary>
        /// Well-known color: OldLace
        /// </summary>
        public static Color OldLace
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.OldLace);
            }
        }

        /// <summary>
        /// Well-known color: Olive
        /// </summary>
        public static Color Olive
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Olive);
            }
        }

        /// <summary>
        /// Well-known color: OliveDrab
        /// </summary>
        public static Color OliveDrab
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.OliveDrab);
            }
        }

        /// <summary>
        /// Well-known color: Orange
        /// </summary>
        public static Color Orange
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Orange);
            }
        }

        /// <summary>
        /// Well-known color: OrangeRed
        /// </summary>
        public static Color OrangeRed
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.OrangeRed);
            }
        }

        /// <summary>
        /// Well-known color: Orchid
        /// </summary>
        public static Color Orchid
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Orchid);
            }
        }

        /// <summary>
        /// Well-known color: PaleGoldenrod
        /// </summary>
        public static Color PaleGoldenrod
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.PaleGoldenrod);
            }
        }

        /// <summary>
        /// Well-known color: PaleGreen
        /// </summary>
        public static Color PaleGreen
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.PaleGreen);
            }
        }

        /// <summary>
        /// Well-known color: PaleTurquoise
        /// </summary>
        public static Color PaleTurquoise
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.PaleTurquoise);
            }
        }

        /// <summary>
        /// Well-known color: PaleVioletRed
        /// </summary>
        public static Color PaleVioletRed
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.PaleVioletRed);
            }
        }

        /// <summary>
        /// Well-known color: PapayaWhip
        /// </summary>
        public static Color PapayaWhip
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.PapayaWhip);
            }
        }

        /// <summary>
        /// Well-known color: PeachPuff
        /// </summary>
        public static Color PeachPuff
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.PeachPuff);
            }
        }

        /// <summary>
        /// Well-known color: Peru
        /// </summary>
        public static Color Peru
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Peru);
            }
        }

        /// <summary>
        /// Well-known color: Pink
        /// </summary>
        public static Color Pink
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Pink);
            }
        }

        /// <summary>
        /// Well-known color: Plum
        /// </summary>
        public static Color Plum
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Plum);
            }
        }

        /// <summary>
        /// Well-known color: PowderBlue
        /// </summary>
        public static Color PowderBlue
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.PowderBlue);
            }
        }

        /// <summary>
        /// Well-known color: Purple
        /// </summary>
        public static Color Purple
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Purple);
            }
        }

        /// <summary>
        /// Well-known color: Red
        /// </summary>
        public static Color Red
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Red);
            }
        }

        /// <summary>
        /// Well-known color: RosyBrown
        /// </summary>
        public static Color RosyBrown
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.RosyBrown);
            }
        }

        /// <summary>
        /// Well-known color: RoyalBlue
        /// </summary>
        public static Color RoyalBlue
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.RoyalBlue);
            }
        }

        /// <summary>
        /// Well-known color: SaddleBrown
        /// </summary>
        public static Color SaddleBrown
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.SaddleBrown);
            }
        }

        /// <summary>
        /// Well-known color: Salmon
        /// </summary>
        public static Color Salmon
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Salmon);
            }
        }

        /// <summary>
        /// Well-known color: SandyBrown
        /// </summary>
        public static Color SandyBrown
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.SandyBrown);
            }
        }

        /// <summary>
        /// Well-known color: SeaGreen
        /// </summary>
        public static Color SeaGreen
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.SeaGreen);
            }
        }

        /// <summary>
        /// Well-known color: SeaShell
        /// </summary>
        public static Color SeaShell
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.SeaShell);
            }
        }

        /// <summary>
        /// Well-known color: Sienna
        /// </summary>
        public static Color Sienna
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Sienna);
            }
        }

        /// <summary>
        /// Well-known color: Silver
        /// </summary>
        public static Color Silver
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Silver);
            }
        }

        /// <summary>
        /// Well-known color: SkyBlue
        /// </summary>
        public static Color SkyBlue
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.SkyBlue);
            }
        }

        /// <summary>
        /// Well-known color: SlateBlue
        /// </summary>
        public static Color SlateBlue
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.SlateBlue);
            }
        }

        /// <summary>
        /// Well-known color: SlateGray
        /// </summary>
        public static Color SlateGray
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.SlateGray);
            }
        }

        /// <summary>
        /// Well-known color: Snow
        /// </summary>
        public static Color Snow
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Snow);
            }
        }

        /// <summary>
        /// Well-known color: SpringGreen
        /// </summary>
        public static Color SpringGreen
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.SpringGreen);
            }
        }

        /// <summary>
        /// Well-known color: SteelBlue
        /// </summary>
        public static Color SteelBlue
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.SteelBlue);
            }
        }

        /// <summary>
        /// Well-known color: Tan
        /// </summary>
        public static Color Tan
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Tan);
            }
        }

        /// <summary>
        /// Well-known color: Teal
        /// </summary>
        public static Color Teal
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Teal);
            }
        }

        /// <summary>
        /// Well-known color: Thistle
        /// </summary>
        public static Color Thistle
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Thistle);
            }
        }

        /// <summary>
        /// Well-known color: Tomato
        /// </summary>
        public static Color Tomato
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Tomato);
            }
        }

        /// <summary>
        /// Well-known color: Transparent
        /// </summary>
        public static Color Transparent
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Transparent);
            }
        }

        /// <summary>
        /// Well-known color: Turquoise
        /// </summary>
        public static Color Turquoise
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Turquoise);
            }
        }

        /// <summary>
        /// Well-known color: Violet
        /// </summary>
        public static Color Violet
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Violet);
            }
        }

        /// <summary>
        /// Well-known color: Wheat
        /// </summary>
        public static Color Wheat
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Wheat);
            }
        }

        /// <summary>
        /// Well-known color: White
        /// </summary>
        public static Color White
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.White);
            }
        }

        /// <summary>
        /// Well-known color: WhiteSmoke
        /// </summary>
        public static Color WhiteSmoke
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.WhiteSmoke);
            }
        }

        /// <summary>
        /// Well-known color: Yellow
        /// </summary>
        public static Color Yellow
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.Yellow);
            }
        }

        /// <summary>
        /// Well-known color: YellowGreen
        /// </summary>
        public static Color YellowGreen
        {
            get
            {
                return Color.FromUInt32((uint)KnownColor.YellowGreen);
            }
        }

        #endregion static Known Colors
    }
}
