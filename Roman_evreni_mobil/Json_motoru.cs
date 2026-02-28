using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using Microsoft.Maui.Storage;

namespace Roman_evreni_mobil;

public static class Json_motoru
{
    private static string Dosya_yolu = Path.Combine(FileSystem.AppDataDirectory, "roman_kayitlari.json");

    public static void Kaydet(List<Evren_verisi> evrenler)
    {
        string json_metni = JsonSerializer.Serialize(evrenler);
        File.WriteAllText(Dosya_yolu, json_metni);
    }

    public static List<Evren_verisi> Yukle()
    {
        if (!File.Exists(Dosya_yolu))
        {
            return new List<Evren_verisi>();
        }

        // Eski yapidaki dosyayi okurken cokerse diye guvenlik onlemi
        try 
        {
            string json_metni = File.ReadAllText(Dosya_yolu);
            return JsonSerializer.Deserialize<List<Evren_verisi>>(json_metni) ?? new List<Evren_verisi>();
        }
        catch 
        {
            return new List<Evren_verisi>(); // Hata verirse temiz bir baslangic yapar
        }
    }
}