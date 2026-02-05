using Roman_Evreni;
using System.IO;  // Dosya işlemleri için gerekli kütüphane

// --- GEÇMİŞİ YÜKLE (LOAD GAME) ---
Console.WriteLine("--- GEÇMİŞ KAYITLAR KONTROL EDİLİYOR ---");

if (File.Exists("Evren_Kayitlari.txt"))
{
    string[] eskiSatirlar = File.ReadAllLines("Evren_Kayitlari.txt");
    foreach (string satir in eskiSatirlar)
    {
        Console.WriteLine(satir);
    }
    Console.WriteLine("--- GEÇMİŞ BAŞARIYLA YÜKLENDİ ---\n");
}
else
{
    Console.WriteLine("Henüz kaydedilmiş bir evren yok.\n");
}

// --- Karakter Listesi ---
// Yeni bir karakter listesi oluşturuyoruz
List<Karakter> Karakterler = new List<Karakter>();

// Listeye karakter ekleme
Karakterler.Add(new Karakter("Alperen", "Savaşçı", 21, "Kuzeyden gelen kod yazarı."));
Karakterler.Add(new Karakter("Melisa", "Büyücü", 20, "Kadim dillerin uzmanı."));
Karakterler.Add(new Karakter("Gölge", "Casus", 35, "Kraliyet sarayının gizli kulagi."));

// --- Mekan Listesi ---
List<Mekan> Mekanlar = new List<Mekan>();

// Listeye mekan ekleme
Mekanlar.Add(new Mekan("Karanlık Orman", "Orman", "Efsaneye göre giren geri dönemez."));
Mekanlar.Add(new Mekan("Ejderha Kalesi", "Kale", "300 yıldır yıkılmayan surlar."));

// --- Ekrana Yazdırma ---
Console.WriteLine("=== ROMAN EVRENİ RAPORU ===\n");

Console.WriteLine("--- KARAKTERLER ---");
// Döngü ile listedeki herkesi tek tek yazdır
foreach (var K in Karakterler)
{
    K.Bilgiyazdir();
}

Console.WriteLine("\n--- MEKANLAR ---");
foreach (var M in Mekanlar)
{
    M.Bilgiyazdir();
}

// --- OLAYLAR ---
Olay savas = new Olay("Büyük Savaş", "1250", "Kuzey Krallığı kazandı.");
savas.Bilgiyazdir();


// --- DOSYAYA KAYIT ---
Console.WriteLine("\n--- KAYIT İŞLEMİ ---");

// Kaydedilecek satırları hazırlayalım
List<string> satirlar = new List<string>();
satirlar.Add($"Rapor Tarihi: {DateTime.Now}");
satirlar.Add("-----------------------------");

// Karakterleri listeye ekle
foreach (var k in Karakterler)
{
    satirlar.Add($"Karakter: {k.Ad} ({k.Rol})");
}

// Dosyayı oluştur ve yaz
File.WriteAllLines("Evren_Kayitlari.txt", satirlar);
Console.WriteLine("Veriler 'Evren_Kayitlari.txt' dosyasına başarıyla kaydedildi!");

// Program kapanmasın
Console.ReadLine();