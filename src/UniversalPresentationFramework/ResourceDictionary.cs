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
        private Dictionary<object, object?> _dictionary = new Dictionary<object, object?>();

        #region Dictionary

        public object? this[object key]
        {
            get
            {
                if (_dictionary.TryGetValue(key, out object? value))
                    return value;
                return null;
            }
            set
            {
                _dictionary[key] = value;
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
            _dictionary.Remove(key);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dictionary.GetEnumerator();
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
