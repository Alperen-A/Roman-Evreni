using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Roman_evreni_mobil;

public static class Yapay_zeka_motoru
{
   private static readonly string Api_key = ""; // API anahtarını buraya yapıştır

    public static async Task<string> Tavsiye_al(string Soru, string Evren_verisi_json)
    {
        string Url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={Api_key}";

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

        return await Api_isteği_gonder(Tam_istek);
    }

    public static async Task<string> Roman_yaz(string Istek, string Evren_verisi_json)
    {
        string Url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={Api_key}";

        string Sistem_komutu = @"Sen yetenekli bir roman yazarısın. Kullanıcının verdiği karakterler, mekanlar ve talimatlar doğrultusunda akıcı, etkileyici ve yaratıcı roman metni yazarsın. 
- Diyalogları canlı ve gerçekçi yaz
- Atmosfer ve duygu aktarımına önem ver  
- Kullanıcının evren verilerindeki karakterlere ve mekanlara sadık kal
- Sadece roman metnini yaz, açıklama ekleme";

        string Tam_istek = $"{Sistem_komutu}\n\nEvren bilgileri:\n{Evren_verisi_json}\n\nYazılacak sahne veya bölüm: {Istek}";

        return await Api_isteği_gonder(Tam_istek);
    }

    private static async Task<string> Api_isteği_gonder(string istek)
    {
        string Url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={Api_key}";
        using var Client = new HttpClient();

        var Body = new { contents = new[] { new { parts = new[] { new { text = istek } } } } };
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