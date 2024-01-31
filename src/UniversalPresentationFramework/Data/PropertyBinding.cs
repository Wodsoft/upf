using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Data
{
    internal abstract class PropertyBinding : IDisposable
    {
        public abstract bool CanSet { get; }

        public abstract bool CanGet { get; }

        public abstract Type ValueType { get; }

        public event EventHandler? ValueChanged;

        private bool _disposed;
        public void Dispose()
        {
            if (_disposed) return;
            OnDispose();
            _disposed = true;
        }

        protected virtual void OnDispose()
        {

        }

        public abstract object? GetValue();

        public abstract void SetValue(object? value);

        protected void NotifyValueChange()
        {
            if (ValueChanged != null)
                ValueChanged(this, EventArgs.Empty);
        }

    }
}
