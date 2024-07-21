using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    public sealed class RotateTransform : Transform
    {
        #region Constructors

        ///<summary>
        ///
        ///</summary>
        public RotateTransform()
        {
        }

        ///<summary>
        /// Create a rotation transformation in degrees.
        ///</summary>
        ///<param name="angle">The angle of rotation in degrees.</param>
        public RotateTransform(float angle)
        {
            Angle = angle;
        }

        ///<summary>
        /// Create a rotation transformation in degrees.
        ///</summary>
        public RotateTransform(
            float angle,
            float centerX,
            float centerY
            ) : this(angle)
        {
            CenterX = centerX;
            CenterY = centerY;
        }

        #endregion

        #region Properties

        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register("Angle",
                                   typeof(float),
                                   typeof(RotateTransform),
                                   new PropertyMetadata(0f));
        public float Angle
        {
            get { return (float)GetValue(AngleProperty)!; }
            set { SetValue(AngleProperty, value); }
        }

        public static readonly DependencyProperty CenterXProperty = DependencyProperty.Register("CenterX",
                                   typeof(float),
                                   typeof(RotateTransform),
                                   new PropertyMetadata(0f));
        public float CenterX
        {
            get { return (float)GetValue(CenterXProperty)!; }
            set { SetValue(CenterXProperty, value); }
        }

        public static readonly DependencyProperty CenterYProperty = DependencyProperty.Register("CenterY",
                                   typeof(float),
                                   typeof(RotateTransform),
                                   new PropertyMetadata(0f));
        public float CenterY
        {
            get { return (float)GetValue(CenterYProperty)!; }
            set { SetValue(CenterYProperty, value); }
        }

        public override Matrix3x2 Value => Matrix3x2.CreateRotation(Angle, new Vector2(CenterX, CenterY));

        #endregion

        #region Freezable

        protected override Freezable CreateInstanceCore() => new RotateTransform();

        /// <summary>
        ///     Shadows inherited Clone() with a strongly typed
        ///     version for convenience.
        /// </summary>
        public new RotateTransform Clone()
        {
            return (RotateTransform)base.Clone();
        }

        /// <summary>
        ///     Shadows inherited CloneCurrentValue() with a strongly typed
        ///     version for convenience.
        /// </summary>
        public new RotateTransform CloneCurrentValue()
        {
            return (RotateTransform)base.CloneCurrentValue();
        }

        #endregion

        #region Format

        protected override string? ConvertToString(string? format, IFormatProvider? provider)
        {
            return $"{{Angle: {Angle}, CenterX: {CenterX}, CenterY: {CenterY}}}";
        }

        #endregion
    }
}
