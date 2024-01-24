namespace ZaemService;

public class zaemi
{
    public int id { get; set; }
    public string familiya { get; set; }
    public string data_podpisaniya { get; set; }
    public string famil { get; set; }
}

public class sotrudniki
{
    public int id { get; set; }
    public string famil { get; set; }
    public string name { get; set; }
    public string otchestv { get; set; }
    public string nazvanie { get; set; }
}

public class zaemshik
{
    public int id { get; set; }
    public string familiya { get; set; }
    public string imya { get; set; }
    public string otchestvo { get; set; }
    public int pasport { get; set; }
    public string telephone { get; set; }
    public string adres_raboti { get; set; }
    public string zdanie_raboti { get; set; }
}