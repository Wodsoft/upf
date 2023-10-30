using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    /// <summary>
    /// OS Window Provider.
    /// </summary>
    public interface IWindowProvider
    {
        IWindowContext CreateContext();
    }
}
