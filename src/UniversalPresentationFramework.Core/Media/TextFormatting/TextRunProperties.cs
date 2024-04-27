using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.TextFormatting
{
    /// <summary>
    /// Properties that can change from one run to the next, such as typeface or foreground brush.
    /// </summary>
    /// <remarks>
    /// The client provides a concrete implementation of this abstract run properties class. This
    /// allows client to implement their run properties the way that fits with their run formatting
    /// store.
    /// </remarks>
    public abstract class TextRunProperties
    {
        /// <summary>
        /// Run typeface
        /// </summary>
        public abstract Typeface Typeface
        { get; }


        /// <summary>
        /// Em size of font used to format and display text
        /// </summary>
        public abstract float FontRenderingEmSize
        { get; }


        /// <summary>
        /// Em size of font to determine subtle change in font hinting default value is 12pt
        /// </summary>
        public abstract float FontHintingEmSize
        { get; }

        ///<summary>
        /// Run TextDecorations. 
        ///</summary>
        public abstract TextDecorationCollection? TextDecorations
        { get; }

        /// <summary>
        /// Brush used to fill text
        /// </summary>
        public abstract Brush? ForegroundBrush
        { get; }


        /// <summary>
        /// Brush used to paint background of run
        /// </summary>
        public abstract Brush? BackgroundBrush
        { get; }


        /// <summary>
        /// Run text culture info
        /// </summary>
        public abstract CultureInfo? CultureInfo
        { get; }


        ///// <summary>
        ///// Run Text effect collection
        ///// </summary>
        //public abstract TextEffectCollection TextEffects
        //{ get; }


        /// <summary>
        /// Run vertical box alignment
        /// </summary>
        public virtual BaselineAlignment BaselineAlignment
        {
            get { return BaselineAlignment.Baseline; }
        }


        ///// <summary>
        ///// Run typography properties
        ///// </summary>
        //public virtual TextRunTypographyProperties TypographyProperties
        //{
        //    get { return null; }
        //}


        /// <summary>
        /// Number substitution options.
        /// </summary>
        public virtual NumberSubstitution? NumberSubstitution
        {
            get { return null; }
        }

        private float _pixelsPerDip = 1f;
        /// <summary>
        /// PixelsPerDip at which the text should be rendered.
        /// </summary>
        public float PixelsPerDip
        {
            get { return _pixelsPerDip; }
            set { _pixelsPerDip = value; }
        }
    }
}
