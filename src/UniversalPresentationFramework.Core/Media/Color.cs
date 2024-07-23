using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    [TypeConverter(typeof(ColorConverter))]
    public struct Color : IFormattable, IEquatable<Color>
    {
        private byte _r, _g, _b, _a;
        private float _rf, _gf, _bf, _af;

        ///<summary>
        /// private helper function to set context values from a color value with a set context and ScRgb values
        ///</summary>
        private static float sRgbToScRgb(byte bval)
        {
            float val = ((float)bval / 255.0f);

            if (!(val > 0.0))       // Handles NaN case too. (Though, NaN isn't actually
                                    // possible in this case.)
            {
                return (0.0f);
            }
            else if (val <= 0.04045)
            {
                return (val / 12.92f);
            }
            else if (val < 1.0f)
            {
                return (float)Math.Pow(((double)val + 0.055) / 1.055, 2.4);
            }
            else
            {
                return (1.0f);
            }
        }

        ///<summary>
        /// private helper function to set context values from a color value with a set context and ScRgb values
        ///</summary>
        ///
        private static byte ScRgbTosRgb(float val)
        {
            if (!(val > 0.0))       // Handles NaN case too
            {
                return (0);
            }
            else if (val <= 0.0031308)
            {
                return ((byte)((255.0f * val * 12.92f) + 0.5f));
            }
            else if (val < 1.0)
            {
                return ((byte)((255.0f * ((1.055f * (float)Math.Pow((double)val, (1.0 / 2.4))) - 0.055f)) + 0.5f));
            }
            else
            {
                return (255);
            }
        }

        public Color(byte a, byte r, byte g, byte b)
        {
            _a = a;
            _r = r;
            _g = g;
            _b = b;
            _af = a / 255f;
            _rf = sRgbToScRgb(r);
            _gf = sRgbToScRgb(g);
            _bf = sRgbToScRgb(b);
        }

        public Color(byte r, byte g, byte b) : this(255, r, g, b) { }

        public Color(float a, float r, float g, float b)
        {
            if (a < 0f) a = 0f;
            if (a > 1f) a = 1f;
            if (r < 0f) r = 0f;
            if (r > 1f) r = 1f;
            if (g < 0f) g = 0f;
            if (g > 1f) g = 1f;
            if (b < 0f) b = 0f;
            if (b > 1f) b = 1f;

            _af = a;
            _rf = r;
            _gf = g;
            _bf = b;
            _a = (byte)(a * 255f);
            _r = ScRgbTosRgb(r);
            _g = ScRgbTosRgb(g);
            _b = ScRgbTosRgb(b);
        }

        public Color(float r, float g, float b) : this(1f, r, g, b) { }

        #region Properties

        public byte R { get => _r; set { _r = value; _rf = sRgbToScRgb(value); } }
        public byte G { get => _g; set { _g = value; _gf = sRgbToScRgb(value); } }
        public byte B { get => _b; set { _b = value; _bf = sRgbToScRgb(value); } }
        public byte A { get => _a; set { _a = value; _af = value / 255f; } }

        public float ScR
        {
            get => _rf; set
            {
                if (value < 0f) value = 0;
                if (value > 1f) value = 1f;
                _rf = value; _r = ScRgbTosRgb(value);
            }
        }
        public float ScG
        {
            get => _gf; set
            {
                if (value < 0f) value = 0;
                if (value > 1f) value = 1f; _gf = value; _g = ScRgbTosRgb(value);
            }
        }
        public float ScB
        {
            get => _bf; set
            {
                if (value < 0f) value = 0;
                if (value > 1f) value = 1f; _bf = value; _b = ScRgbTosRgb(value);
            }
        }
        public float ScA
        {
            get => _af; set
            {
                if (value < 0f) value = 0;
                if (value > 1f) value = 1f; _af = value; _a = (byte)(value * 255f);
            }
        }

        #endregion

        #region Methods

        public static Color FromUInt32(uint argb)
        {

            var a = (byte)((argb & 0xff000000) >> 24);
            var r = (byte)((argb & 0x00ff0000) >> 16);
            var g = (byte)((argb & 0x0000ff00) >> 8);
            var b = (byte)(argb & 0x000000ff);
            return new Color(a, r, g, b);
        }

        public static Color FromScRgb(float a, float r, float g, float b)
        {
            return new Color(a, r, g, b);
        }

        public static Color FromArgb(byte a, byte r, byte g, byte b)
        {
            return new Color(a, r, g, b);
        }

        public static Color FromRgb(byte r, byte g, byte b)
        {
            return new Color(r, g, b);
        }

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            return string.Create(formatProvider, stackalloc char[128], $"#{_a:X2}{_r:X2}{_g:X2}{_b:X2}");
        }

        public static bool Equals(Color color1, Color color2)
        {
            return (color1 == color2);
        }

        public bool Equals(Color color)
        {
            return this == color;
        }

        public override bool Equals(object? o)
        {
            if (o is Color color)
                return (this == color);
            return false;
        }

        public static bool operator ==(Color color1, Color color2)
        {
            return color1.A == color2.A && color1.R == color2.R && color1.G == color2.G && color1.B == color2.B;
        }

        public static bool operator !=(Color color1, Color color2)
        {
            return (!(color1 == color2));
        }

        public override int GetHashCode()
        {
            return _a.GetHashCode() ^ _r.GetHashCode() ^ _g.GetHashCode() ^ _b.GetHashCode();
        }

        public static Color operator +(Color color1, Color color2)
        {
                Color c1 = FromScRgb(
                      color1._af + color2._af,
                      color1._rf + color2._rf,
                      color1._gf + color2._gf,
                      color1._bf + color2._bf);
                return c1;
        }

        public static Color operator -(Color color1, Color color2)
        {
                Color c1 = FromScRgb(
                    color1._af - color2._af,
                    color1._rf - color2._rf,
                    color1._gf - color2._gf,
                    color1._bf - color2._bf
                    );
                return c1;
        }

        public static Color operator *(Color color, float coefficient)
        {
            Color c1 = FromScRgb(color._af * coefficient, color._rf * coefficient, color._gf * coefficient, color._bf * coefficient);
            return c1;
        }

        #endregion

    }
}
