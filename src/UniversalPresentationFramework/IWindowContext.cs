using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public interface IWindowContext
    {
        bool IsClosing { get; }

        void Show();

        void Hide();

        string Title { get; set; }

        int X { get; set; }

        int Y { get; set; }

        int Width { get; set; }

        int Height { get; set; }

        WindowState State { get; set; }

        WindowStartupLocation StartupLocation { get; set; }

        WindowStyle Style { get; set; }

        bool AllowsTransparency { get; set; }
    }
}
