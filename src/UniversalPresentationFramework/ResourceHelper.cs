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
        public static object? FindResource(LogicalObject logicalObject, object key)
        {
            var value = FindResourceInTree(logicalObject, key);
            if (value != DependencyProperty.UnsetValue)
                return value;
            value = FindResourceInApplication(key);
            return value;
        }

        public static object? FindResourceInTree(LogicalObject logicalObject, object key)
        {
            while (true)
            {
                if (logicalObject is FrameworkElement fe)
                {
                    var value = fe.Resources[key];
                    if (value != DependencyProperty.UnsetValue)
                        return value;
                    if (fe.TemplatedParent != null)
                    {
                        var template = fe.TemplatedParent.GetTemplateInternal();
                        if (template != null)
                        {
                            value = template.Resources[key];
                            if (value != DependencyProperty.UnsetValue)
                                return value;
                        }
                        logicalObject = fe.TemplatedParent;
                        continue;
                    }
                }
                if (logicalObject is FrameworkContentElement fce)
                {
                    var value = fce.Resources[key];
                    if (value != DependencyProperty.UnsetValue)
                        return value;
                }
                if (logicalObject.LogicalParent == null)
                    return DependencyProperty.UnsetValue;
                logicalObject = logicalObject.LogicalParent;
            }
        }

        public static object? FindResourceInApplication(object key)
        {
            var app = Application.Current;
            if (app == null || app.Resources == null)
                return DependencyProperty.UnsetValue;
            return app.Resources[key];
        }
    }
}
