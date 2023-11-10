using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    [TypeConverter(typeof(ThicknessConverter))]
    public struct Thickness : IEquatable<Thickness>
    {
        //-------------------------------------------------------------------
        //
        //  Constructors
        //
        //-------------------------------------------------------------------

        #region Constructors
        /// <summary>
        /// This constructur builds a Thickness with a specified value on every side.
        /// </summary>
        /// <param name="uniformLength">The specified uniform length.</param>
        public Thickness(float uniformLength)
        {
            _left = _top = _right = _bottom = uniformLength;
        }

        /// <summary>
        /// This constructor builds a Thickness with the specified number of pixels on each side.
        /// </summary>
        /// <param name="left">The thickness for the left side.</param>
        /// <param name="top">The thickness for the top side.</param>
        /// <param name="right">The thickness for the right side.</param>
        /// <param name="bottom">The thickness for the bottom side.</param>
        public Thickness(float left, float top, float right, float bottom)
        {
            _left = left;
            _top = top;
            _right = right;
            _bottom = bottom;
        }


        #endregion


        //-------------------------------------------------------------------
        //
        //  Public Methods
        //
        //-------------------------------------------------------------------

        #region Public Methods

        /// <summary>
        /// This function compares to the provided object for type and value equality.
        /// </summary>
        /// <param name="obj">Object to compare</param>
        /// <returns>True if object is a Thickness and all sides of it are equal to this Thickness'.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is Thickness otherObj)
            {
                return (this == otherObj);
            }
            return (false);
        }

        /// <summary>
        /// Compares this instance of Thickness with another instance.
        /// </summary>
        /// <param name="thickness">Thickness instance to compare.</param>
        /// <returns><c>true</c>if this Thickness instance has the same value 
        /// and unit type as thickness.</returns>
        public bool Equals(Thickness thickness)
        {
            return (this == thickness);
        }

        /// <summary>
        /// This function returns a hash code.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return _left.GetHashCode() ^ _top.GetHashCode() ^ _right.GetHashCode() ^ _bottom.GetHashCode();
        }

        /// <summary>
        /// Converts this Thickness object to a string.
        /// </summary>
        /// <returns>String conversion.</returns>
        public override string ToString()
        {
            return ThicknessConverter.ToString(this, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts this Thickness object to a string.
        /// </summary>
        /// <returns>String conversion.</returns>
        internal string ToString(CultureInfo cultureInfo)
        {
            return ThicknessConverter.ToString(this, cultureInfo);
        }

        internal bool IsZero
        {
            get
            {
                return FloatUtil.IsZero(Left)
                        && FloatUtil.IsZero(Top)
                        && FloatUtil.IsZero(Right)
                        && FloatUtil.IsZero(Bottom);
            }
        }

        internal bool IsUniform
        {
            get
            {
                return FloatUtil.AreClose(Left, Top)
                        && FloatUtil.AreClose(Left, Right)
                        && FloatUtil.AreClose(Left, Bottom);
            }
        }

        /// <summary>
        /// Verifies if this Thickness contains only valid values
        /// The set of validity checks is passed as parameters.
        /// </summary>
        /// <param name='allowNegative'>allows negative values</param>
        /// <param name='allowNaN'>allows float.NaN</param>
        /// <param name='allowPositiveInfinity'>allows float.PositiveInfinity</param>
        /// <param name='allowNegativeInfinity'>allows float.NegativeInfinity</param>
        /// <returns>Whether or not the thickness complies to the range specified</returns>
        internal bool IsValid(bool allowNegative, bool allowNaN, bool allowPositiveInfinity, bool allowNegativeInfinity)
        {
            if (!allowNegative)
            {
                if (Left < 0d || Right < 0d || Top < 0d || Bottom < 0d)
                    return false;
            }

            if (!allowNaN)
            {
                if (float.IsNaN(Left) || float.IsNaN(Right) || float.IsNaN(Top) || float.IsNaN(Bottom))
                    return false;
            }

            if (!allowPositiveInfinity)
            {
                if (float.IsPositiveInfinity(Left) || float.IsPositiveInfinity(Right) || float.IsPositiveInfinity(Top) || float.IsPositiveInfinity(Bottom))
                {
                    return false;
                }
            }

            if (!allowNegativeInfinity)
            {
                if (float.IsNegativeInfinity(Left) || float.IsNegativeInfinity(Right) || float.IsNegativeInfinity(Top) || float.IsNegativeInfinity(Bottom))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Compares two thicknesses for fuzzy equality.  This function
        /// helps compensate for the fact that float values can 
        /// acquire error when operated upon
        /// </summary>
        /// <param name='thickness'>The thickness to compare to this</param>
        /// <returns>Whether or not the two points are equal</returns>
        internal bool IsClose(Thickness thickness)
        {
            return (FloatUtil.AreClose(Left, thickness.Left)
                    && FloatUtil.AreClose(Top, thickness.Top)
                    && FloatUtil.AreClose(Right, thickness.Right)
                    && FloatUtil.AreClose(Bottom, thickness.Bottom));
        }

        /// <summary>
        /// Compares two thicknesses for fuzzy equality.  This function
        /// helps compensate for the fact that float values can 
        /// acquire error when operated upon
        /// </summary>
        /// <param name='thickness0'>The first thickness to compare</param>
        /// <param name='thickness1'>The second thickness to compare</param>
        /// <returns>Whether or not the two thicknesses are equal</returns>
        static internal bool AreClose(Thickness thickness0, Thickness thickness1)
        {
            return thickness0.IsClose(thickness1);
        }

        #endregion


        //-------------------------------------------------------------------
        //
        //  Public Operators
        //
        //-------------------------------------------------------------------

        #region Public Operators

        /// <summary>
        /// Overloaded operator to compare two Thicknesses for equality.
        /// </summary>
        /// <param name="t1">first Thickness to compare</param>
        /// <param name="t2">second Thickness to compare</param>
        /// <returns>True if all sides of the Thickness are equal, false otherwise</returns>
        //  SEEALSO
        public static bool operator ==(Thickness t1, Thickness t2)
        {
            return ((t1._left == t2._left || (float.IsNaN(t1._left) && float.IsNaN(t2._left)))
                    && (t1._top == t2._top || (float.IsNaN(t1._top) && float.IsNaN(t2._top)))
                    && (t1._right == t2._right || (float.IsNaN(t1._right) && float.IsNaN(t2._right)))
                    && (t1._bottom == t2._bottom || (float.IsNaN(t1._bottom) && float.IsNaN(t2._bottom)))
                    );
        }

        /// <summary>
        /// Overloaded operator to compare two Thicknesses for inequality.
        /// </summary>
        /// <param name="t1">first Thickness to compare</param>
        /// <param name="t2">second Thickness to compare</param>
        /// <returns>False if all sides of the Thickness are equal, true otherwise</returns>
        //  SEEALSO
        public static bool operator !=(Thickness t1, Thickness t2)
        {
            return (!(t1 == t2));
        }

        #endregion


        //-------------------------------------------------------------------
        //
        //  Public Properties
        //
        //-------------------------------------------------------------------

        #region Public Properties

        /// <summary>This property is the Length on the thickness' left side</summary>
        public float Left
        {
            get { return _left; }
            set { _left = value; }
        }

        /// <summary>This property is the Length on the thickness' top side</summary>
        public float Top
        {
            get { return _top; }
            set { _top = value; }
        }

        /// <summary>This property is the Length on the thickness' right side</summary>
        public float Right
        {
            get { return _right; }
            set { _right = value; }
        }

        /// <summary>This property is the Length on the thickness' bottom side</summary>
        public float Bottom
        {
            get { return _bottom; }
            set { _bottom = value; }
        }
        #endregion

        //-------------------------------------------------------------------
        //
        //  INternal API
        //
        //-------------------------------------------------------------------

        #region Internal API

        internal Size Size
        {
            get
            {
                return new Size(_left + _right, _top + _bottom);
            }
        }

        #endregion

        //-------------------------------------------------------------------
        //
        //  Private Fields
        //
        //-------------------------------------------------------------------

        #region Private Fields

        private float _left;
        private float _top;
        private float _right;
        private float _bottom;

        #endregion
    }
}
