using Roman_Evreni;
using System.IO;

// --- 1. BAĞIMSIZ LİSTELER ---
List<Karakter> Karakterler = new List<Karakter>();
List<Mekan> Mekanlar = new List<Mekan>();
List<Olay> Olaylar = new List<Olay>();

while (true) 
{
    // --- 2. DASHBOARD (İSTATİSTİKLER) ---
    Console.Clear(); 
    Console.WriteLine("=================================================");
    Console.WriteLine($"   ROMAN EVRENİ | K: {Karakterler.Count} | M: {Mekanlar.Count} | O: {Olaylar.Count}   ");
    Console.WriteLine("=================================================");
    Console.WriteLine("1. Karakter Yönetimi");
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
            Console.WriteLine("[A] Ekle | [S] Sil | [L] Listele | [F] Filtrele | [G] Geri");
            Console.Write("Seçim: ");
            string kSecim = Console.ReadLine()?.ToUpper() ?? "";
            if (kSecim == "G") break;

            if (kSecim == "A") {
                Console.Write("İsim (Veya iptal): "); string n = Console.ReadLine() ?? "İsimsiz";
                if(n.ToLower() == "iptal") continue;
                Console.Write("Rol: "); string r = Console.ReadLine() ?? "Köylü";
                Karakterler.Add(new Karakter(n, r, "Hikaye yok."));
                Console.WriteLine("--> Eklendi!");
            }
            else if (kSecim == "L") {
                foreach (var k in Karakterler) k.Bilgiyazdir();
                Console.WriteLine("\nDevam için Enter..."); Console.ReadLine();
            }
            else if (kSecim == "S") {
                Console.Write("Silinecek İsim: "); string s = Console.ReadLine() ?? "";
                var h = Karakterler.Find(x => x.Ad.ToLower() == s.ToLower());
                if (h != null) { Karakterler.Remove(h); Console.WriteLine("Silindi!"); }
                else Console.WriteLine("Bulunamadı.");
            }
            else if (kSecim == "F") {
                Console.Write("Aranan Rol (Örn: Büyücü): "); string f = Console.ReadLine()?.ToLower() ?? "";
                var sonuclar = Karakterler.FindAll(x => x.Rol.ToLower().Contains(f));
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
            Console.WriteLine("[A] Ekle | [S] Sil | [L] Listele | [F] Filtrele | [G] Geri");
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
            else if (mSecim == "F") {
                Console.Write("Aranan Tür (Örn: Kale): "); string f = Console.ReadLine()?.ToLower() ?? "";
                var sonuclar = Mekanlar.FindAll(x => x.Tur.ToLower().Contains(f));
                foreach (var item in sonuclar) item.Bilgiyazdir();
                Console.ReadLine();
            }
        }
    }
    else if (anaSecim == "3") // --- OLAY ODASI ---
    {
        while (true)
        {
            Console.WriteLine("\n-- OLAY MENÜSÜ --");
            Console.WriteLine("[A] Ekle | [S] Sil | [L] Listele | [F] Filtrele | [G] Geri");
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
            else if (oSecim == "F") {
                Console.Write("Aranan Tarih veya İsim: "); string f = Console.ReadLine()?.ToLower() ?? "";
                var sonuclar = Olaylar.FindAll(x => x.Ad.ToLower().Contains(f) || x.Tarih.Contains(f));
                foreach (var item in sonuclar) item.Bilgiyazdir();
                Console.ReadLine();
            }
        }
    }
    else if (anaSecim == "4") // --- KAYDET VE ÇIKIŞ ---
    {
        List<string> satirlar = new List<string>();
        satirlar.Add($"\n--- EVREN GÜNCELLEMESİ: {DateTime.Now} ---");
        
        foreach (var k in Karakterler) satirlar.Add($"Karakter: {k.Ad} ({k.Rol})");
        foreach (var m in Mekanlar) satirlar.Add($"Mekan: {m.Ad} ({m.Tur})");
        foreach (var o in Olaylar) satirlar.Add($"Olay: {o.Ad} ({o.Tarih})");
        
        File.AppendAllLines("Evren_Kayitlari.txt", satirlar);
        Console.WriteLine("Her şey kaydedildi. Roman evreninden çıkılıyor...");
        break;
    }
}