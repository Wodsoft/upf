using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Documents
{
    public class TextContainer : ReadOnlyTextContainer, ITextContainer
    {
        private readonly bool _isPlainText;
        private int _generation;

        public TextContainer(bool isPlainText)
        {
            _isPlainText = isPlainText;
        }

        public override int Generation => _generation;

        public bool IsPlainText => _isPlainText;

        #region Modify

        public void BeginChange() => BeginChange(true);

        public void BeginChange(bool undo)
        {

        }

        public void EndChange()
        {
            _generation++;
        }

        public void InsertElement(TextPointer pointer, TextElement element)
        {
            throw new NotImplementedException();
        }

        public void InsertText(TextPointer position, string text)
        {
            throw new NotImplementedException();
        }

        public void DeleteContent(TextPointer startPosition, TextPointer endPosition)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
