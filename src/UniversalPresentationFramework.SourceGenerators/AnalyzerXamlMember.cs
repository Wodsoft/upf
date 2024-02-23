using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xaml;
using System.Xaml.Markup;
using System.Xaml.Schema;

namespace UniversalPresentationFramework.SourceGenerators
{
    public class AnalyzerXamlMember : XamlMember
    {
        private readonly AnalyzerXamlType _memberType;
        private readonly bool _isReadOnly, _isWriteOnly, _isEvent;
        private readonly bool _hasDefferloader;

        public AnalyzerXamlMember(IPropertySymbol property, AnalyzerXamlType declaringType, AnalyzerXamlType memberType) : base(property.Name, declaringType, false)
        {
            _memberType = memberType;
            _isReadOnly = property.IsReadOnly;
            _isWriteOnly = property.IsWriteOnly;
            _hasDefferloader = property.GetAttributes().Any(t => SymbolEqualityComparer.Default.Equals(t.AttributeClass, memberType.SchemaContext.XamlDeferLoadAttribute));
        }

        public AnalyzerXamlMember(IEventSymbol eventSymbol, AnalyzerXamlType declaringType, AnalyzerXamlType memberType) : base(eventSymbol.Name, declaringType, false)
        {
            _memberType = memberType;
            _isEvent = true;
        }

        public AnalyzerXamlMember(string name, AnalyzerXamlType declaringType, AnalyzerXamlType memberType, bool canGet, bool canSet)
            : base(name, declaringType, true)
        {
            _memberType = memberType;
            _isReadOnly = canGet && !canSet;
            _isWriteOnly = canSet && !canGet;
        }

        protected override XamlType LookupType()
        {
            return _memberType;
        }

        protected override bool LookupIsReadOnly()
        {
            return _isReadOnly;
        }

        protected override bool LookupIsWriteOnly()
        {
            return _isWriteOnly;
        }

        protected override bool LookupIsEvent()
        {
            return _isEvent;
        }

        protected override XamlValueConverter<XamlDeferringLoader> LookupDeferringLoader()
        {
            if (_isEvent)
                return null;
            if (_hasDefferloader)
                return new XamlValueConverter<XamlDeferringLoader>(typeof(object), null);
            return _memberType?.DeferringLoader;
        }

        protected override bool LookupIsUnknown()
        {
            return false;
        }

        public override string ToString()
        {
            return DeclaringType.ToString() + "." + Name;
        }        
    }
}
