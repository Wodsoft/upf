using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Input
{
    public struct KeyboardInput
    {
        public int MessageTime;
        public Key Key;
        public KeyStates KeyStates;
        public char CharCode;
        public bool IsCharCode;
    }
}
