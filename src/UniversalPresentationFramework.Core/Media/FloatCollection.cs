using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    public class FloatCollection : Freezable, IList<float>
    {
        private List<float> _list;

        public FloatCollection()
        {
            _list = new List<float>();
        }

        public FloatCollection(IEnumerable<float> items)
        {
            _list = new List<float>(items);
        }

        public readonly static FloatCollection Empty;

        static FloatCollection()
        {
            Empty = new FloatCollection();
            Empty.Freeze();
        }

        public int Count => _list.Count;

        bool ICollection<float>.IsReadOnly => IsFrozen;

        public float this[int index]
        {
            get => _list[index];
            set
            {
                if (IsFrozen)
                    throw new InvalidOperationException("Object is frozen.");
                _list[index] = value;
            }
        }

        public int IndexOf(float item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, float item)
        {
            if (IsFrozen)
                throw new InvalidOperationException("Object is frozen.");
            _list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            if (IsFrozen)
                throw new InvalidOperationException("Object is frozen.");
            _list.RemoveAt(index);
        }

        public void Add(float item)
        {
            if (IsFrozen)
                throw new InvalidOperationException("Object is frozen.");
            _list.Add(item);
        }

        public void Clear()
        {
            if (IsFrozen)
                throw new InvalidOperationException("Object is frozen.");
            _list.Clear();
        }

        public bool Contains(float item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(float[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public bool Remove(float item)
        {
            if (IsFrozen)
                throw new InvalidOperationException("Object is frozen.");
            return _list.Remove(item);
        }

        public IEnumerator<float> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}
