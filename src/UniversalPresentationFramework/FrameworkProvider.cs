using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Providers;

namespace Wodsoft.UI
{
    internal static class FrameworkProvider
    {
        internal static IResourceProvider? ResourceProvider;

        internal static IThemeProvider? ThemeProvider;

        internal static IResourceProvider GetResourceProvider()
        {
            if (ResourceProvider == null)
                throw new InvalidOperationException("Framework not initialized.");
            return ResourceProvider;
        }

        internal static IThemeProvider GetThemeProvider()
        {
            if (ThemeProvider == null)
                throw new InvalidOperationException("Framework not initialized.");
            return ThemeProvider;
        }
    }
}
