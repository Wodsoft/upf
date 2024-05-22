using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Threading;

namespace Wodsoft.UI
{
    public abstract class Freezable : DependencyObject
    {
        #region Freeze

        private bool _isFrozen;

        public virtual bool CanFreeze => !_isFrozen && FreezeCore(true);

        public bool IsFrozen => _isFrozen;

        public void Freeze()
        {
            if (!CanFreeze)
                throw new InvalidOperationException("Object can't freeze.");
            WritePreamble();
            FreezeCore(false);
            _isFrozen = true;
        }

        protected virtual bool FreezeCore(bool isChecking) => true;

        protected override void SetValueCore(DependencyProperty dp, object? value)
        {
            WritePreamble();
            base.SetValueCore(dp, value);
        }

        protected virtual void ReadPreamble() { }

        protected virtual void WritePreamble()
        {
            if (_isFrozen)
                throw new InvalidOperationException("Object is frozen.");
        }

        #endregion

        #region Clone

        public Freezable Clone()
        {
            Freezable clone = CreateInstance();
            clone.CloneCore(this);
            return clone;
        }

        protected Freezable CreateInstance()
        {
            return CreateInstanceCore();
        }

        protected abstract Freezable CreateInstanceCore();

        protected virtual void CloneCore(Freezable sourceFreezable)
        {
            CloneCoreCommon(sourceFreezable,
                /* useCurrentValue = */ false,
                /* cloneFrozenValues = */ true);
        }

        public Freezable CloneCurrentValue()
        {
            Freezable clone = CreateInstance();
            clone.CloneCurrentValueCore(this);
            return clone;
        }

        protected virtual void CloneCurrentValueCore(Freezable sourceFreezable)
        {
            CloneCoreCommon(sourceFreezable,
                /* useCurrentValue = */ true,
                /* cloneFrozenValues = */ true);
        }

        private void CloneCoreCommon(Freezable sourceFreezable, bool useCurrentValue, bool cloneFrozenValues)
        {
            foreach (var entry in sourceFreezable.GetEffectiveValues())
            {
                if (entry.Property.ReadOnly)
                    continue;
                object? sourceValue;
                if (useCurrentValue)
                {
                    sourceValue = entry.Value.Value;
                }
                else
                {
                    if (entry.Value.Source == DependencyEffectiveSource.Local || entry.Value.Source == DependencyEffectiveSource.Expression)
                        sourceValue = entry.Value.Value;
                    //else if (entry.Value.Source == DependencyEffectiveSource.Expression)
                    //    sourceValue = entry.Value.Expression!.Value;
                    else
                        continue;
                }
                Freezable? valueAsFreezable = sourceValue as Freezable;

                if (valueAsFreezable != null)
                {
                    Freezable valueAsFreezableClone;

                    //
                    // Choose between the four possible ways of
                    // cloning a Freezable
                    //
                    if (cloneFrozenValues) //CloneCore and CloneCurrentValueCore
                    {
                        valueAsFreezableClone = valueAsFreezable.CreateInstanceCore();

                        if (useCurrentValue)
                        {
                            // CloneCurrentValueCore implementation.  We clone even if the
                            // Freezable is frozen by recursing into CloneCurrentValueCore.
                            valueAsFreezableClone.CloneCurrentValueCore(valueAsFreezable);
                        }
                        else
                        {
                            // CloneCore implementation.  We clone even if the Freezable is
                            // frozen by recursing into CloneCore.
                            valueAsFreezableClone.CloneCore(valueAsFreezable);
                        }

                        sourceValue = valueAsFreezableClone;
                    }
                    else // skip cloning frozen values
                    {
                        if (!valueAsFreezable.IsFrozen)
                        {
                            valueAsFreezableClone = valueAsFreezable.CreateInstanceCore();

                            if (useCurrentValue)
                            {
                                // GetCurrentValueAsFrozenCore implementation.  Only clone if the
                                // Freezable is mutable by recursing into GetCurrentValueAsFrozenCore.
                                valueAsFreezableClone.GetCurrentValueAsFrozenCore(valueAsFreezable);
                            }
                            else
                            {
                                // GetAsFrozenCore implementation.  Only clone if the Freezable is
                                // mutable by recursing into GetAsFrozenCore.
                                valueAsFreezableClone.GetAsFrozenCore(valueAsFreezable);
                            }

                            sourceValue = valueAsFreezableClone;
                        }
                    }
                }

                SetValue(entry.Property, sourceValue);
            }
        }

        public Freezable GetAsFrozen()
        {
            if (_isFrozen)
                return this;
            Freezable clone = CreateInstance();
            clone.GetAsFrozenCore(this);
            clone.Freeze();
            return clone;
        }

        protected virtual void GetAsFrozenCore(Freezable sourceFreezable)
        {
            CloneCoreCommon(sourceFreezable,
                /* useCurrentValue = */ false,
                /* cloneFrozenValues = */ false);
        }

        public Freezable GetCurrentValueAsFrozen()
        {
            if (_isFrozen)
                return this;
            Freezable clone = CreateInstance();
            clone.GetCurrentValueAsFrozenCore(this);
            clone.Freeze();
            return clone;
        }

        protected virtual void GetCurrentValueAsFrozenCore(Freezable sourceFreezable)
        {
            CloneCoreCommon(sourceFreezable,
                /* useCurrentValue = */ true,
                /* cloneFrozenValues = */ false);
        }

        #endregion

        #region InheritanceContext

        private List<(DependencyObject, DependencyProperty?)>? _inheritanceContext;

        public override DependencyObject? InheritanceContext
        {
            get
            {
                if (_inheritanceContext == null || _inheritanceContext.Count != 1)
                    return null;
                return _inheritanceContext[0].Item1;
            }
        }

        public override event EventHandler? InheritanceContextChanged;

        protected override void AddInheritanceContext(DependencyObject context, DependencyProperty? property)
        {
            if (IsInheritanceContextSealed)
                return;
            if (_inheritanceContext == null)
                _inheritanceContext = new List<(DependencyObject, DependencyProperty?)>();
            _inheritanceContext.Add((context, property));
            InheritanceContextChanged?.Invoke(this, EventArgs.Empty);
        }

        protected override void RemoveInheritanceContext(DependencyObject context, DependencyProperty? property)
        {
            if (IsInheritanceContextSealed)
                return;
            if (_inheritanceContext == null)
                return;
            _inheritanceContext.Remove((context, property));
            InheritanceContextChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region PropertyChange

        protected void OnFreezablePropertyChanged(DependencyObject? oldValue, DependencyObject? newValue)
        {
            OnFreezablePropertyChanged(oldValue, newValue, null);
        }

        protected void OnFreezablePropertyChanged(DependencyObject? oldValue, DependencyObject? newValue, DependencyProperty? property)
        {
            if (oldValue != null)
            {
                RemoveSelfAsInheritanceContext(oldValue, property);
            }

            if (newValue != null)
            {
                ProvideSelfAsInheritanceContext(newValue, property);
            }
        }

        #endregion

        #region Dispatcher

        public override Dispatcher Dispatcher
        {
            get
            {
                var inheritanceContext = InheritanceContext;
                if (inheritanceContext != null)
                    return inheritanceContext.Dispatcher;
                return base.Dispatcher;
            }
        }

        #endregion
    }
}
