using System;
using System.Collections.Generic;
using System.IO;
using Roman_Evreni;

// --- 1. Listeler ---
List<Karakter> Karakterler = new List<Karakter>();
List<Mekan> Mekanlar = new List<Mekan>();
List<Olay> Olaylar = new List<Olay>();

while (true) 
{
    // --- 2. Dashboard ---
    Console.Clear(); 
    Console.WriteLine("-------------------------------------------------");
    Console.WriteLine($"  Roman evreni | K: {Karakterler.Count} | M: {Mekanlar.Count} | O: {Olaylar.Count}   ");
    Console.WriteLine("-------------------------------------------------");
    Console.WriteLine("1. Karakter yönetimi (Görünüm ve hikaye odaklı)");
    Console.WriteLine("2. Mekan yönetimi");
    Console.WriteLine("3. Olay yönetimi");
    Console.WriteLine("4. Kaydet ve çıkış");
    Console.Write("\nBölüm seçiniz: ");
    
    string mainSecim = Console.ReadLine() ?? "";

    if (mainSecim == "1") // --- Karakter odasi ---
    {
        while (true)
        {
            Console.WriteLine("\n-- Karakter menüsü --");
            Console.WriteLine("[A] Ekle | [S] Sil | [L] Listele | [F] Hikayede ara | [G] Geri");
            Console.Write("Seçim: ");
            string kSecim = Console.ReadLine() ?? "";
            
            if (kSecim.ToLower() == "g") break;

            if (kSecim.ToLower() == "a")
            {
                Console.Write("Karakter adı (Veya iptal): "); 
                string ad = Console.ReadLine() ?? "İsimsiz";
                if(ad.ToLower() == "iptal") continue;

                Console.Write("Fiziksel görünüm (Boy, saç, göz, giysi vb.): "); 
                string gorunum = Console.ReadLine() ?? "Belirtilmedi";

                Console.WriteLine("Arka plan / Hikaye (İstediğin kadar uzun yazıp enter tuşuna bas): ");
                string hikaye = Console.ReadLine() ?? "Hikaye girilmedi.";

                Karakterler.Add(new Karakter(ad, gorunum, hikaye));
                Console.WriteLine("--> Karakter detaylarıyla kaydedildi!");
            }
            else if (kSecim.ToLower() == "l")
            {
                Console.WriteLine("\n--- Karakter detayları ---");
                foreach (var k in Karakterler) k.Bilgiyazdir();
                Console.WriteLine("Devam için enter tuşuna bas..."); Console.ReadLine();
            }
            else if (kSecim.ToLower() == "s")
            {
                Console.Write("Silinecek isim: "); string s = Console.ReadLine() ?? "";
                var h = Karakterler.Find(x => x.Ad.ToLower() == s.ToLower());
                if (h != null) { Karakterler.Remove(h); Console.WriteLine("Silindi!"); }
                else Console.WriteLine("Bulunamadı.");
            }
            else if (kSecim.ToLower() == "f")
            {
                Console.Write("Hikayede veya görünümde geçen kelime: "); 
                string f = Console.ReadLine()?.ToLower() ?? "";
                
                var sonuclar = Karakterler.FindAll(x => x.Hikaye.ToLower().Contains(f) || x.Gorunum.ToLower().Contains(f));
                
                Console.WriteLine($"\n--- '{f}' Içeren karakterler ---");
                foreach (var item in sonuclar) item.Bilgiyazdir();
                Console.ReadLine();
            }
        }
    }
    else if (mainSecim == "2") // --- Mekan odasi ---
    {
        while (true)
        {
            Console.WriteLine("\n-- Mekan menüsü --");
            Console.WriteLine("[A] Ekle | [S] Sil | [L] Listele | [G] Geri");
            Console.Write("Seçim: ");
            string mSecim = Console.ReadLine() ?? "";
            
            if (mSecim.ToLower() == "g") break;

            if (mSecim.ToLower() == "a") {
                Console.Write("Mekan adı: "); string n = Console.ReadLine() ?? "İsimsiz";
                Console.Write("Betimleme (Atmosfer, koku, gizli geçitler vb.): "); string b = Console.ReadLine() ?? "Belirtilmedi";
                Mekanlar.Add(new Mekan(n, b));
                Console.WriteLine("--> Mekan eklendi!");
            }
            else if (mSecim.ToLower() == "l") {
                Console.WriteLine("\n--- Mekan detayları ---");
                foreach (var m in Mekanlar) m.Bilgiyazdir();
                Console.WriteLine("Devam için enter tuşuna bas..."); Console.ReadLine();
            }
            else if (mSecim.ToLower() == "s") {
                Console.Write("Silinecek mekan adı: "); string s = Console.ReadLine() ?? "";
                var h = Mekanlar.Find(x => x.Ad.ToLower() == s.ToLower());
                if (h != null) { Mekanlar.Remove(h); Console.WriteLine("Mekan silindi!"); }
                else Console.WriteLine("Bulunamadı.");
            }
        }
    }
    else if (mainSecim == "3") // --- Olay odasi ---
    {
        while (true)
        {
            Console.WriteLine("\n-- Olay menüsü --");
            Console.WriteLine("[A] Ekle | [S] Sil | [L] Listele | [G] Geri");
            Console.Write("Seçim: ");
            string oSecim = Console.ReadLine() ?? "";
            
            if (oSecim.ToLower() == "g") break;

            if (oSecim.ToLower() == "a") {
                Console.Write("Olay adı: "); string n = Console.ReadLine() ?? "Bilinmiyor";
                Console.Write("İçindeki karakterler (Örn: Alperen, melisa): "); string k = Console.ReadLine() ?? "Belirtilmedi";
                Console.Write("Tarih: "); string t = Console.ReadLine() ?? "Bilinmiyor";
                Console.Write("Betimleme (Sonuçlar, kayıplar, değişen dengeler vb.): "); string b = Console.ReadLine() ?? "Belirtilmedi";
                Olaylar.Add(new Olay(n, k, t, b));
                Console.WriteLine("--> Olay kaydedildi!");
            }
            else if (oSecim.ToLower() == "l") {
                Console.WriteLine("\n--- Olay detayları ---");
                foreach (var o in Olaylar) o.Bilgiyazdir();
                Console.WriteLine("Devam için enter tuşuna bas..."); Console.ReadLine();
            }
            else if (oSecim.ToLower() == "s") {
                Console.Write("Silinecek olay adı: "); string s = Console.ReadLine() ?? "";
                var h = Olaylar.Find(x => x.Ad.ToLower() == s.ToLower());
                if (h != null) { Olaylar.Remove(h); Console.WriteLine("Olay silindi!"); }
                else Console.WriteLine("Bulunamadı.");
            }
        }
    }
    else if (mainSecim == "4") // --- Kaydet ve çikis ---
    {
        List<string> satirlar = new List<string>();
        satirlar.Add($"\n--- Detaylı evren raporu: {DateTime.Now} ---");
        
        satirlar.Add("\n--- Karakterler ---");
        foreach (var k in Karakterler) 
        {
            satirlar.Add($"İsim: {k.Ad}");
            satirlar.Add($"Görünüm: {k.Gorunum}");
            satirlar.Add($"Hikaye: {k.Hikaye}");
            satirlar.Add("-------------------------");
        }

        satirlar.Add("\n--- Mekanlar ---");
        foreach (var m in Mekanlar) 
        {
            satirlar.Add($"Mekan: {m.Ad}");
            satirlar.Add($"Betimleme: {m.Betimleme}");
            satirlar.Add("-------------------------");
        }
        
        satirlar.Add("\n--- Olaylar ---");
        foreach (var o in Olaylar) 
        {
            satirlar.Add($"Olay: {o.Ad} ({o.Tarih})");
            satirlar.Add($"Karakterler: {o.Karakterler}");
            satirlar.Add($"Betimleme: {o.Betimleme}");
            satirlar.Add("-------------------------");
        }
        
        File.AppendAllLines("Evren_kayitlari.txt", satirlar);
        Console.WriteLine("Tüm hikaye ve detaylar kaydedildi. Iyi günler!");
        break;
    }
}