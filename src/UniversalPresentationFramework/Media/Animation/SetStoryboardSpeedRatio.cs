using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Controls;

namespace Wodsoft.UI.Media.Animation
{
    public sealed class SetStoryboardSpeedRatio : ControllableStoryboardAction
    {
        private float _speedRatio = 1f;
        public float SpeedRatio
        {
            get
            {
                return _speedRatio;
            }
            set
            {
                CheckSealed();
                _speedRatio = value;
            }
        }

        protected override void Invoke(FrameworkElement container, Storyboard storyboard)
        {
            storyboard.SetSpeedRatio(container, _speedRatio);
        }
    }
}
