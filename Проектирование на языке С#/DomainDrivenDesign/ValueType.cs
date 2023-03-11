using Ddd.Taxi.Domain;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ddd.Infrastructure
{
    public class ValueType<T>
	{
        public static PropertyInfo[] properties { get; private set; }
        
        static ValueType()
        {
            properties = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .OrderBy(s => s.Name)
                .ToArray();
        }
        
        public bool Equals(PersonName name)
        {
            if (ReferenceEquals(null, name)) return false;
            if (ReferenceEquals(this, name)) return true;
            if (name.GetType() != typeof(T)) return false;

            return properties
                .Select(s => s.GetValue(this))
                .SequenceEqual(properties.Select(s => s.GetValue(name)));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(T)) return false;

            return properties
                .Select(s => s.GetValue(this))
                .SequenceEqual(properties.Select(s => s.GetValue(obj)));
        }

        public override int GetHashCode()
        {
            int hash = 0;
            unchecked 
            {
                foreach (var property in properties)
                    hash = (hash ^ property.GetValue(this).GetHashCode()) * 16777619;
            }
            return hash;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"{typeof(T).Name}(");
            var isFirst = true;

            foreach (var property in properties)
            {
                if(isFirst) isFirst= false;
                else stringBuilder.Append("; ");
                stringBuilder.Append($"{property.Name}: {property.GetValue(this)}");
            }

            stringBuilder.Append(")");
            return stringBuilder.ToString();
        }
    }
}