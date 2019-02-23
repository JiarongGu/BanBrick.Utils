using BanBrick.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BanBrick.Utils
{
    public abstract class Immutable<T> : ICloneable, IComparable, IComparable<T>, IEquatable<T>
    {
        private static readonly BindingFlags innerBindings =
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.GetProperty;
        private static Lazy<IEnumerable<FieldInfo>> _fields = new Lazy<IEnumerable<FieldInfo>>(() =>
            typeof(T).GetFields(innerBindings));

        private static readonly BindingFlags staticBindings =
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField | BindingFlags.GetProperty;
        private static Lazy<IEnumerable<FieldInfo>> _staticFields = new Lazy<IEnumerable<FieldInfo>>(() =>
            typeof(T).GetFields(staticBindings));

        private static Lazy<IDictionary<string, PropertyInfo>> _staticInstanceDictionary = new Lazy<IDictionary<string, PropertyInfo>>(() =>
            typeof(T).GetProperties(staticBindings).Where(x => x.PropertyType == typeof(T)).ToDictionary(x => ((T)x.GetValue(null)).ToString(), x => x));

        public override abstract string ToString();

        public virtual bool Equals(T other)
        {
            if (ReferenceEquals(other, null)) return false;

            return ToString() == other.ToString();
        }

        public virtual object Clone()
        {
            return MemberwiseClone();
        }

        public virtual int CompareTo(T other)
        {
            var compareString = string.Empty;

            if (!ReferenceEquals(other, null))
                compareString = other.ToString();

            return ToString().CompareTo(compareString);
        }

        public int CompareTo(object obj)
        {
            return CompareTo((T)obj);
        }

        public override bool Equals(object obj)
        {
            return Equals((T)obj);
        }

        public override int GetHashCode()
        {
            var hash = 13 * 7;

            foreach (var field in _fields.Value)
            {
                hash = hash + field.GetValue(this).ToString().GetHashCode();
            }

            return hash;
        }

        public static T TryParse(string value)
        {
            if (value.IsNullOrEmpty())
                return default(T);

            if (_staticInstanceDictionary.Value.ContainsKey(value))
                return (T)_staticInstanceDictionary.Value[value].GetValue(null);

            return default(T);
        }

        public static T FindOrDefault(T other)
        {
            return _staticFields.Value
                .Where(x => x.FieldType == typeof(T))
                .Select(x => (T)x.GetValue(null))
                .FirstOrDefault(x => x.Equals(other));
        }

        public static bool operator ==(Immutable<T> a, Immutable<T> b)
        {
            if (ReferenceEquals(a, null))
            {
                return ReferenceEquals(b, null);
            }

            return a.Equals(b);
        }

        public static bool operator !=(Immutable<T> a, Immutable<T> b)
        {
            return !(a == b);
        }
    }
}
