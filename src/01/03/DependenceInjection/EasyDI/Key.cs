using System;
using System.Linq;

namespace EasyDI
{
    public class Key : IEquatable<Key>
    {
        public ServiceRegistry Registry { get; }
        public Type[] GenericArguments { get; }

        public Key(ServiceRegistry registry, Type[] genericArguments)
        {
            Registry = registry;
            GenericArguments = genericArguments;
        }

        public bool Equals(Key other)
        {
            if (other == null)
            {
                return false;
            }
            if (Registry != other.Registry)
            {
                return false;
            }

            if (GenericArguments.Length != other.GenericArguments.Length)
            {
                return false;
            }

            return !GenericArguments.Where((t, index) => t != other.GenericArguments[index]).Any();
        }

        public override int GetHashCode()
        {
            var hashCode = Registry.GetHashCode();

            return GenericArguments.Aggregate(hashCode, (current, t) => current ^ t.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            return obj is Key key && Equals(key);
        }
    }
}