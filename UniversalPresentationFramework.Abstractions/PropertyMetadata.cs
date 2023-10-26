using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public class PropertyMetadata
    {
        public static readonly PropertyMetadata DefaultMetadata;
        private CoerceValueCallback? _coerceValueCallback;
        private object? _defaultValue;
        private PropertyChangedCallback? _propertyChangedCallback;
        private bool _isSealed;

        static PropertyMetadata()
        {
            DefaultMetadata = new PropertyMetadata();
            DefaultMetadata.Seal();
        }

        public PropertyMetadata() { }

        public PropertyMetadata(object? defaultValue)
        {
            _defaultValue = defaultValue;
        }

        public PropertyMetadata(PropertyChangedCallback propertyChangedCallback)
        {
            _propertyChangedCallback = propertyChangedCallback;
        }

        public PropertyMetadata(object? defaultValue, PropertyChangedCallback? propertyChangedCallback)
        {
            _defaultValue = defaultValue;
            _propertyChangedCallback = propertyChangedCallback;
        }

        public PropertyMetadata(object? defaultValue, PropertyChangedCallback? propertyChangedCallback, CoerceValueCallback? coerceValueCallback)
        {
            _defaultValue = defaultValue;
            _propertyChangedCallback = propertyChangedCallback;
            _coerceValueCallback = coerceValueCallback;
        }

        public CoerceValueCallback? CoerceValueCallback { get => _coerceValueCallback; set { SealCheck(); _coerceValueCallback = value; } }

        public object? DefaultValue { get => _defaultValue; set { SealCheck(); _defaultValue = value; } }

        public PropertyChangedCallback? PropertyChangedCallback { get => _propertyChangedCallback; set { SealCheck(); _propertyChangedCallback = value; } }

        public bool IsSealed { get => _isSealed; }

        private void SealCheck()
        {
            if (_isSealed)
                throw new InvalidOperationException("Can not change a sealed property metadata.");
        }

        public void Seal()
        {
            _isSealed = true;
        }

        public virtual bool IsInherited => false;
    }
}
