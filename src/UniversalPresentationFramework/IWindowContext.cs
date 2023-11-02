using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public interface IWindowContext : IDisposable
    {
        bool IsClosing { get; }

        void Show();

        void Hide();

        void Close();

        string Title { get; set; }

        int X { get; set; }

        int Y { get; set; }

        int Width { get; set; }

        int Height { get; set; }

        WindowState State { get; set; }

        WindowStartupLocation StartupLocation { get; set; }

        WindowStyle Style { get; set; }

        bool AllowsTransparency { get; set; }

        bool IsInputProcessing { get; }

        public event CancelEventHandler Closing;
        public event WindowContextEventHandler Closed;
        public event WindowContextEventHandler Activated;
        public event WindowContextEventHandler Deactivated;
        public event WindowContextEventHandler LocationChanged;
        public event WindowContextEventHandler StateChanged;
        public event WindowContextEventHandler<DpiScale> DpiChanged;
        public event WindowContextEventHandler<Size> SizeChanged;
    }
}
