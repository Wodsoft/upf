﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    [TypeConverter(typeof(Int32RectConverter))]
    public partial struct Int32Rect : IFormattable
    {
        /// <summary>
        /// Constructor which sets the initial values to the values of the parameters.
        /// </summary>
        public Int32Rect(int x,
                    int y,
                    int width,
                    int height)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }

        /// <summary>
        /// Empty - a static property which provides an Empty Int32Rectangle.
        /// </summary>
        public static Int32Rect Empty
        {
            get
            {
                return _Empty;
            }
        }

        /// <summary>
        /// Returns true if this Int32Rect is the Empty integer rectangle.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return (_x == 0) && (_y == 0) && (_width == 0) && (_height == 0);
            }
        }

        /// <summary>
        /// Returns true if this Int32Rect has area.
        /// </summary>
        public bool HasArea
        {
            get
            {
                return _width > 0 && _height > 0;
            }
        }

        private readonly static Int32Rect _Empty = new Int32Rect(0, 0, 0, 0);

        #region Public Methods

        /// <summary>
        /// Compares two Int32Rect instances for exact equality.
        /// Note that double values can acquire error when operated upon, such that
        /// an exact comparison between two values which are logically equal may fail.
        /// Furthermore, using this equality operator, Double.NaN is not equal to itself.
        /// </summary>
        /// <returns>
        /// bool - true if the two Int32Rect instances are exactly equal, false otherwise
        /// </returns>
        /// <param name='int32Rect1'>The first Int32Rect to compare</param>
        /// <param name='int32Rect2'>The second Int32Rect to compare</param>
        public static bool operator ==(Int32Rect int32Rect1, Int32Rect int32Rect2)
        {
            return int32Rect1.X == int32Rect2.X &&
                   int32Rect1.Y == int32Rect2.Y &&
                   int32Rect1.Width == int32Rect2.Width &&
                   int32Rect1.Height == int32Rect2.Height;
        }

        /// <summary>
        /// Compares two Int32Rect instances for exact inequality.
        /// Note that double values can acquire error when operated upon, such that
        /// an exact comparison between two values which are logically equal may fail.
        /// Furthermore, using this equality operator, Double.NaN is not equal to itself.
        /// </summary>
        /// <returns>
        /// bool - true if the two Int32Rect instances are exactly unequal, false otherwise
        /// </returns>
        /// <param name='int32Rect1'>The first Int32Rect to compare</param>
        /// <param name='int32Rect2'>The second Int32Rect to compare</param>
        public static bool operator !=(Int32Rect int32Rect1, Int32Rect int32Rect2)
        {
            return !(int32Rect1 == int32Rect2);
        }
        /// <summary>
        /// Compares two Int32Rect instances for object equality.  In this equality
        /// Double.NaN is equal to itself, unlike in numeric equality.
        /// Note that double values can acquire error when operated upon, such that
        /// an exact comparison between two values which
        /// are logically equal may fail.
        /// </summary>
        /// <returns>
        /// bool - true if the two Int32Rect instances are exactly equal, false otherwise
        /// </returns>
        /// <param name='int32Rect1'>The first Int32Rect to compare</param>
        /// <param name='int32Rect2'>The second Int32Rect to compare</param>
        public static bool Equals(Int32Rect int32Rect1, Int32Rect int32Rect2)
        {
            if (int32Rect1.IsEmpty)
            {
                return int32Rect2.IsEmpty;
            }
            else
            {
                return int32Rect1.X.Equals(int32Rect2.X) &&
                       int32Rect1.Y.Equals(int32Rect2.Y) &&
                       int32Rect1.Width.Equals(int32Rect2.Width) &&
                       int32Rect1.Height.Equals(int32Rect2.Height);
            }
        }

        /// <summary>
        /// Equals - compares this Int32Rect with the passed in object.  In this equality
        /// Double.NaN is equal to itself, unlike in numeric equality.
        /// Note that double values can acquire error when operated upon, such that
        /// an exact comparison between two values which
        /// are logically equal may fail.
        /// </summary>
        /// <returns>
        /// bool - true if the object is an instance of Int32Rect and if it's equal to "this".
        /// </returns>
        /// <param name='o'>The object to compare to "this"</param>
        public override bool Equals(object? o)
        {
            if (o is Int32Rect value)
            {
                return Equals(this, value);
            }
            return false;
        }

        /// <summary>
        /// Equals - compares this Int32Rect with the passed in object.  In this equality
        /// Double.NaN is equal to itself, unlike in numeric equality.
        /// Note that double values can acquire error when operated upon, such that
        /// an exact comparison between two values which
        /// are logically equal may fail.
        /// </summary>
        /// <returns>
        /// bool - true if "value" is equal to "this".
        /// </returns>
        /// <param name='value'>The Int32Rect to compare to "this"</param>
        public bool Equals(Int32Rect value)
        {
            return Equals(this, value);
        }
        /// <summary>
        /// Returns the HashCode for this Int32Rect
        /// </summary>
        /// <returns>
        /// int - the HashCode for this Int32Rect
        /// </returns>
        public override int GetHashCode()
        {
            if (IsEmpty)
            {
                return 0;
            }
            else
            {
                // Perform field-by-field XOR of HashCodes
                return X.GetHashCode() ^
                       Y.GetHashCode() ^
                       Width.GetHashCode() ^
                       Height.GetHashCode();
            }
        }

        /// <summary>
        /// Parse - returns an instance converted from the provided string using
        /// the culture "en-US"
        /// <param name="source"> string with Int32Rect data </param>
        /// </summary>
        public static Int32Rect Parse(string source)
        {
            IFormatProvider formatProvider = CultureInfo.InvariantCulture;

            TokenizerHelper th = new TokenizerHelper(source, formatProvider);

            Int32Rect value;

            string? firstToken = th.NextTokenRequired();

            // The token will already have had whitespace trimmed so we can do a
            // simple string compare.
            if (firstToken == "Empty")
            {
                value = Empty;
            }
            else
            {
                value = new Int32Rect(
                    Convert.ToInt32(firstToken, formatProvider),
                    Convert.ToInt32(th.NextTokenRequired(), formatProvider),
                    Convert.ToInt32(th.NextTokenRequired(), formatProvider),
                    Convert.ToInt32(th.NextTokenRequired(), formatProvider));
            }

            // There should be no more tokens in this string.
            th.LastTokenRequired();

            return value;
        }

        #endregion Public Methods

        #region Public Properties

        /// <summary>
        ///     X - int.  Default value is 0.
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
        ///     Y - int.  Default value is 0.
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

        /// <summary>
        ///     Width - int.  Default value is 0.
        /// </summary>
        public int Width
        {
            get
            {
                return _width;
            }

            set
            {
                _width = value;
            }
        }

        /// <summary>
        ///     Height - int.  Default value is 0.
        /// </summary>
        public int Height
        {
            get
            {
                return _height;
            }

            set
            {
                _height = value;
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
            if (IsEmpty)
            {
                return "Empty";
            }

            // Helper to get the numeric list separator for a given culture.
            char separator = TokenizerHelper.GetNumericListSeparator(provider);
            return String.Format(provider,
                                 "{1:" + format + "}{0}{2:" + format + "}{0}{3:" + format + "}{0}{4:" + format + "}",
                                 separator,
                                 _x,
                                 _y,
                                 _width,
                                 _height);
        }



        #endregion Internal Properties

        #region Internal Fields


        internal int _x;
        internal int _y;
        internal int _width;
        internal int _height;

        #endregion Internal Fields
    }
}
