using Npgsql;
using System.Text;

namespace LiaLista;

public class PGSqlRepo : IRepository
{
    private string _connectionString;

    public PGSqlRepo(string connectionString)
    {
        _connectionString = connectionString;
    }

    public string GetAll()
    {
        StringBuilder sb = new();

        using var connection = new NpgsqlConnection(_connectionString);
        using var command = new NpgsqlCommand(
                "SELECT Name, Number, Website, Focus, Location, Intrest, Contacted, Response" +
                " FROM Company ORDER BY Intrest DESC", connection);
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
        catch (PostgresException e)
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
        using var connection = new NpgsqlConnection(_connectionString);
        using var command = new NpgsqlCommand($"SELECT * FROM Company WHERE Name = '{companyName}' LIMIT 1", connection);

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
        catch (PostgresException e)
        {
            Console.WriteLine(e.Message);
        }

        return "There where no company with that name";
    }

    public string Add(Company company)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        using var command = new NpgsqlCommand(
                "INSERT INTO Company (Name, Number, Website, Focus, Location, Intrest)" +
                "VALUES (@Name, @Number, @Website, @Focus, @Location, @Intrest)", connection);

        command.Parameters.AddWithValue("@Name", company.CompanyName);
        command.Parameters.AddWithValue("@Number", company.PhoneNumber);
        command.Parameters.AddWithValue("@Website", company.Website);
        command.Parameters.AddWithValue("@Focus", company.Focus);
        command.Parameters.AddWithValue("@Location", company.Location);
        command.Parameters.AddWithValue("@Intrest", company.Intrest);

        try
        {
            connection.Open();
            command.ExecuteNonQuery();
            return @$"Company Successfully added.

{company.ToString()}";
        }
        catch (PostgresException e)
        {
            Console.WriteLine(e.Message);
        }
        return "There was already a Company with that name";
    }

    public string SetResponse(string companyName, string response)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        using var command = new NpgsqlCommand(
                "UPDATE Company SET Response = @Value WHERE Name = @CompanyName", connection);

        command.Parameters.AddWithValue("@Value", response);
        command.Parameters.AddWithValue("@CompanyName", companyName);

        try
        {
            connection.Open();
            command.ExecuteNonQuery();
            return "Company has been set to Responded";
        }
        catch (PostgresException e)
        {
            Console.WriteLine(e.Message);
        }
        return "----";
    }

    public string SetContacted(string companyName)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        using var command = new NpgsqlCommand(
                "UPDATE Company SET Contacted = @Value WHERE Name = @CompanyName", connection);

        command.Parameters.AddWithValue("@Value", true);
        command.Parameters.AddWithValue("@CompanyName", companyName);

        try
        {
            connection.Open();
            command.ExecuteNonQuery();
            return "Company has been set to Contacted";
        }
        catch (PostgresException e)
        {
            Console.WriteLine(e.Message);
        }
        return "----";
    }

    public string GetWaitingForResponse()
    {
        StringBuilder sb = new();

        using var connection = new NpgsqlConnection(_connectionString);
        using var command = new NpgsqlCommand(
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
        catch (PostgresException e)
        {
            Console.WriteLine(e.Message);
        }

        return sb.ToString();

    }

    public string GetResponded()
    {
        StringBuilder sb = new();

        using var connection = new NpgsqlConnection(_connectionString);
        using var command = new NpgsqlCommand(
                "SELECT * FROM Company" +
                " WHERE Response IS NOT NULL" +
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
        catch (PostgresException e)
        {
            Console.WriteLine(e.Message);
        }

        return sb.ToString();
    }

    public string Remove(string companyName)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        using var command = new NpgsqlCommand(
                "DELETE FROM Company WHERE Name = @CompanyName", connection);

        command.Parameters.AddWithValue("@CompanyName", companyName);

        try
        {
            connection.Open();
            command.ExecuteNonQuery();
            return "Company has been removed";
        }
        catch (PostgresException e)
        {
            Console.WriteLine(e.Message);
        }
        return "There was already a Company with that name";
    }
}