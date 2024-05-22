using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Wodsoft.UI.Controls;

namespace Wodsoft.UI.Data
{
    public class BindingExpression : BindingExpressionBase
    {
        private readonly Binding _binding;
        private bool _hasError, _isDataContext;
        private object? _source;
        private BindingContext? _propertyBinding;
        private BindingMode _mode;
        private System.Timers.Timer? _timer;
        private List<ValidationError>? _errors;
        private ReadOnlyCollection<ValidationError>? _readonlyErrors;

        internal BindingExpression(Binding binding, FrameworkElement targetObject, DependencyProperty targetProperty) : base(targetObject, targetProperty)
        {
            _binding = binding;
        }

        public override bool HasError => _hasError;

        public override bool HasValidationError => _errors != null;

        public override ValidationError? ValidationError => _errors?[0];

        public override ReadOnlyCollection<ValidationError>? ValidationErrors => _readonlyErrors;

        public object? ResolvedSource => _source;

        public string? ResolvedSourcePropertyName { get; }

        public override BindingBase ParentBindingBase => _binding;

        public override bool CanUpdateSource => _mode == BindingMode.OneWayToSource || _mode == BindingMode.TwoWay;

        public override bool CanUpdateTarget => _mode != BindingMode.OneWayToSource;

        #region Attaching

        protected override bool OnAttachCore()
        {
            _isDataContext = false;
            object? source;
            _hasError = true;
            if (_binding.RelativeSource != null)
            {
                source = _binding.RelativeSource.GetSource(Target!);
            }
            else
            {
                if (_binding.ElementName != null)
                {
                    source = Target!.FindName(_binding.ElementName);
                }
                else
                {
                    if (_binding.Source != null)
                    {
                        source = _binding.Source;
                    }
                    else
                    {
                        source = AttachedObject!.GetValue(FrameworkElement.DataContextProperty);
                        _isDataContext = true;
                    }
                }
            }
            if (source == null)
                return false;
            var mode = _binding.Mode;
            if (mode == BindingMode.Default)
            {
                if (TargetProperty.GetMetadata(Target!.GetType()) is FrameworkPropertyMetadata metadata && metadata.Flags.HasFlag(FrameworkPropertyMetadataOptions.BindsTwoWayByDefault))
                    mode = BindingMode.TwoWay;
                else
                    mode = BindingMode.OneWay;
            }
            if (_binding.Path != null)
            {
                var propertyBinding = _binding.Path.Bind(source);
                //switch (mode)
                //{
                //    case BindingMode.OneWay:
                //    case BindingMode.OneTime:
                //        if (!propertyBinding.CanGet)
                //        {
                //            propertyBinding.Dispose();
                //            return false;
                //        }
                //        break;
                //    case BindingMode.OneWayToSource:
                //        if (!propertyBinding.CanSet)
                //        {
                //            propertyBinding.Dispose();
                //            return false;
                //        }
                //        break;
                //    case BindingMode.TwoWay:
                //        if (!propertyBinding.CanGet || !propertyBinding.CanSet)
                //        {
                //            propertyBinding.Dispose();
                //            return false;
                //        }
                //        break;
                //}
                _propertyBinding = propertyBinding;
                if (propertyBinding.CanGet)
                    propertyBinding.ValueChanged += SourceChanged;
            }
            else
            {
                if (mode == BindingMode.TwoWay || mode == BindingMode.OneWayToSource)
                    return false;
            }
            if (_isDataContext)
            {
                AttachedObject!.DependencyPropertyChanged += PropertyChanged;
            }
            _hasError = false;
            _mode = mode;
            _source = source;
            if (_binding.Delay > 0)
            {
                _timer = new System.Timers.Timer();
                _timer.AutoReset = false;
                _timer.Interval = _binding.Delay;
                _timer.Elapsed += TimerElapsed;
            }
            return true;
        }

        private void PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == FrameworkElement.DataContextProperty)
            {
                _propertyBinding!.SetSource(e.NewValue);
            }
        }

        protected override void OnDetach()
        {
            if (_isDataContext)
            {
                AttachedObject!.DependencyPropertyChanged -= PropertyChanged;
            }
            if (_timer != null)
            {
                _timer.Elapsed -= TimerElapsed;
                _timer.Dispose();
                _timer = null;
            }
            if (_propertyBinding != null)
            {
                _propertyBinding.ValueChanged -= SourceChanged;
                _propertyBinding.Dispose();
                _propertyBinding = null;
            }
            _source = null;
        }

        #endregion

        private object? _lastTargetValue;
        protected override void SetSourceValue(object? value)
        {
            if (_hasError)
                return;
            if (_timer != null)
            {
                _timer.Stop();
                _lastTargetValue = value;
                _timer.Start();
            }
            else
            {
                SetSourceValueCore(value);
            }
        }
        private void TimerElapsed(object? sender, ElapsedEventArgs e)
        {
            object? value = null;
            Interlocked.Exchange(ref _lastTargetValue, value);
            SetSourceValueCore(value);
        }
        private void SetSourceValueCore(object? value)
        {
            _readonlyErrors = null;
            _errors = null;
            if (value == DependencyProperty.UnsetValue)
                value = Target!.GetMetadata(TargetProperty).DefaultValue;
            if (_binding.Converter != null)
            {
                try
                {
                    value = _binding.Converter.ConvertBack(value, TargetProperty.PropertyType, _binding.ConverterParameter, _binding.ConverterCulture);
                }
                catch (Exception ex)
                {
                    _errors = [new ValidationError(this, ex.Message, ex)];
                    _readonlyErrors = new ReadOnlyCollection<ValidationError>(_errors);
                    return;
                }
            }
            if (_propertyBinding!.CanSet)
                _propertyBinding!.SetValue(value);
        }

        protected internal override object? GetSourceValue()
        {
            if (_hasError)
                return Target!.GetMetadata(TargetProperty).DefaultValue;
            object? value;
            if (_propertyBinding == null)
                value = _source;
            else
            {
                if (_propertyBinding.CanGet)
                {
                    try
                    {
                        value = _propertyBinding.GetValue();
                    }
                    catch
                    {
                        return Target!.GetMetadata(TargetProperty).DefaultValue;
                    }
                }
                else
                {
                    value = Target!.GetMetadata(TargetProperty).DefaultValue;
                }
            }
            if (_binding.Converter != null)
            {
                try
                {
                    value = _binding.Converter.Convert(value, TargetProperty.PropertyType, _binding.ConverterParameter, _binding.ConverterCulture);
                }
                catch (Exception ex)
                {
                    _errors = [new ValidationError(this, ex.Message, ex)];
                    _readonlyErrors = new ReadOnlyCollection<ValidationError>(_errors);
                    return Target!.GetMetadata(TargetProperty).DefaultValue;
                }
            }
            return value;
        }

        private void SourceChanged(object? sender, EventArgs e)
        {
            UpdateTarget();
        }

        protected override bool TryUpdateExpressionValue(object? value)
        {
            if (!base.TryUpdateExpressionValue(value))
            {
                _errors = [new ValidationError(this)];
                _readonlyErrors = new ReadOnlyCollection<ValidationError>(_errors);
                return false;
            }
            return true;
        }
    }
}
