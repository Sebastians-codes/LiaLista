using System.Data.Common;
using System.Text;

namespace LiaLista.SqlDatabases;

public class SqlRepo
{
    private string _connectionString;
    private ISqlDatabase _database;

    public SqlRepo(SqlType sqlType)
    {
        _database = GetDatabase(sqlType);

        if (sqlType != SqlType.Sqlite)
        {
            _connectionString = CreateConnectionString(sqlType);
        }
        else
        {
            _connectionString = $"Data Source={Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "lialista.db")}";

            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "lialista.db")))
            {
                return;
            }

            using var connection = _database.CreateConnection(_connectionString);
            connection.Open();

            string queryString = @"
                CREATE TABLE IF NOT EXISTS Company (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Number TEXT,
                    Website TEXT,
                    Focus TEXT,
                    Location TEXT,
                    Intrest INTEGER,
                    Contacted INTEGER DEFAULT 0,
                    Response TEXT DEFAULT ''
                );";

            using var command = _database.CreateCommand(queryString, connection);

            command.ExecuteNonQuery();
        }

        if (sqlType == SqlType.MsSql)
        {
            _connectionString += ";TrustServerCertificate=true";
        }
    }

    private ISqlDatabase GetDatabase(SqlType sqlType)
    {
        return sqlType switch
        {
            SqlType.PostgreSql => new PostgreSql(),
            SqlType.MsSql => new MsSql(),
            SqlType.Sqlite => new SqLite(),
            _ => throw new NotImplementedException()
        };
    }

    private string CreateConnectionString(SqlType sqlType)
    {
        if (!File.Exists(".env"))
        {
            throw new FileNotFoundException();
        }

        StringBuilder sb = new();
        var parts = File.ReadAllLines(".env");
        var type = _database.GetConnectionStringParts();

        for (int i = 0; i < parts.Length; i++)
        {
            if (parts[i] == sqlType.ToString())
            {
                for (int j = 1; j < type.Length; j++)
                {
                    sb.Append($"{type[j]} = {parts[++i]}");
                }
                break;
            }
        }

        return sb.ToString();
    }

    public string GetAll()
    {
        StringBuilder sb = new();

        string queryString = "SELECT Name, Number, Website, Focus, Location, Intrest, Contacted, Response" +
                                " FROM Company ORDER BY Intrest DESC";

        using DbConnection connection = _database.CreateConnection(_connectionString);
        using DbCommand command = _database.CreateCommand(queryString, connection);

        try
        {
            connection.Open();
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                string name = reader.GetString(reader.GetOrdinal("Name"));
                string number = reader.GetString(reader.GetOrdinal("Number"));
                string website = reader.GetString(reader.GetOrdinal("Website"));
                string focus = reader.GetString(reader.GetOrdinal("Focus"));
                string location = reader.GetString(reader.GetOrdinal("Location"));
                int intrest = reader.GetInt32(reader.GetOrdinal("Intrest"));
                bool contacted = reader.GetBoolean(reader.GetOrdinal("Contacted"));
                string response = reader.GetString(reader.GetOrdinal("Response"));

                Company company = new(name, number, website, focus, location, intrest, contacted, response);
                sb.Append(company.ToString());
            }
        }
        catch (DbException e)
        {
            Console.WriteLine(e.Message);
        }

        if (sb.Length == 0)
        {
            return "There is no companies in the database";
        }

        return sb.ToString();
    }

    public string GetCompany(string companyName)
    {
        string queryString = _database.GetCommands()["Get Company"];
        queryString = queryString.Replace("companyName", $"{companyName}");
        using var connection = _database.CreateConnection(_connectionString);
        using var command = _database.CreateCommand(queryString, connection);

        try
        {
            connection.Open();
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                string name = reader.GetString(reader.GetOrdinal("Name"));
                string number = reader.GetString(reader.GetOrdinal("Number"));
                string website = reader.GetString(reader.GetOrdinal("Website"));
                string focus = reader.GetString(reader.GetOrdinal("Focus"));
                string location = reader.GetString(reader.GetOrdinal("Location"));
                int intrest = reader.GetInt32(reader.GetOrdinal("Intrest"));
                bool contacted = reader.GetBoolean(reader.GetOrdinal("Contacted"));
                string? response = reader.IsDBNull(reader.GetOrdinal("Response")) ?
                    string.Empty : reader.GetString(reader.GetOrdinal("Response"));
                Company company = new(name, number, website, focus, location, intrest, contacted, response);

                return company.ToString();
            }
        }
        catch (DbException e)
        {
            Console.WriteLine(e.Message);
        }

        return "There where no company with that name";
    }

    public string Add(Company company)
    {
        using var connection = _database.CreateConnection(_connectionString);
        using var command = _database.CreateCommand(
                "INSERT INTO Company (Name, Number, Website, Focus, Location, Intrest)" +
                "VALUES (@Name, @Number, @Website, @Focus, @Location, @Intrest)", connection);

        command.Parameters.Add(CreateParameter(command, "@Name", company.CompanyName));
        command.Parameters.Add(CreateParameter(command, "@Number", company.PhoneNumber));
        command.Parameters.Add(CreateParameter(command, "@Website", company.Website));
        command.Parameters.Add(CreateParameter(command, "@Focus", company.Focus));
        command.Parameters.Add(CreateParameter(command, "@Location", company.Location));
        command.Parameters.Add(CreateParameter(command, "@Intrest", company.Intrest));

        try
        {
            connection.Open();
            command.ExecuteNonQuery();
            return @$"Company Successfully added.

{company.ToString()}";
        }
        catch (DbException e)
        {
            Console.WriteLine(e.Message);
        }

        return "There was already a Company with that name";
    }

    public string SetResponse(string companyName, string response)
    {
        using var connection = _database.CreateConnection(_connectionString);
        using var command = _database.CreateCommand(
                "UPDATE Company SET Response = @Value WHERE Name = @CompanyName", connection);

        command.Parameters.Add(CreateParameter(command, "@Value", response));
        command.Parameters.Add(CreateParameter(command, "@CompanyName", companyName));

        try
        {
            connection.Open();
            command.ExecuteNonQuery();
            return "Company has been set to Responded";
        }
        catch (DbException e)
        {
            Console.WriteLine(e.Message);
        }

        return "----";
    }

    public string SetContacted(string companyName)
    {
        using var connection = _database.CreateConnection(_connectionString);
        using var command = _database.CreateCommand(
                "UPDATE Company SET Contacted = @Value WHERE Name = @CompanyName", connection);

        command.Parameters.Add(CreateParameter(command, "@Value", true));
        command.Parameters.Add(CreateParameter(command, "@CompanyName", companyName));

        try
        {
            connection.Open();
            command.ExecuteNonQuery();
            return "Company has been set to Contacted";
        }
        catch (DbException e)
        {
            Console.WriteLine(e.Message);
        }

        return "----";
    }

    public string GetWaitingForResponse()
    {
        StringBuilder sb = new();

        using var connection = _database.CreateConnection(_connectionString);
        using var command = _database.CreateCommand(
                "SELECT * FROM Company" +
                " WHERE Contacted = true AND Response = ''" +
                " ORDER BY Intrest DESC", connection);
        try
        {
            connection.Open();
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                string name = reader.GetString(reader.GetOrdinal("Name"));
                string number = reader.GetString(reader.GetOrdinal("Number"));
                string website = reader.GetString(reader.GetOrdinal("Website"));
                string focus = reader.GetString(reader.GetOrdinal("Focus"));
                string location = reader.GetString(reader.GetOrdinal("Location"));
                int intrest = reader.GetInt32(reader.GetOrdinal("Intrest"));
                bool contacted = reader.GetBoolean(reader.GetOrdinal("Contacted"));
                string? response = reader.IsDBNull(reader.GetOrdinal("Response")) ?
                    string.Empty : reader.GetString(reader.GetOrdinal("Response"));
                Company company = new(name, number, website, focus, location, intrest, contacted, response);
                sb.Append(company.ToString());
            }
        }
        catch (DbException e)
        {
            Console.WriteLine(e.Message);
        }

        return sb.ToString();

    }

    public string GetResponded()
    {
        StringBuilder sb = new();

        using var connection = _database.CreateConnection(_connectionString);
        using var command = _database.CreateCommand(
                "SELECT * FROM Company" +
                " WHERE Response != ''" +
                " ORDER BY Intrest", connection);

        try
        {
            connection.Open();
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                string name = reader.GetString(reader.GetOrdinal("Name"));
                string number = reader.GetString(reader.GetOrdinal("Number"));
                string website = reader.GetString(reader.GetOrdinal("Website"));
                string focus = reader.GetString(reader.GetOrdinal("Focus"));
                string location = reader.GetString(reader.GetOrdinal("Location"));
                int intrest = reader.GetInt32(reader.GetOrdinal("Intrest"));
                bool contacted = reader.GetBoolean(reader.GetOrdinal("Contacted"));
                string? response = reader.IsDBNull(reader.GetOrdinal("Response")) ?
                    string.Empty : reader.GetString(reader.GetOrdinal("Response"));
                Company company = new(name, number, website, focus, location, intrest, contacted, response);
                sb.Append(company.ToString());
            }
        }
        catch (DbException e)
        {
            Console.WriteLine(e.Message);
        }

        return sb.ToString();
    }

    public string Remove(string companyName)
    {
        using var connection = _database.CreateConnection(_connectionString);
        using var command = _database.CreateCommand(
                "DELETE FROM Company WHERE Name = @CompanyName", connection);

        command.Parameters.Add(CreateParameter(command, "@CompanyName", companyName));

        try
        {
            connection.Open();
            command.ExecuteNonQuery();
            return "Company has been removed";
        }
        catch (DbException e)
        {
            Console.WriteLine(e.Message);
        }

        return "There was already a Company with that name";
    }

    private DbParameter CreateParameter(DbCommand command, string name, object value)
    {
        DbParameter parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value;
        return parameter;
    }
}