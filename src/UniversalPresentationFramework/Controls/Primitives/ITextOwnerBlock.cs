using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Controls.Primitives
{
    public interface ITextOwnerBlock
    {
        ITextOwnerInline FirstInline { get; }

        ITextOwnerInline LastInline { get; }

        float LineHeight { get; }

        float Baseline { get; }

        int Position { get; }

        int Length { get; }

        ITextOwnerBlock? PreviousBlock { get; }

        ITextOwnerBlock? NextBlock { get; }
    }
}
