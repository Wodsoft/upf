using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public class FrameworkElement : UIElement
    {
        #region Logical

        public FrameworkElement? Parent { get; private set; }

        protected override DependencyObject? GetInheritanceParent()
        {
            return Parent;
        }

        protected internal void AddLogicalChild(FrameworkElement child)
        {
            if (child == null)
                throw new ArgumentNullException(nameof(child));
            if (child.Parent != null)
                throw new InvalidOperationException("Child has its parent already.");
            child.Parent = this;
        }

        protected internal void RemoveLogicalChild(FrameworkElement child)
        {
            if (child == null)
                throw new ArgumentNullException(nameof(child));
            if (child.Parent == null)
                throw new InvalidOperationException("Child has no parent.");
            if (child.Parent != this)
                throw new InvalidOperationException("This element is not the parent of child.");
            child.Parent = null;
        }

        #endregion

        #region Layout

        protected sealed override Size MeasureCore(Size availableSize)
        {
            return base.MeasureCore(availableSize);
        }

        protected sealed override void ArrangeCore(Rect finalRect)
        {
            base.ArrangeCore(finalRect);
        }

        protected virtual Size MeasureOverride(Size availableSize)
        {
            return new Size(0, 0);
        }

        protected virtual Size ArrangeOverride(Size finalSize)
        {
            return finalSize;
        }

        #endregion
    }
}
