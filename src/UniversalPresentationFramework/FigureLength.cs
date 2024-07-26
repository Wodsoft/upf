using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    [TypeConverter(typeof(FigureLengthConverter))]
    public struct FigureLength : IEquatable<FigureLength>
    {
        #region Constructors

        /// <summary>
        /// Constructor, initializes the FigureLength as absolute value in pixels.
        /// </summary>
        /// <param name="pixels">Specifies the number of 'device-independent pixels' 
        /// (96 pixels-per-inch).</param>
        /// <exception cref="ArgumentException">
        /// If <c>pixels</c> parameter is <c>float.NaN</c>
        /// or <c>pixels</c> parameter is <c>float.NegativeInfinity</c>
        /// or <c>pixels</c> parameter is <c>float.PositiveInfinity</c>.
        /// or <c>value</c> parameter is <c>negative</c>.
        /// </exception>
        public FigureLength(float pixels)
            : this(pixels, FigureUnitType.Pixel)
        {
        }

        /// <summary>
        /// Constructor, initializes the FigureLength and specifies what kind of value 
        /// it will hold.
        /// </summary>
        /// <param name="value">Value to be stored by this FigureLength 
        /// instance.</param>
        /// <param name="type">Type of the value to be stored by this FigureLength 
        /// instance.</param>
        /// <remarks> 
        /// If the <c>type</c> parameter is <c>FigureUnitType.Auto</c>, 
        /// then passed in value is ignored and replaced with <c>0</c>.
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// If <c>value</c> parameter is <c>float.NaN</c>
        /// or <c>value</c> parameter is <c>float.NegativeInfinity</c>
        /// or <c>value</c> parameter is <c>float.PositiveInfinity</c>.
        /// or <c>value</c> parameter is <c>negative</c>.
        /// </exception>
        public FigureLength(float value, FigureUnitType type)
        {
            float maxColumns = 1000;
            float maxPixel = 1000000;

            if (float.IsNaN(value))
            {
                throw new ArgumentException("NaN is invalid length.", "value");
            }
            if (float.IsInfinity(value))
            {
                throw new ArgumentException("Infinity is invalid length", "value");
            }
            if (value < 0f)
            {
                throw new ArgumentOutOfRangeException("Negative is invalid legnth.", "value");
            }
            if (type != FigureUnitType.Auto
                && type != FigureUnitType.Pixel
                && type != FigureUnitType.Column
                && type != FigureUnitType.Content
                && type != FigureUnitType.Page)
            {
                throw new ArgumentException("Unknown FigureUnitType.", "type");
            }
            if (value > 1.0 && (type == FigureUnitType.Content || type == FigureUnitType.Page))
            {
                throw new ArgumentOutOfRangeException("value");
            }
            if (value > maxColumns && type == FigureUnitType.Column)
            {
                throw new ArgumentOutOfRangeException("value");
            }
            if (value > maxPixel && type == FigureUnitType.Pixel)
            {
                throw new ArgumentOutOfRangeException("value");
            }

            _unitValue = (type == FigureUnitType.Auto) ? 0f : value;
            _unitType = type;
        }

        #endregion Constructors

        //------------------------------------------------------
        //
        //  Public Methods
        //
        //------------------------------------------------------

        #region Public Methods 

        /// <summary>
        /// Overloaded operator, compares 2 FigureLengths.
        /// </summary>
        /// <param name="fl1">first FigureLength to compare.</param>
        /// <param name="fl2">second FigureLength to compare.</param>
        /// <returns>true if specified FigureLengths have same value 
        /// and unit type.</returns>
        public static bool operator ==(FigureLength fl1, FigureLength fl2)
        {
            return (fl1.FigureUnitType == fl2.FigureUnitType
                    && fl1.Value == fl2.Value);
        }

        /// <summary>
        /// Overloaded operator, compares 2 FigureLengths.
        /// </summary>
        /// <param name="fl1">first FigureLength to compare.</param>
        /// <param name="fl2">second FigureLength to compare.</param>
        /// <returns>true if specified FigureLengths have either different value or 
        /// unit type.</returns>
        public static bool operator !=(FigureLength fl1, FigureLength fl2)
        {
            return (fl1.FigureUnitType != fl2.FigureUnitType
                    || fl1.Value != fl2.Value);
        }

        /// <summary>
        /// Compares this instance of FigureLength with another object.
        /// </summary>
        /// <param name="oCompare">Reference to an object for comparison.</param>
        /// <returns><c>true</c>if this FigureLength instance has the same value 
        /// and unit type as oCompare.</returns>
        override public bool Equals(object? oCompare)
        {
            if (oCompare is FigureLength l)
            {
                return (this == l);
            }
            else
                return false;
        }

        /// <summary>
        /// Compares this instance of FigureLength with another object.
        /// </summary>
        /// <param name="figureLength">FigureLength to compare.</param>
        /// <returns><c>true</c>if this FigureLength instance has the same value 
        /// and unit type as figureLength.</returns>
        public bool Equals(FigureLength figureLength)
        {
            return (this == figureLength);
        }

        /// <summary>
        /// <see cref="Object.GetHashCode"/>
        /// </summary>
        /// <returns><see cref="Object.GetHashCode"/></returns>
        public override int GetHashCode()
        {
            return ((int)_unitValue + (int)_unitType);
        }

        /// <summary>
        /// Returns <c>true</c> if this FigureLength instance holds 
        /// an absolute (pixel) value.
        /// </summary>
        public bool IsAbsolute { get { return (_unitType == FigureUnitType.Pixel); } }

        /// <summary>
        /// Returns <c>true</c> if this FigureLength instance is 
        /// automatic (not specified).
        /// </summary>
        public bool IsAuto { get { return (_unitType == FigureUnitType.Auto); } }

        /// <summary>
        /// Returns <c>true</c> if this FigureLength instance is column relative.
        /// </summary>
        public bool IsColumn { get { return (_unitType == FigureUnitType.Column); } }

        /// <summary>
        /// Returns <c>true</c> if this FigureLength instance is content relative.
        /// </summary>
        public bool IsContent { get { return (_unitType == FigureUnitType.Content); } }

        /// <summary>
        /// Returns <c>true</c> if this FigureLength instance is page relative.
        /// </summary>
        public bool IsPage { get { return (_unitType == FigureUnitType.Page); } }

        /// <summary>
        /// Returns value part of this FigureLength instance.
        /// </summary>
        public float Value { get { return ((_unitType == FigureUnitType.Auto) ? 1f : _unitValue); } }

        /// <summary>
        /// Returns unit type of this FigureLength instance.
        /// </summary>
        public FigureUnitType FigureUnitType { get { return (_unitType); } }

        /// <summary>
        /// Returns the string representation of this object.
        /// </summary>
        public override string ToString()
        {
            return FigureLengthConverter.ToString(this, CultureInfo.InvariantCulture);
        }

        #endregion Public Methods 

        //------------------------------------------------------
        //
        //  Private Fields
        //
        //------------------------------------------------------

        #region Private Fields 
        private float _unitValue;      //  unit value storage
        private FigureUnitType _unitType; //  unit type storage
        #endregion Private Fields 
    }
}
