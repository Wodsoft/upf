using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Controls
{
    public interface IGeneratorHost
    {
        IList Items { get; }

        UIElement GetContainerForItem(object? item);

        void PrepareItemContainer(UIElement container, object? item);

        void ClearContainerForItem(UIElement container, object? item);

        //bool IsHostForItemContainer(UIElement container);

        void ListenCollectionChanged(NotifyCollectionChangedEventHandler handler);

        bool IsItemItsOwnContainer(object item);
    }
}
