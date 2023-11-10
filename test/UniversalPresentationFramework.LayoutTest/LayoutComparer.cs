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
