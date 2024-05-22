using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Providers;

namespace Wodsoft.UI.Test
{
    public class TestThemeProvider : IThemeProvider
    {
        public string Name => "Aero";

        public string Color => "NormalColor";

        public event EventHandler? ThemeChanged;

        public object? GetResourceValue(SystemResourceKeyID key)
        {
            return null;
        }
    }
}
