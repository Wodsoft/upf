using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Data
{
    public class SliceList : IList
    {
        private readonly IList _source;
        private readonly int _startIndex;
        private readonly int _count;

        public SliceList(IList source, int startIndex, int count)
        {
            _source = source;
            _startIndex = startIndex;
            _count = count;
        }

        public object? this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                    throw new ArgumentOutOfRangeException("index");
                return _source[index + _startIndex];
            }
            set => throw new NotSupportedException("List is readonly.");
        }

        public bool IsFixedSize => true;

        public bool IsReadOnly => true;

        public int Count => _count;

        public bool IsSynchronized => _source.IsSynchronized;

        public object SyncRoot => _source.SyncRoot;

        public int Add(object? value)
        {
            throw new NotSupportedException("List is readonly.");
        }

        public void Clear()
        {
            throw new NotSupportedException("List is readonly.");
        }

        public bool Contains(object? value)
        {
            for (int i = 0; i < _count; i++)
            {
                if (Equals(_source[_startIndex], value))
                    return true;
            }
            return false;
        }

        public void CopyTo(Array array, int index)
        {
            for (int i = 0; i < _count; i++)
            {
                array.SetValue(_source[_startIndex + i], index + i);
            }
        }

        public IEnumerator GetEnumerator()
        {
            return new Enumerator(_source, _startIndex, _count);
        }

        public int IndexOf(object? value)
        {
            for (int i = 0; i < _count; i++)
            {
                if (Equals(_source[_startIndex], value))
                    return i;
            }
            return -1;
        }

        public void Insert(int index, object? value)
        {
            throw new NotSupportedException("List is readonly.");
        }

        public void Remove(object? value)
        {
            throw new NotSupportedException("List is readonly.");
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException("List is readonly.");
        }

        public class Enumerator : IEnumerator
        {
            private readonly IList _source;
            private readonly int _startIndex;
            private readonly int _count;
            private int _current;

            public Enumerator(IList source, int startIndex, int count)
            {
                _source = source;
                _startIndex = startIndex;
                _count = count;
            }

            public object? Current => _source[_startIndex + _current];

            public bool MoveNext()
            {
                if (_current < _count - 1)
                {
                    _current++;
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                _current = 0;
            }
        }
    }
}
