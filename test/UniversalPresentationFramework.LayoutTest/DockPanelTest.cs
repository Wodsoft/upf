using System.IO;
using UPFControls = Wodsoft.UI.Controls;
using WPFControls = System.Windows.Controls;

namespace Wodsoft.UI.Test
{
    public class DockPanelTest : BaseTest
    {
        [WpfFact]
        public void DockPanelNormalTest()
        {
            var xaml = File.ReadAllText("DockPanelTest.xaml");
            var upfDock = LoadUpfWithWpfXaml<UPFControls.DockPanel>(xaml);
            var wpfDock = (WPFControls.DockPanel)System.Windows.Markup.XamlReader.Parse(xaml);
            upfDock.Arrange(new Rect(0, 0, 400, 400));
            wpfDock.Arrange(new System.Windows.Rect(0, 0, 400, 400));
            LayoutComparer.ComparerUIElement(upfDock, wpfDock);

            upfDock.UpdateLayout();
            upfDock.Arrange(new Rect(0, 0, 800, 800));
            wpfDock.UpdateLayout();
            wpfDock.Arrange(new System.Windows.Rect(0, 0, 800, 800));
            LayoutComparer.ComparerUIElement(upfDock, wpfDock);
        }
    }
}

