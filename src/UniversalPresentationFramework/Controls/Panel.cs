using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using Wodsoft.UI.Markup;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Controls
{
    [ContentProperty("Children")]
    public class Panel : FrameworkElement, IAddChild
    {
        private UIElementCollection? _children;

        public UIElementCollection Children
        {
            get
            {
                if (_children == null)
                {
                    if (IsItemsHost)
                        _children = new UIElementCollection(this, null);
                    else
                        _children = new UIElementCollection(this, this);
                }
                return _children;
            }
        }

        //protected internal override IEnumerable<LogicalObject>? LogicalChildren => _children;

        protected internal override int VisualChildrenCount => _children?.Count ?? 0;

        protected internal override Visual GetVisualChild(int index)
        {
            if (_children == null)
                throw new ArgumentOutOfRangeException(nameof(index));
            return _children[index]!;
        }

        void IAddChild.AddChild(object value)
        {
            AddChild(value);
        }

        protected virtual void AddChild(object value)
        {
            if (value is UIElement element)
                Children.Add(element);
            else
                throw new NotSupportedException("Panel can add UIElement only.");
        }

        void IAddChild.AddText(string text)
        {
            throw new NotSupportedException("Panel doesn't support add text.");
        }

        public static readonly DependencyProperty IsItemsHostProperty =
        DependencyProperty.Register(
                "IsItemsHost",
                typeof(bool),
                typeof(Panel),
                new FrameworkPropertyMetadata(
                        false, // defaultValue
                        FrameworkPropertyMetadataOptions.NotDataBindable));
        public bool IsItemsHost
        {
            get { return (bool)GetValue(IsItemsHostProperty)!; }
            set { SetValue(IsItemsHostProperty, value); }
        }
    }
}
