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
    public class GridTest : BaseTest
    {
        [WpfFact]
        public void NormalTest()
        {
            var xaml = File.ReadAllText("GridTest.xaml");
            var upfGrid = LoadUpfWithWpfXaml<UPFControls.Grid>(xaml);
            var wpfGrid = (WPFControls.Grid)System.Windows.Markup.XamlReader.Parse(xaml);
            upfGrid.Arrange(new Rect(0, 0, 400, 400));
            wpfGrid.Arrange(new System.Windows.Rect(0, 0, 400, 400));
            LayoutComparer.ComparerUIElement(upfGrid, wpfGrid);

            upfGrid.UpdateLayout();
            upfGrid.Arrange(new Rect(0, 0, 800, 800));
            wpfGrid.UpdateLayout();
            wpfGrid.Arrange(new System.Windows.Rect(0, 0, 800, 800));
            LayoutComparer.ComparerUIElement(upfGrid, wpfGrid);
        }

        [WpfFact]
        public void DefaultCellTest()
        {
            var xaml = File.ReadAllText("GridDefaultCellTest.xaml");
            var upfGrid = LoadUpfWithWpfXaml<UPFControls.Grid>(xaml);
            var wpfGrid = (WPFControls.Grid)System.Windows.Markup.XamlReader.Parse(xaml);
            upfGrid.Arrange(new Rect(0, 0, 400, 400));
            wpfGrid.Arrange(new System.Windows.Rect(0, 0, 400, 400));

            LayoutComparer.ComparerUIElement(upfGrid, wpfGrid);
        }
    }
}
