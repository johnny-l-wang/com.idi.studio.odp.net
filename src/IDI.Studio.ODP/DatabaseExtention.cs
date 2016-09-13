using System;
using System.Collections.Generic;
using System.Xml;
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

            var properties = instance.GetType().GetProperties();

            foreach (var property in properties)
            {
                var accessor = FastReflectionCaches.PropertyAccessorCache.Get(property);
                var value = reader[property.Name];

                try
                {
                    if (value != DBNull.Value)
                    {
                        accessor.SetValue(instance, value);
                    }
                }
                catch (InvalidCastException)
                {
                    throw new InvalidCastException(string.Format("Specified cast is not valid. {0} set value {1}", property.Name, value));
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return instance;
        }

        public static string GetScripts(string code)
        {

            //var document = new XmlDocument();
            //document.Load(@"..\..\Book.xml");

            //string directory = AppDomain.CurrentDomain.BaseDirectory;
            string directory = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            Console.WriteLine("directory='{0}'", directory);

            return string.Empty;
        }
    }
}
