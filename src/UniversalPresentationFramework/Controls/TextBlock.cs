using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Controls
{
    public class TextBlock : FrameworkElement
    {
        public static readonly DependencyProperty TextProperty =
                DependencyProperty.Register(
                        "Text",
                        typeof(string),
                        typeof(TextBlock),
                        new FrameworkPropertyMetadata(
                                string.Empty,
                                FrameworkPropertyMetadataOptions.AffectsMeasure |
                                FrameworkPropertyMetadataOptions.AffectsRender,
                                new PropertyChangedCallback(OnTextChanged),
                                new CoerceValueCallback(CoerceText)));

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OnTextChanged((TextBlock)d, (string?)e.NewValue);
        }

        private static object CoerceText(DependencyObject d, object? value)
        {
            TextBlock textblock = (TextBlock)d;

            if (value == null)
            {
                value = String.Empty;
            }

            //if (textblock._complexContent != null &&
            //    !textblock.CheckFlags(Flags.TextContentChanging) &&
            //    (string)value == (string)textblock.GetValue(TextProperty))
            //{
            //    // If the new value equals the old value, then the property
            //    // system will optimize out the call to OnTextChanged.  We can't
            //    // skip this call because there's ambiguity between the TextProperty
            //    // view of content and actual content -- we might have a new
            //    // value even if strings match.
            //    //
            //    // E.g.: content = <Image/>, TextProperty == " "
            //    // Now setting TextProperty = " " really changes content, replacing
            //    // the Image with a space char.
            //    OnTextChanged(d, (string)value);
            //}

            return value;
        }

        public string? Text
        {
            get { return (string?)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private static void OnTextChanged(TextBlock text, string? newText)
        {
            //if (text.CheckFlags(Flags.TextContentChanging))
            //{
            //    // The update originated in a TextContainer change -- don't update
            //    // the TextContainer a second time.
            //    return;
            //}

            //if (text._complexContent == null)
            //{
            //    text._contentCache = (newText != null) ? newText : String.Empty;
            //}
            //else
            //{
            //    text.SetFlags(true, Flags.TextContentChanging);
            //    try
            //    {
            //        bool exceptionThrown = true;

            //        Invariant.Assert(text._contentCache == null, "Content cache should be null when complex content exists.");

            //        text._complexContent.TextContainer.BeginChange();
            //        try
            //        {
            //            ((TextContainer)text._complexContent.TextContainer).DeleteContentInternal((TextPointer)text._complexContent.TextContainer.Start, (TextPointer)text._complexContent.TextContainer.End);
            //            InsertTextRun(text._complexContent.TextContainer.End, newText, /*whitespacesIgnorable:*/true);
            //            exceptionThrown = false;
            //        }
            //        finally
            //        {
            //            text._complexContent.TextContainer.EndChange();

            //            if (exceptionThrown)
            //            {
            //                text.ClearLineMetrics();
            //            }
            //        }
            //    }
            //    finally
            //    {
            //        text.SetFlags(false, Flags.TextContentChanging);
            //    }
            //}
        }
    }
}
