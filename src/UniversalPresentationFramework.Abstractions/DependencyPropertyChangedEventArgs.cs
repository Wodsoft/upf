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

        public DependencyPropertyChangedEventArgs(DependencyProperty property, object? oldValue, object? newValue)
        {
            _property = property;
            _metadata = null;
            _old = oldValue;
            _new = newValue;
        }

        public DependencyPropertyChangedEventArgs(DependencyProperty property, PropertyMetadata? metadata, object? oldValue, object? newValue)
        {
            _property = property;
            _metadata = metadata;
            _old = oldValue;
            _new = newValue;
        }

        public DependencyPropertyChangedEventArgs(DependencyProperty property, PropertyMetadata? metadata, object? oldValue, object? newValue, DependencyPropertyChangedEventFlags flags)
        {
            _property = property;
            _metadata = metadata;
            _old = oldValue;
            _new = newValue;
            _flags = flags;
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

        public PropertyMetadata? Metadata => _metadata;

        public DependencyPropertyChangedEventFlags Flags => _flags;

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

        private readonly DependencyProperty _property;
        private readonly PropertyMetadata? _metadata;
        private readonly object? _old;
        private readonly object? _new;
        private readonly DependencyPropertyChangedEventFlags _flags;

        #endregion Data
    }
}
