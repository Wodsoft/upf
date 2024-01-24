using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Exp = System.Linq.Expressions.Expression;

namespace Wodsoft.UI.Data
{
    internal class IndexPropertyBinding : PropertyBinding
    {
        private readonly object _source;
        private readonly object?[] _parameters;
        private readonly bool _canSet, _canGet;
        private readonly MethodCache _cache;

        public IndexPropertyBinding(object source, object?[] parameters, PropertyInfo propertyInfo)
        {
            _source = source;
            _parameters = parameters;
            _canGet = propertyInfo.CanRead;
            _canSet = propertyInfo.CanWrite;
            _cache = GetCache(propertyInfo);
            if (parameters.Length == 1 && parameters[0] is int && source is IList && source is INotifyCollectionChanged notify)
                notify.CollectionChanged += CollectionChanged;
        }

        private void CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            var index = (int)_parameters[0]!;
            if ((e.OldStartingIndex == index || (e.OldStartingIndex <= index && (e.OldStartingIndex + e.OldItems?.Count ?? 0) > index)) ||
                (e.NewStartingIndex == index || (e.NewStartingIndex <= index && (e.NewStartingIndex + e.NewItems?.Count ?? 0) > index)))
                NotifyValueChange();
        }

        public override bool CanSet => _canSet;

        public override bool CanGet => _canGet;

        public override object? GetValue()
        {
            if (!_canGet)
                throw new NotSupportedException();
            return _cache.Get!(_source, _parameters);
        }

        public override void SetValue(object? value)
        {
            if (!_canSet)
                throw new NotSupportedException();
            _cache.Set!(_source, _parameters, value);
        }

        protected override void OnDispose()
        {
            if (_parameters.Length == 1 && _parameters[0] is int && _source is IList && _source is INotifyCollectionChanged notify)
                notify.CollectionChanged -= CollectionChanged;
        }

        private static Dictionary<PropertyInfo, MethodCache> _Caches = new Dictionary<PropertyInfo, MethodCache>();
        internal static MethodCache GetCache(PropertyInfo propertyInfo)
        {
            if (_Caches.TryGetValue(propertyInfo, out MethodCache cache)) { return cache; }
            cache = new MethodCache();
            if (propertyInfo.CanRead)
            {
                var indexParameters = propertyInfo.GetIndexParameters();
                var sourceParameter = Exp.Parameter(typeof(object));
                var parametersParameter = Exp.Parameter(typeof(object[]));
                var parametersExp = new Exp[indexParameters.Length];
                for (int i = 0; i < indexParameters.Length; i++)
                {
                    parametersExp[i] = Exp.Convert(Exp.ArrayIndex(parametersParameter, Exp.Constant(i)), indexParameters[i].ParameterType);
                }
                Exp exp = Exp.Call(Exp.Convert(sourceParameter, propertyInfo.DeclaringType!), propertyInfo.GetGetMethod()!, parametersExp);
                if (propertyInfo.PropertyType.IsValueType)
                    exp = Exp.Convert(exp, typeof(object));
                cache.Get = Exp.Lambda<Func<object, object?[], object?>>(exp, sourceParameter, parametersParameter).Compile();
            }
            if (propertyInfo.CanWrite)
            {
                var indexParameters = propertyInfo.GetIndexParameters();
                var sourceParameter = Exp.Parameter(typeof(object));
                var parametersParameter = Exp.Parameter(typeof(object[]));
                var valueParameter = Exp.Parameter(typeof(object));
                var parametersExp = new Exp[indexParameters.Length + 1];
                for (int i = 0; i < indexParameters.Length; i++)
                {
                    parametersExp[i] = Exp.Convert(Exp.ArrayIndex(parametersParameter, Exp.Constant(i)), indexParameters[i].ParameterType);
                }
                parametersExp[indexParameters.Length] = Exp.Convert(valueParameter, propertyInfo.PropertyType);
                Exp exp = Exp.Call(Exp.Convert(sourceParameter, propertyInfo.DeclaringType!), propertyInfo.GetSetMethod()!, parametersExp);
                cache.Set = Exp.Lambda<Action<object, object?[], object?>>(exp, sourceParameter, parametersParameter, valueParameter).Compile();
            }
            _Caches.Add(propertyInfo, cache);
            return cache;
        }

        internal struct MethodCache
        {
            public Func<object, object?[], object?>? Get;
            public Action<object, object?[], object?>? Set;
        }
    }
}
