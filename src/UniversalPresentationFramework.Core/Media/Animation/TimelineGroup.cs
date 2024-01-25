using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;

namespace Wodsoft.UI.Media.Animation
{
    [ContentProperty("Children")]
    public abstract class TimelineGroup : Timeline
    {
        #region Properties

        private TimelineCollection? _children;
        public TimelineCollection Children
        {
            get
            {
                if (_children == null)
                {
                    _children = new TimelineCollection(this);
                    if (IsFrozen)
                        _children.Freeze();
                }
                return _children;
            }
        }

        #endregion

        #region Clock

        protected internal override Clock AllocateClock()
        {
            return new ClockGroup(this);
        }

        #endregion
    }
}
