using System.Data.Common;

namespace LiaLista.SqlDatabases
{
    public interface ISqlDatabase
    {
        DbConnection CreateConnection(string connectionString);
        DbCommand CreateCommand(string queryCommand, DbConnection connection);
        string[] GetConnectionStringParts();
        Dictionary<string, string> GetCommands();
    }
}