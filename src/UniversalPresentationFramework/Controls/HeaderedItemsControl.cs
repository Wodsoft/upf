using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Data;

namespace Wodsoft.UI.Controls
{
    [DefaultProperty("Header")]
    public class HeaderedItemsControl : ItemsControl, IItemContainer
    {
        #region Properties

        /// <summary>
        ///     The DependencyProperty for the Header property.
        ///     Flags:              None
        ///     Default Value:      null
        /// </summary>
        public static readonly DependencyProperty HeaderProperty =
                HeaderedContentControl.HeaderProperty.AddOwner(
                        typeof(HeaderedItemsControl),
                        new FrameworkPropertyMetadata(
                                null,
                                new PropertyChangedCallback(OnHeaderChanged)));


        /// <summary>
        ///     Header is the data used to for the header of each item in the control.
        /// </summary>
        [Bindable(true)]
        public object? Header
        {
            get { return GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        ///     Called when HeaderProperty is invalidated on "d."
        /// </summary>
        private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HeaderedItemsControl ctrl = (HeaderedItemsControl)d;

            ctrl.SetValue(HeaderedContentControl.HasHeaderPropertyKey, e.NewValue != null);
            ctrl.OnHeaderChanged(e.OldValue, e.NewValue);
        }

        /// <summary>
        ///     This method is invoked when the Header property changes.
        /// </summary>
        /// <param name="oldHeader">The old value of the Header property.</param>
        /// <param name="newHeader">The new value of the Header property.</param>
        protected virtual void OnHeaderChanged(object? oldHeader, object? newHeader)
        {
            // if Header should not be treated as a logical child, there's
            // nothing to do
            if (!IsHeaderLogical())
                return;

            if (oldHeader is LogicalObject oldLogical)
                RemoveLogicalChild(oldLogical);
            if (newHeader is LogicalObject newLogical)
                AddLogicalChild(newLogical);
        }

        /// <summary>
        ///     The DependencyProperty for the HasHeader property.
        ///     Flags:              None
        ///     Other:              Read-Only
        ///     Default Value:      false
        /// </summary>
        public static readonly DependencyProperty HasHeaderProperty = HeaderedContentControl.HasHeaderProperty.AddOwner(typeof(HeaderedItemsControl));

        /// <summary>
        ///     True if Header is non-null, false otherwise.
        /// </summary>
        [Bindable(false), Browsable(false)]
        public bool HasHeader
        {
            get { return (bool)GetValue(HasHeaderProperty)!; }
        }

        /// <summary>
        ///     The DependencyProperty for the HeaderTemplate property.
        ///     Flags:              Can be used in style rules
        ///     Default Value:      null
        /// </summary>
        public static readonly DependencyProperty HeaderTemplateProperty =
                HeaderedContentControl.HeaderTemplateProperty.AddOwner(
                        typeof(HeaderedItemsControl),
                        new FrameworkPropertyMetadata(
                                null,
                                new PropertyChangedCallback(OnHeaderTemplateChanged)));

        /// <summary>
        ///     HeaderTemplate is the template used to display the header of each item.
        /// </summary>
        [Bindable(true)]
        public DataTemplate? HeaderTemplate
        {
            get { return (DataTemplate?)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        /// <summary>
        ///     Called when HeaderTemplateProperty is invalidated on "d."
        /// </summary>
        private static void OnHeaderTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HeaderedItemsControl ctrl = (HeaderedItemsControl)d;
            ctrl.OnHeaderTemplateChanged((DataTemplate?)e.OldValue, (DataTemplate?)e.NewValue);
        }

        /// <summary>
        ///     This method is invoked when the HeaderTemplate property changes.
        /// </summary>
        /// <param name="oldHeaderTemplate">The old value of the HeaderTemplate property.</param>
        /// <param name="newHeaderTemplate">The new value of the HeaderTemplate property.</param>
        protected virtual void OnHeaderTemplateChanged(DataTemplate? oldHeaderTemplate, DataTemplate? newHeaderTemplate)
        {

        }


        /// <summary>
        ///     The DependencyProperty for the HeaderTemplateSelector property.
        ///     Flags:              none
        ///     Default Value:      null
        /// </summary>
        public static readonly DependencyProperty HeaderTemplateSelectorProperty =
                HeaderedContentControl.HeaderTemplateSelectorProperty.AddOwner(
                        typeof(HeaderedItemsControl),
                        new FrameworkPropertyMetadata(
                                null,
                                new PropertyChangedCallback(OnHeaderTemplateSelectorChanged)));

        /// <summary>
        ///     HeaderTemplateSelector allows the application writer to provide custom logic
        ///     for choosing the template used to display the header of each item.
        /// </summary>
        /// <remarks>
        ///     This property is ignored if <seealso cref="HeaderTemplate"/> is set.
        /// </remarks>
        [Bindable(true)]
        public DataTemplateSelector? HeaderTemplateSelector
        {
            get { return (DataTemplateSelector?)GetValue(HeaderTemplateSelectorProperty); }
            set { SetValue(HeaderTemplateSelectorProperty, value); }
        }

        /// <summary>
        ///     Called when HeaderTemplateSelectorProperty is invalidated on "d."
        /// </summary>
        private static void OnHeaderTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HeaderedItemsControl ctrl = (HeaderedItemsControl)d;
            ctrl.OnHeaderTemplateSelectorChanged((DataTemplateSelector?)e.OldValue, (DataTemplateSelector?)e.NewValue);
        }

