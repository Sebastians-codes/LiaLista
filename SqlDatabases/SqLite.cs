using Microsoft.Data.Sqlite;
using System.Data.Common;

namespace LiaLista.SqlDatabases;

public class SqLite : ISqlDatabase
{
    public DbConnection CreateConnection(string connectionString) =>
        new SqliteConnection(connectionString);

    public DbCommand CreateCommand(string commandText, DbConnection connection) =>
        new SqliteCommand(commandText, (SqliteConnection)connection);

    public string[] GetConnectionStringParts() =>
        new[] { "Data Source" };

    public Dictionary<string, string> GetCommands() =>
            commands;

    private Dictionary<string, string> commands = new Dictionary<string, string>()
    {
        {"Get Company",$"SELECT * FROM Company WHERE Name = 'companyName' LIMIT 1"},
        {"Get Waiting","SELECT * FROM Company" +
                " WHERE Contacted = true AND Response = ''" +
                " ORDER BY Intrest DESC"},

    };
}