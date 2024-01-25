using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public class ParallelTimeline : TimelineGroup
    {
        #region Properties

        public static readonly DependencyProperty SlipBehaviorProperty =
            DependencyProperty.Register(
                "SlipBehavior",
                typeof(SlipBehavior),
                typeof(ParallelTimeline),
                new PropertyMetadata(SlipBehavior.Grow));
        public SlipBehavior SlipBehavior { get { return (SlipBehavior)GetValue(SlipBehaviorProperty)!; } set { SetValue(SlipBehaviorProperty, value); } }

        #endregion

        protected override Freezable CreateInstanceCore()
        {
            return new ParallelTimeline();
        }
    }
}
