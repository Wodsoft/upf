﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public interface ISealable
    {
        bool IsSealed { get; }

        void Seal();
    }
}
