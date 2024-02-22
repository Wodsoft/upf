using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public abstract class TemplateKey : ResourceKey
    {
        public abstract Type DataType { get; }

        public override int GetHashCode()=> DataType.GetHashCode();

        public override Assembly Assembly => DataType.Assembly;
    }
}
