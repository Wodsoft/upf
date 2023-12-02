using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPF = Wodsoft.UI;
using WPF = System.Windows;

namespace Wodsoft.UI.Test
{
    public static class LayoutComparer
    {
        public static void ComparerUIElement(UPF.UIElement upfElement, WPF.UIElement wpfElement)
        {
            if (upfElement == null)
                throw new ArgumentNullException(nameof(upfElement));
            if (wpfElement == null)
                throw new ArgumentNullException(nameof(wpfElement));
            Assert.True(IsSameRenderSize(upfElement, wpfElement));
            Assert.True(IsSameVisualOffset(upfElement, wpfElement));
            var upfChildrenCount = UPF.Media.VisualTreeHelper.GetChildrenCount(upfElement);
            var wpfChildrenCount = WPF.Media.VisualTreeHelper.GetChildrenCount(wpfElement);
            Assert.Equal(upfChildrenCount, wpfChildrenCount);
            for (int i = 0; i < upfChildrenCount; i++)
            {
                var upfChild = (UPF.UIElement)UPF.Media.VisualTreeHelper.GetChild(upfElement, i);
                var wpfChild = (WPF.UIElement)WPF.Media.VisualTreeHelper.GetChild(wpfElement, i);
                ComparerUIElement(upfChild, wpfChild);
            }
        }

        public static bool IsSameRenderSize(UPF.UIElement upfElement, WPF.UIElement wpfElement)
        {
            return upfElement.RenderSize.Width == wpfElement.RenderSize.Width && upfElement.RenderSize.Height == wpfElement.RenderSize.Height;
        }

        public static bool IsSameVisualOffset(UPF.UIElement upfElement, WPF.UIElement wpfElement)
        {
            var wpfOffset = WPF.Media.VisualTreeHelper.GetOffset(wpfElement);
            return upfElement.VisualOffset.X == wpfOffset.X && upfElement.VisualOffset.Y == wpfOffset.Y;
        }
    }
}
