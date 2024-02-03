using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    internal class TriggerStorage
    {
        private readonly SortedList<byte, List<TriggerValue>> _values = new SortedList<byte, List<TriggerValue>>();

        public void AddValue(byte layer, TriggerValue value)
        {
            if (!_values.TryGetValue(layer, out var list))
            {
                list = new List<TriggerValue>();
                _values.Add(layer, list);
            }
            list.Add(value);
        }

        public void RemoveValue(byte layer, TriggerValue value)
        {
            if (_values.TryGetValue(layer, out var list))
            {
                list.Remove(value);
                if (list.Count == 0)
                    _values.Remove(layer);
            }
        }

        public bool TryGetValue(byte layer, out object? value)
        {
            if (_values.TryGetValue(layer, out var list))
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    var triggerValue = list[i];
                    if (triggerValue.IsEnabled)
                    {
                        value = triggerValue.Value;
                        return true;
                    }
                }
            }
            value = null;
            return false;
        }
    }
}
