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

        internal static Visual? GetRootVisual(DependencyObject d)
        {
            Visual? rootVisual = GetContainingVisual(d);
            Visual? parentVisual;
            while (rootVisual != null && ((parentVisual = VisualTreeHelper.GetParent(rootVisual)) != null))
            {
                rootVisual = parentVisual;
            }
            return rootVisual;
        }

        internal static Visual GetRootVisual(Visual visual)
        {
            Visual rootVisual = visual;
            Visual? parentVisual;
            while ((parentVisual = VisualTreeHelper.GetParent(rootVisual)) != null)
            {
                rootVisual = parentVisual;
            }
            return rootVisual;
        }

        internal static Visual? GetContainingVisual(DependencyObject o)
        {
            Visual? v = o as Visual;
            if (v != null)
                return v;
            var logicalObject = o as LogicalObject;
            while (logicalObject != null)
            {
                if (logicalObject.LogicalParent == null)
                    return null;
                logicalObject = logicalObject.LogicalParent;
                if (logicalObject is Visual visual)
                    return visual;
            }
            return null;
        }
    }
}
