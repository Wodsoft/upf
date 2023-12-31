using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public class Sealable : ISealable
    {
        #region Seal

        private bool _isSealed;
        public bool IsSealed => _isSealed;

        protected void CheckSealed()
        {
            if (_isSealed)
                throw new InvalidOperationException("Can not change values after sealed.");
        }

        /// <summary>
        /// This Style and all factories/triggers are now immutable
        /// </summary>
        public void Seal()
        {
            if (_isSealed)
                if (ThrowIfSealTwice)
                    throw new InvalidOperationException("Object is sealed.");
                else
                    return;

            OnSeal();

            _isSealed = true;
        }

        protected virtual void OnSeal()
        {

        }

        protected virtual bool ThrowIfSealTwice => false;

        protected virtual bool CanSeal => !_isSealed;

        #endregion
    }
}
