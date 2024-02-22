//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xaml.Markup;
//using Wodsoft.UI.Data;

//namespace Wodsoft.UI.Controls
//{
//    [ContentProperty("Items")]
//    public class ItemsControl : Control
//    {
//        #region Properties


//        public static readonly DependencyProperty ItemsSourceProperty
//            = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(ItemsControl),
//                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnItemsSourceChanged)));
//        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
//        {
//            ItemsControl ic = (ItemsControl)d;
//            IEnumerable? oldValue = (IEnumerable?)e.OldValue;
//            IEnumerable? newValue = (IEnumerable?)e.NewValue;

//            ((IContainItemStorage)ic).Clear();

//            BindingExpressionBase beb = BindingOperations.GetBindingExpressionBase(d, ItemsSourceProperty);
//            if (beb != null)
//            {
//                // ItemsSource is data-bound.   Always go to ItemsSource mode.
//                // Also, extract the source item, to supply as context to the
//                // CollectionRegistering event
//                ic.Items.SetItemsSource(newValue, (object x) => beb.GetSourceItem(x));
//            }
//            else if (e.NewValue != null)
//            {
//                // ItemsSource is non-null, but not data-bound.  Go to ItemsSource mode
//                ic.Items.SetItemsSource(newValue);
//            }
//            else
//            {
//                // ItemsSource is explicitly null.  Return to normal mode.
//                ic.Items.ClearItemsSource();
//            }

//            ic.OnItemsSourceChanged(oldValue, newValue);
//        }
//        protected virtual void OnItemsSourceChanged(IEnumerable? oldValue, IEnumerable? newValue)
//        {
//        }
//        public IEnumerable? ItemsSource
//        {
//            get { return Items.ItemsSource; }
//            set
//            {
//                if (value == null)
//                {
//                    ClearValue(ItemsSourceProperty);
//                }
//                else
//                {
//                    SetValue(ItemsSourceProperty, value);
//                }
//            }
//        }

//        #endregion
//    }
//}
