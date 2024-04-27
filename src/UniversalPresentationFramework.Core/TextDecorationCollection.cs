using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media.Animation;

namespace Wodsoft.UI
{
    public sealed class TextDecorationCollection : Freezable, IList, IList<TextDecoration>
    {
        private List<TextDecoration> _collection = new List<TextDecoration>();

        #region IList<T>

        /// <summary>
        ///     Adds "value" to the list
        /// </summary>
        public void Add(TextDecoration value)
        {
            AddHelper(value);
        }

        /// <summary>
        ///     Removes all elements from the list
        /// </summary>
        public void Clear()
        {
            WritePreamble();

            for (int i = _collection.Count - 1; i >= 0; i--)
            {
                OnFreezablePropertyChanged(/* oldValue = */ _collection[i], /* newValue = */ null);
            }

            _collection.Clear();
        }

        /// <summary>
        ///     Determines if the list contains "value"
        /// </summary>
        public bool Contains(TextDecoration value)
        {
            ReadPreamble();

            return _collection.Contains(value);
        }

        /// <summary>
        ///     Returns the index of "value" in the list
        /// </summary>
        public int IndexOf(TextDecoration value)
        {
            ReadPreamble();

            return _collection.IndexOf(value);
        }

        /// <summary>
        ///     Inserts "value" into the list at the specified position
        /// </summary>
        public void Insert(int index, TextDecoration value)
        {
            if (value == null)
                throw new System.ArgumentNullException("value");

            WritePreamble();

            OnFreezablePropertyChanged(/* oldValue = */ null, /* newValue = */ value);

            _collection.Insert(index, value);
        }

        /// <summary>
        ///     Removes "value" from the list
        /// </summary>
        public bool Remove(TextDecoration? value)
        {
            if (value == null)
                return false;

            WritePreamble();

            // By design collections "succeed silently" if you attempt to remove an item
            // not in the collection.  Therefore we need to first verify the old value exists
            // before calling OnFreezablePropertyChanged.  Since we already need to locate
            // the item in the collection we keep the index and use RemoveAt(...) to do
            // the work.  (Windows OS #1016178)

            // We use the public IndexOf to guard our UIContext since OnFreezablePropertyChanged
            // is only called conditionally.  IList.IndexOf returns -1 if the value is not found.
            int index = IndexOf(value);

            if (index >= 0)
            {
                TextDecoration oldValue = _collection[index];

                OnFreezablePropertyChanged(oldValue, null);

                _collection.RemoveAt(index);

                return true;
            }

            // Collection_Remove returns true, calls WritePostscript,
            // increments version, and does UpdateResource if it succeeds

            return false;
        }

        /// <summary>
        ///     Removes the element at the specified index
        /// </summary>
        public void RemoveAt(int index)
        {
            RemoveAtWithoutFiringPublicEvents(index);
        }


        /// <summary>
        ///     Removes the element at the specified index without firing
        ///     the public Changed event.
        ///     The caller - typically a public method - is responsible for calling
        ///     WritePostscript if appropriate.
        /// </summary>
        internal void RemoveAtWithoutFiringPublicEvents(int index)
        {
            WritePreamble();

            TextDecoration oldValue = _collection[index];

            OnFreezablePropertyChanged(oldValue, null);

            _collection.RemoveAt(index);
        }


        /// <summary>
        ///     Indexer for the collection
        /// </summary>
        public TextDecoration this[int index]
        {
            get
            {
                ReadPreamble();

                return _collection[index];
            }
            set
            {
                if (value == null)
                    throw new System.ArgumentNullException("value");

                WritePreamble();

                if (!ReferenceEquals(_collection[index], value))
                {
                    TextDecoration oldValue = _collection[index];
                    OnFreezablePropertyChanged(oldValue, value);
                    _collection[index] = value;
                }
            }
        }

        #endregion

        #region ICollection<T>

        /// <summary>
        ///     The number of elements contained in the collection.
        /// </summary>
        public int Count
        {
            get
            {
                ReadPreamble();

                return _collection.Count;
            }
        }

        /// <summary>
        ///     Copies the elements of the collection into "array" starting at "index"
        /// </summary>
        public void CopyTo(TextDecoration[] array, int index)
        {
            ReadPreamble();

            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            // This will not throw in the case that we are copying
            // from an empty collection.  This is consistent with the
            // BCL Collection implementations. (Windows 1587365)
            if (index < 0 || (index + _collection.Count) > array.Length)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            _collection.CopyTo(array, index);
        }

        bool ICollection<TextDecoration>.IsReadOnly
        {
            get
            {
                ReadPreamble();

                return IsFrozen;
            }
        }

        #endregion

        #region IEnumerable<T>

        /// <summary>
        /// Returns an enumerator for the collection
        /// </summary>
        public IEnumerator GetEnumerator()
        {
            ReadPreamble();

            return _collection.GetEnumerator();
        }

        IEnumerator<TextDecoration> IEnumerable<TextDecoration>.GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        #endregion

        #region IList

        bool IList.IsReadOnly
        {
            get
            {
                return IsFrozen;
            }
        }

        bool IList.IsFixedSize
        {
            get
            {
                ReadPreamble();

                return IsFrozen;
            }
        }

