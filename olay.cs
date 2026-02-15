namespace Roman_Evreni;

public class Olay
{
    public string Ad { get; set; }
    public string Tarih { get; set; }
    public string Sonuc { get; set; }

    public Olay(string ad, string tarih, string sonuc)
    {
        Ad = ad;
        Tarih = tarih;
        Sonuc = sonuc;
    }

    public void Bilgiyazdir()
    {
        Console.WriteLine($"Olay: {Ad} - Tarih: {Tarih}");
        Console.WriteLine($"Sonuç: {Sonuc}");
        Console.WriteLine("-------------------------");
    }
}