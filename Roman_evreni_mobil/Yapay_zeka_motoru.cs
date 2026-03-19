using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Roman_evreni_mobil;

public static class Yapay_zeka_motoru
{
    private static string? _api_key;
    private static string Api_key
    {
        get
        {
            if (_api_key != null) return _api_key;
            try
            {
                using var stream = FileSystem.OpenAppPackageFileAsync("appsettings.json").Result;
                using var reader = new System.IO.StreamReader(stream);
                var json = reader.ReadToEnd();
                using var doc = JsonDocument.Parse(json);
                _api_key = doc.RootElement.GetProperty("GeminiApiKey").GetString() ?? "";
            }
            catch { _api_key = ""; }
            return _api_key;
        }
    }

    public static async Task<string> Tavsiye_al(string Soru, string Evren_verisi_json)
    {
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
        string Url = $"https://api.groq.com/openai/v1/chat/completions";
        using var Client = new HttpClient();
        Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Api_key}");

        var Body = new
        {
            model = "llama-3.3-70b-versatile",
            messages = new[]
            {
                new { role = "user", content = istek }
            },
            max_tokens = 2048
        };

        var Content = new StringContent(JsonSerializer.Serialize(Body), Encoding.UTF8, "application/json");

        try
        {
            var Response = await Client.PostAsync(Url, Content);
            if (!Response.IsSuccessStatusCode) return $"Hata: {Response.StatusCode} - {await Response.Content.ReadAsStringAsync()}";

            string Response_json = await Response.Content.ReadAsStringAsync();
            using JsonDocument Doc = JsonDocument.Parse(Response_json);
            var Text = Doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return Text ?? "Cevap anlasilamadi.";
        }
        catch (Exception Ex)
        {
            return $"Sistem hatasi: {Ex.Message}";
        }
    }
}