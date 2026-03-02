using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Roman_evreni_mobil;

public static class Yapay_zeka_motoru
{
    private static readonly string Api_key = "AIzaSyArUY_t9wZVhwpd52yAbEhELVw5QQdqKi0"; 

    public static async Task<string> Tavsiye_al(string Soru, string Evren_verisi_json)
    {
        string Url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={Api_key}";

        using var Client = new HttpClient();
        
        string Sistem_komutu = @"Sen profesyonel bir fantastik roman yazar asistanisin.
Gorev 1: Kullanici sana uzun bir hikaye notu veya paragraf atarsa, icindeki karakterleri, mekanlari ve olaylari analiz et.
Gorev 2: Buldugun her yeni karakter icin sadece su tam formati kullan:
[Karakter_ekle: İsim | Dış görünüşü | Arka plan hikayesi]
Gorev 3: Buldugun mekanlar icin sadece su formati kullan:
[Mekan_ekle: İsim | İklimi | Hükümdarı | Tarihi]
Gorev 4: Buldugun olaylar icin sadece su formati kullan:
[Olay_ekle: Olay adı | İlgili mekanlar | İlgili karakterler | Olayın detayı]
Gorev 5: Eger listeye eklenecek bir sey yoksa normal sohbet et ve tavsiye ver.";

        string Tam_istek = $"{Sistem_komutu}\n\nMevcut evren:\n{Evren_verisi_json}\n\nYazarin girdisi: {Soru}";

        var Body = new { contents = new[] { new { parts = new[] { new { text = Tam_istek } } } } };
        var Content = new StringContent(JsonSerializer.Serialize(Body), Encoding.UTF8, "application/json");

        try
        {
            var Response = await Client.PostAsync(Url, Content);
            if (!Response.IsSuccessStatusCode) return $"Google hatasi: {Response.StatusCode} - {Response.ReasonPhrase}";
            
            string Response_json = await Response.Content.ReadAsStringAsync();
            using JsonDocument Doc = JsonDocument.Parse(Response_json);
            var Text = Doc.RootElement.GetProperty("candidates")[0]
                           .GetProperty("content")
                           .GetProperty("parts")[0]
                           .GetProperty("text").GetString();

            return Text ?? "Cevap anlasilamadi.";
        }
        catch (Exception Ex)
        {
            return $"Sistem hatasi: {Ex.Message}";
        }
    }
}