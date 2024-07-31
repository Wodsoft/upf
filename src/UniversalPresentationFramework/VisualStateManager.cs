using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public class VisualStateManager
    {
        private static readonly VisualStateManager _Default = new VisualStateManager();

        #region Properties

        private static readonly DependencyPropertyKey _VisualStateGroupsPropertyKey =
            DependencyProperty.RegisterAttachedReadOnly(
            "VisualStateGroups",
            typeof(IList<VisualStateGroup>),
            typeof(VisualStateManager),
            new FrameworkPropertyMetadata(new ObservableCollectionDefaultValueFactory<VisualStateGroup>()));
        public static readonly DependencyProperty VisualStateGroupsProperty = _VisualStateGroupsPropertyKey.DependencyProperty;
        public static IList<VisualStateGroup> GetVisualStateGroups(DependencyObject d)
        {
            if (d == null)
                throw new ArgumentNullException("d");
            return (IList<VisualStateGroup>)d.GetValue(VisualStateGroupsProperty)!;
        }


        public static readonly DependencyProperty CustomVisualStateManagerProperty = DependencyProperty.RegisterAttached("CustomVisualStateManager", typeof(VisualStateManager), typeof(VisualStateManager));
        public static VisualStateManager? GetCustomVisualStateManager(DependencyObject d)
        {
            if (d == null)
                throw new ArgumentNullException("d");

            return (VisualStateManager?)d.GetValue(CustomVisualStateManagerProperty);
        }

        public static void SetCustomVisualStateManager(FrameworkElement d, VisualStateManager? value)
        {
            if (d == null)
                throw new ArgumentNullException("d");
            if (value == null)
                d.ClearValue(CustomVisualStateManagerProperty);
            else
                d.SetValue(CustomVisualStateManagerProperty, value);
        }

        #endregion

        #region State Control

        public static bool GoToState(FrameworkElement control, string stateName, bool useTransitions)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));
            if (stateName == null)
                throw new ArgumentNullException(nameof(stateName));

            var templatedChild = control.TemplatedChild;
            if (templatedChild == null)
                return false;

            ref readonly var effectiveValue = ref templatedChild.GetEffectiveValue(VisualStateGroupsProperty);
            if (effectiveValue.Source != DependencyEffectiveSource.Local)
                return false;
            var groups = (IList<VisualStateGroup>)effectiveValue.Value!;
            VisualStateGroup? group = null;
            VisualState? state = null;
            for (int i = 0; i < groups.Count; i++)
            {
                group = groups[i];
                if (!group.IsFrozen)
                    group.Freeze();
                state = group.GetState(stateName);
                if (state != null)
                    break;
            }
            if (state == null)
                return false;
            var manager = GetCustomVisualStateManager(templatedChild) ?? _Default;
            return manager.GoToState(control, templatedChild, group!, state, useTransitions);
        }

        protected virtual bool GoToState(FrameworkElement control, FrameworkElement stateGroupsRoot, VisualStateGroup group, VisualState state, bool useTransitions)
        {
            return group.GoToState(control, stateGroupsRoot, state, useTransitions);
        }

        #endregion
    }
}
