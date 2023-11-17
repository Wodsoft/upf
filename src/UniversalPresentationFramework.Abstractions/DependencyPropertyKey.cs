using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public sealed class DependencyPropertyKey
    {
        /// <summary>
        ///     The DependencyProperty associated with this access key.  This key
        /// does not authorize access to any other property.
        /// </summary>
        public DependencyProperty DependencyProperty
        {
            get
            {
                return _dp;
            }
        }

        internal DependencyPropertyKey(DependencyProperty? dp)
        {
            _dp = dp;
        }

        /// <summary>
        ///     Override the metadata of a property that is already secured with
        /// this key.
        /// </summary>
        public void OverrideMetadata(Type forType, PropertyMetadata typeMetadata)
        {
            if (_dp == null)
            {
                throw new InvalidOperationException();
            }

            _dp.OverrideMetadata(forType, typeMetadata, this);
        }

        internal void SetDependencyProperty(DependencyProperty dp)
        {
            Debug.Assert(_dp == null, "This should only be used when we need a placeholder and have a temporary value of null. It should not be used to change this property.");
            _dp = dp;
        }

        private DependencyProperty? _dp = null;
    }
}
