using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public class DataTemplateKey : TemplateKey
    {
        public DataTemplateKey(Type dataType)
        {
            DataType = dataType;
        }

        public override Type DataType { get; }
    }
}
