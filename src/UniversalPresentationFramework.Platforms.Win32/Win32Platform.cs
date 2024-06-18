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
        private readonly Win32RendererContextType _rendererContextType;
        private readonly Win32InputProvider _inputProvider;

        public Win32Platform()
        {
            if (SkiaRendererD3D12Provider.TryCreate(out var d3d12Provider))
            {
                _rendererProvider = d3d12Provider;
                _rendererContextType = Win32RendererContextType.Direct3D12;
            }
            else
            if (Win32RendererVulkanProvider.TryCreate(out var vulkanProvider))
            {
                _rendererProvider = vulkanProvider;
                _rendererContextType = Win32RendererContextType.Vulkan;
            }
            else
            if (Win32RendererOpenGLProvider.TryCreate(out var openGLProvider))
            {
                _rendererProvider = openGLProvider;
                _rendererContextType = Win32RendererContextType.OpenGL;
            }
            else
            {
                _rendererProvider = new SkiaRendererProvider();
                _rendererContextType = Win32RendererContextType.Software;
            }
            _windowProvider = new WindowProvider(this);
            _themeProvider = new ThemeProvider();
            _inputProvider = new Win32InputProvider();
            _lifecycleProvider = new LifecycleProvider(_windowProvider);
        }

        public WindowProvider WindowProvider => _windowProvider;

        public SkiaRendererProvider RendererProvider => _rendererProvider;

        public ThemeProvider ThemeProvider => _themeProvider;

        public LifecycleProvider LifecycleProvider => _lifecycleProvider;

        public Win32RendererContextType RendererContextType => _rendererContextType;

        public Win32InputProvider InputProvider => _inputProvider;

        public void Dispose()
        {

        }
    }
}
