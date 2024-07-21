using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Data
{
    public class Binding : BindingBase
    {
        public Binding() { }

        public Binding(string path)
        {
            Path = new PropertyPath(path);
        }

        #region Properties

        private PropertyPath? _path;
        public PropertyPath? Path { get => _path; set { CheckSealed(); _path = value; } }

        private BindingMode _mode;
        public BindingMode Mode { get => _mode; set { CheckSealed(); _mode = value; } }

        private UpdateSourceTrigger _updateSourceTrigger;
        public UpdateSourceTrigger UpdateSourceTrigger { get => _updateSourceTrigger; set { CheckSealed(); _updateSourceTrigger = value; } }

        private bool _notifyOnSourceUpdated;
        public bool NotifyOnSourceUpdated { get => _notifyOnSourceUpdated; set { CheckSealed(); _notifyOnSourceUpdated = value; } }

        private bool _notifyOnTargetUpdated;
        public bool NotifyOnTargetUpdated { get => _notifyOnTargetUpdated; set { CheckSealed(); _notifyOnTargetUpdated = value; } }

        private bool _notifyOnValidationError;
        public bool NotifyOnValidationError { get => _notifyOnValidationError; set { CheckSealed(); _notifyOnValidationError = value; } }

        private IValueConverter? _converter;
        public IValueConverter? Converter { get => _converter; set { CheckSealed(); _converter = value; } }

        private object? _converterParameter;
        public object? ConverterParameter { get => _converterParameter; set { CheckSealed(); _converterParameter = value; } }

        private CultureInfo? _converterCulture;
        public CultureInfo? ConverterCulture { get => _converterCulture; set { CheckSealed(); _converterCulture = value; } }

        private object? _source;
        public object? Source { get => _source; set { CheckSealed(); _source = value; } }

        private RelativeSource? _relativeSource;
        public RelativeSource? RelativeSource { get => _relativeSource; set { CheckSealed(); _relativeSource = value; } }

        private string? _elementName;
        public string? ElementName { get => _elementName; set { CheckSealed(); _elementName = value; } }

        #endregion

        #region Methods

        protected override BindingExpressionBase CreateBindingExpressionCore(DependencyObject targetObject, DependencyProperty targetProperty)
        {
            return new BindingExpression(this, targetObject, targetProperty);
        }

        public override bool IsEqual(BindingBase bindingBase)
        {
            if (bindingBase is Binding binding)
            {
                return _path?.Path == binding.Path?.Path &&
                    _mode == binding._mode &&
                    _elementName == binding._elementName &&
                    _source == binding._source &&
                    _relativeSource == binding._relativeSource &&
                    _converter == binding._converter &&
                    _converterParameter == binding._converterParameter &&
                    _converterCulture == binding._converterCulture &&
                    FallbackValue == binding.FallbackValue &&
                    StringFormat == binding.StringFormat &&
                    TargetNullValue == binding.TargetNullValue;
            }
            return false;
        }

        #endregion
    }
}
