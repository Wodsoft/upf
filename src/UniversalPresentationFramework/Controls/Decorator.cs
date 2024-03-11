using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using Wodsoft.UI.Markup;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Controls
{
    [ContentProperty("Child")]
    public class Decorator : FrameworkElement, IAddChild
    {
        private UIElement? _child;
        public virtual UIElement? Child
        {
            get
            {
                return _child;
            }

            set
            {
                if (_child == value)
                    return;
                if (_child != null)
                {
                    // notify the visual layer that the old child has been removed.
                    RemoveVisualChild(_child);
                    //need to remove old element from logical tree
                    RemoveLogicalChild(_child);
                }
                _child = value;
                if (value != null)
                {
                    AddLogicalChild(value);
                    // notify the visual layer about the new child.
                    AddVisualChild(value);
                    InvalidateMeasure();
                }
            }
        }

        void IAddChild.AddChild(object value)
        {
            if (value is UIElement element)
                Child = element;
            else
                throw new NotSupportedException("Decorator only support UIElement child.");
        }

        void IAddChild.AddText(string text)
        {
            throw new NotSupportedException("Decorator not support text child.");
        }

        protected internal override int VisualChildrenCount => _child == null ? 0 : 1;

        protected internal override Visual GetVisualChild(int index)
        {
            if (_child == null || index != 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            return _child;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (_child == null)
                return new Size(0.0f, 0.0f);
            _child.Measure(availableSize);
            return _child.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (_child != null)
                _child.Arrange(new Rect(finalSize));
            return finalSize;
        }
    }
}
