using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Input
{
    public class InputMethodCompositionEventArgs : EventArgs
    {
        public InputMethodCompositionEventArgs(string text, int caretPosition)
        {
            Text = text;
            CaretPosition = caretPosition;
        }

        public string Text { get; }

        public int CaretPosition { get; }
    }
}
