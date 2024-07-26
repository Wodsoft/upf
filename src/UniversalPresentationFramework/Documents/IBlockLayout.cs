using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Documents
{
    public interface IBlockLayout
    {
        void Measure(Size availableSize);

        float Width { get; }

        float Height { get; }

        void Draw(DrawingContext drawingContext, in Point origin, in Rect clip);

        Rect GetRectAtCharacter(TextPointer pointer);

        TextPointer GetCharacterAtPoint(in Point point);

        bool GetCharacterRelateToCharacter(TextPointer pointer, in LogicalDirection direction, [NotNullWhen(true)] out TextPointer? position);
    }
}
