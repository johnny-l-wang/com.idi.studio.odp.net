using System.Collections.Generic;
using System.Reflection;
using IDI.Studio.ODP.FastReflection;
using Oracle.ManagedDataAccess.Client;

namespace IDI.Studio.ODP
{
    internal static class DatabaseExtention
    {
        public static List<T> ToList<T>(this OracleDataReader reader) where T : new()
        {
            var list = new List<T>();

            while (reader.Read())
            {
                list.Add(reader.ToObject<T>());
            }

            return list;
        }

        private static T ToObject<T>(this OracleDataReader reader) where T : new()
        {
            var instance = new T();

            var properties = instance.GetType().GetProperties(BindingFlags.Public);

            foreach (var property in properties)
            {
                var accessor = FastReflectionCaches.PropertyAccessorCache.Get(property);
                accessor.SetValue(instance, reader[property.Name]);
            }

            return instance;
        }
    }
}
