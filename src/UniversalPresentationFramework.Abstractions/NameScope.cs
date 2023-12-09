using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;

namespace Wodsoft.UI
{
    public class NameScope : INameScopeDictionary
    {
        public static bool IsValidIdentifierName(string name)
        {
            // Grammar:
            // <identifier> ::= <identifier_start> ( <identifier_start> | <identifier_extend> )*
            // <identifier_start> ::= [{Lu}{Ll}{Lt}{Lo}{Nl}('_')]
            // <identifier_extend> ::= [{Mn}{Mc}{Lm}{Nd}]
            UnicodeCategory uc;
            for (int i = 0; i < name.Length; i++)
            {
                uc = Char.GetUnicodeCategory(name[i]);
                bool idStart = (uc == UnicodeCategory.UppercaseLetter || // (Lu)
                             uc == UnicodeCategory.LowercaseLetter || // (Ll)
                             uc == UnicodeCategory.TitlecaseLetter || // (Lt)
                             uc == UnicodeCategory.OtherLetter || // (Lo)
                             uc == UnicodeCategory.LetterNumber || // (Nl)
                             name[i] == '_');
                bool idExtend = (uc == UnicodeCategory.NonSpacingMark || // (Mn)
                              uc == UnicodeCategory.SpacingCombiningMark || // (Mc)
                              uc == UnicodeCategory.ModifierLetter || // (Lm)
                              uc == UnicodeCategory.DecimalDigitNumber); // (Nd)
                if (i == 0)
                {
                    if (!idStart)
                    {
                        return false;
                    }
                }
                else if (!(idStart || idExtend))
                {
                    return false;
                }
            }
            return true;
        }

        #region INameScope

        /// <summary>
        /// Register Name-Object Map 
        /// </summary>
        /// <param name="name">name to be registered</param>
        /// <param name="scopedElement">object mapped to name</param>
        public void RegisterName(string name, object scopedElement)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            if (scopedElement == null)
                throw new ArgumentNullException("scopedElement");

            if (name == string.Empty)
                throw new ArgumentException("Name can't be empty string.");

            if (!IsValidIdentifierName(name))
            {
                throw new ArgumentException("Invalid name.");
            }

            if (_nameMap == null)
            {
                _nameMap = new HybridDictionary();
                _nameMap[name] = scopedElement;
            }
            else
            {
                object? nameContext = _nameMap[name];
                // first time adding the Name, set it
                if (nameContext == null)
                {
                    _nameMap[name] = scopedElement;
                }
                else if (scopedElement != nameContext)
                {
                    throw new ArgumentException("Name duplicated.");
                }
            }
        }

        /// <summary>
        /// Unregister Name-Object Map 
        /// </summary>
        /// <param name="name">name to be registered</param>
        public void UnregisterName(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            if (name == string.Empty)
                throw new ArgumentException("Name can't be empty string.");

            if (_nameMap != null && _nameMap[name] != null)
            {
                _nameMap.Remove(name);
            }
            else
            {
                throw new ArgumentException("Name not found.");
            }
        }

        /// <summary>
        /// Find - Find the corresponding object given a Name
        /// </summary>
        /// <param name="name">Name for which context needs to be retrieved</param>
        /// <returns>corresponding Context if found, else null</returns>
        public object? FindName(string name)
        {
            if (_nameMap == null || name == null || name == string.Empty)
                return null;

            return _nameMap[name];
        }

        #endregion INameScope

        #region InternalMethods

        internal static INameScope? NameScopeFromObject(object obj)
        {
            INameScope? nameScope = obj as INameScope;
            if (nameScope == null)
            {
                if (obj is DependencyObject o)
                    nameScope = GetNameScope(o);
            }
            return nameScope;
        }

        #endregion InternalMethods

        #region DependencyProperties        

        /// <summary>
        /// NameScope property. This is an attached property. 
        /// This property allows the dynamic attachment of NameScopes
        /// </summary>
        public static readonly DependencyProperty NameScopeProperty = DependencyProperty.RegisterAttached("NameScope", typeof(INameScope), typeof(NameScope));

        /// <summary>
        /// Helper for setting NameScope property on a DependencyObject. 
        /// </summary>
        /// <param name="dependencyObject">Dependency Object  to set NameScope property on.</param>
        /// <param name="value">NameScope property value.</param>
        public static void SetNameScope(DependencyObject dependencyObject, INameScope value)
        {
            if (dependencyObject == null)
                throw new ArgumentNullException("dependencyObject");

            dependencyObject.SetValue(NameScopeProperty, value);
        }

