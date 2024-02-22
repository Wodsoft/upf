using System;
using System.Collections.Generic;
using System.Text;
using System.Xaml;

namespace UniversalPresentationFramework.SourceGenerators
{
    public record struct ConnectItem
    {
        public string Name;
        public XamlType Type;
        public XamlMember Member;
    }
}
