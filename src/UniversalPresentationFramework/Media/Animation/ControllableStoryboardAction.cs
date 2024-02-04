using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using Wodsoft.UI.Controls;

namespace Wodsoft.UI.Media.Animation
{
    public abstract class ControllableStoryboardAction : TriggerAction
    {
        private string? _beginStoryboardName;
        public string? BeginStoryboardName
        {
            get
            {
                return _beginStoryboardName;
            }
            set
            {
                CheckSealed();
                _beginStoryboardName = value;
            }
        }

        protected override void OnSeal()
        {
            if (_beginStoryboardName == null)
                throw new InvalidOperationException("Action must have a BeginStoryboardName.");
        }

        public override void Invoke(object source, DependencyObject container, INameScope? nameScope)
        {
            var fe = (FrameworkElement)container;
            Invoke(fe, GetStoryboard(fe, source as INameScope));
        }

        private Storyboard GetStoryboard(FrameworkElement fe, INameScope? nameScope)
        {
            BeginStoryboard? beginStoryboard;
            if (nameScope != null)
                beginStoryboard = nameScope.FindName(_beginStoryboardName!) as BeginStoryboard;
            else
                beginStoryboard = fe.FindName(_beginStoryboardName!) as BeginStoryboard;
            if (beginStoryboard == null)
                throw new InvalidOperationException($"BeginStoryboard with name \"{_beginStoryboardName}\" not found.");
            Storyboard storyboard = beginStoryboard.Storyboard!;
            if (storyboard == null)
                throw new InvalidOperationException($"BeginStoryboard must have a storyboard.");
            return storyboard;
        }

        protected abstract void Invoke(FrameworkElement container, Storyboard storyboard);
    }
}
