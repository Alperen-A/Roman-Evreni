namespace Roman_Evreni;

public class Olay
{
    public string Ad { get; set; }
    public string Karakterler { get; set; }
    public string Tarih { get; set; }
    public string Betimleme { get; set; }

    public Olay(string ad, string karakterler, string tarih, string betimleme)
    {
        Ad = ad;
        Karakterler = karakterler;
        Tarih = tarih;
        Betimleme = betimleme;
    }

    public void Bilgiyazdir()
    {
        Console.WriteLine($"Olay: {Ad} - Tarih: {Tarih}");
        Console.WriteLine($"Karakterler: {Karakterler}");
        Console.WriteLine($"Betimleme: {Betimleme}");
        Console.WriteLine("-------------------------");
    }
}