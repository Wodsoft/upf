using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    internal struct ResolvedKeyFrameEntry : IComparable
    {
        internal int _originalKeyFrameIndex;
        internal TimeSpan _resolvedKeyTime;

        public int CompareTo(object? other)
        {
            ResolvedKeyFrameEntry otherEntry = (ResolvedKeyFrameEntry)other!;

            if (otherEntry._resolvedKeyTime > _resolvedKeyTime)
            {
                return -1;
            }
            else if (otherEntry._resolvedKeyTime < _resolvedKeyTime)
            {
                return 1;
            }
            else
            {
                if (otherEntry._originalKeyFrameIndex > _originalKeyFrameIndex)
                {
                    return -1;
                }
                else if (otherEntry._originalKeyFrameIndex < _originalKeyFrameIndex)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
