using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using Wodsoft.UI.Media;
using Wodsoft.UI.Media.Animation;

namespace Wodsoft.UI
{
    [ContentProperty("States")]
    [RuntimeNameProperty("Name")]
    public class VisualStateGroup : Freezable
    {
        private Dictionary<string, VisualState>? _stateCache;

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

        private FreezableCollection<VisualState>? _states;
        public IList<VisualState> States
        {
            get
            {
                if (_states == null)
                {
                    _states = new FreezableCollection<VisualState>();
                    if (IsFrozen)
                        _states.Freeze();
                }
                return _states;
            }
        }

        private FreezableCollection<VisualTransition>? _transitions;
        public IList<VisualTransition> Transitions
        {
            get
            {
                if (_transitions == null)
                {
                    _transitions = new FreezableCollection<VisualTransition>();
                    if (IsFrozen)
                        _transitions.Freeze();
                }
                return _transitions;
            }
        }

        public VisualState? GetState(string stateName)
        {
            if (_stateCache != null)
            {
                _stateCache.TryGetValue(stateName, out var state);
                return state;
            }
            return null;
        }

        public VisualTransition? GetTransitions(string? from, string? to)
        {
            if (_transitions == null)
                return null;

            VisualTransition? best = null;
            VisualTransition? defaultTransition = null;
            int bestScore = -1;
            for (int i = 0; i < _transitions.Count; i++)
            {
                var transition = _transitions[i];
                if (defaultTransition == null && transition.From == null && transition.To == null)
                {
                    defaultTransition = transition;
                    continue;
                }

                int score = -1;

                if (transition.From == from)
                {
                    score += 1;
                }
                else if (from != null)
                {
                    continue;
                }

                if (transition.To == to)
                {
                    score += 2;
                }
                else if (to != null)
                {
                    continue;
                }

                if (score > bestScore)
                {
                    bestScore = score;
                    best = transition;
                }
            }
            return best ?? defaultTransition;
        }

        private VisualState? _currentState;
        public VisualState? CurrentState => _currentState;

        #region Transition

        private List<Storyboard> _storyboards = new List<Storyboard>();
        private List<VisualStateTrigger> _triggeres = new List<VisualStateTrigger>();
        private List<EventSetter> _eventSetters = new List<EventSetter>();

        internal bool GoToState(FrameworkElement control, FrameworkElement stateGroupsRoot, VisualState state, bool useTransitions)
        {
            VisualState? lastState = _currentState;
            if (state == lastState)
                return true;

            for (int i = 0; i < _triggeres.Count; i++)
                _triggeres[i].DisconnectTrigger(control, stateGroupsRoot, null);
            _triggeres.Clear();
            for (int i = 0; i < _eventSetters.Count; i++)
                control.RemoveHandler(_eventSetters[i].Event!, _eventSetters[i].Handler!);
            _eventSetters.Clear();

            var setters = state.InternalSetters;
            if (setters != null)
            {
                for (int i = 0; i < setters.Count; i++)
                {
                    var setterBase = setters[i];
                    if (setterBase is Setter setter)
                    {
                        var trigger = new VisualStateTrigger(setter);
                        _triggeres.Add(trigger);
                        trigger.ConnectTrigger(control, stateGroupsRoot, null);
                    }
                    else if (setterBase is EventSetter eventSetter)
                    {
                        _eventSetters.Add(eventSetter);
                        control.AddHandler(eventSetter.Event!, eventSetter.Handler!);
                    }
                }
            }

            VisualTransition? transition = useTransitions ? GetTransitions(lastState?.Name, state.Name) : null;
            if (transition == null || (transition.GeneratedDuration == Duration.Zero && (transition.Storyboard == null || transition.Storyboard.Duration == Duration.Zero)))
            {
                if (transition != null && transition.Storyboard != null)
                    BeginStoryboards(stateGroupsRoot, transition.Storyboard, state.Storyboard!);
                else
                    BeginStoryboards(stateGroupsRoot, state.Storyboard!);
            }
            else
            {
                Storyboard dynamicTransition = GenerateDynamicTransitionAnimations(stateGroupsRoot, state, transition);

                dynamicTransition.Completed += (sender, e) =>
                {
                    if (transition.Storyboard == null || transition.ExplicitStoryboardCompleted)
                    {
                        if (state == _currentState)
                        {
                            BeginStoryboards(stateGroupsRoot, state.Storyboard);
                        }

                        //RaiseCurrentStateChanged(stateGroupsRoot, lastState, state, control);
                    }

                    transition.DynamicStoryboardCompleted = true;
                };
                if (transition.Storyboard != null && transition.ExplicitStoryboardCompleted == true)
                {
                    EventHandler? transitionCompleted = null;
                    transitionCompleted = (sender, e) =>
                    {
                        if (transition.DynamicStoryboardCompleted)
                        {
                            if (state == _currentState)
                            {
                                BeginStoryboards(stateGroupsRoot, state.Storyboard);
                            }
                            //RaiseCurrentStateChanged(stateGroupsRoot, lastState, state, control);
                        }

                        transition.Storyboard.Completed -= transitionCompleted;
                        transition.ExplicitStoryboardCompleted = true;
                    };

                    // hook up explicit storyboard's Completed event handler
                    transition.ExplicitStoryboardCompleted = false;
                    transition.Storyboard.Completed += transitionCompleted;
                }

                BeginStoryboards(stateGroupsRoot, transition.Storyboard, dynamicTransition);
            }
            _currentState = state;
            return true;
        }

