using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;

namespace Wodsoft.UI
{
    public class FrameworkElementFactory
    {
        private Type _type;

        public FrameworkElementFactory(Type type)
        {
            _type = type;
        }

        public Type Type { get => _type; set => _type = value; }

        public FrameworkElement Create(out NameScope nameScope)
        {
            throw new NotImplementedException();
        }
    }
}
