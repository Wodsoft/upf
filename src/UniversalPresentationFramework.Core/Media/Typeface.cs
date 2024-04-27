using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    /// <summary>
    /// A Typeface is a combination of family, weight, style and stretch:
    /// </summary>
    public class Typeface
    {
        private readonly FontFamily _fontFamily;

        // these _style, _weight and _stretch are only used for storing what was passed into the constructor.
        // Since FontFamily may change these values when it includes a style name implicitly,
        private readonly FontStyle _style;
        private readonly FontWeight _weight;
        private readonly FontStretch _stretch;

        private readonly FontFamily _fallbackFontFamily;

        /// <summary>
        /// Construct a typeface
        /// </summary>
        /// <param name="typefaceName">font typeface name</param>
        public Typeface(
            string typefaceName
            )
            // assume face name is family name until we get face name resolved properly.
            : this(
                new FontFamily(typefaceName),
                FontStyles.Normal,
                FontWeights.Normal,
                FontStretches.Normal
                )
        { }



        /// <summary>
        /// Construct a typeface
        /// </summary>
        /// <param name="fontFamily">Font family</param>
        /// <param name="style">Font style</param>
        /// <param name="weight">Boldness of font</param>
        /// <param name="stretch">Width of characters</param>
        public Typeface(
            FontFamily fontFamily,
            FontStyle style,
            FontWeight weight,
            FontStretch stretch
            )
            : this(
                fontFamily,
                style,
                weight,
                stretch,
                FontFamily.FontFamilyGlobalUI
                )
        { }



        /// <summary>
        /// Construct a typeface
        /// </summary>
        /// <param name="fontFamily">Font family</param>
        /// <param name="style">Font style</param>
        /// <param name="weight">Boldness of font</param>
        /// <param name="stretch">Width of characters</param>
        /// <param name="fallbackFontFamily">fallback font family</param>
        public Typeface(
            FontFamily fontFamily,
            FontStyle style,
            FontWeight weight,
            FontStretch stretch,
            FontFamily fallbackFontFamily
            )
        {
            if (fontFamily == null)
            {
                throw new ArgumentNullException("fontFamily");
            }

            _fontFamily = fontFamily;
            _style = style;
            _weight = weight;
            _stretch = stretch;
            _fallbackFontFamily = fallbackFontFamily;
        }



        /// <summary>
        /// Font family
        /// </summary>
        public FontFamily FontFamily
        {
            get { return _fontFamily; }
        }


        /// <summary>
        /// Font weight (light, bold, etc.)
        /// </summary>
        public FontWeight Weight
        {
            get { return _weight; }
        }


        /// <summary>
        /// Font style (italic, oblique)
        /// </summary>
        public FontStyle Style
        {
            get { return _style; }
        }


        /// <summary>
        /// Font Stretch (narrow, wide, etc.)
        /// </summary>
        public FontStretch Stretch
        {
            get { return _stretch; }
        }

        /// <summary>
        /// Fallback font family
        /// </summary>
        internal FontFamily FallbackFontFamily
        {
            get { return _fallbackFontFamily; }
        }

        /// <summary>
        /// Create correspondent hash code for the object
        /// </summary>
        /// <returns>object hash code</returns>
        public override int GetHashCode()
        {
            int hash = _fontFamily.GetHashCode();

            if (_fallbackFontFamily != null)
                hash = HashFn.HashMultiply(hash) + _fallbackFontFamily.GetHashCode();

            hash = HashFn.HashMultiply(hash) + _style.GetHashCode();
            hash = HashFn.HashMultiply(hash) + _weight.GetHashCode();
            hash = HashFn.HashMultiply(hash) + _stretch.GetHashCode();
            return HashFn.HashScramble(hash);
        }

        /// <summary>
        /// Equality check
        /// </summary>
        public override bool Equals(object? o)
        {
            if (o is Typeface t)
                return _style == t._style
                    && _weight == t._weight
                    && _stretch == t._stretch
                    && _fontFamily.Equals(t._fontFamily)
                    && CompareFallbackFontFamily(t._fallbackFontFamily);
            return false;
        }


        internal bool CompareFallbackFontFamily(FontFamily fallbackFontFamily)
        {
            if (fallbackFontFamily == null || _fallbackFontFamily == null)
                return fallbackFontFamily == _fallbackFontFamily;

            return _fallbackFontFamily.Equals(fallbackFontFamily);
        }

        public GlyphTypeface? GetGlyphTypeface()
        {
            return _GlyphTypefaceCache.GetOrAdd(this, t => t._fontFamily.GetGlyphTypeface(t._style, t._weight, t._stretch));
        }

        private static ConcurrentDictionary<Typeface, GlyphTypeface?> _GlyphTypefaceCache = new ConcurrentDictionary<Typeface, GlyphTypeface?>();
    }
}
