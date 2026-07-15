using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Threading;

namespace Wodsoft.UI.Test
{
    public class DispatcherTimerTest
    {
        [Fact]
        public void EmptyDispatcherTimerTest()
        {
            int timer1Count = 0, timer2Count = 0, timer3Count = 0;
            var timer1 = new DispatcherTimer();
            timer1.Interval = TimeSpan.FromMilliseconds(100);
            timer1.Tick += (sender, e) =>
            {
                timer1Count++;
            };
            var timer2 = new DispatcherTimer();
            timer2.Interval = TimeSpan.FromMilliseconds(300);
            timer2.Tick += (sender, e) =>
            {
                timer2Count++;
            };
            var timer3 = new DispatcherTimer();
            timer3.Interval = TimeSpan.FromMilliseconds(500);
            timer3.Tick += (sender, e) =>
            {
                timer3Count++;
            };
            timer1.Start();
            timer2.Start();
            timer3.Start();
            Thread.Sleep(5000);
            timer1.Stop();
            timer2.Stop();
            timer3.Stop();
            Assert.True(timer1Count > 5000 / 125);
            Assert.Equal(timer1Count / 3, timer2Count);
            Assert.Equal(timer1Count / 5, timer3Count);
        }
    }
}
