using Npgsql;
using System.Data.Common;

namespace LiaLista.SqlDatabases;

public class PostgreSql : ISqlDatabase
{
    private readonly Dictionary<string, string> commands = new()
    {
        { "Get Company", "SELECT * FROM Company WHERE Name = 'companyName' LIMIT 1" },
        {
            "Get Waiting", "SELECT * FROM Company" +
                           " WHERE Contacted = true AND Response = ''" +
                           " ORDER BY Intrest DESC"
        }
    };

    public DbConnection CreateConnection(string connectionString)
    {
        return new NpgsqlConnection(connectionString);
    }

    public DbCommand CreateCommand(string commandText, DbConnection connection)
    {
        return new NpgsqlCommand(commandText, (NpgsqlConnection)connection);
    }

    public string[] GetConnectionStringParts()
    {
        return new[] { "PostgreSql", "Host", ";Port", ";Database", ";Username", ";Password" };
    }

    public Dictionary<string, string> GetCommands()
    {
        return commands;
    }
}