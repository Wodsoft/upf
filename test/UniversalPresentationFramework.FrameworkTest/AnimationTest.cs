using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media.Animation;

namespace Wodsoft.UI.Test
{
    public class AnimationTest : BaseTest
    {
        [Fact]
        public void StoryboardTest()
        {
            var xaml = File.ReadAllText("AnimationStoryboardTest.xaml");
            var grid = LoadUpfXaml<Grid>(xaml);
            var myObject = (MyObject)grid.FindName("target")!;
            Assert.Equal(100f, myObject.Width);
            var storyboard = (Storyboard)grid.Resources["story1"]!;
            storyboard.Begin();
            ApplyTick(TimeSpan.FromMilliseconds(500));
            Assert.Equal(150f, myObject.Width);
            ApplyTick(TimeSpan.FromMilliseconds(250));
            Assert.Equal(175f, myObject.Width);
            ApplyTick(TimeSpan.FromMilliseconds(1000));
            Assert.Equal(200f, myObject.Width);
        }

        [Fact]
        public void StoryboardFillBehaviorStopTest()
        {
            var xaml = File.ReadAllText("AnimationStoryboardFillBehaviorStopTest.xaml");
            var grid = LoadUpfXaml<Grid>(xaml);
            grid.UpdateBinding();
            var myObject = (MyObject)grid.FindName("target")!;
            Assert.Equal(100f, myObject.Width);
            var storyboard = (Storyboard)grid.Resources["story1"]!;
            storyboard.Begin();
            ApplyTick(TimeSpan.FromMilliseconds(500));
            Assert.Equal(150f, myObject.Width);
            ApplyTick(TimeSpan.FromMilliseconds(250));
            Assert.Equal(175f, myObject.Width);
            ApplyTick(TimeSpan.FromMilliseconds(1000));
            Assert.Equal(100f, myObject.Width);
        }

        [Fact]
        public void StoryboardForeverTest()
        {
            var xaml = File.ReadAllText("AnimationStoryboardForeverTest.xaml");
            var grid = LoadUpfXaml<Grid>(xaml);
            var myObject = (MyObject)grid.FindName("target")!;
            Assert.Equal(100f, myObject.Width);
            var storyboard = (Storyboard)grid.Resources["story1"]!;
            storyboard.Begin();
            ApplyTick(TimeSpan.FromMilliseconds(500));
            Assert.Equal(150f, myObject.Width);
            ApplyTick(TimeSpan.FromMilliseconds(250));
            Assert.Equal(175f, myObject.Width);
            ApplyTick(TimeSpan.FromMilliseconds(250));
            Assert.Equal(200f, myObject.Width);
            ApplyTick(TimeSpan.FromMilliseconds(250));
            Assert.Equal(125f, myObject.Width);
            ApplyTick(TimeSpan.FromMilliseconds(250));
            Assert.Equal(150f, myObject.Width);
            ApplyTick(TimeSpan.FromMilliseconds(250));
            Assert.Equal(175f, myObject.Width);
            ApplyTick(TimeSpan.FromMilliseconds(250));
            Assert.Equal(200f, myObject.Width);
        }

        [Fact]
        public void StoryboardReverseTest()
        {
            var xaml = File.ReadAllText("AnimationStoryboardReverseTest.xaml");
            var grid = LoadUpfXaml<Grid>(xaml);
            var myObject = (MyObject)grid.FindName("target")!;
            Assert.Equal(100f, myObject.Width);
            var storyboard = (Storyboard)grid.Resources["story1"]!;
            storyboard.Begin();
            ApplyTick(TimeSpan.FromMilliseconds(250));
            Assert.Equal(225f, myObject.Width);
            ApplyTick(TimeSpan.FromMilliseconds(250));
            Assert.Equal(250f, myObject.Width);
            ApplyTick(TimeSpan.FromMilliseconds(500));
            Assert.Equal(300f, myObject.Width);
            ApplyTick(TimeSpan.FromMilliseconds(250));
            Assert.Equal(275f, myObject.Width);
            ApplyTick(TimeSpan.FromMilliseconds(500));
            Assert.Equal(225f, myObject.Width);
            ApplyTick(TimeSpan.FromMilliseconds(250));
            Assert.Equal(200f, myObject.Width);
        }

        [Fact]
        public void StoryboardReverseStopTest()
        {
            var xaml = File.ReadAllText("AnimationStoryboardReverseStopTest.xaml");
            var grid = LoadUpfXaml<Grid>(xaml);
            var myObject = (MyObject)grid.FindName("target")!;
            Assert.Equal(100f, myObject.Width);
            var storyboard = (Storyboard)grid.Resources["story1"]!;
            storyboard.Begin();
            ApplyTick(TimeSpan.FromMilliseconds(250));
            Assert.Equal(225f, myObject.Width);
            ApplyTick(TimeSpan.FromMilliseconds(250));
            Assert.Equal(250f, myObject.Width);
            ApplyTick(TimeSpan.FromMilliseconds(500));
            Assert.Equal(300f, myObject.Width);
            ApplyTick(TimeSpan.FromMilliseconds(250));
            Assert.Equal(275f, myObject.Width);
            ApplyTick(TimeSpan.FromMilliseconds(500));
            Assert.Equal(225f, myObject.Width);
            ApplyTick(TimeSpan.FromMilliseconds(500));
            Assert.Equal(100f, myObject.Width);
        }
    }
}
