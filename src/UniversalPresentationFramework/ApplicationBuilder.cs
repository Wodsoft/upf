using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public class ApplicationBuilder
    {
        private Type? _appType;

        public ApplicationBuilder UseApplication<T>() where T : Application, new()
        {
            if (_appType != null)
                throw new InvalidOperationException("Application can't use twice.");
            _appType = typeof(T);
            return this;
        }
    }
}
