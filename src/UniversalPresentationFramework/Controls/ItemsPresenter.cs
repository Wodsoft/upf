using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Controls
{
    public class ItemsPresenter : FrameworkElement
    {
        #region Template

        private IItemsControl? _owner;
        protected override void OnPreApplyTemplate()
        {
            //get template root element
            var rootElement = LogicalRoot as FrameworkElement;
            if (rootElement == null)
            {
                DisconnectOwner(_owner);
                return;
            }
            //get ItemsControl owner
            var owner = rootElement.TemplatedParent as IItemsControl;
            if (owner == null)
            {
                DisconnectOwner(_owner);
                return;
            }
            if (owner != _owner)
                ConnectOwner(owner);
        }

        private void DisconnectOwner(IItemsControl? owner)
        {
            if (owner == null)
                return;
            owner.ItemsPanelChanged += Owner_DependencyPropertyChanged;
            _owner = null;
        }

        private void ConnectOwner(IItemsControl owner)
        {
            owner.ItemsPanelChanged -= Owner_DependencyPropertyChanged;
            _owner = owner;
        }

        private void Owner_DependencyPropertyChanged(object? sender, EventArgs e)
        {
            OnTemplateChanged();
            InvalidateMeasure();
        }

        protected override FrameworkTemplate? GetTemplate() => _owner?.ItemsPanel;

        private Panel? _panel;
        protected override void OnApplyTemplate()
        {
            if (_panel != null)
                DisconnectPanel(_panel);
            var panel = TemplatedChild as Panel;
            if (panel == null)
                throw new InvalidOperationException("Templated child must be a panel.");
            if (panel.VisualChildrenCount != 0)
                throw new InvalidOperationException("Items panel must be empty.");
            ConnectPanel(panel);
        }

        private void ConnectPanel(Panel panel)
        {
            panel.IsItemsHost = true;
        }

        private void DisconnectPanel(Panel panel)
        {
            panel.IsItemsHost = false;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (TemplatedChild == null)
                return new Size(0.0f, 0.0f);
            TemplatedChild.Measure(availableSize);
            return TemplatedChild.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (TemplatedChild != null)
                TemplatedChild.Arrange(new Rect(finalSize));
            return finalSize;
        }

        #endregion

        #region Properties

        public IItemsControl? Owner => _owner;

        #endregion
    }
}
