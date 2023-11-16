using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Imaging
{
    internal class ImagingDownloader
    {
        private readonly Uri _uri;
        private readonly ManualResetEvent _event;
        private byte[]? _data;
        private int _downloaded;

        public ImagingDownloader(Uri uri)
        {
            _uri = uri;
            _event = new ManualResetEvent(false);
            IsDownloading = true;
            _ = Process();
        }

        public bool IsDownloading { get; private set; }

        public Exception? Exception { get; private set; }

        public Stream GetStream()
        {
            if (IsDownloading)
                _event.WaitOne();
            if (Exception != null)
                throw Exception;
            return new MemoryStream(_data!, 0, _downloaded, false);
        }

        private async Task Process()
        {
            if (_uri.Scheme.Equals("HTTP", StringComparison.OrdinalIgnoreCase) || _uri.Scheme.Equals("HTTPS", StringComparison.OrdinalIgnoreCase))
            {
                var httpClient = new HttpClient();
                try
                {
                    var response = await httpClient.GetAsync(_uri);
                    var length = response.Content.Headers.ContentLength;
                    var sourceStream = await response.Content.ReadAsStreamAsync();
                    if (length.HasValue)
                    {
                        _data = new byte[length.Value];
                        int read;
                        while (true)
                        {
                            read = sourceStream.Read(_data, _downloaded, _downloaded + 4096 > _data.Length ? _data.Length - _downloaded : 4096);
                            if (read == 0)
                                break;
                            _downloaded += read;
                            DownloadProgress?.Invoke(this, new DownloadProgressEventArgs(_downloaded * 100 / _data.Length));
                            if (_data.Length == _downloaded)
                                break;
                        }
                    }
                    else
                    {
                        var buffer = new byte[4096];
                        MemoryStream destStream = new MemoryStream();
                        int read;
                        while (true)
                        {
                            read = sourceStream.Read(buffer);
                            if (read == 0)
                                break;
                            destStream.Write(buffer, 0, read);
                            _downloaded += read;
                        }
                        _data = destStream.GetBuffer();
                    }
                    sourceStream.Close();
                    sourceStream.Dispose();
                    response.Dispose();
                }
                catch (Exception ex)
                {
                    Exception = ex;
                    DownloadFailed?.Invoke(this, new ExceptionEventArgs(ex));
                    return;
                }
                finally
                {
                    httpClient.Dispose();
                }
                IsDownloading = false;
                _event.Set();
                DownloadCompleted?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                Exception = new NotSupportedException($"Image uri not support scheme \"{_uri.Scheme}\".");
                DownloadFailed?.Invoke(this, new ExceptionEventArgs(Exception));
            }
        }

        public event EventHandler<ExceptionEventArgs>? DownloadFailed;

        public event EventHandler<DownloadProgressEventArgs>? DownloadProgress;

        public event EventHandler? DownloadCompleted;
    }
}
