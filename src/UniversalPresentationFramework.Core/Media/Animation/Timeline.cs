﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public abstract class Timeline : Animatable
    {
        #region Properties

        public static readonly DependencyProperty AccelerationRatioProperty =
            DependencyProperty.Register(
                "AccelerationRatio",
                typeof(float),
                typeof(Timeline),
                new PropertyMetadata(0.0f),
                new ValidateValueCallback(ValidateAccelerationDecelerationRatio));
        private static bool ValidateAccelerationDecelerationRatio(object? value)
        {
            if (value is float newValue)
            {
                if (newValue < 0 || newValue > 1 || float.IsNaN(newValue))
                    return false;
                return true;
            }
            return false;
        }
        public float AccelerationRatio { get { return (float)GetValue(AccelerationRatioProperty)!; } set { SetValue(AccelerationRatioProperty, value); } }

        public static readonly DependencyProperty AutoReverseProperty =
            DependencyProperty.Register(
                "AutoReverse",
                typeof(bool),
                typeof(Timeline),
                new PropertyMetadata(false));
        public bool AutoReverse { get { return (bool)GetValue(AutoReverseProperty)!; } set { SetValue(AutoReverseProperty, value); } }

        public static readonly DependencyProperty BeginTimeProperty =
            DependencyProperty.Register(
                "BeginTime",
                typeof(TimeSpan?),
                typeof(Timeline),
                new PropertyMetadata((TimeSpan?)TimeSpan.Zero));
        public TimeSpan? BeginTime { get { return (TimeSpan?)GetValue(BeginTimeProperty); } set { SetValue(BeginTimeProperty, value); } }


        public static readonly DependencyProperty DecelerationRatioProperty =
            DependencyProperty.Register(
                "DecelerationRatio",
                typeof(float),
                typeof(Timeline),
                new PropertyMetadata(0.0f),
                new ValidateValueCallback(ValidateAccelerationDecelerationRatio));
        public float DecelerationRatio { get { return (float)GetValue(DecelerationRatioProperty)!; } set { SetValue(DecelerationRatioProperty, value); } }

        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register(
                "Duration",
                typeof(Duration),
                typeof(Timeline),
                new PropertyMetadata(Duration.Automatic));
        public Duration Duration { get { return (Duration)GetValue(DurationProperty)!; } set { SetValue(DurationProperty, value); } }

        public static readonly DependencyProperty FillBehaviorProperty =
            DependencyProperty.Register(
                "FillBehavior",
                typeof(FillBehavior),
                typeof(Timeline),
                new PropertyMetadata(FillBehavior.HoldEnd));
        public FillBehavior FillBehavior { get { return (FillBehavior)GetValue(FillBehaviorProperty)!; } set { SetValue(FillBehaviorProperty, value); } }

        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register(
                        "Name",
                        typeof(string),
                        typeof(Timeline),
                        new PropertyMetadata(string.Empty),
                        new ValidateValueCallback(NameValidationCallback));
        private static bool NameValidationCallback(object? candidateName)
        {
            string? name = candidateName as string;
            if (name != null)
            {
                // Non-null string, ask the XAML validation code for blessing.
                return NameScope.IsValidIdentifierName(name);
            }
            else if (candidateName == null)
            {
                // Null string is allowed
                return true;
            }
            else
            {
                // candiateName is not a string object.
                return false;
            }
        }
        public string? Name { get { return (string?)GetValue(NameProperty); } set { SetValue(NameProperty, value); } }

        public static readonly DependencyProperty RepeatBehaviorProperty =
            DependencyProperty.Register(
                "RepeatBehavior",
                typeof(RepeatBehavior),
                typeof(Timeline),
                new PropertyMetadata(new RepeatBehavior(1f)));
        public RepeatBehavior RepeatBehavior { get { return (RepeatBehavior)GetValue(RepeatBehaviorProperty)!; } set { SetValue(RepeatBehaviorProperty, value); } }

        public static readonly DependencyProperty SpeedRatioProperty =
            DependencyProperty.Register(
                "SpeedRatio",
                typeof(float),
                typeof(Timeline),
                new PropertyMetadata(1f),
                new ValidateValueCallback(ValidateSpeedRatio));
        private static bool ValidateSpeedRatio(object? value)
        {
            if (value is float newValue)
            {
                if (newValue <= 0 || newValue > float.MaxValue || float.IsNaN(newValue))
                    return false;
                return true;
            }
            return false;
        }
        public float SpeedRatio { get { return (float)GetValue(SpeedRatioProperty)!; } set { SetValue(SpeedRatioProperty, value); } }

        #endregion

        #region InheritanceContext

        private static readonly DependencyProperty _ParentProperty = DependencyProperty.Register("Parent", typeof(Timeline), typeof(Timeline));

        internal void SetParent(Timeline timeline)
        {
            AddInheritanceContext(timeline, _ParentProperty);
        }

        internal void RemoveParent(Timeline timeline)
        {
            RemoveInheritanceContext(timeline, _ParentProperty);
        }

        #endregion

        #region Clock

        protected internal virtual Clock AllocateClock()
        {
            return new Clock(this);
        }

        public Clock CreateClock()
        {
            return CreateClock(true);
        }

        public Clock CreateClock(bool hasControllableRoot)
        {
            var clock = AllocateClock();
            clock.IsRoot = true;
            clock.HasControllableRoot = hasControllableRoot;
            clock.Measure();
            FrameworkCoreProvider.GetClockProvider().RegisterClock(this, clock);
            return clock;
        }

        #endregion

        #region Events

        public event EventHandler? Completed;

        public event EventHandler? CurrentTimeInvalidated;

        public event EventHandler? CurrentStateInvalidated;

        public event EventHandler? RemoveRequested;

        internal void RaiseCompleted(object sender)
        {
            Completed?.Invoke(sender, EventArgs.Empty);
        }

        internal void RaiseCurrentTimeInvalidated(object sender)
        {
            CurrentTimeInvalidated?.Invoke(sender, EventArgs.Empty);
        }

        internal void RaiseCurrentStateInvalidated(object sender)
        {
            CurrentStateInvalidated?.Invoke(sender, EventArgs.Empty);
        }

        internal void RaiseRemoveRequested(object sender)
        {
            RemoveRequested?.Invoke(sender, EventArgs.Empty);
        }

        #endregion
    }
}
