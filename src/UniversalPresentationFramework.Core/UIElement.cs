using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;

namespace Wodsoft.UI
{
    public class UIElement : Visual
    {
        #region Layout

        public void Measure(Size availableSize)
        {
            throw new NotImplementedException();
        }

        public void Arrange(Rect finalRect)
        {
            throw new NotImplementedException();
        }

        protected virtual Size MeasureCore(Size availableSize)
        {
            throw new NotImplementedException();
        }

        protected virtual void ArrangeCore(Rect finalRect)
        {
            throw new NotImplementedException();
        }

        public void InvalidateMeasure()
        {

        }

        public void InvalidateArrange()
        {

        }

        #endregion

        public static readonly DependencyProperty VisibilityProperty =
        DependencyProperty.Register(
                "Visibility",
                typeof(Visibility),
                typeof(UIElement),
                new PropertyMetadata(
                        Visibility.Visible,
                        new PropertyChangedCallback(OnVisibilityChanged)),
                new ValidateValueCallback(ValidateVisibility));

        private static void OnVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIElement uie = (UIElement)d;
            Visibility newVisibility = (Visibility)e.NewValue!;
            uie._visibility = newVisibility;
        }

        private static bool ValidateVisibility(object? o)
        {
            if (o is Visibility value)
                return (value == Visibility.Visible) || (value == Visibility.Hidden) || (value == Visibility.Collapsed);
            return false;
        }

        private Visibility _visibility;
        public Visibility Visibility
        {
            get { return _visibility; }
            set { SetValue(VisibilityProperty, value); }
        }
    }
}
