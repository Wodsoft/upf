using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.ComponentModel
{
    public readonly struct SortDescription
    {
        public SortDescription(string propertyName, ListSortDirection direction)
        {
            PropertyName = propertyName;
            Direction = direction;
        }

        /// <summary>
        /// Property name to sort by.
        /// </summary>
        public readonly string PropertyName;

        /// <summary>
        /// Sort direction.
        /// </summary>
        public readonly ListSortDirection Direction;
    }
}
