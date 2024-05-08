using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Test
{
    public class EventTest
    {
        [Fact]
        public void BubbleTest1()
        {
            int i = 0;
            EventObject obj1 = new EventObject();
            obj1.Click += (sender, e) =>
            {
                i++;
                Assert.Equal(3, i);
            };
            EventObject obj2 = new EventObject();
            obj2.Click += (sender, e) =>
            {
                i++;
                Assert.Equal(2, i);
            };
            EventObject obj3 = new EventObject();
            obj3.Click += (sender, e) =>
            {
                i++;
                Assert.Equal(1, i);
            };
            obj1.Content = obj2;
            obj2.Content = obj3;
            obj3.RaiseClick();
            Assert.Equal(3, i);
        }

        [Fact]
        public void BubbleTest2()
        {
            int i = 0;
            EventObject obj1 = new EventObject();
            obj1.Click += (sender, e) =>
            {
                i++;
                Assert.Equal(2, i);
            };
            EventObject obj2 = new EventObject();
            obj2.Click += (sender, e) =>
            {
                i++;
                Assert.Equal(1, i);
            };
            EventObject obj3 = new EventObject();
            obj3.Click += (sender, e) =>
            {
                throw new Exception();
            };
            obj1.Content = obj2;
            obj2.Content = obj3;
            obj2.RaiseClick();
            Assert.Equal(2, i);
        }

        [Fact]
        public void TunnelTest1()
        {
            int i = 0;
            EventObject obj1 = new EventObject();
            obj1.PreClick += (sender, e) =>
            {
                i++;
                Assert.Equal(1, i);
            };
            EventObject obj2 = new EventObject();
            obj2.PreClick += (sender, e) =>
            {
                i++;
                Assert.Equal(2, i);
            };
            EventObject obj3 = new EventObject();
            obj3.PreClick += (sender, e) =>
            {
                i++;
                Assert.Equal(3, i);
            };
            obj1.Content = obj2;
            obj2.Content = obj3;
            obj3.RaisePreClick();
            Assert.Equal(3, i);
        }

        [Fact]
        public void TunnelTest2()
        {
            int i = 0;
            EventObject obj1 = new EventObject();
            obj1.PreClick += (sender, e) =>
            {
                i++;
                Assert.Equal(1, i);
            };
            EventObject obj2 = new EventObject();
            obj2.PreClick += (sender, e) =>
            {
                i++;
                Assert.Equal(2, i);
            };
            EventObject obj3 = new EventObject();
            obj3.PreClick += (sender, e) =>
            {
                throw new Exception();
            };
            obj1.Content = obj2;
            obj2.Content = obj3;
            obj2.RaisePreClick();
            Assert.Equal(2, i);
        }

        [Fact]
        public void DirectTest()
        {
            int i = 0;
            EventObject obj1 = new EventObject();
            obj1.Direct += (sender, e) =>
            {
                throw new Exception();
            };
            EventObject obj2 = new EventObject();
            obj2.Direct += (sender, e) =>
            {
                i++;
                Assert.Equal(1, i);
            };
            EventObject obj3 = new EventObject();
            obj3.Direct += (sender, e) =>
            {
                throw new Exception();
            };
            obj1.Content = obj2;
            obj2.Content = obj3;
            obj2.RaiseDirect();
            Assert.Equal(1, i);
        }
    }
}
