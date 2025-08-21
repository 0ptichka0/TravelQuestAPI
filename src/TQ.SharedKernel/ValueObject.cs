using System.Collections.Concurrent;
using System.Reflection;

namespace TQ.SharedKernel
{
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        // Кеш для рефлексии - критически важно для производительности
        private static readonly ConcurrentDictionary<Type, List<PropertyInfo>> _propertyCache = new();
        private static readonly ConcurrentDictionary<Type, List<FieldInfo>> _fieldCache = new();

        // Ленивая инициализация для экземпляра
        private List<PropertyInfo> _properties;
        private List<FieldInfo> _fields;

        public static bool operator ==(ValueObject obj1, ValueObject obj2)
        {
            if (ReferenceEquals(obj1, obj2))
                return true;
            if (obj1 is null || obj2 is null)
                return false;
            return obj1.Equals(obj2);
        }

        public static bool operator !=(ValueObject obj1, ValueObject obj2)
        {
            return !(obj1 == obj2);
        }

        public bool Equals(ValueObject obj)
        {
            return Equals(obj as object);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return GetProperties().All(p => PropertiesAreEqual(obj, p))
                && GetFields().All(f => FieldsAreEqual(obj, f));
        }

        private bool PropertiesAreEqual(object obj, PropertyInfo p)
        {
            var thisValue = p.GetValue(this, null);
            var otherValue = p.GetValue(obj, null);
            if (thisValue == null && otherValue == null)
                return true;
            if (thisValue == null || otherValue == null)
                return false;
            return thisValue.Equals(otherValue);
        }

        private bool FieldsAreEqual(object obj, FieldInfo f)
        {
            var thisValue = f.GetValue(this);
            var otherValue = f.GetValue(obj);
            if (thisValue == null && otherValue == null)
                return true;
            if (thisValue == null || otherValue == null)
                return false;
            return thisValue.Equals(otherValue);
        }

        private IEnumerable<PropertyInfo> GetProperties()
        {
            if (_properties == null)
            {
                var type = GetType();
                _properties = _propertyCache.GetOrAdd(type, t =>
                    t.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                     .Where(p => !Attribute.IsDefined(p, typeof(IgnoreMemberAttribute)))
                     .ToList());
            }
            return _properties;
        }

        private IEnumerable<FieldInfo> GetFields()
        {
            if (_fields == null)
            {
                var type = GetType();
                _fields = _fieldCache.GetOrAdd(type, t =>
                    t.GetFields(BindingFlags.Instance | BindingFlags.Public)
                     .Where(f => !Attribute.IsDefined(f, typeof(IgnoreMemberAttribute)))
                     .ToList());
            }
            return _fields;
        }

        public override int GetHashCode()
        {
            // Используем более эффективный алгоритм хеширования
            var hashCode = new HashCode();
            foreach (var prop in GetProperties())
            {
                var value = prop.GetValue(this, null);
                hashCode.Add(value);
            }
            foreach (var field in GetFields())
            {
                var value = field.GetValue(this);
                hashCode.Add(value);
            }
            return hashCode.ToHashCode();
        }

        // Виртуальный метод для оптимизации простых ValueObject
        public virtual object GetEqualityComponents()
        {
            return new
            {
                Properties = GetProperties().Select(p => p.GetValue(this, null)),
                Fields = GetFields().Select(f => f.GetValue(this))
            };
        }
    }

    //// source: https://github.com/jhewlett/ValueObject
    //public abstract class ValueObject : IEquatable<ValueObject>
    //{
    //    private List<PropertyInfo> properties;
    //    private List<FieldInfo> fields;

    //    public static bool operator ==(ValueObject obj1, ValueObject obj2)
    //    {
    //        if (object.Equals(obj1, null))
    //        {
    //            if (object.Equals(obj2, null))
    //            {
    //                return true;
    //            }
    //            return false;
    //        }
    //        return obj1.Equals(obj2);
    //    }

    //    public static bool operator !=(ValueObject obj1, ValueObject obj2)
    //    {
    //        return !(obj1 == obj2);
    //    }

    //    public bool Equals(ValueObject obj)
    //    {
    //        return Equals(obj as object);
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        if (obj == null || GetType() != obj.GetType()) return false;

    //        return GetProperties().All(p => PropertiesAreEqual(obj, p))
    //            && GetFields().All(f => FieldsAreEqual(obj, f));
    //    }

    //    private bool PropertiesAreEqual(object obj, PropertyInfo p)
    //    {
    //        return object.Equals(p.GetValue(this, null), p.GetValue(obj, null));
    //    }

    //    private bool FieldsAreEqual(object obj, FieldInfo f)
    //    {
    //        return object.Equals(f.GetValue(this), f.GetValue(obj));
    //    }

    //    private IEnumerable<PropertyInfo> GetProperties()
    //    {
    //        if (this.properties == null)
    //        {
    //            this.properties = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
    //                .Where(p => !Attribute.IsDefined(p, typeof(IgnoreMemberAttribute))).ToList();
    //        }

    //        return this.properties;
    //    }

    //    private IEnumerable<FieldInfo> GetFields()
    //    {
    //        if (this.fields == null)
    //        {
    //            this.fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public)
    //                .Where(f => !Attribute.IsDefined(f, typeof(IgnoreMemberAttribute))).ToList();
    //        }

    //        return this.fields;
    //    }

    //    public override int GetHashCode()
    //    {
    //        unchecked   //allow overflow
    //        {
    //            int hash = 17;
    //            foreach (var prop in GetProperties())
    //            {
    //                var value = prop.GetValue(this, null);
    //                hash = HashValue(hash, value);
    //            }

    //            foreach (var field in GetFields())
    //            {
    //                var value = field.GetValue(this);
    //                hash = HashValue(hash, value);
    //            }

    //            return hash;
    //        }
    //    }

    //    private int HashValue(int seed, object value)
    //    {
    //        var currentHash = value != null
    //            ? value.GetHashCode()
    //            : 0;

    //        return seed * 23 + currentHash;
    //    }
    //}
}
