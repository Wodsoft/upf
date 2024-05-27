using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Controls
{
    public class ScrollViewer : ContentControl
    {
        #region Properties

        public static readonly DependencyProperty CanContentScrollProperty =
                DependencyProperty.RegisterAttached(
                        "CanContentScroll",
                        typeof(bool),
                        typeof(ScrollViewer),
                        new FrameworkPropertyMetadata(false));
        public static void SetCanContentScroll(DependencyObject element, bool canContentScroll)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            element.SetValue(CanContentScrollProperty, canContentScroll);
        }
        public static bool GetCanContentScroll(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return (bool)element.GetValue(CanContentScrollProperty)!;
        }
        public bool CanContentScroll
        {
            get { return (bool)GetValue(CanContentScrollProperty)!; }
            set { SetValue(CanContentScrollProperty, value); }
        }

        public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty =
                DependencyProperty.RegisterAttached(
                        "HorizontalScrollBarVisibility",
                        typeof(ScrollBarVisibility),
                        typeof(ScrollViewer),
                        new FrameworkPropertyMetadata(
                                ScrollBarVisibility.Disabled,
                                FrameworkPropertyMetadataOptions.AffectsMeasure),
                        new ValidateValueCallback(IsValidScrollBarVisibility));
        public static void SetHorizontalScrollBarVisibility(DependencyObject element, ScrollBarVisibility horizontalScrollBarVisibility)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            element.SetValue(HorizontalScrollBarVisibilityProperty, horizontalScrollBarVisibility);
        }
        public static ScrollBarVisibility GetHorizontalScrollBarVisibility(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return (ScrollBarVisibility)element.GetValue(HorizontalScrollBarVisibilityProperty)!;
        }
        public ScrollBarVisibility HorizontalScrollBarVisibility
        {
            get { return (ScrollBarVisibility)GetValue(HorizontalScrollBarVisibilityProperty)!; }
            set { SetValue(HorizontalScrollBarVisibilityProperty, value); }
        }

        public static readonly DependencyProperty VerticalScrollBarVisibilityProperty =
                DependencyProperty.RegisterAttached(
                        "VerticalScrollBarVisibility",
                        typeof(ScrollBarVisibility),
                        typeof(ScrollViewer),
                        new FrameworkPropertyMetadata(
                                ScrollBarVisibility.Visible,
                                FrameworkPropertyMetadataOptions.AffectsMeasure),
                        new ValidateValueCallback(IsValidScrollBarVisibility));
        public static void SetVerticalScrollBarVisibility(DependencyObject element, ScrollBarVisibility verticalScrollBarVisibility)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            element.SetValue(VerticalScrollBarVisibilityProperty, verticalScrollBarVisibility);
        }
        public static ScrollBarVisibility GetVerticalScrollBarVisibility(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return (ScrollBarVisibility)element.GetValue(VerticalScrollBarVisibilityProperty)!;
        }
        public ScrollBarVisibility VerticalScrollBarVisibility
        {
            get { return (ScrollBarVisibility)GetValue(VerticalScrollBarVisibilityProperty)!; }
            set { SetValue(VerticalScrollBarVisibilityProperty, value); }
        }

        private static bool IsValidScrollBarVisibility(object? o)
        {
            ScrollBarVisibility value = (ScrollBarVisibility)o!;
            return (value == ScrollBarVisibility.Disabled
                || value == ScrollBarVisibility.Auto
                || value == ScrollBarVisibility.Hidden
                || value == ScrollBarVisibility.Visible);
        }

        private static readonly DependencyPropertyKey _HorizontalOffsetPropertyKey =
            DependencyProperty.RegisterReadOnly(
                        "HorizontalOffset",
                        typeof(float),
                        typeof(ScrollViewer),
                        new FrameworkPropertyMetadata(0f));
        public static readonly DependencyProperty HorizontalOffsetProperty = _HorizontalOffsetPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey _VerticalOffsetPropertyKey =
            DependencyProperty.RegisterReadOnly(
                        "VerticalOffset",
                        typeof(float),
                        typeof(ScrollViewer),
                        new FrameworkPropertyMetadata(0f));
        public static readonly DependencyProperty VerticalOffsetProperty = _VerticalOffsetPropertyKey.DependencyProperty;

        #endregion
    }
}