        /// <summary>
        /// Helper for reading NameScope property from a DependencyObject.
        /// </summary>
        /// <param name="dependencyObject">DependencyObject to read NameScope property from.</param>
        /// <returns>NameScope property value.</returns>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static INameScope? GetNameScope(DependencyObject dependencyObject)
        {
            if (dependencyObject == null)
                throw new ArgumentNullException("dependencyObject");

            return ((INameScope?)dependencyObject.GetValue(NameScopeProperty));
        }

        #endregion DependencyProperties


        #region Data

        // This is a HybridDictionary of Name-Object maps
        private HybridDictionary? _nameMap;

        #endregion Data

        IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
        {
            return new Enumerator(_nameMap);
        }

        #region IEnumerable methods
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region IEnumerable<KeyValuePair<string, object> methods
        IEnumerator<KeyValuePair<string, object?>> IEnumerable<KeyValuePair<string, object?>>.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region ICollection<KeyValuePair<string, object> methods
        public int Count
        {
            get
            {
                if (_nameMap == null)
                {
                    return 0;
                }
                return _nameMap.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public void Clear()
        {
            _nameMap = null;
        }

        public void CopyTo(KeyValuePair<string, object?>[] array, int arrayIndex)
        {
            if (_nameMap == null)
                return;

            foreach (DictionaryEntry entry in _nameMap)
            {
                array[arrayIndex++] = new KeyValuePair<string, object?>((string)entry.Key, entry.Value);
            }
        }

        public bool Remove(KeyValuePair<string, object?> item)
        {
            if (!Contains(item))
            {
                return false;
            }

            if (item.Value != this[item.Key])
            {
                return false;
            }
            return Remove(item.Key);
        }

        public void Add(KeyValuePair<string, object?> item)
        {
            if (item.Key == null)
                throw new ArgumentException("Key must not be null.", "item");

            if (item.Value == null)
                throw new ArgumentException("Value must not be null.", "item");

            Add(item.Key, item.Value);
        }

        public bool Contains(KeyValuePair<string, object?> item)
        {
            if (item.Key == null)
                throw new ArgumentException("Key must not be null.", "item");

            return ContainsKey(item.Key);
        }
        #endregion

        #region IDictionary<string, object> methods
        public object? this[string key]
        {
            get
            {
                if (key == null)
                {
                    throw new ArgumentNullException("key");
                }
                return FindName(key);
            }
            set
            {
                if (key == null)
                {
                    throw new ArgumentNullException("key");
                }

                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                RegisterName(key, value);
            }
        }

        public void Add(string key, object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            RegisterName(key, value);
        }

        public bool ContainsKey(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            object? value = FindName(key);
            return (value != null);
        }

        public bool Remove(string key)
        {
            if (!ContainsKey(key))
            {
                return false;
            }
            UnregisterName(key);
            return true;
        }

        public bool TryGetValue(string key, out object? value)
        {
            if (!ContainsKey(key))
            {
                value = null;
                return false;
            }
            value = FindName(key);
            return true;
        }

        public ICollection<string> Keys
        {
            get
            {
                if (_nameMap == null)
                    return Array.Empty<string>();

                var list = new List<string>();
                foreach (string key in _nameMap.Keys)
                {
                    list.Add(key);
                }
                return list;
            }
        }

        public ICollection<object> Values
        {
            get
            {
                if (_nameMap == null)
                {
                    return Array.Empty<object>();
                }

                var list = new List<object>();
                foreach (object value in _nameMap.Values)
                {
                    list.Add(value);
                }
                return list;
            }
        }
        #endregion

        #region class Enumerator
        class Enumerator : IEnumerator<KeyValuePair<string, object?>>
        {
            IDictionaryEnumerator? _enumerator;

            public Enumerator(HybridDictionary? nameMap)
            {
                _enumerator = null;

                if (nameMap != null)
                {
                    _enumerator = nameMap.GetEnumerator();
                }
            }

            public void Dispose()
            {
                GC.SuppressFinalize(this);
            }

            public KeyValuePair<string, object?> Current
            {
                get
                {
                    if (_enumerator == null)
                    {
                        return default(KeyValuePair<string, object?>);
                    }
                    return new KeyValuePair<string, object?>((string)_enumerator.Key, _enumerator.Value);
                }
            }

            public bool MoveNext()
            {
                if (_enumerator == null)
                {
                    return false;
                }
                return _enumerator.MoveNext();
            }

            object IEnumerator.Current
            {
                get
                {
                    return this.Current;
                }
            }

            void IEnumerator.Reset()
            {
                if (_enumerator != null)
                {
                    _enumerator.Reset();
                }
            }
        }
        #endregion
    }
}
