using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media.TextFormatting;

namespace Wodsoft.UI.Controls.Primitives
{
    public interface ITextHost : IInputElement
    {
        char GetChar(int position);
        
        IReadOnlyList<ITextHostLine> Lines { get; }

        bool AcceptsReturn { get; }

        int SelectionStart { get; }

        int SelectionLength { get; }

        int TextLength { get; }

        void Select(int position, int length);

        bool IsSelectable { get; }

        bool IsReadOnly { get; }

        bool IsReadOnlyCaretVisible { get; }

        event TextChangedEventHandler TextChanged;

        event RoutedEventHandler SelectionChanged;
    }
}
