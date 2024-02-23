using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using Wodsoft.UI.Data;
using Wodsoft.UI.Media;
using static System.Net.Mime.MediaTypeNames;

namespace Wodsoft.UI.Controls
{
    public class ContentPresenter : FrameworkElement
    {
        #region Static

        //private static readonly DataTemplate _AccessTextTemplate;
        private static readonly DataTemplate _StringTemplate;
        //private static readonly DataTemplate _XmlNodeTemplate;
        private static readonly DataTemplate _UIElementTemplate;
        private static readonly DataTemplate _DefaultTemplate;
        private static readonly DefaultSelector _DefaultTemplateSelector;

        static ContentPresenter()
        {
            _DefaultTemplateSelector = new DefaultSelector();
            {
                var template = new DataTemplate();
                FrameworkElementFactory text = new FrameworkElementFactory(typeof(TextBlock));
                text.SetValue(TextBlock.TextProperty, new TemplateBindingExtension(ContentProperty));
                template.VisualTree = text;
                template.Seal();
                _StringTemplate = template;
            }
            {
                var template = new DataTemplate();
                Binding binding = new Binding();
                binding.Converter = new StringConverter();
                FrameworkElementFactory text = new FrameworkElementFactory(typeof(TextBlock));
                text.SetBinding(TextBlock.TextProperty, binding);
                template.VisualTree = text;
                template.Seal();
                _DefaultTemplate = template;
            }
            {
                var template = new UseContentTemplate();
                template.Seal();
                _UIElementTemplate = template;
            }
        }

        #endregion

        #region Properties

        public static readonly DependencyProperty ContentProperty =
                ContentControl.ContentProperty.AddOwner(
                        typeof(ContentPresenter),
                        new FrameworkPropertyMetadata(
                            null,
                            FrameworkPropertyMetadataOptions.AffectsMeasure,
                            new PropertyChangedCallback(OnContentChanged)));
        private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ContentPresenter ctrl = (ContentPresenter)d;
            // keep the DataContext in sync with Content
            ctrl.DataContext = e.NewValue;
        }
        public object? Content { get { return GetValue(ContentControl.ContentProperty); } set { SetValue(ContentControl.ContentProperty, value); } }


