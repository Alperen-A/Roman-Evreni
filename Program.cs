using Roman_Evreni;
using System.IO;

// --- 1. GEÇMİŞİ YÜKLE ---
Console.WriteLine("--- GEÇMİŞ KAYITLAR KONTROL EDİLİYOR ---");
if (File.Exists("Evren_Kayitlari.txt"))
{
    string[] eskiSatirlar = File.ReadAllLines("Evren_Kayitlari.txt");
    foreach (string satir in eskiSatirlar) Console.WriteLine(satir);
    Console.WriteLine("--- GEÇMİŞ BAŞARIYLA YÜKLENDİ ---\n");
}
else
{
    Console.WriteLine("Henüz kaydedilmiş bir evren yok.\n");
}

// --- 2. LİSTELERİ OLUŞTUR ---
List<Karakter> Karakterler = new List<Karakter>();
List<Mekan> Mekanlar = new List<Mekan>();
List<Olay> Olaylar = new List<Olay>();

// --- 3. MENÜ SİSTEMİ ---
while (true) 
{
    Console.WriteLine("\n=============================");
    Console.WriteLine("       ROMAN EVRENİ v1.1      ");
    Console.WriteLine("=============================");
    Console.WriteLine("1. Yeni Karakter Ekle");
    Console.WriteLine("2. Listele (Karakter/Mekan/Olay)");
    Console.WriteLine("3. Karakter Sil (YENİ)"); // <-- YENİ ÖZELLİK
    Console.WriteLine("4. Kaydet ve Çıkış");      // <-- NUMARASI DEĞİŞTİ
    Console.Write("Seçiminiz (1-4): ");
    
    string secim = Console.ReadLine() ?? "";

    // --- SEÇENEK 1: EKLEME ---
    if (secim == "1") 
    {
        Console.WriteLine("\n--- YENİ KARAKTER EKLE ---");
        Console.Write("Karakterin Adı: ");
        string isim = Console.ReadLine() ?? ""; 

        if (isim.ToLower() == "iptal") { Console.WriteLine("İptal edildi."); continue; }
        if (string.IsNullOrWhiteSpace(isim)) isim = "İsimsiz";

        Console.Write("Karakterin Rolü: ");
        string rol = Console.ReadLine() ?? "Köylü";

        Karakterler.Add(new Karakter(isim, rol, "Hikaye girilmedi."));
        Console.WriteLine($"--> {isim} listeye eklendi!");
    }
    
    // --- SEÇENEK 2: LİSTELEME ---
    else if (secim == "2") 
    {
        Console.WriteLine("\n=== GÜNCEL EVREN RAPORU ===");
        if (Karakterler.Count == 0) Console.WriteLine("(Karakter listesi boş.)");
        
        foreach (var k in Karakterler) k.Bilgiyazdir();
        // (Mekan ve Olayları da buraya ekleyebilirsin)

        Console.WriteLine("\n(Devam etmek için Enter'a bas...)");
        Console.ReadLine(); 
    }

    // --- SEÇENEK 3: SİLME (YENİ) ---
    else if (secim == "3")
    {
        Console.WriteLine("\n--- KARAKTER SİLME ---");
        Console.Write("Silinecek karakterin adı: ");
        string silinecekIsim = Console.ReadLine() ?? "";

        // Listede bu ismi arayalım (Büyük/küçük harf fark etmesin)
        Karakter? silinecekKarakter = null;

        foreach (var k in Karakterler)
        {
            if (k.Ad.ToLower() == silinecekIsim.ToLower())
            {
                silinecekKarakter = k;
                break; // Bulduk, aramayı bitir.
            }
        }

        // Eğer bulduysak siliyoruz
        if (silinecekKarakter != null)
        {
            Karakterler.Remove(silinecekKarakter);
            Console.WriteLine($"--> {silinecekKarakter.Ad} başarıyla silindi!");
        }
        else
        {
            Console.WriteLine("--> Böyle bir karakter bulunamadı!");
        }
    }
    
    // --- SEÇENEK 4: ÇIKIŞ ---
    else if (secim == "4") 
    {
        Console.WriteLine("Çıkış yapılıyor...");
        break; 
    }
    else
    {
        Console.WriteLine("Hatalı seçim! Lütfen 1-4 arasında bir sayı girin.");
    }
}

// --- 4. DOSYAYA KAYIT ---
Console.WriteLine("\n--- KAYIT İŞLEMİ ---");
List<string> satirlar = new List<string>();

if (Karakterler.Count > 0)
{
    satirlar.Add($"\n--- Rapor Tarihi: {DateTime.Now} ---");
    foreach (var k in Karakterler) satirlar.Add($"Karakter: {k.Ad} ({k.Rol})");
    
    File.AppendAllLines("Evren_Kayitlari.txt", satirlar);
    Console.WriteLine("Veriler kaydedildi!");
}
else
{
    Console.WriteLine("Listede yeni veri yok, kayıt yapılmadı.");
}