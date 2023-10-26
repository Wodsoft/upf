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

        #endregion
    }
}
