using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Controls
{
    public class HeaderedContentControl : ContentControl
    {
        #region Constructors

        static HeaderedContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HeaderedContentControl), new FrameworkPropertyMetadata(typeof(HeaderedContentControl)));
        }

        #endregion

        #region Properties

        /// <summary>
        ///     The DependencyProperty for the Header property.
        ///     Flags:              None
        ///     Default Value:      null
        /// </summary>
        public static readonly DependencyProperty HeaderProperty =
                DependencyProperty.Register(
                        "Header",
                        typeof(object),
                        typeof(HeaderedContentControl),
                        new FrameworkPropertyMetadata(
                                null,
                                new PropertyChangedCallback(OnHeaderChanged)));


        /// <summary>
        ///     Header is the data used to for the header of each item in the control.
        /// </summary>
        [Bindable(true), Category("Content")]
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
            HeaderedContentControl ctrl = (HeaderedContentControl)d;

            ctrl.SetValue(HasHeaderPropertyKey, (e.NewValue != null));
            ctrl.OnHeaderChanged(e.OldValue, e.NewValue);
        }

        /// <summary>
        ///     This method is invoked when the Header property changes.
        /// </summary>
        /// <param name="oldHeader">The old value of the Header property.</param>
        /// <param name="newHeader">The new value of the Header property.</param>
        protected virtual void OnHeaderChanged(object? oldHeader, object? newHeader)
        {
            if (oldHeader is LogicalObject oldLogical)
                RemoveLogicalChild(oldLogical);
            if (newHeader is LogicalObject newLogical)
                AddLogicalChild(newLogical);
        }

        /// <summary>
        ///     The key needed set a read-only property.
        /// </summary>
        internal static readonly DependencyPropertyKey HasHeaderPropertyKey =
                DependencyProperty.RegisterReadOnly(
                        "HasHeader",
                        typeof(bool),
                        typeof(HeaderedContentControl),
                        new FrameworkPropertyMetadata(false));

        /// <summary>
        ///     The DependencyProperty for the HasHeader property.
        ///     Flags:              None
        ///     Other:              Read-Only
        ///     Default Value:      false
        /// </summary>
        public static readonly DependencyProperty HasHeaderProperty =
                HasHeaderPropertyKey.DependencyProperty;

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
                DependencyProperty.Register(
                        "HeaderTemplate",
                        typeof(DataTemplate),
                        typeof(HeaderedContentControl),
                        new FrameworkPropertyMetadata(
                                null,
                                new PropertyChangedCallback(OnHeaderTemplateChanged)));

        /// <summary>
        ///     HeaderTemplate is the template used to display the <seealso cref="Header"/>.
        /// </summary>
        [Bindable(true), Category("Content")]
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
            HeaderedContentControl ctrl = (HeaderedContentControl)d;
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
                DependencyProperty.Register(
                        "HeaderTemplateSelector",
                        typeof(DataTemplateSelector),
                        typeof(HeaderedContentControl),
                        new FrameworkPropertyMetadata(
                                null,
                                new PropertyChangedCallback(OnHeaderTemplateSelectorChanged)));

        /// <summary>
        ///     HeaderTemplateSelector allows the application writer to provide custom logic
        ///     for choosing the template used to display the <seealso cref="Header"/>.
        /// </summary>
        /// <remarks>
        ///     This property is ignored if <seealso cref="HeaderTemplate"/> is set.
        /// </remarks>
        [Bindable(true), Category("Content")]
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
            HeaderedContentControl ctrl = (HeaderedContentControl)d;

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
                        typeof(HeaderedContentControl),
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
            HeaderedContentControl ctrl = (HeaderedContentControl)d;
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
    }
}
