using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Data;

namespace Wodsoft.UI.Controls
{
    public class ItemCollection : CollectionView, IList//, IEditableCollectionViewAddNewItem, ICollectionViewLiveShaping, IItemProperties, IWeakEventListener
    {
        private readonly ObservableCollection<object?> _items;
        private IEnumerable? _itemsSource;

        public ItemCollection() : base(new ObservableCollection<object?>())
        {
            _items = (ObservableCollection<object?>)SourceCollection;
        }

        public ItemCollection(IEnumerable source) : base(source)
        {
            _itemsSource = source;
            _items = new ObservableCollection<object?>();
        }

        #region Methods

        private void CheckUsingItemsSource()
        {
            if (_itemsSource != null)
                throw new InvalidOperationException("Can not modify items while using ItemsSource.");
        }

        internal void SetItemsSource(IEnumerable? source)
        {
            if (_items.Count != 0)
                throw new InvalidOperationException("Can not set items source when items is not empty.");
            if (source == null)
                SetSourceCollection(_items);
            else
                SetSourceCollection(source);
            _itemsSource = source;
        }

        #endregion

        #region IList

        public object? this[int index]
        {
            get
            {
                VerifyRefreshNotDeferred();
                if (_itemsSource == null && Filter == null)
                    return _items[index];
                return GetItemAt(index);
            }
            set
            {
                CheckUsingItemsSource();
                _items[index] = value;
            }
        }

        bool IList.IsFixedSize => _itemsSource != null;

        bool IList.IsReadOnly => _itemsSource != null;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot
        {
            get
            {
                if (_itemsSource != null)
                    throw new NotSupportedException("Should use items source sync root.");
                return SyncRoot;
            }
        }

        public int Add(object? value)
        {
            CheckUsingItemsSource();
            _items.Add(value);
            return _items.Count - 1;
        }

        public void Clear()
        {
            CheckUsingItemsSource();
            _items.Clear();
        }

        public void CopyTo(Array array, int index)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if (array.Rank > 1)
                throw new ArgumentException("Rank should be 1.", "array");
            if (index < 0)
                throw new ArgumentOutOfRangeException("index");
        }

        public void Insert(int index, object? value)
        {
            CheckUsingItemsSource();
            _items.Insert(index, value);
        }

        public void Remove(object? value)
        {
            CheckUsingItemsSource();
            _items.Remove(value);
        }

        public void RemoveAt(int index)
        {
            CheckUsingItemsSource();
            _items.RemoveAt(index);
        }

        #endregion

        #region ICollectionView

        public override int Count
        {
            get
            {
                if (_itemsSource == null && Filter == null)
                {
                    VerifyRefreshNotDeferred();
                    return _items.Count;
                }
                return base.Count;
            }
        }

        public override bool IsEmpty
        {
            get
            {
                if (_itemsSource == null && Filter == null)
                {
                    VerifyRefreshNotDeferred();
                    return _items.Count == 0;
                }
                return base.IsEmpty;
            }
        }

        protected override void ProcessCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (_itemsSource == null && Filter == null)
                OnCollectionChanged(e);
            else
                base.ProcessCollectionChanged(e);
        }

        #endregion
    }
}
