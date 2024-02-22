using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;

namespace Wodsoft.UI.Controls
{
    [DictionaryKeyProperty("ItemContainerTemplateKey")]
    public class ItemContainerTemplate : DataTemplate
    {
        public ItemContainerTemplateKey? ItemContainerTemplateKey
        {
            get
            {
                return (DataType != null) ? new ItemContainerTemplateKey(DataType) : null;
            }
        }
    }
}
