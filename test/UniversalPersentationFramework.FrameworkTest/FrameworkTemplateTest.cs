using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Test
{
    public class FrameworkTemplateTest : BaseTest
    {
        [Fact]
        public void Test1()
        {
            var xaml = File.ReadAllText("ContentControlTemplate.xaml");
            var contentControl = LoadUpfXaml<ContentControl>(xaml);
            contentControl.Arrange(new Rect(0, 0, 100, 100));
        }
    }
}
