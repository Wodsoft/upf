﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Controls;

namespace Wodsoft.UI
{
    public sealed class ConditionCollection : Collection<Condition>
    {
        #region ProtectedMethods

        /// <summary>
        ///     ClearItems override
        /// </summary>
        protected override void ClearItems()
        {
            CheckSealed();
            base.ClearItems();
        }

        /// <summary>
        ///     InsertItem override
        /// </summary>
        protected override void InsertItem(int index, Condition item)
        {
            CheckSealed();
            base.InsertItem(index, item);
        }

        /// <summary>
        ///     RemoveItem override
        /// </summary>
        protected override void RemoveItem(int index)
        {
            CheckSealed();
            base.RemoveItem(index);
        }

        /// <summary>
        ///     SetItem override
        /// </summary>
        protected override void SetItem(int index, Condition item)
        {
            CheckSealed();
            base.SetItem(index, item);
        }

        #endregion ProtectedMethods

        #region PublicMethods

        /// <summary>
        ///     Returns the sealed state of this object.  If true, any attempt
        ///     at modifying the state of this object will trigger an exception.
        /// </summary>
        public bool IsSealed => _isSealed;

        #endregion PublicMethods

        #region InternalMethods

        internal void Seal()
        {
            _isSealed = true;

            // Seal all the conditions
            for (int i = 0; i < Count; i++)
            {
                this[i].Seal();
            }
        }

        #endregion InternalMethods

        #region PrivateMethods

        private void CheckSealed()
        {
            if (_isSealed)
                throw new InvalidOperationException("Condition collection is sealed.");
        }

        #endregion PrivateMethods

        #region Data

        private bool _isSealed;

        #endregion Data
    }
}
