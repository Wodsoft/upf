using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Imaging
{
    public class DownloadProgressEventArgs : EventArgs
    {
        public DownloadProgressEventArgs(int progress)
        {
            Progress = progress;
        }

        public int Progress { get; }
    }
}
