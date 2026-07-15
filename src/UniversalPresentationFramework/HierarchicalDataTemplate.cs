using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Controls;
using Wodsoft.UI.Data;

namespace Wodsoft.UI
{
    public class HierarchicalDataTemplate : DataTemplate
    {
        private BindingBase? _itemsSourceBinding;
        private DataTemplate? _itemTemplate;
        private DataTemplateSelector? _itemTemplateSelector;
        private Style? _itemContainerStyle;
        private StyleSelector? _itemContainerStyleSelector;
        private string? _itemStringFormat;

        public HierarchicalDataTemplate() { }

        public HierarchicalDataTemplate(Type dataType) : base(dataType) { }

        /// <summary>
        ///     ItemsSource binding for this DataTemplate.  This is applied
        ///     to the ItemsSource property on a generated HeaderedItemsControl,
        ///     to indicate where to find the collection that represents the
        ///     next level in the data hierarchy.
        /// </summary>
        public BindingBase? ItemsSource
        {
            get { return _itemsSourceBinding; }
            set
            {
                CheckSealed();
                _itemsSourceBinding = value;
            }
        }

        /// <summary>
        ///     ItemTemplate for this DataTemplate.  This is applied
        ///     to the ItemTemplate property on a generated HeaderedItemsControl,
        ///     to indicate how to display items from the next level in the
        ///     data hierarchy.
        /// </summary>
        public DataTemplate? ItemTemplate
        {
            get { return _itemTemplate; }
            set
            {
                CheckSealed();
                _itemTemplate = value;
            }
        }

        /// <summary>
        ///     ItemTemplateSelector for this DataTemplate.  This is applied
        ///     to the ItemTemplateSelector property on a generated HeaderedItemsControl,
        ///     to indicate how to select a template to display items from the
        ///     next level in the data hierarchy.
        /// </summary>
        public DataTemplateSelector? ItemTemplateSelector
        {
            get { return _itemTemplateSelector; }
            set
            {
                CheckSealed();
                _itemTemplateSelector = value;
            }
        }

        /// <summary>
        ///     ItemContainerStyle for this DataTemplate.  This is applied
        ///     to the ItemContainerStyle property on a generated HeaderedItemsControl,
        ///     to indicate a style for the containers it generates.
        /// </summary>
        public Style? ItemContainerStyle
        {
            get { return _itemContainerStyle; }
            set
            {
                CheckSealed();
                _itemContainerStyle = value;
            }
        }

        /// <summary>
        ///     ItemContainerStyleSelector for this DataTemplate.  This is applied
        ///     to the ItemContainerStyleSelector property on a generated HeaderedItemsControl,
        ///     to indicate how to select a style for the containers it generates.
        /// </summary>
        public StyleSelector? ItemContainerStyleSelector
        {
            get { return _itemContainerStyleSelector; }
            set
            {
                CheckSealed();
                _itemContainerStyleSelector = value;
            }
        }

        /// <summary>
        ///     ItemStringFormat for this DataTemplate.  This is applied
        ///     to the ItemStringFormat property on a generated HeaderedItemsControl,
        ///     to indicate how to format items from the
        ///     next level in the data hierarchy.
        /// </summary>
        public string? ItemStringFormat
        {
            get { return _itemStringFormat; }
            set
            {
                CheckSealed();
                _itemStringFormat = value;
            }
        }
    }
}
