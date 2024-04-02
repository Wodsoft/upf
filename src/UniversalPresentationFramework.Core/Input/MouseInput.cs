using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Input
{
    internal record struct MouseInput
    {
        public int MessageTime;
        public MouseActions Actions;
        public MouseButton? Button;
        public MousePoint Point;
        public int Wheel;
    }
}
