using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Test
{
    public class StaticResourceTest : BaseTest
    {
        [Fact]
        public void DirectResourceTest()
        {
            var xaml = File.ReadAllText("StaticResourceDirectResourceTest.xaml");
            var grid = LoadUpfXaml<Grid>(xaml);
            var obj = (MyObject)grid.FindName("target")!;
            Assert.Equal("test", obj.TextA);
        }

        [Fact]
        public void TemplateResourceTest()
        {
            var xaml = File.ReadAllText("StaticResourceTemplateResourceTest.xaml");
            var grid = LoadUpfXaml<Grid>(xaml);
            grid.Arrange(new Rect(0, 0, 100, 100));
            var obj = (MyObject)grid.FindName("target")!;
            var text = (TextObject)obj.TemplatedChild!.FindName("text")!;
            Assert.Equal("test", text.Text);
        }

        [Fact]
        public void TemplateOuterResourceTest()
        {
            var xaml = File.ReadAllText("StaticResourceTemplateOuterResourceTest.xaml");
            var grid = LoadUpfXaml<Grid>(xaml);
            grid.Arrange(new Rect(0, 0, 100, 100));
            var obj = (MyObject)grid.FindName("target")!;
            var text = (TextObject)obj.TemplatedChild!.FindName("text")!;
            Assert.Equal("test", text.Text);
        }

        [Fact]
        public void DeepTemplateResourceTest()
        {
            var xaml = File.ReadAllText("StaticResourceDeepTemplateResourceTest.xaml");
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
            var xaml = File.ReadAllText("StaticResourceDeepTemplateOuterResourceTest.xaml");
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
            var xaml = File.ReadAllText("StaticResourceDeepTemplateMultipleResourceTest.xaml");
            var grid = LoadUpfXaml<Grid>(xaml);
            grid.Arrange(new Rect(0, 0, 100, 100));
            var obj = (MyObject)grid.FindName("target")!;
            var innerObject = (MyObject)obj.TemplatedChild!.FindName("innerObject")!;
            var text = (TextObject)innerObject.TemplatedChild!.FindName("text")!;
            Assert.Equal("template text", text.Text);
        }
    }
}
