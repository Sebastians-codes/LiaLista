using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace LiaLista.SqlDatabases
{
    public class MsSql : ISqlDatabase
    {
        private readonly Dictionary<string, string> commands = new()
        {
            { "Get Company", "SELECT TOP 1 * FROM Company WHERE Name = 'companyName'" },
            {
                "Get Waiting", "SELECT * FROM Company" +
                               " WHERE Contacted = true AND Response IS NULL" +
                               " ORDER BY Intrest DESC"
            }
        };

        public DbConnection CreateConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        public DbCommand CreateCommand(string commandText, DbConnection connection)
        {
            return new SqlCommand(commandText, (SqlConnection)connection);
        }

        public string[] GetConnectionStringParts()
        {
            return new[] { "MsSql", ";Server", ";Database", ";User Id", ";Password" };
        }

        public Dictionary<string, string> GetCommands()
        {
            return commands;
        }
    }
}