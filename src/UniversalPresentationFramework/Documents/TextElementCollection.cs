using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Documents
{
    public abstract class TextElementCollection<TElement> : IList, IList<TElement>, IReadOnlyList<TElement>
        where TElement : TextElement
    {
        private readonly LogicalObject _parent;
        private readonly TextTreeNode _parentNode;
        private List<TElement> _elements;
        private int _version;

        protected TextElementCollection(LogicalObject parent, TextTreeNode parentNode)
        {
            _elements = new List<TElement>();
            _parent = parent;
            _parentNode = parentNode;
        }

        public TElement this[int index]
        {
            get => _elements[index];
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
            }
        }

        public int Count => _elements.Count;

        public bool IsReadOnly => false;

        public int Version => _version;

        public void Add(TElement item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            AddElement(item, _parentNode, ElementEdge.BeforeEnd);
            _elements.Add(item);
        }

        public void Clear()
        {
            for (int i = 0; i < _elements.Count; i++)
                RemoveElement(_elements[i]);
            _elements.Clear();
        }

        public bool Contains(TElement item) => _elements.Contains(item);

        public void CopyTo(TElement[] array, int arrayIndex) => _elements.CopyTo(array, arrayIndex);

        public IEnumerator<TElement> GetEnumerator() => _elements.GetEnumerator();

        public int IndexOf(TElement item) => _elements.IndexOf(item);

        public void Insert(int index, TElement item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            if (index == 0)
                AddElement(item, _parentNode, ElementEdge.AfterStart);
            else
                AddElement(item, _elements[index - 1].TextElementNode, ElementEdge.AfterEnd);
            _elements.Insert(index, item);
        }

        public bool Remove(TElement item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            RemoveElement(item);
            return _elements.Remove(item);
        }

        public void RemoveAt(int index)
        {
            var item = _elements[index];
            RemoveElement(item);
            _elements.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator() => _elements.GetEnumerator();

        private void AddElement(TElement item, TextTreeNode relativeTo, ElementEdge edge)
        {
            _parent.AddLogicalChild(item);
            relativeTo.InsertNodeAt(item.TextElementNode, edge);
            OnAddElement(item);
            unchecked { _version++; }
        }

        private void RemoveElement(TElement item)
        {
            OnRemoveElement(item);
            item.TextElementNode.RemoveFromTree();
            _parent.RemoveLogicalChild(item);
            unchecked { _version++; }
        }

        protected virtual void OnAddElement(TElement item) { }

        protected virtual void OnRemoveElement(TElement item) { }

        #region IList

        bool IList.IsFixedSize => false;

        bool ICollection.IsSynchronized => ((ICollection)_elements).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)_elements).SyncRoot;

        object? IList.this[int index]
        {
            get => this[index]; set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                var element = ConvertToElement(value);
                this[index] = element;
            }
        }

        int IList.Add(object? value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            Add(ConvertToElement(value));
            return _elements.Count - 1;
        }

        bool IList.Contains(object? value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            return Contains(ConvertToElement(value));
        }

        int IList.IndexOf(object? value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (value is TElement element)
                return IndexOf(element);
            throw new ArgumentException($"IndexOf only support \"{typeof(TElement).FullName}\" element.");
        }

        void IList.Insert(int index, object? value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            Insert(index, ConvertToElement(value));
        }

        void IList.Remove(object? value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (value is TElement element)
            {
                Remove(element);
                return;
            }
            throw new ArgumentException($"IndexOf only support \"{typeof(TElement).FullName}\" element.");
        }

        void ICollection.CopyTo(Array array, int index)
        {
            CopyTo((TElement[])array, index);
        }

        protected abstract TElement ConvertToElement(object value);

        #endregion
    }
}
