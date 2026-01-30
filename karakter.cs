namespace Roman_Evreni;

public class Karakter
{
    // Karakterin temel özellikleri
    public string Ad { get; set; }
    public string Rol { get; set; }
    public int Yas { get; set; }
    public string Hikaye { get; set; }

    // Yeni bir karakter oluştururken çalışan kısım
    public Karakter(string ad, string rol, int yas, string hikaye)
    {
        Ad = ad;
        Rol = rol;
        Yas = yas;
        Hikaye = hikaye;
    }

    // Bilgileri ekrana yazdıran fonksiyon
    public void Bilgiyazdir()
    {
        Console.WriteLine($"--- {Ad} ---");
        Console.WriteLine($"Rol:    {Rol}");
        Console.WriteLine($"Yaş:    {Yas}");
        Console.WriteLine($"Hikaye: {Hikaye}");
        Console.WriteLine("---------------------------------");
    }
}