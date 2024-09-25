using LiaLista;

Repository repo = new("/home/zerq/Source/SUVNET24/LiaLista/companies.csv");
string companyName;

if (args.Length == 0)
{
    Console.WriteLine(repo.GetAll());
    return;
}

if (args.Length == 1)
{
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
        case "response":
            companyName = Utils.GetString("Företags namn: ->> ");
            repo.SetResponse(companyName, Utils.GetResponse);
            break;
        default:
            Console.WriteLine("Felaktigt argument");
            break;
    }
}
else if (args.Length == 2)
{
    switch (args[0])
    {
        case "get":
            Console.WriteLine(repo.GetCompany(args[1]));
            break;
        case "remove":
            repo.Remove(args[1]);
            break;
        case "response":
            repo.SetResponse(args[1], Utils.GetResponse);
            break;
        default:
            Console.WriteLine("Felaktigt argument");
            break;
    }
}