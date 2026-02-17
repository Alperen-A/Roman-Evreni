using Roman_Evreni;
using System.IO;

// --- 1. LİSTELER VE YÜKLEME ---
List<Karakter> Karakterler = new List<Karakter>();
List<Mekan> Mekanlar = new List<Mekan>();
List<Olay> Olaylar = new List<Olay>();

// Program açılırken dosyayı oku (Geçmişi Hatırla)
if (File.Exists("Evren_Kayitlari.txt"))
{
    Console.WriteLine("--- GEÇMİŞ KAYITLAR YÜKLENİYOR ---");
    // Dosya okuma mantığını buraya geliştirerek ekleyebiliriz
}

while (true) 
{
    // --- 2. DASHBOARD (İSTATİSTİKLER) ---
    Console.Clear(); 
    Console.WriteLine("=================================================");
    Console.WriteLine($"   ROMAN EVRENİ | K: {Karakterler.Count} | M: {Mekanlar.Count} | O: {Olaylar.Count}   ");
    Console.WriteLine("=================================================");
    Console.WriteLine("1. Karakter Yönetimi (Ekle/Sil/Filtrele)");
    Console.WriteLine("2. Mekan Yönetimi (Yarın)");
    Console.WriteLine("3. Olay Yönetimi (Yarın)");
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

            if (kSecim == "G") break; // Ana menüye fırlatır

            if (kSecim == "A") {
                Console.Write("İsim: "); string n = Console.ReadLine() ?? "İsimsiz";
                if(n.ToLower() == "iptal") continue;
                Console.Write("Rol: "); string r = Console.ReadLine() ?? "Köylü";
                Karakterler.Add(new Karakter(n, r, "Hikaye yok."));
            }
            else if (kSecim == "L") {
                foreach (var k in Karakterler) k.Bilgiyazdir();
                Console.WriteLine("Devam için Enter..."); Console.ReadLine();
            }
            else if (kSecim == "S") {
                Console.Write("Silinecek İsim: "); string s = Console.ReadLine() ?? "";
                var hedef = Karakterler.Find(x => x.Ad.ToLower() == s.ToLower());
                if (hedef != null) { Karakterler.Remove(hedef); Console.WriteLine("Silindi!"); }
            }
            else if (kSecim == "F") { // FİLTRELEME
                Console.Write("Aranan Rol: "); string aranan = Console.ReadLine()?.ToLower() ?? "";
                var bulunanlar = Karakterler.FindAll(x => x.Rol.ToLower().Contains(aranan));
                foreach (var b in bulunanlar) b.Bilgiyazdir();
                Console.ReadLine();
            }
        }
    }
    else if (anaSecim == "4") // KAYDET VE ÇIK
    {
        List<string> satirlar = new List<string>();
        foreach (var k in Karakterler) satirlar.Add($"Karakter: {k.Ad} ({k.Rol})");
        File.AppendAllLines("Evren_Kayitlari.txt", satirlar);
        Console.WriteLine("Kaydedildi. Güle güle!");
        break;
    }
}