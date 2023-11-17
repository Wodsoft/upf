using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    internal class TreeMap<T>
    {
        public ItemStructList<TreeMapNode<T>> Children { get; } = new ItemStructList<TreeMapNode<T>>();
    }

    internal class TreeMapNode<T> : TreeMap<T>
    {
        public TreeMapNode(T value)
        {
            Value = value;
        }

        public T Value { get; set; }
    }
}
