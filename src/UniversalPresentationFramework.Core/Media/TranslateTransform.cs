using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    public sealed class TranslateTransform : Transform
    {
        #region Constructors

        ///<summary>
        ///
        ///</summary>
        public TranslateTransform()
        {
        }

        ///<summary>
        /// Create a translation transformation.
        ///</summary>
        ///<param name="offsetX">Displacement amount in x direction.</param>
        ///<param name="offsetY">Displacement amount in y direction.</param>
        public TranslateTransform(
            float offsetX,
            float offsetY
            )
        {
            X = offsetX;
            Y = offsetY;
        }

        #endregion

        #region Properties

        public static readonly DependencyProperty XProperty = DependencyProperty.Register("X",
                                   typeof(float),
                                   typeof(TranslateTransform),
                                   new PropertyMetadata(1f));
        public float X
        {
            get { return (float)GetValue(XProperty)!; }
            set { SetValue(XProperty, value); }
        }

        public static readonly DependencyProperty YProperty = DependencyProperty.Register("Y",
                                   typeof(float),
                                   typeof(TranslateTransform),
                                   new PropertyMetadata(1f));
        public float Y
        {
            get { return (float)GetValue(YProperty)!; }
            set { SetValue(YProperty, value); }
        }

        public override Matrix3x2 Value => Matrix3x2.CreateTranslation(X, Y);

        #endregion

        #region Freezable

        protected override Freezable CreateInstanceCore() => new TranslateTransform();

        /// <summary>
        ///     Shadows inherited Clone() with a strongly typed
        ///     version for convenience.
        /// </summary>
        public new TranslateTransform Clone()
        {
            return (TranslateTransform)base.Clone();
        }

        /// <summary>
        ///     Shadows inherited CloneCurrentValue() with a strongly typed
        ///     version for convenience.
        /// </summary>
        public new TranslateTransform CloneCurrentValue()
        {
            return (TranslateTransform)base.CloneCurrentValue();
        }

        #endregion

        #region Format

        protected override string? ConvertToString(string? format, IFormatProvider? provider)
        {
            return $"{{X: {X}, Y: {Y}}}";
        }

        #endregion
    }
}
