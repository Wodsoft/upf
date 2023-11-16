using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Imaging
{
    public abstract class BitmapSource : ImageSource
    {
        public abstract int PixelWidth { get; }

        public abstract int PixelHeight { get; }

        public override float Width => PixelWidth;

        public override float Height => PixelHeight;

        public virtual bool IsDownloading { get; } = false;

        protected abstract bool DelayCreation { get; }

        public event EventHandler? DownloadCompleted;

        public event EventHandler<ExceptionEventArgs>? DecodeFailed;

        public event EventHandler<ExceptionEventArgs>? DownloadFailed;

        public event EventHandler<DownloadProgressEventArgs>? DownloadProgress;

        protected void OnDownloadCompleted()
        {
            DownloadCompleted?.Invoke(this, EventArgs.Empty);
        }

        protected void OnDecodeFailed(Exception exception)
        {
            DecodeFailed?.Invoke(this, new ExceptionEventArgs(exception));
        }

        protected void OnDownloadFailed(Exception exception)
        {
            DownloadFailed?.Invoke(this, new ExceptionEventArgs(exception));
        }

        protected void OnDownloadProgress(int progress)
        {
            if (progress < 0)
                progress = 0;
            if (progress > 100)
                progress = 100;
            DownloadProgress?.Invoke(this, new DownloadProgressEventArgs(progress));
        }

        internal virtual void EnsureCreation()
        {

        }
    }
}
