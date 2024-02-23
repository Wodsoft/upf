using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using Wodsoft.UI.Markup;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Controls
{
    [DefaultProperty("Content")]
    [ContentProperty("Content")]
    public class ContentControl : Control, IAddChild
    {
        static ContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentControl), new FrameworkPropertyMetadata(typeof(ContentControl)));
        }

        #region Properties

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

        public static readonly DependencyProperty ContentTemplateProperty =
                DependencyProperty.Register(
                        "ContentTemplate",
                        typeof(DataTemplate),
                        typeof(ContentControl),
                        new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnContentTemplateChanged)));
        private static void OnContentTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ContentControl ctrl = (ContentControl)d;
            ctrl.OnContentTemplateChanged((DataTemplate?)e.OldValue, (DataTemplate?)e.NewValue);
        }
        public DataTemplate? ContentTemplate { get { return (DataTemplate?)GetValue(ContentTemplateProperty); } set { SetValue(ContentTemplateProperty, value); } }
        protected virtual void OnContentTemplateChanged(DataTemplate? oldContentTemplate, DataTemplate? newContentTemplate)
        {
            OnTemplateSettingChanged();
        }

        public static readonly DependencyProperty ContentTemplateSelectorProperty =
                DependencyProperty.Register(
                        "ContentTemplateSelector",
                        typeof(DataTemplateSelector),
                        typeof(ContentControl),
                        new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnContentTemplateSelectorChanged)));
        private static void OnContentTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ContentControl ctrl = (ContentControl)d;
            ctrl.OnContentTemplateSelectorChanged((DataTemplateSelector?)e.NewValue, (DataTemplateSelector?)e.NewValue);
        }
        public DataTemplateSelector? ContentTemplateSelector { get { return (DataTemplateSelector?)GetValue(ContentTemplateSelectorProperty); } set { SetValue(ContentTemplateSelectorProperty, value); } }        
        protected virtual void OnContentTemplateSelectorChanged(DataTemplateSelector? oldContentTemplateSelector, DataTemplateSelector? newContentTemplateSelector)
        {
            OnTemplateSettingChanged();
        }

        private void OnTemplateSettingChanged()
        {

        }


        public static readonly DependencyProperty ContentStringFormatProperty =
                DependencyProperty.Register(
                        "ContentStringFormat",
                        typeof(string),
                        typeof(ContentControl),
                        new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnContentStringFormatChanged)));
        private static void OnContentStringFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ContentControl ctrl = (ContentControl)d;
            ctrl.OnContentStringFormatChanged((string?)e.OldValue, (string?)e.NewValue);
        }
        public string? ContentStringFormat { get { return (string?)GetValue(ContentStringFormatProperty); } set { SetValue(ContentStringFormatProperty, value); } }

        protected virtual void OnContentStringFormatChanged(string? oldContentStringFormat, string? newContentStringFormat)
        {
        }

        #endregion

        #region AddChild

        void IAddChild.AddChild(object value)
        {
            Content = value;
        }

        void IAddChild.AddText(string text)
        {
            Content = text;
        }

        #endregion
    }
}
