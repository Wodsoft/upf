using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Input
{
    public interface IInputMethodSource
    {
        UIElement UIScope { get; }

        Vector2 CaretPosition { get; }
    }
}
