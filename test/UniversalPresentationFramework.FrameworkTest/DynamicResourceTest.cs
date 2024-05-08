using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Test
{
    public class DynamicResourceTest : BaseTest
    {
        [Fact]
        public void DirectResourceTest()
        {
            var xaml = File.ReadAllText("DynamicResourceDirectResourceTest.xaml");
            var grid = LoadUpfXaml<Grid>(xaml);
            var obj = (MyObject)grid.FindName("target")!;
            Assert.Equal("test", obj.TextA);
        }

        [Fact]
        public void TemplateResourceTest()
        {
            var xaml = File.ReadAllText("DynamicResourceTemplateResourceTest.xaml");
            var grid = LoadUpfXaml<Grid>(xaml);
            grid.Arrange(new Rect(0, 0, 100, 100));
            var obj = (MyObject)grid.FindName("target")!;
            var text = (TextObject)obj.TemplatedChild!.FindName("text")!;
            Assert.Equal("test", text.Text);
        }

        [Fact]
        public void ParentChangeTest()
        {
            var xaml = File.ReadAllText("DynamicResourceparentChangeTest.xaml");
            var grid = LoadUpfXaml<Grid>(xaml);
            grid.Arrange(new Rect(0, 0, 100, 100));
            var container1 = (Grid)grid.FindName("container1")!;
            var container2 = (Grid)grid.FindName("container2")!;
            var obj = (MyObject)grid.FindName("object")!;
            Assert.Equal("text1", obj.TextA);
            container1.Children.Remove(obj);
            Assert.Null(obj.TextA);
            container2.Children.Add(obj);
            Assert.Equal("text2", obj.TextA);
        }

        [Fact]
        public void TemplateOuterResourceTest()
        {
            var xaml = File.ReadAllText("DynamicResourceTemplateOuterResourceTest.xaml");
            var grid = LoadUpfXaml<Grid>(xaml);
            grid.Arrange(new Rect(0, 0, 100, 100));
            var obj = (MyObject)grid.FindName("target")!;
            var text = (TextObject)obj.TemplatedChild!.FindName("text")!;
            Assert.Equal("test", text.Text);
        }

        [Fact]
        public void DeepTemplateResourceTest()
        {
            var xaml = File.ReadAllText("DynamicResourceDeepTemplateResourceTest.xaml");
            var grid = LoadUpfXaml<Grid>(xaml);
            grid.Arrange(new Rect(0, 0, 100, 100));
            var obj = (MyObject)grid.FindName("target")!;
            var innerObject = (MyObject)obj.TemplatedChild!.FindName("innerObject")!;
            var text = (TextObject)innerObject.TemplatedChild!.FindName("text")!;
            Assert.Equal("test", text.Text);
        }

        [Fact]
        public void DeepTemplateOuterResourceTest()
        {
            var xaml = File.ReadAllText("DynamicResourceDeepTemplateOuterResourceTest.xaml");
            var grid = LoadUpfXaml<Grid>(xaml);
            grid.Arrange(new Rect(0, 0, 100, 100));
            var obj = (MyObject)grid.FindName("target")!;
            var innerObject = (MyObject)obj.TemplatedChild!.FindName("innerObject")!;
            var text = (TextObject)innerObject.TemplatedChild!.FindName("text")!;
            Assert.Equal("test", text.Text);
        }

        [Fact]
        public void DeepTemplateMultipleResourceTest()
        {
            var xaml = File.ReadAllText("DynamicResourceDeepTemplateMultipleResourceTest.xaml");
            var grid = LoadUpfXaml<Grid>(xaml);
            grid.Arrange(new Rect(0, 0, 100, 100));
            var obj = (MyObject)grid.FindName("target")!;
            var innerObject = (MyObject)obj.TemplatedChild!.FindName("innerObject")!;
            var text = (TextObject)innerObject.TemplatedChild!.FindName("text")!;
            Assert.Equal("template text", text.Text);
        }
    }
}
