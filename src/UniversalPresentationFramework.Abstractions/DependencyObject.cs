using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public class DependencyObject
    {
        private Type? _type;
        private SortedList<int, DependencyValueStore> _valueStores = new SortedList<int, DependencyValueStore>();

        public void ClearValue(DependencyProperty dp)
        {
            if (dp == null)
                throw new ArgumentNullException(nameof(dp));
            ClearValueCore(dp);
        }
        public void ClearValue(DependencyPropertyKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            ClearValueCore(key.DependencyProperty);
        }
        private void ClearValueCore(DependencyProperty dp)
        {
            if (_valueStores.TryGetValue(dp.GlobalIndex, out var store))
            {
                _valueStores.Remove(dp.GlobalIndex);
                var metadata = GetMetadata(dp);
                if (metadata.PropertyChangedCallback != null && store.Value != DependencyProperty.UnsetValue)
                    metadata.PropertyChangedCallback(this, new DependencyPropertyChangedEventArgs(dp, store.Value, metadata.DefaultValue));
            }
        }

        public object? GetValue(DependencyProperty dp)
        {
            if (dp == null)
                throw new ArgumentNullException(nameof(dp));
            return GetValueCore(dp);
        }
        protected virtual object? GetValueCore(DependencyProperty dp)
        {
            if (!_valueStores.TryGetValue(dp.GlobalIndex, out var store))
            {
                var metadata = GetMetadata(dp);
                if (metadata.IsInherited)
                {
                    var parent = GetInheritanceParent();
                    if (parent == null)
                        return metadata.DefaultValue;
                    return parent.GetValueCore(dp);
                }
                return metadata.DefaultValue;
            }
            if (store.Value == DependencyProperty.UnsetValue)
            {
                var metadata = GetMetadata(dp);
                return metadata.DefaultValue;
            }
            return store.Value;
        }

        public void SetValue(DependencyProperty dp, object? value)
        {
            if (dp == null)
                throw new ArgumentNullException(nameof(dp));
            SetValueCore(dp, value);
        }
        public void SetValue(DependencyPropertyKey key, object? value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            SetValueCore(key.DependencyProperty, value);
        }
        protected virtual void SetValueCore(DependencyProperty dp, object? value)
        {
            var metadata = GetMetadata(dp);
            if (metadata.CoerceValueCallback != null)
                value = metadata.CoerceValueCallback(this, value);
            if (dp.ValidateValueCallback != null && !dp.ValidateValueCallback(value))
                throw new ArgumentException("Value validate failed.", nameof(value));
            if (value != null && !dp.PropertyType.IsAssignableFrom(value.GetType()))
                throw new ArgumentNullException($"Value can not assignable from \"{dp.PropertyType}\".");
            object? oldValue;
            if (_valueStores.TryGetValue(dp.GlobalIndex, out var store))
            {
                if (store.Value == DependencyProperty.UnsetValue)
                    oldValue = metadata.DefaultValue;
                else
                    oldValue = store.Value;
            }
            else
            {
                oldValue = metadata.DefaultValue;
                store = new DependencyValueStore();
                _valueStores.Add(dp.GlobalIndex, store);
            }
            store.Value = value;
            if (oldValue != value)
                OnPropertyChanged(new DependencyPropertyChangedEventArgs(dp, metadata, oldValue, value));
        }

        private PropertyMetadata GetMetadata(DependencyProperty dp)
        {
            if (_type == null)
                _type = GetType();
            return dp.GetMetadata(_type);
        }

        /// <summary>
        /// Get inheritance parent dependency object.
        /// </summary>
        /// <returns></returns>
        protected virtual DependencyObject? GetInheritanceParent()
        {
            return null;
        }

        protected virtual void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Metadata?.PropertyChangedCallback != null)
                e.Metadata.PropertyChangedCallback(this, e);
        }
    }
}
