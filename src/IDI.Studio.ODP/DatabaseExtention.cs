using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            return new T();
        }
    }
}
