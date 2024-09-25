// Cli för att skapa en lista av intressanta företag.
// Företags namn, Nummer, Hemsida, Orientering, Ort, Intresse.
using LiaLista;

Repository repo = new("companies.csv");

//repo.Add(CompanyFactory.Make());

Console.WriteLine(repo.GetAll());