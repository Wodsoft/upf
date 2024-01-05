using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Test
{
    public class EventRoot : Grid
    {
        private void Clicked(object sender, RoutedEventArgs e)
        {
            ClickCount++;
        }

        public int ClickCount { get; private set; }
    }
}
