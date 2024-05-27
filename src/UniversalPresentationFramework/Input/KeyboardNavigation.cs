using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Input
{
    public class KeyboardNavigation
    {
        #region Properties

        public static readonly DependencyProperty AcceptsReturnProperty =
                DependencyProperty.RegisterAttached(
                        "AcceptsReturn",
                        typeof(bool),
                        typeof(KeyboardNavigation),
                        new FrameworkPropertyMetadata(false));
        public static void SetAcceptsReturn(DependencyObject element, bool enabled)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            element.SetValue(AcceptsReturnProperty, enabled);
        }
        public static bool GetAcceptsReturn(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return (bool)element.GetValue(AcceptsReturnProperty)!;
        }

        #endregion
    }
}
