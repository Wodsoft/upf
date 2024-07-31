using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using Wodsoft.UI.Media.Animation;

namespace Wodsoft.UI
{
    [ContentProperty("Storyboard")]
    [RuntimeNameProperty("Name")]
    public class VisualState : Freezable
    {
        private string? _name;
        public string? Name
        {
            get => _name;
            set
            {
                WritePreamble();
                _name = value;
            }
        }

        public static readonly DependencyProperty StoryboardProperty = DependencyProperty.Register("Storyboard", typeof(Storyboard), typeof(VisualState));
        public Storyboard? Storyboard
        {
            get { return (Storyboard?)GetValue(StoryboardProperty); }
            set { SetValue(StoryboardProperty, value); }
        }

        private SetterBaseCollection? _setters;
        public SetterBaseCollection Setters
        {
            get
            {
                if (_setters == null)
                {
                    _setters = new SetterBaseCollection();

                    if (IsSealed)
                        _setters.Seal();
                }
                return _setters;
            }
        }
        internal SetterBaseCollection? InternalSetters => _setters;

        #region Freezable

        protected override Freezable CreateInstanceCore() => new VisualState();

        protected override void CloneCore(Freezable sourceFreezable)
        {
            var source = (VisualState)sourceFreezable;
            Name = source.Name;
            Storyboard = source.Storyboard;
            _setters = source._setters;
            base.CloneCore(sourceFreezable);
        }

        protected override void CloneCurrentValueCore(Freezable sourceFreezable)
        {
            var source = (VisualState)sourceFreezable;
            Name = source.Name;
            Storyboard = source.Storyboard;
            _setters = source._setters;
            base.CloneCurrentValueCore(sourceFreezable);
        }

        protected override void GetAsFrozenCore(Freezable sourceFreezable)
        {
            var source = (VisualState)sourceFreezable;
            Name = source.Name;
            Storyboard = source.Storyboard;
            _setters = source._setters;
            base.GetAsFrozenCore(sourceFreezable);
        }

        protected override void GetCurrentValueAsFrozenCore(Freezable sourceFreezable)
        {
            var source = (VisualState)sourceFreezable;
            Name = source.Name;
            Storyboard = source.Storyboard;
            _setters = source._setters;
            base.GetCurrentValueAsFrozenCore(sourceFreezable);
        }

        protected override bool FreezeCore(bool isChecking)
        {
            if (isChecking)
            {
                if (_name == null)
                    return false;
                if (_setters == null)
                    return true;
                return _setters.All(t => t is Setter or EventSetter);
            }
            else
            {
                _setters?.Seal();
            }
            return true;
        }

        #endregion
    }
}
