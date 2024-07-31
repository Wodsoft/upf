using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public interface IHaveEmpty<T>
    {
        abstract static T Empty { get; }
    }
}
