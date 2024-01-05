using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public class LogicalObject : DependencyObject
    {
        #region Logical

        private List<LogicalObject>? _children;
        private ReadOnlyCollection<LogicalObject>? _readonlyChildren;
        private LogicalObject? _root, _parent;

        public event EventHandler<LogicalParentChangedEventArgs>? LogicalParentChanged;

        public event EventHandler<LogicalRootChangedEventArgs>? LogicalRootChanged;

        protected internal LogicalObject? LogicalParent => _parent;

        protected internal LogicalObject? LogicalRoot => _root;

        protected internal IEnumerable<LogicalObject>? LogicalChildren => _readonlyChildren;

        protected override DependencyObject? GetInheritanceParent()
        {
            return _parent;
        }

        protected internal void AddLogicalChild(LogicalObject child)
        {
            if (child == null)
                throw new ArgumentNullException(nameof(child));
            if (child._parent != null)
                throw new InvalidOperationException("Child has its parent already.");
            if (_root == child)
                throw new InvalidOperationException("Can not add child because it's root of current object.");
            child._parent = this;
            if (_children == null)
            {
                _children = new List<LogicalObject>();
                _readonlyChildren = _children.AsReadOnly();
            }
            _children.Add(child);
            child.LogicalParentChanged?.Invoke(child, new LogicalParentChangedEventArgs(null, this));
            child.RootChanged(_root ?? this);
        }

        protected internal void RemoveLogicalChild(LogicalObject child)
        {
            if (child == null)
                throw new ArgumentNullException(nameof(child));
            if (child._parent == null)
                throw new InvalidOperationException("Child has no parent.");
            if (child._parent != this)
                throw new InvalidOperationException("This element is not the parent of child.");
            child._parent = null;
            _children!.Remove(child);
            if (_children.Count == 0)
            {
                _children = null;
                _readonlyChildren = null;
            }
            child.LogicalParentChanged?.Invoke(child, new LogicalParentChangedEventArgs(this, null));
            child.RootChanged(null);
        }

        private void RootChanged(LogicalObject? root)
        {
            var oldRoot = _root;
            _root = root;
            if (_children != null)
            {
                foreach (var child in _children)
                    child.RootChanged(root ?? this);
            }
            LogicalRootChanged?.Invoke(this, new LogicalRootChangedEventArgs(oldRoot ?? this, root ?? this));
            OnLogicalRootChanged(oldRoot ?? this, root ?? this);
        }

        protected virtual void OnLogicalRootChanged(LogicalObject oldRoot, LogicalObject newRoot)
        {

        }

        #endregion

        #region InheritanceContext

        protected override bool CanBeInheritanceContext => true;

        protected override bool ShouldProvideInheritanceContext(DependencyObject target, DependencyProperty property)
        {
            return target is not LogicalObject;
        }

        #endregion
    }
}
