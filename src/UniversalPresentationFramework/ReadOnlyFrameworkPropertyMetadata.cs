using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public class ReadOnlyFrameworkPropertyMetadata : FrameworkPropertyMetadata
    {
        private GetReadOnlyValueCallback _getValueCallback;

        public ReadOnlyFrameworkPropertyMetadata(object defaultValue, GetReadOnlyValueCallback getValueCallback) :
            base(defaultValue)
        {
            _getValueCallback = getValueCallback;
        }

        public override GetReadOnlyValueCallback? GetReadOnlyValueCallback => _getValueCallback;
    }
}
