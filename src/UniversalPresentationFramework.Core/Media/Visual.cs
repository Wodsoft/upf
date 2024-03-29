using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    public abstract class Visual : LogicalObject
    {
        #region Tree

        private Visual? _parent;
        internal int ParentIndex;

        protected internal void AddVisualChild(Visual child)
        {
            if (child == null)
                return;
            if (child._parent != null)
                throw new ArgumentException("Child has parent.");
            child._parent = this;
            OnVisualChildrenChanged(child, null);
            child.OnVisualParentChanged(null);
        }

        protected internal void RemoveVisualChild(Visual child)
        {
            if (child == null)
                return;
            if (child._parent != this)
                throw new ArgumentException("Not child parent.");
            child._parent = null;
            child.OnVisualParentChanged(this);
            OnVisualChildrenChanged(null, child);
        }

        protected internal virtual Visual GetVisualChild(int index)
        {
            throw new ArgumentOutOfRangeException("index");
        }

        protected internal virtual int VisualChildrenCount => 0;

        public Visual? VisualParent => _parent;

        protected virtual void OnVisualChildrenChanged(Visual? visualAdded, Visual? visualRemoved) { }

        protected virtual void OnVisualParentChanged(Visual? oldParent) { }

        public bool IsRootElement { get; }

        public bool HasVisualParent => _parent != null;

        #endregion

        #region Render

        public abstract void RenderContext(RenderContext renderContext);

        public abstract bool HasRenderContent { get; }

        public virtual Size GetVisualSize()
        {
            return Size.Empty;
        }

        private Vector2 _visualOffset;
        public Vector2 VisualOffset
        {
            get => _visualOffset;
            protected set => _visualOffset = value;
        }

        public virtual DpiScale GetDpi()
        {
            return DpiScale.Default;
        }

        #endregion
    }
}
