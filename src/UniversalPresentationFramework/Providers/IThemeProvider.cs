using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Providers
{
    public interface IThemeProvider
    {
        string Name { get; }

        string Color { get; }

        event EventHandler? ThemeChanged;
    }
}
