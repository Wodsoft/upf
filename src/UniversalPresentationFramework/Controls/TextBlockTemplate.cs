﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Controls
{
    public partial class TextBlock
    {
        private class TextBlockTemplate : FrameworkTemplate
        {
            protected internal override Type TargetTypeInternal => typeof(TextBlock);
        }
    }
}
