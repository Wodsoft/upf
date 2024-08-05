using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media.Animation;

namespace Wodsoft.UI.Media
{
    public class GradientStop : Animatable, IFormattable
    {
        #region Constructors

        /// <summary>
        /// GradientStop - Initialize this GradientStop
        /// </summary>
        public GradientStop()
        {
        }

        /// <summary>
        /// GradientStop - Initialize this GradientStop from a constant Color and offset.
        /// </summary>
        /// <param name="color"> The Color at this offset. </param>
        /// <param name="offset"> The offset within the Gradient. </param>
        public GradientStop(Color color, float offset)
        {
            Color = color;
            Offset = offset;
        }

        #endregion Constructors

        #region Clone

        /// <summary>
        ///     Shadows inherited Clone() with a strongly typed
        ///     version for convenience.
        /// </summary>
        public new GradientStop Clone()
        {
            return (GradientStop)base.Clone();
        }

        /// <summary>
        ///     Shadows inherited CloneCurrentValue() with a strongly typed
        ///     version for convenience.
        /// </summary>
        public new GradientStop CloneCurrentValue()
        {
            return (GradientStop)base.CloneCurrentValue();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new GradientStop();
        }

        #endregion

        #region Properties

        public static readonly DependencyProperty ColorProperty
            = RegisterProperty("Color",
                               typeof(Color),
                               typeof(GradientStop),
                               Colors.Transparent,
                               null,
                               null,
                               /* isIndependentlyAnimated  = */ false,
                               /* coerceValueCallback */ null);
        public Color? Color
        {
            get { return (Color?)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public static readonly DependencyProperty OffsetProperty
            = RegisterProperty("Offset",
                               typeof(float),
                               typeof(GradientStop),
                               0f,
                               null,
                               null,
                               /* isIndependentlyAnimated  = */ false,
                               /* coerceValueCallback */ null);
        public float Offset
        {
            get { return (float)GetValue(OffsetProperty)!; }
            set { SetValue(OffsetProperty, value); }
        }

        #endregion

        #region ToString

        /// <summary>
        /// Creates a string representation of this object based on the current culture.
        /// </summary>
        /// <returns>
        /// A string representation of this object.
        /// </returns>
        public override string ToString()
        {
            ReadPreamble();
            // Delegate to the internal method which implements all ToString calls.
            return ConvertToString(null /* format string */, null /* format provider */);
        }

        /// <summary>
        /// Creates a string representation of this object based on the IFormatProvider
        /// passed in.  If the provider is null, the CurrentCulture is used.
        /// </summary>
        /// <returns>
        /// A string representation of this object.
        /// </returns>
        public string ToString(IFormatProvider provider)
        {
            ReadPreamble();
            // Delegate to the internal method which implements all ToString calls.
            return ConvertToString(null /* format string */, provider);
        }

        /// <summary>
        /// Creates a string representation of this object based on the format string
        /// and IFormatProvider passed in.
        /// If the provider is null, the CurrentCulture is used.
        /// See the documentation for IFormattable for more information.
        /// </summary>
        /// <returns>
        /// A string representation of this object.
        /// </returns>
        string IFormattable.ToString(string? format, IFormatProvider? provider)
        {
            ReadPreamble();
            // Delegate to the internal method which implements all ToString calls.
            return ConvertToString(format, provider);
        }

        /// <summary>
        /// Creates a string representation of this object based on the format string
        /// and IFormatProvider passed in.
        /// If the provider is null, the CurrentCulture is used.
        /// See the documentation for IFormattable for more information.
        /// </summary>
        /// <returns>
        /// A string representation of this object.
        /// </returns>
        internal string ConvertToString(string? format, IFormatProvider? provider)
        {
            // Helper to get the numeric list separator for a given culture.
            char separator = TokenizerHelper.GetNumericListSeparator(provider);
            return String.Format(provider,
                                 "{1:" + format + "}{0}{2:" + format + "}",
                                 separator,
                                 Color,
                                 Offset);
        }

        #endregion
    }
}
