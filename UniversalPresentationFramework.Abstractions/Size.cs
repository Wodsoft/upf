using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    [TypeConverter(typeof(SizeConverter))]
    public struct Size : IFormattable
    {
        private float _width;
        private float _height;

        #region Constructors

        /// <summary>
        /// Constructor which sets the size's initial values.  Width and Height must be non-negative
        /// </summary>
        /// <param name="width"> float - The initial Width </param>
        /// <param name="height"> float - THe initial Height </param>
        public Size(float width, float height)
        {
            if (width < 0 || height < 0)
            {
                throw new System.ArgumentException("Width and height can not be negative.");
            }

            _width = width;
            _height = height;
        }

        #endregion Constructors

        #region Statics

        /// <summary>
        /// Empty - a static property which provides an Empty size.  Width and Height are 
        /// negative-infinity.  This is the only situation
        /// where size can be negative.
        /// </summary>
        public static Size Empty
        {
            get
            {
                return _Empty;
            }
        }

        #endregion Statics

        #region Public Methods and Properties

        /// <summary>
        /// IsEmpty - this returns true if this size is the Empty size.
        /// Note: If size is 0 this Size still contains a 0 or 1 dimensional set
        /// of points, so this method should not be used to check for 0 area.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return _width < 0;
            }
        }

        /// <summary>
        /// Width - Default is 0, must be non-negative
        /// </summary>
        public float Width
        {
            get
            {
                return _width;
            }
            set
            {
                if (IsEmpty)
                {
                    throw new System.InvalidOperationException("Can not modify empty size.");
                }

                if (value < 0)
                {
                    throw new System.ArgumentException("Width can not be negative.");
                }

                _width = value;
            }
        }

        /// <summary>
        /// Height - Default is 0, must be non-negative.
        /// </summary>
        public float Height
        {
            get
            {
                return _height;
            }
            set
            {
                if (IsEmpty)
                {
                    throw new System.InvalidOperationException("Can not modify empty size.");
                }

                if (value < 0)
                {
                    throw new System.ArgumentException("Height can not be negative.");
                }

                _height = value;
            }
        }


        /// <summary>
        /// Compares two Size instances for exact equality.
        /// Note that float values can acquire error when operated upon, such that
        /// an exact comparison between two values which are logically equal may fail.
        /// Furthermore, using this equality operator, float.NaN is not equal to itself.
        /// </summary>
        /// <returns>
        /// bool - true if the two Size instances are exactly equal, false otherwise
        /// </returns>
        /// <param name='size1'>The first Size to compare</param>
        /// <param name='size2'>The second Size to compare</param>
        public static bool operator ==(Size size1, Size size2)
        {
            return size1.Width == size2.Width &&
                   size1.Height == size2.Height;
        }

        /// <summary>
        /// Compares two Size instances for exact inequality.
        /// Note that float values can acquire error when operated upon, such that
        /// an exact comparison between two values which are logically equal may fail.
        /// Furthermore, using this equality operator, float.NaN is not equal to itself.
        /// </summary>
        /// <returns>
        /// bool - true if the two Size instances are exactly unequal, false otherwise
        /// </returns>
        /// <param name='size1'>The first Size to compare</param>
        /// <param name='size2'>The second Size to compare</param>
        public static bool operator !=(Size size1, Size size2)
        {
            return !(size1 == size2);
        }
        /// <summary>
        /// Compares two Size instances for object equality.  In this equality
        /// float.NaN is equal to itself, unlike in numeric equality.
        /// Note that float values can acquire error when operated upon, such that
        /// an exact comparison between two values which
        /// are logically equal may fail.
        /// </summary>
        /// <returns>
        /// bool - true if the two Size instances are exactly equal, false otherwise
        /// </returns>
        /// <param name='size1'>The first Size to compare</param>
        /// <param name='size2'>The second Size to compare</param>
        public static bool Equals(Size size1, Size size2)
        {
            if (size1.IsEmpty)
            {
                return size2.IsEmpty;
            }
            else
            {
                return size1.Width.Equals(size2.Width) &&
                       size1.Height.Equals(size2.Height);
            }
        }

        /// <summary>
        /// Equals - compares this Size with the passed in object.  In this equality
        /// float.NaN is equal to itself, unlike in numeric equality.
        /// Note that float values can acquire error when operated upon, such that
        /// an exact comparison between two values which
        /// are logically equal may fail.
        /// </summary>
        /// <returns>
        /// bool - true if the object is an instance of Size and if it's equal to "this".
        /// </returns>
        /// <param name='o'>The object to compare to "this"</param>
        public override bool Equals(object? o)
        {
            if ((null == o) || !(o is Size))
            {
                return false;
            }

            Size value = (Size)o;
            return Size.Equals(this, value);
        }

        /// <summary>
        /// Equals - compares this Size with the passed in object.  In this equality
        /// float.NaN is equal to itself, unlike in numeric equality.
        /// Note that float values can acquire error when operated upon, such that
        /// an exact comparison between two values which
        /// are logically equal may fail.
        /// </summary>
        /// <returns>
        /// bool - true if "value" is equal to "this".
        /// </returns>
        /// <param name='value'>The Size to compare to "this"</param>
        public bool Equals(Size value)
        {
            return Size.Equals(this, value);
        }
        /// <summary>
        /// Returns the HashCode for this Size
        /// </summary>
        /// <returns>
        /// int - the HashCode for this Size
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
                return Width.GetHashCode() ^
                       Height.GetHashCode();
            }
        }

        /// <summary>
        /// Parse - returns an instance converted from the provided string using
        /// the culture "en-US"
        /// <param name="source"> string with Size data </param>
        /// </summary>
        public static Size Parse(string source)
        {
            IFormatProvider formatProvider = CultureInfo.InvariantCulture;

            TokenizerHelper th = new TokenizerHelper(source, formatProvider);

            Size value;

            string? firstToken = th.NextTokenRequired();

            // The token will already have had whitespace trimmed so we can do a
            // simple string compare.
            if (firstToken == "Empty")
            {
                value = Empty;
            }
            else
            {
                value = new Size(
                    Convert.ToSingle(firstToken, formatProvider),
                    Convert.ToSingle(th.NextTokenRequired(), formatProvider));
            }

            // There should be no more tokens in this string.
            th.LastTokenRequired();

            return value;
        }

        #endregion Public Methods

        #region Public Operators

        /// <summary>
        /// Explicit conversion to Vector.
        /// </summary>
        /// <returns>
        /// Vector - A Vector equal to this Size
        /// </returns>
        /// <param name="size"> Size - the Size to convert to a Vector </param>
        public static explicit operator Vector2(Size size)
        {
            return new Vector2(size._width, size._height);
        }

        /// <summary>
        /// Explicit conversion to Point
        /// </summary>
        /// <returns>
        /// Point - A Point equal to this Size
        /// </returns>
        /// <param name="size"> Size - the Size to convert to a Point </param>
        public static explicit operator Point(Size size)
        {
            return new Point(size._width, size._height);
        }

        #endregion Public Operators

        #region Private Methods

        static private Size CreateEmptySize()
        {
            Size size = new Size();
            // We can't set these via the property setters because negatives widths
            // are rejected in those APIs.
            size._width = float.NegativeInfinity;
            size._height = float.NegativeInfinity;
            return size;
        }

        #endregion Private Methods

        #region Private Fields

        private readonly static Size _Empty = CreateEmptySize();

        #endregion Private Fields


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
                                 "{1:" + format + "}{0}{2:" + format + "}",
                                 separator,
                                 _width,
                                 _height);
        }



        #endregion Internal Properties
    }
}
