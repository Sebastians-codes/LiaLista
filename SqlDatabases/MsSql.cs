using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace LiaLista.SqlDatabases;

public class MsSql : ISqlDatabase
{
    public DbConnection CreateConnection(string connectionString) =>
        new SqlConnection(connectionString);

    public DbCommand CreateCommand(string commandText, DbConnection connection) =>
        new SqlCommand(commandText, (SqlConnection)connection);

    public string[] GetConnectionStringParts() =>
        new[] { "MsSql", ";Server", ";Database", ";User Id", ";Password" };

    public Dictionary<string, string> GetCommands() =>
        commands;

    private Dictionary<string, string> commands = new Dictionary<string, string>()
    {
        {"Get Company",$"SELECT TOP 1 * FROM Company WHERE Name = 'companyName'"},
        {"Get Waiting","SELECT * FROM Company" +
                " WHERE Contacted = true AND Response IS NULL" +
                " ORDER BY Intrest DESC"},

    };
}