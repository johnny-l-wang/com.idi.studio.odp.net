using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace IDI.Studio.ODP
{
    public sealed class Database
    {
        public static readonly Database Instance = new Database();

        private string connectionString = string.Empty;

        private Database()
        {
            connectionString = ConfigurationManager.ConnectionStrings["Oracle"].ToString();
        }

        public List<T> Query<T>(string sql) where T : new()
        {
            var list = new List<T>();

            using (var connection = new OracleConnection(connectionString))
            {
                var command = new OracleCommand(sql, connection);
                command.CommandType = CommandType.Text;
                command.CommandTimeout = 90;

                using (var reader = command.ExecuteReader(CommandBehavior.Default))
                {
                    while (reader.Read())
                    {

                    }
                }
            }

            return list;
        }
    }
}
