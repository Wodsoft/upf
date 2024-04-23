using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Test
{
    public class ObjectSpansTest
    {
        [Fact]
        public void FullTest()
        {
            var objectSpans = new ObjectSpans<int>(10, 0);
            var spans = objectSpans.ToList();
            Assert.Single(spans);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(9, spans[0].End);
            Assert.Equal(0, spans[0].Value);

            objectSpans.SetValue(2, 4, _ => 1);
            spans = objectSpans.ToList();
            Assert.Equal(3, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(1, spans[0].End);
            Assert.Equal(0, spans[0].Value);
            Assert.Equal(2, spans[1].Start);
            Assert.Equal(4, spans[1].End);
            Assert.Equal(1, spans[1].Value);
            Assert.Equal(5, spans[2].Start);
            Assert.Equal(9, spans[2].End);
            Assert.Equal(0, spans[2].Value);

            objectSpans.SetValue(7, 9, _ => 2);
            spans = objectSpans.ToList();
            Assert.Equal(4, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(1, spans[0].End);
            Assert.Equal(0, spans[0].Value);
            Assert.Equal(2, spans[1].Start);
            Assert.Equal(4, spans[1].End);
            Assert.Equal(1, spans[1].Value);
            Assert.Equal(5, spans[2].Start);
            Assert.Equal(6, spans[2].End);
            Assert.Equal(0, spans[2].Value);
            Assert.Equal(7, spans[3].Start);
            Assert.Equal(9, spans[3].End);
            Assert.Equal(2, spans[3].Value);

            objectSpans.SetValue(4, 7, _ => 3);
            spans = objectSpans.ToList();
            Assert.Equal(4, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(1, spans[0].End);
            Assert.Equal(0, spans[0].Value);
            Assert.Equal(2, spans[1].Start);
            Assert.Equal(3, spans[1].End);
            Assert.Equal(1, spans[1].Value);
            Assert.Equal(4, spans[2].Start);
            Assert.Equal(7, spans[2].End);
            Assert.Equal(3, spans[2].Value);
            Assert.Equal(8, spans[3].Start);
            Assert.Equal(9, spans[3].End);
            Assert.Equal(2, spans[3].Value);

            objectSpans.SetValue(2, 3, _ => 3);
            spans = objectSpans.ToList();
            Assert.Equal(3, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(1, spans[0].End);
            Assert.Equal(0, spans[0].Value);
            Assert.Equal(2, spans[1].Start);
            Assert.Equal(7, spans[1].End);
            Assert.Equal(3, spans[1].Value);
            Assert.Equal(8, spans[2].Start);
            Assert.Equal(9, spans[2].End);
            Assert.Equal(2, spans[2].Value);

            objectSpans.SetValue(4, 9, _ => 3);
            spans = objectSpans.ToList();
            Assert.Equal(2, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(1, spans[0].End);
            Assert.Equal(0, spans[0].Value);
            Assert.Equal(2, spans[1].Start);
            Assert.Equal(9, spans[1].End);
            Assert.Equal(3, spans[1].Value);

            objectSpans.SetValue(0, 5, _ => 3);
            spans = objectSpans.ToList();
            Assert.Single(spans);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(9, spans[0].End);
            Assert.Equal(3, spans[0].Value);
        }

        [Fact]
        public void DefaultSet()
        {
            var objectSpans = new ObjectSpans<int>(10, 0);
            var spans = objectSpans.ToList();
            Assert.Single(spans);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(9, spans[0].End);
            Assert.Equal(0, spans[0].Value);

            objectSpans.SetValue(2, 7, _ => 0);
            Assert.Single(spans);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(9, spans[0].End);
            Assert.Equal(0, spans[0].Value);
        }

        [Fact]
        public void LeftOutSet()
        {
            var objectSpans = new ObjectSpans<int>(10, 0);
            var spans = objectSpans.ToList();
            Assert.Single(spans);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(9, spans[0].End);
            Assert.Equal(0, spans[0].Value);

            objectSpans.SetValue(5, 9, _ => 1);
            spans = objectSpans.ToList();
            Assert.Equal(2, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(4, spans[0].End);
            Assert.Equal(0, spans[0].Value);
            Assert.Equal(5, spans[1].Start);
            Assert.Equal(9, spans[1].End);
            Assert.Equal(1, spans[1].Value);

            objectSpans.SetValue(0, 4, _ => 2);
            spans = objectSpans.ToList();
            Assert.Equal(2, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(4, spans[0].End);
            Assert.Equal(2, spans[0].Value);
            Assert.Equal(5, spans[1].Start);
            Assert.Equal(9, spans[1].End);
            Assert.Equal(1, spans[1].Value);
        }

        [Fact]
        public void LeftOverrideSet()
        {
            var objectSpans = new ObjectSpans<(int A, int B)>(10, (0, 0));
            var spans = objectSpans.ToList();
            Assert.Single(spans);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(9, spans[0].End);
            Assert.Equal((0, 0), spans[0].Value);

            objectSpans.SetValue(6, 9, v => (1, v.B));
            spans = objectSpans.ToList();
            Assert.Equal(2, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(5, spans[0].End);
            Assert.Equal((0, 0), spans[0].Value);
            Assert.Equal(6, spans[1].Start);
            Assert.Equal(9, spans[1].End);
            Assert.Equal((1, 0), spans[1].Value);

            objectSpans.SetValue(5, 9, v => (1, v.B));
            spans = objectSpans.ToList();
            Assert.Equal(2, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(4, spans[0].End);
            Assert.Equal((0, 0), spans[0].Value);
            Assert.Equal(5, spans[1].Start);
            Assert.Equal(9, spans[1].End);
            Assert.Equal((1, 0), spans[1].Value);

            objectSpans.SetValue(0, 9, v => (v.A, 1));
            spans = objectSpans.ToList();
            Assert.Equal(2, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(4, spans[0].End);
            Assert.Equal((0, 1), spans[0].Value);
            Assert.Equal(5, spans[1].Start);
            Assert.Equal(9, spans[1].End);
            Assert.Equal((1, 1), spans[1].Value);
        }

        [Fact]
        public void LeftFillSet()
        {
            var objectSpans = new ObjectSpans<int>(10, 0);
            var spans = objectSpans.ToList();
            Assert.Single(spans);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(9, spans[0].End);
            Assert.Equal(0, spans[0].Value);

            objectSpans.SetValue(2, 3, _ => 1);
            objectSpans.SetValue(6, 7, _ => 1);
            spans = objectSpans.ToList();
            Assert.Equal(5, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(1, spans[0].End);
            Assert.Equal(0, spans[0].Value);
            Assert.Equal(2, spans[1].Start);
            Assert.Equal(3, spans[1].End);
            Assert.Equal(1, spans[1].Value);
            Assert.Equal(4, spans[2].Start);
            Assert.Equal(5, spans[2].End);
            Assert.Equal(0, spans[2].Value);
            Assert.Equal(6, spans[3].Start);
            Assert.Equal(7, spans[3].End);
            Assert.Equal(1, spans[3].Value);
            Assert.Equal(8, spans[4].Start);
            Assert.Equal(9, spans[4].End);
            Assert.Equal(0, spans[4].Value);

            objectSpans.SetValue(2, 7, _ => 1);
            spans = objectSpans.ToList();
            Assert.Equal(3, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(1, spans[0].End);
            Assert.Equal(0, spans[0].Value);
            Assert.Equal(2, spans[1].Start);
            Assert.Equal(7, spans[1].End);
            Assert.Equal(1, spans[1].Value);
            Assert.Equal(8, spans[2].Start);
            Assert.Equal(9, spans[2].End);
            Assert.Equal(0, spans[2].Value);
        }

        [Fact]
        public void LeftFillSet2()
        {
            var objectSpans = new ObjectSpans<(int, int)>(10, (0, 0));
            var spans = objectSpans.ToList();
            Assert.Single(spans);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(9, spans[0].End);
            Assert.Equal((0, 0), spans[0].Value);

            objectSpans.SetValue(2, 3, _ => (1, 0));
            objectSpans.SetValue(6, 7, _ => (1, 0));
            spans = objectSpans.ToList();
            Assert.Equal(5, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(1, spans[0].End);
            Assert.Equal((0, 0), spans[0].Value);
            Assert.Equal(2, spans[1].Start);
            Assert.Equal(3, spans[1].End);
            Assert.Equal((1, 0), spans[1].Value);
            Assert.Equal(4, spans[2].Start);
            Assert.Equal(5, spans[2].End);
            Assert.Equal((0, 0), spans[2].Value);
            Assert.Equal(6, spans[3].Start);
            Assert.Equal(7, spans[3].End);
            Assert.Equal((1, 0), spans[3].Value);
            Assert.Equal(8, spans[4].Start);
            Assert.Equal(9, spans[4].End);
            Assert.Equal((0, 0), spans[4].Value);

            objectSpans.SetValue(2, 7, v => (v.Item1, 1));
            spans = objectSpans.ToList();
            Assert.Equal(5, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(1, spans[0].End);
            Assert.Equal((0, 0), spans[0].Value);
            Assert.Equal(2, spans[1].Start);
            Assert.Equal(3, spans[1].End);
            Assert.Equal((1, 1), spans[1].Value);
            Assert.Equal(4, spans[2].Start);
            Assert.Equal(5, spans[2].End);
            Assert.Equal((0, 1), spans[2].Value);
            Assert.Equal(6, spans[3].Start);
            Assert.Equal(7, spans[3].End);
            Assert.Equal((1, 1), spans[3].Value);
            Assert.Equal(8, spans[4].Start);
            Assert.Equal(9, spans[4].End);
            Assert.Equal((0, 0), spans[4].Value);
        }

        [Fact]
        public void LeftFillReset()
        {
            var objectSpans = new ObjectSpans<int>(10, 0);
            var spans = objectSpans.ToList();
            Assert.Single(spans);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(9, spans[0].End);
            Assert.Equal(0, spans[0].Value);

            objectSpans.SetValue(2, 3, _ => 1);
            objectSpans.SetValue(6, 7, _ => 1);
            spans = objectSpans.ToList();
            Assert.Equal(5, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(1, spans[0].End);
            Assert.Equal(0, spans[0].Value);
            Assert.Equal(2, spans[1].Start);
            Assert.Equal(3, spans[1].End);
            Assert.Equal(1, spans[1].Value);
            Assert.Equal(4, spans[2].Start);
            Assert.Equal(5, spans[2].End);
            Assert.Equal(0, spans[2].Value);
            Assert.Equal(6, spans[3].Start);
            Assert.Equal(7, spans[3].End);
            Assert.Equal(1, spans[3].Value);
            Assert.Equal(8, spans[4].Start);
            Assert.Equal(9, spans[4].End);
            Assert.Equal(0, spans[4].Value);

            objectSpans.SetValue(2, 7, _ => 0);
            spans = objectSpans.ToList();
            Assert.Single(spans);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(9, spans[0].End);
            Assert.Equal(0, spans[0].Value);
        }

        [Fact]
        public void LeftSplitSet()
        {
            var objectSpans = new ObjectSpans<(int, int)>(10, (0, 0));
            var spans = objectSpans.ToList();
            Assert.Single(spans);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(9, spans[0].End);
            Assert.Equal((0, 0), spans[0].Value);

            objectSpans.SetValue(2, 4, _ => (1, 0));
            objectSpans.SetValue(5, 7, _ => (0, 1));
            spans = objectSpans.ToList();
            Assert.Equal(4, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(1, spans[0].End);
            Assert.Equal((0, 0), spans[0].Value);
            Assert.Equal(2, spans[1].Start);
            Assert.Equal(4, spans[1].End);
            Assert.Equal((1, 0), spans[1].Value);
            Assert.Equal(5, spans[2].Start);
            Assert.Equal(7, spans[2].End);
            Assert.Equal((0, 1), spans[2].Value);
            Assert.Equal(8, spans[3].Start);
            Assert.Equal(9, spans[3].End);
            Assert.Equal((0, 0), spans[3].Value);

            objectSpans.SetValue(3, 7, v => (2, v.Item2));
            spans = objectSpans.ToList();
            Assert.Equal(5, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(1, spans[0].End);
            Assert.Equal((0, 0), spans[0].Value);
            Assert.Equal(2, spans[1].Start);
            Assert.Equal(2, spans[1].End);
            Assert.Equal((1, 0), spans[1].Value);
            Assert.Equal(3, spans[2].Start);
            Assert.Equal(4, spans[2].End);
            Assert.Equal((2, 0), spans[2].Value);
            Assert.Equal(5, spans[3].Start);
            Assert.Equal(7, spans[3].End);
            Assert.Equal((2, 1), spans[3].Value);
            Assert.Equal(8, spans[4].Start);
            Assert.Equal(9, spans[4].End);
            Assert.Equal((0, 0), spans[4].Value);
        }

        [Fact]
        public void LeftSplitSet2()
        {
            var objectSpans = new ObjectSpans<int>(10, 0);
            var spans = objectSpans.ToList();
            Assert.Single(spans);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(9, spans[0].End);
            Assert.Equal(0, spans[0].Value);

            objectSpans.SetValue(2, 4, _ => 2);
            objectSpans.SetValue(5, 7, _ => 1);
            spans = objectSpans.ToList();
            Assert.Equal(4, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(1, spans[0].End);
            Assert.Equal(0, spans[0].Value);
            Assert.Equal(2, spans[1].Start);
            Assert.Equal(4, spans[1].End);
            Assert.Equal(2, spans[1].Value);
            Assert.Equal(5, spans[2].Start);
            Assert.Equal(7, spans[2].End);
            Assert.Equal(1, spans[2].Value);
            Assert.Equal(8, spans[3].Start);
            Assert.Equal(9, spans[3].End);
            Assert.Equal(0, spans[3].Value);

            objectSpans.SetValue(4, 7, _ => 1);
            spans = objectSpans.ToList();
            Assert.Equal(4, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(1, spans[0].End);
            Assert.Equal(0, spans[0].Value);
            Assert.Equal(2, spans[1].Start);
            Assert.Equal(3, spans[1].End);
            Assert.Equal(2, spans[1].Value);
            Assert.Equal(4, spans[2].Start);
            Assert.Equal(7, spans[2].End);
            Assert.Equal(1, spans[2].Value);
            Assert.Equal(8, spans[3].Start);
            Assert.Equal(9, spans[3].End);
            Assert.Equal(0, spans[3].Value);
        }

        [Fact]
        public void RightOutSet()
        {
            var objectSpans = new ObjectSpans<int>(10, 0);
            var spans = objectSpans.ToList();
            Assert.Single(spans);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(9, spans[0].End);
            Assert.Equal(0, spans[0].Value);

            objectSpans.SetValue(0, 4, _ => 1);
            spans = objectSpans.ToList();
            Assert.Equal(2, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(4, spans[0].End);
            Assert.Equal(1, spans[0].Value);
            Assert.Equal(5, spans[1].Start);
            Assert.Equal(9, spans[1].End);
            Assert.Equal(0, spans[1].Value);

            objectSpans.SetValue(5, 9, _ => 2);
            spans = objectSpans.ToList();
            Assert.Equal(2, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(4, spans[0].End);
            Assert.Equal(1, spans[0].Value);
            Assert.Equal(5, spans[1].Start);
            Assert.Equal(9, spans[1].End);
            Assert.Equal(2, spans[1].Value);
        }

        [Fact]
        public void RightOverrideSet()
        {
            var objectSpans = new ObjectSpans<(int A, int B)>(10, (0, 0));
            var spans = objectSpans.ToList();
            Assert.Single(spans);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(9, spans[0].End);
            Assert.Equal((0, 0), spans[0].Value);

            objectSpans.SetValue(0, 4, v => (1, v.B));
            spans = objectSpans.ToList();
            Assert.Equal(2, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(4, spans[0].End);
            Assert.Equal((1, 0), spans[0].Value);
            Assert.Equal(5, spans[1].Start);
            Assert.Equal(9, spans[1].End);
            Assert.Equal((0, 0), spans[1].Value);

            objectSpans.SetValue(0, 5, v => (1, v.B));
            spans = objectSpans.ToList();
            Assert.Equal(2, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(5, spans[0].End);
            Assert.Equal((1, 0), spans[0].Value);
            Assert.Equal(6, spans[1].Start);
            Assert.Equal(9, spans[1].End);
            Assert.Equal((0, 0), spans[1].Value);

            objectSpans.SetValue(0, 9, v => (v.A, 1));
            spans = objectSpans.ToList();
            Assert.Equal(2, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(5, spans[0].End);
            Assert.Equal((1, 1), spans[0].Value);
            Assert.Equal(6, spans[1].Start);
            Assert.Equal(9, spans[1].End);
            Assert.Equal((0, 1), spans[1].Value);
        }

        [Fact]
        public void RightFillSet()
        {
            var objectSpans = new ObjectSpans<int>(10, 0);
            var spans = objectSpans.ToList();
            Assert.Single(spans);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(9, spans[0].End);
            Assert.Equal(0, spans[0].Value);

            objectSpans.SetValue(6, 7, _ => 1);
            objectSpans.SetValue(2, 3, _ => 1);
            spans = objectSpans.ToList();
            Assert.Equal(5, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(1, spans[0].End);
            Assert.Equal(0, spans[0].Value);
            Assert.Equal(2, spans[1].Start);
            Assert.Equal(3, spans[1].End);
            Assert.Equal(1, spans[1].Value);
            Assert.Equal(4, spans[2].Start);
            Assert.Equal(5, spans[2].End);
            Assert.Equal(0, spans[2].Value);
            Assert.Equal(6, spans[3].Start);
            Assert.Equal(7, spans[3].End);
            Assert.Equal(1, spans[3].Value);
            Assert.Equal(8, spans[4].Start);
            Assert.Equal(9, spans[4].End);
            Assert.Equal(0, spans[4].Value);

            objectSpans.SetValue(2, 7, _ => 1);
            spans = objectSpans.ToList();
            Assert.Equal(3, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(1, spans[0].End);
            Assert.Equal(0, spans[0].Value);
            Assert.Equal(2, spans[1].Start);
            Assert.Equal(7, spans[1].End);
            Assert.Equal(1, spans[1].Value);
            Assert.Equal(8, spans[2].Start);
            Assert.Equal(9, spans[2].End);
            Assert.Equal(0, spans[2].Value);
        }

        [Fact]
        public void RightFillSet2()
        {
            var objectSpans = new ObjectSpans<(int, int)>(10, (0, 0));
            var spans = objectSpans.ToList();
            Assert.Single(spans);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(9, spans[0].End);
            Assert.Equal((0, 0), spans[0].Value);

            objectSpans.SetValue(6, 7, _ => (1, 0));
            objectSpans.SetValue(2, 3, _ => (1, 0));
            spans = objectSpans.ToList();
            Assert.Equal(5, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(1, spans[0].End);
            Assert.Equal((0, 0), spans[0].Value);
            Assert.Equal(2, spans[1].Start);
            Assert.Equal(3, spans[1].End);
            Assert.Equal((1, 0), spans[1].Value);
            Assert.Equal(4, spans[2].Start);
            Assert.Equal(5, spans[2].End);
            Assert.Equal((0, 0), spans[2].Value);
            Assert.Equal(6, spans[3].Start);
            Assert.Equal(7, spans[3].End);
            Assert.Equal((1, 0), spans[3].Value);
            Assert.Equal(8, spans[4].Start);
            Assert.Equal(9, spans[4].End);
            Assert.Equal((0, 0), spans[4].Value);

            objectSpans.SetValue(2, 7, v => (v.Item1, 1));
            spans = objectSpans.ToList();
            Assert.Equal(5, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(1, spans[0].End);
            Assert.Equal((0, 0), spans[0].Value);
            Assert.Equal(2, spans[1].Start);
            Assert.Equal(3, spans[1].End);
            Assert.Equal((1, 1), spans[1].Value);
            Assert.Equal(4, spans[2].Start);
            Assert.Equal(5, spans[2].End);
            Assert.Equal((0, 1), spans[2].Value);
            Assert.Equal(6, spans[3].Start);
            Assert.Equal(7, spans[3].End);
            Assert.Equal((1, 1), spans[3].Value);
            Assert.Equal(8, spans[4].Start);
            Assert.Equal(9, spans[4].End);
            Assert.Equal((0, 0), spans[4].Value);
        }

        [Fact]
        public void RightFillReset()
        {
            var objectSpans = new ObjectSpans<int>(10, 0);
            var spans = objectSpans.ToList();
            Assert.Single(spans);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(9, spans[0].End);
            Assert.Equal(0, spans[0].Value);

            objectSpans.SetValue(6, 7, _ => 1);
            objectSpans.SetValue(2, 3, _ => 1);
            spans = objectSpans.ToList();
            Assert.Equal(5, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(1, spans[0].End);
            Assert.Equal(0, spans[0].Value);
            Assert.Equal(2, spans[1].Start);
            Assert.Equal(3, spans[1].End);
            Assert.Equal(1, spans[1].Value);
            Assert.Equal(4, spans[2].Start);
            Assert.Equal(5, spans[2].End);
            Assert.Equal(0, spans[2].Value);
            Assert.Equal(6, spans[3].Start);
            Assert.Equal(7, spans[3].End);
            Assert.Equal(1, spans[3].Value);
            Assert.Equal(8, spans[4].Start);
            Assert.Equal(9, spans[4].End);
            Assert.Equal(0, spans[4].Value);

            objectSpans.SetValue(2, 7, _ => 0);
            spans = objectSpans.ToList();
            Assert.Single(spans);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(9, spans[0].End);
            Assert.Equal(0, spans[0].Value);
        }

        [Fact]
        public void RightSplitSet()
        {
            var objectSpans = new ObjectSpans<(int, int)>(10, (0, 0));
            var spans = objectSpans.ToList();
            Assert.Single(spans);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(9, spans[0].End);
            Assert.Equal((0, 0), spans[0].Value);

            objectSpans.SetValue(5, 7, _ => (1, 0));
            objectSpans.SetValue(2, 4, _ => (0, 1));
            spans = objectSpans.ToList();
            Assert.Equal(4, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(1, spans[0].End);
            Assert.Equal((0, 0), spans[0].Value);
            Assert.Equal(2, spans[1].Start);
            Assert.Equal(4, spans[1].End);
            Assert.Equal((0, 1), spans[1].Value);
            Assert.Equal(5, spans[2].Start);
            Assert.Equal(7, spans[2].End);
            Assert.Equal((1, 0), spans[2].Value);
            Assert.Equal(8, spans[3].Start);
            Assert.Equal(9, spans[3].End);
            Assert.Equal((0, 0), spans[3].Value);

            objectSpans.SetValue(2, 6, v => (2, v.Item2));
            spans = objectSpans.ToList();
            Assert.Equal(5, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(1, spans[0].End);
            Assert.Equal((0, 0), spans[0].Value);
            Assert.Equal(2, spans[1].Start);
            Assert.Equal(4, spans[1].End);
            Assert.Equal((2, 1), spans[1].Value);
            Assert.Equal(5, spans[2].Start);
            Assert.Equal(6, spans[2].End);
            Assert.Equal((2, 0), spans[2].Value);
            Assert.Equal(7, spans[3].Start);
            Assert.Equal(7, spans[3].End);
            Assert.Equal((1, 0), spans[3].Value);
            Assert.Equal(8, spans[4].Start);
            Assert.Equal(9, spans[4].End);
            Assert.Equal((0, 0), spans[4].Value);
        }

        [Fact]
        public void RightSplitSet2()
        {
            var objectSpans = new ObjectSpans<int>(10, 0);
            var spans = objectSpans.ToList();
            Assert.Single(spans);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(9, spans[0].End);
            Assert.Equal(0, spans[0].Value);

            objectSpans.SetValue(5, 7, _ => 1);
            objectSpans.SetValue(2, 4, _ => 2);
            spans = objectSpans.ToList();
            Assert.Equal(4, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(1, spans[0].End);
            Assert.Equal(0, spans[0].Value);
            Assert.Equal(2, spans[1].Start);
            Assert.Equal(4, spans[1].End);
            Assert.Equal(2, spans[1].Value);
            Assert.Equal(5, spans[2].Start);
            Assert.Equal(7, spans[2].End);
            Assert.Equal(1, spans[2].Value);
            Assert.Equal(8, spans[3].Start);
            Assert.Equal(9, spans[3].End);
            Assert.Equal(0, spans[3].Value);

            objectSpans.SetValue(2, 5, _ => 2);
            spans = objectSpans.ToList();
            Assert.Equal(4, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(1, spans[0].End);
            Assert.Equal(0, spans[0].Value);
            Assert.Equal(2, spans[1].Start);
            Assert.Equal(5, spans[1].End);
            Assert.Equal(2, spans[1].Value);
            Assert.Equal(6, spans[2].Start);
            Assert.Equal(7, spans[2].End);
            Assert.Equal(1, spans[2].Value);
            Assert.Equal(8, spans[3].Start);
            Assert.Equal(9, spans[3].End);
            Assert.Equal(0, spans[3].Value);
        }

        [Fact]
        public void CenterSet()
        {
            var objectSpans = new ObjectSpans<int>(10, 0);
            var spans = objectSpans.ToList();
            Assert.Single(spans);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(9, spans[0].End);
            Assert.Equal(0, spans[0].Value);

            objectSpans.SetValue(1, 2, _ => 1);
            objectSpans.SetValue(3, 6, _ => 2);
            objectSpans.SetValue(7, 8, _ => 1);
            spans = objectSpans.ToList();
            Assert.Equal(5, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(0, spans[0].End);
            Assert.Equal(0, spans[0].Value);
            Assert.Equal(1, spans[1].Start);
            Assert.Equal(2, spans[1].End);
            Assert.Equal(1, spans[1].Value);
            Assert.Equal(3, spans[2].Start);
            Assert.Equal(6, spans[2].End);
            Assert.Equal(2, spans[2].Value);
            Assert.Equal(7, spans[3].Start);
            Assert.Equal(8, spans[3].End);
            Assert.Equal(1, spans[3].Value);
            Assert.Equal(9, spans[4].Start);
            Assert.Equal(9, spans[4].End);
            Assert.Equal(0, spans[4].Value);

            objectSpans.SetValue(4, 5, _ => 1);
            spans = objectSpans.ToList();
            Assert.Equal(7, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(0, spans[0].End);
            Assert.Equal(0, spans[0].Value);
            Assert.Equal(1, spans[1].Start);
            Assert.Equal(2, spans[1].End);
            Assert.Equal(1, spans[1].Value);
            Assert.Equal(3, spans[2].Start);
            Assert.Equal(3, spans[2].End);
            Assert.Equal(2, spans[2].Value);
            Assert.Equal(4, spans[3].Start);
            Assert.Equal(5, spans[3].End);
            Assert.Equal(1, spans[3].Value);
            Assert.Equal(6, spans[4].Start);
            Assert.Equal(6, spans[4].End);
            Assert.Equal(2, spans[4].Value);
            Assert.Equal(7, spans[5].Start);
            Assert.Equal(8, spans[5].End);
            Assert.Equal(1, spans[5].Value);
            Assert.Equal(9, spans[6].Start);
            Assert.Equal(9, spans[6].End);
            Assert.Equal(0, spans[6].Value);
        }

        [Fact]
        public void CenterSplitSet()
        {
            var objectSpans = new ObjectSpans<int>(10, 0);
            var spans = objectSpans.ToList();
            Assert.Single(spans);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(9, spans[0].End);
            Assert.Equal(0, spans[0].Value);

            objectSpans.SetValue(1, 3, _ => 1);
            objectSpans.SetValue(4, 5, _ => 2);
            objectSpans.SetValue(6, 8, _ => 1);
            spans = objectSpans.ToList();
            Assert.Equal(5, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(0, spans[0].End);
            Assert.Equal(0, spans[0].Value);
            Assert.Equal(1, spans[1].Start);
            Assert.Equal(3, spans[1].End);
            Assert.Equal(1, spans[1].Value);
            Assert.Equal(4, spans[2].Start);
            Assert.Equal(5, spans[2].End);
            Assert.Equal(2, spans[2].Value);
            Assert.Equal(6, spans[3].Start);
            Assert.Equal(8, spans[3].End);
            Assert.Equal(1, spans[3].Value);
            Assert.Equal(9, spans[4].Start);
            Assert.Equal(9, spans[4].End);
            Assert.Equal(0, spans[4].Value);

            objectSpans.SetValue(3, 6, _ => 3);
            spans = objectSpans.ToList();
            Assert.Equal(5, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(0, spans[0].End);
            Assert.Equal(0, spans[0].Value);
            Assert.Equal(1, spans[1].Start);
            Assert.Equal(2, spans[1].End);
            Assert.Equal(1, spans[1].Value);
            Assert.Equal(3, spans[2].Start);
            Assert.Equal(6, spans[2].End);
            Assert.Equal(3, spans[2].Value);
            Assert.Equal(7, spans[3].Start);
            Assert.Equal(8, spans[3].End);
            Assert.Equal(1, spans[3].Value);
            Assert.Equal(9, spans[4].Start);
            Assert.Equal(9, spans[4].End);
            Assert.Equal(0, spans[4].Value);
        }

        [Fact]
        public void CenterLeftSet()
        {
            var objectSpans = new ObjectSpans<int>(10, 0);
            var spans = objectSpans.ToList();
            Assert.Single(spans);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(9, spans[0].End);
            Assert.Equal(0, spans[0].Value);

            objectSpans.SetValue(1, 2, _ => 1);
            objectSpans.SetValue(3, 6, _ => 2);
            objectSpans.SetValue(7, 8, _ => 1);
            spans = objectSpans.ToList();
            Assert.Equal(5, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(0, spans[0].End);
            Assert.Equal(0, spans[0].Value);
            Assert.Equal(1, spans[1].Start);
            Assert.Equal(2, spans[1].End);
            Assert.Equal(1, spans[1].Value);
            Assert.Equal(3, spans[2].Start);
            Assert.Equal(6, spans[2].End);
            Assert.Equal(2, spans[2].Value);
            Assert.Equal(7, spans[3].Start);
            Assert.Equal(8, spans[3].End);
            Assert.Equal(1, spans[3].Value);
            Assert.Equal(9, spans[4].Start);
            Assert.Equal(9, spans[4].End);
            Assert.Equal(0, spans[4].Value);

            objectSpans.SetValue(3, 5, _ => 3);
            spans = objectSpans.ToList();
            Assert.Equal(6, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(0, spans[0].End);
            Assert.Equal(0, spans[0].Value);
            Assert.Equal(1, spans[1].Start);
            Assert.Equal(2, spans[1].End);
            Assert.Equal(1, spans[1].Value);
            Assert.Equal(3, spans[2].Start);
            Assert.Equal(5, spans[2].End);
            Assert.Equal(3, spans[2].Value);
            Assert.Equal(6, spans[3].Start);
            Assert.Equal(6, spans[3].End);
            Assert.Equal(2, spans[3].Value);
            Assert.Equal(7, spans[4].Start);
            Assert.Equal(8, spans[4].End);
            Assert.Equal(1, spans[4].Value);
            Assert.Equal(9, spans[5].Start);
            Assert.Equal(9, spans[5].End);
            Assert.Equal(0, spans[5].Value);
        }

        [Fact]
        public void CenterLeftMergeSet()
        {
            var objectSpans = new ObjectSpans<int>(10, 0);
            var spans = objectSpans.ToList();
            Assert.Single(spans);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(9, spans[0].End);
            Assert.Equal(0, spans[0].Value);

            objectSpans.SetValue(1, 2, _ => 1);
            objectSpans.SetValue(3, 6, _ => 2);
            objectSpans.SetValue(7, 8, _ => 1);
            spans = objectSpans.ToList();
            Assert.Equal(5, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(0, spans[0].End);
            Assert.Equal(0, spans[0].Value);
            Assert.Equal(1, spans[1].Start);
            Assert.Equal(2, spans[1].End);
            Assert.Equal(1, spans[1].Value);
            Assert.Equal(3, spans[2].Start);
            Assert.Equal(6, spans[2].End);
            Assert.Equal(2, spans[2].Value);
            Assert.Equal(7, spans[3].Start);
            Assert.Equal(8, spans[3].End);
            Assert.Equal(1, spans[3].Value);
            Assert.Equal(9, spans[4].Start);
            Assert.Equal(9, spans[4].End);
            Assert.Equal(0, spans[4].Value);

            objectSpans.SetValue(3, 5, _ => 1);
            spans = objectSpans.ToList();
            Assert.Equal(5, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(0, spans[0].End);
            Assert.Equal(0, spans[0].Value);
            Assert.Equal(1, spans[1].Start);
            Assert.Equal(5, spans[1].End);
            Assert.Equal(1, spans[1].Value);
            Assert.Equal(6, spans[2].Start);
            Assert.Equal(6, spans[2].End);
            Assert.Equal(2, spans[2].Value);
            Assert.Equal(7, spans[3].Start);
            Assert.Equal(8, spans[3].End);
            Assert.Equal(1, spans[3].Value);
            Assert.Equal(9, spans[4].Start);
            Assert.Equal(9, spans[4].End);
            Assert.Equal(0, spans[4].Value);
        }

        [Fact]
        public void CenterRightSet()
        {
            var objectSpans = new ObjectSpans<int>(10, 0);
            var spans = objectSpans.ToList();
            Assert.Single(spans);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(9, spans[0].End);
            Assert.Equal(0, spans[0].Value);

            objectSpans.SetValue(1, 2, _ => 1);
            objectSpans.SetValue(3, 6, _ => 2);
            objectSpans.SetValue(7, 8, _ => 1);
            spans = objectSpans.ToList();
            Assert.Equal(5, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(0, spans[0].End);
            Assert.Equal(0, spans[0].Value);
            Assert.Equal(1, spans[1].Start);
            Assert.Equal(2, spans[1].End);
            Assert.Equal(1, spans[1].Value);
            Assert.Equal(3, spans[2].Start);
            Assert.Equal(6, spans[2].End);
            Assert.Equal(2, spans[2].Value);
            Assert.Equal(7, spans[3].Start);
            Assert.Equal(8, spans[3].End);
            Assert.Equal(1, spans[3].Value);
            Assert.Equal(9, spans[4].Start);
            Assert.Equal(9, spans[4].End);
            Assert.Equal(0, spans[4].Value);

            objectSpans.SetValue(4, 6, _ => 3);
            spans = objectSpans.ToList();
            Assert.Equal(6, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(0, spans[0].End);
            Assert.Equal(0, spans[0].Value);
            Assert.Equal(1, spans[1].Start);
            Assert.Equal(2, spans[1].End);
            Assert.Equal(1, spans[1].Value);
            Assert.Equal(3, spans[2].Start);
            Assert.Equal(3, spans[2].End);
            Assert.Equal(2, spans[2].Value);
            Assert.Equal(4, spans[3].Start);
            Assert.Equal(6, spans[3].End);
            Assert.Equal(3, spans[3].Value);
            Assert.Equal(7, spans[4].Start);
            Assert.Equal(8, spans[4].End);
            Assert.Equal(1, spans[4].Value);
            Assert.Equal(9, spans[5].Start);
            Assert.Equal(9, spans[5].End);
            Assert.Equal(0, spans[5].Value);
        }

        [Fact]
        public void CenterRightMergeSet()
        {
            var objectSpans = new ObjectSpans<int>(10, 0);
            var spans = objectSpans.ToList();
            Assert.Single(spans);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(9, spans[0].End);
            Assert.Equal(0, spans[0].Value);

            objectSpans.SetValue(1, 2, _ => 1);
            objectSpans.SetValue(3, 6, _ => 2);
            objectSpans.SetValue(7, 8, _ => 1);
            spans = objectSpans.ToList();
            Assert.Equal(5, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(0, spans[0].End);
            Assert.Equal(0, spans[0].Value);
            Assert.Equal(1, spans[1].Start);
            Assert.Equal(2, spans[1].End);
            Assert.Equal(1, spans[1].Value);
            Assert.Equal(3, spans[2].Start);
            Assert.Equal(6, spans[2].End);
            Assert.Equal(2, spans[2].Value);
            Assert.Equal(7, spans[3].Start);
            Assert.Equal(8, spans[3].End);
            Assert.Equal(1, spans[3].Value);
            Assert.Equal(9, spans[4].Start);
            Assert.Equal(9, spans[4].End);
            Assert.Equal(0, spans[4].Value);

            objectSpans.SetValue(4, 6, _ => 1);
            spans = objectSpans.ToList();
            Assert.Equal(5, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(0, spans[0].End);
            Assert.Equal(0, spans[0].Value);
            Assert.Equal(1, spans[1].Start);
            Assert.Equal(2, spans[1].End);
            Assert.Equal(1, spans[1].Value);
            Assert.Equal(3, spans[2].Start);
            Assert.Equal(3, spans[2].End);
            Assert.Equal(2, spans[2].Value);
            Assert.Equal(4, spans[3].Start);
            Assert.Equal(8, spans[3].End);
            Assert.Equal(1, spans[3].Value);
            Assert.Equal(9, spans[4].Start);
            Assert.Equal(9, spans[4].End);
            Assert.Equal(0, spans[4].Value);
        }

        [Fact]
        public void LeftMerge()
        {
            var objectSpans = new ObjectSpans<int>(10, 0);
            var spans = objectSpans.ToList();
            Assert.Single(spans);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(9, spans[0].End);
            Assert.Equal(0, spans[0].Value);

            objectSpans.SetValue(1, 3, _ => 1);
            objectSpans.SetValue(4, 5, _ => 2);
            objectSpans.SetValue(6, 8, _ => 3);
            spans = objectSpans.ToList();
            Assert.Equal(5, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(0, spans[0].End);
            Assert.Equal(0, spans[0].Value);
            Assert.Equal(1, spans[1].Start);
            Assert.Equal(3, spans[1].End);
            Assert.Equal(1, spans[1].Value);
            Assert.Equal(4, spans[2].Start);
            Assert.Equal(5, spans[2].End);
            Assert.Equal(2, spans[2].Value);
            Assert.Equal(6, spans[3].Start);
            Assert.Equal(8, spans[3].End);
            Assert.Equal(3, spans[3].Value);
            Assert.Equal(9, spans[4].Start);
            Assert.Equal(9, spans[4].End);
            Assert.Equal(0, spans[4].Value);

            objectSpans.SetValue(1, 5, _ => 2);
            spans = objectSpans.ToList();
            Assert.Equal(4, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(0, spans[0].End);
            Assert.Equal(0, spans[0].Value);
            Assert.Equal(1, spans[1].Start);
            Assert.Equal(5, spans[1].End);
            Assert.Equal(2, spans[1].Value);
            Assert.Equal(6, spans[2].Start);
            Assert.Equal(8, spans[2].End);
            Assert.Equal(3, spans[2].Value);
            Assert.Equal(9, spans[3].Start);
            Assert.Equal(9, spans[3].End);
            Assert.Equal(0, spans[3].Value);
        }

        [Fact]
        public void RightMerge()
        {
            var objectSpans = new ObjectSpans<int>(10, 0);
            var spans = objectSpans.ToList();
            Assert.Single(spans);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(9, spans[0].End);
            Assert.Equal(0, spans[0].Value);

            objectSpans.SetValue(6, 8, _ => 3);
            objectSpans.SetValue(1, 3, _ => 1);
            objectSpans.SetValue(4, 5, _ => 2);
            spans = objectSpans.ToList();
            Assert.Equal(5, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(0, spans[0].End);
            Assert.Equal(0, spans[0].Value);
            Assert.Equal(1, spans[1].Start);
            Assert.Equal(3, spans[1].End);
            Assert.Equal(1, spans[1].Value);
            Assert.Equal(4, spans[2].Start);
            Assert.Equal(5, spans[2].End);
            Assert.Equal(2, spans[2].Value);
            Assert.Equal(6, spans[3].Start);
            Assert.Equal(8, spans[3].End);
            Assert.Equal(3, spans[3].Value);
            Assert.Equal(9, spans[4].Start);
            Assert.Equal(9, spans[4].End);
            Assert.Equal(0, spans[4].Value);

            objectSpans.SetValue(4, 8, _ => 2);
            spans = objectSpans.ToList();
            Assert.Equal(4, spans.Count);
            Assert.Equal(0, spans[0].Start);
            Assert.Equal(0, spans[0].End);
            Assert.Equal(0, spans[0].Value);
            Assert.Equal(1, spans[1].Start);
            Assert.Equal(3, spans[1].End);
            Assert.Equal(1, spans[1].Value);
            Assert.Equal(4, spans[2].Start);
            Assert.Equal(8, spans[2].End);
            Assert.Equal(2, spans[2].Value);
            Assert.Equal(9, spans[3].Start);
            Assert.Equal(9, spans[3].End);
            Assert.Equal(0, spans[3].Value);
        }
    }
}
