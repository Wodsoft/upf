using Microsoft.CodeAnalysis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xaml;
using System.Xaml.Markup;
using System.Xaml.Schema;

namespace UniversalPresentationFramework.SourceGenerators
{
    public class AnalyzerXamlType : XamlType
    {
        private readonly AnalyzerXamlType _baseType;
        private readonly ITypeSymbol _type;
        private readonly AnalyzerXamlSchemaContext _schemaContext;

        public AnalyzerXamlType(AnalyzerXamlType baseType, ITypeSymbol type, AnalyzerXamlSchemaContext schemaContext) : base(type.Name, null, schemaContext)
        {
            _baseType = baseType;
            _type = type;
            _schemaContext = schemaContext;
        }

        public ITypeSymbol Type => _type;

        protected override XamlMember LookupAttachableMember(string name)
        {
            var getMethod = _type.GetMembers("Get" + name).FirstOrDefault(t => t.IsStatic && t.Kind == SymbolKind.Method);
            var canSet = _type.GetMembers("Set" + name).FirstOrDefault(t => t.IsStatic && t.Kind == SymbolKind.Method) != null;
            if (getMethod != null || canSet)
                return new AnalyzerXamlMember(name, this, _schemaContext.GetXamlType(((IMethodSymbol)getMethod).ReturnType), getMethod != null, canSet);
            else
                return null;
        }

        protected override XamlMember LookupMember(string name, bool skipReadOnlyCheck)
        {
            var member = _type.GetMembers(name).FirstOrDefault(t => !t.IsStatic && t.Kind == SymbolKind.Property || t.Kind == SymbolKind.Event);
            if (member is IPropertySymbol property)
                return new AnalyzerXamlMember(property, this, _schemaContext.GetXamlType(property.Type));
            else if (member is IEventSymbol eventSymbol)
                return new AnalyzerXamlMember(eventSymbol, this, _schemaContext.GetXamlType(eventSymbol.Type));
            return _baseType?.GetMember(name);
        }

        protected override XamlMember LookupContentProperty()
        {
            if (_schemaContext.ContentPropertyAttribute == null)
                return null;
            var contentAttr = _type.GetAttributes().FirstOrDefault(t => SymbolEqualityComparer.Default.Equals(t.AttributeClass, _schemaContext.ContentPropertyAttribute));
            if (contentAttr == null)
                return _baseType?.ContentProperty;
            if (contentAttr.ConstructorArguments.Length != 1)
                return null;
            var propertyName = contentAttr.ConstructorArguments[0].Value as string;
            return GetMember(propertyName);
        }

        public override bool CanAssignTo(XamlType xamlType)
        {
            if (xamlType == this)
                return true;
            if (xamlType is AnalyzerXamlType analyzerXamlType)
            {
                if (SymbolEqualityComparer.Default.Equals(_type, analyzerXamlType.Type))
                    return true;
                return IsInherit(_type, analyzerXamlType.Type);
            }
            return false;
        }

        private static bool IsInherit(ITypeSymbol sourceSymbol, ITypeSymbol targetSymbol)
        {
            if (targetSymbol.TypeKind == TypeKind.Interface)
            {
                foreach (var interfaceSymbol in sourceSymbol.AllInterfaces)
                {
                    if (SymbolEqualityComparer.Default.Equals(interfaceSymbol, targetSymbol))
                        return true;
                }
            }

            var baseType = sourceSymbol.BaseType;
            while (baseType != null)
            {
                if (SymbolEqualityComparer.Default.Equals(baseType, targetSymbol))
                    return true;
                baseType = baseType.BaseType;
            }
            return false;
        }

        protected override XamlCollectionKind LookupCollectionKind()
        {
            if (_type.TypeKind == TypeKind.Array)
                return XamlCollectionKind.Array;
            else
            {
                var isImmutable = SymbolEqualityComparer.Default.Equals(_type.ContainingAssembly, _schemaContext.ImmutableAssembly);
                foreach (var interfaceSymbol in _type.AllInterfaces)
                {
                    if (SymbolEqualityComparer.Default.Equals(interfaceSymbol, _schemaContext.DictionaryInterface) ||
                        (SymbolEqualityComparer.Default.Equals(interfaceSymbol.ContainingAssembly, _schemaContext.DictionaryInterface.ContainingAssembly) && interfaceSymbol.Name == "IDictionary"))
                        return XamlCollectionKind.Dictionary;
                    else if (SymbolEqualityComparer.Default.Equals(interfaceSymbol, _schemaContext.ListInterface) ||
                        (SymbolEqualityComparer.Default.Equals(interfaceSymbol.ContainingAssembly, _schemaContext.DictionaryInterface.ContainingAssembly) && interfaceSymbol.Name == "ICollection"))
                        return XamlCollectionKind.Collection;
                    else if (isImmutable && SymbolEqualityComparer.Default.Equals(interfaceSymbol, _schemaContext.EnumerableInterface))
                        return XamlCollectionKind.Collection;
                }
            }
            return XamlCollectionKind.None;
        }

        protected override XamlValueConverter<XamlDeferringLoader> LookupDeferringLoader()
        {
            if (_type.GetAttributes().Any(t => SymbolEqualityComparer.Default.Equals(t.AttributeClass, _schemaContext.XamlDeferLoadAttribute)))
                return new XamlValueConverter<XamlDeferringLoader>(typeof(object), null);
            return null;
        }

        public new AnalyzerXamlSchemaContext SchemaContext => _schemaContext;

        public override string ToString()
        {
            return _type.ToDisplayString();
        }
    }
}
