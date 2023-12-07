using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Controls
{
    [ContentProperty("Children")]
    public class Panel : FrameworkElement
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

        protected internal override int VisualChildrenCount => _children?.Count ?? 0;

        protected internal override Visual GetVisualChild(int index)
        {
            if (_children == null)
                throw new ArgumentOutOfRangeException(nameof(index));
            return _children[index]!;
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
