using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Providers
{
    public interface IResourceProvider
    {
        object? FindSystemResource(object key);
    }
}
