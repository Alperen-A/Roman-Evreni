using System.Collections.Generic;

namespace Roman_evreni_mobil;

public class Evren_verisi
{
    public string Evren_adi { get; set; } = "";
    public List<string> Karakterler { get; set; } = new();
    public List<string> Mekanlar { get; set; } = new();
    public List<string> Olaylar { get; set; } = new();
}