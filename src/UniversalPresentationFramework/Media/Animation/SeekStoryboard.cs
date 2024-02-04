using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Controls;

namespace Wodsoft.UI.Media.Animation
{
    public sealed class SeekStoryboard : ControllableStoryboardAction
    {
        private TimeSpan _offset;
        public TimeSpan Offset
        {
            get
            {
                return _offset;
            }
            set
            {
                CheckSealed();
                _offset = value;
            }
        }

        private TimeSeekOrigin _origin;
        public TimeSeekOrigin Origin
        {
            get
            {
                return _origin;
            }
            set
            {
                CheckSealed();
                _origin = value;
            }
        }

        protected override void Invoke(FrameworkElement container, Storyboard storyboard)
        {
            storyboard.Seek(container, _offset, _origin);
        }
    }
}
