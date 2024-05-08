using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Test
{
    public class DependencyDataSource : DependencyObject
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(DependencyDataSource));
        public string? Text { get => (string?)GetValue(TextProperty); set => SetValue(TextProperty, value); }


        public static readonly DependencyProperty IntegerProperty = DependencyProperty.Register("Integer", typeof(int), typeof(DependencyDataSource));
        public int Integer { get => (int)GetValue(IntegerProperty)!; set => SetValue(IntegerProperty, value); }
    }
}
