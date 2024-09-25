namespace LiaLista;

public static class CompanyFactory
{
    public static Company Make()
    {
        string companyName = Utils.GetString("Företags namn: ->> ");
        string phoneNumber = Utils.GetString("Telefon nummer: ->> ");
        string website = Utils.GetString("Hemsida: ->> ");
        string focus = Utils.GetString("Fokus: ->> ");
        string location = Utils.GetString("Address: ->> ");
        int intrest = Utils.GetInt("Intresse: ->> ");

        return new Company(companyName, phoneNumber, website, focus, location, intrest);
    }
}