using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class TimelineCollection : Freezable, IList<Timeline>
    {
        private readonly TimelineGroup _group;
        private readonly List<Timeline> _timelines;
        public TimelineCollection(TimelineGroup group)
        {
            _group = group;
            _timelines = new List<Timeline>();
        }
        private TimelineCollection(TimelineGroup group, List<Timeline> timelines)
        {
            _group = group;
            _timelines = new List<Timeline>(timelines);
        }

        public Timeline this[int index]
        {
            get
            {
                return _timelines[index];
            }
            set
            {
                WritePreamble();
                if (value == null)
                    throw new ArgumentNullException("value");
                var oldItem = _timelines[index];
                RemoveTimeline(oldItem);
                AddTimeline(value);
                _timelines[index] = value;
            }
        }

        public int Count => _timelines.Count;

        public bool IsReadOnly => IsFrozen;

        public void Add(Timeline item)
        {
            WritePreamble();
            AddTimeline(item);
            _timelines.Add(item);
        }

        public void Clear()
        {
            WritePreamble();
            for (int i = _timelines.Count - 1; i >= 0; i--)
            {
                RemoveTimeline(_timelines[i]);
                _timelines.RemoveAt(i);
            }
        }

        public bool Contains(Timeline item)
        {
            return _timelines.Contains(item);
        }

        public void CopyTo(Timeline[] array, int arrayIndex)
        {
            _timelines.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Timeline> GetEnumerator()
        {
            return _timelines.GetEnumerator();
        }

        public int IndexOf(Timeline item)
        {
            return _timelines.IndexOf(item);
        }

        public void Insert(int index, Timeline item)
        {
            WritePreamble();
            AddTimeline(item);
            _timelines.Insert(index, item);
        }

        public bool Remove(Timeline item)
        {
            WritePreamble();
            var index = _timelines.IndexOf(item);
            if (index == -1)
                return false;
            RemoveTimeline(_timelines[index]);
            _timelines.RemoveAt(index);
            return true;
        }

        public void RemoveAt(int index)
        {
            WritePreamble();
            RemoveTimeline(_timelines[index]);
            _timelines.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _timelines.GetEnumerator();
        }

        private void AddTimeline(Timeline timeline)
        {
            timeline.SetParent(_group);
        }

        private void RemoveTimeline(Timeline timeline)
        {
            timeline.RemoveParent(_group);
        }

        protected override Freezable CreateInstanceCore()
        {
            return new TimelineCollection(_group, _timelines);
        }
    }
}
