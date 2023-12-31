using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Wodsoft.UI
{
    internal static class ResourceHelper
    {
        public static object? FindResource(FrameworkElement element, object key)
        {
            var value = FindResourceInTree(element, key);
            if (value != null)
                return value;
            value = FindResourceInApplication(key);
            return value;
        }

        public static object? FindResourceInTree(FrameworkElement fe, object key)
        {
            LogicalObject logicalObject = fe;
            while (true)
            {
                if (logicalObject is FrameworkElement element && element.Resources != null)
                {
                    var value = element.Resources[key];
                    if (value != null)
                        return value;
                }
                if (logicalObject.LogicalParent == null)
                    return null;
                logicalObject = logicalObject.LogicalParent;
            }
        }

        public static object? FindResourceInApplication(object key)
        {
            var app = Application.Current;
            if (app == null || app.Resources == null)
                return null;
            return app.Resources[key];
        }
    }
}
