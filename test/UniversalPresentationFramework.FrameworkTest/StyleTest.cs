using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Test
{
    public class StyleTest : BaseTest
    {
        [Fact]
        public void DirectTest()
        {
            Style style = new Style(typeof(MyObject));
            style.Setters.Add(new Setter(MyObject.TextAProperty, "test"));
            MyObject obj = new MyObject();
            Assert.Null(obj.TextA);
            obj.Style = style;
            Assert.Equal("test", obj.TextA);
        }

        [Fact]
        public void XamlDirectTest()
        {
            var xaml = File.ReadAllText("StyleDirectTest.xaml");
            var obj = LoadUpfXaml<MyObject>(xaml);
            Assert.Equal("test", obj.TextA);
        }

        [Fact]
        public void XamlResourcesTest()
        {
            var xaml = File.ReadAllText("StyleResourcesTest.xaml");
            var grid = LoadUpfXaml<Grid>(xaml);
            var obj = (MyObject)grid.FindName("target")!;
            Assert.Equal("test", obj.TextA);
        }

        [Fact]
        public void EventTest()
        {
            int clickCount = 0;
            Style style = new Style(typeof(EventObject));
            style.Setters.Add(new EventSetter(EventObject.ClickEvent, new RoutedEventHandler((sender, e) => clickCount++)));
            EventObject obj = new EventObject();
            obj.Style = style;
            obj.RaiseClick();
            Assert.Equal(1, clickCount);
        }

        [Fact]
        public void XamlEventTest()
        {
            var xaml = File.ReadAllText("StyleEventTest.xaml");
            var root = new EventRoot();
            LoadUpfXaml(root, xaml);
            var obj = (EventObject)root.FindName("eventObj")!;
            obj.RaiseClick();
            Assert.Equal(1, root.ClickCount);
        }
    }
}
