using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Controls
{
    public class StyleSelector
    {
        /// <summary>
        /// Override this method to return an app specific <seealso cref="Style"/>.
        /// </summary>
        /// <param name="item">The data content</param>
        /// <param name="container">The element to which the style will be applied</param>
        /// <returns>an app-specific style to apply, or null.</returns>
        public virtual Style? SelectStyle(object item, DependencyObject container)
        {
            return null;
        }
    }
}
