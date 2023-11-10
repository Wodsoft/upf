using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Controls
{
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
