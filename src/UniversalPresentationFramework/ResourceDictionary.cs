using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;

namespace Wodsoft.UI
{
    [Ambient]
    public class ResourceDictionary : IDictionary, ISupportInitialize, INameScope
    {
        private static readonly DependencyProperty _ContextProperty = DependencyProperty.Register("Context", typeof(object), typeof(ResourceDictionary));
        private readonly Dictionary<object, object?> _dictionary = new Dictionary<object, object?>();
        private DependencyObject? _owner;

        public ResourceDictionary() { }

        public ResourceDictionary(DependencyObject owner)
        {
            _owner = owner;
        }

        #region Dictionary

        public object? this[object key]
        {
            get
            {
                if (_dictionary.TryGetValue(key, out object? value))
                    return value;
                return DependencyProperty.UnsetValue;
            }
            set
            {
                if (_dictionary.TryGetValue(key, out var oldValue))
                {
                    if (oldValue == value)
                        return;
                    RemoveInheritanceContext(oldValue);
                }
                _dictionary[key] = value;
                AddInheritanceContext(value);
            }
        }

        public bool IsFixedSize => ((IDictionary)_dictionary).IsFixedSize;

        public bool IsReadOnly => false;

        public ICollection Keys => _dictionary.Keys;

        public ICollection Values => _dictionary.Values;

        public int Count => _dictionary.Count;

        public bool IsSynchronized => ((IDictionary)_dictionary).IsSynchronized;

        public object SyncRoot => ((IDictionary)_dictionary).SyncRoot;

        public void Add(object key, object? value)
        {
            _dictionary.Add(key, value);
            AddInheritanceContext(value);
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public bool Contains(object key)
        {
            return _dictionary.ContainsKey(key);
        }

        public void CopyTo(Array array, int index)
        {
            ((IDictionary)_dictionary).CopyTo(array, index);
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        public void Remove(object key)
        {
            if (_dictionary.TryGetValue(key, out var value))
            {
                _dictionary.Remove(key);
                RemoveInheritanceContext(value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        internal void AddOwner(DependencyObject owner)
        {
            if (_owner != null)
                return;
            _owner = owner;
            foreach (var obj in _dictionary.Values)
                AddInheritanceContext(obj);
        }

        internal void RemoveOwner(DependencyObject owner)
        {
            if (_owner == owner)
            {
                foreach (var obj in _dictionary.Values)
                    RemoveInheritanceContext(obj);
                _owner = null;
            }
        }

        private void AddInheritanceContext(object? item)
        {
            if (_owner != null && item is DependencyObject d)
            {
                if (_owner.ProvideSelfAsInheritanceContext(d, _ContextProperty))
                    d.IsInheritanceContextSealed = true;
            }
        }

        private void RemoveInheritanceContext(object? item)
        {
            if (_owner != null && item is DependencyObject d && d.IsInheritanceContextSealed && d.InheritanceContext == _owner)
            {
                d.IsInheritanceContextSealed = false;
                _owner!.RemoveSelfAsInheritanceContext(d, _ContextProperty);
            }
        }

        #endregion

        #region NameScope

        public object? FindName(string name)
        {
            return null;
        }

        public void RegisterName(string name, object scopedElement)
        {
            throw new NotSupportedException("ResourceDictionary does not support Name.");
        }

        public void UnregisterName(string name)
        {

        }

        #endregion

        #region Initialize

        void ISupportInitialize.BeginInit()
        {

        }

        void ISupportInitialize.EndInit()
        {

        }

        #endregion

        #region Resource

        //private void GetValue(object key, ref object value, out bool canCache)
        //{

        //}

        #endregion
    }
}