        /// <summary>
        ///     This method is invoked when the HeaderTemplateSelector property changes.
        /// </summary>
        /// <param name="oldHeaderTemplateSelector">The old value of the HeaderTemplateSelector property.</param>
        /// <param name="newHeaderTemplateSelector">The new value of the HeaderTemplateSelector property.</param>
        protected virtual void OnHeaderTemplateSelectorChanged(DataTemplateSelector? oldHeaderTemplateSelector, DataTemplateSelector? newHeaderTemplateSelector)
        {

        }

        /// <summary>
        ///     The DependencyProperty for the HeaderStringFormat property.
        ///     Flags:              None
        ///     Default Value:      null
        /// </summary>
        public static readonly DependencyProperty HeaderStringFormatProperty =
                DependencyProperty.Register(
                        "HeaderStringFormat",
                        typeof(string),
                        typeof(HeaderedItemsControl),
                        new FrameworkPropertyMetadata(
                              null,
                              new PropertyChangedCallback(OnHeaderStringFormatChanged)));


        /// <summary>
        ///     HeaderStringFormat is the format used to display the header content as a string.
        ///     This arises only when no template is available.
        /// </summary>
        [Bindable(true)]
        public string? HeaderStringFormat
        {
            get { return (string?)GetValue(HeaderStringFormatProperty); }
            set { SetValue(HeaderStringFormatProperty, value); }
        }

        /// <summary>
        ///     Called when HeaderStringFormatProperty is invalidated on "d."
        /// </summary>
        private static void OnHeaderStringFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HeaderedItemsControl ctrl = (HeaderedItemsControl)d;
            ctrl.OnHeaderStringFormatChanged((string?)e.OldValue, (string?)e.NewValue);
        }

        /// <summary>
        ///     This method is invoked when the HeaderStringFormat property changes.
        /// </summary>
        /// <param name="oldHeaderStringFormat">The old value of the HeaderStringFormat property.</param>
        /// <param name="newHeaderStringFormat">The new value of the HeaderStringFormat property.</param>
        protected virtual void OnHeaderStringFormatChanged(string? oldHeaderStringFormat, string? newHeaderStringFormat)
        {
        }

        #endregion

        #region PreparableContainer

        private bool _headerIsNotLogical;
        private bool _headerIsItem;

        public override void PrepareContainer(ItemsControl parent, object? item)
        {
            bool headerIsNotLogical = item != this;
            // don't treat Header as a logical child
            _headerIsNotLogical = headerIsNotLogical;

            // copy styles from parent ItemsControl
            PrepareItemsControl(item, parent);

            if (headerIsNotLogical)
            {
                if (_headerIsItem || !HasNonDefaultValue(HeaderProperty))
                {
                    Header = item;
                    _headerIsItem = true;
                }

                var itemTemplate = parent.ItemTemplate;
                var itemTemplateSelector = parent.ItemTemplateSelector;
                var itemStringFormat = parent.ItemStringFormat;

                if (itemTemplate != null)
                {
                    HeaderTemplate = itemTemplate;
                }
                if (itemTemplateSelector != null)
                {
                    HeaderTemplateSelector = itemTemplateSelector;
                }
                if (itemStringFormat != null && !HasNonDefaultValue(HeaderStringFormatProperty))
                {
                    HeaderStringFormat = itemStringFormat;
                }

                PrepareHierarchy(item, parent);
            }
        }

