using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;
using System.Xaml.Markup;

namespace Wodsoft.UI.Controls
{
    public class Control : FrameworkElement
    {
        #region Template

        private ControlTemplate? _template;
        private bool _templateGenerated;
        private FrameworkElement? _templatedContent;
        private INameScope? _templateNameScope;
        public override bool ApplyTemplate()
        {
            OnPreApplyTemplate();

            var result = false;

            if (_template != null && !_templateGenerated)
            {
                if (_templatedContent != null)
                {
                    RemoveVisualChild(_templatedContent);
                }
                _templatedContent = _template.LoadContent(out _templateNameScope);
                if (_templatedContent != null)
                    AddVisualChild(_templatedContent);
                OnApplyTemplate();
            }

            OnPostApplyTemplate();

            return result;
        }

        protected virtual void OnPreApplyTemplate() { }

        protected virtual void OnApplyTemplate() { }

        protected virtual void OnPostApplyTemplate() { }

        protected DependencyObject? GetTemplateChild(string childName)
        {
            if (_templateNameScope == null)
                return null;
            return _templateNameScope.FindName(childName) as DependencyObject;
        }

        #endregion

        #region Layout

        protected override Size MeasureOverride(Size availableSize)
        {
            if (_templatedContent == null)
                return new Size(0.0f, 0.0f);
            _templatedContent.Measure(availableSize);
            return _templatedContent.DesiredSize;
        }

        /// <summary>
        ///     Default control arrangement is to only arrange
        ///     the first visual child. No transforms will be applied.
        /// </summary>
        /// <param name="arrangeBounds">The computed size.</param>
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (_templatedContent != null)
            {
                _templatedContent.Arrange(new Rect(finalSize));
            }
            return finalSize;
        }

        #endregion

        #region Methods

        public override void EndInit()
        {
            if (_template == null)
            {
                PropertyMetadata metadata = TemplateProperty.GetMetadata(GetType());
                ControlTemplate? defaultValue = (ControlTemplate?)metadata.DefaultValue;
                if (defaultValue != null)
                {
                    OnTemplateChanged(this, new DependencyPropertyChangedEventArgs(TemplateProperty, metadata, null, defaultValue));
                }
            }
            base.EndInit();
        }

        #endregion

        #region Properties

        public static readonly DependencyProperty TemplateProperty =
            DependencyProperty.Register(
                "Template",
                typeof(ControlTemplate),
                typeof(Control),
                new FrameworkPropertyMetadata(
                        null,  // default value
                        FrameworkPropertyMetadataOptions.AffectsMeasure,
                        new PropertyChangedCallback(OnTemplateChanged)));
        private static void OnTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Control c = (Control)d;
            c._template = (ControlTemplate?)e.NewValue;
            c._templateGenerated = false;
            //StyleHelper.UpdateTemplateCache(c, (FrameworkTemplate)e.OldValue, (FrameworkTemplate)e.NewValue, TemplateProperty);
        }

        public ControlTemplate? Template
        {
            get { return _template; }
            set { SetValue(TemplateProperty, value); }
        }

        #endregion
    }
}
