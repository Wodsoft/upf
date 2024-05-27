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
        IReadOnlyList<ITextHostLine> Lines { get; }

        bool AcceptsReturn { get; }

        int SelectionStart { get; }

        int SelectionLength { get; }

        void Select(int position, int length);

        bool IsSelectable { get; }

        bool IsReadOnly { get; }

        bool IsFocused { get; }

        event EventHandler TextChanged;

        event EventHandler SelectionChanged;
    }
}
