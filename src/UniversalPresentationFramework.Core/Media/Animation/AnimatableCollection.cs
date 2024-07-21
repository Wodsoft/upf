using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public abstract class AnimatableCollection<T> : Animatable, IList, IList<T>
        where T : Animatable
    {
        internal List<T> _collection;

        #region Construction

        /// <summary>
        /// Initializes a new instance that is empty.
        /// </summary>
        public AnimatableCollection()
        {
            _collection = new List<T>();
        }

        /// <summary>
        /// Initializes a new instance that is empty and has the specified initial capacity.
        /// </summary>
        /// <param name="capacity"> int - The number of elements that the new list is initially capable of storing. </param>
        public AnimatableCollection(int capacity)
        {
            _collection = new List<T>(capacity);
        }

        /// <summary>
        /// Creates a TCollection with all of the same elements as collection
        /// </summary>
        public AnimatableCollection(IEnumerable<T> collection)
        {
            if (collection != null)
            {
                _collection = new List<T>(collection);

                foreach (T item in collection)
                {
                    if (item == null)
                        throw new System.ArgumentException("Collection not allow null item.");
                    OnFreezablePropertyChanged(/* oldValue = */ null, item);
                    OnInsert(item);
                }


                WritePostscript();
            }
            else
            {
                throw new ArgumentNullException("collection");
            }
        }

        #endregion

        #region IList<T>

        /// <summary>
        ///     Adds "value" to the list
        /// </summary>
        public void Add(T value)
        {
            AddHelper(value);
        }

        /// <summary>
        ///     Removes all elements from the list
        /// </summary>
        public void Clear()
        {
            WritePreamble();

            // As part of Clear()'ing the collection, we will iterate it and call
            // OnFreezablePropertyChanged and OnRemove for each item.
            // However, OnRemove assumes that the item to be removed has already been
            // pulled from the underlying collection.  To statisfy this condition,
            // we store the old collection and clear _collection before we call these methods.
            // As Clear() semantics do not include TrimToFit behavior, we create the new
            // collection storage at the same size as the previous.  This is to provide
            // as close as possible the same perf characteristics as less complicated collections.
            List<T> oldCollection = _collection;
            _collection = new List<T>(_collection.Capacity);

            for (int i = oldCollection.Count - 1; i >= 0; i--)
            {
                OnFreezablePropertyChanged(/* oldValue = */ oldCollection[i], /* newValue = */ null);

                // Fire the OnRemove handlers for each item.  We're not ensuring that
                // all OnRemove's get called if a resumable exception is thrown.
                // At this time, these call-outs are not public, so we do not handle exceptions.
                OnRemove( /* oldValue */ oldCollection[i]);
            }

            WritePostscript();
        }

        /// <summary>
        ///     Determines if the list contains "value"
        /// </summary>
        public bool Contains(T value)
        {
            ReadPreamble();

            return _collection.Contains(value);
        }

        /// <summary>
        ///     Returns the index of "value" in the list
        /// </summary>
        public int IndexOf(T value)
        {
            ReadPreamble();

            return _collection.IndexOf(value);
        }

        /// <summary>
        ///     Inserts "value" into the list at the specified position
        /// </summary>
        public void Insert(int index, T value)
        {
            if (value == null)
                throw new System.ArgumentException("Not allow null item.");

            WritePreamble();

            OnFreezablePropertyChanged(/* oldValue = */ null, /* newValue = */ value);

            _collection.Insert(index, value);
            OnInsert(value);

            WritePostscript();
        }

        /// <summary>
        ///     Removes "value" from the list
        /// </summary>
        public bool Remove(T value)
        {
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
                T oldValue = _collection[index];

                OnFreezablePropertyChanged(oldValue, null);

                _collection.RemoveAt(index);

                OnRemove(oldValue);

                WritePostscript();

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

            // RemoveAtWithoutFiringPublicEvents incremented the version

            WritePostscript();
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

            T oldValue = _collection[index];

            OnFreezablePropertyChanged(oldValue, null);

            _collection.RemoveAt(index);

            OnRemove(oldValue);

            // No WritePostScript to avoid firing the Changed event.
        }


        /// <summary>
        ///     Indexer for the collection
        /// </summary>
        public T this[int index]
        {
            get
            {
                ReadPreamble();

                return _collection[index];
            }
            set
            {
                if (value == null)
                    throw new System.ArgumentException("Not allow null item.");

                WritePreamble();

                if (!ReferenceEquals(_collection[index], value))
                {
                    T oldValue = _collection[index];
                    OnFreezablePropertyChanged(oldValue, value);

                    _collection[index] = value;

                    OnSet(oldValue, value);
                }

                WritePostscript();
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
        public void CopyTo(T[] array, int index)
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

        bool ICollection<T>.IsReadOnly
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
        public IEnumerator<T> GetEnumerator()
        {
            ReadPreamble();

            return _collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IList

        bool IList.IsReadOnly
        {
            get
            {
                return ((ICollection<T>)this).IsReadOnly;
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
            return Contains(Cast(value));
        }

        int IList.IndexOf(object? value)
        {
            return IndexOf(Cast(value));
        }

        void IList.Insert(int index, object? value)
        {
            // Forward to IList<T> Insert
            Insert(index, Cast(value));
        }

        void IList.Remove(object? value)
        {
            Remove(Cast(value));
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
                throw new ArgumentException("Bad rank.");
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
                throw new ArgumentException("Bad destination array type.");
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                ReadPreamble();

                return IsFrozen || Dispatcher != null;
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

        #region Private Helpers

        private T Cast(object? value)
        {
            if (value == null)
                throw new System.ArgumentNullException("value");

            if (!(value is T))
                throw new System.ArgumentException("Bad type.");

            return (T)value;
        }

        // IList.Add returns int and IList<T>.Add does not. This
        // is called by both Adds and IList<T>'s just ignores the
        // integer
        private int AddHelper(T value)
        {
            int index = AddWithoutFiringPublicEvents(value);

            WritePostscript();

            return index;
        }

        internal int AddWithoutFiringPublicEvents(T value)
        {
            int index = -1;
            if (value == null)
                throw new ArgumentNullException("value");
            WritePreamble();
            T newValue = value;
            OnFreezablePropertyChanged(/* oldValue = */ null, newValue);
            _collection.Add(newValue);
            index = _collection.Count - 1;
            OnInsert(newValue);
            return index;
        }

        public event ItemChangedEventHandler<T>? ItemInserted;
        public event ItemChangedEventHandler<T>? ItemRemoved;


        private void OnInsert(T item)
        {
            if (ItemInserted != null)
            {
                ItemInserted(this, item);
            }
        }

        private void OnRemove(T oldValue)
        {
            if (ItemRemoved != null)
            {
                ItemRemoved(this, oldValue);
            }
        }

        private void OnSet(T oldValue, T newValue)
        {
            OnInsert(newValue);
            OnRemove(oldValue);
        }

        #endregion Private Helpers

        #region Freezable


        /// <summary>
        /// Implementation of Freezable.CloneCore()
        /// </summary>
        protected override void CloneCore(Freezable source)
        {
            AnimatableCollection<T> sourceTCollection = (AnimatableCollection<T>)source;

            base.CloneCore(source);

            int count = sourceTCollection._collection.Count;

            _collection = new List<T>(count);

            for (int i = 0; i < count; i++)
            {
                T newValue = (T)sourceTCollection._collection[i].Clone();
                OnFreezablePropertyChanged(/* oldValue = */ null, newValue);
                _collection.Add(newValue);
                OnInsert(newValue);
            }
        }

        /// <summary>
        /// Implementation of Freezable.CloneCurrentValueCore()
        /// </summary>
        protected override void CloneCurrentValueCore(Freezable source)
        {
            AnimatableCollection<T> sourceTCollection = (AnimatableCollection<T>)source;

            base.CloneCurrentValueCore(source);

            int count = sourceTCollection._collection.Count;

            _collection = new List<T>(count);

            for (int i = 0; i < count; i++)
            {
                T newValue = (T)sourceTCollection._collection[i].CloneCurrentValue();
                OnFreezablePropertyChanged(/* oldValue = */ null, newValue);
                _collection.Add(newValue);
                OnInsert(newValue);
            }
        }
        /// <summary>
        /// Implementation of Freezable.GetAsFrozenCore()
        /// </summary>
        protected override void GetAsFrozenCore(Freezable source)
        {
            AnimatableCollection<T> sourceTCollection = (AnimatableCollection<T>)source;

            base.GetAsFrozenCore(source);

            int count = sourceTCollection._collection.Count;

            _collection = new List<T>(count);

            for (int i = 0; i < count; i++)
            {
                T newValue = (T)sourceTCollection._collection[i].GetAsFrozen();
                OnFreezablePropertyChanged(/* oldValue = */ null, newValue);
                _collection.Add(newValue);
                OnInsert(newValue);
            }
        }
        /// <summary>
        /// Implementation of Freezable.GetCurrentValueAsFrozenCore()
        /// </summary>
        protected override void GetCurrentValueAsFrozenCore(Freezable source)
        {
            AnimatableCollection<T> sourceTCollection = (AnimatableCollection<T>)source;

            base.GetCurrentValueAsFrozenCore(source);

            int count = sourceTCollection._collection.Count;

            _collection = new List<T>(count);

            for (int i = 0; i < count; i++)
            {
                T newValue = (T)sourceTCollection._collection[i].GetCurrentValueAsFrozen();
                OnFreezablePropertyChanged(/* oldValue = */ null, newValue);
                _collection.Add(newValue);
                OnInsert(newValue);
            }
        }
        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.FreezeCore">Freezable.FreezeCore</see>.
        /// </summary>
        protected override bool FreezeCore(bool isChecking)
        {
            bool canFreeze = base.FreezeCore(isChecking);

            int count = _collection.Count;
            for (int i = 0; i < count && canFreeze; i++)
            {
                canFreeze &= Freeze(_collection[i], isChecking);
            }

            return canFreeze;
        }

        #endregion
    }
}
