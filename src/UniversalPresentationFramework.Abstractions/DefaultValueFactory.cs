using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public abstract class DefaultValueFactory
    {
        public abstract object? DefaultValue { get; }

        public abstract object CreateDefaultValue(DependencyObject owner, DependencyProperty property, DependencyPropertyKey? key);
    }
}
