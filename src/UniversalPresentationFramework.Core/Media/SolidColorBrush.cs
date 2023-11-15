using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    public class SolidColorBrush : Brush
    {
        public SolidColorBrush() { }

        public SolidColorBrush(Color color)
        {
            Color = color;
        }


        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color",
                                   typeof(Color),
                                   typeof(SolidColorBrush),
                                   new PropertyMetadata(Colors.Transparent));
        public Color Color { get { return (Color)GetValue(ColorProperty)!; } set { SetValue(ColorProperty, value); } }
    }
}
