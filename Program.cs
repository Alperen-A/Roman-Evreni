using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Roman_Evreni;

// --- 1. Listeler ve degiskenler ---
List<Karakter> Karakterler = new List<Karakter>();
List<Mekan> Mekanlar = new List<Mekan>();
List<Olay> Olaylar = new List<Olay>();

string aktifEvren = "";
string dosyaYolu = "";

// --- 2. Giris ve onay ekrani ---
while (true)
{
    Console.Clear();
    Console.WriteLine("--- Roman evreni yonetim sistemi ---");
    Console.Write("Hangi roman evreni uzerinde calismak istiyorsun? (Cikis icin iptal yaz): ");
    aktifEvren = Console.ReadLine() ?? "";

    if (aktifEvren.ToLower() == "iptal") 
    {
        Console.WriteLine("Cikis yapiliyor...");
        return; 
    }

    if (string.IsNullOrWhiteSpace(aktifEvren)) continue;

    dosyaYolu = aktifEvren + ".json";

    if (File.Exists(dosyaYolu))
    {
        Console.Write($"'{aktifEvren}' adli mevcut kayit bulundu. Yuklensin mi? (Evet icin e, Hayir/Geri icin h): ");
        string onay = Console.ReadLine()?.ToLower() ?? "";
        
        if (onay == "e")
        {
            string jsonOku = File.ReadAllText(dosyaYolu);
            var yuklenenVeri = JsonSerializer.Deserialize<EvrenVerisi>(jsonOku);
            if (yuklenenVeri != null)
            {
                Karakterler = yuklenenVeri.Karakterler ?? new List<Karakter>();
                Mekanlar = yuklenenVeri.Mekanlar ?? new List<Mekan>();
                Olaylar = yuklenenVeri.Olaylar ?? new List<Olay>();
            }
            Console.WriteLine("Kayit basariyla yuklendi! Devam etmek icin enter tusuna bas...");
            Console.ReadLine();
            break; 
        }
    }
    else
    {
        Console.Write($"'{aktifEvren}' adinda yeni bir evren olusturulacak. Onayliyor musun? (Evet icin e, Iptal/Geri icin h): ");
        string onay = Console.ReadLine()?.ToLower() ?? "";
        
        if (onay == "e")
        {
            Console.WriteLine("Yeni evren hazir! Devam etmek icin enter tusuna bas...");
            Console.ReadLine();
            break; 
        }
    }
}

