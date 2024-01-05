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

        protected override FrameworkTemplate? GetTemplate() => _template;

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
            {
                TemplatedChild.Arrange(new Rect(finalSize));
            }
            return finalSize;
        }

        #endregion

        #region Methods

        protected override void EndInit()
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
            c.OnTemplateChanged();
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
