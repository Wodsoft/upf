using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Providers;

namespace Wodsoft.UI.Platforms.Win32
{
    public class ThemeProvider : IThemeProvider
    {
        public string Name => "Areo";

        public string Color => "NormalColor";

        public event EventHandler? ThemeChanged;
    }
}
