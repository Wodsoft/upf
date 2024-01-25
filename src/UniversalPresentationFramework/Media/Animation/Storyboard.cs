using System;
using System.Collections.Generic;
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
            if (FrameworkProvider.ClockProvider == null)
                throw new InvalidOperationException("Framework not initialized.");
            if (BeginTime == null)
                return;

            var clock = CreateClock(isControllable);
            HandleClock(container, clock, null, null, null, handoffBehavior);
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

        #endregion

        #region Clone

        protected override Freezable CreateInstanceCore()
        {
            return new Storyboard();
        }

        #endregion
    }
}
