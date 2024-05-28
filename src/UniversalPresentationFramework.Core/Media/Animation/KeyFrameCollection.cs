using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public abstract class KeyFrameCollection<T> : Freezable, IList<T>
        where T : Freezable, IKeyFrame
    {
        private List<T> _keyFrames;

        protected KeyFrameCollection()
        {
            _keyFrames = new List<T>(2);
        }

        protected List<T> KeyFrames { get => _keyFrames; set => _keyFrames = value; }

        #region IEnumerable

        /// <summary>
        /// Returns an enumerator of the BooleanKeyFrames in the collection.
        /// </summary>
        public IEnumerator GetEnumerator()
        {
            return _keyFrames.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _keyFrames.GetEnumerator();
        }

        #endregion

        #region ICollection

        /// <summary>
        /// Returns the number of BooleanKeyFrames in the collection.
        /// </summary>
        public int Count
        {
            get
            {
                return _keyFrames.Count;
            }
        }

        /// <summary>
        /// See <see cref="System.Collections.ICollection.IsSynchronized">ICollection.IsSynchronized</see>.
        /// </summary>
        public bool IsSynchronized
        {
            get
            {
                return (IsFrozen || Dispatcher != null);
            }
        }

        /// <summary>
        /// See <see cref="System.Collections.ICollection.SyncRoot">ICollection.SyncRoot</see>.
        /// </summary>
        public object SyncRoot
        {
            get
            {
                return ((ICollection)_keyFrames).SyncRoot;
            }
        }

        /// <summary>
        /// Copies all of the BooleanKeyFrames in the collection to an
        /// array.
        /// </summary>
        void ICollection<T>.CopyTo(T[] array, int index)
        {
            ((ICollection)_keyFrames).CopyTo(array, index);
        }

        /// <summary>
        /// Copies all of the BooleanKeyFrames in the collection to an
        /// array of BooleanKeyFrames.
        /// </summary>
        public void CopyTo(T[] array, int index)
        {
            _keyFrames.CopyTo(array, index);
        }

        #endregion

        #region IList

        /// <summary>
        /// Adds a BooleanKeyFrame to the collection.
        /// </summary>
        void ICollection<T>.Add(T keyFrame)
        {
            Add(keyFrame);
        }

        /// <summary>
        /// Adds a BooleanKeyFrame to the collection.
        /// </summary>
        public int Add(T keyFrame)
        {
            if (keyFrame == null)
            {
                throw new ArgumentNullException("keyFrame");
            }

            OnFreezablePropertyChanged(null, keyFrame);
            _keyFrames.Add(keyFrame);

            return _keyFrames.Count - 1;
        }

        /// <summary>
        /// Removes all BooleanKeyFrames from the collection.
        /// </summary>
        public void Clear()
        {
            if (_keyFrames.Count > 0)
            {
                for (int i = 0; i < _keyFrames.Count; i++)
                {
                    OnFreezablePropertyChanged(_keyFrames[i], null);
                }

                _keyFrames.Clear();
            }
        }

        /// <summary>
        /// Returns true of the collection contains the given BooleanKeyFrame.
        /// </summary>
        bool ICollection<T>.Contains(T keyFrame)
        {
            return Contains(keyFrame);
        }

        /// <summary>
        /// Returns true of the collection contains the given BooleanKeyFrame.
        /// </summary>
        public bool Contains(T keyFrame)
        {
            return _keyFrames.Contains(keyFrame);
        }

        /// <summary>
        /// Returns the index of a given BooleanKeyFrame in the collection. 
        /// </summary>
        int IList<T>.IndexOf(T keyFrame)
        {
            return IndexOf(keyFrame);
        }

        /// <summary>
        /// Returns the index of a given BooleanKeyFrame in the collection. 
        /// </summary>
        public int IndexOf(T keyFrame)
        {
            return _keyFrames.IndexOf(keyFrame);
        }

        /// <summary>
        /// Inserts a BooleanKeyFrame into a specific location in the collection. 
        /// </summary>
        void IList<T>.Insert(int index, T keyFrame)
        {
            Insert(index, keyFrame);
        }

        /// <summary>
        /// Inserts a BooleanKeyFrame into a specific location in the collection. 
        /// </summary>
        public void Insert(int index, T keyFrame)
        {
            if (keyFrame == null)
            {
                throw new ArgumentNullException("keyFrame");
            }

            OnFreezablePropertyChanged(null, keyFrame);
            _keyFrames.Insert(index, keyFrame);
        }

        /// <summary>
        /// Returns true if the collection is frozen.
        /// </summary>
        public bool IsFixedSize
        {
            get
            {
                return IsFrozen;
            }
        }

        /// <summary>
        /// Returns true if the collection is frozen.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return IsFrozen;
            }
        }

        bool ICollection<T>.Remove(T item)
        {
            return Remove(item);
        }

        /// <summary>
        /// Removes a BooleanKeyFrame from the collection.
        /// </summary>
        public bool Remove(T keyFrame)
        {
            if (_keyFrames.Contains(keyFrame))
            {
                OnFreezablePropertyChanged(keyFrame, null);
                return _keyFrames.Remove(keyFrame);
            }
            return false;
        }
        /// <summary>
        /// Removes a BooleanKeyFrame from the collection.
        /// </summary>
        void IList<T>.RemoveAt(int index)
        {
            _keyFrames.RemoveAt(index);
        }

        /// <summary>
        /// Removes the BooleanKeyFrame at the specified index from the collection.
        /// </summary>
        public void RemoveAt(int index)
        {
            OnFreezablePropertyChanged(_keyFrames[index], null);
            _keyFrames.RemoveAt(index);
        }


        /// <summary>
        /// Gets or sets the BooleanKeyFrame at a given index.
        /// </summary>
        T IList<T>.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                this[index] = value;
            }
        }

        /// <summary>
        /// Gets or sets the BooleanKeyFrame at a given index.
        /// </summary>
        public T this[int index]
        {
            get
            {
                return _keyFrames[index];
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(String.Format(CultureInfo.InvariantCulture, "BooleanKeyFrameCollection[{0}]", index));

                if (value != _keyFrames[index])
                {
                    OnFreezablePropertyChanged(_keyFrames[index], value);
                    _keyFrames[index] = value;

                    Debug.Assert(_keyFrames[index] != null);
                }
            }
        }

        #endregion
    }
}
