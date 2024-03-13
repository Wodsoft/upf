using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Renderers;

namespace Wodsoft.UI.Platforms.Win32
{
    public class Win32Platform : IDisposable
    {
        private readonly WindowProvider _windowProvider;
        private readonly SkiaRendererProvider _rendererProvider;
        private readonly ThemeProvider _themeProvider;
        private readonly LifecycleProvider _lifecycleProvider;

        public Win32Platform()
        {
            _windowProvider = new WindowProvider(this);
            _rendererProvider = new SkiaRendererProvider();
            _themeProvider = new ThemeProvider();
            _lifecycleProvider = new LifecycleProvider(_windowProvider);
        }

        public WindowProvider WindowProvider => _windowProvider;

        public SkiaRendererProvider RendererProvider => _rendererProvider;

        public ThemeProvider ThemeProvider => _themeProvider;

        public LifecycleProvider LifecycleProvider => _lifecycleProvider;

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
