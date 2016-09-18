using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
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

            return Execute<T>(sql, parameters);
        }

        public List<T> Query<T>(string code, List<string> conditions, Dictionary<string, object> parameters = null) where T : new()
        {
            string sql = DatabaseExtention.GetCommandText(code);

            sql = BuildScript(sql, conditions);

            return Execute<T>(sql, parameters);
        }

        public string BuildScript(string sql, List<string> conditions)
        {
            var builder = new StringBuilder();
            builder.AppendFormat(sql);

            int line = 0;

            foreach (var condition in conditions)
            {
                line += 1;

                if (line == 1 && !sql.HasKeyword("where"))
                {
                    builder.AppendFormat(" where {0}", condition);
                }
                else
                {
                    builder.AppendFormat(" and {0}", condition);
                }
            }

            return builder.ToString();
        }

        private List<T> Execute<T>(string sql, Dictionary<string, object> parameters = null) where T : new()
        {
            if (string.IsNullOrEmpty(sql))
                throw new ArgumentNullException("sql");

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
