using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Markup;

namespace Wodsoft.UI.Documents
{
    public class InlineCollection : TextElementCollection<Inline>, IAddChild
    {
        public InlineCollection(LogicalObject parent, TextTreeNode parentNode) : base(parent, parentNode)
        {
        }

        public void AddChild(object value)
        {
            if (value is string stringValue)
                AddText(stringValue);
            else if (value is UIElement uiElement)
                Add(new InlineUIContainer(uiElement));
            else if (value is Inline inline)
                Add(inline);
            else
                throw new InvalidCastException("Only support Inline object.");
        }

        public void AddText(string text)
        {
            Add(new Run(text));
        }

        protected override Inline ConvertToElement(object value)
        {
            if (value is string stringValue)
                return new Run(stringValue);
            else if (value is UIElement uiElement)
                return new InlineUIContainer(uiElement);
            else if (value is Inline inline)
                return inline;
            else
                throw new InvalidCastException("Only support Inline object.");
        }
    }
}
