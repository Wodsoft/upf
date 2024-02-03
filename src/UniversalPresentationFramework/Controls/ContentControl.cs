using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Controls
{
    public class ContentControl : Control
    {
        #region Properties
        private bool _isLogicalContent;

        public static readonly DependencyProperty ContentProperty =
                DependencyProperty.Register(
                        "Content",
                        typeof(object),
                        typeof(ContentControl),
                        new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnContentChanged)));
        private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ContentControl ctrl = (ContentControl)d;
            ctrl.SetValue(_HasContentPropertyKey, (e.NewValue != null) ? true : false);
            ctrl.OnContentChanged(e.OldValue, e.NewValue);
        }
        protected virtual void OnContentChanged(object? oldContent, object? newContent)
        {
            // Remove the old content child
            if (oldContent is LogicalObject oldLogicalObject)
                RemoveLogicalChild(oldLogicalObject);

            //// if Content should not be treated as a logical child, there's
            //// nothing to do
            //if (ContentIsNotLogical)
            //    return;

            // Add the new content child
            if (newContent is LogicalObject newLogicalObject)
                AddLogicalChild(newLogicalObject);
        }
        public object? Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        private static readonly DependencyPropertyKey _HasContentPropertyKey =
                DependencyProperty.RegisterReadOnly(
                        "HasContent",
                        typeof(bool),
                        typeof(ContentControl),
                        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None));
        public static readonly DependencyProperty HasContentProperty = _HasContentPropertyKey.DependencyProperty;
        public bool HasContent { get { return (bool)GetValue(HasContentProperty)!; } }

        #endregion
    }
}
