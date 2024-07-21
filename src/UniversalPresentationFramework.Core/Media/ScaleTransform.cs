using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    public sealed class ScaleTransform : Transform
    {
        #region Constructors

        ///<summary>
        /// Create a scale transformation.
        ///</summary>
        public ScaleTransform()
        {
        }

        ///<summary>
        /// Create a scale transformation.
        ///</summary>
        public ScaleTransform(
            float scaleX,
            float scaleY
            )
        {
            ScaleX = scaleX;
            ScaleY = scaleY;
        }

        ///<summary>
        /// Create a scale transformation.
        ///</summary>
        public ScaleTransform(
            float scaleX,
            float scaleY,
            float centerX,
            float centerY
            ) : this(scaleX, scaleY)
        {
            CenterX = centerX;
            CenterY = centerY;
        }

        #endregion

        #region Properties

        public static readonly DependencyProperty ScaleXProperty = DependencyProperty.Register("ScaleX",
                                   typeof(float),
                                   typeof(ScaleTransform),
                                   new PropertyMetadata(1f));
        public float ScaleX
        {
            get { return (float)GetValue(ScaleXProperty)!; }
            set { SetValue(ScaleXProperty, value); }
        }

        public static readonly DependencyProperty ScaleYProperty = DependencyProperty.Register("ScaleY",
                                   typeof(float),
                                   typeof(ScaleTransform),
                                   new PropertyMetadata(1f));
        public float ScaleY
        {
            get { return (float)GetValue(ScaleYProperty)!; }
            set { SetValue(ScaleYProperty, value); }
        }

        public static readonly DependencyProperty CenterXProperty = DependencyProperty.Register("CenterX",
                                   typeof(float),
                                   typeof(ScaleTransform),
                                   new PropertyMetadata(0f));
        public float CenterX
        {
            get { return (float)GetValue(CenterXProperty)!; }
            set { SetValue(CenterXProperty, value); }
        }

        public static readonly DependencyProperty CenterYProperty = DependencyProperty.Register("CenterY",
                                   typeof(float),
                                   typeof(ScaleTransform),
                                   new PropertyMetadata(0f));
        public float CenterY
        {
            get { return (float)GetValue(CenterYProperty)!; }
            set { SetValue(CenterYProperty, value); }
        }

        public override Matrix3x2 Value => Matrix3x2.CreateScale(ScaleX, ScaleY, new Vector2(CenterX, CenterY));

        #endregion

        #region Freezable

        protected override Freezable CreateInstanceCore() => new ScaleTransform();

        /// <summary>
        ///     Shadows inherited Clone() with a strongly typed
        ///     version for convenience.
        /// </summary>
        public new ScaleTransform Clone()
        {
            return (ScaleTransform)base.Clone();
        }

        /// <summary>
        ///     Shadows inherited CloneCurrentValue() with a strongly typed
        ///     version for convenience.
        /// </summary>
        public new ScaleTransform CloneCurrentValue()
        {
            return (ScaleTransform)base.CloneCurrentValue();
        }

        #endregion

        #region Format

        protected override string? ConvertToString(string? format, IFormatProvider? provider)
        {
            return $"{{ScaleX: {ScaleX}, ScaleY: {ScaleY}, CenterX: {CenterX}, CenterY: {CenterY}}}";
        }

        #endregion
    }
}
