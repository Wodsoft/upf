using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.TextFormatting
{
    internal class GenericTextRunProperties : TextRunProperties, ICloneable, IEquatable<GenericTextRunProperties>
    {
        private Typeface _typeface;
        private float _emSize;
        private float _emHintingSize;
        private TextDecorationCollection? _textDecorations;
        private Brush? _foregroundBrush;
        private Brush? _backgroundBrush;
        private BaselineAlignment _baselineAlignment;
        private CultureInfo? _culture;
        private NumberSubstitution? _numberSubstitution;

        public GenericTextRunProperties(
            Typeface typeface,
            float size,
            float hintingSize,
            float pixelsPerDip,
            TextDecorationCollection? textDecorations,
            Brush? foregroundBrush,
            Brush? backgroundBrush,
            BaselineAlignment baselineAlignment,
            CultureInfo? culture,
            NumberSubstitution? substitution
        )
        {
            _typeface = typeface;
            _emSize = size;
            _emHintingSize = hintingSize;
            _textDecorations = textDecorations;
            _foregroundBrush = foregroundBrush;
            _backgroundBrush = backgroundBrush;
            _baselineAlignment = baselineAlignment;
            _culture = culture;
            _numberSubstitution = substitution;
            PixelsPerDip = pixelsPerDip;
        }

        /// <summary>
        /// Run typeface
        /// </summary>
        public override Typeface Typeface
        {
            get { return _typeface; }
        }


        /// <summary>
        /// Em size of font used to format and display text
        /// </summary>
        public override float FontRenderingEmSize
        {
            get { return _emSize; }
        }


        /// <summary>
        /// Em size of font to determine subtle change in font hinting default value is 12pt
        /// </summary>
        public override float FontHintingEmSize
        {
            get { return _emHintingSize; }
        }


        /// <summary>
        /// Run text decoration
        /// </summary>
        public override TextDecorationCollection? TextDecorations
        {
            get { return _textDecorations; }
        }

        /// <summary>
        /// Run text foreground brush
        /// </summary>
        public override Brush? ForegroundBrush
        {
            get { return _foregroundBrush; }
        }


        /// <summary>
        /// Run text highlight background brush
        /// </summary>
        public override Brush? BackgroundBrush
        {
            get { return _backgroundBrush; }
        }


        /// <summary>
        /// Run vertical box alignment
        /// </summary>
        public override BaselineAlignment BaselineAlignment
        {
            get { return _baselineAlignment; }
        }


        /// <summary>
        /// Run text Culture Info
        /// </summary>
        public override CultureInfo? CultureInfo
        {
            get { return _culture; }
        }

        ///// <summary>
        ///// Run typography properties
        ///// </summary>
        //public override TextRunTypographyProperties TypographyProperties
        //{
        //    get { return null; }
        //}

        ///// <summary>
        ///// Run Text effects
        ///// </summary>
        //public override TextEffectCollection TextEffects
        //{
        //    get { return null; }
        //}

        /// <summary>
        /// Number substitution
        /// </summary>
        public override NumberSubstitution? NumberSubstitution
        {
            get { return _numberSubstitution; }
        }

        public object Clone()
        {
            return new GenericTextRunProperties(_typeface, _emSize, _emHintingSize, PixelsPerDip, _textDecorations, _foregroundBrush, _backgroundBrush, _baselineAlignment, _culture, _numberSubstitution);
        }

        public bool Equals(GenericTextRunProperties? textRunProperties)
        {
            if (textRunProperties == null)
                return false;
            return _emSize == textRunProperties.FontRenderingEmSize
          && _emHintingSize == textRunProperties.FontHintingEmSize
          && _culture == textRunProperties.CultureInfo
          && _typeface.Equals(textRunProperties.Typeface)
          && ((_textDecorations == null) ? textRunProperties.TextDecorations == null : _textDecorations.ValueEquals(textRunProperties.TextDecorations))
          && _baselineAlignment == textRunProperties.BaselineAlignment
          && ((_foregroundBrush == null) ? (textRunProperties.ForegroundBrush == null) : (_foregroundBrush.Equals(textRunProperties.ForegroundBrush)))
          && ((_backgroundBrush == null) ? (textRunProperties.BackgroundBrush == null) : (_backgroundBrush.Equals(textRunProperties.BackgroundBrush)))
          && ((_numberSubstitution == null) ? (textRunProperties.NumberSubstitution == null) : (_numberSubstitution.Equals(textRunProperties.NumberSubstitution)));
        }
    }
}
