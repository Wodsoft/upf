using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    internal class Constants
    {
        public const float DefaultRealToIdeal = 28800.0f / 96f;
        public const float DefaultIdealToReal = 1 / DefaultRealToIdeal;
        public const int IdealInfiniteWidth = 0x3FFFFFFE;
        public const float RealInfiniteWidth = IdealInfiniteWidth * DefaultIdealToReal;
        public const float GreatestMutiplierOfEm = 100;
    }
}
