namespace Roman_Evreni;

public class Karakter
{
    public string Ad { get; set; }
    public string Rol { get; set; }
    public string Hikaye { get; set; }

    // Kurucu metod: Sadece 3 bilgi istiyoruz (Sayı yok!)
    public Karakter(string ad, string rol, string hikaye)
    {
        Ad = ad;
        Rol = rol;
        Hikaye = hikaye;
    }

    // Ekrana yazdırma özelliği
    public void Bilgiyazdir()
    {
        Console.WriteLine($"Karakter: {Ad} - {Rol}");
        Console.WriteLine($"Hikaye: {Hikaye}");
        Console.WriteLine("-------------------------");
    }
}