using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public sealed class StopStoryboard : ControllableStoryboardAction
    {
        protected override void Invoke(FrameworkElement container, Storyboard storyboard)
        {
            storyboard.Stop(container);
        }
    }
}
