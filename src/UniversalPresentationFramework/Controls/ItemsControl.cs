using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using Wodsoft.UI.Controls.Primitives;
using Wodsoft.UI.Data;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Controls
{
    [ContentProperty("Items")]
    public class ItemsControl : Control, IItemsControl
    {
        private ItemContainerGenerator? _itemContainerGenerator;

        static ItemsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ItemsControl), new FrameworkPropertyMetadata(typeof(ItemsControl)));
        }

        #region Properties

        private ItemCollection? _items;
        public ItemCollection Items
        {
            get
            {
                if (_items == null)
                    CreateItemCollectionAndGenerator(ItemsSource);
                return _items;
            }
        }

        public static readonly DependencyProperty ItemsSourceProperty
            = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(ItemsControl),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnItemsSourceChanged)));
        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ItemsControl ic = (ItemsControl)d;
            IEnumerable? oldValue = (IEnumerable?)e.OldValue;
            IEnumerable? newValue = (IEnumerable?)e.NewValue;

            if (ic._items != null)
                ic._items.SetItemsSource(newValue);

            //((IContainItemStorage)ic).Clear();

            //BindingExpressionBase beb = BindingOperations.GetBindingExpressionBase(d, ItemsSourceProperty);
            //if (beb != null)
            //{
            //    // ItemsSource is data-bound.   Always go to ItemsSource mode.
            //    // Also, extract the source item, to supply as context to the
            //    // CollectionRegistering event
            //    ic.Items.SetItemsSource(newValue, (object x) => beb.GetSourceItem(x));
            //}
            //else if (e.NewValue != null)
            //{
            //    // ItemsSource is non-null, but not data-bound.  Go to ItemsSource mode
            //    ic.Items.SetItemsSource(newValue);
            //}
            //else
            //{
            //    // ItemsSource is explicitly null.  Return to normal mode.
            //    ic.Items.ClearItemsSource();
            //}

            ic.OnItemsSourceChanged(oldValue, newValue);
        }
        protected virtual void OnItemsSourceChanged(IEnumerable? oldValue, IEnumerable? newValue)
        {
        }

        public IEnumerable? ItemsSource
        {
            get { return (IEnumerable?)GetValue(ItemsSourceProperty); }
            set
            {
                if (value == null)
                {
                    ClearValue(ItemsSourceProperty);
                }
                else
                {
                    SetValue(ItemsSourceProperty, value);
                }
            }
        }

        public static readonly DependencyProperty ItemTemplateProperty =
                DependencyProperty.Register(
                        "ItemTemplate",
                        typeof(DataTemplate),
                        typeof(ItemsControl),
                        new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnItemTemplateChanged)));
        private static void OnItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ItemsControl)d).OnItemTemplateChanged((DataTemplate?)e.OldValue, (DataTemplate?)e.NewValue);
        }
        public DataTemplate? ItemTemplate
        {
            get { return (DataTemplate?)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }
        protected virtual void OnItemTemplateChanged(DataTemplate? oldItemTemplate, DataTemplate? newItemTemplate)
        {
            //CheckTemplateSource();
            if (_itemContainerGenerator != null)
            {
                _itemContainerGenerator.Refresh();
            }
        }

        public static readonly DependencyProperty ItemTemplateSelectorProperty =
                DependencyProperty.Register(
                        "ItemTemplateSelector",
                        typeof(DataTemplateSelector),
                        typeof(ItemsControl),
                        new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnItemTemplateSelectorChanged)));
        private static void OnItemTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ItemsControl)d).OnItemTemplateSelectorChanged((DataTemplateSelector?)e.OldValue, (DataTemplateSelector?)e.NewValue);
        }
        public DataTemplateSelector? ItemTemplateSelector
        {
            get { return (DataTemplateSelector?)GetValue(ItemTemplateSelectorProperty); }
            set { SetValue(ItemTemplateSelectorProperty, value); }
        }
        protected virtual void OnItemTemplateSelectorChanged(DataTemplateSelector? oldItemTemplateSelector, DataTemplateSelector? newItemTemplateSelector)
        {
            //CheckTemplateSource();
            if ((_itemContainerGenerator != null) && (ItemTemplate == null))
            {
                _itemContainerGenerator.Refresh();
            }
        }

        public static readonly DependencyProperty ItemStringFormatProperty =
                DependencyProperty.Register(
                        "ItemStringFormat",
                        typeof(string),
                        typeof(ItemsControl),
                        new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnItemStringFormatChanged)));
        private static void OnItemStringFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ItemsControl ctrl = (ItemsControl)d;

            ctrl.OnItemStringFormatChanged((string?)e.OldValue, (string?)e.NewValue);
            //ctrl.UpdateDisplayMemberTemplateSelector();
        }
        public string? ItemStringFormat
        {
            get { return (string?)GetValue(ItemStringFormatProperty); }
            set { SetValue(ItemStringFormatProperty, value); }
        }
        protected virtual void OnItemStringFormatChanged(string? oldItemStringFormat, string? newItemStringFormat)
        {
        }


        public static readonly DependencyProperty ItemContainerStyleProperty =
                DependencyProperty.Register(
                        "ItemContainerStyle",
                        typeof(Style),
                        typeof(ItemsControl),
                        new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnItemContainerStyleChanged)));
        private static void OnItemContainerStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ItemsControl)d).OnItemContainerStyleChanged((Style?)e.OldValue, (Style?)e.NewValue);
        }
        public Style? ItemContainerStyle
        {
            get { return (Style?)GetValue(ItemContainerStyleProperty); }
            set { SetValue(ItemContainerStyleProperty, value); }
        }
        protected virtual void OnItemContainerStyleChanged(Style? oldItemContainerStyle, Style? newItemContainerStyle)
        {
            //Helper.CheckStyleAndStyleSelector("ItemContainer", ItemContainerStyleProperty, ItemContainerStyleSelectorProperty, this);

            if (_itemContainerGenerator != null)
            {
                _itemContainerGenerator.Refresh();
            }
        }

        public static readonly DependencyProperty ItemContainerStyleSelectorProperty =
                DependencyProperty.Register(
                        "ItemContainerStyleSelector",
                        typeof(StyleSelector),
                        typeof(ItemsControl),
                        new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnItemContainerStyleSelectorChanged)));
        private static void OnItemContainerStyleSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ItemsControl)d).OnItemContainerStyleSelectorChanged((StyleSelector?)e.OldValue, (StyleSelector?)e.NewValue);
        }
        public StyleSelector? ItemContainerStyleSelector
        {
            get { return (StyleSelector?)GetValue(ItemContainerStyleSelectorProperty); }
            set { SetValue(ItemContainerStyleSelectorProperty, value); }
        }
        protected virtual void OnItemContainerStyleSelectorChanged(StyleSelector? oldItemContainerStyleSelector, StyleSelector? newItemContainerStyleSelector)
        {
            //Helper.CheckStyleAndStyleSelector("ItemContainer", ItemContainerStyleProperty, ItemContainerStyleSelectorProperty, this);

            if ((_itemContainerGenerator != null) && (ItemContainerStyle == null))
            {
                _itemContainerGenerator.Refresh();
            }
        }

        public static readonly DependencyProperty ItemsPanelProperty
            = DependencyProperty.Register("ItemsPanel", typeof(ItemsPanelTemplate), typeof(ItemsControl),
                                          new FrameworkPropertyMetadata(GetDefaultItemsPanelTemplate(), new PropertyChangedCallback(OnItemsPanelChanged)));
        private static ItemsPanelTemplate GetDefaultItemsPanelTemplate()
        {
            ItemsPanelTemplate template = new ItemsPanelTemplate(new FrameworkElementFactory(typeof(StackPanel)));
            template.Seal();
            return template;
        }
        private static void OnItemsPanelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ItemsControl control = (ItemsControl)d;
            control.ItemsPanelChanged?.Invoke(control, EventArgs.Empty);
            control.OnItemsPanelChanged((ItemsPanelTemplate?)e.OldValue, (ItemsPanelTemplate?)e.NewValue);
        }
        public ItemsPanelTemplate? ItemsPanel
        {
            get { return (ItemsPanelTemplate?)GetValue(ItemsPanelProperty); }
            set { SetValue(ItemsPanelProperty, value); }
        }

        public IItemContainerGenerator ItemContainerGenerator
        {
            get
            {
                if (_itemContainerGenerator == null)
                    CreateItemCollectionAndGenerator(ItemsSource);
                return _itemContainerGenerator;
            }
        }

        protected virtual void OnItemsPanelChanged(ItemsPanelTemplate? oldItemsPanel, ItemsPanelTemplate? newItemsPanel)
        {

        }


        #endregion

        #region Events

        public event EventHandler? ItemsPanelChanged;

        #endregion

        #region Methods

        public bool IsItemItsOwnContainer(object item)
        {
            return IsItemItsOwnContainerOverride(item);
        }

        protected virtual bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is UIElement);
        }

        protected virtual FrameworkElement GetContainerForItemOverride()
        {
            return new ContentPresenter();
        }

        protected virtual void PrepareContainerForItemOverride(UIElement element, object? item)
        {
            // Each type of "ItemContainer" element may require its own initialization.
            // We use explicit polymorphism via internal methods for this.
            //
            // Another way would be to define an interface IGeneratedItemContainer with
            // corresponding virtual "core" methods.  Base classes (ContentControl,
            // ItemsControl, ContentPresenter) would implement the interface
            // and forward the work to subclasses via the "core" methods.
            //
            // While this is better from an OO point of view, and extends to
            // 3rd-party elements used as containers, it exposes more public API.
            // Management considers this undesirable, hence the following rather
            // inelegant code.

            var itemTemplate = ItemTemplate;
            var itemTemplateSelector = ItemTemplateSelector;
            var itemStringFormat = ItemStringFormat;

            //if (element is HeaderedContentControl hcc)
            //{
            //    hcc.PrepareHeaderedContentControl(item, ItemTemplate, ItemTemplateSelector, ItemStringFormat);
            //}
            if (element is ContentControl cc)
            {
                if (itemTemplate != null)
                    cc.ContentTemplate = itemTemplate;
                if (itemTemplateSelector != null)
                    cc.ContentTemplateSelector = itemTemplateSelector;
                if (itemStringFormat != null)
                    cc.ContentStringFormat = itemStringFormat;
                cc.Content = item;
            }
            else if (element is ContentPresenter cp)
            {
                if (itemTemplate != null)
                    cp.ContentTemplate = itemTemplate;
                if (itemTemplateSelector != null)
                    cp.ContentTemplateSelector = itemTemplateSelector;
                if (itemStringFormat != null)
                    cp.ContentStringFormat = itemStringFormat;
                cp.Content = item;
            }
            //else if (element is HeaderedItemsControl hic)
            //{
            //    hic.PrepareHeaderedItemsControl(item, this);
            //}
            //else if (element is ItemsControl ic)
            //{
            //    if (ic != this)
            //    {
            //        ic.PrepareItemsControl(item, this);
            //    }
            //}
        }

        protected virtual void ClearContainerForItemOverride(UIElement element, object? item)
        {
            //if (element is HeaderedContentControl hcc)
            //{
            //
            //}
            if (element is ContentControl cc)
            {
                cc.ClearValue(ContentControl.ContentProperty);
            }
            else if (element is ContentPresenter cp)
            {
                cp.ClearValue(ContentPresenter.ContentProperty);
            }
            //else if (element is HeaderedItemsControl hic)
            //{
            //
            //}
            //else if (element is ItemsControl ic)
            //{
            //
            //}
        }

        protected virtual bool ShouldApplyItemContainerStyle(UIElement container, object? item)
        {
            return true;
        }

        public static ItemsControl? ItemsControlFromItemContainer(UIElement container)
        {
            // ui appeared in items collection
            ItemsControl? ic = LogicalTreeHelper.GetParent(container) as ItemsControl;
            if (ic != null)
            {
                // this is the right ItemsControl as long as the item
                // is (or is eligible to be) its own container
                if (ic.IsItemItsOwnContainer(container))
                    return ic;
                else
                    return null;
            }

            var panel = VisualTreeHelper.GetParent(container) as Panel;
            if (panel == null)
                return null;
            return ItemsControl.GetItemsOwner(panel);
        }

        public static UIElement? ContainerFromElement(ItemsControl itemsControl, UIElement element)
        {
            if (itemsControl == null)
                throw new ArgumentNullException(nameof(itemsControl));
            if (element == null)
                throw new ArgumentNullException("element");

            // if the element is itself the desired container, return it
            if (IsContainerForItemsControl(element, itemsControl))
            {
                return element;
            }

            // start the tree walk at the element's parent
            var visualParent = element.VisualParent;

            // walk up, stopping when we reach the desired container
            while (visualParent != null)
            {
                if (visualParent is UIElement ui && IsContainerForItemsControl(ui, itemsControl))
                {
                    return ui;
                }

                visualParent = visualParent.VisualParent;
            }
            return null;
        }

        public UIElement? ContainerFromElement(UIElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return ContainerFromElement(this, element);
        }

        private static bool IsContainerForItemsControl(UIElement element, ItemsControl itemsControl)
        {
            // is the element a container?
            if (itemsControl._itemContainerGenerator != null && itemsControl._itemContainerGenerator.IsItemContainer(element))
            {
                // does the element belong to the itemsControl?
                return itemsControl == ItemsControlFromItemContainer(element);
            }

            return false;
        }

        [MemberNotNull(nameof(_items))]
        [MemberNotNull(nameof(_itemContainerGenerator))]
        private void CreateItemCollectionAndGenerator(IEnumerable? source)
        {
            _items = CreateItemCollection(source);
            _itemContainerGenerator = new ItemContainerGenerator(new GeneratorHost(this));
        }

        protected virtual ItemCollection CreateItemCollection(IEnumerable? source)
        {
            if (source == null)
                return new ItemCollection();
            else
                return new ItemCollection(source);
        }

        public static ItemsControl? GetItemsOwner(Panel panel)
        {
            if (panel.IsItemsHost)
            {
                // see if element was generated for an ItemsPresenter
                ItemsPresenter? ip = panel.TemplatedParent as ItemsPresenter;

                if (ip != null)
                {
                    // if so use the element whose style begat the ItemsPresenter
                    return ip.Owner as ItemsControl;
                }
                else
                {
                    // otherwise use element's templated parent
                    return panel.TemplatedParent as ItemsControl;
                }
            }
            return null;
        }

        #endregion

        private class GeneratorHost : IGeneratorHost, IDisposable
        {
            private readonly ItemsControl _itemsControl;
            private readonly List<NotifyCollectionChangedEventHandler> _collectionChangedHandlers;

            public GeneratorHost(ItemsControl itemsControl)
            {
                _itemsControl = itemsControl;
                _collectionChangedHandlers = new List<NotifyCollectionChangedEventHandler>();
            }

            public IList Items => _itemsControl.Items;

            public void ClearContainerForItem(UIElement container, object? item) => _itemsControl.ClearContainerForItemOverride(container, item);

            public UIElement GetContainerForItem(object? item)
            {
                if (item != null && _itemsControl.IsItemItsOwnContainer(item))
                {
                    UIElement element = (UIElement)item;
                    return element;
                }
                return _itemsControl.GetContainerForItemOverride();
            }

            //public bool IsHostForItemContainer(UIElement container)
            //{
            //    throw new NotImplementedException();
            //}

            public bool IsItemItsOwnContainer(object item) => _itemsControl.IsItemItsOwnContainer(item);

            public void ListenCollectionChanged(NotifyCollectionChangedEventHandler handler)
            {
                _collectionChangedHandlers.Add(handler);
                _itemsControl.Items.CollectionChanged += handler;
            }

            public void PrepareItemContainer(UIElement container, object? item) => _itemsControl.PrepareContainerForItemOverride(container, item);

            public void Dispose()
            {
                foreach (var handler in _collectionChangedHandlers)
                    _itemsControl.Items.CollectionChanged -= handler;
            }
        }
    }
}
