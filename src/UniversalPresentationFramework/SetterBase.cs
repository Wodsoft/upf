﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public abstract class SetterBase : SealableDependencyObject
    {
        internal abstract bool IsSameTarget(SetterBase setterBase);
    }
}
