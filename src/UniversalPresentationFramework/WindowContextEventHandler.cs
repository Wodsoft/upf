using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public delegate void WindowContextEventHandler(IWindowContext context);
    public delegate void WindowContextEventHandler<T>(IWindowContext context, T e);
}
