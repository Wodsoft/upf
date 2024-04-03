using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    [TypeConverter(typeof(Int32PointConverter))]
    public struct Int32Point : IFormattable
    {
        private int _x;
        private int _y;

        #region Constructors

        /// <summary>
        /// Constructor which accepts the X and Y values
        /// </summary>
        /// <param name="x">The value for the X coordinate of the new Int32Point</param>
        /// <param name="y">The value for the Y coordinate of the new Int32Point</param>
        public Int32Point(int x, int y)
        {
            _x = x;
            _y = y;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Offset - update the location by adding offsetX to X and offsetY to Y
        /// </summary>
        /// <param name="offsetX"> The offset in the x dimension </param>
        /// <param name="offsetY"> The offset in the y dimension </param>
        public void Offset(int offsetX, int offsetY)
        {
            _x += offsetX;
            _y += offsetY;
        }

        /// <summary>
        /// Operator Int32Point - Int32Point
        /// </summary>
        /// <returns>
        /// Vector2 - The result of the subtraction
        /// </returns>
        /// <param name="point1"> The Int32Point from which point2 is subtracted </param>
        /// <param name="point2"> The Int32Point subtracted from point1 </param>
        public static Vector2 operator -(Int32Point point1, Int32Point point2)
        {
            return new Vector2(point1._x - point2._x, point1._y - point2._y);
        }

        /// <summary>
        /// Subtract: Int32Point - Int32Point
        /// </summary>
        /// <returns>
        /// Vector2 - The result of the subtraction
        /// </returns>
        /// <param name="point1"> The Int32Point from which point2 is subtracted </param>
        /// <param name="point2"> The Int32Point subtracted from point1 </param>
        public static Vector2 Subtract(Int32Point point1, Int32Point point2)
        {
            return new Vector2(point1._x - point2._x, point1._y - point2._y);
        }

        /// <summary>
        /// Operator Int32Point * Matrix
        /// </summary>
        public static Int32Point operator *(Int32Point Int32Point, Matrix3x2 matrix)
        {
            ref var vector = ref Unsafe.As<Int32Point, Vector2>(ref Int32Point);
            vector = Vector2.Transform(vector, matrix);
            return Int32Point;
        }

        /// <summary>
        /// Multiply: Int32Point * Matrix
        /// </summary>
        public static Int32Point Multiply(Int32Point Int32Point, Matrix3x2 matrix)
        {
            ref var vector = ref Unsafe.As<Int32Point, Vector2>(ref Int32Point);
            vector = Vector2.Transform(vector, matrix);
            return Int32Point;
        }

        /// <summary>
        /// Explicit conversion to Size.  Note that since Size cannot contain negative values,
        /// the resulting size will contains the absolute values of X and Y
        /// </summary>
        /// <returns>
        /// Size - A Size equal to this Int32Point
        /// </returns>
        /// <param name="Int32Point"> Int32Point - the Int32Point to convert to a Size </param>
        public static explicit operator Size(Int32Point Int32Point)
        {
            return new Size(Math.Abs(Int32Point._x), Math.Abs(Int32Point._y));
        }

        /// <summary>
        /// Explicit conversion to Vector
        /// </summary>
        /// <returns>
        /// Vector2 - A Vector2 equal to this Int32Point
        /// </returns>
        /// <param name="Int32Point"> Int32Point - the Int32Point to convert to a Vector2 </param>
        public static explicit operator Vector2(Int32Point Int32Point)
        {
            return new Vector2(Int32Point._x, Int32Point._y);
        }


        /// <summary>
        /// Compares two Int32Point instances for exact equality.
        /// Note that double values can acquire error when operated upon, such that
        /// an exact comparison between two values which are logically equal may fail.
        /// Furthermore, using this equality operator, Double.NaN is not equal to itself.
        /// </summary>
        /// <returns>
        /// bool - true if the two Int32Point instances are exactly equal, false otherwise
        /// </returns>
        /// <param name='point1'>The first Int32Point to compare</param>
        /// <param name='point2'>The second Int32Point to compare</param>
        public static bool operator ==(Int32Point point1, Int32Point point2)
        {
            return point1.X == point2.X &&
                   point1.Y == point2.Y;
        }

        /// <summary>
        /// Compares two Int32Point instances for exact inequality.
        /// Note that double values can acquire error when operated upon, such that
        /// an exact comparison between two values which are logically equal may fail.
        /// Furthermore, using this equality operator, Double.NaN is not equal to itself.
        /// </summary>
        /// <returns>
        /// bool - true if the two Int32Point instances are exactly unequal, false otherwise
        /// </returns>
        /// <param name='point1'>The first Int32Point to compare</param>
        /// <param name='point2'>The second Int32Point to compare</param>
        public static bool operator !=(Int32Point point1, Int32Point point2)
        {
            return !(point1 == point2);
        }
        /// <summary>
        /// Compares two Int32Point instances for object equality.  In this equality
        /// Double.NaN is equal to itself, unlike in numeric equality.
        /// Note that double values can acquire error when operated upon, such that
        /// an exact comparison between two values which
        /// are logically equal may fail.
        /// </summary>
        /// <returns>
        /// bool - true if the two Int32Point instances are exactly equal, false otherwise
        /// </returns>
        /// <param name='point1'>The first Int32Point to compare</param>
        /// <param name='point2'>The second Int32Point to compare</param>
        public static bool Equals(Int32Point point1, Int32Point point2)
        {
            return point1.X.Equals(point2.X) &&
                   point1.Y.Equals(point2.Y);
        }

        /// <summary>
        /// Equals - compares this Int32Point with the passed in object.  In this equality
        /// Double.NaN is equal to itself, unlike in numeric equality.
        /// Note that double values can acquire error when operated upon, such that
        /// an exact comparison between two values which
        /// are logically equal may fail.
        /// </summary>
        /// <returns>
        /// bool - true if the object is an instance of Int32Point and if it's equal to "this".
        /// </returns>
        /// <param name='o'>The object to compare to "this"</param>
        public override bool Equals(object? o)
        {
            if ((null == o) || !(o is Int32Point))
            {
                return false;
            }

            Int32Point value = (Int32Point)o;
            return Int32Point.Equals(this, value);
        }

        /// <summary>
        /// Equals - compares this Int32Point with the passed in object.  In this equality
        /// Double.NaN is equal to itself, unlike in numeric equality.
        /// Note that double values can acquire error when operated upon, such that
        /// an exact comparison between two values which
        /// are logically equal may fail.
        /// </summary>
        /// <returns>
        /// bool - true if "value" is equal to "this".
        /// </returns>
        /// <param name='value'>The Int32Point to compare to "this"</param>
        public bool Equals(Int32Point value)
        {
            return Int32Point.Equals(this, value);
        }
        /// <summary>
        /// Returns the HashCode for this Int32Point
        /// </summary>
        /// <returns>
        /// int - the HashCode for this Int32Point
        /// </returns>
        public override int GetHashCode()
        {
            // Perform field-by-field XOR of HashCodes
            return X.GetHashCode() ^
                   Y.GetHashCode();
        }

        /// <summary>
        /// Parse - returns an instance converted from the provided string using
        /// the culture "en-US"
        /// <param name="source"> string with Int32Point data </param>
        /// </summary>
        public static Int32Point Parse(string source)
        {
            IFormatProvider formatProvider = CultureInfo.InvariantCulture;

            TokenizerHelper th = new TokenizerHelper(source, formatProvider);

            Int32Point value;

            string? firstToken = th.NextTokenRequired();

            value = new Int32Point(
                Convert.ToInt32(firstToken, formatProvider),
                Convert.ToInt32(th.NextTokenRequired(), formatProvider));

            // There should be no more tokens in this string.
            th.LastTokenRequired();

            return value;
        }

        #endregion Public Methods

        #region Public Properties

        /// <summary>
        ///     X - double.  Default value is 0.
        /// </summary>
        public int X
        {
            get
            {
                return _x;
            }

            set
            {
                _x = value;
            }
        }

        /// <summary>
        ///     Y - double.  Default value is 0.
        /// </summary>
        public int Y
        {
            get
            {
                return _y;
            }

            set
            {
                _y = value;
            }
        }

        #endregion Public Properties

        #region Internal Properties


        /// <summary>
        /// Creates a string representation of this object based on the current culture.
        /// </summary>
        /// <returns>
        /// A string representation of this object.
        /// </returns>
        public override string ToString()
        {
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
        public string ToString(IFormatProvider? provider)
        {
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
                                 _x,
                                 _y);
        }

        #endregion Internal Properties
    }
}
