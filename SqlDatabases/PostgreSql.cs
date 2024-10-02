using Npgsql;
using System.Data.Common;

namespace LiaLista.SqlDatabases;

public class PostgreSql : ISqlDatabase
{
    public DbConnection CreateConnection(string connectionString) =>
        new NpgsqlConnection(connectionString);

    public DbCommand CreateCommand(string commandText, DbConnection connection) =>
        new NpgsqlCommand(commandText, (NpgsqlConnection)connection);

    public string[] GetConnectionStringParts() =>
        new[] { "PostgreSql", "Host", ";Port", ";Database", ";Username", ";Password" };

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