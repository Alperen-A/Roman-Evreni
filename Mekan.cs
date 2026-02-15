namespace Roman_Evreni;

public class Mekan
{
    public string Ad { get; set; }
    public string Tur { get; set; }
    public string Hikaye { get; set; }

    public Mekan(string ad, string tur, string hikaye)
    {
        Ad = ad;
        Tur = tur;
        Hikaye = hikaye;
    }

    public void Bilgiyazdir()
    {
        Console.WriteLine($"Mekan: {Ad} ({Tur})");
        Console.WriteLine($"Detay: {Hikaye}");
        Console.WriteLine("-------------------------");
    }
}