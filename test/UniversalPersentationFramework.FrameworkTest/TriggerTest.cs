using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Controls;
using Wodsoft.UI.Shapes;

namespace Wodsoft.UI.Test
{
    public class TriggerTest : BaseTest
    {
        [Fact]
        public void TriggerStyleTest()
        {
            var xaml = File.ReadAllText("TriggerStyleTest.xaml");
            var obj = LoadUpfXaml<MyObject>(xaml);
            Assert.Null(obj.TextC);
            Assert.Null(obj.TextD);
            obj.TextA = "text";
            Assert.Equal("trigger text", obj.TextC);
            Assert.Null(obj.TextD);
            obj.TextB = "text2";
            Assert.Equal("trigger text", obj.TextC);
            Assert.Equal("D text", obj.TextD);
        }

        [Fact]
        public void DataTriggerStyleTest()
        {
            var xaml = File.ReadAllText("TriggerStyleDataTest.xaml");
            var grid = LoadUpfXaml<Grid>(xaml);
            var target = (MyObject)grid.FindName("target")!;
            Assert.Null(target.TextC);
            Assert.Null(target.TextD);
            var dataSource = new DependencyDataSource();
            dataSource.Text = "text";
            grid.DataContext = dataSource;
            grid.UpdateBinding();
            Assert.Equal("trigger text", target.TextC);
            Assert.Null(target.TextD);
            dataSource.Text = "text2";
            dataSource.Integer = 100;
            Assert.Null(target.TextC);
            Assert.Equal("D text", target.TextD);
        }

        [Fact]
        public void TemplateTest()
        {
            var xaml = File.ReadAllText("TriggerTemplateTest.xaml");
            var grid = LoadUpfXaml<Grid>(xaml);
            grid.Arrange(new Rect(0, 0, 100, 100));
            var obj = (MyObject)grid.FindName("target")!;
            var rect = (Rectangle)obj.FindTemplateChild("rect")!;
            Assert.Null(obj.TextC);
            Assert.Null(obj.TextD);
            obj.TextA = "text";
            Assert.Equal("trigger text", obj.TextC);
            Assert.Equal(50d, rect.Width);
            Assert.Null(obj.TextD);
            obj.TextB = "text2";
            Assert.Equal("trigger text", obj.TextC);
            Assert.Equal("D text", obj.TextD);
            Assert.Equal(150d, rect.Width);
        }
    }
}
