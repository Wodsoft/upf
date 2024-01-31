using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Data
{
    internal class BindingContext : PropertyBinding
    {
        private readonly SourceValueInfo[] _sourceValueInfos;
        private readonly PropertyBinding?[] _bindings;
        private object? _source;
        private int _bindLevel;
        private PropertyBinding? _finalBinding;

        public BindingContext(object source, SourceValueInfo[] sourceValueInfos)
        {
            _source = source;
            _sourceValueInfos = sourceValueInfos;
            _bindings = new PropertyBinding?[sourceValueInfos.Length];
            _bindLevel = -1;
            Bind(in _source);
        }

        private void Bind(in object? current)
        {
            if (current == null)
            {
                _finalBinding = null;
                return;
            }
            PropertyBinding? binding = null;
            var svi = _sourceValueInfos[_bindLevel + 1];
            switch (svi.Type)
            {
                case SourceValueType.Property:
                    {
                        var type = _bindLevel == -1 ? _source!.GetType() : _bindings[_bindLevel]!.ValueType;
                        if (current is DependencyObject d)
                        {
                            var dp = DependencyProperty.FromName(svi.Name!, type);
                            if (dp != null)
                            {
                                binding = new DependencyPropertyBinding(d, dp);
                                break;
                            }
                        }
                        var propertyInfo = type.GetProperty(svi.Name!);
                        if (propertyInfo == null)
                        {
                            break;
                        }
                        binding = new ClrPropertyBinding(current, propertyInfo);
                        break;
                    }
                case SourceValueType.Indexer:
                    {
                        var type = current.GetType();
                        var properties = type.GetProperties(BindingFlags.Default | BindingFlags.Instance | BindingFlags.Public);
                        foreach (var propertyInfo in properties.Where(t => t.GetIndexParameters().Length == svi.ParamList!.Count))
                        {
                            var converters = propertyInfo.GetIndexParameters().Select(t => TypeDescriptor.GetConverter(t.ParameterType)).ToArray();
                            if (converters.Any(t => !t.CanConvertFrom(typeof(string))))
                                continue;
                            object?[] parameters = new object?[converters.Length];
                            for (int ii = 0; ii < svi.ParamList!.Count; ii++)
                            {
                                try
                                {
                                    parameters[ii] = converters[ii].ConvertFromString(svi.ParamList[ii].valueString);
                                }
                                catch
                                {
                                    continue;
                                }
                            }
                            binding = new IndexPropertyBinding(current, parameters, propertyInfo);
                            break;
                        }
                        break;
                    }
            }
            if (binding != null)
            {
                _bindLevel++;
                _bindings[_bindLevel] = binding;
                binding.ValueChanged += Binding_ValueChanged;
                if (_bindLevel < _bindings.Length - 1)
                {
                    Bind(binding.GetValue());
                }
                else
                {
                    _finalBinding = binding;
                }
            }
        }

        private void Binding_ValueChanged(object? sender, EventArgs e)
        {
            var level = Array.IndexOf(_bindings, sender);
            if (level < _bindings.Length - 1)
            {
                for (int i = level + 1; i <= _bindLevel; i++)
                {
                    _bindings[i]!.Dispose();
                    _bindings[i] = null;
                }
                _bindLevel = level;
                Bind(_bindings[level]!.GetValue());
            }
            NotifyValueChange();
        }

        public void SetSource(object? source)
        {
            for (int i = 0; i <= _bindLevel; i++)
            {
                _bindings[i]!.Dispose();
                _bindings[i] = null;
            }
            _bindLevel = -1;
            _source = source;
            Bind(in source);
            NotifyValueChange();
        }

        public override bool CanSet => _finalBinding?.CanSet ?? false;

        public override bool CanGet => _finalBinding?.CanGet ?? false;

        public override Type ValueType => _finalBinding?.ValueType ?? typeof(object);

        public override object? GetValue()
        {
            return _finalBinding?.GetValue();
        }

        public override void SetValue(object? value)
        {
            _finalBinding?.SetValue(value);
        }
    }
}
