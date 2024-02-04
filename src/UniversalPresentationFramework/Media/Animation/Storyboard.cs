using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using Wodsoft.UI.Controls;
using Wodsoft.UI.Data;

namespace Wodsoft.UI.Media.Animation
{
    public class Storyboard : ParallelTimeline
    {
        #region Properties

        private static readonly DependencyProperty _ClockProperty = DependencyProperty.RegisterAttached("Clock", typeof(Dictionary<Storyboard, WeakReference<Clock>>), typeof(Storyboard));
        public static readonly DependencyProperty TargetProperty = DependencyProperty.RegisterAttached("Target", typeof(DependencyObject), typeof(Storyboard));
        public static DependencyObject? GetTarget(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return (DependencyObject?)element.GetValue(TargetProperty);
        }
        public static void SetTarget(DependencyObject element, DependencyObject value)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            element.SetValue(TargetProperty, value);
        }

        public static readonly DependencyProperty TargetNameProperty = DependencyProperty.RegisterAttached("TargetName", typeof(string), typeof(Storyboard));
        public static string? GetTargetName(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return (string?)element.GetValue(TargetNameProperty);
        }
        public static void SetTargetName(DependencyObject element, String name)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            if (name == null)
                throw new ArgumentNullException("name");
            element.SetValue(TargetNameProperty, name);
        }

        public static readonly DependencyProperty TargetPropertyProperty = DependencyProperty.RegisterAttached("TargetProperty", typeof(PropertyPath), typeof(Storyboard));
        public static PropertyPath? GetTargetProperty(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return (PropertyPath?)element.GetValue(TargetPropertyProperty);
        }
        public static void SetTargetProperty(DependencyObject element, PropertyPath path)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            if (path == null)
                throw new ArgumentNullException("path");
            element.SetValue(TargetPropertyProperty, path);
        }


        #endregion

        #region Story

        public void Begin()
        {
            Begin(this, HandoffBehavior.SnapshotAndReplace, true);
        }

        public void Begin(DependencyObject container, HandoffBehavior handoffBehavior, bool isControllable)
        {
            Begin(container, null, handoffBehavior, isControllable);
        }

        public void Begin(FrameworkElement container, HandoffBehavior handoffBehavior, bool isControllable)
        {
            Begin(container, (INameScope?)null, handoffBehavior, isControllable);
        }

        public void Begin(FrameworkElement container, FrameworkTemplate template, HandoffBehavior handoffBehavior, bool isControllable)
        {
            if (container.GetTemplateInternal() != template)
                throw new InvalidOperationException("Template not equal to container's template.");
            INameScope? nameScope;
            if (container.TemplatedChild == null)
                nameScope = null;
            else
                nameScope = NameScope.GetNameScope(container.TemplatedChild);
            Begin(container, nameScope, handoffBehavior, isControllable);
        }

        public void Begin(DependencyObject container, INameScope? nameScope, HandoffBehavior handoffBehavior, bool isControllable)
        {
            if (FrameworkProvider.ClockProvider == null)
                throw new InvalidOperationException("Framework not initialized.");
            if (BeginTime == null)
                return;

            var clock = CreateClock(isControllable);
            HandleClock(container, clock, nameScope, null, null, handoffBehavior);
            if (isControllable)
                SetStoryboardClock(container, clock);
        }

        private void HandleClock(DependencyObject container, Clock clock, INameScope? nameScope, DependencyObject? parentObject, PropertyPath? parentPropertyPath,
            HandoffBehavior handoffBehavior)
        {
            var timeline = clock.Timeline;

            var targetObject = GetTarget(timeline);
            if (targetObject == null)
                targetObject = parentObject;
            var targetProperty = GetTargetProperty(timeline) ?? parentPropertyPath;

            if (clock is AnimationClock animationClock)
            {
                if (targetObject == null)
                {
                    var targetName = GetTargetName(timeline);
                    if (targetName == null)
                    {
                        targetObject = container as FrameworkElement;
                        if (targetObject == null)
                            targetObject = container as FrameworkContentElement;
                        if (targetObject == null)
                            throw new InvalidOperationException("Storyboard need a target.");
                    }
                    else
                    {
                        if (nameScope is Style)
                            throw new InvalidOperationException("TargetName not allowed in Style.");
                        targetObject = FindTarget(container, nameScope, targetName);
                        if (targetObject == null)
                            throw new InvalidOperationException("Storyboard can't find dependency object target.");
                    }
                }
                if (targetProperty == null)
                    throw new InvalidOperationException("Storyboard need a target property.");
                if (!targetProperty.SetContext(targetObject))
                    throw new InvalidOperationException("Storyboard property path unresolved. " + targetProperty.Path);
                if (targetProperty.LastBinding is DependencyPropertyBindingContext bindingContext)
                {
                    var property = bindingContext.Property;
                    targetObject = (DependencyObject)targetProperty.LastObject!;
                    var animatable = targetObject as IAnimatable;
                    if (animatable == null)
                        throw new InvalidOperationException("Storyboard target must be animatable.");
                    animatable.ApplyAnimationClock(property, animationClock, handoffBehavior);
                }
                else
                    throw new InvalidOperationException("Storyboard property path must be a dependency property. " + targetProperty.Path);

            }
            else if (clock is ClockGroup group)
            {
                for (int i = 0; i < group.Children.Count; i++)
                {
                    HandleClock(container, group.Children[i], nameScope, targetObject, targetProperty, handoffBehavior);
                }
            }

        }

        private DependencyObject? FindTarget(DependencyObject container, INameScope? nameScope, string name)
        {
            if (nameScope == null)
            {
                if (container is INameScope nameScope2)
                    return nameScope2.FindName(name) as DependencyObject;
                else
                {
                    var mentor = LogicalTreeHelper.FindMentor(container);
                    if (mentor is FrameworkElement fe)
                        return fe.FindName(name) as DependencyObject;
                    else if (mentor is FrameworkContentElement fce)
                        return fce.FindName(name) as DependencyObject;
                    else
                        return null;
                }
            }
            else
            {
                return nameScope.FindName(name) as DependencyObject;
            }
        }

        private void SetStoryboardClock(DependencyObject container, Clock clock)
        {
            var clocks = (Dictionary<Storyboard, WeakReference<Clock>>?)container.GetValue(_ClockProperty);
            if (clocks == null)
            {
                clocks = new Dictionary<Storyboard, WeakReference<Clock>>();
                container.SetValue(_ClockProperty, clocks);
            }
            clocks[this] = new WeakReference<Clock>(clock);
        }

        private Clock? GetStoryboardClock(DependencyObject container, bool remove = false)
        {
            var clocks = (Dictionary<Storyboard, WeakReference<Clock>>?)container.GetValue(_ClockProperty);
            if (clocks == null)
                return null;
            if (clocks.TryGetValue(this, out var reference))
            {
                if (reference.TryGetTarget(out var clock))
                {
                    if (remove)
                        clocks.Remove(this);
                    return clock;
                }
                clocks.Remove(this);
            }
            return null;
        }

        public void Pause()
        {
            Pause(this);
        }

        public void Pause(DependencyObject container)
        {
            GetStoryboardClock(container)?.Pause();
        }

        public void Resume()
        {
            Resume(this);
        }

        public void Resume(DependencyObject container)
        {
            GetStoryboardClock(container)?.Resume();
        }

        public void Stop()
        {
            Stop(this);
        }

        public void Stop(DependencyObject container)
        {
            GetStoryboardClock(container)?.Stop();
        }

        public void Remove()
        {
            Remove(this);
        }

        public void Remove(DependencyObject container)
        {
            GetStoryboardClock(container, true)?.Remove();
        }

        public void SetSpeedRatio(double speedRatio)
        {
            SetSpeedRatio(this, speedRatio);
        }

        public void SetSpeedRatio(DependencyObject containingObject, double speedRatio)
        {
            var clock = GetStoryboardClock(containingObject);
            if (clock != null)
                clock.Controller!.SpeedRatio = speedRatio;
        }

        public void Seek(TimeSpan offset)
        {
            Seek(this, offset, TimeSeekOrigin.BeginTime);
        }

        public void Seek(TimeSpan offset, TimeSeekOrigin origin)
        {
            Seek(this, offset, origin);
        }

        public void Seek(DependencyObject container, TimeSpan offset, TimeSeekOrigin origin)
        {
            Clock? clock = GetStoryboardClock(container);
            if (clock != null)
            {
                clock.Controller!.Seek(offset, origin);
            }
        }

        public void SkipToFill()
        {
            SkipToFill(this);
        }

        public void SkipToFill(DependencyObject container)
        {
            GetStoryboardClock(container)?.SkipToFill();
        }

        #endregion

        #region Clone

        protected override Freezable CreateInstanceCore()
        {
            return new Storyboard();
        }

        #endregion
    }
}
