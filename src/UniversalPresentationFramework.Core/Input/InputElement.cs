using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Input
{
    internal class InputElement
    {
        internal static DependencyObject? GetContainingUIElement(DependencyObject? o)
        {
            DependencyObject? container = null;

            if (o != null)
            {
                if (o is UIElement)
                {
                    container = o;
                }
                else if (o is ContentElement contentElement)
                {
                    DependencyObject? parent = LogicalTreeHelper.GetParent(contentElement);
                    if (parent != null)
                    {
                        container = GetContainingUIElement(parent);
                    }
                }
                else if (o is Visual v)
                {
                    DependencyObject? parent = VisualTreeHelper.GetParent(v);
                    if (parent != null)
                    {
                        container = GetContainingUIElement(parent);
                    }
                }
            }

            return container;
        }
    }
}
