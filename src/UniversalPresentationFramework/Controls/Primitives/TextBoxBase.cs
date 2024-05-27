using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Input;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Controls.Primitives
{
    [TemplatePart(Name = "PART_TextHost", Type = typeof(TextHolder))]
    public abstract class TextBoxBase : Control
    {
        #region Properties

        internal static readonly DependencyProperty IsReadOnlyProperty =
                DependencyProperty.RegisterAttached(
                        "IsReadOnly",
                        typeof(bool),
                        typeof(TextBoxBase),
                        new FrameworkPropertyMetadata(
                                false,
                                FrameworkPropertyMetadataOptions.Inherits));
        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty)!; }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public static readonly DependencyProperty IsReadOnlyCaretVisibleProperty =
            DependencyProperty.Register("IsReadOnlyCaretVisible",
                typeof(bool),
                typeof(TextBoxBase),
                new FrameworkPropertyMetadata(false));
        public bool IsReadOnlyCaretVisible
        {
            get { return (bool)GetValue(IsReadOnlyCaretVisibleProperty)!; }
            set { SetValue(IsReadOnlyCaretVisibleProperty, value); }
        }

        public static readonly DependencyProperty AcceptsReturnProperty = KeyboardNavigation.AcceptsReturnProperty.AddOwner(typeof(TextBoxBase));
        public bool AcceptsReturn
        {
            get { return (bool)GetValue(AcceptsReturnProperty)!; }
            set { SetValue(AcceptsReturnProperty, value); }
        }

        public static readonly DependencyProperty AcceptsTabProperty =
                DependencyProperty.Register(
                        "AcceptsTab", // Property name
                        typeof(bool), // Property type
                        typeof(TextBoxBase), // Property owner
                        new FrameworkPropertyMetadata(false /*default value*/));
        public bool AcceptsTab
        {
            get { return (bool)GetValue(AcceptsTabProperty)!; }
            set { SetValue(AcceptsTabProperty, value); }
        }

        public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty = ScrollViewer.HorizontalScrollBarVisibilityProperty.AddOwner(typeof(TextBoxBase),
            new FrameworkPropertyMetadata(ScrollBarVisibility.Hidden));
        public ScrollBarVisibility HorizontalScrollBarVisibility
        {
            get { return (ScrollBarVisibility)GetValue(HorizontalScrollBarVisibilityProperty)!; }
            set { SetValue(HorizontalScrollBarVisibilityProperty, value); }
        }

        public static readonly DependencyProperty VerticalScrollBarVisibilityProperty = ScrollViewer.VerticalScrollBarVisibilityProperty.AddOwner(typeof(TextBoxBase),
            new FrameworkPropertyMetadata(ScrollBarVisibility.Hidden));
        public ScrollBarVisibility VerticalScrollBarVisibility
        {
            get { return (ScrollBarVisibility)GetValue(VerticalScrollBarVisibilityProperty)!; }
            set { SetValue(VerticalScrollBarVisibilityProperty, value); }
        }

        public static readonly DependencyProperty SelectionBrushProperty =
            DependencyProperty.Register("SelectionBrush", typeof(Brush), typeof(TextBoxBase),
                new FrameworkPropertyMetadata(new SolidColorBrush(SystemColors.HighlightColor)));
        public Brush? SelectionBrush
        {
            get { return (Brush?)GetValue(SelectionBrushProperty); }
            set { SetValue(SelectionBrushProperty, value); }
        }

        public static readonly DependencyProperty SelectionTextBrushProperty =
            DependencyProperty.Register("SelectionTextBrush", typeof(Brush), typeof(TextBoxBase),
                new FrameworkPropertyMetadata(new SolidColorBrush(SystemColors.HighlightTextColor)));
        public Brush? SelectionTextBrush
        {
            get { return (Brush?)GetValue(SelectionTextBrushProperty); }
            set { SetValue(SelectionTextBrushProperty, value); }
        }

        public static readonly DependencyProperty SelectionOpacityProperty =
            DependencyProperty.Register("SelectionOpacity", typeof(float), typeof(TextBoxBase),
                new FrameworkPropertyMetadata(1f));
        public float SelectionOpacity
        {
            get { return (float)GetValue(SelectionOpacityProperty)!; }
            set { SetValue(SelectionOpacityProperty, value); }
        }

        public static readonly DependencyProperty CaretBrushProperty =
            DependencyProperty.Register("CaretBrush", typeof(Brush), typeof(TextBoxBase),
                new FrameworkPropertyMetadata(null));
        public Brush? CaretBrush
        {
            get { return (Brush?)GetValue(CaretBrushProperty); }
            set { SetValue(CaretBrushProperty, value); }
        }


        #endregion
    }
}
