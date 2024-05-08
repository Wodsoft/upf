using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Test
{
    public class TextObject : FrameworkElement
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(TextObject));
        public string? Text { get => (string?)GetValue(TextProperty); set => SetValue(TextProperty, value); }
    }
}
