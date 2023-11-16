using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Imaging
{
    internal static class ImagingCache
    {
        private static ConcurrentDictionary<Uri, ImagingDownloader> _Caches = new ConcurrentDictionary<Uri, ImagingDownloader>();

        public static void RemoveCache(Uri uri)
        {
            _Caches.Remove(uri, out _);
        }

        public static ImagingDownloader GetCache(Uri uri)
        {
            return _Caches.GetOrAdd(uri, value => new ImagingDownloader(uri));
        }
    }
}
