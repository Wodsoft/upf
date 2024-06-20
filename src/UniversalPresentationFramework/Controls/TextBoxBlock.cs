using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Controls.Primitives;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Controls
{
    public partial class TextBox
    {
        private class TextBoxBlock : ITextOwnerBlock
        {
            private readonly TextBox _textBox;
            private readonly int _length;
            private ITextOwnerInline? _firstInline, _lastInline;
            private GlyphTypeface? _typeface;
            private bool _typefaceLoaded;

            public TextBoxBlock(TextBox textBox, int length)
            {
                _textBox = textBox;
                _length = length;
            }

            public TextBox TextBox => _textBox;

            public float LineHeight => float.NaN;

            public float Baseline => 0f;

            public int Position => 0;

            public int Length => _length;

            public ITextOwnerInline FirstInline
            {
                get
                {
                    if (_firstInline == null)
                    {
                        var index = _textBox._text.AsSpan().IndexOf('\n');
                        if (index == -1)
                            _lastInline = _firstInline = new TextBoxInline(this, 0, _length, false);
                        else
                            _firstInline = new TextBoxInline(this, 0, index, true);
                    }
                    return _firstInline;
                }
            }

            public ITextOwnerInline LastInline
            {
                get
                {
                    if (_lastInline == null)
                    {
                        var index = _textBox._text.AsSpan().LastIndexOf('\n');
                        if (index == -1)
                            _lastInline = _firstInline = new TextBoxInline(this, 0, _length, false);
                        else
                            _lastInline = new TextBoxInline(this, index + 1, _length - index - 1, false);
                    }
                    return _lastInline;
                }
            }

            public ITextOwnerBlock? PreviousBlock => null;

            public ITextOwnerBlock? NextBlock => null;

            public GlyphTypeface? GetTypeface()
            {
                if (!_typefaceLoaded)
                {
                    _typefaceLoaded = true;
                    var font = _textBox.FontFamily;
                    if (font != null)
                        _typeface = font.GetGlyphTypeface(_textBox.FontStyle, _textBox.FontWeight, _textBox.FontStretch);
                }
                return _typeface;
            }
        }
    }
}
