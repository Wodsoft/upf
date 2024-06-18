using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Controls.Primitives;
using Wodsoft.UI.Data;
using Wodsoft.UI.Documents;
using Wodsoft.UI.Input;

namespace Wodsoft.UI.Controls
{
    public class TextBox : TextBoxBase, ITextHost
    {
        private string _text = string.Empty;
        private int _selectionStart, _selectionLength, _compositionStart, _compositionLength;
        private bool _isComposition, _isEditing;

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
                                    FrameworkPropertyMetadataOptions.Journal | FrameworkPropertyMetadataOptions.AffectsRender,
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
                RaiseEvent(new RoutedEventArgs(SelectionChangedEvent));
            }
        }

        #endregion

        #region DependencyValue

        #endregion

        #region Text Host

        private List<ITextHostLine> _lines = new List<ITextHostLine>();
        IReadOnlyList<ITextHostLine> ITextHost.Lines
        {
            get
            {
                if (_lines.Count == 0)
                {
                    if (_text.Length == 0)
                    {
                        _lines.Add(new TextBoxHostLine(this, 0, 0));
                        return _lines;
                    }
                    var text = _text.AsSpan();
                    int newLine;
                    int start = 0;
                    _lines = new List<ITextHostLine>();
                    while ((newLine = text.IndexOf('\n')) != -1)
                    {
                        _lines.Add(new TextBoxHostLine(this, start, newLine));
                        start += newLine + 1;
                        text = text.Slice(newLine + 1);
                    }
                    _lines.Add(new TextBoxHostLine(this, start, text.Length));
                }
                return _lines;
            }
        }

        private void OnTextChanged(in string oldValue, in string newValue)
        {
            _lines.Clear();
            if (!_isEditing)
            {
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
                _compositionLength = 0;
            }
            TextChangedEventArgs e = new TextChangedEventArgs(TextChangedEvent, UndoAction.None);
            RaiseEvent(e);
        }

        internal ReadOnlySpan<char> GetText() => _text.AsSpan();

        internal int CompositionLength => _compositionLength;

        internal int ComposttionStart => _compositionStart;

        bool ITextHost.IsSelectable => true;

        char ITextHost.GetChar(int position)
        {
            if (_text.Length == 0 || position < 0 || position >= _text.Length)
                return default;
            return _text[position];
        }

        int ITextHost.TextLength => _text.Length;

        #endregion

        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            string text;
            if (e.TextComposition.Stage == TextCompositionStage.Started)
            {
                _isComposition = true;
                text = ReplaceSelection(e.Text);
                _selectionStart += e.Text.Length;
                _selectionLength = 0;
                _compositionStart = e.TextComposition.CaretPosition;
                _compositionLength = e.Text.Length;
                _isEditing = true;
                Text = text;
                _isEditing = false;
            }
            else if (_isComposition || (e.Text.Length == 1 && char.IsLetter(e.Text, 0)))
            {
                if (_compositionLength == _text.Length)
                    text = e.Text;
                else if (_compositionLength == _selectionStart + _compositionLength - _compositionStart)
                    text = e.Text + _text.Substring(_compositionLength);
                else if (_selectionStart + _compositionLength - _compositionStart == _text.Length)
                    text = _text.Substring(0, _text.Length - _compositionLength) + e.Text;
                else
                    text = _text.Substring(0, _selectionStart - _compositionStart) + e.Text + _text.Substring(_selectionStart + _compositionLength - _compositionStart);
                _selectionStart = _selectionStart - _compositionStart + e.TextComposition.CaretPosition;
                _compositionLength = e.Text.Length;
                _compositionStart = e.TextComposition.CaretPosition;
            }
            else
                return;
            _isEditing = true;
            Text = text;
            _isEditing = false;
            if (e.TextComposition.Stage == TextCompositionStage.Completed)
            {
                _isComposition = false;
                _compositionStart = _compositionLength = 0;
            }
        }

        private string ReplaceSelection(string? replaceText)
        {
            string text;
            if (_selectionLength == _text.Length)
                return replaceText ?? string.Empty;
            else if (_selectionStart == 0)
            {
                if (replaceText == null)
                    return _text.Substring(_selectionLength);
                else
                    text = replaceText + _text.Substring(_selectionLength);
            }
            else if (_selectionStart + _selectionLength == _text.Length)
            {
                if (replaceText == null)
                    return _text.Substring(0, _selectionStart);
                else
                    text = _text.Substring(0, _selectionStart) + replaceText;
            }
            else
            {
                if (replaceText == null)
                    text = _text.Remove(_selectionStart, _selectionLength);
                else
                    text = _text.Substring(0, _selectionStart) + replaceText + _text.Substring(_selectionStart + _selectionLength);
            }
            return text;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                string text;
                if (_selectionLength == 0)
                {
                    if (_selectionStart == 0)
                        return;
                    else if (_selectionStart == _text.Length)
                        text = _text.Substring(0, _selectionStart - 1);
                    else
                        text = _text.Remove(_selectionStart - 1, 1);
                    _selectionStart--;
                }
                else
                {
                    text = ReplaceSelection(null);
                    _selectionLength = 0;
                }
                _isEditing = true;
                Text = text;
                _isEditing = false;
                e.Handled = true;
            }
            else if (e.Key == Key.Delete)
            {
                string text;
                if (_selectionLength == 0)
                {
                    if (_selectionStart == _text.Length)
                        return;
                    else if (_selectionStart == 0)
                        text = _text.Substring(1);
                    else
                        text = _text.Remove(_selectionStart, 1);
                }
                else
                {
                    text = ReplaceSelection(null);
                    _selectionLength = 0;
                }
                _isEditing = true;
                Text = text;
                _isEditing = false;
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                string text;
                if (_selectionLength == 0)
                {
                    if (_selectionStart == 0)
                        text = "\n" + _text;
                    else if (_selectionStart == _text.Length)
                        text = _text + "\n";
                    else
                        text = _text.Substring(0, _selectionStart) + "\n" + _text.Substring(_selectionStart);
                }
                else
                {
                    text = ReplaceSelection("\n");
                    _selectionLength = 0;
                }
                _selectionStart++;
                _isEditing = true;
                Text = text;
                _isEditing = false;
                e.Handled = true;
            }
            else if (e.Key == Key.Space)
            {
                string text;
                if (_selectionLength == 0)
                {
                    if (_selectionStart == 0)
                        text = " " + _text;
                    else if (_selectionStart == _text.Length)
                        text = _text + " ";
                    else
                        text = _text.Substring(0, _selectionStart) + " " + _text.Substring(_selectionStart);
                }
                else
                {
                    text = ReplaceSelection(" ");
                    _selectionLength = 0;
                }
                _selectionStart++;
                _isEditing = true;
                Text = text;
                _isEditing = false;
                e.Handled = true;
            }
        }
    }
}
