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

        string[] companies = _companies.Select(x => CsvSerialize(x)).ToArray();

        File.WriteAllLines(_path, companies);
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
        StringBuilder sb = new();

        foreach (var company in _companies)
        {
            sb.Append(company.ToString());
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

    private string CsvSerialize(Company company) =>
        $"{company.CompanyName},{company.PhoneNumber}," +
        $"{company.Website},{company.Focus},{company.Location}," +
        $"{company.Intrest}";

    private Company CsvDeserialize(string company)
    {
        string[] data = company.Split(",");

        return new Company(data[0], data[1], data[2], data[3], data[4], int.Parse(data[5]));
    }
}