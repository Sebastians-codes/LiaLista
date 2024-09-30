using System.Text;

namespace LiaLista;

public class SqlRepo
{
    private string _connectionString;
    private SqlType _sqlType;

    public SqlRepo(SqlType sqlType)
    {
        _sqlType = sqlType;
        _connectionString = CreateConnectionString();
    }

    public IRepository Repository()
    {
        switch (_sqlType)
        {
            case SqlType.MsSql:
                return new MsSqlRepo(_connectionString);
            case SqlType.PostgreSql:
                return new PGSqlRepo(_connectionString);
        }
        return new CsvRepo("lia_lista.csv");
    }

    private string CreateConnectionString()
    {
        if (!File.Exists(".env"))
        {
            throw new FileNotFoundException();
        }

        StringBuilder sb = new();
        var parts = File.ReadAllLines(".env");
        Dictionary<SqlType, string[]> sqls = [];
        sqls[SqlType.MsSql] = ["Server", "Database", "User Id", "Password"];
        sqls[SqlType.PostgreSql] = ["Host", ";Port", ";Database", ";Username", ";Password"];
        sqls[SqlType.Sqlite] = ["Data Source"];

        var type = sqls[_sqlType];

        for (int i = 0; i < type.Length; i++)
        {
            sb.Append($"{type[i]} = {parts[i]}");
        }

        return sb.ToString();
    }
}