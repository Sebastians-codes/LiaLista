using System.Text;

namespace LiaLista;

public class CsvRepo
{
    private readonly string _path;
    private List<Company> _companies = [];

    public CsvRepo(string path)
    {
        _path = path;
        CreateDataFolder();
        InitializeDatabase();
    }

    public string Add(Company company)
    {
        if (_companies.Where(x => x.CompanyName == company.CompanyName).Count() != 0)
        {
            return "There is already a company with that name registered.";
        }

        File.AppendAllLines(_path, [CsvSerialize(company)]);

        return "Company successfully added!";
    }

    public string Remove(string companyName)
    {
        string returnString = "No company with that name in the Database.";

        if (_companies.Where(x => x.CompanyName == companyName).Count() > 0)
        {
            returnString = $"{companyName} Has been removed.";
        }
        else
        {
            return returnString;
        }

        _companies = _companies.Where(x => x.CompanyName != companyName).ToList();

        List<string> companies = _companies.Select(x => CsvSerialize(x)).ToList();
        File.WriteAllLines(_path, companies);

        return returnString;
    }

    public string GetResponded()
    {
        Console.Clear();
        StringBuilder sb = new();

        var companies = _companies.OrderByDescending(x => x.Intrest)
                .Where(x => x.Response != string.Empty);

        foreach (var company in companies)
        {
            sb.Append(company.ToString());
        }

        if (sb.Length == 0)
        {
            return "There is no Companies that has given a response";
        }

        return sb.ToString();
    }

    public string GetWaitingForResponse()
    {
        Console.Clear();
        StringBuilder sb = new();

        var companies = _companies.OrderByDescending(x => x.Intrest)
                .Where(x => x.Contacted == true && x.Response == string.Empty);

        foreach (var company in companies)
        {
            sb.Append(company.ToString());
        }

        if (sb.Length == 0)
        {
            return "Not waiting for a response from any company";
        }

        return sb.ToString();
    }

    public string SetContacted(string companyName)
    {
        if (_companies.Count == 0)
        {
            return "There is no companies in the Database.";
        }

        foreach (var company in _companies)
        {
            if (company.CompanyName == companyName)
            {
                if (company.Contacted != false)
                {
                    return "This company has already been set to contacted.";
                }

                Company contacted = new(
                        company.CompanyName,
                        company.PhoneNumber,
                        company.Website,
                        company.Focus,
                        company.Location,
                        company.Intrest,
                        true);
                Remove(companyName);
                Add(contacted);

                return $"{companyName} has been marked as contacted";
            }
        }

        return $"There was no company with the name {companyName}";
    }

    public string SetResponse(string companyName, string response)
    {
        foreach (var company in _companies)
        {
            if (company.CompanyName == companyName)
            {
                if (company.Contacted == false)
                {
                    return "You have not set this company as contacted yet. please mark it as contacted before" +
                        " setting a response.";
                }

                Company withRespone = new(
                        company.CompanyName,
                        company.PhoneNumber,
                        company.Website,
                        company.Focus,
                        company.Location,
                        company.Intrest,
                        company.Contacted,
                        response);
                Remove(companyName);
                Add(withRespone);

                return "Company response added";
            }
        }

        return $"There was no company with the name {companyName}";
    }

    public string GetCompany(string companyName)
    {
        foreach (var company in _companies)
        {
            if (company.CompanyName == companyName)
            {
                return company.ToString();
            }
        }

        return $"There was no company with the name {companyName} in the database.";
    }

    public string GetAll()
    {
        Console.Clear();
        StringBuilder sb = new();

        foreach (var company in _companies.OrderByDescending(x => x.Intrest))
        {
            sb.Append(company.ToString());
        }

        if (sb.Length == 0)
        {
            return "There is no Companies Registered";
        }

        return sb.ToString();
    }

    private void InitializeDatabase()
    {
        if (!File.Exists(_path))
        {
            return;
        }

        string[] companies = File.ReadAllLines(_path);

        if (companies.Length == 0)
        {
            return;
        }

        _companies = companies.Select(x => CsvDeserialize(x)).ToList();
    }

    private string CsvSerialize(Company company)
    {
        if (company.Contacted == false)
        {
            return $"{company.CompanyName},{company.PhoneNumber}," +
            $"{company.Website},{company.Focus},{company.Location}," +
            $"{company.Intrest}";
        }
        else if (company.Response == string.Empty)
        {
            return $"{company.CompanyName},{company.PhoneNumber}," +
                $"{company.Website},{company.Focus},{company.Location}," +
                $"{company.Intrest},{company.Contacted}";
        }
        else
        {
            return $"{company.CompanyName},{company.PhoneNumber}," +
                $"{company.Website},{company.Focus},{company.Location}," +
                $"{company.Intrest},{company.Contacted},{company.Response}";
        }
    }

    private Company CsvDeserialize(string company)
    {
        string[] data = company.Split(",");
        if (data.Length == 6)
        {
            return new Company(data[0], data[1], data[2], data[3], data[4], int.Parse(data[5]));
        }
        else if (data.Length == 7)
        {
            return new Company(data[0], data[1], data[2], data[3], data[4], int.Parse(data[5]), true);
        }
        else
        {
            return new Company(data[0], data[1], data[2], data[3], data[4], int.Parse(data[5]), true, data[7]);
        }
    }

    private void CreateDataFolder()
    {
        var path = _path.Split("/");
        string homeDir = string.Join("/", path[..2]);

        if (Directory.Exists(homeDir))
        {
            return;
        }

        Directory.CreateDirectory(homeDir);
    }
}