        private void BeginStoryboards(FrameworkElement root, params Storyboard?[] storyboards)
        {
            var currentStoryboards = _storyboards;
            for (int i = 0; i < currentStoryboards.Count; i++)
            {
                currentStoryboards[i].Remove(root);
            }
            currentStoryboards.Clear();
            for (int i = 0; i < storyboards.Length; i++)
            {
                var storyboard = storyboards[i];
                if (storyboard == null)
                    continue;
                storyboard.Begin(root, HandoffBehavior.SnapshotAndReplace, true);
                currentStoryboards.Add(storyboard);
            }
        }

        private Storyboard GenerateDynamicTransitionAnimations(FrameworkElement root, VisualState newState, VisualTransition? transition)
        {
            IEasingFunction? easingFunction = null;
            Storyboard dynamic = new Storyboard();

            if (transition != null)
            {
                if (transition.GeneratedDuration != Duration.Zero)
                {
                    dynamic.Duration = transition.GeneratedDuration;
                }

                easingFunction = transition.GeneratedEasingFunction;
            }
            else
            {
                dynamic.Duration = new Duration(TimeSpan.Zero);
            }

            Dictionary<TimelineDataToken, Timeline> currentAnimations = FlattenTimelines(_storyboards);
            Dictionary<TimelineDataToken, Timeline> transitionAnimations = FlattenTimelines(transition != null ? transition.Storyboard : null);
            Dictionary<TimelineDataToken, Timeline> newStateAnimations = FlattenTimelines(newState.Storyboard);

            // Remove any animations that the transition already animates.
            // There is no need to create an interstitial animation if one already exists.
            foreach (KeyValuePair<TimelineDataToken, Timeline> pair in transitionAnimations)
            {
                currentAnimations.Remove(pair.Key);
                newStateAnimations.Remove(pair.Key);
            }

            // Generate the "to" animations
            foreach (KeyValuePair<TimelineDataToken, Timeline> pair in newStateAnimations)
            {
                // The new "To" Animation -- the root is passed as a reference point for name
                // lookup.  
                Timeline? toAnimation = GenerateToAnimation(root, pair.Value, easingFunction, true);

                // If the animation is of a type that we can't generate transition animations
                // for, GenerateToAnimation will return null, and we should just keep going.
                if (toAnimation != null)
                {
                    toAnimation.Duration = dynamic.Duration;
                    dynamic.Children.Add(toAnimation);
                }

                // Remove this from the list of current state animations we have to consider next
                currentAnimations.Remove(pair.Key);
            }

            // Generate the "from" animations
            foreach (KeyValuePair<TimelineDataToken, Timeline> pair in currentAnimations)
            {
                Timeline? fromAnimation = GenerateFromAnimation(root, pair.Value, easingFunction);
                if (fromAnimation != null)
                {
                    fromAnimation.Duration = dynamic.Duration;
                    dynamic.Children.Add(fromAnimation);
                }
            }

            return dynamic;
        }

