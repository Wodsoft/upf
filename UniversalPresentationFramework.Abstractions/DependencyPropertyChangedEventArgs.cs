using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public struct DependencyPropertyChangedEventArgs
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the DependencyPropertyChangedEventArgs class.
        /// </summary>
        /// <param name="property">
        ///     The property whose value changed.
        /// </param>
        /// <param name="oldValue">
        ///     The value of the property before the change.
        /// </param>
        /// <param name="newValue">
        ///     The value of the property after the change.
        /// </param>
        public DependencyPropertyChangedEventArgs(DependencyProperty property, object? oldValue, object? newValue)
        {
            _property = property;
            _old = oldValue;
            _new = newValue;
        }

        #endregion Constructors


        #region Properties

        /// <summary>
        ///     The property whose value changed.
        /// </summary>
        public DependencyProperty Property
        {
            get { return _property; }
        }

        public object? OldValue => _old;

        public object? NewValue => _new;

        #endregion Properties

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (obj is DependencyPropertyChangedEventArgs e)
                return Equals(e);
            return false;
        }

        public bool Equals(DependencyPropertyChangedEventArgs args)
        {
            return (_property == args._property &&
                    _old == args._old &&
                    _new == args._new);
        }

        public static bool operator ==(DependencyPropertyChangedEventArgs left, DependencyPropertyChangedEventArgs right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DependencyPropertyChangedEventArgs left, DependencyPropertyChangedEventArgs right)
        {
            return !left.Equals(right);
        }

        #region Data

        private DependencyProperty _property;

        private object? _old;
        private object? _new;

        #endregion Data
    }
}
