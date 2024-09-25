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
        foreach (var savedCompany in _companies)
        {
            if (savedCompany.CompanyName == company.CompanyName)
            {
                return;
            }
        }

        _companies.Add(company);

        File.AppendAllLines(_path, [CsvSerialize(company)]);
    }

    public void Remove(string companyName)
    {
        foreach (var company in _companies)
        {
            if (company.CompanyName == companyName)
            {
                _companies.Remove(company);
                break;
            }
        }

        List<string> companies = _companies.Select(x => CsvSerialize(x)).ToList();

        File.WriteAllLines(_path, companies);
    }

    public void SetResponse(string companyName, Func<string> method)
    {
        foreach (var company in _companies)
        {
            if (company.CompanyName == companyName)
            {
                Company WithRespone = new(
                        company.CompanyName,
                        company.PhoneNumber,
                        company.Website,
                        company.Focus,
                        company.Location,
                        company.Intrest,
                        method());
                Remove(companyName);
                Add(WithRespone);
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
        if (company.Response == string.Empty)
        {
            return $"{company.CompanyName},{company.PhoneNumber}," +
            $"{company.Website},{company.Focus},{company.Location}," +
            $"{company.Intrest}";
        }

        return $"{company.CompanyName},{company.PhoneNumber}," +
            $"{company.Website},{company.Focus},{company.Location}," +
            $"{company.Intrest},{company.Response}";
    }

    private Company CsvDeserialize(string company)
    {
        string[] data = company.Split(",");
        if (data.Length == 6)
        {
            return new Company(data[0], data[1], data[2], data[3], data[4], int.Parse(data[5]));
        }
        else
        {
            return new Company(data[0], data[1], data[2], data[3], data[4], int.Parse(data[5]), data[6]);
        }
    }
}