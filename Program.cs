using LiaLista;

Repository repo = new(Path.Join(Environment.SpecialFolder.Desktop.ToString(), "LiaLista", "companies.csv"));

string Help()
{
    Console.Clear();
    return @"Methods to use - flist 'method'
add - adds a new company to the list.
get - gets a company stored in the database.
all or flist only - get all companies and info stored in the database.
remove - remove a company in the database.
contacted - Mark company as contacted and waiting for response.
response - Sets contacted to recieved response and lets you add a response field.
waiting - gets all companies that hasnt given a response.
responded - gets all companies that has responded.
help - shows the help menu.

Extra Functionality.
get 'Company name' - gets you the company.
remove 'Company name' - removes the company from the database.
contacted 'Company name' - Marks the company waiting for response.
response 'Company name' - Marks the company recieved response and lets you add a response.";
}

if (args.Length == 0)
{
    Console.WriteLine(repo.GetAll());
    Console.WriteLine("flist help to get se all methods available");
    return;
}

if (args.Length == 1)
{
    string returnString = args[0] switch
    {
        "add" => repo.Add(CompanyFactory.Make()),
        "get" => repo.GetCompany(Utils.GetString("Företags namn: ->> ")),
        "all" => repo.GetAll(),
        "remove" => repo.Remove(Utils.GetString("Företags namn: ->> ")),
        "contacted" => repo.SetContacted(Utils.GetString("Företags namn: ->> ")),
        "response" => repo.SetResponse(Utils.GetString("Företags namn: ->> "), Utils.GetResponse),
        "waiting" => repo.GetWaitingForResponse(),
        "responded" => repo.GetResponded(),
        "help" => Help(),
        _ => "Felaktigt Argument."
    };

    Console.WriteLine(returnString);
}
else if (args.Length == 2 && args[1].Length > 1)
{
    string input = $"{char.ToUpper(args[1][0])}{args[1][1..].ToLower()}";
    string returnString = args[0] switch
    {
        "get" => repo.GetCompany(input),
        "remove" => repo.Remove(input),
        "contacted" => repo.SetContacted(input),
        "response" => repo.SetResponse(input, Utils.GetResponse),
        _ => "Felaktigt argument"
    };

    Console.WriteLine(returnString);
}