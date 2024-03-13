using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Markup;

namespace Wodsoft.UI.Providers
{
    public class EmbeddedResourceProvider : IResourceProvider
    {
        //private readonly Dictionary<object, object?> _caches = new Dictionary<object, object?>();
        private readonly Dictionary<Assembly, AssemblyResource> _assemblyResources = new Dictionary<Assembly, AssemblyResource>();
        private readonly object _lock = new object();

        public object? FindSystemResource(object key)
        {
            object? resource;
            Type? typeKey = key as Type;
            ResourceKey? resourceKey = (typeKey == null) ? (key as ResourceKey) : null;

            if (typeKey == null && resourceKey == null)
                return null;
            if (resourceKey != null && resourceKey.HasResource)
                return resourceKey.Resource;

            //if (!FindCachedResource(key, ref resource))
            //{
            lock (_lock)
            {
                var assembly = typeKey == null ? resourceKey!.Assembly : typeKey.Assembly;
                if (!_assemblyResources.TryGetValue(assembly, out var assemblyResource))
                {
                    assemblyResource = new AssemblyResource(assembly);
                    _assemblyResources.Add(assembly, assemblyResource);
                }
                var resources = assemblyResource.LoadThemeResource();
                if (resources != null)
                {
                    resource = resources[key];
                    if (resource != DependencyProperty.UnsetValue)
                        return resource;
                }
                resources = assemblyResource.LoadGenericResource();
                if (resources != null)
                {
                    resource = resources[key];
                    if (resource != DependencyProperty.UnsetValue)
                        return resource;
                }
            }
            return null;
            //}
        }

        //private bool FindCachedResource(object key, ref object? resource)
        //{
        //    resource = ref CollectionsMarshal.GetValueRefOrNullRef(_caches, key);
        //    if (Unsafe.IsNullRef(ref resource))
        //        return false;
        //    return true;
        //}

        private class AssemblyResource
        {
            private static readonly Assembly _FrameworkAssembly = typeof(FrameworkElement).Assembly;
            private readonly ResourceDictionaryLocation _themedLocation;
            private readonly ResourceDictionaryLocation _genericLocation;
            private readonly Assembly _assembly;
            private string? _themeName, _colorName;
            private ResourceDictionary? _themeResource, _genericResource;
            private bool _genericLoaded, _themeLoaded;

            public AssemblyResource(Assembly assembly)
            {
                if (assembly == _FrameworkAssembly)
                {
                    _themedLocation = ResourceDictionaryLocation.ExternalAssembly;
                    _genericLocation = ResourceDictionaryLocation.None;
                }
                else
                {
                    ThemeInfoAttribute? locations = ThemeInfoAttribute.FromAssembly(assembly);
                    if (locations != null)
                    {
                        _themedLocation = locations.ThemeDictionaryLocation;
                        _genericLocation = locations.GenericDictionaryLocation;
                    }
                    else
                    {
                        _themedLocation = ResourceDictionaryLocation.None;
                        _genericLocation = ResourceDictionaryLocation.None;
                    }
                }
                _assembly = assembly;
            }

            public ResourceDictionary? LoadThemeResource()
            {
                if (FrameworkProvider.ThemeProvider == null)
                    return null;
                if (FrameworkProvider.ThemeProvider.Name != _themeName || FrameworkProvider.ThemeProvider.Color != _colorName)
                {
                    _themeLoaded = false;
                    _themeName = FrameworkProvider.ThemeProvider.Name;
                    _colorName = FrameworkProvider.ThemeProvider.Color;
                }
                if (_themeLoaded || _themedLocation == ResourceDictionaryLocation.None)
                    return _themeResource;
                _themeLoaded = true;
                Assembly assembly;
                if (_themedLocation == ResourceDictionaryLocation.ExternalAssembly)
                {
                    try
                    {
                        assembly = Assembly.Load($"{_assembly.GetName().Name}.{_themeName}");
                    }
                    catch
                    {
                        return null;
                    }
                }
                else
                    assembly = _assembly;
                var loadResourceFunc = BamlResource.GetLoadResourcesFunction(assembly);
                if (loadResourceFunc == null)
                    return null;
                _themeResource = loadResourceFunc($"themes/{_themeName}.{_colorName}.xaml") as ResourceDictionary;
                return _themeResource;
            }

            public ResourceDictionary? LoadGenericResource()
            {
                if (_genericResource != null)
                    return _genericResource;
                if (_genericLoaded || _genericLocation == ResourceDictionaryLocation.None)
                    return null;
                _genericLoaded = true;
                Assembly assembly;
                if (_genericLocation == ResourceDictionaryLocation.ExternalAssembly)
                {
                    try
                    {
                        assembly = Assembly.Load($"{_assembly.GetName().Name}.generic");
                    }
                    catch
                    {
                        return null;
                    }
                }
                else
                    assembly = _assembly;

                var loadResourceFunc = BamlResource.GetLoadResourcesFunction(assembly);
                if (loadResourceFunc == null)
                    return null;
                _genericResource = loadResourceFunc("themes/generic.xaml") as ResourceDictionary;
                return _genericResource;
            }
        }
    }
}
