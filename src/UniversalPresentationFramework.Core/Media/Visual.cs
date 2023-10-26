using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    public abstract class Visual : DependencyObject
    {
        private Visual? _parent;

        protected void AddVisualChild(Visual child)
        {
            if (child == null)
                return;
            if (child._parent != null)
                throw new ArgumentException("Child has parent.");
            child._parent = this;
            OnVisualChildrenChanged(child, null);
            child.OnVisualParentChanged(null);
        }

        protected void RemoveVisualChild(Visual child)
        {
            if (child == null)
                return;
            if (child._parent != this)
                throw new ArgumentException("Not child parent.");
            child._parent = null;
            child.OnVisualParentChanged(this);
            OnVisualChildrenChanged(null, child);
        }

        protected virtual Visual GetVisualChild(int index)
        {
            throw new ArgumentOutOfRangeException("index");
        }

        protected virtual int VisualChildrenCount => 0;

        protected Visual? VisualParent => _parent;

        //internal void InternalAddVisualChild(Visual child)
        //{
        //    AddVisualChild(child);
        //}

        //internal void InternalRemoveVisualChild(Visual child)
        //{
        //    RemoveVisualChild(child);
        //}

        //internal Visual InternalGetVisualChild(int index)
        //{
        //    return GetVisualChild(index);
        //}

        protected virtual void OnVisualChildrenChanged(Visual? visualAdded, Visual? visualRemoved) { }

        protected virtual void OnVisualParentChanged(Visual? oldParent) { }
    }
}
