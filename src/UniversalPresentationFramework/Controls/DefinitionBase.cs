using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Controls
{
    public abstract class DefinitionBase : FrameworkContentElement
    {
        private Grid? _grid;

        internal Grid? Grid => _grid;

        internal void ConnectParent(Grid grid)
        {
            if (_grid != null)
                throw new InvalidOperationException("Definition was connected to a grid.");
            _grid = grid;
            _grid.AddLogicalChild(this);
        }

        internal void DisconnectParent(Grid grid)
        {
            if (_grid == null)
                throw new InvalidOperationException("Definition haven't connect to a grid.");
            if (grid != _grid)
                throw new InvalidOperationException("Definition not belong to this grid.");
            grid.RemoveLogicalChild(this);
            _grid = null;
        }

        protected override DependencyObject? GetInheritanceParent()
        {
            return _grid;
        }

        internal float Percent, Offset;
    }
}
