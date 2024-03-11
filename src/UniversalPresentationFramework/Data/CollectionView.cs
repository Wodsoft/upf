using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Data
{
    public class CollectionView : ICollectionView, INotifyPropertyChanged
    {
        private IEnumerable _sourceCollection;
        private int _deferLevel, _timestamp;
        private bool _needRefresh, _cachedIsEmpty;
        private Predicate<object?>? _filter;
        private IndexedCollection? _wrapperCollection;

        #region Constructors

        public CollectionView(IEnumerable collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            SetSourceCollection(collection);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns true if the resulting (filtered) view is emtpy.
        /// </summary>
        public virtual bool IsEmpty
        {
            get { return WrapperCollection.IsEmpty; }
        }
        public virtual int Count
        {
            get
            {
                VerifyRefreshNotDeferred();

                return WrapperCollection.Count;
            }
        }

        public virtual bool NeedsRefresh => _needRefresh;

        public virtual bool IsInUse
        {
            get
            {
                return CollectionChanged != null || PropertyChanged != null;// ||
                                                                            //CurrentChanged != null || CurrentChanging != null;
            }
        }

        protected object SyncRoot { get; } = new object();

        #endregion

        #region Events

        public event NotifyCollectionChangedEventHandler? CollectionChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        #region Methods

        protected virtual void HandleNotifyCollection(INotifyCollectionChanged notifyCollection)
        {
            notifyCollection.CollectionChanged += OnCollectionChanged;
        }

        [MemberNotNull(nameof(_sourceCollection))]
        protected void SetSourceCollection(IEnumerable collection)
        {
            _sourceCollection = collection;
            if (collection is INotifyCollectionChanged notifyCollection)
                HandleNotifyCollection(notifyCollection);
        }

        /// <summary>
        /// Returns an object that enumerates the items in this view.
        /// </summary>
        protected virtual IEnumerator GetEnumerator()
        {
            VerifyRefreshNotDeferred();

            return WrapperCollection.GetEnumerator();
        }

        /// <summary>
        /// Returns an object that enumerates the items in this view.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Call when items of collection view changed.
        /// </summary>
        /// <param name="args"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            if (args == null)
                throw new ArgumentNullException("args");

            unchecked { ++_timestamp; }    // invalidate enumerators because of a change

            if (CollectionChanged != null)
                CollectionChanged(this, args);

            // Collection changes change the count unless an item is being
            // replaced or moved within the collection.
            if (args.Action != NotifyCollectionChangedAction.Replace &&
                args.Action != NotifyCollectionChangedAction.Move)
            {
                OnPropertyChanged("Count");
            }

            var isEmpty = IsEmpty;
            if (_cachedIsEmpty != isEmpty)
            {
                _cachedIsEmpty = isEmpty;
                OnPropertyChanged("IsEmpty");
            }
        }

        /// <summary>
        /// Call when source collection changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs args)
        {
            ProcessCollectionChanged(args);
        }

        /// <summary>
        /// Process source collection changed.
        /// Override if not use collection view enumerator.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void ProcessCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            Func<object?, bool> filter;
            if (!CanFilter || Filter == null)
                filter = (_) => true;
            else
                filter = new Func<object?, bool>(Filter);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (_wrapperCollection != null)
                    {
                        int count = 0;
                        int lastWrapperIndex = -1;
                        int lastSourceIndex = -1;
                        for (int i = e.NewItems!.Count - 1; i >= 0; i--)
                        {
                            var item = e.NewItems[i];
                            if (filter(item))
                            {
                                var index = _wrapperCollection.InsertAt(item, e.NewStartingIndex + i);
                                if (index == lastWrapperIndex - 1 || lastWrapperIndex == -1)
                                {
                                    count++;
                                }
                                else
                                {
                                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new SliceList(e.NewItems, i + 1, count), lastWrapperIndex));
                                    count = 1;
                                }
                                lastWrapperIndex = index;
                                lastSourceIndex = i;
                            }
                        }
                        if (count != 0)
                            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new SliceList(e.NewItems, lastSourceIndex, count), lastWrapperIndex));
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (_wrapperCollection != null)
                    {
                        int count = 0;
                        int lastWrapperIndex = -1;
                        int lastSourceIndex = -1;
                        for (int i = e.OldItems!.Count - 1; i >= 0; i--)
                        {
                            var item = e.OldItems[i];
                            if (filter(item))
                            {
                                var index = _wrapperCollection.RemoveAt(e.OldStartingIndex + i);
                                if (index == lastWrapperIndex - 1 || lastWrapperIndex == -1)
                                {
                                    count++;
                                }
                                else
                                {
                                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new SliceList(e.OldItems, i + 1, count), lastWrapperIndex));
                                    count = 1;
                                }
                                lastWrapperIndex = index;
                                lastSourceIndex = i;
                            }
                        }
                        if (count != 0)
                            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new SliceList(e.OldItems, lastSourceIndex, count), lastWrapperIndex));
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (_wrapperCollection != null)
                    {
                        int count = 0;
                        int lastWrapperIndex = -1;
                        int lastSourceIndex = -1;
                        NotifyCollectionChangedAction lastAction = NotifyCollectionChangedAction.Reset;
                        for (int i = 0; i < e.OldItems!.Count; i++)
                        {
                            var oldItem = e.OldItems[i];
                            var newItem = e.NewItems![i];
                            var oldUsed = filter(oldItem) && _wrapperCollection.ContainsIndex(e.OldStartingIndex + i);
                            var newUsed = filter(newItem) && _wrapperCollection.ContainsIndex(e.NewStartingIndex + i);
                            NotifyCollectionChangedAction actuallyAction;
                            if (oldUsed && newUsed)
                                actuallyAction = NotifyCollectionChangedAction.Replace;
                            else if (oldUsed && !newUsed)
                                actuallyAction = NotifyCollectionChangedAction.Remove;
                            else if (!oldUsed && newUsed)
                                actuallyAction = NotifyCollectionChangedAction.Add;
                            else
                                actuallyAction = NotifyCollectionChangedAction.Reset;
                            var handleLast = () =>
                            {
                                if (lastAction == NotifyCollectionChangedAction.Reset)
                                    return;
                                switch (lastAction)
                                {
                                    case NotifyCollectionChangedAction.Add:
                                        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new SliceList(e.NewItems, i + 1, count), lastWrapperIndex));
                                        break;
                                    case NotifyCollectionChangedAction.Remove:
                                        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new SliceList(e.OldItems, i + 1, count), lastWrapperIndex));
                                        break;
                                    case NotifyCollectionChangedAction.Replace:
                                        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, new SliceList(e.NewItems, i + 1, count), new SliceList(e.OldItems, i + 1, count), lastWrapperIndex));
                                        break;
                                }
                                lastWrapperIndex = -1;
                                count = 0;
                            };
                            if (lastAction != actuallyAction)
                                handleLast();
                            int index;
                            switch (actuallyAction)
                            {
                                case NotifyCollectionChangedAction.Add:
                                    index = _wrapperCollection.InsertAt(newItem, e.NewStartingIndex + i);
                                    if (index == lastWrapperIndex - 1 || lastWrapperIndex == -1)
                                    {
                                        count++;
                                    }
                                    else
                                    {
                                        handleLast();
                                        count = 1;
                                    }
                                    lastWrapperIndex = index;
                                    lastSourceIndex = i;
                                    break;
                                case NotifyCollectionChangedAction.Remove:
                                    index = _wrapperCollection.RemoveAt(e.OldStartingIndex + i);
                                    if (index == lastWrapperIndex - 1 || lastWrapperIndex == -1)
                                    {
                                        count++;
                                    }
                                    else
                                    {
                                        handleLast();
                                        count = 1;
                                    }
                                    lastWrapperIndex = index;
                                    lastSourceIndex = i;
                                    break;
                                case NotifyCollectionChangedAction.Replace:
                                    index = _wrapperCollection.ReplaceAt(newItem, e.NewStartingIndex + i);
                                    if (index == lastWrapperIndex - 1 || lastWrapperIndex == -1)
                                    {
                                        count++;
                                    }
                                    else
                                    {
                                        handleLast();
                                        count = 1;
                                    }
                                    lastWrapperIndex = index;
                                    lastSourceIndex = i;
                                    break;
                            }
                            lastAction = actuallyAction;
                        }
                        switch (lastAction)
                        {
                            case NotifyCollectionChangedAction.Add:
                                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new SliceList(e.NewItems!, lastSourceIndex, count), lastWrapperIndex));
                                break;
                            case NotifyCollectionChangedAction.Remove:
                                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new SliceList(e.OldItems, lastSourceIndex, count), lastWrapperIndex));
                                break;
                            case NotifyCollectionChangedAction.Replace:
                                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, new SliceList(e.NewItems!, lastSourceIndex, count), new SliceList(e.OldItems, lastSourceIndex, count), lastWrapperIndex));
                                break;
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    if (_wrapperCollection != null)
                    {
                        int count = 0;
                        int lastFromIndex = -1, lastToIndex = -1;
                        int lastSourceIndex = -1;
                        NotifyCollectionChangedAction lastAction = NotifyCollectionChangedAction.Reset;
                        for (int i = 0; i < e.OldItems!.Count; i++)
                        {
                            var oldItem = e.OldItems[i];
                            var newItem = e.NewItems![i];
                            var oldUsed = filter(oldItem) && _wrapperCollection.ContainsIndex(e.OldStartingIndex + i);
                            var newUsed = filter(newItem) && _wrapperCollection.ContainsIndex(e.NewStartingIndex + i);
                            NotifyCollectionChangedAction actuallyAction;
                            if (oldUsed && newUsed)
                                actuallyAction = NotifyCollectionChangedAction.Move;
                            else if (oldUsed && !newUsed)
                                actuallyAction = NotifyCollectionChangedAction.Remove;
                            else if (!oldUsed && newUsed)
                                actuallyAction = NotifyCollectionChangedAction.Add;
                            else
                                actuallyAction = NotifyCollectionChangedAction.Reset;
                            var handleLast = () =>
                            {
                                if (lastAction == NotifyCollectionChangedAction.Reset)
                                    return;
                                switch (lastAction)
                                {
                                    case NotifyCollectionChangedAction.Add:
                                        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new SliceList(e.NewItems, i + 1, count), lastToIndex));
                                        lastToIndex = -1;
                                        break;
                                    case NotifyCollectionChangedAction.Remove:
                                        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new SliceList(e.OldItems, i + 1, count), lastFromIndex));
                                        lastFromIndex = -1;
                                        break;
                                    case NotifyCollectionChangedAction.Move:
                                        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, new SliceList(e.NewItems, i + 1, count), lastToIndex, lastFromIndex));
                                        lastToIndex = -1;
                                        lastFromIndex = -1;
                                        break;
                                }
                                count = 0;
                            };
                            if (lastAction != actuallyAction)
                                handleLast();
                            int fromIndex, toIndex;
                            switch (actuallyAction)
                            {
                                case NotifyCollectionChangedAction.Add:
                                    toIndex = _wrapperCollection.InsertAt(newItem, e.NewStartingIndex + i);
                                    if (toIndex == lastToIndex - 1 || lastToIndex == -1)
                                    {
                                        count++;
                                    }
                                    else
                                    {
                                        handleLast();
                                        count = 1;
                                    }
                                    lastToIndex = toIndex;
                                    lastSourceIndex = i;
                                    break;
                                case NotifyCollectionChangedAction.Remove:
                                    fromIndex = _wrapperCollection.RemoveAt(e.OldStartingIndex + i);
                                    if (fromIndex == lastFromIndex - 1 || lastFromIndex == -1)
                                    {
                                        count++;
                                    }
                                    else
                                    {
                                        handleLast();
                                        count = 1;
                                    }
                                    lastFromIndex = fromIndex;
                                    lastSourceIndex = i;
                                    break;
                                case NotifyCollectionChangedAction.Move:
                                    _wrapperCollection.MoveAt(e.OldStartingIndex + i, e.NewStartingIndex + i, out fromIndex, out toIndex);
                                    if (fromIndex == lastFromIndex - 1 && toIndex == lastToIndex - 1)
                                    {
                                        count++;
                                    }
                                    else
                                    {
                                        handleLast();
                                        count = 1;
                                    }
                                    lastFromIndex = fromIndex;
                                    lastToIndex = toIndex;
                                    lastSourceIndex = i;
                                    break;
                            }
                            lastAction = actuallyAction;
                        }
                        switch (lastAction)
                        {
                            case NotifyCollectionChangedAction.Add:
                                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new SliceList(e.NewItems!, lastSourceIndex, count), lastToIndex));
                                break;
                            case NotifyCollectionChangedAction.Remove:
                                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new SliceList(e.OldItems, lastSourceIndex, count), lastFromIndex));
                                break;
                            case NotifyCollectionChangedAction.Move:
                                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, new SliceList(e.NewItems!, lastSourceIndex, count), lastToIndex, lastFromIndex));
                                break;
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    RefreshOrDefer();
                    break;
            }
        }

        public virtual int IndexOf(object? item)
        {
            VerifyRefreshNotDeferred();

            return WrapperCollection.IndexOf(item);
        }

        protected virtual void VerifyRefreshNotDeferred()
        {
            if (_deferLevel > 0)
                throw new InvalidOperationException("No check or change when deferred.");
        }

        protected void RefreshOrDefer()
        {
            if (IsRefreshDeferred)
                _needRefresh = true;
            else
                Refresh();
        }

        [MemberNotNull(nameof(_wrapperCollection))]
        private void CreateWrapperCollection()
        {
            _wrapperCollection = new IndexedCollection(this);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public virtual object? GetItemAt(int index)
        {
            return WrapperCollection[index];
        }

        #endregion

        #region Properties

        protected bool IsRefreshDeferred
        {
            get
            {
                return _deferLevel > 0;
            }
        }

        private IndexedCollection WrapperCollection
        {
            get
            {
                if (_wrapperCollection == null)
                    CreateWrapperCollection();
                return _wrapperCollection;
            }
        }

        #endregion

        #region ICollectionView

        private CultureInfo? _culture;
        /// <summary>
        /// Culture to use during sorting.
        /// </summary>
        [TypeConverter(typeof(CultureInfoIetfLanguageTagConverter))]
        public virtual CultureInfo? Culture
        {
            get { return _culture; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                if (_culture != value)
                {
                    _culture = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Return true if the item belongs to this view.  No assumptions are
        /// made about the item. This method will behave similarly to IList.Contains().
        /// </summary>
        /// <remarks>
        /// <p>If the caller knows that the item belongs to the
        /// underlying collection, it is more efficient to call PassesFilter.
        /// If the underlying collection is only of type IEnumerable, this method
        /// is a O(N) operation</p>
        /// </remarks>
        public virtual bool Contains(object? item)
        {
            VerifyRefreshNotDeferred();

            return (IndexOf(item) >= 0);
        }

        /// <summary>
        /// Returns the underlying collection.
        /// </summary>
        public virtual IEnumerable SourceCollection
        {
            get { return _sourceCollection; }
        }

        /// <summary>
        /// Filter is a callback set by the consumer of the ICollectionView
        /// and used by the implementation of the ICollectionView to determine if an
        /// item is suitable for inclusion in the view.
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// Simpler implementations do not support filtering and will throw a NotSupportedException.
        /// Use <seealso cref="CanFilter"/> property to test if filtering is supported before
        /// assigning a non-null value.
        /// </exception>
        public virtual Predicate<object?>? Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                if (!CanFilter)
                    throw new NotSupportedException();

                _filter = value;

                RefreshOrDefer();
            }
        }

        /// <summary>
        /// Indicates whether or not this ICollectionView can do any filtering.
        /// When false, set <seealso cref="Filter"/> will throw an exception.
        /// </summary>
        public virtual bool CanFilter
        {
            get
            {
                return true;
            }
        }

        ///// <summary>
        ///// Collection of Sort criteria to sort items in this view over the SourceCollection.
        ///// </summary>
        ///// <remarks>
        ///// <p>
        ///// Simpler implementations do not support sorting and will return an empty
        ///// and immutable / read-only SortDescription collection.
        ///// Attempting to modify such a collection will cause NotSupportedException.
        ///// Use <seealso cref="CanSort"/> property on CollectionView to test if sorting is supported
        ///// before modifying the returned collection.
        ///// </p>
        ///// <p>
        ///// One or more sort criteria in form of <seealso cref="SortDescription"/>
        ///// can be added, each specifying a property and direction to sort by.
        ///// </p>
        ///// </remarks>
        //public virtual SortDescriptionCollection SortDescriptions
        //{
        //    get { return SortDescriptionCollection.Empty; }
        //}

        ///// <summary>
        ///// Test if this ICollectionView supports sorting before adding
        ///// to <seealso cref="SortDescriptions"/>.
        ///// </summary>
        //public virtual bool CanSort
        //{
        //    get { return false; }
        //}

        /// <summary>
        /// Re-create the view, using any <seealso cref="SortDescriptions"/> and/or <seealso cref="Filter"/>.
        /// </summary>
        public virtual void Refresh()
        {
            CreateWrapperCollection();
        }

        /// <summary>
        /// Enter a Defer Cycle.
        /// Defer cycles are used to coalesce changes to the ICollectionView.
        /// </summary>
        public virtual IDisposable DeferRefresh()
        {
            ++_deferLevel;
            return new DeferHelper(this);
        }

        #endregion ICollectionView

        private class DeferHelper : IDisposable
        {
            private readonly CollectionView _view;
            private bool _disposed;

            public DeferHelper(CollectionView view)
            {
                _view = view;
            }

            public void Dispose()
            {
                if (_disposed)
                    return;
                _disposed = true;
                _view._deferLevel--;
                if (_view._needRefresh && _view._deferLevel == 0)
                    _view.Refresh();
            }
        }

        private class IndexedCollection : IEnumerable
        {
            private readonly CollectionView _view;
            private readonly List<object?> _items;
            private readonly List<int> _index;

            public IndexedCollection(CollectionView view)
            {
                _view = view;
                _items = new List<object?>();
                _index = new List<int>();
                var filter = view.CanFilter ? view.Filter : null;
                int i = 0;
                foreach (var item in view.SourceCollection)
                {
                    if (filter == null || filter(item))
                    {
                        _items.Add(item);
                        _index.Add(i);
                    }
                    i++;
                }
            }

            public object? this[int index]
            {
                get
                {
                    if (index < 0 || index >= _items.Count)
                        throw new ArgumentOutOfRangeException(nameof(index));
                    return _items[index];
                }
            }

            public bool IsEmpty => _items.Count == 0;

            public int Count => _items.Count;

            public int IndexOf(object? item) => _items.IndexOf(item);

            public int InsertAt(object? item, int index)
            {
                var i = _index.BinarySearch(index);
                _items.Insert(i, item);
                _index.Insert(i, index);
                return i;
            }

            public int RemoveAt(int index)
            {
                var i = _index.BinarySearch(index);
                _items.RemoveAt(i);
                _index.RemoveAt(i);
                return i;
            }

            public int ReplaceAt(object? item, int index)
            {
                var i = _index.BinarySearch(index);
                _items[i] = item;
                return i;
            }

            public void MoveAt(int oldIndex, int newIndex, out int fromIndex, out int toIndex)
            {
                fromIndex = _index.BinarySearch(oldIndex);
                toIndex = _index.BinarySearch(newIndex);
                var fromItem = _items[fromIndex];
                _items[fromIndex] = _items[toIndex];
                _items[toIndex] = fromItem;
            }

            public bool ContainsIndex(int index)
            {
                return _index.Contains(index);
            }

            public IEnumerator GetEnumerator() => new Enumerator(_items.GetEnumerator(), _view._timestamp, () => _view._timestamp);

            public class Enumerator : IEnumerator
            {
                private readonly IEnumerator _enumerator;
                private readonly int _timestamp;
                private readonly Func<int> _currentTimestamp;

                public Enumerator(IEnumerator enumerator, int timestamp, Func<int> currentTimestamp)
                {
                    _enumerator = enumerator;
                    _timestamp = timestamp;
                    _currentTimestamp = currentTimestamp;
                }

                public object Current => _enumerator.Current;

                public bool MoveNext()
                {
                    if (_timestamp != _currentTimestamp())
                        throw new InvalidOperationException("Items of source collection have been changed.");
                    return _enumerator.MoveNext();
                }

                public void Reset()
                {
                    if (_timestamp != _currentTimestamp())
                        throw new InvalidOperationException("Items of source collection have been changed.");
                    _enumerator.Reset();
                }
            }
        }
    }
}
