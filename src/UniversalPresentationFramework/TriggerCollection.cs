using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Controls;

namespace Wodsoft.UI
{
    public class TriggerCollection : Collection<TriggerBase>, ISealable
    {
        private bool _sealed;
        private readonly FrameworkElement? _owner;

        #region Constructors

        internal TriggerCollection()
        {
        }

        internal TriggerCollection(FrameworkElement owner)
        {
            _sealed = false;
            _owner = owner;
        }

        #endregion Constructors

        #region ProtectedMethods

        /// <summary>
        ///     ClearItems override
        /// </summary>
        protected override void ClearItems()
        {
            CheckSealed();
            OnClear();
            base.ClearItems();
        }

        /// <summary>
        ///     InsertItem override
        /// </summary>
        protected override void InsertItem(int index, TriggerBase item)
        {
            CheckSealed();
            if (item == null)
                throw new ArgumentNullException("triggerBase");
            OnAdd(item);
            base.InsertItem(index, item);
        }

        /// <summary>
        ///     RemoveItem override
        /// </summary>
        protected override void RemoveItem(int index)
        {
            CheckSealed();
            TriggerBase triggerBase = this[index];
            OnRemove(triggerBase);
            base.RemoveItem(index);
        }

        /// <summary>
        ///     SetItem override
        /// </summary>
        protected override void SetItem(int index, TriggerBase item)
        {
            CheckSealed();
            if (item == null)
                throw new ArgumentNullException("triggerBase");
            OnAdd(item);
            base.SetItem(index, item);
        }

        #endregion ProtectedMethods

        #region Seal

        /// <summary>
        ///     Returns the sealed state of this object.  If true, any attempt
        ///     at modifying the state of this object will trigger an exception.
        /// </summary>
        public bool IsSealed
        {
            get
            {
                return _sealed;
            }
        }
        
        public void Seal()
        {
            _sealed = true;

            // Seal all the setters
            for (int i = 0; i < Count; i++)
            {
                this[i].Seal();
            }
        }

        private void CheckSealed()
        {
            if (_sealed)
                throw new InvalidOperationException("Trigger collection is sealed.");
        }

        #endregion

        #region InternalMethods

        // This may be null (i.e. when used in a style or template)
        internal FrameworkElement? Owner
        {
            get { return _owner; }
        }

        #endregion InternalMethods

        #region PrivateMethods

        // Called by GenericCollection.tb when a trigger is added to the collection.
        // We use this opportunity to hook it into the tree.
        private void OnAdd(TriggerBase triggerBase)
        {
            //// If we don't have an Owner (the Style/Template case), or the
            //// element isn't initialized yet, we don't need to do anything
            //if (Owner != null && Owner.IsInitialized)
            //{
            //    EventTrigger.ProcessOneTrigger(Owner, triggerBase);
            //}

            //InheritanceContextHelper.ProvideContextForObject(Owner, triggerBase);
        }

        // Called by GenericCollection.tb when a trigger is removed from the collection.
        // We use this opportunity to pull its hooks out of the tree.
        private void OnRemove(TriggerBase triggerBase)
        {
            //// If we don't have an Owner (the Style/Template case), 
            //// we don't need to do anything
            //if (Owner != null)
            //{
            //    // If the owner is initialized, we need to disconnect the trigger.
            //    if (Owner.IsInitialized)
            //    {
            //        EventTrigger.DisconnectOneTrigger(Owner, triggerBase);
            //    }

            //    // We always need to update the inheritance context

            //    InheritanceContextHelper.RemoveContextFromObject(Owner, triggerBase);
            //}
        }

        // Called by GenericCollection.tb when the collection is cleared.
        // We use this opportunity to pull all the hooks out of the tree.
        private void OnClear()
        {
            //// If we don't have an Owner (the Style/Template case), 
            //// we don't need to do anything
            //if (Owner != null)
            //{
            //    // If the owner is initialized, we need to disconnect all the trigger.
            //    if (Owner.IsInitialized)
            //    {
            //        EventTrigger.DisconnectAllTriggers(Owner);
            //    }

            //    // We always need to update the inheritance context
            //    for (int i = Count - 1; i >= 0; i--)
            //    {
            //        InheritanceContextHelper.RemoveContextFromObject(Owner, this[i]);
            //    }
            //}
        }

        #endregion PrivateMethods
    }
}