        public static readonly DependencyProperty ContentTemplateProperty =
                ContentControl.ContentTemplateProperty.AddOwner(
                        typeof(ContentPresenter),
                        new FrameworkPropertyMetadata(
                                null,
                                FrameworkPropertyMetadataOptions.AffectsMeasure,
                                new PropertyChangedCallback(OnContentTemplateChanged)));
        private static void OnContentTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ContentPresenter ctrl = (ContentPresenter)d;
            ctrl._templateChanged = true;
            ctrl.OnTemplateChanged();
            ctrl.OnContentTemplateChanged((DataTemplate?)e.OldValue, (DataTemplate?)e.NewValue);
        }
        public DataTemplate? ContentTemplate { get { return (DataTemplate?)GetValue(ContentControl.ContentTemplateProperty); } set { SetValue(ContentControl.ContentTemplateProperty, value); } }
        protected virtual void OnContentTemplateChanged(DataTemplate? oldContentTemplate, DataTemplate? newContentTemplate)
        {

        }

        public static readonly DependencyProperty ContentTemplateSelectorProperty =
                ContentControl.ContentTemplateSelectorProperty.AddOwner(
                        typeof(ContentPresenter),
                        new FrameworkPropertyMetadata(
                                null,
                                FrameworkPropertyMetadataOptions.AffectsMeasure,
                                new PropertyChangedCallback(OnContentTemplateSelectorChanged)));
        private static void OnContentTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ContentPresenter ctrl = (ContentPresenter)d;
            ctrl._templateChanged = true;
            ctrl.OnTemplateChanged();
            ctrl.OnContentTemplateSelectorChanged((DataTemplateSelector?)e.OldValue, (DataTemplateSelector?)e.NewValue);
        }
        public DataTemplateSelector? ContentTemplateSelector { get { return (DataTemplateSelector?)GetValue(ContentControl.ContentTemplateSelectorProperty); } set { SetValue(ContentControl.ContentTemplateSelectorProperty, value); } }
        protected virtual void OnContentTemplateSelectorChanged(DataTemplateSelector? oldContentTemplateSelector, DataTemplateSelector? newContentTemplateSelector)
        {

        }

        public static readonly DependencyProperty ContentStringFormatProperty =
                DependencyProperty.Register(
                        "ContentStringFormat",
                        typeof(string),
                        typeof(ContentPresenter),
                        new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnContentStringFormatChanged)));
        private static void OnContentStringFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ContentPresenter ctrl = (ContentPresenter)d;
            ctrl.OnContentStringFormatChanged((string?)e.OldValue, (string?)e.NewValue);
        }
        public string? ContentStringFormat { get { return (string?)GetValue(ContentStringFormatProperty); } set { SetValue(ContentStringFormatProperty, value); } }
        protected virtual void OnContentStringFormatChanged(string? oldContentStringFormat, string? newContentStringFormat)
        {

        }


        public static readonly DependencyProperty ContentSourceProperty =
                DependencyProperty.Register(
                        "ContentSource",
                        typeof(string),
                        typeof(ContentPresenter),
                        new FrameworkPropertyMetadata("Content"));
        public string? ContentSource { get { return (string?)GetValue(ContentSourceProperty); } set { SetValue(ContentSourceProperty, value); } }

        #endregion

        #region Template

        private FrameworkTemplate? _template;
        private bool _templateChanged;

        protected override FrameworkTemplate? GetTemplate() => _template;

        protected override void OnPreApplyTemplate()
        {
            if (_template == null || _templateChanged)
            {
                _template = ChooseTemplate();
                _templateChanged = false;
            }
        }

        protected virtual DataTemplate ChooseTemplate()
        {
            DataTemplate? template = ContentTemplate;
            object? content = Content;

            // no ContentTemplate set, try ContentTemplateSelector
            if (template == null)
            {
                if (ContentTemplateSelector != null)
                {
                    template = ContentTemplateSelector.SelectTemplate(content, this);
                }
            }

            // if that failed, try the default TemplateSelector
            if (template == null)
            {
                template = _DefaultTemplateSelector.SelectTemplate(content, this);
            }

            return template;
        }

        #endregion

        #region Layout

        protected override Size MeasureOverride(Size availableSize)
        {
            if (TemplatedChild == null)
                return new Size(0.0f, 0.0f);
            TemplatedChild.Measure(availableSize);
            return TemplatedChild.DesiredSize;
        }

        /// <summary>
        ///     Default control arrangement is to only arrange
        ///     the first visual child. No transforms will be applied.
        /// </summary>
        /// <param name="arrangeBounds">The computed size.</param>
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (TemplatedChild != null)
                TemplatedChild.Arrange(new Rect(finalSize));
            return finalSize;
        }

        #endregion


        #region Visual

        protected internal override int VisualChildrenCount => TemplatedChild == null ? 0 : 1;

        protected internal override Visual GetVisualChild(int index)
        {
            if (TemplatedChild == null)
                throw new ArgumentOutOfRangeException("index");
            if (index != 0)
                throw new ArgumentOutOfRangeException("index");
            return TemplatedChild;
        }

        #endregion

        #region Selector

        internal static Type? DataTypeForItem(object? item, DependencyObject target)
        {
            if (item == null)
                return null;
            if (item is ICustomTypeProvider provider)
                return provider.GetCustomType();
            else
                return item.GetType();
        }

        private class DefaultSelector : DataTemplateSelector
        {
            /// <summary>
            /// Override this method to return an app specific <seealso cref="Template"/>.
            /// </summary>
            /// <param name="item">The data content</param>
            /// <param name="container">The container in which the content is to be displayed</param>
            /// <returns>a app specific template to apply.</returns>
            public override DataTemplate SelectTemplate(object? item, FrameworkElement container)
            {
                DataTemplate? template = null;

                // Lookup template for typeof(Content) in resource dictionaries.
                if (item != null)
                {
                    template = (DataTemplate?)ResourceHelper.FindTemplateResource(container, item, typeof(DataTemplate));
                }

                // default templates for well known types:
                if (template == null)
                {
                    //TypeConverter? tc = null;
                    if (item is string s)
                    {
                        var stringFormat = ((ContentPresenter)container).ContentStringFormat;
                        if (stringFormat == null)
                            template = _StringTemplate;
                        else
                        {
                            Binding binding = new Binding();
                            binding.StringFormat = stringFormat;

                            FrameworkElementFactory text = new FrameworkElementFactory(typeof(TextBlock));
                            text.SetBinding(TextBlock.TextProperty, binding);

                            template = new DataTemplate();
                            template.VisualTree = text;
                            template.Seal();
                        }
                    }
                    else if (item is FrameworkElement)
                        template = _UIElementTemplate;
                    //else if (SystemXmlHelper.IsXmlNode(item))
                    //    template = ((ContentPresenter)container).SelectTemplateForXML();
                    //else if (item is Inline)
                    //    template = DefaultContentTemplate;
                    //else if (item != null &&
                    //            (tc = TypeDescriptor.GetConverter(ReflectionHelper.GetReflectionType(item))) != null &&
                    //            tc.CanConvertTo(typeof(UIElement)))
                    //    template = UIElementContentTemplate;
                    else
                        template = _DefaultTemplate;
                }

                return template;
            }
        }

        private class StringConverter : IValueConverter
        {
            public object? Convert(object? value, Type targetType, object? parameter, CultureInfo? culture)
            {
                if (value == null)
                    return null;
                if (targetType == typeof(string))
                {
                    if (value is string)
                        return value;
                    var converter = TypeDescriptor.GetConverter(value.GetType());
                    if (converter.CanConvertTo(typeof(string)))
                    {
                        return converter.ConvertToString(value);
                    }
                }
                return null;
            }

            public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo? culture)
            {
                throw new NotSupportedException();
            }
        }


        private class UseContentTemplate : DataTemplate
        {
            public UseContentTemplate()
            {

            }

            protected internal override FrameworkElement? LoadContent(FrameworkElement container, out INameScope? nameScope)
            {
                nameScope = null;
                object? content = ((ContentPresenter)container).Content;
                if (content is FrameworkElement fe)
                    return fe;
                return null;
            }
        }

        #endregion
    }
}
