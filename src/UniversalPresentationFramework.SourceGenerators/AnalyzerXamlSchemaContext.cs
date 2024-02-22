using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xaml;
using System.Xaml.Markup;

namespace UniversalPresentationFramework.SourceGenerators
{
    public class AnalyzerXamlSchemaContext : XamlSchemaContext
    {
        private Compilation _compilation;
        private readonly Dictionary<string, IAssemblySymbol> _assemblies = new Dictionary<string, IAssemblySymbol>();
        private readonly Dictionary<string, List<(IAssemblySymbol Assembly, string Namespace)>> _namespaceMaps = new Dictionary<string, List<(IAssemblySymbol Assembly, string Namespace)>>();
        private readonly Dictionary<(string Namespace, string Name), XamlType> _namespaceCaches = new Dictionary<(string Namespace, string Name), XamlType>();
        private readonly Dictionary<ITypeSymbol, AnalyzerXamlType> _typeCaches = new Dictionary<ITypeSymbol, AnalyzerXamlType>();
        private INamedTypeSymbol _contentPropertyAttribute, _objectType, _dictionaryInterface, _listInterface, _enumerableInterface, _xamlDeferLoadAttribute;
        private IAssemblySymbol _immutableAssembly;

        public void SetCompilation(Compilation compilation)
        {
            if (_compilation == null)
            {
                _compilation = compilation;
                _assemblies.Add(compilation.Assembly.Name, compilation.Assembly);
                foreach (var assemblySymbol in compilation.SourceModule.ReferencedAssemblySymbols)
                {
                    _assemblies.Add(assemblySymbol.Name, assemblySymbol);
                    var attributes = assemblySymbol.GetAttributes();
                    foreach (var attr in attributes)
                    {
                        if (attr.AttributeClass.ToDisplayString() != "System.Xaml.Markup.XmlnsDefinitionAttribute")
                            continue;
                        if (attr.ConstructorArguments.Length != 2)
                            continue;
                        var xmlNs = attr.ConstructorArguments[0].Value as string;
                        var clrNs = attr.ConstructorArguments[1].Value as string;
                        if (xmlNs == null || clrNs == null)
                            continue;
                        if (!_namespaceMaps.TryGetValue(xmlNs, out var map))
                        {
                            map = new List<(IAssemblySymbol, string)>();
                            _namespaceMaps.Add(xmlNs, map);
                        }
                        map.Add((assemblySymbol, clrNs));
                    }
                }
                if (_assemblies.TryGetValue("UniversalPresentationFramework.Xaml", out var xamlAssembly))
                    _contentPropertyAttribute = xamlAssembly.GetTypeByMetadataName("System.Xaml.Markup.ContentPropertyAttribute");
                _objectType = compilation.GetTypeByMetadataName("System.Object");
                _dictionaryInterface = compilation.GetTypeByMetadataName("System.Collections.IDictionary");
                _listInterface = compilation.GetTypeByMetadataName("System.Collections.IList");
                _enumerableInterface = compilation.GetTypeByMetadataName("System.Collections.IEnumerable");
                _xamlDeferLoadAttribute = compilation.GetTypeByMetadataName("System.Xaml.Markup.XamlDeferLoadAttribute");
                _assemblies.TryGetValue("System.Collections.Immutable", out _immutableAssembly);
            }
        }

        public Compilation Compilation => _compilation;

        public INamedTypeSymbol ContentPropertyAttribute => _contentPropertyAttribute;

        public INamedTypeSymbol ObjectType => _objectType;

        public INamedTypeSymbol DictionaryInterface => _dictionaryInterface;

        public INamedTypeSymbol ListInterface => _listInterface;

        public INamedTypeSymbol EnumerableInterface => _enumerableInterface;

        public INamedTypeSymbol XamlDeferLoadAttribute => _xamlDeferLoadAttribute;

        public IAssemblySymbol ImmutableAssembly => _immutableAssembly;

        const string _XamlClrNSPrefix = "clr-namespace:";
        const string _XamlClrAssemblyPrefix = "assembly:";
        protected override XamlType GetXamlType(string xamlNamespace, string name, params XamlType[] typeArguments)
        {
            if (_namespaceCaches.TryGetValue((xamlNamespace, name), out var cache))
                return cache;
            if (xamlNamespace.StartsWith(_XamlClrNSPrefix))
            {
                var nsSplits = xamlNamespace.Split(';');
                string assemblyName = null, ns = null;
                for (int i = 0; i < nsSplits.Length; i++)
                {
                    var text = nsSplits[i];
                    if (text.StartsWith(_XamlClrNSPrefix))
                        ns = text.Substring(_XamlClrNSPrefix.Length);
                    else if (text.StartsWith(_XamlClrAssemblyPrefix))
                        assemblyName = text.Substring(_XamlClrAssemblyPrefix.Length);
                }
                if (!string.IsNullOrEmpty(ns))
                {
                    if (string.IsNullOrEmpty(assemblyName))
                        assemblyName = _compilation.Assembly.Name;
                    if (!_assemblies.TryGetValue(assemblyName, out var assembly))
                        return null;
                    var type = assembly.GetTypeByMetadataName(ns + "." + name);
                    if (type != null)
                    {
                        AnalyzerXamlType xamlType = GetXamlType(type);
                        _namespaceCaches.Add((xamlNamespace, name), xamlType);
                        return xamlType;
                    }
                }
            }
            else if (_namespaceMaps.TryGetValue(xamlNamespace, out var map))
            {
                foreach (var item in map)
                {
                    var type = item.Assembly.GetTypeByMetadataName(item.Namespace + "." + name);
                    if (type != null)
                    {
                        AnalyzerXamlType xamlType = GetXamlType(type);
                        _namespaceCaches.Add((xamlNamespace, name), xamlType);
                        return xamlType;
                    }
                }
            }
            return base.GetXamlType(xamlNamespace, name, typeArguments);
        }

        public AnalyzerXamlType GetXamlType(ITypeSymbol type)
        {
            if (_typeCaches.TryGetValue(type, out var typeCache))
                return typeCache;
            AnalyzerXamlType parent;
            if (type.BaseType != null && !SymbolEqualityComparer.Default.Equals(type.BaseType, _objectType))
                parent = GetXamlType(type.BaseType);
            else
                parent = null;
            typeCache = new AnalyzerXamlType(parent, type, this);
            _typeCaches.Add(type, typeCache);
            return typeCache;
        }
    }
}
