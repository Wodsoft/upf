using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public class ObservableCollectionDefaultValueFactory<T> : DefaultValueFactory
    {
        private readonly ObservableCollection<T> _defaultValue;

        public ObservableCollectionDefaultValueFactory()
        {
            _defaultValue = new ObservableCollection<T>();
        }

        public override object? DefaultValue => _defaultValue;

        public override object CreateDefaultValue(DependencyObject owner, DependencyProperty property, DependencyPropertyKey? key)
        {
            var value = new ObservableCollection<T>();
            new ObservableCollectionDefaultValueHandler(owner, property, key, value);
            return value;
        }

        private class ObservableCollectionDefaultValueHandler
        {
            private readonly DependencyObject _owner;
            private readonly DependencyProperty _property;
            private readonly DependencyPropertyKey? _key;
            private readonly ObservableCollection<T> _collection;

            public ObservableCollectionDefaultValueHandler(DependencyObject owner, DependencyProperty property, DependencyPropertyKey? key, ObservableCollection<T> collection)
            {
                _owner = owner;
                _property = property;
                _key = key;
                _collection = collection;

                _collection.CollectionChanged += CollectionChanged;
            }

            private void CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
            {
                _collection.CollectionChanged -= CollectionChanged;
                var metadata = _owner.GetMetadata(_property);
                metadata.ClearCachedDefaultValue(_owner, _property);
                ref readonly var effectiveValue = ref _owner.GetEffectiveValue(_property);
                if (effectiveValue.Source != DependencyEffectiveSource.Local)
                    if (_key == null)
                        _owner.SetValue(_property, _collection);
                    else
                        _owner.SetValue(_key, _collection);
            }
        }
    }
}
