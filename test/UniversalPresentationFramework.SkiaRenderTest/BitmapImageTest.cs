using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media.Imaging;

namespace Wodsoft.UI.Test
{
    public class BitmapImageTest : RenderTest
    {
        [Fact]
        public void DownloadTest()
        {
            ManualResetEvent resetEvent = new ManualResetEvent(false);
            bool completed = false, failed = false;
            BitmapImage image = new BitmapImage();
            image.DownloadCompleted += delegate
            {
                completed = true;
                resetEvent.Set();
            };
            image.DownloadFailed += delegate
            {
                failed = true;
                resetEvent.Set();
            };
            image.BeginInit();
            image.UriSource = new Uri("https://www.baidu.com/img/flexible/logo/pc/result.png");
            image.EndInit();
            resetEvent.WaitOne();
            Assert.True(completed);
            Assert.False(failed);
        }
    }
}
