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
    public partial class TextBlock
    {
        private class TextBlockBlock : Block, IReadOnlyList<Inline>
        {
            private readonly TextBlock _textBlock;

            public TextBlockBlock(TextBlock textBlock)
            {
                _textBlock = textBlock;
                SetBinding(TextWrappingProperty, new Binding("TextWrapping") { Source = textBlock });
                SetBinding(TextTrimmingProperty, new Binding("TextTrimming") { Source = textBlock });
            }

            private ParagraphLayout? _layout;
            public override IBlockLayout Layout => _layout ??= new ParagraphLayout(this, this);

            int IReadOnlyCollection<Inline>.Count => _textBlock._inlines.Count + 1;

            Inline IReadOnlyList<Inline>.this[int index]
            {
                get
                {
                    if (index == _textBlock._inlines.Count)
                        return _textBlock._textRun;
                    return _textBlock._inlines[index];
                }
            }

            public IEnumerator<Inline> GetEnumerator()
            {
                return new BlockEnumerator(this);
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            private class BlockEnumerator : IEnumerator<Inline>
            {
                private readonly InlineCollection _inlines;
                private readonly Run _run;
                private int _index, _count, _version;

                public BlockEnumerator(TextBlockBlock block)
                {
                    _inlines = block._textBlock._inlines;
                    _run = block._textBlock._textRun;
                    _version = _inlines.Version;
                }

                public Inline Current
                {
                    get
                    {
                        if (_version != _inlines.Version)
                            throw new InvalidOperationException("Inlines changed.");
                        return _index == _count ? _run : _inlines[_index];
                    }
                }

                object IEnumerator.Current => Current;

                public void Dispose()
                {

                }

                public bool MoveNext()
                {
                    if (_version != _inlines.Version)
                        throw new InvalidOperationException("Inlines changed.");
                    if (_index == _inlines.Count)
                        return false;
                    _index++;
                    return true;
                }

                public void Reset()
                {
                    _index = -1;
                }
            }
        }
    }
}
