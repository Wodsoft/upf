using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;

namespace Wodsoft.UI.Markup
{
    public class BamlResourceReader : System.Xaml.XamlReader, IXamlLineInfo
    {
        private readonly IReadOnlyList<BamlResourceNode> _nodes;
        private BamlResourceNode _node;
        private int _position;

        public BamlResourceReader(BamlResource resource)
        {
            _nodes = resource.Nodes;
        }

        public override bool IsEof => _position == _nodes.Count;

        public override XamlMember? Member => _node.Member;

        public override NamespaceDeclaration? Namespace => _node.Namespace;

        public override XamlNodeType NodeType => _node.NodeType;

        public override XamlSchemaContext SchemaContext => XamlReader.SchemaContext;

        public override XamlType? Type => _node.Type;

        public override object? Value => _node.Value;

        public override bool Read()
        {
            if (_position == _nodes.Count)
                return false;
            _node = _nodes[_position];
            _position++;
            return true;
        }

        public bool HasLineInfo => _node.HasLineInfo;

        public int LineNumber => _node.LineNumber;

        public int LinePosition => _node.LinePosition;
    }
}
