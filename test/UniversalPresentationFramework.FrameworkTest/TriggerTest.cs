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
            Assert.Equal(50f, rect.Width);
            Assert.Null(obj.TextD);
            obj.TextB = "text2";
            Assert.Equal("trigger text", obj.TextC);
            Assert.Equal("D text", obj.TextD);
            Assert.Equal(150f, rect.Width);
        }

        [Fact]
        public void ActionTest()
        {
            var xaml = File.ReadAllText("TriggerActionTemplateTest.xaml");
            var grid = LoadUpfXaml<Grid>(xaml);
            grid.Arrange(new Rect(0, 0, 100, 100));
            var obj = (MyObject)grid.FindName("target")!;
            var rect = (Rectangle)obj.FindTemplateChild("rect")!;
            Assert.Null(obj.TextC);
            Assert.Null(obj.TextD);
            obj.TextA = "run";
            Assert.Equal("trigger text", obj.TextC);
            Assert.Equal(100f, rect.Width);
            Assert.Null(obj.TextD);
            ApplyTick(TimeSpan.FromMilliseconds(250));
            Assert.Equal(125f, rect.Width);
            obj.TextB = "pause";
            Assert.Equal("D text", obj.TextD);
            ApplyTick(TimeSpan.FromMilliseconds(500));
            Assert.Equal(125f, rect.Width);
            obj.TextB = null;
            Assert.Null(obj.TextD);
            Assert.Equal(125f, rect.Width);
            obj.TextB = "half";
            ApplyTick(TimeSpan.FromMilliseconds(1000));
            Assert.Equal(175f, rect.Width);
            obj.TextB = null;
            ApplyTick(TimeSpan.FromMilliseconds(500));
            Assert.Equal(200d, rect.Width);
            obj.TextB = "seek";
            Assert.Equal(150d, rect.Width);
            obj.TextB = "fill";
            Assert.Equal(200d, rect.Width);
            obj.TextA = "stop";
            Assert.Null(obj.TextC);
            Assert.Equal(100f, rect.Width);
        }
    }
}
