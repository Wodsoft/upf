using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Controls
{
    public class DefinitionCollection<T> : IList<T>
        where T : DefinitionBase
    {
        private readonly Grid _grid;
        private readonly List<T> _items;

        internal DefinitionCollection(Grid grid)
        {
            _grid = grid;
            _items = new List<T>();
        }

        public T this[int index]
        {
            get => _items[index];
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                value.ConnectParent(_grid);
                _items[index].DisconnectParent(_grid);
                _items[index] = value;
            }
        }

        public int Count => _items.Count;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            if (IsReadOnly)
                throw new InvalidOperationException("Collection is readonly right now.");
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            item.ConnectParent(_grid);
            _items.Add(item);
        }

        public void Clear()
        {
            if (IsReadOnly)
                throw new InvalidOperationException("Collection is readonly right now.");
            for (int i = 0; i < _items.Count; i++)
                _items[i].DisconnectParent(_grid);
            _items.Clear();
        }

        public bool Contains(T item) => _items.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

        public int IndexOf(T item) => _items.IndexOf(item);

        public void Insert(int index, T item)
        {
            if (IsReadOnly)
                throw new InvalidOperationException("Collection is readonly right now.");
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (index < 0 || index >= _items.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            item.ConnectParent(_grid);
            _items.Insert(index, item);
        }

        public bool Remove(T item)
        {
            if (IsReadOnly)
                throw new InvalidOperationException("Collection is readonly right now.");
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            item.DisconnectParent(_grid);
            return _items.Remove(item);
        }

        public void RemoveAt(int index)
        {
            if (IsReadOnly)
                throw new InvalidOperationException("Collection is readonly right now.");
            var item = _items[index];
            item.DisconnectParent(_grid);
            _items.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();

        public List<T> GetRange(int index, int count)
        {
            return _items.GetRange(index, count);
        }
    }
}
