using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Imaging
{
    public sealed class BitmapImage : BitmapSource, ISupportInitialize
    {
        private bool _isInInit, _isInitialized, _delayCreation, _isDownloading;
        private IImageContext? _context;
        private ImagingDownloader? _downloader;
        private ManualResetEvent _resetEvent = new ManualResetEvent(false);

        public override int PixelWidth
        {
            get
            {
                CheckState();
                EnsureCreation();
                return _context!.Width;
            }
        }

        public override int PixelHeight
        {
            get
            {
                CheckState();
                EnsureCreation();
                return _context!.Height;
            }
        }

        private void CheckState()
        {
            if (_isInInit)
                throw new InvalidOperationException("Image is initializing.");
            if (!_isInitialized)
                throw new InvalidOperationException("Image not initialized.");
        }

        public void BeginInit()
        {
            _resetEvent.Reset();
            _isInInit = true;
        }

        public void EndInit()
        {
            _isInInit = false;
            _isInitialized = true;
            if (UriSource == null && StreamSource == null)
                throw new InvalidOperationException("Bitmap image must have a UriSource or StreamSource value.");
            var uriSource = UriSource;
            if (uriSource != null && CreateOptions.HasFlag(BitmapCreateOptions.DelayCreation))
                _delayCreation = true;
            if (!_delayCreation)
                FinalizeCreation();
        }

        internal override void EnsureCreation()
        {
            _resetEvent.WaitOne();
            if (_context == null)
                throw new InvalidOperationException("Image load failed.");
        }

        private void FinalizeCreation()
        {
            if (FrameworkProvider.RendererProvider == null)
                throw new InvalidOperationException("Framework not initialized.");
            if (_uriSource != null)
            {
                Uri? uri = _uriSource;
                if (_baseUri != null)
                    uri = new Uri(_baseUri, uri);
                if (_createOptions.HasFlag(BitmapCreateOptions.IgnoreImageCache))
                    ImagingCache.RemoveCache(uri);
                var downloader = ImagingCache.GetCache(uri);
                if (downloader.IsDownloading)
                {
                    _isDownloading = true;
                    _downloader = downloader;
                    downloader.DownloadProgress += Downloader_DownloadProgress;
                    downloader.DownloadFailed += Downloader_DownloadFailed;
                    downloader.DownloadCompleted += Downloader_DownloadCompleted;
                    if (!downloader.IsDownloading)
                        Downloader_DownloadCompleted(downloader, EventArgs.Empty);
                }
                else
                {
                    _downloader = downloader;
                    HandleDownloadCompleted();
                }
            }
            else if (_streamSource != null)
            {
                var stream = _streamSource;
                if (!stream.CanSeek)
                {
                    try
                    {
                        stream = new MemoryStream();
                        _streamSource.CopyTo(stream);
                        stream.Position = 0;
                    }
                    catch (Exception ex)
                    {
                        OnDownloadFailed(ex);
                        _resetEvent.Set();
                        return;
                    }
                }
                try
                {
                    _context = FrameworkProvider.RendererProvider.CreateImageContext(stream, DecodePixelWidth, DecodePixelHeight, Rotation);
                }
                catch (Exception ex)
                {
                    OnDecodeFailed(ex);
                }
                finally
                {
                    _resetEvent.Set();
                }
            }
        }

        private void HandleDownloadCompleted()
        {
            if (_downloader!.Exception == null)
            {
                try
                {
                    _context = FrameworkProvider.RendererProvider!.CreateImageContext(_downloader.GetStream(), DecodePixelWidth, DecodePixelHeight, Rotation);
                    OnDownloadCompleted();
                }
                catch (Exception ex)
                {
                    OnDecodeFailed(ex);
                }
                finally
                {
                    _resetEvent.Set();
                }
            }
            else
            {
                OnDownloadFailed(_downloader.Exception);
                _resetEvent.Set();
            }
        }

        private void Downloader_DownloadCompleted(object? sender, EventArgs e)
        {
            lock (_downloader!)
            {
                if (!_isDownloading)
                    return;
                _isDownloading = false;
                _downloader!.DownloadProgress -= Downloader_DownloadProgress;
                _downloader!.DownloadFailed -= Downloader_DownloadFailed;
                _downloader!.DownloadCompleted -= Downloader_DownloadCompleted;
            }
            HandleDownloadCompleted();
        }

        private void Downloader_DownloadFailed(object? sender, ExceptionEventArgs e)
        {
            _downloader!.DownloadProgress -= Downloader_DownloadProgress;
            _downloader!.DownloadFailed -= Downloader_DownloadFailed;
            _downloader!.DownloadCompleted -= Downloader_DownloadCompleted;
            _isDownloading = false;
        }

        private void Downloader_DownloadProgress(object? sender, DownloadProgressEventArgs e)
        {
            OnDownloadProgress(e.Progress);
        }

        #region Properties

        private RequestCachePolicy? _uriCachePolicy;
        public static readonly DependencyProperty UriCachePolicyProperty =
                  DependencyProperty.Register("UriCachePolicy",
                                   typeof(RequestCachePolicy),
                                   typeof(BitmapImage),
                                   new PropertyMetadata(null, UriCachePolicyPropertyChanged, CoerceUriCachePolicy));
        private static void UriCachePolicyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BitmapImage image = (BitmapImage)d;
            image._uriCachePolicy = (RequestCachePolicy?)e.NewValue;
        }
        private static object? CoerceUriCachePolicy(DependencyObject d, object? baseValue)
        {
            BitmapImage image = (BitmapImage)d;
            if (image._isInInit)
                return baseValue;
            return image._uriCachePolicy;
        }
        public RequestCachePolicy? UriCachePolicy { get { return (RequestCachePolicy?)GetValue(UriCachePolicyProperty); } set { SetValue(UriCachePolicyProperty, value); } }

        private Uri? _uriSource;
        public static readonly DependencyProperty UriSourceProperty =
                  DependencyProperty.Register("UriSource",
                                   typeof(Uri),
                                   typeof(BitmapImage),
                                   new PropertyMetadata(null, UriSourcePropertyChanged, CoerceUriSource));
        private static void UriSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BitmapImage image = (BitmapImage)d;
            image._uriSource = (Uri?)e.NewValue;
        }
        private static object? CoerceUriSource(DependencyObject d, object? baseValue)
        {
            BitmapImage image = (BitmapImage)d;
            if (image._isInInit)
                return baseValue;
            return image._uriSource;
        }
        public Uri? UriSource { get { return (Uri?)GetValue(UriSourceProperty); } set { SetValue(UriSourceProperty, value); } }

        private Stream? _streamSource;
        public static readonly DependencyProperty StreamSourceProperty =
                  DependencyProperty.Register("StreamSource",
                                   typeof(Stream),
                                   typeof(BitmapImage),
                                   new PropertyMetadata(null, StreamSourcePropertyChanged, CoerceStreamSource));
        private static void StreamSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BitmapImage image = (BitmapImage)d;
            image._streamSource = (Stream?)e.NewValue;
        }
        private static object? CoerceStreamSource(DependencyObject d, object? baseValue)
        {
            BitmapImage image = (BitmapImage)d;
            if (image._isInInit)
                return baseValue;
            return image._streamSource;
        }
        public Stream? StreamSource { get { return (Stream?)GetValue(StreamSourceProperty); } set { SetValue(StreamSourceProperty, value); } }

        private int _decodePixelWidth;
        public static readonly DependencyProperty DecodePixelWidthProperty =
                  DependencyProperty.Register("DecodePixelWidth",
                                   typeof(int),
                                   typeof(BitmapImage),
                                   new PropertyMetadata(0, DecodePixelWidthPropertyChanged, CoerceDecodePixelWidth));
        private static void DecodePixelWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BitmapImage image = (BitmapImage)d;
            image._decodePixelWidth = (int)e.NewValue!;
        }
        private static object? CoerceDecodePixelWidth(DependencyObject d, object? baseValue)
        {
            BitmapImage image = (BitmapImage)d;
            if (image._isInInit)
                return baseValue;
            return image._decodePixelWidth;
        }
        public int DecodePixelWidth { get { return (int)GetValue(DecodePixelWidthProperty)!; } set { SetValue(DecodePixelWidthProperty, value); } }

        private int _decodePixelHeight;
        public static readonly DependencyProperty DecodePixelHeightProperty =
                  DependencyProperty.Register("DecodePixelHeight",
                                   typeof(int),
                                   typeof(BitmapImage),
                                   new PropertyMetadata(0, DecodePixelHeightPropertyChanged, CoerceDecodePixelHeight));
        private static void DecodePixelHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BitmapImage image = (BitmapImage)d;
            image._decodePixelHeight = (int)e.NewValue!;
        }
        private static object? CoerceDecodePixelHeight(DependencyObject d, object? baseValue)
        {
            BitmapImage image = (BitmapImage)d;
            if (image._isInInit)
                return baseValue;
            return image._decodePixelHeight;
        }
        public int DecodePixelHeight { get { return (int)GetValue(DecodePixelHeightProperty)!; } set { SetValue(DecodePixelHeightProperty, value); } }

        private Rotation _rotation;
        public static readonly DependencyProperty RotationProperty =
                  DependencyProperty.Register("Rotation",
                                   typeof(Rotation),
                                   typeof(BitmapImage),
                                   new PropertyMetadata(Rotation.Rotate0, RotationPropertyChanged, CoerceRotation));
        private static void RotationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BitmapImage image = (BitmapImage)d;
            image._rotation = (Rotation)e.NewValue!;
        }
        private static object? CoerceRotation(DependencyObject d, object? baseValue)
        {
            BitmapImage image = (BitmapImage)d;
            if (image._isInInit)
                return baseValue;
            return image._rotation;
        }
        public Rotation Rotation { get { return (Rotation)GetValue(RotationProperty)!; } set { SetValue(RotationProperty, value); } }

        private Int32Rect _sourceRect;
        public static readonly DependencyProperty SourceRectProperty =
                  DependencyProperty.Register("SourceRect",
                                   typeof(Int32Rect),
                                   typeof(BitmapImage),
                                   new PropertyMetadata(Int32Rect.Empty, SourceRectPropertyChanged, CoerceSourceRect));
        private static void SourceRectPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BitmapImage image = (BitmapImage)d;
            image._sourceRect = (Int32Rect)e.NewValue!;
        }
        private static object? CoerceSourceRect(DependencyObject d, object? baseValue)
        {
            BitmapImage image = (BitmapImage)d;
            if (image._isInInit)
                return baseValue;
            return image._sourceRect;
        }
        public Int32Rect SourceRect { get { return (Int32Rect)GetValue(SourceRectProperty)!; } set { SetValue(SourceRectProperty, value); } }

        private BitmapCreateOptions _createOptions;
        public static readonly DependencyProperty CreateOptionsProperty =
                  DependencyProperty.Register("CreateOptions",
                                   typeof(BitmapCreateOptions),
                                   typeof(BitmapImage),
                                   new PropertyMetadata(BitmapCreateOptions.None, CreateOptionsPropertyChanged, CoerceCreateOptions));
        private static void CreateOptionsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BitmapImage image = (BitmapImage)d;
            image._createOptions = (BitmapCreateOptions)e.NewValue!;
        }
        private static object? CoerceCreateOptions(DependencyObject d, object? baseValue)
        {
            BitmapImage image = (BitmapImage)d;
            if (image._isInInit)
                return baseValue;
            return image._createOptions;
        }
        public BitmapCreateOptions CreateOptions { get { return (BitmapCreateOptions)GetValue(CreateOptionsProperty)!; } set { SetValue(CreateOptionsProperty, value); } }

        private BitmapCacheOption _cacheOption;
        public static readonly DependencyProperty CacheOptionProperty =
                  DependencyProperty.Register("CacheOption",
                                   typeof(BitmapCacheOption),
                                   typeof(BitmapImage),
                                   new PropertyMetadata(BitmapCacheOption.Default, CacheOptionPropertyChanged, CoerceCacheOption));
        private static void CacheOptionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BitmapImage image = (BitmapImage)d;
            image._cacheOption = (BitmapCacheOption)e.NewValue!;
        }
        private static object? CoerceCacheOption(DependencyObject d, object? baseValue)
        {
            BitmapImage image = (BitmapImage)d;
            if (image._isInInit)
                return baseValue;
            return image._cacheOption;
        }
        public BitmapCacheOption CacheOption { get { return (BitmapCacheOption)GetValue(CacheOptionProperty)!; } set { SetValue(CacheOptionProperty, value); } }

        private Uri? _baseUri;
        public Uri? BaseUri
        {
            get
            {
                return _baseUri;
            }
            set
            {
                if (!_isInitialized && _baseUri != value)
                {
                    _baseUri = value;
                }
            }
        }

        public override IImageContext Context
        {
            get
            {
                CheckState();
                EnsureCreation();
                return _context!;
            }
        }

        protected override bool DelayCreation => _delayCreation;

        public override bool IsDownloading => _isDownloading;

        public override PixelFormat Format
        {
            get
            {
                CheckState();
                EnsureCreation();
                return _context!.PixelFormat;
            }
        }

        #endregion
    }
}
