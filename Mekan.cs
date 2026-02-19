namespace Roman_Evreni;

public class Mekan
{
    public string Ad { get; set; }
    public string Betimleme { get; set; }

    public Mekan(string ad, string betimleme)
    {
        Ad = ad;
        Betimleme = betimleme;
    }

    public void Bilgiyazdir()
    {
        Console.WriteLine($"Mekan: {Ad}");
        Console.WriteLine($"Betimleme: {Betimleme}");
        Console.WriteLine("-------------------------");
    }
}