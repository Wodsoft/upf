using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Controls.Primitives
{
    public interface ITextHostLine
    {
        IReadOnlyList<ITextHostRun> Runs { get; }

        float LineHeight { get; }

        float Baseline { get; }
    }
}
