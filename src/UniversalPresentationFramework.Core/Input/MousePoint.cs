using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Input
{
    internal record struct MousePoint
    {
        public MousePoint(int x, int y)
        {
            X=x;
            Y = y;
        }

        public int X;
        public int Y;
    }
}
