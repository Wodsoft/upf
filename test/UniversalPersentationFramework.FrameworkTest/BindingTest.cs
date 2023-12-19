using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Data;

namespace Wodsoft.UI.Test
{
    public class BindingTest : BaseTest
    {
        [Fact]
        public void SinglePathDependencyPropertyTest()
        {
            var xaml = File.ReadAllText("BindingTest1.xaml");
            var grid = LoadUpfXaml<Grid>(xaml);
            var target = (MyObject)grid.FindName("target")!;
            var source = (MyObject)grid.FindName("source")!;
            grid.UpdateBinding();
            Assert.Equal(source.TextA, target.TextB);
            source.TextA = "new value";
            Assert.Equal(source.TextA, target.TextB);
        }

        [Fact]
        public void SingleListTest()
        {
            var list = new ObservableCollection<string>();
            list.Add("a");
            list.Add("b");
            MyObject obj = new MyObject();
            obj.SetBinding(MyObject.TextAProperty, new Binding { Source = list, Path = new PropertyPath("[1]") });
            Assert.Equal("b", obj.TextA);
            list[1] = "c";
            Assert.Equal("c", obj.TextA);
            list.RemoveAt(1);
            Assert.Null(obj.TextA);
            list.Add("d");
            Assert.Equal("d", obj.TextA);
        }

        [Fact]
        public void SinglePathClrPropertyTest()
        {
            var source = new ClrDataSource();
            source.Text = "a";
            MyObject obj = new MyObject();
            obj.SetBinding(MyObject.TextAProperty, new Binding { Source = source, Path = new PropertyPath("Text") });
            Assert.Equal(source.Text, obj.TextA);
            source.Text = "b";
            Assert.Equal(source.Text, obj.TextA);
            source.Text = null;
            Assert.Equal(source.Text, obj.TextA);
        }

        [Fact]
        public void ComplexPathListTest()
        {
            var list = new ObservableCollection<string>();
            list.Add("a");
            list.Add("b");
            var source = new ClrDataSource();
            source.Content = list;
            MyObject obj = new MyObject();
            obj.SetBinding(MyObject.TextAProperty, new Binding { Source = source, Path = new PropertyPath("Content[1]") });
            Assert.Equal("b", obj.TextA);
            list[1] = "c";
            Assert.Equal("c", obj.TextA);
            list.RemoveAt(1);
            Assert.Null(obj.TextA);
            list.Add("d");
            Assert.Equal("d", obj.TextA);
        }
    }
}
