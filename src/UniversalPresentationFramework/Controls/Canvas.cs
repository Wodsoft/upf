using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Controls
{
    public class Canvas : Panel
    {
        public static readonly DependencyProperty LeftProperty = DependencyProperty.RegisterAttached("Left", typeof(float), typeof(Canvas),
            new FrameworkPropertyMetadata(float.NaN, new PropertyChangedCallback(OnPositioningChanged)),
            new ValidateValueCallback(IsFiniteOrNaN));


        public static readonly DependencyProperty TopProperty = DependencyProperty.RegisterAttached("Top", typeof(float), typeof(Canvas),
                    new FrameworkPropertyMetadata(float.NaN, new PropertyChangedCallback(OnPositioningChanged)),
                    new ValidateValueCallback(IsFiniteOrNaN));
        private static void OnPositioningChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIElement uie = (UIElement)d;
            if (uie != null)
            {
                Canvas? p = uie.VisualParent as Canvas;
                if (p != null)
                    p.InvalidateArrange();
            }
        }
        private static bool IsFiniteOrNaN(object? o)
        {
            if (o is float value)
                return !float.IsInfinity(value);
            return false;
        }
    }
}
