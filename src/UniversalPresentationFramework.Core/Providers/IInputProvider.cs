using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Input;

namespace Wodsoft.UI.Providers
{
    public interface IInputProvider
    {
        ICursorContext CreateCursorContext(string filename);

        ICursorContext CreateCursorContext(Stream stream);

        void SetCursor(ICursorContext cursorContext);

        void SetCursor(CursorType cursorType);
    }
}
