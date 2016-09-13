using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using IDI.Studio.ODP.FastReflection;
using Oracle.ManagedDataAccess.Client;

namespace IDI.Studio.ODP
{
    internal static class DatabaseExtention
    {
        private static Dictionary<string, string> commandCache = new Dictionary<string, string>();

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

        public static string GetCommandText(string code)
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentNullException("code");

            string directory = AppDomain.CurrentDomain.BaseDirectory;

            string file = string.Format(@"{0}\Files\query.{1}.xml", directory, code);

            if (!File.Exists(file))
                throw new FileNotFoundException(string.Format("Cannot found file '{0}'.", file));

            Console.WriteLine("File='{0}'", file);

            if (!commandCache.ContainsKey(code))
            {
                string commandText = GetQueryScript(file);
                commandCache.Add(code, commandText);
            }

            Console.WriteLine("Excute:'{0}'", commandCache[code]);

            return commandCache[code];
        }

        private static string GetQueryScript(string file)
        {
            var document = new XmlDocument();
            document.Load(file);

            string commandText = string.Empty;

            try
            {
                var root = document.SelectSingleNode("Query");
                var commandNode = root.SelectSingleNode("Command");
                commandText = commandNode.InnerText;
            }
            catch (XPathException)
            {
                throw new InvalidDataException(string.Format("Invalid query file '{0}'.", file));
            }

            if (string.IsNullOrEmpty(commandText))
                throw new InvalidDataException("Invalid query script.");

            return commandText;
        }

        public static void SetParameters(this OracleCommand command, Dictionary<string, object> parameters)
        {
            if (parameters == null)
                return;

            foreach (var kvp in parameters)
            {
                command.Parameters.Add(new OracleParameter(kvp.Key, kvp.Value));
            }
        }
    }
}
