using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.ComponentModel
{
    public class SortDescriptionCollection : Collection<SortDescription>, INotifyCollectionChanged
    {
        #region Events

        /// <summary>
        /// Occurs when the collection changes, either by adding or removing an item.
        /// </summary>
        /// <remarks>
        /// see <seealso cref="INotifyCollectionChanged"/>
        /// </remarks>
        event NotifyCollectionChangedEventHandler? INotifyCollectionChanged.CollectionChanged
        {
            add
            {
                CollectionChanged += value;
            }
            remove
            {
                CollectionChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when the collection changes, either by adding or removing an item.
        /// </summary>
        protected event NotifyCollectionChangedEventHandler? CollectionChanged;

        #endregion

        #region Methods

        /// <summary>
        /// called by base class Collection&lt;T&gt; when the list is being cleared;
        /// raises a CollectionChanged event to any listeners
        /// </summary>
        protected override void ClearItems()
        {
            base.ClearItems();
            OnCollectionChanged(NotifyCollectionChangedAction.Reset);
        }

        /// <summary>
        /// called by base class Collection&lt;T&gt; when an item is removed from list;
        /// raises a CollectionChanged event to any listeners
        /// </summary>
        protected override void RemoveItem(int index)
        {
            SortDescription removedItem = this[index];
            base.RemoveItem(index);
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, removedItem, index);
        }

        /// <summary>
        /// called by base class Collection&lt;T&gt; when an item is added to list;
        /// raises a CollectionChanged event to any listeners
        /// </summary>
        protected override void InsertItem(int index, SortDescription item)
        {
            base.InsertItem(index, item);
            OnCollectionChanged(NotifyCollectionChangedAction.Add, item, index);
        }

        /// <summary>
        /// called by base class Collection&lt;T&gt; when an item is set in the list;
        /// raises a CollectionChanged event to any listeners
        /// </summary>
        protected override void SetItem(int index, SortDescription item)
        {
            SortDescription originalItem = this[index];
            base.SetItem(index, item);
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, originalItem, index);
            OnCollectionChanged(NotifyCollectionChangedAction.Add, item, index);
        }

        /// <summary>
        /// raise CollectionChanged event to any listeners
        /// </summary>
        private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, item, index));
            }
        }
        // raise CollectionChanged event to any listeners
        void OnCollectionChanged(NotifyCollectionChangedAction action)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(action));
            }
        }
        #endregion

        /// <summary>
        /// Immutable, read-only SortDescriptionCollection
        /// </summary>
        private class EmptySortDescriptionCollection : SortDescriptionCollection, IList
        {
            #region Protected Methods

            /// <summary>
            /// called by base class Collection&lt;T&gt; when the list is being cleared;
            /// raises a CollectionChanged event to any listeners
            /// </summary>
            protected override void ClearItems()
            {
                throw new NotSupportedException();
            }

            /// <summary>
            /// called by base class Collection&lt;T&gt; when an item is removed from list;
            /// raises a CollectionChanged event to any listeners
            /// </summary>
            protected override void RemoveItem(int index)
            {
                throw new NotSupportedException();
            }

            /// <summary>
            /// called by base class Collection&lt;T&gt; when an item is added to list;
            /// raises a CollectionChanged event to any listeners
            /// </summary>
            protected override void InsertItem(int index, SortDescription item)
            {
                throw new NotSupportedException();
            }

            /// <summary>
            /// called by base class Collection&lt;T&gt; when an item is set in list;
            /// raises a CollectionChanged event to any listeners
            /// </summary>
            protected override void SetItem(int index, SortDescription item)
            {
                throw new NotSupportedException();
            }

            #endregion Protected Methods

            #region IList Implementations

            // explicit implementation to override the IsReadOnly and IsFixedSize properties

            bool IList.IsFixedSize
            {
                get { return true; }
            }

            bool IList.IsReadOnly
            {
                get { return true; }
            }
            #endregion IList Implementations

        }

        /// <summary>
        /// returns an empty and non-modifiable SortDescriptionCollection
        /// </summary>
        public static readonly SortDescriptionCollection Empty = new EmptySortDescriptionCollection();
    }
}
