using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public record struct Size
    {
        public Size(float width, float height)
        {
            _width = width;
            _height = height;
        }

        public float _width, _height;

        public float Width
        {
            get => _width;
            set
            {
                _width = value;
            }
        }

        public float Height
        {
            get => _height;
            set
            {
                _height = value;
            }
        }

        public readonly static Size Empty = new Size(0, 0);
    }
}
