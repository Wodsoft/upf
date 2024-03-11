using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.ComponentModel
{
    public abstract class GroupDescription : INotifyPropertyChanged
    {
        public GroupDescription()
        {
            _explicitGroupNames = new ObservableCollection<object>();
            _explicitGroupNames.CollectionChanged += new NotifyCollectionChangedEventHandler(OnGroupNamesChanged);
        }

        #region INotifyPropertyChanged

        /// <summary>
        ///     This event is raised when a property of the group description has changed.
        /// </summary>
        event PropertyChangedEventHandler? INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                PropertyChanged += value;
            }
            remove
            {
                PropertyChanged -= value;
            }
        }

        /// <summary>
        /// PropertyChanged event (per <see cref="INotifyPropertyChanged" />).
        /// </summary>
        protected virtual event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// A subclass can call this method to raise the PropertyChanged event.
        /// </summary>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion

        #region Public Properties

        private ObservableCollection<object> _explicitGroupNames;
        /// <summary>
        /// This list of names is used to initialize a group with a set of
        /// subgroups with the given names.  (Additional subgroups may be
        /// added later, if there are items that don't match any of the names.)
        /// </summary>
        public ObservableCollection<object> GroupNames
        {
            get { return _explicitGroupNames; }
        }

        private SortDescriptionCollection? _sort;
        /// <summary>
        /// Collection of Sort criteria to sort the groups.
        /// </summary>
        public SortDescriptionCollection SortDescriptions
        {
            get
            {
                if (_sort == null)
                    SetSortDescriptions(new SortDescriptionCollection());
                return _sort!;
            }
        }

        private IComparer? _customSort;
        /// <summary>
        /// Set a custom comparer to sort groups using an object that implements IComparer.
        /// </summary>
        /// <remarks>
        /// Note: Setting the custom comparer object will clear previously set <seealso cref="GroupDescription.SortDescriptions"/>.
        /// </remarks>
        public IComparer? CustomSort
        {
            get { return _customSort; }
            set
            {
                _customSort = value;
                SetSortDescriptions(null);
                OnPropertyChanged(new PropertyChangedEventArgs("CustomSort"));
            }
        }

        #endregion

        #region Methods

        public abstract object GroupNameFromItem(object item, int level, CultureInfo culture);

        public virtual bool NamesMatch(object groupName, object itemName)
        {
            return Equals(groupName, itemName);
        }

        private void OnGroupNamesChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("GroupNames"));
        }

        private void SetSortDescriptions(SortDescriptionCollection? descriptions)
        {
            if (_sort != null)
            {
                ((INotifyCollectionChanged)_sort).CollectionChanged -= SortDescriptionsChanged;
            }

            bool raiseChangeEvent = (_sort != descriptions);

            _sort = descriptions;

            if (_sort != null)
            {
                ((INotifyCollectionChanged)_sort).CollectionChanged += SortDescriptionsChanged;
            }

            if (raiseChangeEvent)
            {
                OnPropertyChanged(new PropertyChangedEventArgs("SortDescriptions"));
            }
        }

        private void SortDescriptionsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            // adding to SortDescriptions overrides custom sort
            if (_sort!.Count > 0)
            {
                if (_customSort != null)
                {
                    _customSort = null;
                    OnPropertyChanged(new PropertyChangedEventArgs("CustomSort"));
                }
            }

            OnPropertyChanged(new PropertyChangedEventArgs("SortDescriptions"));
        }

        #endregion
    }
}
