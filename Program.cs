using Roman_Evreni;
using System.IO;

// --- 1. LİSTELER ---
List<Karakter> Karakterler = new List<Karakter>();
List<Mekan> Mekanlar = new List<Mekan>();
List<Olay> Olaylar = new List<Olay>();

while (true) 
{
    // --- 2. DASHBOARD ---
    Console.Clear(); 
    Console.WriteLine("=================================================");
    Console.WriteLine($"   ROMAN EVRENİ | K: {Karakterler.Count} | M: {Mekanlar.Count} | O: {Olaylar.Count}   ");
    Console.WriteLine("=================================================");
    Console.WriteLine("1. Karakter Yönetimi (Görünüm ve Hikaye Odaklı)");
    Console.WriteLine("2. Mekan Yönetimi");
    Console.WriteLine("3. Olay Yönetimi");
    Console.WriteLine("4. Kaydet ve Çıkış");
    Console.Write("\nBölüm Seçiniz: ");
    
    string anaSecim = Console.ReadLine() ?? "";

    if (anaSecim == "1") // --- KARAKTER ODASI ---
    {
        while (true)
        {
            Console.WriteLine("\n-- KARAKTER MENÜSÜ --");
            Console.WriteLine("[A] Ekle | [S] Sil | [L] Listele | [F] Hikayede Ara | [G] Geri");
            Console.Write("Seçim: ");
            string kSecim = Console.ReadLine()?.ToUpper() ?? "";

            if (kSecim == "G") break;

            if (kSecim == "A") // EKLEME (GÜNCELLENDİ)
            {
                Console.Write("Karakter Adı (Veya iptal): "); 
                string ad = Console.ReadLine() ?? "İsimsiz";
                if(ad.ToLower() == "iptal") continue;

                // Rol yerine Fiziksel Görünüm soruyoruz
                Console.Write("Fiziksel Görünüm (Boy, saç, göz, giysi vb.): "); 
                string gorunum = Console.ReadLine() ?? "Belirtilmedi";

                // Uzun hikaye girişi
                Console.WriteLine("Arka Plan / Hikaye (İstediğin kadar uzun yazıp Enter'a bas): ");
                string hikaye = Console.ReadLine() ?? "Hikaye girilmedi.";

                Karakterler.Add(new Karakter(ad, gorunum, hikaye));
                Console.WriteLine("--> Karakter detaylarıyla kaydedildi!");
            }
            else if (kSecim == "L") // LİSTELEME
            {
                Console.WriteLine("\n--- KARAKTER DETAYLARI ---");
                foreach (var k in Karakterler) k.Bilgiyazdir();
                Console.WriteLine("Devam için Enter..."); Console.ReadLine();
            }
            else if (kSecim == "S") // SİLME
            {
                Console.Write("Silinecek İsim: "); string s = Console.ReadLine() ?? "";
                var h = Karakterler.Find(x => x.Ad.ToLower() == s.ToLower());
                if (h != null) { Karakterler.Remove(h); Console.WriteLine("Silindi!"); }
                else Console.WriteLine("Bulunamadı.");
            }
            else if (kSecim == "F") // FİLTRELEME (GÜNCELLENDİ)
            {
                // Artık Rol olmadığı için hikaye içinde kelime arıyoruz
                Console.Write("Hikayede veya Görünümde geçen kelime (Örn: 'yara izi', 'büyücü'): "); 
                string f = Console.ReadLine()?.ToLower() ?? "";
                
                var sonuclar = Karakterler.FindAll(x => x.Hikaye.ToLower().Contains(f) || x.Gorunum.ToLower().Contains(f));
                
                Console.WriteLine($"\n--- '{f.ToUpper()}' İÇEREN KARAKTERLER ---");
                foreach (var item in sonuclar) item.Bilgiyazdir();
                Console.ReadLine();
            }
        }
    }
    else if (anaSecim == "2") // --- MEKAN ODASI ---
    {
        while (true)
        {
            Console.WriteLine("\n-- MEKAN MENÜSÜ --");
            Console.WriteLine("[A] Ekle | [S] Sil | [L] Listele | [G] Geri");
            Console.Write("Seçim: ");
            string mSecim = Console.ReadLine()?.ToUpper() ?? "";
            if (mSecim == "G") break;

            if (mSecim == "A") {
                Console.Write("Mekan Adı: "); string n = Console.ReadLine() ?? "İsimsiz";
                Console.Write("Tür (Orman, Kale vb.): "); string t = Console.ReadLine() ?? "Bilinmiyor";
                Mekanlar.Add(new Mekan(n, t, "Detay yok."));
                Console.WriteLine("--> Mekan eklendi!");
            }
            else if (mSecim == "L") {
                foreach (var m in Mekanlar) m.Bilgiyazdir();
                Console.WriteLine("\nDevam için Enter..."); Console.ReadLine();
            }
            else if (mSecim == "S") {
                Console.Write("Silinecek Mekan Adı: "); string s = Console.ReadLine() ?? "";
                var h = Mekanlar.Find(x => x.Ad.ToLower() == s.ToLower());
                if (h != null) { Mekanlar.Remove(h); Console.WriteLine("Mekan silindi!"); }
            }
        }
    }
    else if (anaSecim == "3") // --- OLAY ODASI ---
    {
        while (true)
        {
            Console.WriteLine("\n-- OLAY MENÜSÜ --");
            Console.WriteLine("[A] Ekle | [S] Sil | [L] Listele | [G] Geri");
            Console.Write("Seçim: ");
            string oSecim = Console.ReadLine()?.ToUpper() ?? "";
            if (oSecim == "G") break;

            if (oSecim == "A") {
                Console.Write("Olay Adı: "); string n = Console.ReadLine() ?? "Bilinmiyor";
                Console.Write("Tarih: "); string t = Console.ReadLine() ?? "Bilinmiyor";
                Olaylar.Add(new Olay(n, t, "Sonuç girilmedi."));
                Console.WriteLine("--> Olay kaydedildi!");
            }
            else if (oSecim == "L") {
                foreach (var o in Olaylar) o.Bilgiyazdir();
                Console.WriteLine("\nDevam için Enter..."); Console.ReadLine();
            }
            else if (oSecim == "S") {
                Console.Write("Silinecek Olay Adı: "); string s = Console.ReadLine() ?? "";
                var h = Olaylar.Find(x => x.Ad.ToLower() == s.ToLower());
                if (h != null) { Olaylar.Remove(h); Console.WriteLine("Olay silindi!"); }
            }
        }
    }
    else if (anaSecim == "4") // --- KAYDET VE ÇIKIŞ ---
    {
        List<string> satirlar = new List<string>();
        satirlar.Add($"\n--- DETAYLI EVREN RAPORU: {DateTime.Now} ---");
        
        satirlar.Add("\n--- KARAKTERLER ---");
        foreach (var k in Karakterler) 
        {
            satirlar.Add($"İsim: {k.Ad}");
            satirlar.Add($"Görünüm: {k.Gorunum}");
            satirlar.Add($"Hikaye: {k.Hikaye}");
            satirlar.Add("-------------------------");
        }

        satirlar.Add("\n--- MEKANLAR ---");
        foreach (var m in Mekanlar) satirlar.Add($"Mekan: {m.Ad} ({m.Tur})");
        
        satirlar.Add("\n--- OLAYLAR ---");
        foreach (var o in Olaylar) satirlar.Add($"Olay: {o.Ad} ({o.Tarih})");
        
        File.AppendAllLines("Evren_Kayitlari.txt", satirlar);
        Console.WriteLine("Tüm hikaye ve detaylar kaydedildi. İyi günler!");
        break;
    }
}