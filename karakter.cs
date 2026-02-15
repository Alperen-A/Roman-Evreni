namespace Roman_Evreni;

public class Karakter
{
    public string Ad { get; set; }
    public string Rol { get; set; }
    public string Hikaye { get; set; }

    // Kurucu metod (Sayı yok, sadece 3 bilgi)
    public Karakter(string ad, string rol, string hikaye)
    {
        Ad = ad;
        Rol = rol;
        Hikaye = hikaye;
    }

    // İŞTE EKSİK OLAN KISIM BURASIYDI:
    public void Bilgiyazdir()
    {
        Console.WriteLine($"Karakter: {Ad} - {Rol}");
        Console.WriteLine($"Hikaye: {Hikaye}");
        Console.WriteLine("-------------------------");
    }
}