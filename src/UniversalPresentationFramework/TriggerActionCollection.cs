using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Controls;

namespace Wodsoft.UI
{
    public sealed class TriggerActionCollection : IList<TriggerAction>
    {
        private readonly List<TriggerAction> _list;
        private readonly TriggerBase _owner;
        private bool _isSealed;

        public TriggerActionCollection(TriggerBase owner)
        {
            _owner = owner;
            _list = new List<TriggerAction>();
        }

        internal TriggerActionCollection(TriggerBase owner, List<TriggerAction> actions)
        {
            _owner = owner;
            _list = actions;
        }

        //public TriggerActionCollection(int initialSize)
        //{
        //    _rawList = new List<TriggerAction>(initialSize);
        //}

        public int Count
        {
            get
            {
                return _list.Count;
            }
        }

        /// <summary>
        ///     IList.IsReadOnly
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return _isSealed;
            }
        }

        /// <summary>
        ///     IList.Clear
        /// </summary>
        public void Clear()
        {
            CheckSealed();

            for (int i = _list.Count - 1; i >= 0; i--)
            {
                _owner.RemoveSelfAsInheritanceContext(_list[i], null);
            }

            _list.Clear();
        }

        /// <summary>
        ///     IList.RemoveAt
        /// </summary>
        public void RemoveAt(int index)
        {
            CheckSealed();
            TriggerAction oldValue = _list[index];
            _owner.RemoveSelfAsInheritanceContext(oldValue, null);
            _list.RemoveAt(index);

        }

        ///////////////////////////////////////////////////////////////////////
        //  Strongly-typed implementations

        /// <summary>
        ///     IList.Add
        /// </summary>

        public void Add(TriggerAction value)
        {
            CheckSealed();
            _owner.ProvideSelfAsInheritanceContext(value, null);
            _list.Add(value);
        }


        /// <summary>
        ///     IList.Contains
        /// </summary>
        public bool Contains(TriggerAction value)
        {
            return _list.Contains(value);
        }

        /// <summary>
        ///     ICollection.CopyTo
        /// </summary>
        public void CopyTo(TriggerAction[] array, int index)
        {
            _list.CopyTo(array, index);
        }

        /// <summary>
        ///     IList.IndexOf
        /// </summary>
        public int IndexOf(TriggerAction value)
        {
            return _list.IndexOf(value);
        }

        /// <summary>
        ///     IList.Insert
        /// </summary>
        public void Insert(int index, TriggerAction value)
        {
            CheckSealed();
            _owner.ProvideSelfAsInheritanceContext(value, null);
            _list.Insert(index, value);

        }

        /// <summary>
        ///     IList.Remove
        /// </summary>
        public bool Remove(TriggerAction value)
        {
            CheckSealed();
            _owner.RemoveSelfAsInheritanceContext(value, null);
            bool wasRemoved = _list.Remove(value);
            return wasRemoved;
        }

        /// <summary>
        ///     IList.Item
        /// </summary>
        public TriggerAction this[int index]
        {
            get
            {
                return _list[index];
            }
            set
            {
                CheckSealed();

                var oldValue = _list[index];
                _owner.RemoveSelfAsInheritanceContext(oldValue, null);
                _list[index] = value;
                _owner.ProvideSelfAsInheritanceContext(value, null);
            }
        }

        public IEnumerator<TriggerAction> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_list).GetEnumerator();
        }

        internal void Seal()
        {
            for (int i = 0; i < _list.Count; i++)
            {
                _list[i].Seal();
            }
        }

        private void CheckSealed()
        {
            if (_isSealed)
                throw new InvalidOperationException("Object is sealed.");
        }

        internal static readonly TriggerActionCollection ReadOnly;

        static TriggerActionCollection()
        {
#pragma warning disable CS8625 // 无法将 null 字面量转换为非 null 的引用类型。
            ReadOnly = new TriggerActionCollection(null);
            ReadOnly.Seal();
#pragma warning restore CS8625 // 无法将 null 字面量转换为非 null 的引用类型。
        }
    }
}
