using System.Collections.Generic;

namespace Roman_evreni_mobil;

public class Karakter_bilgisi
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Ad { get; set; } = "";
    public string Dis_gorunus { get; set; } = "";
    public string Hikaye { get; set; } = "";
    public bool Favori { get; set; } = false;
}

public class Mekan_bilgisi
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Ad { get; set; } = "";
    public string Iklim { get; set; } = "";
    public string Hukumdar { get; set; } = "";
    public string Aciklama { get; set; } = "";
    public bool Favori { get; set; } = false;
}

public class Olay_bilgisi
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Ad { get; set; } = "";
    public List<string> Karakter_idleri { get; set; } = new();
    public List<string> Mekan_idleri { get; set; } = new();
    public string Aciklama { get; set; } = "";
    public bool Favori { get; set; } = false;
}

public class Evren_verisi
{
    public string Evren_adi { get; set; } = "";
    public List<Karakter_bilgisi> Karakterler { get; set; } = new();
    public List<Mekan_bilgisi> Mekanlar { get; set; } = new();
    public List<Olay_bilgisi> Olaylar { get; set; } = new();
}