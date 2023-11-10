using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPFControls = Wodsoft.UI.Controls;
using WPFControls = System.Windows.Controls;

namespace Wodsoft.UI.Test
{
    public class StackPanelTest
    {
        [WpfFact]
        public void Vertical()
        {
            UPFControls.StackPanel upfStackPanel = new UPFControls.StackPanel();
            WPFControls.StackPanel wpfStackPanel = new WPFControls.StackPanel();
            wpfStackPanel.Width = upfStackPanel.Width = 640f;
            wpfStackPanel.Height = upfStackPanel.Height = 480f;
            upfStackPanel.Orientation = UPFControls.Orientation.Vertical;
            wpfStackPanel.Orientation = WPFControls.Orientation.Vertical;
            UPFControls.ContentControl upfContentControl1 = new UPFControls.ContentControl();
            WPFControls.ContentControl wpfContentControl1 = new WPFControls.ContentControl();
            wpfContentControl1.Width = upfContentControl1.Width = 100f;
            wpfContentControl1.Height = upfContentControl1.Height = 80f;
            upfContentControl1.HorizontalAlignment = HorizontalAlignment.Center;
            wpfContentControl1.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            upfContentControl1.Margin = new Thickness(120, 60, -20, -20);
            wpfContentControl1.Margin = new System.Windows.Thickness(120, 60, -20, -20);
            upfStackPanel.Children.Add(upfContentControl1);
            wpfStackPanel.Children.Add(wpfContentControl1);
            UPFControls.ContentControl upfContentControl2 = new UPFControls.ContentControl();
            WPFControls.ContentControl wpfContentControl2 = new WPFControls.ContentControl();
            wpfContentControl2.Width = upfContentControl2.Width = 120f;
            wpfContentControl2.Height = upfContentControl2.Height = 60f;
            upfContentControl2.HorizontalAlignment = HorizontalAlignment.Left;
            wpfContentControl2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            upfContentControl2.Margin = new Thickness(120, 60, -20, -20);
            wpfContentControl2.Margin = new System.Windows.Thickness(120, 60, -20, -20);
            upfStackPanel.Children.Add(upfContentControl2);
            wpfStackPanel.Children.Add(wpfContentControl2);
            UPFControls.ContentControl upfContentControl3 = new UPFControls.ContentControl();
            WPFControls.ContentControl wpfContentControl3 = new WPFControls.ContentControl();
            wpfContentControl3.Width = upfContentControl3.Width = 120f;
            wpfContentControl3.Height = upfContentControl3.Height = 60f;
            upfContentControl3.HorizontalAlignment = HorizontalAlignment.Right;
            wpfContentControl3.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            upfContentControl3.Margin = new Thickness(120, 60, -20, -20);
            wpfContentControl3.Margin = new System.Windows.Thickness(120, 60, -20, -20);
            upfStackPanel.Children.Add(upfContentControl3);
            wpfStackPanel.Children.Add(wpfContentControl3);

            upfContentControl1.Arrange(new Rect(0, 0, 640f, 480f));
            wpfContentControl1.Arrange(new System.Windows.Rect(0, 0, 640d, 480d));

            Assert.True(LayoutComparer.IsSameRenderSize(upfContentControl1, wpfContentControl1));
            Assert.True(LayoutComparer.IsSameVisualOffset(upfContentControl1, wpfContentControl1));
            Assert.True(LayoutComparer.IsSameRenderSize(upfContentControl2, wpfContentControl2));
            Assert.True(LayoutComparer.IsSameVisualOffset(upfContentControl2, wpfContentControl2));
            Assert.True(LayoutComparer.IsSameRenderSize(upfContentControl3, wpfContentControl3));
            Assert.True(LayoutComparer.IsSameVisualOffset(upfContentControl3, wpfContentControl3));
        }

        [WpfFact]
        public void Horizontal()
        {
            UPFControls.StackPanel upfStackPanel = new UPFControls.StackPanel();
            WPFControls.StackPanel wpfStackPanel = new WPFControls.StackPanel();
            wpfStackPanel.Width = upfStackPanel.Width = 640f;
            wpfStackPanel.Height = upfStackPanel.Height = 480f;
            upfStackPanel.Orientation = UPFControls.Orientation.Horizontal;
            wpfStackPanel.Orientation = WPFControls.Orientation.Horizontal;
            UPFControls.ContentControl upfContentControl1 = new UPFControls.ContentControl();
            WPFControls.ContentControl wpfContentControl1 = new WPFControls.ContentControl();
            wpfContentControl1.Width = upfContentControl1.Width = 40f;
            wpfContentControl1.Height = upfContentControl1.Height = 80f;
            upfContentControl1.HorizontalAlignment = HorizontalAlignment.Center;
            wpfContentControl1.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            upfContentControl1.Margin = new Thickness(120, 60, -20, -20);
            wpfContentControl1.Margin = new System.Windows.Thickness(120, 60, -20, -20);
            upfStackPanel.Children.Add(upfContentControl1);
            wpfStackPanel.Children.Add(wpfContentControl1);
            UPFControls.ContentControl upfContentControl2 = new UPFControls.ContentControl();
            WPFControls.ContentControl wpfContentControl2 = new WPFControls.ContentControl();
            wpfContentControl2.Width = upfContentControl2.Width = 60f;
            wpfContentControl2.Height = upfContentControl2.Height = 60f;
            upfContentControl2.HorizontalAlignment = HorizontalAlignment.Left;
            wpfContentControl2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            upfContentControl2.Margin = new Thickness(120, 60, -20, -20);
            wpfContentControl2.Margin = new System.Windows.Thickness(120, 60, -20, -20);
            upfStackPanel.Children.Add(upfContentControl2);
            wpfStackPanel.Children.Add(wpfContentControl2);
            UPFControls.ContentControl upfContentControl3 = new UPFControls.ContentControl();
            WPFControls.ContentControl wpfContentControl3 = new WPFControls.ContentControl();
            wpfContentControl3.Width = upfContentControl3.Width = 50f;
            wpfContentControl3.Height = upfContentControl3.Height = 60f;
            upfContentControl3.HorizontalAlignment = HorizontalAlignment.Right;
            wpfContentControl3.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            upfContentControl3.Margin = new Thickness(120, 60, -20, -20);
            wpfContentControl3.Margin = new System.Windows.Thickness(120, 60, -20, -20);
            upfStackPanel.Children.Add(upfContentControl3);
            wpfStackPanel.Children.Add(wpfContentControl3);

            upfContentControl1.Arrange(new Rect(0, 0, 640f, 480f));
            wpfContentControl1.Arrange(new System.Windows.Rect(0, 0, 640d, 480d));

            Assert.True(LayoutComparer.IsSameRenderSize(upfContentControl1, wpfContentControl1));
            Assert.True(LayoutComparer.IsSameVisualOffset(upfContentControl1, wpfContentControl1));
            Assert.True(LayoutComparer.IsSameRenderSize(upfContentControl2, wpfContentControl2));
            Assert.True(LayoutComparer.IsSameVisualOffset(upfContentControl2, wpfContentControl2));
            Assert.True(LayoutComparer.IsSameRenderSize(upfContentControl3, wpfContentControl3));
            Assert.True(LayoutComparer.IsSameVisualOffset(upfContentControl3, wpfContentControl3));
        }
    }
}
