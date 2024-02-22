using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPFControls = Wodsoft.UI.Controls;
using WPFControls = System.Windows.Controls;

namespace Wodsoft.UI.Test
{
    public class ContentTest : BaseTest
    {
        [WpfFact]
        public void NormalTest()
        {
            var xaml = File.ReadAllText("ContentTest.xaml");
            var upfContentControl = LoadUpfWithWpfXaml<UPFControls.ContentControl>(xaml);
            var wpfContentControl = (WPFControls.ContentControl)System.Windows.Markup.XamlReader.Parse(xaml);
            upfContentControl.Arrange(new Rect(0, 0, 400, 400));
            wpfContentControl.Arrange(new System.Windows.Rect(0, 0, 400, 400));

            LayoutComparer.ComparerUIElement(upfContentControl, wpfContentControl);
        }
    }
}
