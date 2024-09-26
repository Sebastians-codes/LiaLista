using LiaLista;

Repository repo = new("/home/zerq/Source/SUVNET24/LiaLista/companies.csv");
string companyName;

if (args.Length == 0)
{
    Console.WriteLine(repo.GetAll());
    Console.WriteLine("flist help     to get se all methods available");
    return;
}

if (args.Length == 1)
{
    switch (args[0])
    {
        case "add":
            Console.WriteLine(repo.Add(CompanyFactory.Make()));
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
            Console.WriteLine(repo.Remove(companyName));
            break;
        case "response":
            companyName = Utils.GetString("Företags namn: ->> ");
            Console.WriteLine(repo.SetResponse(companyName, Utils.GetResponse));
            break;
        case "contacted":
            companyName = Utils.GetString("Företags namn: ->> ");
            Console.WriteLine(repo.SetContacted(companyName));
            break;
        case "waiting":
            Console.WriteLine(repo.GetWaitingForResponse());
            break;
        case "responded":
            Console.WriteLine(repo.GetResponded());
            break;
        case "help":
            Console.Clear();
            Console.WriteLine(@"Methods to use - flist 'method'
add - adds a new company to the list.
get - gets a company stored in the database.
all or flist only - get all companies and info stored in the database.
remove - remove a company in the database.
contacted - Mark company as contacted and waiting for response.
response - Sets contacted to recieved response and lets you add a response field.
waiting - gets all companies that hasnt given a response.
responded - gets all companies that has responded.
help - shows the help menu.

Extra Functionality Case sensitive.
get 'Company name' - gets you the company.
remove 'Company name' - removes the company from the database.
contacted 'Company name' - Marks the company waiting for response.
response 'Company name' - Marks the company recieved response and lets you add a response.");
            break;
        default:
            Console.WriteLine("Felaktigt argument");
            break;
    }
}
else if (args.Length == 2 && args[1].Length > 1)
{
    string input = $"{char.ToUpper(args[1][0])}{args[1][1..].ToLower()}";
    switch (args[0])
    {
        case "get":
            Console.WriteLine(repo.GetCompany(input));
            break;
        case "remove":
            Console.WriteLine(repo.Remove(input));
            break;
        case "response":
            Console.WriteLine(repo.SetResponse(input, Utils.GetResponse));
            break;
        case "contacted":
            Console.WriteLine(repo.SetContacted(input));
            break;
        default:
            Console.WriteLine("Felaktigt argument");
            break;
    }
}