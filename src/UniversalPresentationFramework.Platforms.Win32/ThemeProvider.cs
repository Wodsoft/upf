using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using Wodsoft.UI.Providers;


namespace Wodsoft.UI.Platforms.Win32
{
    public partial class ThemeProvider : IThemeProvider
    {
        private readonly object _themeLock = new object();
        private ThemeInfo? _themeInfo;

        public ThemeProvider()
        {
            _themeInfo = GetCurrentTheme();
        }

        public string Name => _themeInfo?.Name ?? "Areo";

        public string Color => _themeInfo?.Color ?? "NormalColor";

        public event EventHandler? ThemeChanged;

        public object? GetResourceValue(SystemResourceKeyID key)
        {
            switch (key)
            {
                case SystemResourceKeyID.Border:
                    return Border;
                default:
                    return null;
            }
        }

        internal void OnThemeChanged()
        {
            lock (_themeLock)
            {
                var themeInfo = GetCurrentTheme();
                if (themeInfo != _themeInfo)
                {
                    _themeInfo = themeInfo;
                    ThemeChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private unsafe ThemeInfo? GetCurrentTheme()
        {
            char[] themeFileName = new char[260];
            char[] themeColor = new char[260];
            char[] themeSize = new char[260];
            fixed (char* c1 = &themeFileName[0])
            {
                fixed (char* c2 = &themeColor[0])
                {
                    fixed (char* c3 = &themeSize[0])
                    {
                        PWSTR themeFileNamePtr = new PWSTR(c1);
                        PWSTR themeColorPtr = new PWSTR(c2);
                        PWSTR themeSizePtr = new PWSTR(c3);
                        var result = PInvoke.GetCurrentThemeName(themeFileNamePtr, 260, themeColorPtr, 260, themeSizePtr, 260);
                        if (result.Succeeded)
                        {
                            return new ThemeInfo(Path.GetFileNameWithoutExtension(themeFileNamePtr.ToString()), themeColorPtr.ToString(), themeSizePtr.ToString());
                        }
                        return null;
                    }
                }
            }
        }

        private record class ThemeInfo
        {
            public ThemeInfo(string name, string color, string size)
            {
                Name = name;
                Color = color;
                Size = size;
            }

            public string Name;

            public string Color;

            public string Size;
        }
    }
}