        object? IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                // Forwards to typed implementation
                this[index] = Cast(value);
            }
        }

        int IList.Add(object? value)
        {
            // Forward to typed helper
            return AddHelper(Cast(value));
        }

        bool IList.Contains(object? value)
        {
            if (value is TextDecoration item)
                return Contains(item);
            return false;
        }

        int IList.IndexOf(object? value)
        {
            if (value is TextDecoration item)
                return IndexOf(item);
            return -1;
        }

        void IList.Insert(int index, object? value)
        {
            // Forward to IList<T> Insert
            Insert(index, Cast(value));
        }

        void IList.Remove(object? value)
        {
            if (value == null)
                throw new System.ArgumentNullException("value");
            Remove(value as TextDecoration);
        }

        #endregion

        #region ICollection

        void ICollection.CopyTo(Array array, int index)
        {
            ReadPreamble();

            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            // This will not throw in the case that we are copying
            // from an empty collection.  This is consistent with the
            // BCL Collection implementations. (Windows 1587365)
            if (index < 0 || (index + _collection.Count) > array.Length)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            if (array.Rank != 1)
            {
                throw new ArgumentException("Array rank must be one.");
            }

            // Elsewhere in the collection we throw an AE when the type is
            // bad so we do it here as well to be consistent
            try
            {
                int count = _collection.Count;
                for (int i = 0; i < count; i++)
                {
                    array.SetValue(_collection[i], index + i);
                }
            }
            catch (InvalidCastException)
            {
                throw new ArgumentException("Bad destination array.");
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                ReadPreamble();

                return IsFrozen;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                ReadPreamble();
                return this;
            }
        }
        #endregion

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Private Helpers

        private TextDecoration Cast(object? value)
        {
            if (value == null)
            {
                throw new System.ArgumentNullException("value");
            }

            if (!(value is TextDecoration))
            {
                throw new System.ArgumentException("Value must be TextDecoration.");
            }

            return (TextDecoration)value;
        }

        // IList.Add returns int and IList<T>.Add does not. This
        // is called by both Adds and IList<T>'s just ignores the
        // integer
        private int AddHelper(TextDecoration value)
        {
            int index = AddWithoutFiringPublicEvents(value);

            return index;
        }

        internal int AddWithoutFiringPublicEvents(TextDecoration? value)
        {
            int index = -1;

            if (value == null)
                throw new System.ArgumentNullException("value");
            WritePreamble();
            TextDecoration newValue = value;
            OnFreezablePropertyChanged(/* oldValue = */ null, newValue);
            _collection.Add(newValue);
            index = _collection.Count - 1;
            return index;
        }

        #endregion Private Helpers

        #region Clone

        protected override Freezable CreateInstanceCore()
        {
            return new TextDecorationCollection();
        }

        protected override void CloneCore(Freezable source)
        {
            TextDecorationCollection sourceTextDecorationCollection = (TextDecorationCollection)source;

            base.CloneCore(source);

            int count = sourceTextDecorationCollection._collection.Count;

            _collection = new List<TextDecoration>(count);

            for (int i = 0; i < count; i++)
            {
                TextDecoration newValue = (TextDecoration)sourceTextDecorationCollection._collection[i].Clone();
                OnFreezablePropertyChanged(/* oldValue = */ null, newValue);
                _collection.Add(newValue);
            }
        }

        protected override void CloneCurrentValueCore(Freezable source)
        {
            TextDecorationCollection sourceTextDecorationCollection = (TextDecorationCollection)source;

            base.CloneCurrentValueCore(source);

            int count = sourceTextDecorationCollection._collection.Count;

            _collection = new List<TextDecoration>(count);

            for (int i = 0; i < count; i++)
            {
                TextDecoration newValue = (TextDecoration)sourceTextDecorationCollection._collection[i].CloneCurrentValue();
                OnFreezablePropertyChanged(/* oldValue = */ null, newValue);
                _collection.Add(newValue);
            }
        }

        protected override void GetAsFrozenCore(Freezable source)
        {
            TextDecorationCollection sourceTextDecorationCollection = (TextDecorationCollection)source;

            base.GetAsFrozenCore(source);

            int count = sourceTextDecorationCollection._collection.Count;

            _collection = new List<TextDecoration>(count);

            for (int i = 0; i < count; i++)
            {
                TextDecoration newValue = (TextDecoration)sourceTextDecorationCollection._collection[i].GetAsFrozen();
                OnFreezablePropertyChanged(/* oldValue = */ null, newValue);
                _collection.Add(newValue);
            }
        }

        protected override void GetCurrentValueAsFrozenCore(Freezable source)
        {
            TextDecorationCollection sourceTextDecorationCollection = (TextDecorationCollection)source;

            base.GetCurrentValueAsFrozenCore(source);

            int count = sourceTextDecorationCollection._collection.Count;

            _collection = new List<TextDecoration>(count);

            for (int i = 0; i < count; i++)
            {
                TextDecoration newValue = (TextDecoration)sourceTextDecorationCollection._collection[i].GetCurrentValueAsFrozen();
                OnFreezablePropertyChanged(/* oldValue = */ null, newValue);
                _collection.Add(newValue);
            }
        }

        protected override bool FreezeCore(bool isChecking)
        {
            bool canFreeze = base.FreezeCore(isChecking);

            int count = _collection.Count;
            for (int i = 0; i < count && canFreeze; i++)
            {
                canFreeze &= _collection[i].CanFreeze;
            }

            return canFreeze;
        }


        internal bool ValueEquals(TextDecorationCollection? textDecorations)
        {
            if (textDecorations == null)
                return false;   // o is either null or not TextDecorations object

            if (this == textDecorations)
                return true;    // Reference equality.

            if (this.Count != textDecorations.Count)
                return false;   // Two counts are different.

            // To be considered equal, TextDecorations should be same in the exact order.
            // Order matters because they imply the Z-order of the text decorations on screen.
            // Same set of text decorations drawn with different orders may have different result.
            for (int i = 0; i < this.Count; i++)
            {
                if (!this[i].ValueEquals(textDecorations[i]))
                    return false;
            }
            return true;
        }

        #endregion
    }
}
