using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Controls
{
    public abstract class DataTemplateSelector
    {
        /// <summary>
        /// Override this method to return an app specific <seealso cref="DataTemplate"/>.
        /// </summary>
        /// <param name="item">The data content</param>
        /// <param name="container">The element to which the template will be applied</param>
        /// <returns>an app-specific template to apply, or null.</returns>
        public abstract DataTemplate SelectTemplate(object? item, FrameworkElement container);
    }
}
