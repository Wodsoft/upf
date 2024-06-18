using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Controls
{
    /// <summary>
    /// Specifies the changes applied to TextContainer content.
    /// </summary>
    public class TextChange
    {
        private readonly int _offset;
        private readonly int _addedLength;
        private readonly int _removedLength;

        public TextChange(int offset, int addedLength, int removedLength)
        {
            _offset = offset;
            _addedLength = addedLength;
            _removedLength = removedLength;
        }

        public int Offset => _offset;

        public int AddedLength => _addedLength;

        public int RemovedLength => _removedLength;
    }
}
