using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Markup
{
    public interface IAddChild
    {
        void AddChild(object value);

        void AddText(string text);
    }
}
