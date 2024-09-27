using Microsoft.Data.SqlClient;
using System.Text;

namespace LiaLista;

public class MsSqlRepo : IRepository
{
    private string _connectionString;

    public MsSqlRepo()
    {
        _connectionString = GetConnectionString();
    }

    public string GetAll()
    {
        StringBuilder sb = new();

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
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
                string? response = reader.IsDBNull(reader.GetOrdinal("Response")) ?
                    string.Empty : reader.GetString(reader.GetOrdinal("Response"));
                Company company = new(name, number, website, focus, location, intrest, contacted, response);
                sb.Append(company.ToString());
            }
        }
        catch (SqlException e)
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
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand($"SELECT TOP 1 * FROM Company WHERE Name = '{companyName}'", connection);
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
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
        }

        return "There where no company with that name";
    }

    public string SetResponse(string companyName, string response)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
                "UPDATE Company SET Response = @Value WHERE Name = @CompanyName", connection);

        command.Parameters.AddWithValue("@Value", response);
        command.Parameters.AddWithValue("@CompanyName", companyName);

        try
        {
            connection.Open();
            command.ExecuteNonQuery();
            return "Company has been set to Responded";
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
        }
        return "----";
    }

    public string SetContacted(string companyName)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
                "UPDATE Company SET Contacted = @Value WHERE Name = @CompanyName", connection);

        command.Parameters.AddWithValue("@Value", 1);
        command.Parameters.AddWithValue("@CompanyName", companyName);

        try
        {
            connection.Open();
            command.ExecuteNonQuery();
            return "Company has been set to Contacted";
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
        }
        return "----";
    }

    public string GetWaitingForResponse()
    {
        StringBuilder sb = new();

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
                "SELECT * FROM Company" +
                " WHERE Contacted = 1 AND Response IS NULL" +
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
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
        }

        return sb.ToString();

    }

    public string GetResponded()
    {
        StringBuilder sb = new();

        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
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
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
        }

        return sb.ToString();
    }

    public string Remove(string companyName)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
                "DELETE FROM Company WHERE Name = @CompanyName", connection);

        command.Parameters.AddWithValue("@CompanyName", companyName);

        try
        {
            connection.Open();
            command.ExecuteNonQuery();
            return "Company has been removed";
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
        }
        return "There was already a Company with that name";
    }

    public string Add(Company company)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(
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
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
        }
        return "There was already a Company with that name";
    }

    private Dictionary<string, string> LoadEnviromentVariabels()
    {
        Dictionary<string, string> envVars = [];

        foreach (var field in File.ReadAllLines(".env"))
        {
            var part = field.Split('=', StringSplitOptions.RemoveEmptyEntries);
            if (part.Length == 2)
            {
                envVars[part[0]] = part[1];
            }
        }

        return envVars;
    }

    private string GetConnectionString()
    {
        var env = LoadEnviromentVariabels();

        return $"Server={env["Server"]};" +
               $"Database={env["DatabaseName"]};" +
               $"User Id={env["Username"]};" +
               $"Password={env["Password"]};" +
               "TrustServerCertificate=True;";
    }
}