using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.TextFormatting
{
    public abstract class TextRun
    {
        /// <summary>
        /// Characters
        /// </summary>
        public abstract ReadOnlySpan<char> Characters { get; }

        /// <summary>
        /// Character length
        /// </summary>
        public abstract int Length { get; }

        /// <summary>
        /// A set of properties shared by every characters in the run
        /// </summary>
        public abstract TextRunProperties Properties { get; }

        public abstract bool IsEndOfNewLine { get; }

        public abstract int GetNextLineBreakPosition();

        public abstract TextRun Slice(int start, int length);

        public TextRun Slice(int start)
        {
            return Slice(start, Length - start);
        }

        public abstract int Start { get; }
    }
}
