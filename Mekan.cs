namespace Roman_Evreni;

public class Mekan
{
    // Mekanın temel özellikleri
    public string Ad { get; set; }
    public string Tur { get; set; } // Örn: Kale, Orman, Şehir
    public string Tarihce { get; set; }

    // Yeni mekan oluştururken çalışan kısım
    public Mekan(string ad, string tur, string tarihce)
    {
        Ad = ad;
        Tur = tur;
        Tarihce = tarihce;
    }

    // Bilgileri ekrana yazdıran fonksiyon
    public void Bilgiyazdir()
    {
        Console.WriteLine($"--- {Ad} ---");
        Console.WriteLine($"Tür:      {Tur}");
        Console.WriteLine($"Tarihçe:  {Tarihce}");
        Console.WriteLine("---------------------------------");
    }
}