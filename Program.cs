using LiaLista;

Repository repo = new("/home/zerq/Source/SUVNET24/LiaLista/companies.csv");
string companyName;

if (args.Length == 0)
{
    Console.WriteLine(repo.GetAll());
    return;
}

switch (args[0])
{
    case "add":
        repo.Add(CompanyFactory.Make());
        break;
    case "get":
        companyName = Utils.GetString("Företags namn: ->> ");
        Console.WriteLine(repo.GetCompany(companyName));
        break;
    case "all":
        Console.WriteLine(repo.GetAll());
        break;
    case "remove":
        companyName = Utils.GetString("Företags namn: ->> ");
        repo.Remove(companyName);
        break;
    default:
        Console.WriteLine("Felaktigt argument");
        break;
}