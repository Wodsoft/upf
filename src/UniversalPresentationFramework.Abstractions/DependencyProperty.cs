using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Wodsoft.UI
{
    public class DependencyProperty
    {
        private DependencyPropertyKey? _key;
        private bool _isNullable;
        private static int _Count;
        private static object _GlobalLocker = new object();
        private object _metadataLocker = new object();
        private static Hashtable _NameTables = new Hashtable();
        private TreeMap<TypeMetadata> _metadatas = new TreeMap<TypeMetadata>();
        private int _index;

        private DependencyProperty(string name, Type propertyType, Type ownerType, PropertyMetadata metadata, ValidateValueCallback? validateValueCallback, bool isReadOnly, bool isNullable)
        {
            Name = name;
            PropertyType = propertyType;
            OwnerType = ownerType;
            DefaultMetadata = metadata;
            ReadOnly = isReadOnly;
            ValidateValueCallback = validateValueCallback;
            if (isReadOnly)
                _key = new DependencyPropertyKey(this);
            _isNullable = isNullable;
        }

        public static DependencyProperty Register(string name, Type propertyType, Type ownerType)
        {
            return RegisterCommon(name, propertyType, ownerType, null, null, false);
        }
        public static DependencyProperty Register(string name, Type propertyType, Type ownerType, PropertyMetadata metadata)
        {
            return RegisterCommon(name, propertyType, ownerType, metadata, null, false);
        }
        public static DependencyProperty Register(string name, Type propertyType, Type ownerType, PropertyMetadata? metadata, ValidateValueCallback validateValueCallback)
        {
            return RegisterCommon(name, propertyType, ownerType, metadata, validateValueCallback, false);
        }

        public static DependencyProperty RegisterAttached(string name, Type propertyType, Type ownerType)
        {
            return RegisterCommon(name, propertyType, ownerType, null, null, false);
        }
        public static DependencyProperty RegisterAttached(string name, Type propertyType, Type ownerType, PropertyMetadata metadata)
        {
            return RegisterCommon(name, propertyType, ownerType, metadata, null, false);
        }
        public static DependencyProperty RegisterAttached(string name, Type propertyType, Type ownerType, PropertyMetadata? metadata, ValidateValueCallback validateValueCallback)
        {
            return RegisterCommon(name, propertyType, ownerType, metadata, validateValueCallback, false);
        }
        public static DependencyPropertyKey RegisterAttachedReadOnly(string name, Type propertyType, Type ownerType, PropertyMetadata metadata)
        {
            var property = RegisterCommon(name, propertyType, ownerType, metadata, null, true);
            return property._key!;
        }
        public static DependencyPropertyKey RegisterAttachedReadOnly(string name, Type propertyType, Type ownerType, PropertyMetadata? metadata, ValidateValueCallback validateValueCallback)
        {
            var property = RegisterCommon(name, propertyType, ownerType, metadata, validateValueCallback, true);
            return property._key!;
        }
        public static DependencyPropertyKey RegisterReadOnly(string name, Type propertyType, Type ownerType, PropertyMetadata metadata)
        {
            var property = RegisterCommon(name, propertyType, ownerType, metadata, null, true);
            return property._key!;
        }
        public static DependencyPropertyKey RegisterReadOnly(string name, Type propertyType, Type ownerType, PropertyMetadata? metadata, ValidateValueCallback validateValueCallback)
        {
            var property = RegisterCommon(name, propertyType, ownerType, metadata, validateValueCallback, true);
            return property._key!;
        }

        private static DependencyProperty RegisterCommon(string name, Type propertyType, Type ownerType, PropertyMetadata? metadata, ValidateValueCallback? validateValueCallback, bool isReadOnly)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (propertyType == null)
                throw new ArgumentNullException(nameof(propertyType));
            if (ownerType == null)
                throw new ArgumentNullException(nameof(ownerType));
            var key = new FromNameKey(name, ownerType);
            lock (_GlobalLocker)
            {
                if (_NameTables.ContainsKey(key))
                    throw new ArgumentException("Property already registered.");
                if (metadata == null)
                    metadata = new PropertyMetadata();
                if (metadata.IsSealed)
                    throw new ArgumentException("Metadata is sealed.", nameof(metadata));
                var isNullable = !propertyType.IsValueType || Nullable.GetUnderlyingType(propertyType) != null;
                if (metadata.DefaultValue == null)
                {
                    if (!isNullable)
                        metadata.DefaultValue = Activator.CreateInstance(propertyType);
                    if (validateValueCallback != null && !validateValueCallback(metadata.DefaultValue))
                        throw new ArgumentException($"Default value of type \"{propertyType.FullName}\" validate failed.");
                }
                else
                {
                    if (validateValueCallback != null && !validateValueCallback(metadata.DefaultValue))
                        throw new ArgumentException("Default value of metadata validate failed.");
                }
                var dp = new DependencyProperty(name, propertyType, ownerType, metadata, validateValueCallback, isReadOnly, isNullable);
                dp._index = _Count++;
                _NameTables[key] = dp;
                return dp;
            }
        }

        public DependencyProperty AddOwner(Type ownerType, PropertyMetadata? typeMetadata)
        {
            var key = new FromNameKey(Name, ownerType);
            lock (_GlobalLocker)
            {
                if (_NameTables.ContainsKey(key))
                    throw new ArgumentException("Property already registered.");
                if (typeMetadata != null)
                    OverrideMetadata(ownerType, typeMetadata);
                _NameTables.Add(key, this);
            }
            return this;
        }
        public DependencyProperty AddOwner(Type ownerType)
        {
            return AddOwner(ownerType, null);
        }

        public bool IsValidType(object value)
        {
            if (value == null)
                return _isNullable;
            return value.GetType().IsAssignableFrom(PropertyType);
        }
        public bool IsValidValue(object value)
        {
            if (ValidateValueCallback == null)
                return true;
            return ValidateValueCallback(value);
        }

        public void OverrideMetadata(Type type, PropertyMetadata metadata, DependencyPropertyKey key)
        {
            if (!ReadOnly)
                throw new InvalidOperationException("Dependency property is not readonly.");
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (metadata == null)
                throw new ArgumentNullException(nameof(metadata));
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (key.DependencyProperty != this || _key != key)
                throw new ArgumentException("The key is not belong to this dependency property.", nameof(key));
            OverrideMetadataCore(type, metadata);
        }
        public void OverrideMetadata(Type type, PropertyMetadata metadata)
        {
            if (ReadOnly)
                throw new InvalidOperationException("Not allowed to override a readonly dependency property.");
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (metadata == null)
                throw new ArgumentNullException(nameof(metadata));
            OverrideMetadataCore(type, metadata);
        }
        private void OverrideMetadataCore(Type type, PropertyMetadata metadata)
        {
            if (metadata.IsSealed)
                throw new ArgumentException("Metadata is sealed.", nameof(metadata));
            if (metadata.DefaultValue == null)
            {
                if (!_isNullable)
                    metadata.DefaultValue = DefaultMetadata.DefaultValue;
                if (ValidateValueCallback != null && !ValidateValueCallback(metadata.DefaultValue))
                    throw new ArgumentException($"Default value of type \"{PropertyType.FullName}\" validate failed.");
            }
            else
            {
                if (ValidateValueCallback != null && !ValidateValueCallback(metadata.DefaultValue))
                    throw new ArgumentException("Default value of metadata validate failed.");
            }
            lock (_metadataLocker)
            {
                if (!SetMetadata(_metadatas, type, metadata))
                    throw new InvalidOperationException($"Type \"{type.FullName}\" has override metadata already.");
            }
        }

        public PropertyMetadata GetMetadata(Type type)
        {
            if (_metadatas.Children.Count != 0)
            {
                lock (_metadataLocker)
                {
                    return GetMetadata(_metadatas, type);
                }
            }
            return DefaultMetadata;
        }
        private PropertyMetadata GetMetadata(TreeMap<TypeMetadata> node, Type type)
        {
            for (int i = 0; i < node.Children.Count; i++)
            {
                var childNode = node.Children.List[i];
                var childValue = node.Children.List[i].Value;
                if (childValue.Type == type)
                    return childValue.PropertyMetadata;
                if (type.IsSubclassOf(childValue.Type))
                {
                    if (childNode.Children.Count == 0)
                        return childValue.PropertyMetadata;
                    return GetMetadata(childNode, type);
                }
            }
            return DefaultMetadata;
        }
        private bool SetMetadata(TreeMap<TypeMetadata> node, Type type, PropertyMetadata metadata)
        {
            if (node.Children.Count != 0)
            {
                for (int i = 0; i < node.Children.Count; i++)
                {
                    var childNode = node.Children.List[i];
                    var childValue = node.Children.List[i].Value;
                    if (childValue.Type == type)
                        return false;
                    if (type.IsSubclassOf(childValue.Type))
                        return SetMetadata(childNode, type, metadata);
                }
            }
            metadata.Seal();
            node.Children.Add(new TreeMapNode<TypeMetadata>(new TypeMetadata(type, metadata)));
            return true;
        }

        public static DependencyProperty? FromName(string name, Type ownerType, bool inherited = true)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (ownerType == null)
                throw new ArgumentNullException(nameof(ownerType));
            Type? type = ownerType;
            object? dp;
            while (type != null)
            {
                var key = new FromNameKey(name, type);
                lock (_GlobalLocker)
                    dp = _NameTables[key];
                if (dp != null)
                    return (DependencyProperty)dp;
                if (!inherited)
                    return null;
                type = type.BaseType;
            }
            return null;
        }

        public Type PropertyType { get; }
        public Type OwnerType { get; }
        public string Name { get; }
        public int GlobalIndex => _index;
        public PropertyMetadata DefaultMetadata { get; }
        public bool ReadOnly { get; }
        public ValidateValueCallback? ValidateValueCallback { get; }

        public static object UnsetValue { get; } = new object();

        private record struct FromNameKey
        {
            public FromNameKey(string name, Type key)
            {
                Name = name;
                Key = key;
            }

            public string Name;

            public Type Key;
        }

        private record struct TypeMetadata
        {
            public TypeMetadata(Type type, PropertyMetadata propertyMetadata)
            {
                Type = type;
                PropertyMetadata = propertyMetadata;
            }

            public Type Type;

            public PropertyMetadata PropertyMetadata;
        }
    }
}
