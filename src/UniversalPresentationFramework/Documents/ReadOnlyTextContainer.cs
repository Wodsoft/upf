using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Controls;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Documents
{
    public class ReadOnlyTextContainer : IReadOnlyTextContainer
    {
        private TextPointer? _documentStart, _documentEnd, _selectionStart, _selectionEnd;
        private TextTreeNode? _root;

        public ReadOnlyTextContainer() { }

        public ReadOnlyTextContainer(TextTreeNode root)
        {
            _root = root;
        }

        public TextTreeNode Root => _root ??= CreateRoot();

        public TextPointer DocumentStart => _documentStart ??= new TextPointer(this, Root, ElementEdge.BeforeStart, LogicalDirection.Forward);

        public TextPointer DocumentEnd => _documentEnd ??= new TextPointer(this, Root, ElementEdge.AfterEnd, LogicalDirection.Backward);

        public virtual TextPointer SelectionStart => _selectionStart ?? DocumentStart;

        public virtual TextPointer SelectionEnd => _selectionEnd ?? DocumentStart;

        public virtual bool IsSelectable => false;

        public virtual bool IsCaretVisible => false;

        public virtual Brush? SelectionBrush => SystemColors.HighlightBrush;

        public virtual Brush? SelectionTextBrush => SystemColors.HighlightTextBrush;

        public virtual int Generation => 0;

        public event RoutedEventHandler? SelectionChanged;

        public event TextChangedEventHandler? TextChanged;

        public void Select(TextPointer start, TextPointer end)
        {
            if (!IsSelectable)
                return;
            if (start.TextContainer != this)
                throw new InvalidOperationException("Start position not belong to this text container.");
            if (end.TextContainer != this)
                throw new InvalidOperationException("End position not belong to this text container.");
            _selectionStart = start;
            _selectionEnd = end;
        }

        protected virtual TextTreeNode CreateRoot() => new TextTreeRootNode(this);
    }
}
