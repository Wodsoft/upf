using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Wodsoft.UI.Controls.Primitives
{
    public class ItemsChangedEventArgs : EventArgs
    {

        public ItemsChangedEventArgs(NotifyCollectionChangedAction action, GeneratorPosition position, GeneratorPosition oldPosition, int itemCount, int itemUICount)
        {
            Action = action;
            Position = position;
            OldPosition = oldPosition;
            ItemCount = itemCount;
            ItemUICount = itemUICount;
        }

        public ItemsChangedEventArgs(NotifyCollectionChangedAction action, GeneratorPosition position, int itemCount, int itemUICount) 
            : this(action, position, new GeneratorPosition(-1, 0), itemCount, itemUICount)

        {
        }

        public NotifyCollectionChangedAction Action { get; }
        public GeneratorPosition Position { get; }
        public GeneratorPosition OldPosition { get; }
        public int ItemCount { get; }
        public int ItemUICount { get; }
    }
}
