using System.Text;

namespace LiaLista;

public class Repository
{
    private readonly string _path;
    private List<Company> _companies = [];

    public Repository(string path)
    {
        _path = path;
        InitializeDatabase();
    }

    public void Add(Company company)
    {
        if (_companies.Where(x => x.CompanyName == company.CompanyName).Count() != 0)
        {
            return;
        }

        File.AppendAllLines(_path, [CsvSerialize(company)]);
    }

    public void Remove(string companyName)
    {
        _companies = _companies.Where(x => x.CompanyName != companyName).ToList();

        List<string> companies = _companies.Select(x => CsvSerialize(x)).ToList();
        File.WriteAllLines(_path, companies);
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

    public void SetContacted(string companyName)
    {
        if (_companies.Count == 0)
        {
            return;
        }

        foreach (var company in _companies)
        {
            if (company.CompanyName == companyName && company.Contacted == false)
            {
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
            }
        }
    }

    public void SetResponse(string companyName, Func<string> method)
    {
        foreach (var company in _companies)
        {
            if (company.CompanyName == companyName)
            {
                if (company.Contacted == false)
                {
                    return;
                }

                Company withRespone = new(
                        company.CompanyName,
                        company.PhoneNumber,
                        company.Website,
                        company.Focus,
                        company.Location,
                        company.Intrest,
                        company.Contacted,
                        method());
                Remove(companyName);
                Add(withRespone);
                break;
            }
        }
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

        return "";
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
}