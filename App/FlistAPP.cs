using LiaLista.CompanyStructure;
using LiaLista.SqlDatabases;

namespace LiaLista.App;

public class FlistApp(string[] args)
{
    public void Run()
    {
        SqlRepo repo = new(SqlType.Sqlite);

        switch (args.Length)
        {
            case 0:
                Console.WriteLine(repo.GetAll());
                Console.WriteLine("flist help to se all methods available");
                return;
            case 1:
                {
                    string returnString = args[0] switch
                    {
                        "add" => repo.Add(CompanyFactory.Make()),
                        "get" => repo.GetCompany(Utils.GetString("Företags namn: ->> ")),
                        "all" => repo.GetAll(),
                        "remove" => repo.Remove(Utils.GetString("Företags namn: ->> ")),
                        "contacted" => repo.SetContacted(Utils.GetString("Företags namn: ->> ")),
                        "response" => repo.SetResponse(Utils.GetString("Företags namn: ->> "), Utils.GetResponse()),
                        "waiting" => repo.GetWaitingForResponse(),
                        "responded" => repo.GetResponded(),
                        "help" => Help(),
                        _ => "Felaktigt Argument."
                    };

                    Console.WriteLine(returnString);
                    break;
                }
            case 2 when args[1].Length > 1:
                {
                    string input = $"{char.ToUpper(args[1][0])}{args[1][1..].ToLower()}";
                    input = input.Replace(',', '.');
                    string returnString = args[0] switch
                    {
                        "get" => repo.GetCompany(input),
                        "remove" => repo.Remove(input),
                        "contacted" => repo.SetContacted(input),
                        "response" => repo.SetResponse(input, Utils.GetResponse()),
                        _ => "Felaktigt argument"
                    };

                    Console.WriteLine(returnString);
                    break;
                }
        }

        return;

        string Help()
        {
            Console.Clear();
            return """
                   Methods to use - flist 'method'
                   add - adds a new company to the list.
                   get - gets a company stored in the database.
                   all or flist only - get all companies and info stored in the database.
                   remove - remove a company in the database.
                   contacted - Mark company as contacted and waiting for response.
                   response - Sets contacted to received response and lets you add a response field.
                   waiting - gets all companies that hasn't given a response.
                   responded - gets all companies that has responded.
                   help - shows the help menu.

                   Extra Functionality.
                   get 'Company name' - gets you the company.
                   remove 'Company name' - removes the company from the database.
                   contacted 'Company name' - Marks the company waiting for response.
                   response 'Company name' - Marks the company received response and lets you add a response.
                   """;
        }
    }
}