using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Controls.Primitives;
using Wodsoft.UI.Data;
using Wodsoft.UI.Documents;

namespace Wodsoft.UI.Controls
{
    public class TextBox : TextBoxBase, ITextHost
    {
        private string _text = string.Empty;

        static TextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBox), new FrameworkPropertyMetadata(typeof(TextBox)));
        }

        #region Properties

        public static readonly DependencyProperty TextWrappingProperty =
                TextBlock.TextWrappingProperty.AddOwner(
                        typeof(TextBox),
                        new FrameworkPropertyMetadata(TextWrapping.NoWrap));
        public TextWrapping TextWrapping
        {
            get => (TextWrapping)GetValue(TextWrappingProperty)!;
            set => SetValue(TextWrappingProperty, value);
        }

        public static readonly DependencyProperty MinLinesProperty =
                DependencyProperty.Register(
                        "MinLines", // Property name
                        typeof(int), // Property type
                        typeof(TextBox), // Property owner
                        new FrameworkPropertyMetadata(
                                1,
                                FrameworkPropertyMetadataOptions.AffectsMeasure),
                        new ValidateValueCallback(MinLinesValidateValue));
        private static bool MinLinesValidateValue(object? value)
        {
            return (int)value! > 0;
        }
        public int MinLines
        {
            get { return (int)GetValue(MinLinesProperty)!; }
            set { SetValue(MinLinesProperty, value); }
        }

        public static readonly DependencyProperty MaxLinesProperty =
                DependencyProperty.Register(
                        "MaxLines", // Property name
                        typeof(int), // Property type
                        typeof(TextBox), // Property owner
                        new FrameworkPropertyMetadata(
                                int.MaxValue,
                                FrameworkPropertyMetadataOptions.AffectsMeasure),
                        new ValidateValueCallback(MaxLinesValidateValue));
        private static bool MaxLinesValidateValue(object? value)
        {
            return (int)value! > 0;
        }
        public int MaxLines
        {
            get { return (int)GetValue(MaxLinesProperty)!; }
            set { SetValue(MaxLinesProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
                DependencyProperty.Register(
                        "Text", // Property name
                        typeof(string), // Property type
                        typeof(TextBox), // Property owner
                        new FrameworkPropertyMetadata( // Property metadata
                                string.Empty, // default value
                                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | // Flags
                                    FrameworkPropertyMetadataOptions.Journal,
                                new PropertyChangedCallback(OnTextPropertyChanged),    // property changed callback
                                new CoerceValueCallback(CoerceText),
                                true));
        private static object CoerceText(DependencyObject d, object? value)
        {
            if (value == null)
                return string.Empty;
            return value;
        }
        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = (TextBox)d;
            string newValue = (string?)e.NewValue ?? string.Empty;
            string oldValue = textBox._text;
            textBox._text = newValue;
            textBox.OnTextChanged(oldValue, newValue);
        }
        public string Text
        {
            get { return (string)GetValue(TextProperty)!; }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty CharacterCasingProperty =
                DependencyProperty.Register(
                        "CharacterCasing", // Property name
                        typeof(CharacterCasing), // Property type
                        typeof(TextBox), // Property owner
                        new FrameworkPropertyMetadata(CharacterCasing.Normal));
        public CharacterCasing CharacterCasing
        {
            get { return (CharacterCasing)GetValue(CharacterCasingProperty)!; }
            set { SetValue(CharacterCasingProperty, value); }
        }

        public static readonly DependencyProperty MaxLengthProperty =
                DependencyProperty.Register(
                    "MaxLength", // Property name
                    typeof(int), // Property type
                    typeof(TextBox), // Property owner
                    new FrameworkPropertyMetadata(0), /*default value*/
                    new ValidateValueCallback(MaxLengthValidateValue));
        private static bool MaxLengthValidateValue(object? value)
        {
            return ((int)value!) >= 0;
        }
        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty)!; }
            set { SetValue(MaxLengthProperty, value); }
        }

        public static readonly DependencyProperty TextAlignmentProperty = Block.TextAlignmentProperty.AddOwner(typeof(TextBox));
        public TextAlignment TextAlignment
        {
            get => (TextAlignment)GetValue(TextAlignmentProperty)!;
            set => SetValue(TextAlignmentProperty, value);
        }

        public int LineCount => throw new NotImplementedException();


        #endregion

        #region Selection

        private int _selectionStart, _selectionLength;

        public ReadOnlySpan<char> SelectedText
        {
            get => _text.AsSpan().Slice(_selectionStart, _selectionLength);
            set { }
        }

        public int SelectionLength
        {
            get => _selectionLength;
            set => Select(_selectionStart, value);
        }

        public int SelectionStart
        {
            get => _selectionStart;
            set => Select(value, _selectionLength);
        }

        public int CaretIndex
        {
            get => SelectionStart;
            set => Select(value, 0);
        }

        public void Select(int start, int length)
        {
            if (start < 0)
                throw new ArgumentOutOfRangeException(nameof(start), "Parameter can't be negative.");
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), "Parameter can't be negative.");
            if (start == _selectionStart && length == _selectionLength)
                return;
            if (start + length > _text.Length)
                length = _text.Length - start;
            if (start > _text.Length)
                start = _text.Length;
            if (length < 0)
                length = 0;
            bool changed = _selectionStart != start || _selectionLength != length;
            if (changed)
            {
                _selectionStart = start;
                _selectionLength = length;
                _selectionChangedDelegate?.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Text Host

        private EventHandler? _textChangedDelegate, _selectionChangedDelegate;
        private List<ITextHostLine> _lines = new List<ITextHostLine>();
        IReadOnlyList<ITextHostLine> ITextHost.Lines
        {
            get
            {
                if (_lines.Count == 0)
                {
                    var text = Text.AsSpan();
                    int newLine;
                    int start = 0;
                    _lines = new List<ITextHostLine>();
                    while ((newLine = text.IndexOf('\n')) != -1)
                    {
                        _lines.Add(new TextBoxHostLine(this, start, newLine));
                        start += newLine + 1;
                        text = text.Slice(newLine + 1);
                    }
                    if (text.Length != 0)
                        _lines.Add(new TextBoxHostLine(this, start, text.Length));
                }
                return _lines;
            }
        }

        private void OnTextChanged(in string oldValue, in string newValue)
        {
            _lines.Clear();
            var selectionStart = _selectionStart;
            var selectionLength = _selectionLength;
            if (selectionStart + selectionLength > newValue.Length)
                selectionLength = _text.Length - selectionStart;
            if (selectionStart > _text.Length)
                selectionStart = _text.Length;
            if (selectionLength < 0)
                selectionLength = 0;
            bool selectionChanged = selectionStart != _selectionStart || selectionLength != _selectionLength;
            if (selectionChanged)
            {
                _selectionStart = selectionStart;
                _selectionLength = selectionLength;
            }
            _textChangedDelegate?.Invoke(this, EventArgs.Empty);
        }

        internal ReadOnlySpan<char> GetText() => _text.AsSpan();

        event EventHandler ITextHost.TextChanged
        {
            add
            {
                if (_textChangedDelegate == null)
                    _textChangedDelegate = value;
                else
                    _textChangedDelegate = (EventHandler)Delegate.Combine(_textChangedDelegate, value);
            }
            remove
            {
                _textChangedDelegate = (EventHandler?)Delegate.Remove(_textChangedDelegate, value);
            }
        }

        event EventHandler ITextHost.SelectionChanged
        {
            add
            {
                if (_selectionChangedDelegate == null)
                    _selectionChangedDelegate = value;
                else
                    _selectionChangedDelegate = (EventHandler)Delegate.Combine(_selectionChangedDelegate, value);
            }
            remove
            {
                _selectionChangedDelegate = (EventHandler?)Delegate.Remove(_selectionChangedDelegate, value);
            }
        }

        bool ITextHost.IsSelectable => true;

        #endregion
    }
}
