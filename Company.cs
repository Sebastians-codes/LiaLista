namespace LiaLista;

public struct Company(
        string companyName,
        string phoneNumber,
        string website,
        string focus,
        string location,
        int intrest)
{
    public string CompanyName { get; init; } = companyName;
    public string PhoneNumber { get; init; } = phoneNumber;
    public string Website { get; init; } = website;
    public string Focus { get; init; } = focus;
    public string Location { get; init; } = location;
    public int Intrest { get; init; } = intrest;

    public override string ToString() =>
        @$"_________________________
Företags namn: {CompanyName}.

Kontakt Uppgifter:
Telefon Nummer: {PhoneNumber}.
Hemsida: {Website}.
Adress: {Location}.

Teknologier: {Focus}.

Intresse Rating: {Intrest}.{"\n\n"}";
}