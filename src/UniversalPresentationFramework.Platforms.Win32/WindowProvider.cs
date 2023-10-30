using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Platforms.Win32
{
    public class WindowProvider : IWindowProvider
    {
        public IWindowContext CreateContext()
        {
            return new WindowContext();
        }
    }
}
