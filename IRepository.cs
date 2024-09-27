namespace LiaLista;

public interface IRepository
{
    public string Add(Company company);
    public string Remove(string companyName);
    public string GetResponded();
    public string GetWaitingForResponse();
    public string SetContacted(string companyName);
    public string SetResponse(string companyName, string response);
    public string GetCompany(string companyName);
    public string GetAll();
}