using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Exp = System.Linq.Expressions.Expression;

namespace Wodsoft.UI.Data
{
    internal class ClrPropertyBinding : PropertyBinding
    {
        private readonly object _source;
        private readonly bool _canSet, _canGet;
        private readonly MethodCache _cache;
        private readonly string _name;

        public ClrPropertyBinding(object source, PropertyInfo propertyInfo)
        {
            _source = source;
            _canGet = propertyInfo.CanRead;
            _canSet = propertyInfo.CanWrite;
            _cache = GetCache(propertyInfo);
            _name = propertyInfo.Name;
            if (_canGet && source is INotifyPropertyChanged notify)
                notify.PropertyChanged += PropertyChanged;
        }

        private void PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == _name)
                NotifyValueChange();
        }

        public override bool CanSet => _canSet;

        public override bool CanGet => _canGet;

        public override object? GetValue()
        {
            if (!_canGet)
                throw new NotSupportedException();
            return _cache.Get!(_source);
        }

        public override void SetValue(object? value)
        {
            if (!_canSet)
                throw new NotSupportedException();
            _cache.Set!(_source, value);
        }

        protected override void OnDispose()
        {
            if (_canGet && _source is INotifyPropertyChanged notify)
                notify.PropertyChanged -= PropertyChanged;
        }

        private static Dictionary<PropertyInfo, MethodCache> _Caches = new Dictionary<PropertyInfo, MethodCache>();
        internal static MethodCache GetCache(PropertyInfo propertyInfo)
        {
            if (_Caches.TryGetValue(propertyInfo, out MethodCache cache)) { return cache; }
            cache = new MethodCache();
            if (propertyInfo.CanRead)
            {
                var sourceParameter = Exp.Parameter(typeof(object));
                Exp exp = Exp.Call(Exp.Convert(sourceParameter, propertyInfo.DeclaringType!), propertyInfo.GetGetMethod()!);
                if (propertyInfo.PropertyType.IsValueType)
                    exp = Exp.Convert(exp, typeof(object));
                cache.Get = Exp.Lambda<Func<object, object?>>(exp, sourceParameter).Compile();
            }
            if (propertyInfo.CanWrite)
            {
                var sourceParameter = Exp.Parameter(typeof(object));
                var valueParameter = Exp.Parameter(typeof(object));
                Exp exp = Exp.Call(Exp.Convert(sourceParameter, propertyInfo.DeclaringType!), propertyInfo.GetSetMethod()!, Exp.Convert(valueParameter, propertyInfo.PropertyType));
                cache.Set = Exp.Lambda<Action<object, object?>>(exp, sourceParameter, valueParameter).Compile();
            }
            _Caches.Add(propertyInfo, cache);
            return cache;
        }

        internal struct MethodCache
        {
            public Func<object, object?>? Get;
            public Action<object, object?>? Set;
        }
    }
}
