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
    public class ItemsControlTest : BaseTest
    {
        [WpfFact]
        public void NormalTest()
        {
            List<float> upfData = [
                10,
                20,
                30,
                40,
                50
            ];
            List<double> wpfData = [
                10,
                20,
                30,
                40,
                50
            ];

            var xaml = File.ReadAllText("ItemsControlTest.xaml");
            var upfItemsControl = LoadUpfWithWpfXaml<UPFControls.ItemsControl>(xaml);
            upfItemsControl.ItemsSource = upfData;
            var wpfItemsControl = (WPFControls.ItemsControl)System.Windows.Markup.XamlReader.Parse(xaml);
            
            wpfItemsControl.ItemsSource = wpfData;
            upfItemsControl.Arrange(new Rect(0, 0, 400, 400));
            wpfItemsControl.Arrange(new System.Windows.Rect(0, 0, 400, 400));

            LayoutComparer.ComparerUIElement(upfItemsControl, wpfItemsControl);
        }
    }
}
