using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Documents;

namespace Wodsoft.UI.Controls
{
    public partial class TextBlock
    {
        private class TextBlockTextContainer : ReadOnlyTextContainer
        {
            private readonly TextBlock _textBlock;

            public TextBlockTextContainer(TextBlock textBlock)
            {
                _textBlock = textBlock;
            }

            public override bool IsSelectable => _textBlock.IsTextSelectionEnabled;

            protected override TextTreeNode CreateRoot()
            {
                var root = new TextBlockRootNode(this);
                TextBlockBlock blockNode = new TextBlockBlock(_textBlock);
                root.InsertNodeAt(blockNode.TextElementNode, ElementEdge.BeforeEnd);
                return root;
            }
        }

        private class TextBlockRootNode : TextTreeRootNode
        {
            public TextBlockRootNode(IReadOnlyTextContainer textContainer) : base(textContainer)
            {
            }

            public void AddNode(TextTreeTextElementNode node)
            {
                InsertNodeAt(node, ElementEdge.BeforeEnd);
            }
        }
    }
}
