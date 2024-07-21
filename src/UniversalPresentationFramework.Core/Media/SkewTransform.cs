using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    public sealed class SkewTransform : Transform
    {
        #region Consturctors

        ///<summary>
        /// 
        ///</summary>
        public SkewTransform()
        {
        }

        ///<summary>
        ///
        ///</summary>
        public SkewTransform(float angleX, float angleY)
        {
            AngleX = angleX;
            AngleY = angleY;
        }

        ///<summary>
        ///
        ///</summary>
        public SkewTransform(float angleX, float angleY, float centerX, float centerY) : this(angleX, angleY)
        {
            CenterX = centerX;
            CenterY = centerY;
        }

        #endregion

        #region Properties

        public static readonly DependencyProperty AngleXProperty = DependencyProperty.Register("AngleX",
                                   typeof(float),
                                   typeof(SkewTransform),
                                   new PropertyMetadata(0f));
        public float AngleX
        {
            get { return (float)GetValue(AngleXProperty)!; }
            set { SetValue(AngleXProperty, value); }
        }

        public static readonly DependencyProperty AngleYProperty = DependencyProperty.Register("AngleY",
                                   typeof(float),
                                   typeof(SkewTransform),
                                   new PropertyMetadata(0f));
        public float AngleY
        {
            get { return (float)GetValue(AngleYProperty)!; }
            set { SetValue(AngleYProperty, value); }
        }


        public static readonly DependencyProperty CenterXProperty = DependencyProperty.Register("CenterX",
                                   typeof(float),
                                   typeof(SkewTransform),
                                   new PropertyMetadata(0f));
        public float CenterX
        {
            get { return (float)GetValue(CenterXProperty)!; }
            set { SetValue(CenterXProperty, value); }
        }

        public static readonly DependencyProperty CenterYProperty = DependencyProperty.Register("CenterY",
                                   typeof(float),
                                   typeof(SkewTransform),
                                   new PropertyMetadata(0f));
        public float CenterY
        {
            get { return (float)GetValue(CenterYProperty)!; }
            set { SetValue(CenterYProperty, value); }
        }

        public override Matrix3x2 Value => Matrix3x2.CreateSkew(AngleX, AngleY, new Vector2(CenterX, CenterY));

        #endregion

        #region Freezable

        protected override Freezable CreateInstanceCore() => new SkewTransform();

        /// <summary>
        ///     Shadows inherited Clone() with a strongly typed
        ///     version for convenience.
        /// </summary>
        public new SkewTransform Clone()
        {
            return (SkewTransform)base.Clone();
        }

        /// <summary>
        ///     Shadows inherited CloneCurrentValue() with a strongly typed
        ///     version for convenience.
        /// </summary>
        public new SkewTransform CloneCurrentValue()
        {
            return (SkewTransform)base.CloneCurrentValue();
        }

        #endregion

        #region Format

        protected override string? ConvertToString(string? format, IFormatProvider? provider)
        {
            return $"{{AngleX: {AngleX}, AngleY: {AngleY}, CenterX: {CenterX}, CenterY: {CenterY}}}";
        }

        #endregion
    }
}
