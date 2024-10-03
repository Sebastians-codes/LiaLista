using Microsoft.Data.Sqlite;
using System.Data.Common;

namespace LiaLista.SqlDatabases;

public class SqLite : ISqlDatabase
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
        return new SqliteConnection(connectionString);
    }

    public DbCommand CreateCommand(string commandText, DbConnection connection)
    {
        return new SqliteCommand(commandText, (SqliteConnection)connection);
    }

    public string[] GetConnectionStringParts()
    {
        return new[] { "Data Source" };
    }

    public Dictionary<string, string> GetCommands()
    {
        return commands;
    }
}