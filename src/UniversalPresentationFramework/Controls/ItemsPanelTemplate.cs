using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Controls
{
    public class ItemsPanelTemplate : FrameworkTemplate
    {
        public ItemsPanelTemplate()
        {
        }

        public ItemsPanelTemplate(FrameworkElementFactory root)
        {
            VisualTree = root;
        }


        protected internal override Type TargetTypeInternal => typeof(ItemsPresenter);

        protected override void OnSeal()
        {
            if (Template != null)
            {
                if (Template.RootType == null || !typeof(Panel).IsAssignableFrom(Template.RootType))
                    throw new InvalidOperationException($"ItemsPanel must be a Panel. Currently is \"{Template.RootType?.FullName ?? "null"}\".");
            }
            else if (VisualTree != null)
            {
                if (!typeof(Panel).IsAssignableFrom(VisualTree.Type))
                    throw new InvalidOperationException($"ItemsPanel must be a Panel. Currently is \"{VisualTree.Type.FullName}\".");
                //VisualTree.SetValue(Panel.IsItemsHostProperty, true);
            }
        }

        protected override void ValidateTemplatedParent(FrameworkElement templatedParent)
        {
            // A ItemsPanelTemplate must be applied to an ItemsPresenter
            if (templatedParent is not ItemsPresenter)
                throw new ArgumentException($"Template target must be ItemsPresenter. Currently is \"{templatedParent.GetType().FullName}\".");
        }
    }
}
