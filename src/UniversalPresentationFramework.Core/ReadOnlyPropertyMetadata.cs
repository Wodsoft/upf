using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public class ReadOnlyPropertyMetadata : PropertyMetadata
    {
        private GetReadOnlyValueCallback _getValueCallback;

        public ReadOnlyPropertyMetadata(object defaultValue,
                                        GetReadOnlyValueCallback getValueCallback,
                                        PropertyChangedCallback propertyChangedCallback) :
                                        base(defaultValue, propertyChangedCallback)
        {
            _getValueCallback = getValueCallback;
        }

        public override GetReadOnlyValueCallback GetReadOnlyValueCallback
        {
            get
            {
                return _getValueCallback;
            }
        }
    }
}
