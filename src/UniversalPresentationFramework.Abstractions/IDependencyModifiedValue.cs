using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public interface IDependencyModifiedValue
    {
        object? GetValue(ref DependencyEffectiveValue effectiveValue);
    }
}
