using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Wodsoft.UI.Controls;

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
                else if (logicalObject is FrameworkContentElement fce)
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


        public static object? FindTemplateResource(LogicalObject target, object item, Type templateType)
        {
            Type? type = ContentPresenter.DataTypeForItem(item, target);

            List<object> keys = new List<object>();

            // add compound keys for the dataType and all its base types
            while (type != null)
            {
                object? key = null;
                if (templateType == typeof(ItemContainerTemplate))
                    key = new ItemContainerTemplateKey(type);
                else if (templateType == typeof(DataTemplate))
                    key = new DataTemplateKey(type);

                if (key != null)
                    keys.Add(key);

                type = type.BaseType;
                if (type == typeof(object))     // don't search for Object - perf
                    type = null;
            }

            Span<object> searchKeys = CollectionsMarshal.AsSpan(keys);

            // Search the parent chain
            object? resource = FindTemplateResourceInTree(target, searchKeys, out var matchIndex);

            if (matchIndex != 0)
            {
                if (matchIndex != -1)
                    searchKeys = searchKeys.Slice(0, matchIndex);
                // Exact match not found in the parent chain.  Try App and System Resources.
                object? appResource = FindTemplateResourceFromAppOrSystem(target, searchKeys, out matchIndex);
                if (appResource != null)
                    resource = appResource;
            }

            return resource;
        }

        private static object? FindTemplateResourceInTree(LogicalObject? target, Span<object> keys, out int matchIndex)
        {
            ResourceDictionary? resources;
            object? value = null;
            matchIndex = -1;
            while (target != null)
            {
                if (target is not FrameworkElement && target is not FrameworkContentElement)
                {
                    target = target.LogicalParent;
                    continue;
                }

                object? candidate;

                // -------------------------------------------
                //  Lookup ResourceDictionary on the current instance
                // -------------------------------------------

                // Fetch the ResourceDictionary
                // for the given target element
                resources = GetFrameworkResources(target);
                if (resources != null)
                {
                    candidate = FindBestMatchInResourceDictionary(resources, keys, out matchIndex);
                    if (candidate != null)
                    {
                        value = candidate;
                        if (matchIndex == 0)
                        {
                            // Exact match found, stop here.
                            return value;
                        }
                        else
                        {
                            keys = keys.Slice(0, matchIndex);
                        }
                    }
                }

                // -------------------------------------------
                //  Lookup ResourceDictionary on the current instance's Style, if one exists.
                // -------------------------------------------

                resources = GetStyleResources(target);
                if (resources != null)
                {
                    candidate = FindBestMatchInResourceDictionary(resources, keys, out matchIndex);
                    if (candidate != null)
                    {
                        value = candidate;
                        if (matchIndex == 0)
                        {
                            // Exact match found, stop here.
                            return value;
                        }
                        else
                        {
                            keys = keys.Slice(0, matchIndex);
                        }
                    }
                }

                // -------------------------------------------
                //  Lookup ResourceDictionary on the current instance's Theme Style, if one exists.
                // -------------------------------------------

                resources = GetThemeResources(target);
                if (resources != null)
                {
                    candidate = FindBestMatchInResourceDictionary(resources, keys, out matchIndex);
                    if (candidate != null)
                    {
                        value = candidate;
                        if (matchIndex == 0)
                        {
                            // Exact match found, stop here.
                            return value;
                        }
                        else
                        {
                            keys = keys.Slice(0, matchIndex);
                        }
                    }
                }

                // -------------------------------------------
                //  Lookup ResourceDictionary on the current instance's Template, if one exists.
                // -------------------------------------------

                resources = GetTemplateResources(target);
                if (resources != null)
                {
                    candidate = FindBestMatchInResourceDictionary(resources, keys, out matchIndex);
                    if (candidate != null)
                    {
                        value = candidate;
                        if (matchIndex == 0)
                        {
                            // Exact match found, stop here.
                            return value;
                        }
                        else
                        {
                            keys = keys.Slice(0, matchIndex);
                        }
                    }
                }

                // -------------------------------------------
                //  Find the next parent instance to lookup
                // -------------------------------------------

                // Get Framework Parent
                target = target.LogicalParent;
            }

            return value;
        }

        private static object? FindBestMatchInResourceDictionary(
            ResourceDictionary resources, Span<object> keys, out int matchIndex)
        {
            object? resource = null;

            // Search target element's ResourceDictionary for the resource
            if (resources != null)
            {
                for (var k = 0; k < keys.Length; k++)
                {
                    object? candidate = resources[keys[k]];
                    if (candidate != null)
                    {
                        resource = candidate;
                        matchIndex = k;
                        return resource;
                    }
                }
            }
            matchIndex = -1;
            return resource;
        }


        private static object? FindTemplateResourceFromAppOrSystem(LogicalObject target, Span<object> keys, out int matchIndex)
        {
            object? resource = null;
            int k;
            matchIndex = -1;

            Application? app = Application.Current;
            if (app != null)
            {
                // If the element is rooted to a Window and App exists, defer to App.
                for (k = 0; k < keys.Length; k++)
                {
                    resource = app.Resources?[keys[k]];
                    if (resource != null)
                    {
                        matchIndex = k;
                        if (matchIndex == 0)
                            return resource;
                        else
                        {
                            keys = keys.Slice(0, matchIndex);
                            break;
                        }
                    }
                }
            }

            // if best match is not found from the application level,
            // try it from system level.
            if (FrameworkProvider.ResourceProvider != null)
            {
                // Try the system resource collection.
                for (k = 0; k < keys.Length; k++)
                {
                    object? sysResource = FrameworkProvider.ResourceProvider.FindSystemResource(keys[k]);
                    if (sysResource != null)
                    {
                        matchIndex = k;
                        resource = sysResource;
                        return resource;
                    }
                }
            }

            return resource;
        }

        private static ResourceDictionary? GetFrameworkResources(LogicalObject target)
        {
            if (target is FrameworkElement fe)
                return fe.Resources;
            else if (target is FrameworkContentElement fce)
                return fce.Resources;
            else
                return null;
        }

        private static ResourceDictionary? GetStyleResources(LogicalObject target)
        {
            if (target is FrameworkElement fe)
                return fe.Style?.Resources;
            //else if (target is FrameworkContentElement fce)
            //    return fce.Style?.Resources;
            else
                return null;
        }

        private static ResourceDictionary? GetThemeResources(LogicalObject target)
        {
            if (target is FrameworkElement fe)
                return fe.ThemeStyle?.Resources;
            //else if (target is FrameworkContentElement fce)
            //    return fce.Style?.Resources;
            else
                return null;
        }

        private static ResourceDictionary? GetTemplateResources(LogicalObject target)
        {
            if (target is FrameworkElement fe)
                return fe.LastTemplate?.Resources;
            //else if (target is FrameworkContentElement fce)
            //    return fce.Style?.Resources;
            else
                return null;
        }
    }
}
