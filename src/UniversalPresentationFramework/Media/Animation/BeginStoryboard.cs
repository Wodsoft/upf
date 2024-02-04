using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using Wodsoft.UI.Controls;

namespace Wodsoft.UI.Media.Animation
{
    [RuntimeNameProperty("Name")]
    [ContentProperty("Storyboard")]
    public sealed class BeginStoryboard : TriggerAction
    {
        #region Properties

        private string? _name;
        private HandoffBehavior _handoffBehavior;

        public static readonly DependencyProperty StoryboardProperty = DependencyProperty.Register("Storyboard", typeof(Storyboard), typeof(BeginStoryboard));

        public Storyboard? Storyboard { get { return (Storyboard?)GetValue(StoryboardProperty); } set { SetValue(StoryboardProperty, value); } }

        public HandoffBehavior HandoffBehavior
        {
            get
            {
                return _handoffBehavior;
            }
            set
            {
                CheckSealed();
                _handoffBehavior = value;
            }
        }

        public string? Name
        {
            get => _name;
            set
            {
                CheckSealed();
                if (value != null && !NameScope.IsValidIdentifierName(value))
                    throw new ArgumentException("Invalid name.");
                _name = value;
            }
        }

        #endregion

        protected override void OnSeal()
        {
            var storyboard = Storyboard;
            if (storyboard == null)
                throw new InvalidOperationException("Storyboard can't be null.");
            storyboard.Freeze();
        }

        public override void Invoke(object source, DependencyObject container, INameScope? nameScope)
        {
            Storyboard!.Begin(container, nameScope, _handoffBehavior, _name != null);
        }
    }
}
