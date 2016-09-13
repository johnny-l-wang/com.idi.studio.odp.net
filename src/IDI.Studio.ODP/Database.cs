using System.Collections.Generic;
using System.Configuration;
using System.Data;
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

        public List<T> Query<T>(string code, Dictionary<string, object> parameters = null) where T : new()
        {
            string sql = DatabaseExtention.GetCommandText(code);

            var list = new List<T>();

            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    var command = new OracleCommand(sql, connection);
                    command.SetParameters(parameters);
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 90;

                    using (var reader = command.ExecuteReader(CommandBehavior.Default))
                    {
                        list = reader.ToList<T>();
                    }
                }
                finally
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return list;
        }
    }
}
