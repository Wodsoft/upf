using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Documents
{
    public interface ITextContainer : IReadOnlyTextContainer
    {
        void InsertText(TextPointer position, string text);

        void InsertElement(TextPointer pointer, TextElement element);

        void DeleteContent(TextPointer startPosition, TextPointer endPosition);

        bool IsPlainText { get; }

        //void Redo(int count);

        //void Undo(int count);

        //int RedoCount { get; }

        //int UndoCount { get; }
    }
}
