using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;

namespace Wodsoft.UI.Markup
{
    public readonly record struct BamlResourceNode
    {
        public BamlResourceNode(XamlNodeType nodeType, object? value, XamlType? type, XamlMember? member, NamespaceDeclaration? ns)
        {
            NodeType = nodeType;
            Value = value;
            Type = type;
            Member = member;
            Namespace = ns;
        }

        public BamlResourceNode(XamlNodeType nodeType, object? value, XamlType? type, XamlMember? member, NamespaceDeclaration? ns, int lineNumber, int linePosition)
        {
            NodeType = nodeType;
            Value = value;
            Type = type;
            Member = member;
            Namespace = ns;
            HasLineInfo = true;
            LineNumber = lineNumber;
            LinePosition = linePosition;
        }

        public readonly XamlNodeType NodeType;

        public readonly object? Value;

        public readonly XamlType? Type;

        public readonly XamlMember? Member;

        public readonly NamespaceDeclaration? Namespace;

        public readonly bool HasLineInfo;

        public readonly int LineNumber;

        public readonly int LinePosition;
    }
}
