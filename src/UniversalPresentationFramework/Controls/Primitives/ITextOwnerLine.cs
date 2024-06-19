using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Controls.Primitives
{
    public interface ITextOwnerLine
    {
        IReadOnlyList<ITextOwnerRun> Runs { get; }

        float LineHeight { get; }

        float Baseline { get; }

        int Position { get; }

        int Length { get; }
    }
}
