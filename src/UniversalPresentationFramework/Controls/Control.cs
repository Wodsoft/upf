using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;
using System.Xaml.Markup;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Controls
{
    public class Control : FrameworkElement
    {
        #region Template

        private ControlTemplate? _template;

        protected override FrameworkTemplate? GetTemplate() => _template;

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

        public static readonly DependencyProperty BorderBrushProperty
                = Border.BorderBrushProperty.AddOwner(typeof(Control),
                    new FrameworkPropertyMetadata(
                        Border.BorderBrushProperty.DefaultMetadata.DefaultValue,
                        FrameworkPropertyMetadataOptions.None));
        public Brush? BorderBrush
        {
            get { return (Brush?)GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }

        public static readonly DependencyProperty BorderThicknessProperty
                = Border.BorderThicknessProperty.AddOwner(typeof(Control),
                    new FrameworkPropertyMetadata(
                        Border.BorderThicknessProperty.DefaultMetadata.DefaultValue,
                        FrameworkPropertyMetadataOptions.None));
        public Thickness BorderThickness
        {
            get { return (Thickness)GetValue(BorderThicknessProperty)!; }
            set { SetValue(BorderThicknessProperty, value); }
        }

        public static readonly DependencyProperty BackgroundProperty =
                Panel.BackgroundProperty.AddOwner(typeof(Control),
                    new FrameworkPropertyMetadata(
                        Panel.BackgroundProperty.DefaultMetadata.DefaultValue,
                        FrameworkPropertyMetadataOptions.None));
        public Brush? Background
        {
            get { return (Brush?)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        //public static readonly DependencyProperty ForegroundProperty =
        //        TextElement.ForegroundProperty.AddOwner(
        //                typeof(Control),
        //                new FrameworkPropertyMetadata(SystemColors.ControlTextBrush,
        //                    FrameworkPropertyMetadataOptions.Inherits));
        //public Brush? Foreground
        //{
        //    get { return (Brush?)GetValue(ForegroundProperty); }
        //    set { SetValue(ForegroundProperty, value); }
        //}

        public static readonly DependencyProperty HorizontalContentAlignmentProperty =
                    DependencyProperty.Register(
                                "HorizontalContentAlignment",
                                typeof(HorizontalAlignment),
                                typeof(Control),
                                new FrameworkPropertyMetadata(HorizontalAlignment.Left));
        public HorizontalAlignment HorizontalContentAlignment
        {
            get { return (HorizontalAlignment)GetValue(HorizontalContentAlignmentProperty)!; }
            set { SetValue(HorizontalContentAlignmentProperty, value); }
        }

        public static readonly DependencyProperty VerticalContentAlignmentProperty =
                    DependencyProperty.Register(
                                "VerticalContentAlignment",
                                typeof(VerticalAlignment),
                                typeof(Control),
                                new FrameworkPropertyMetadata(VerticalAlignment.Top));
        public VerticalAlignment VerticalContentAlignment
        {
            get { return (VerticalAlignment)GetValue(VerticalContentAlignmentProperty)!; }
            set { SetValue(VerticalContentAlignmentProperty, value); }
        }

        public static readonly DependencyProperty PaddingProperty
            = DependencyProperty.Register("Padding",
                                        typeof(Thickness), typeof(Control),
                                        new FrameworkPropertyMetadata(
                                                new Thickness(),
                                                FrameworkPropertyMetadataOptions.AffectsParentMeasure));
        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty)!; }
            set { SetValue(PaddingProperty, value); }
        }

        #endregion
    }
}