// --- 3. Main menu ---
while (true) 
{
    Console.Clear(); 
    Console.WriteLine("-------------------------------------------------");
    Console.WriteLine($"  Evren: {aktifEvren} | K: {Karakterler.Count} | M: {Mekanlar.Count} | O: {Olaylar.Count}   ");
    Console.WriteLine("-------------------------------------------------");
    Console.WriteLine("1. Karakter yonetimi");
    Console.WriteLine("2. Mekan yonetimi");
    Console.WriteLine("3. Olay yonetimi");
    Console.WriteLine("4. Kaydet ve cikis");
    Console.WriteLine("5. Yapay zeka ile metin analizi");
    Console.WriteLine("6. Mevcut evreni kalici olarak sil");
    Console.Write("\nBolum seciniz: ");
    
    string mainSecim = Console.ReadLine() ?? "";

    if (mainSecim == "1") 
    {
        while (true)
        {
            Console.WriteLine("\n-- Karakter menusu --");
            Console.WriteLine("[A] Ekle | [S] Sil | [D] Duzelt | [L] Listele | [F] Ara | [G] Geri");
            Console.Write("Secim: ");
            string kSecim = Console.ReadLine() ?? "";
            
            if (kSecim.ToLower() == "g") break;

            if (kSecim.ToLower() == "a")
            {
                Console.Write("Karakter adi (Veya iptal): "); 
                string ad = Console.ReadLine() ?? "Isimsiz";
                if(ad.ToLower() == "iptal") continue;

                Console.Write("Fiziksel gorunum: "); 
                string gorunum = Console.ReadLine() ?? "Belirtilmedi";

                Console.WriteLine("Arka plan / Hikaye: ");
                string hikaye = Console.ReadLine() ?? "Hikaye girilmedi.";

                Karakterler.Add(new Karakter(ad, gorunum, hikaye));
                Console.WriteLine("--> Karakter eklendi!");
            }
            else if (kSecim.ToLower() == "s")
            {
                Console.Write("Silinecek isim (Veya iptal): "); string s = Console.ReadLine() ?? "";
                if(s.ToLower() == "iptal") continue;
                
                var h = Karakterler.Find(x => x.Ad.ToLower() == s.ToLower());
                if (h != null) { Karakterler.Remove(h); Console.WriteLine("Silindi!"); }
                else Console.WriteLine("Bulunamadi.");
            }
            else if (kSecim.ToLower() == "d") // Yeni duzeltme ozelligi
            {
                Console.Write("Duzeltilecek karakterin adi (Veya iptal): "); string s = Console.ReadLine() ?? "";
                if(s.ToLower() == "iptal") continue;

                var k = Karakterler.Find(x => x.Ad.ToLower() == s.ToLower());
                if (k != null)
                {
                    Console.WriteLine($"Mevcut gorunum: {k.Gorunum}");
                    Console.Write("Yeni gorunum (Ayni kalmasi icin bos birakip enter'a bas): ");
                    string yGorunum = Console.ReadLine() ?? "";
                    if (!string.IsNullOrWhiteSpace(yGorunum)) k.Gorunum = yGorunum;

                    Console.WriteLine($"Mevcut hikaye: {k.Hikaye}");
                    Console.Write("Yeni hikaye (Ayni kalmasi icin bos birakip enter'a bas): ");
                    string yHikaye = Console.ReadLine() ?? "";
                    if (!string.IsNullOrWhiteSpace(yHikaye)) k.Hikaye = yHikaye;

                    Console.WriteLine("--> Karakter basariyla guncellendi!");
                }
                else Console.WriteLine("Karakter bulunamadi.");
            }
            else if (kSecim.ToLower() == "l")
            {
                Console.WriteLine("\n--- Karakter detaylari ---");
                foreach (var k in Karakterler) k.Bilgiyazdir();
                Console.WriteLine("Devam icin enter..."); Console.ReadLine();
            }
            else if (kSecim.ToLower() == "f")
            {
                Console.Write("Aranacak kelime: "); 
                string f = Console.ReadLine()?.ToLower() ?? "";
                var sonuclar = Karakterler.FindAll(x => x.Hikaye.ToLower().Contains(f) || x.Gorunum.ToLower().Contains(f));
                
                Console.WriteLine($"\n--- '{f}' Iceren karakterler ---");
                foreach (var item in sonuclar) item.Bilgiyazdir();
                Console.ReadLine();
            }
        }
    }
    else if (mainSecim == "2") 
    {
        while (true)
        {
            Console.WriteLine("\n-- Mekan menusu --");
            Console.WriteLine("[A] Ekle | [S] Sil | [D] Duzelt | [L] Listele | [G] Geri");
            Console.Write("Secim: ");
            string mSecim = Console.ReadLine() ?? "";
            
            if (mSecim.ToLower() == "g") break;

            if (mSecim.ToLower() == "a") {
                Console.Write("Mekan adi (Veya iptal): "); string n = Console.ReadLine() ?? "Isimsiz";
                if(n.ToLower() == "iptal") continue;
                
                Console.Write("Betimleme (Atmosfer, koku vb.): "); string b = Console.ReadLine() ?? "Belirtilmedi";
                Mekanlar.Add(new Mekan(n, b));
                Console.WriteLine("--> Mekan eklendi!");
            }
            else if (mSecim.ToLower() == "s") {
                Console.Write("Silinecek mekan adi (Veya iptal): "); string s = Console.ReadLine() ?? "";
                if(s.ToLower() == "iptal") continue;
                
                var h = Mekanlar.Find(x => x.Ad.ToLower() == s.ToLower());
                if (h != null) { Mekanlar.Remove(h); Console.WriteLine("Silindi!"); }
                else Console.WriteLine("Bulunamadi.");
            }
            else if (mSecim.ToLower() == "d") // Yeni duzeltme ozelligi
            {
                Console.Write("Duzeltilecek mekanin adi (Veya iptal): "); string s = Console.ReadLine() ?? "";
                if(s.ToLower() == "iptal") continue;

                var m = Mekanlar.Find(x => x.Ad.ToLower() == s.ToLower());
                if (m != null)
                {
                    Console.WriteLine($"Mevcut betimleme: {m.Betimleme}");
                    Console.Write("Yeni betimleme (Ayni kalmasi icin bos birakip enter'a bas): ");
                    string yBetimleme = Console.ReadLine() ?? "";
                    if (!string.IsNullOrWhiteSpace(yBetimleme)) m.Betimleme = yBetimleme;

                    Console.WriteLine("--> Mekan basariyla guncellendi!");
                }
                else Console.WriteLine("Mekan bulunamadi.");
            }
            else if (mSecim.ToLower() == "l") {
                Console.WriteLine("\n--- Mekan detaylari ---");
                foreach (var m in Mekanlar) m.Bilgiyazdir();
                Console.WriteLine("Devam icin enter..."); Console.ReadLine();
            }
        }
    }
    else if (mainSecim == "3") 
    {
        while (true)
        {
            Console.WriteLine("\n-- Olay menusu --");
            Console.WriteLine("[A] Ekle | [S] Sil | [D] Duzelt | [L] Listele | [G] Geri");
            Console.Write("Secim: ");
            string oSecim = Console.ReadLine() ?? "";
            
            if (oSecim.ToLower() == "g") break;

            if (oSecim.ToLower() == "a") {
                Console.Write("Olay adi (Veya iptal): "); string n = Console.ReadLine() ?? "Bilinmiyor";
                if(n.ToLower() == "iptal") continue;
                
                Console.Write("Icindeki karakterler: "); string k = Console.ReadLine() ?? "Belirtilmedi";
                Console.Write("Tarih: "); string t = Console.ReadLine() ?? "Bilinmiyor";
                Console.Write("Betimleme (Sonuclar, kayiplar vb.): "); string b = Console.ReadLine() ?? "Belirtilmedi";
                Olaylar.Add(new Olay(n, k, t, b));
                Console.WriteLine("--> Olay eklendi!");
            }
            else if (oSecim.ToLower() == "s") {
                Console.Write("Silinecek olay adi (Veya iptal): "); string s = Console.ReadLine() ?? "";
                if(s.ToLower() == "iptal") continue;
                
                var h = Olaylar.Find(x => x.Ad.ToLower() == s.ToLower());
                if (h != null) { Olaylar.Remove(h); Console.WriteLine("Silindi!"); }
                else Console.WriteLine("Bulunamadi.");
            }
            else if (oSecim.ToLower() == "d") // Yeni duzeltme ozelligi
            {
                Console.Write("Duzeltilecek olayin adi (Veya iptal): "); string s = Console.ReadLine() ?? "";
                if(s.ToLower() == "iptal") continue;

                var o = Olaylar.Find(x => x.Ad.ToLower() == s.ToLower());
                if (o != null)
                {
                    Console.WriteLine($"Mevcut karakterler: {o.Karakterler}");
                    Console.Write("Yeni karakterler (Ayni kalmasi icin bos birakip enter'a bas): ");
                    string yKarakterler = Console.ReadLine() ?? "";
                    if (!string.IsNullOrWhiteSpace(yKarakterler)) o.Karakterler = yKarakterler;

                    Console.WriteLine($"Mevcut tarih: {o.Tarih}");
                    Console.Write("Yeni tarih (Ayni kalmasi icin bos birakip enter'a bas): ");
                    string yTarih = Console.ReadLine() ?? "";
                    if (!string.IsNullOrWhiteSpace(yTarih)) o.Tarih = yTarih;

                    Console.WriteLine($"Mevcut betimleme: {o.Betimleme}");
                    Console.Write("Yeni betimleme (Ayni kalmasi icin bos birakip enter'a bas): ");
                    string yBetimleme = Console.ReadLine() ?? "";
                    if (!string.IsNullOrWhiteSpace(yBetimleme)) o.Betimleme = yBetimleme;

                    Console.WriteLine("--> Olay basariyla guncellendi!");
                }
                else Console.WriteLine("Olay bulunamadi.");
            }
            else if (oSecim.ToLower() == "l") {
                Console.WriteLine("\n--- Olay detaylari ---");
                foreach (var o in Olaylar) o.Bilgiyazdir();
                Console.WriteLine("Devam icin enter..."); Console.ReadLine();
            }
        }
    }
    else if (mainSecim == "4") 
    {
        var veri = new EvrenVerisi 
        {
            Karakterler = Karakterler,
            Mekanlar = Mekanlar,
            Olaylar = Olaylar
        };
        
        string jsonKayit = JsonSerializer.Serialize(veri, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(dosyaYolu, jsonKayit);
        
        Console.WriteLine($"{aktifEvren} evreni basariyla kaydedildi. Iyi gunler!");
        break;
    }
   
    else if (mainSecim == "5") 
    {
        Console.Clear();
        Console.WriteLine("\n-- Yapay zeka ile metin analizi --");
        Console.WriteLine("1. Metni dogrudan yapistir");
        Console.WriteLine("2. Txt dosyasindan okut");
        Console.WriteLine("3. Geri don");
        Console.Write("Seciminiz: ");
        
        string aiSecim = Console.ReadLine() ?? "";
        string metin = "";

        if (aiSecim == "1")
        {
            Console.WriteLine("Buraya romanindan bir parca yapistir.");
            Console.Write("\nMetni yapistir: ");
            metin = Console.ReadLine() ?? "";
        }
        else if (aiSecim == "2")
        {
            Console.Write("\nTxt dosyasinin tam yolunu gir (Ornek: C:\\yazilar\\bolum1.txt): ");
            string dosyaYoluAi = Console.ReadLine() ?? "";
            
            if (File.Exists(dosyaYoluAi))
            {
                metin = File.ReadAllText(dosyaYoluAi);
                Console.WriteLine("--> Dosya basariyla okundu!");
            }
            else
            {
                Console.WriteLine("Dosya bulunamadi. Lutfen yolu kontrol edip tekrar dene.");
                continue;
            }
        }
        else
        {
            continue;
        }

        if (string.IsNullOrWhiteSpace(metin)) continue;

        Console.WriteLine("\nYapay zeka dusunuyor, lutfen bekle...");

        try
        {
            string apiKey = File.ReadAllText("Api_key.txt").Trim();
            using HttpClient client = new HttpClient();
            string url = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key=" + apiKey;

            string prompt = "Su metni analiz et ve icindeki karakterleri, mekanlari, olaylari bul. Sadece Json formatinda dondur. Kesinlikle markdown kullanma. Format su olsun: {\"Karakterler\": [{\"Ad\": \"...\", \"Gorunum\": \"...\", \"Hikaye\": \"...\"}], \"Mekanlar\": [{\"Ad\": \"...\", \"Betimleme\": \"...\"}], \"Olaylar\": [{\"Ad\": \"...\", \"Karakterler\": \"...\", \"Tarih\": \"...\", \"Betimleme\": \"...\"}]}. Metin: " + metin;

            var istekGovdesi = new
            {
                contents = new[]
                {
                    new { parts = new[] { new { text = prompt } } }
                }
            };

            string jsonIstek = JsonSerializer.Serialize(istekGovdesi);
            var icerik = new StringContent(jsonIstek, Encoding.UTF8, "application/json");

            HttpResponseMessage cevap = await client.PostAsync(url, icerik);
            string jsonCevap = await cevap.Content.ReadAsStringAsync();

            using JsonDocument doc = JsonDocument.Parse(jsonCevap);
            JsonElement root = doc.RootElement;
            string aiMetni = root.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString() ?? "";

            var aiVerisi = JsonSerializer.Deserialize<EvrenVerisi>(aiMetni.Trim());

            if (aiVerisi != null)
            {
                int kSayisi = aiVerisi.Karakterler?.Count ?? 0;
                int mSayisi = aiVerisi.Mekanlar?.Count ?? 0;
                int oSayisi = aiVerisi.Olaylar?.Count ?? 0;

                if (aiVerisi.Karakterler != null) Karakterler.AddRange(aiVerisi.Karakterler);
                if (aiVerisi.Mekanlar != null) Mekanlar.AddRange(aiVerisi.Mekanlar);
                if (aiVerisi.Olaylar != null) Olaylar.AddRange(aiVerisi.Olaylar);

                Console.WriteLine($"\nHarika! Analiz tamamlandi.");
                Console.WriteLine($"Eklenenler: {kSayisi} Karakter, {mSayisi} Mekan, {oSayisi} Olay.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("\nBir hata olustu. Yapay zeka ile baglanti kurulamadi veya metin cozumlenemedi.");
            Console.WriteLine("Hata detayi: " + ex.Message);
        }

        Console.WriteLine("\nDevam etmek icin enter tusuna bas...");
        Console.ReadLine();
    }
    
    else if (mainSecim == "6") // Yeni evren silme ozelligi
    {
        Console.WriteLine("\n-- DIKKAT! TEHLIKELI ISLEM --");
        Console.Write($"'{aktifEvren}' evrenini ve icindeki her seyi KALICI olarak silmek istedigine emin misin? (Evet icin e, Iptal/Geri icin h): ");
        string silOnay = Console.ReadLine()?.ToLower() ?? "";

        if (silOnay == "e")
        {
            if (File.Exists(dosyaYolu))
            {
                File.Delete(dosyaYolu);
            }
            Console.WriteLine($"\n{aktifEvren} evreni tamamen silindi.");
            Console.WriteLine("Program kapatiliyor, yeniden baslatip baska bir evren secebilirsin...");
            break; // Programi sonlandirir
        }
    }
}

// --- 4. Veri tutucu sinif ---
public class EvrenVerisi 
{
    public List<Karakter> Karakterler { get; set; } = new List<Karakter>();
    public List<Mekan> Mekanlar { get; set; } = new List<Mekan>();
    public List<Olay> Olaylar { get; set; } = new List<Olay>();
}
