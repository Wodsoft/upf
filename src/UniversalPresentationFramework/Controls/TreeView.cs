using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Controls
{
    public class TreeView : ItemsControl
    {
        #region Constructors

        static TreeView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeView), new FrameworkPropertyMetadata(typeof(TreeView)));

        }

        #endregion

        #region Properties

        private static readonly DependencyPropertyKey _SelectedItemPropertyKey =
            DependencyProperty.RegisterReadOnly("SelectedItem", typeof(object), typeof(TreeView), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty SelectedItemProperty = _SelectedItemPropertyKey.DependencyProperty;
        public object? SelectedItem { get { return GetValue(SelectedItemProperty); } }


        private static readonly DependencyPropertyKey _SelectedValuePropertyKey = DependencyProperty.RegisterReadOnly("SelectedValue", typeof(object), typeof(TreeView), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty SelectedValueProperty = _SelectedValuePropertyKey.DependencyProperty;
        public object? SelectedValue { get { return GetValue(SelectedValueProperty); } }

        public static readonly DependencyProperty SelectedValuePathProperty =
            DependencyProperty.Register(
                    "SelectedValuePath",
                    typeof(string),
                    typeof(TreeView),
                    new FrameworkPropertyMetadata(
                            string.Empty,
                            new PropertyChangedCallback(OnSelectedValuePathChanged)));
        private static void OnSelectedValuePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TreeView tree = (TreeView)d;
            //SelectedValuePathBindingExpression.ClearValue(tree);
            //tree.UpdateSelectedValue(tree.SelectedItem);
        }
        public string? SelectedValuePath
        {
            get { return (string?)GetValue(SelectedValuePathProperty); }
            set { SetValue(SelectedValuePathProperty, value); }
        }

        #endregion

        #region Events

        public static readonly RoutedEvent SelectedItemChangedEvent = EventManager.RegisterRoutedEvent("SelectedItemChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(TreeView));
        public event RoutedPropertyChangedEventHandler<object> SelectedItemChanged
        {
            add { AddHandler(SelectedItemChangedEvent, value); }
            remove { RemoveHandler(SelectedItemChangedEvent, value); }
        }

        /// <summary>
        ///     Called when <see cref="SelectedItem"/> changes.
        ///     Default implementation fires the <see cref="SelectedItemChanged"/> event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnSelectedItemChanged(RoutedPropertyChangedEventArgs<object> e)
        {
            // Needs an Automation Event
            RaiseEvent(e);
        }

        #endregion

        #region Container

        protected override bool IsItemItsOwnContainerOverride(object item) => item is TreeViewItem;

        protected override FrameworkElement GetContainerForItemOverride() => new TreeViewItem();

        

        #endregion
    }
}
