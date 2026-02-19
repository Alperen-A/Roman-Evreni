using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
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
        return; // Programi tamamen kapatir
    }

    if (string.IsNullOrWhiteSpace(aktifEvren)) continue;

    dosyaYolu = aktifEvren + ".json";

    if (File.Exists(dosyaYolu))
    {
        Console.Write($"'{aktifEvren}' adli mevcut kayit bulundu. Yuklensin mi? (Evet icin e, Hayir/Geri icin h): ");
        string onay = Console.ReadLine()?.ToLower() ?? "";
        
        if (onay == "e")
        {
            // Eski verileri Json formatindan okuyup listelere dolduruyoruz
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
            break; // Onaylandigi icin main menuye gecis yapiyoruz
        }
        // h yazarsa veya baska bir sey yazarsa basa donup tekrar evren ismi sorar
    }
    else
    {
        Console.Write($"'{aktifEvren}' adinda yeni bir evren olusturulacak. Onayliyor musun? (Evet icin e, Iptal/Geri icin h): ");
        string onay = Console.ReadLine()?.ToLower() ?? "";
        
        if (onay == "e")
        {
            Console.WriteLine("Yeni evren hazir! Devam etmek icin enter tusuna bas...");
            Console.ReadLine();
            break; // Onaylandigi icin main menuye bos listelerle gecis yapiyoruz
        }
        // h yazarsa iptal edip basa doner
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
    Console.Write("\nBolum seciniz: ");
    
    string mainSecim = Console.ReadLine() ?? "";

    if (mainSecim == "1") // Karakter odasi
    {
        while (true)
        {
            Console.WriteLine("\n-- Karakter menusu --");
            Console.WriteLine("[A] Ekle | [S] Sil | [L] Listele | [F] Hikayede ara | [G] Geri");
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
            else if (kSecim.ToLower() == "l")
            {
                Console.WriteLine("\n--- Karakter detaylari ---");
                foreach (var k in Karakterler) k.Bilgiyazdir();
                Console.WriteLine("Devam icin enter..."); Console.ReadLine();
            }
            else if (kSecim.ToLower() == "s")
            {
                Console.Write("Silinecek isim (Veya iptal): "); string s = Console.ReadLine() ?? "";
                if(s.ToLower() == "iptal") continue;
                
                var h = Karakterler.Find(x => x.Ad.ToLower() == s.ToLower());
                if (h != null) { Karakterler.Remove(h); Console.WriteLine("Silindi!"); }
                else Console.WriteLine("Bulunamadi.");
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
    else if (mainSecim == "2") // Mekan odasi
    {
        while (true)
        {
            Console.WriteLine("\n-- Mekan menusu --");
            Console.WriteLine("[A] Ekle | [S] Sil | [L] Listele | [G] Geri");
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
            else if (mSecim.ToLower() == "l") {
                Console.WriteLine("\n--- Mekan detaylari ---");
                foreach (var m in Mekanlar) m.Bilgiyazdir();
                Console.WriteLine("Devam icin enter..."); Console.ReadLine();
            }
            else if (mSecim.ToLower() == "s") {
                Console.Write("Silinecek mekan adi (Veya iptal): "); string s = Console.ReadLine() ?? "";
                if(s.ToLower() == "iptal") continue;
                
                var h = Mekanlar.Find(x => x.Ad.ToLower() == s.ToLower());
                if (h != null) { Mekanlar.Remove(h); Console.WriteLine("Silindi!"); }
                else Console.WriteLine("Bulunamadi.");
            }
        }
    }
    else if (mainSecim == "3") // Olay odasi
    {
        while (true)
        {
            Console.WriteLine("\n-- Olay menusu --");
            Console.WriteLine("[A] Ekle | [S] Sil | [L] Listele | [G] Geri");
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
            else if (oSecim.ToLower() == "l") {
                Console.WriteLine("\n--- Olay detaylari ---");
                foreach (var o in Olaylar) o.Bilgiyazdir();
                Console.WriteLine("Devam icin enter..."); Console.ReadLine();
            }
            else if (oSecim.ToLower() == "s") {
                Console.Write("Silinecek olay adi (Veya iptal): "); string s = Console.ReadLine() ?? "";
                if(s.ToLower() == "iptal") continue;
                
                var h = Olaylar.Find(x => x.Ad.ToLower() == s.ToLower());
                if (h != null) { Olaylar.Remove(h); Console.WriteLine("Silindi!"); }
                else Console.WriteLine("Bulunamadi.");
            }
        }
    }
    else if (mainSecim == "4") // Kaydet ve cikis
    {
        // Tum listeleri tek bir pakette topluyoruz
        var veri = new EvrenVerisi 
        {
            Karakterler = Karakterler,
            Mekanlar = Mekanlar,
            Olaylar = Olaylar
        };
        
        // Bu paketi bilgisayarin anlayacagi Json formatina ceviriyoruz
        string jsonKayit = JsonSerializer.Serialize(veri, new JsonSerializerOptions { WriteIndented = true });
        
        // Evren ismine ozel dosyaya kaydediyoruz
        File.WriteAllText(dosyaYolu, jsonKayit);
        
        Console.WriteLine($"{aktifEvren} evreni basariyla kaydedildi. Iyi gunler!");
        break;
    }
}

// --- 4. Veri tutucu sinif (En alta yazilmalidir) ---
public class EvrenVerisi 
{
    public List<Karakter> Karakterler { get; set; } = new List<Karakter>();
    public List<Mekan> Mekanlar { get; set; } = new List<Mekan>();
    public List<Olay> Olaylar { get; set; } = new List<Olay>();
}