using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Input
{
    public interface IInputMethodContext
    {
        //void HandleKeyboard(KeyEventArgs e);

        void Focus();

        void Unfocus();
    }
}
