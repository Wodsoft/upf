using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Controls.Primitives;
using Wodsoft.UI.Controls;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Documents
{
    public interface IReadOnlyTextContainer
    {
        TextTreeNode Root { get; }

        TextPointer DocumentStart { get; }

        TextPointer DocumentEnd { get; }

        TextPointer SelectionStart { get; }

        TextPointer SelectionEnd { get; }

        Brush? SelectionBrush { get; }

        Brush? SelectionTextBrush { get; }

        void Select(TextPointer start, TextPointer end);

        bool IsSelectable { get; }

        bool IsCaretVisible { get; }

        int Generation { get; }

        event RoutedEventHandler SelectionChanged;

        event TextChangedEventHandler TextChanged;
    }
}
