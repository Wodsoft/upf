using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Shapes;

namespace Wodsoft.UI.Test
{
    public class VisualStateTest : BaseTest
    {
        [Fact]
        public void SetterTest()
        {
            var xaml = File.ReadAllText("VisualStateSetterTest.xaml");
            var d = LoadUpfXaml<MyObject>(xaml);
            d.Arrange(new Rect(0, 0, 100, 100));
            var rect = (Rectangle)d.FindTemplateChild("rect")!;
            Assert.Equal(float.NaN, rect.Width);
            Assert.True(VisualStateManager.GoToState(d, "on", true));
            Assert.Equal(100f, rect.Width);
            Assert.True(VisualStateManager.GoToState(d, "off", true));
            Assert.Equal(50f, rect.Width);
        }

        [Fact]
        public void SStoryboardTest()
        {
            var xaml = File.ReadAllText("VisualStateStoryboardTest.xaml");
            var d = LoadUpfXaml<MyObject>(xaml);
            d.Arrange(new Rect(0, 0, 100, 100));
            var rect = (Rectangle)d.FindTemplateChild("rect")!;
            Assert.Equal(0f, rect.Width);
            Assert.True(VisualStateManager.GoToState(d, "on", true));
            ApplyTick(TimeSpan.FromMilliseconds(500));
            Assert.Equal(50f, rect.Width);
            ApplyTick(TimeSpan.FromMilliseconds(500));
            Assert.Equal(100f, rect.Width);
            ApplyTick(TimeSpan.FromMilliseconds(1000));
            Assert.Equal(100f, rect.Width);
            Assert.True(VisualStateManager.GoToState(d, "off", true));
            ApplyTick(TimeSpan.FromMilliseconds(500));
            Assert.Equal(75f, rect.Width);
            ApplyTick(TimeSpan.FromMilliseconds(500));
            Assert.Equal(50f, rect.Width);
            ApplyTick(TimeSpan.FromMilliseconds(1000));
            Assert.Equal(50f, rect.Width);
        }
    }
}