        public override void ClearContainer(object? item)
        {
            if (item != this)
            {
                if (_headerIsItem)
                {
                    ClearValue(HeaderProperty);
                }
            }
        }

        void PrepareHierarchy(object? item, ItemsControl parentItemsControl)
        {
            // get the effective header template
            DataTemplate? headerTemplate = HeaderTemplate;

            if (headerTemplate == null)
            {
                DataTemplateSelector? selector = HeaderTemplateSelector;
                if (selector != null)
                {
                    headerTemplate = selector.SelectTemplate(item, this);
                }

                if (headerTemplate == null && item != null)
                {
                    headerTemplate = (DataTemplate?)ResourceHelper.FindTemplateResource(this, item, typeof(DataTemplate));
                }
            }

            // if the effective template is a HierarchicalDataTemplate, forward
            // the special properties
            HierarchicalDataTemplate? hTemplate = headerTemplate as HierarchicalDataTemplate;
            if (hTemplate != null)
            {
                bool templateMatches = (ItemTemplate == parentItemsControl.ItemTemplate);
                bool containerStyleMatches = (ItemContainerStyle == parentItemsControl.ItemContainerStyle);

                if (hTemplate.ItemsSource != null && !HasNonDefaultValue(ItemsSourceProperty))
                {
                    SetBinding(ItemsSourceProperty, hTemplate.ItemsSource);
                }

                if (hTemplate.ItemStringFormat != null && ItemStringFormat == parentItemsControl.ItemStringFormat)
                {
                    // if the HDT defines a string format, turn off the
                    // forwarding of ItemTemplate[Selector] (which would get in the way).
                    ClearValue(ItemTemplateProperty);
                    ClearValue(ItemTemplateSelectorProperty);

                    ItemStringFormat = hTemplate.ItemStringFormat;
                }

                if (hTemplate.ItemTemplateSelector != null && ItemTemplateSelector == parentItemsControl.ItemTemplateSelector)
                {
                    // if the HDT defines a template selector, turn off the
                    // forwarding of ItemTemplate (which would get in the way).
                    ClearValue(ItemTemplateProperty);

                    ItemTemplateSelector = hTemplate.ItemTemplateSelector;
                }

                if (hTemplate.ItemTemplate != null && templateMatches)
                {
                    ItemTemplate = hTemplate.ItemTemplate;
                }

                if (hTemplate.ItemContainerStyleSelector != null && ItemContainerStyleSelector == parentItemsControl.ItemContainerStyleSelector)
                {
                    // if the HDT defines a container-style selector, turn off the
                    // forwarding of ItemContainerStyle (which would get in the way).
                    ClearValue(ItemContainerStyleProperty);

                    ItemContainerStyleSelector = hTemplate.ItemContainerStyleSelector;
                }

                if (hTemplate.ItemContainerStyle != null && containerStyleMatches)
                {
                    ItemContainerStyle = hTemplate.ItemContainerStyle;
                }

                //if (hTemplate.IsAlternationCountSet && AlternationCount == parentItemsControl.AlternationCount)
                //{
                //    // forward the HDT's alternation count
                //    ClearValue(AlternationCountProperty);
                //    bool setAlternationCount = true;
                //    if (setAlternationCount)
                //    {
                //        AlternationCount = hTemplate.AlternationCount;
                //    }
                //}

                //if (hTemplate.IsItemBindingGroupSet && ItemBindingGroup == parentItemsControl.ItemBindingGroup)
                //{
                //    // forward the HDT's ItemBindingGroup
                //    ClearValue(ItemBindingGroupProperty);
                //    bool setItemBindingGroup = (hTemplate.ItemBindingGroup != null);
                //    if (setItemBindingGroup)
                //    {
                //        ItemBindingGroup = hTemplate.ItemBindingGroup;
                //    }
                //}
            }
        }

        #endregion

        private bool IsHeaderLogical()
        {
            // use cached result, if available
            if (_headerIsNotLogical)
                return false;

            // if Header property is data-bound, it should not be logical
            if (GetBindingExpression(HeaderProperty) != null)
            {
                _headerIsNotLogical = true;
                return false;
            }

            // otherwise, Header is logical
            return true;
        }
    }
}
