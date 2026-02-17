namespace Roman_Evreni;

public class Karakter
{
    public string Ad { get; set; }
    public string Gorunum { get; set; } // YENİ: Fiziksel özellikler
    public string Hikaye { get; set; }  // YENİ: Uzun arka plan hikayesi

    // Kurucu metod güncellendi
    public Karakter(string ad, string gorunum, string hikaye)
    {
        Ad = ad;
        Gorunum = gorunum;
        Hikaye = hikaye;
    }

    public void Bilgiyazdir()
    {
        Console.WriteLine($"┌── {Ad.ToUpper()} ──────────────────────────");
        Console.WriteLine($"│ Görünüm: {Gorunum}");
        Console.WriteLine($"│ Hikaye:  {Hikaye}"); // Uzun hikaye burada görünecek
        Console.WriteLine($"└──────────────────────────────────────────\n");
    }
}