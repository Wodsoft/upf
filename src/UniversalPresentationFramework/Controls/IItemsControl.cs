using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Controls.Primitives;

namespace Wodsoft.UI.Controls
{
    public interface IItemsControl
    {
        event EventHandler? ItemsPanelChanged;

        ItemsPanelTemplate? ItemsPanel { get; }

        IItemContainerGenerator ItemContainerGenerator { get; }
    }
}
