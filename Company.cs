namespace LiaLista;

public struct Company
{
    public string CompanyName { get; init; }
    public string PhoneNumber { get; init; }
    public string Website { get; init; }
    public string Focus { get; init; }
    public string Location { get; init; }
    public int Intrest { get; init; }
    public bool Contacted { get; init; } = false;
    public string Response { get; init; } = string.Empty;

    public Company(
            string companyName,
            string phoneNumber,
            string website,
            string focus,
            string location,
            int intrest)
    {
        CompanyName = companyName;
        PhoneNumber = phoneNumber;
        Website = website;
        Focus = focus;
        Location = location;
        Intrest = intrest;
    }

    public Company(
                string companyName,
                string phoneNumber,
                string website,
                string focus,
                string location,
                int intrest,
                bool contacted)
    {
        CompanyName = companyName;
        PhoneNumber = phoneNumber;
        Website = website;
        Focus = focus;
        Location = location;
        Intrest = intrest;
        Contacted = contacted;
    }

    public Company(
                string companyName,
                string phoneNumber,
                string website,
                string focus,
                string location,
                int intrest,
                bool contacted,
                string response)
    {
        CompanyName = companyName;
        PhoneNumber = phoneNumber;
        Website = website;
        Focus = focus;
        Location = location;
        Intrest = intrest;
        Contacted = contacted;
        Response = response;
    }

    public override string ToString() =>
        @$"_________________________
Företags namn: {CompanyName}.

Kontakt Uppgifter:
Telefon Nummer: {PhoneNumber}.
Hemsida: {Website}.
Adress: {Location}.

Teknologier: {Focus}.

Intresse Rating: {Intrest}.

Kontaktat: {(Contacted
                ? Response != string.Empty
                    ? "Har fått Respons från företaget."
                    : "Väntar på Respons från företaget"
                : "Har inte kontaktat företaget.")}.
{(Response == String.Empty ? "" : $"\nResponse: {Response}\n")}";
}