using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    [TypeConverter(typeof(RepeatBehaviorConverter))]
    public struct RepeatBehavior : IFormattable
    {
        private float _iterationCount;
        private TimeSpan _repeatDuration;
        private RepeatBehaviorType _type;

        #region Constructors

        /// <summary>
        /// Creates a new RepeatBehavior that represents and iteration count.
        /// </summary>
        /// <param name="count">The number of iterations specified by this RepeatBehavior.</param>
        public RepeatBehavior(float count)
        {
            if (float.IsInfinity(count)
                || float.IsNaN(count)
                || count < 0.0)
            {
                throw new ArgumentOutOfRangeException("count", "Invalid iteration count.");
            }

            _repeatDuration = new TimeSpan(0);
            _iterationCount = count;
            _type = RepeatBehaviorType.IterationCount;
        }

        /// <summary>
        /// Creates a new RepeatBehavior that represents a repeat duration for which a Timeline will repeat
        /// its simple duration.
        /// </summary>
        /// <param name="duration">A TimeSpan representing the repeat duration specified by this RepeatBehavior.</param>
        public RepeatBehavior(TimeSpan duration)
        {
            if (duration < new TimeSpan(0))
            {
                throw new ArgumentOutOfRangeException("duration", "Duration can't be negative.");
            }

            _iterationCount = 0.0f;
            _repeatDuration = duration;
            _type = RepeatBehaviorType.RepeatDuration;
        }

        /// <summary>
        /// Creates and returns a RepeatBehavior that indicates that a Timeline should repeat its
        /// simple duration forever.
        /// </summary>
        /// <value>A RepeatBehavior that indicates that a Timeline should repeat its simple duration
        /// forever.</value>
        public static RepeatBehavior Forever
        {
            get
            {
                RepeatBehavior forever = new RepeatBehavior();
                forever._type = RepeatBehaviorType.Forever;

                return forever;
            }
        }

        #endregion // Constructors

        #region Properties

        /// <summary>
        /// Indicates whether this RepeatBehavior represents an iteration count.
        /// </summary>
        /// <value>True if this RepeatBehavior represents an iteration count; otherwise false.</value>
        public bool HasCount
        {
            get
            {
                return _type == RepeatBehaviorType.IterationCount;
            }
        }

        /// <summary>
        /// Indicates whether this RepeatBehavior represents a repeat duration.
        /// </summary>
        /// <value>True if this RepeatBehavior represents a repeat duration; otherwise false.</value>
        public bool HasDuration
        {
            get
            {
                return _type == RepeatBehaviorType.RepeatDuration;
            }
        }

        /// <summary>
        /// Returns the iteration count specified by this RepeatBehavior.
        /// </summary>
        /// <value>The iteration count specified by this RepeatBehavior.</value>
        /// <exception cref="System.InvalidOperationException">Thrown if this RepeatBehavior does not represent an iteration count.</exception>
        public float Count
        {
            get
            {
                if (_type != RepeatBehaviorType.IterationCount)
                {
                    throw new InvalidOperationException("There is no iteration count if not IterationCount.");
                }

                return _iterationCount;
            }
        }

        /// <summary>
        /// Returns the repeat duration specified by this RepeatBehavior.
        /// </summary>
        /// <value>A TimeSpan representing the repeat duration specified by this RepeatBehavior.</value>
        /// <exception cref="System.InvalidOperationException">Thrown if this RepeatBehavior does not represent a repeat duration.</exception>
        public TimeSpan Duration
        {
            get
            {
                if (_type != RepeatBehaviorType.RepeatDuration)
                {
                    throw new InvalidOperationException("There is no duration if not RepeatDuration.");
                }

                return _repeatDuration;
            }
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Indicates whether the specified Object is equal to this RepeatBehavior.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>true if value is a RepeatBehavior and is equal to this instance; otherwise false.</returns>
        public override bool Equals(object? value)
        {
            if (value is RepeatBehavior repeatBehavior)
            {
                return Equals(repeatBehavior);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Indicates whether the specified RepeatBehavior is equal to this RepeatBehavior.
        /// </summary>
        /// <param name="repeatBehavior">A RepeatBehavior to compare with this RepeatBehavior.</param>
        /// <returns>true if repeatBehavior is equal to this instance; otherwise false.</returns>
        public bool Equals(RepeatBehavior repeatBehavior)
        {
            if (_type == repeatBehavior._type)
            {
                switch (_type)
                {
                    case RepeatBehaviorType.Forever:

                        return true;

                    case RepeatBehaviorType.IterationCount:

                        return _iterationCount == repeatBehavior._iterationCount;

                    case RepeatBehaviorType.RepeatDuration:

                        return _repeatDuration == repeatBehavior._repeatDuration;

                    default:

                        Debug.Fail("Unhandled RepeatBehaviorType");
                        return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Indicates whether the specified RepeatBehaviors are equal to each other.
        /// </summary>
        /// <param name="repeatBehavior1"></param>
        /// <param name="repeatBehavior2"></param>
        /// <returns>true if repeatBehavior1 and repeatBehavior2 are equal; otherwise false.</returns>
        public static bool Equals(RepeatBehavior repeatBehavior1, RepeatBehavior repeatBehavior2)
        {
            return repeatBehavior1.Equals(repeatBehavior2);
        }

        /// <summary>
        /// Generates a hash code for this RepeatBehavior.
        /// </summary>
        /// <returns>A hash code for this RepeatBehavior.</returns>
        public override int GetHashCode()
        {
            switch (_type)
            {
                case RepeatBehaviorType.IterationCount:

                    return _iterationCount.GetHashCode();

                case RepeatBehaviorType.RepeatDuration:

                    return _repeatDuration.GetHashCode();

                case RepeatBehaviorType.Forever:

                    // We try to choose an unlikely hash code value for Forever.
                    // All Forevers need to return the same hash code value.
                    return int.MaxValue - 42;

                default:

                    Debug.Fail("Unhandled RepeatBehaviorType");
                    return base.GetHashCode();
            }
        }

        /// <summary>
        /// Creates a string representation of this RepeatBehavior based on the current culture.
        /// </summary>
        /// <returns>A string representation of this RepeatBehavior based on the current culture.</returns>
        public override string ToString()
        {
            return InternalToString(null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public string ToString(IFormatProvider formatProvider)
        {
            return InternalToString(null, formatProvider);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        string IFormattable.ToString(string? format, IFormatProvider? formatProvider)
        {
            return InternalToString(format, formatProvider);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        internal string InternalToString(string? format, IFormatProvider? formatProvider)
        {
            switch (_type)
            {
                case RepeatBehaviorType.Forever:

                    return "Forever";

                case RepeatBehaviorType.IterationCount:

                    StringBuilder sb = new StringBuilder();

                    sb.AppendFormat(
                        formatProvider,
                        "{0:" + format + "}x",
                        _iterationCount);

                    return sb.ToString();

                case RepeatBehaviorType.RepeatDuration:

                    return _repeatDuration.ToString();

                default:

                    Debug.Fail("Unhandled RepeatBehaviorType.");
                    return null;
            }
        }

        #endregion // Methods

        #region Operators

        /// <summary>
        /// Indicates whether the specified RepeatBehaviors are equal to each other.
        /// </summary>
        /// <param name="repeatBehavior1"></param>
        /// <param name="repeatBehavior2"></param>
        /// <returns>true if repeatBehavior1 and repeatBehavior2 are equal; otherwise false.</returns>
        public static bool operator ==(RepeatBehavior repeatBehavior1, RepeatBehavior repeatBehavior2)
        {
            return repeatBehavior1.Equals(repeatBehavior2);
        }

        /// <summary>
        /// Indicates whether the specified RepeatBehaviors are not equal to each other.
        /// </summary>
        /// <param name="repeatBehavior1"></param>
        /// <param name="repeatBehavior2"></param>
        /// <returns>true if repeatBehavior1 and repeatBehavior2 are not equal; otherwise false.</returns>
        public static bool operator !=(RepeatBehavior repeatBehavior1, RepeatBehavior repeatBehavior2)
        {
            return !repeatBehavior1.Equals(repeatBehavior2);
        }

        #endregion // Operators

        /// <summary>
        /// An enumeration of the different types of RepeatBehavior behaviors.
        /// </summary>
        private enum RepeatBehaviorType
        {
            IterationCount,
            RepeatDuration,
            Forever
        }
    }
}