        private static Dictionary<TimelineDataToken, Timeline> FlattenTimelines(Storyboard? storyboard)
        {
            Dictionary<TimelineDataToken, Timeline> result = new Dictionary<TimelineDataToken, Timeline>();

            FlattenTimelines(storyboard, result);

            return result;
        }

        private static Dictionary<TimelineDataToken, Timeline> FlattenTimelines(List<Storyboard> storyboards)
        {
            Dictionary<TimelineDataToken, Timeline> result = new Dictionary<TimelineDataToken, Timeline>();

            for (int index = 0; index < storyboards.Count; ++index)
            {
                FlattenTimelines(storyboards[index], result);
            }

            return result;
        }

        private static void FlattenTimelines(Storyboard? storyboard, Dictionary<TimelineDataToken, Timeline> result)
        {
            if (storyboard == null)
            {
                return;
            }

            for (int index = 0; index < storyboard.Children.Count; ++index)
            {
                Timeline child = storyboard.Children[index];
                Storyboard? childStoryboard = child as Storyboard;
                if (childStoryboard != null)
                {
                    FlattenTimelines(childStoryboard, result);
                }
                else
                {
                    result[new TimelineDataToken(child)] = child;
                }
            }
        }

        private static Timeline? GenerateFromAnimation(FrameworkElement root, Timeline timeline, IEasingFunction? easingFunction)
        {
            Timeline? result = null;

            //if (timeline is ColorAnimation || timeline is ColorAnimationUsingKeyFrames)
            //{
            //    result = new ColorAnimation() { EasingFunction = easingFunction };
            //}
            //else if (timeline is DoubleAnimation || timeline is DoubleAnimationUsingKeyFrames)
            //{
            //    result = new DoubleAnimation() { EasingFunction = easingFunction };
            //}
            //else if (timeline is PointAnimation || timeline is PointAnimationUsingKeyFrames)
            //{
            //    result = new PointAnimation() { EasingFunction = easingFunction };
            //}

            if (result != null)
            {
                CopyStoryboardTargetProperties(root, timeline, result);
            }

            // All other animation types are ignored. We will not build transitions for them,
            // but they will end up being executed.
            return result;
        }

        private static Timeline? GenerateToAnimation(FrameworkElement root, Timeline timeline, IEasingFunction? easingFunction, bool isEntering)
        {
            Timeline? result = null;

            //Color? targetColor = GetTargetColor(timeline, isEntering);
            //if (targetColor.HasValue)
            //{
            //    ColorAnimation ca = new ColorAnimation() { To = targetColor, EasingFunction = easingFunction };
            //    result = ca;
            //}

            //if (result == null)
            //{
            //    double? targetDouble = GetTargetDouble(timeline, isEntering);
            //    if (targetDouble.HasValue)
            //    {
            //        DoubleAnimation da = new DoubleAnimation() { To = targetDouble, EasingFunction = easingFunction };
            //        result = da;
            //    }
            //}

            //if (result == null)
            //{
            //    Point? targetPoint = GetTargetPoint(timeline, isEntering);
            //    if (targetPoint.HasValue)
            //    {
            //        PointAnimation pa = new PointAnimation() { To = targetPoint, EasingFunction = easingFunction };
            //        result = pa;
            //    }
            //}

            if (result != null)
            {
                CopyStoryboardTargetProperties(root, timeline, result);
            }

            return result;
        }

        private static void CopyStoryboardTargetProperties(FrameworkElement root, Timeline source, Timeline destination)
        {
            // Target takes priority over TargetName
            string? targetName = Storyboard.GetTargetName(source);
            DependencyObject? target = Storyboard.GetTarget(source);
            PropertyPath? path = Storyboard.GetTargetProperty(source);

            if (target == null && !string.IsNullOrEmpty(targetName))
            {
                target = root.FindName(targetName) as DependencyObject;
            }

            if (targetName != null)
            {
                Storyboard.SetTargetName(destination, targetName);
            }

            if (target != null)
            {
                Storyboard.SetTarget(destination, target);
            }

            if (path != null)
            {
                Storyboard.SetTargetProperty(destination, path);
            }
        }

