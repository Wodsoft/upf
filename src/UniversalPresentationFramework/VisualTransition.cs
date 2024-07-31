using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using System.Xml.Linq;
using Wodsoft.UI.Media.Animation;

namespace Wodsoft.UI
{
    [ContentProperty("Storyboard")]
    public class VisualTransition : Freezable
    {
        private string? _from;
        public string? From
        {
            get => _from; set
            {
                WritePreamble();
                _from = value;
            }
        }

        private string? _to;
        public string? To
        {
            get => _to; set
            {
                WritePreamble();
                _to = value;
            }
        }

        private Storyboard? _storyboard;
        public Storyboard? Storyboard
        {
            get => _storyboard; set
            {
                WritePreamble();
                _storyboard = value;
            }
        }

        private Duration _generatedDuration = Duration.Zero;
        public Duration GeneratedDuration
        {
            get => _generatedDuration; set
            {
                WritePreamble();
                _generatedDuration = value;
            }
        }

        private IEasingFunction? _generatedEasingFunction;
        public IEasingFunction? GeneratedEasingFunction
        {
            get => _generatedEasingFunction; set
            {
                WritePreamble();
                _generatedEasingFunction = value;
            }
        }

        internal bool DynamicStoryboardCompleted, ExplicitStoryboardCompleted;


        #region Freezable

        protected override Freezable CreateInstanceCore() => new VisualTransition();

        protected override void CloneCore(Freezable sourceFreezable)
        {
            var source = (VisualTransition)sourceFreezable;
            source._from = _from;
            source._to = _to;
            source._storyboard = _storyboard;
            source._generatedDuration = _generatedDuration;
            source._generatedEasingFunction = _generatedEasingFunction;
            base.CloneCore(sourceFreezable);
        }

        protected override void CloneCurrentValueCore(Freezable sourceFreezable)
        {
            var source = (VisualTransition)sourceFreezable;
            source._from = _from;
            source._to = _to;
            source._storyboard = _storyboard;
            source._generatedDuration = _generatedDuration;
            source._generatedEasingFunction = _generatedEasingFunction;
            base.CloneCurrentValueCore(sourceFreezable);
        }

        protected override void GetAsFrozenCore(Freezable sourceFreezable)
        {
            var source = (VisualTransition)sourceFreezable;
            source._from = _from;
            source._to = _to;
            source._storyboard = _storyboard;
            source._generatedDuration = _generatedDuration;
            source._generatedEasingFunction = _generatedEasingFunction;
            base.GetAsFrozenCore(sourceFreezable);
        }

        protected override void GetCurrentValueAsFrozenCore(Freezable sourceFreezable)
        {
            var source = (VisualTransition)sourceFreezable;
            source._from = _from;
            source._to = _to;
            source._storyboard = _storyboard;
            source._generatedDuration = _generatedDuration;
            source._generatedEasingFunction = _generatedEasingFunction;
            base.GetCurrentValueAsFrozenCore(sourceFreezable);
        }

        protected override bool FreezeCore(bool isChecking)
        {
            if (isChecking)
                return _from != null && _to != null && _storyboard != null;
            else
            {
                _storyboard?.Freeze();
            }
            return true;
        }

        #endregion
    }
}
