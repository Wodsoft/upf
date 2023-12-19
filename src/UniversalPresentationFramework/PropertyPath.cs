using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Data;

namespace Wodsoft.UI
{
    public class PropertyPath
    {
        private string _path;
        private readonly List<object> _parameters;

        public PropertyPath(string path, params object[] parameters)
        {
            _path = path;
            _parameters = new List<object>(parameters);
        }

        public PropertyPath(object parameter) : this("(0)", parameter)
        {

        }

        public string Path { get => _path; set => _path = value; }

        public IList<object> Parameters => _parameters;

        internal bool TryBind(object source, [NotNullWhen(returnValue: true)] out PropertyBinding? binding)
        {
            if (_path == null)
            {
                binding = null;
                return false;
            }
            PathParser parser = new PathParser();
            var svis = parser.Parse(Path);
            if (svis.Length == 0)
            {
                binding = null;
                return false;
            }
            if (svis[0].Type == SourceValueType.Direct)
            {
                binding = new ObjectPropertyBinding(source);
                return true;
            }
            object? parent = null;
            object current = source;
            for (int i = 0; i < svis.Length; i++)
            {
                var svi = svis[i];
                switch (svi.Type)
                {
                    case SourceValueType.Property:
                        {
                            var type = current.GetType();
                            if (current is DependencyObject d)
                            {
                                var dp = DependencyProperty.FromName(svi.Name!, type);
                                if (dp != null)
                                {
                                    if (i == svis.Length - 1 || svis[i + 1].Type == SourceValueType.Direct)
                                    {
                                        binding = new DependencyPropertyBinding(d, dp);
                                        return true;
                                    }
                                    else
                                    {
                                        var value = d.GetValue(dp);
                                        //Have next path but value is null, bind failed
                                        if (value == null)
                                        {
                                            binding = null;
                                            return false;
                                        }
                                        parent = current;
                                        current = value;
                                        continue;
                                    }
                                }
                            }
                            var propertyInfo = type.GetProperty(svi.Name!);
                            if (propertyInfo == null)
                            {
                                binding = null;
                                return false;
                            }
                            binding = new ClrPropertyBinding(current, propertyInfo);
                            if (i == svis.Length - 1 || svis[i + 1].Type == SourceValueType.Direct)
                                return true;
                            var value2 = binding.GetValue();
                            //Have next path but value is null, bind failed
                            if (value2 == null)
                            {
                                binding = null;
                                return false;
                            }
                            parent = current;
                            current = value2;
                            continue;
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
                                if (i == svis.Length - 1 || svis[i + 1].Type == SourceValueType.Direct)
                                    return true;
                                var value = binding.GetValue();
                                //Have next path but value is null, bind failed
                                if (value == null)
                                {
                                    binding = null;
                                    return false;
                                }
                                parent = current;
                                current = value;
                                continue;
                            }
                            binding = null;
                            return false;
                        }
                }
            }
            throw new NotImplementedException();
        }
    }
}