        private struct TimelineDataToken : IEquatable<TimelineDataToken>
        {
            public TimelineDataToken(Timeline timeline)
            {
                _target = Storyboard.GetTarget(timeline);
                _targetName = Storyboard.GetTargetName(timeline);
                _targetProperty = Storyboard.GetTargetProperty(timeline);
            }

            public bool Equals(TimelineDataToken other)
            {
                bool targetsEqual = false;
                if (_targetName != null)
                {
                    targetsEqual = other._targetName == _targetName;
                }
                else if (_target != null)
                {
                    targetsEqual = other._target == _target;
                }
                else
                {
                    targetsEqual = (other._target == null && other._targetName == null);
                }

                if (targetsEqual && _targetProperty != null && other._targetProperty != null &&
                    (other._targetProperty.Path == _targetProperty.Path) &&
                    (other._targetProperty.PathParameters.Count == _targetProperty.PathParameters.Count))
                {
                    bool paramsEqual = true;

                    for (int i = 0, count = _targetProperty.PathParameters.Count; i < count; i++)
                    {
                        if (other._targetProperty.PathParameters[i] != _targetProperty.PathParameters[i])
                        {
                            paramsEqual = false;
                            break;
                        }
                    }

                    return paramsEqual;
                }

                return false;
            }

            public override int GetHashCode()
            {
                //
                // The below code has some limitations.  We don't handle canonicalizing property paths, so
                // having two paths that target the same object/property can easily get different hash codes.
                //
                // For example the Opacity can be specified either from a string "Opacity" or via the string "(0)"
                // and a parameter Visual.OpacityPropety.  These wont match as far as VSM is concerned.
                //
                int targetHash = _target != null ? _target.GetHashCode() : 0;
                int targetNameHash = _targetName != null ? _targetName.GetHashCode() : 0;
                int targetPropertyHash = (_targetProperty != null && _targetProperty.Path != null) ? _targetProperty.Path.GetHashCode() : 0;

                return ((_targetName != null) ? targetNameHash : targetHash) ^ targetPropertyHash;
            }

            private DependencyObject? _target;
            private string? _targetName;
            private PropertyPath? _targetProperty;
        }

        #endregion

        #region Freezable

        protected override Freezable CreateInstanceCore() => new VisualStateGroup();

        protected override void CloneCore(Freezable sourceFreezable)
        {
            var source = (VisualStateGroup)sourceFreezable;
            _name = source.Name;
            _states = source._states;
            _transitions = source._transitions;
            base.CloneCore(sourceFreezable);
        }

        protected override void CloneCurrentValueCore(Freezable sourceFreezable)
        {
            var source = (VisualStateGroup)sourceFreezable;
            _name = source.Name;
            _states = source._states;
            _transitions = source._transitions;
            base.CloneCurrentValueCore(sourceFreezable);
        }

        protected override void GetAsFrozenCore(Freezable sourceFreezable)
        {
            var source = (VisualStateGroup)sourceFreezable;
            _name = source.Name;
            _states = source._states;
            _transitions = source._transitions;
            base.GetAsFrozenCore(sourceFreezable);
        }

        protected override void GetCurrentValueAsFrozenCore(Freezable sourceFreezable)
        {
            var source = (VisualStateGroup)sourceFreezable;
            _name = source.Name;
            _states = source._states;
            _transitions = source._transitions;
            base.GetCurrentValueAsFrozenCore(sourceFreezable);
        }

        protected override bool FreezeCore(bool isChecking)
        {
            if (isChecking)
            {
                if (_transitions != null)
                {
                    List<string> states = _states == null ? new List<string>() : _states.Select(t => t.Name!).ToList();
                    foreach (var transition in _transitions)
                    {
                        if (transition.From != null && !states.Contains(transition.From))
                            return false;
                        if (transition.To != null && !states.Contains(transition.To))
                            return false;
                    }
                }
            }
            else
            {
                _transitions?.Freeze();
                if (_states != null)
                {
                    _states.Freeze();
                    _stateCache = _states.ToDictionary(t => t.Name!, t => t);
                }
            }
            return true;
        }

        #endregion
    }
}
