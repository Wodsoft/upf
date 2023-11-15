using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    public abstract class Brush : DependencyObject
    {
        static Brush()
        {

        }

        #region Properties

        public static readonly DependencyProperty OpacityProperty =
                  DependencyProperty.Register("Opacity",
                                              typeof(double),
                                              typeof(Brush),
                                              new PropertyMetadata(1.0f));
        public float Opacity { get { return (float)GetValue(OpacityProperty)!; } set { SetValue(OpacityProperty, value); } }

        #endregion
    }
}
