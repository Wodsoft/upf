using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Documents;

namespace Wodsoft.UI.Test
{
    public class TextPointerTest
    {
        [Fact]
        public void TextPointerComparerTest()
        {
            ReadOnlyTextContainer container = new ReadOnlyTextContainer();
            Run run = new Run("text");
            Bold bold = new Bold(run);
            Paragraph paragraph = new Paragraph();
            var leftRun = new Run("left");
            var rightRun = new Run("right");
            paragraph.Inlines.Add(leftRun);
            paragraph.Inlines.Add(bold);
            paragraph.Inlines.Add(rightRun);
            container.Root.InsertNodeAt(paragraph.TextElementNode, ElementEdge.AfterStart);

            Assert.Null(leftRun.PreviousInline);
            Assert.Equal(leftRun.NextInline, bold);
            Assert.Equal(leftRun, bold.PreviousInline);
            Assert.Equal(bold.NextInline, rightRun);
            Assert.Equal(bold, rightRun.PreviousInline);
            Assert.Null(rightRun.NextInline);

            Assert.True(paragraph.ElementStart < bold.ElementEnd);
            Assert.True(paragraph.ElementStart < run.ElementEnd);
            Assert.True(paragraph.ElementStart < paragraph.ElementEnd);
            Assert.True(bold.ElementEnd > paragraph.ElementStart);
            Assert.True(run.ElementEnd > paragraph.ElementStart);
            Assert.True(paragraph.ElementEnd > paragraph.ElementStart);

            Assert.True(run.ContentEnd < bold.ElementEnd);
            Assert.True(run.ContentEnd < rightRun.ContentStart);
            Assert.True(run.ContentEnd > leftRun.ContentStart);
            Assert.True(leftRun.ContentStart < run.ContentEnd);
        }
    }
}
