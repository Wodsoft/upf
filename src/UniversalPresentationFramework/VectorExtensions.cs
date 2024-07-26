using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public static class VectorExtensions
    {
        public static T Sum<T>(this Span<T> span)
            where T : struct, IAdditionOperators<T, T, T> => Sum((ReadOnlySpan<T>)span);

        public static T Sum<T>(this ReadOnlySpan<T> span)
            where T : struct, IAdditionOperators<T, T, T>
        {
            if (span.Length == 0)
                return default;
            T sum = default;
            if (Vector.IsHardwareAccelerated)
            {
                var vectorSize = Vector<T>.Count;
                int i = 0;
                for (; i < span.Length - vectorSize; i += vectorSize)
                {
                    var vectors = MemoryMarshal.Cast<T, Vector<T>>(span.Slice(i, vectorSize));
                    sum += Vector.Sum(vectors[0]);
                }
                for (; i < span.Length; i++)
                    sum += span[i];
            }
            else
            {
                for (int i = 0; i < span.Length; i++)
                    sum += span[i];
            }
            return sum;
        }
    }
}
