using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media.TextFormatting;

namespace Wodsoft.UI
{
    public class ObjectSpans<T> : IEnumerable<ObjectSpan<T>>
        where T : IEquatable<T>
    {
        private readonly int _length;
        private readonly T _defaultValue;
        private readonly List<ObjectSpan<T>> _spans;
        private int _lastSpan;
        private int _timestamp;

        public ObjectSpans(int length, T defaultValue)
        {
            _length = length;
            _defaultValue = defaultValue;
            _spans = new List<ObjectSpan<T>>();
        }

        public IEnumerator<ObjectSpan<T>> GetEnumerator()
        {
            return new ObjectSpansEnumerator(this);
        }

        public void SetValue(int start, int end, SetObjectSpanValue<T> setValue)
        {
            FindSpan(start, end, out var index);
            var newValue = setValue(_defaultValue);
            if (index == -1)
            {
                if (_defaultValue.Equals(newValue))
                    return;
                var newSpan = new ObjectSpan<T>();
                newSpan.Start = start;
                newSpan.End = end;
                newSpan.Value = newValue;
                _spans.Add(newSpan);
                _timestamp++;
                return;
            }
            var spans = CollectionsMarshal.AsSpan(_spans);
            ref var span = ref spans[index];
            //range out of spans
            if (span.End < start && index == spans.Length - 1)
            {
                if (!_defaultValue.Equals(newValue))
                {
                    var newSpan = new ObjectSpan<T>();
                    newSpan.Start = start;
                    newSpan.End = end;
                    newSpan.Value = newValue;
                    _spans.Add(newSpan);
                    _lastSpan = _spans.Count - 1;
                    _timestamp++;
                }
                return;
            }
            else if (span.Start > end && index == 0)
            {
                if (!_defaultValue.Equals(newValue))
                {
                    var newSpan = new ObjectSpan<T>();
                    newSpan.Start = start;
                    newSpan.End = end;
                    newSpan.Value = newValue;
                    _spans.Insert(0, newSpan);
                    _lastSpan = 0;
                    _timestamp++;
                }
                return;
            }

            _timestamp++;
            ref var leftSpan = ref span;
            var leftSpanIndex = index;
            //start less than span
            while (leftSpan.Start > start)
            {
                if (leftSpanIndex == 0)
                {
                    //no more left
                    if (!_defaultValue.Equals(newValue))
                    {
                        if (leftSpan.Start <= end + 1 && leftSpan.Value.Equals(newValue))
                        {
                            //increase span
                            leftSpan.Start = start;
                        }
                        else
                        {
                            //new span
                            var newSpan = new ObjectSpan<T>();
                            newSpan.Start = start;
                            newSpan.End = Math.Min(leftSpan.Start - 1, end);
                            newSpan.Value = newValue;
                            _spans.Insert(0, newSpan);
                            _lastSpan++;
                            index++;
                            spans = CollectionsMarshal.AsSpan(_spans);
                            span = ref spans[index];
                        }
                    }
                    break;
                }
                else
                {
                    leftSpanIndex--;
                    ref var previousSpan = ref spans[leftSpanIndex];
                    if (leftSpan.Start - previousSpan.End != 1 && !_defaultValue.Equals(newValue))
                    {
                        if (leftSpan.Value.Equals(newValue))
                        {
                            //increase span
                            leftSpan.Start = Math.Max(start, previousSpan.End + 1);
                        }
                        else
                        {
                            //fill middle space
                            var newSpan = new ObjectSpan<T>();
                            newSpan.Start = Math.Max(start, previousSpan.End + 1);
                            newSpan.End = leftSpan.Start - 1;
                            newSpan.Value = newValue;
                            _spans.Insert(leftSpanIndex + 1, newSpan);
                            _lastSpan++;
                            index++;
                            spans = CollectionsMarshal.AsSpan(_spans);
                            leftSpan = ref spans[leftSpanIndex + 1];
                            span = ref spans[index];
                        }
                    }
                    if (previousSpan.End >= start)
                    {
                        var spanNewValue = setValue(previousSpan.Value);
                        if (!spanNewValue.Equals(previousSpan.Value))
                        {
                            if (previousSpan.Start >= start)
                            {
                                //override span value
                                previousSpan.Value = setValue(previousSpan.Value);
                                if (_defaultValue.Equals(previousSpan.Value))
                                {
                                    //reset span
                                    _spans.RemoveAt(leftSpanIndex);
                                    _lastSpan--;
                                    index--;
                                    spans = CollectionsMarshal.AsSpan(_spans);
                                    span = ref spans[index];
                                }
                                else if (leftSpan.Value.Equals(previousSpan.Value))
                                {
                                    //merge span
                                    leftSpan.Start = previousSpan.Start;
                                    _lastSpan--;
                                    index--;
                                    _spans.RemoveAt(leftSpanIndex);
                                    spans = CollectionsMarshal.AsSpan(_spans);
                                    span = ref spans[index];
                                }
                            }
                            else
                            {
                                //split span
                                if (leftSpan.Start - 1 == previousSpan.End && leftSpan.Value.Equals(spanNewValue))
                                {
                                    leftSpan.Start = start;
                                }
                                else
                                {
                                    var newSpan = new ObjectSpan<T>();
                                    newSpan.Start = start;
                                    newSpan.End = previousSpan.End;
                                    newSpan.Value = newValue;
                                    _spans.Insert(leftSpanIndex + 1, newSpan);
                                    _lastSpan++;
                                    index++;
                                    spans = CollectionsMarshal.AsSpan(_spans);
                                    span = ref spans[index];
                                }
                                previousSpan.End = start - 1;
                                break;
                            }
                        }
                    }
                    leftSpan = ref previousSpan;
                }
            }
            //end greater than span
            ref var rightSpan = ref span;
            var rightSpanIndex = index;
            while (rightSpan.End < end)
            {
                if (rightSpanIndex == _spans.Count - 1)
                {
                    //no more right
                    if (!_defaultValue.Equals(newValue))
                    {
                        if (rightSpan.End >= start - 1 && rightSpan.Value.Equals(newValue))
                        {
                            //increase span
                            rightSpan.End = end;
                        }
                        else
                        {
                            //new span
                            var newSpan = new ObjectSpan<T>();
                            newSpan.Start = Math.Max(rightSpan.End + 1, start);
                            newSpan.End = end;
                            newSpan.Value = newValue;
                            _spans.Add(newSpan);
                            _lastSpan = _spans.Count - 1;
                            spans = CollectionsMarshal.AsSpan(_spans);
                        }
                    }
                    break;
                }
                else
                {
                    rightSpanIndex++;
                    ref var nextSpan = ref spans[rightSpanIndex];
                    if (nextSpan.Start - rightSpan.End != 1 && !_defaultValue.Equals(newValue))
                    {
                        if (rightSpan.Value.Equals(newValue))
                        {
                            //increase span
                            rightSpan.End = Math.Min(end, nextSpan.Start - 1);
                        }
                        else
                        {
                            //fill middle space
                            var newSpan = new ObjectSpan<T>();
                            newSpan.Start = rightSpan.End + 1;
                            newSpan.End = Math.Min(end, nextSpan.Start - 1);
                            newSpan.Value = newValue;
                            _spans.Insert(rightSpanIndex, newSpan);
                            _lastSpan = rightSpanIndex;
                            spans = CollectionsMarshal.AsSpan(_spans);
                            nextSpan = spans[rightSpanIndex];
                        }
                    }
                    if (nextSpan.Start <= end)
                    {
                        var spanNewValue = setValue(nextSpan.Value);
                        if (!spanNewValue.Equals(nextSpan.Value))
                        {
                            if (nextSpan.End <= end)
                            {
                                //override span value
                                nextSpan.Value = setValue(nextSpan.Value);
                                if (_defaultValue.Equals(nextSpan.Value))
                                {
                                    //reset span
                                    _spans.RemoveAt(rightSpanIndex);
                                    _lastSpan = rightSpanIndex - 1;
                                    spans = CollectionsMarshal.AsSpan(_spans);
                                }
                                else if (rightSpan.Value.Equals(nextSpan.Value))
                                {
                                    rightSpan.End = nextSpan.End;
                                    _lastSpan = rightSpanIndex - 1;
                                    _spans.RemoveAt(rightSpanIndex);
                                    rightSpanIndex--;
                                    spans = CollectionsMarshal.AsSpan(_spans);
                                    nextSpan = ref spans[rightSpanIndex];
                                }
                            }
                            else
                            {
                                //split span
                                if (rightSpan.End + 1 == nextSpan.Start && rightSpan.Value.Equals(spanNewValue))
                                {
                                    rightSpan.End = end;
                                }
                                else
                                {
                                    var newSpan = new ObjectSpan<T>();
                                    newSpan.Start = nextSpan.Start;
                                    newSpan.End = end;
                                    newSpan.Value = newValue;
                                    _spans.Insert(rightSpanIndex, newSpan);
                                    rightSpanIndex++;
                                    _lastSpan = rightSpanIndex;
                                    spans = CollectionsMarshal.AsSpan(_spans);
                                    nextSpan = ref spans[rightSpanIndex];

                                }
                                nextSpan.Start = end + 1;
                                break;
                            }
                        }
                    }
                    rightSpan = ref nextSpan;
                }
            }

            if (span.Start >= start && span.End <= end)
            {
                //span within range, override value
                span.Value = setValue(span.Value);
                if (_defaultValue.Equals(span.Value))
                {
                    //reset span
                    _spans.RemoveAt(index);
                    if (_lastSpan >= index)
                        _lastSpan--;
                }
                else
                {
                    if (index != 0)
                    {
                        leftSpan = ref spans[index - 1];
                        if (leftSpan.End + 1 == span.Start && leftSpan.Value.Equals(span.Value))
                        {
                            //merge left
                            leftSpan.End = span.End;
                            span = ref leftSpan;
                            _spans.RemoveAt(index);
                            if (_lastSpan >= index)
                                _lastSpan--;
                            index--;
                            spans = CollectionsMarshal.AsSpan(_spans);
                        }
                    }
                    if (index != _spans.Count - 1)
                    {
                        rightSpan = ref spans[index + 1];
                        if (rightSpan.Start - 1 == span.End && rightSpan.Value.Equals(span.Value))
                        {
                            //merge right
                            rightSpan.Start = span.Start;
                            span = ref rightSpan;
                            _spans.RemoveAt(index);
                            if (_lastSpan >= index)
                                _lastSpan--;
                            spans = CollectionsMarshal.AsSpan(_spans);
                        }
                    }
                }
            }
            else if ((span.Start < start && span.End >= start) || (span.Start <= end && span.End > end))
            {
                var spanNewValue = setValue(span.Value);
                if (!span.Value.Equals(spanNewValue))
                {
                    if (span.Start < start && span.End > end)
                    {
                        //split to 3 piece
                        var newSpan = span;
                        span.End = start - 1;
                        newSpan.Start = end + 1;
                        _spans.Insert(index + 1, newSpan);
                        if (!_defaultValue.Equals(spanNewValue))
                        {
                            var splitSpan = new ObjectSpan<T>();
                            splitSpan.Start = start;
                            splitSpan.End = end;
                            splitSpan.Value = spanNewValue;
                            _spans.Insert(index + 1, splitSpan);
                        }
                    }
                    else
                    {
                        if (span.Start < start)
                        {
                            if (!_defaultValue.Equals(spanNewValue))
                            {
                                if (index != _spans.Count - 1 && spans[index + 1].Start == span.End + 1 && spanNewValue.Equals(spans[index + 1].Value))
                                {
                                    spans[index + 1].Start = start;
                                }
                                else
                                {
                                    var splitSpan = new ObjectSpan<T>();
                                    splitSpan.Start = start;
                                    splitSpan.End = span.End;
                                    splitSpan.Value = spanNewValue;
                                    _spans.Insert(index + 1, splitSpan);
                                }
                            }
                            span.End = start - 1;
                        }
                        else
                        {
                            if (!_defaultValue.Equals(spanNewValue))
                            {
                                if (index != 0 && spans[index - 1].End == span.Start - 1 && spanNewValue.Equals(spans[index - 1].Value))
                                {
                                    spans[index - 1].End = end;
                                }
                                else
                                {
                                    var splitSpan = new ObjectSpan<T>();
                                    splitSpan.Start = span.Start;
                                    splitSpan.End = end;
                                    splitSpan.Value = spanNewValue;
                                    _spans.Insert(index, splitSpan);
                                    spans = CollectionsMarshal.AsSpan(_spans);
                                    span = ref spans[index + 1];
                                }
                            }
                            span.Start = end + 1;
                        }

                    }
                }
            }
        }

        private void FindSpan(int start, int end, out int index)
        {
            if (_spans.Count == 0)
            {
                index = -1;
                return;
            }
            var spans = CollectionsMarshal.AsSpan(_spans);
            ref var span = ref spans[_lastSpan];
            if (span.End < start)
            {
                //Search right span
                for (index = _lastSpan + 1; index < spans.Length; index++)
                {
                    span = ref spans[index];
                    if (span.End < start)
                        continue;
                    return;
                }
                index = spans.Length - 1;
            }
            else if (span.Start > end)
            {
                //Search left span
                for (index = _lastSpan - 1; index >= 0; index--)
                {
                    span = ref spans[index];
                    if (span.Start > end)
                        continue;
                    return;
                }
                index = 0;
            }
            else
            {
                index = _lastSpan;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class ObjectSpansEnumerator : IEnumerator<ObjectSpan<T>>
        {
            private readonly ObjectSpans<T> _spans;
            private readonly int _timestamp;
            private int _index;
            private bool _isEmpty;
            private ObjectSpan<T> _nextSpan, _currentSpan;

            public ObjectSpansEnumerator(ObjectSpans<T> spans)
            {
                _spans = spans;
                _timestamp = spans._timestamp;
                _index = -1;
                _nextSpan = _currentSpan = new ObjectSpan<T>
                {
                    Start = -1,
                    End = -1,
                    Value = _spans._defaultValue
                };
            }

            public ObjectSpan<T> Current => _currentSpan;

            object IEnumerator.Current => Current;

            void IDisposable.Dispose() { }

            public bool MoveNext()
            {
                if (_timestamp != _spans._timestamp)
                    throw new InvalidOperationException("ObjectSpans was changed.");
                if (_currentSpan.Start != _nextSpan.Start)
                {
                    _currentSpan = _nextSpan;
                    return true;
                }
                else
                {
                    _index++;
                    if (_index >= _spans._spans.Count)
                    {
                        if (_currentSpan.End < _spans._length - 1)
                        {
                            _nextSpan = _currentSpan = new ObjectSpan<T>
                            {
                                Start = _currentSpan.End + 1,
                                End = _spans._length - 1,
                                Value = _spans._defaultValue
                            };
                            return true;
                        }
                        return false;
                    }
                    _nextSpan = _spans._spans[_index];
                    if (_currentSpan.End < _nextSpan.Start - 1)
                    {
                        _currentSpan = new ObjectSpan<T>
                        {
                            Start = _currentSpan.End + 1,
                            End = _nextSpan.Start - 1,
                            Value = _spans._defaultValue
                        };
                    }
                    else
                    {
                        _currentSpan = _nextSpan;
                    }
                    return true;
                }
            }

            public void Reset()
            {
                if (_timestamp != _spans._timestamp)
                    throw new InvalidOperationException("ObjectSpans was changed.");
                _index = -1;
                _nextSpan = _currentSpan = new ObjectSpan<T>
                {
                    Start = -1,
                    End = -1,
                    Value = _spans._defaultValue
                };
            }
        }
    }

    public struct ObjectSpan<T>
        where T : IEquatable<T>
    {
        public int Start;
        public int End;
        public T Value;

        public override string ToString()
        {
            return $"({Start},{End})={Value}";
        }
    }

    public delegate T SetObjectSpanValue<T>(T oldValue);
}
