# Roman Evreni Mobil – Yapılacaklar Listesi

Bu dosya, kod incelemesi sonrası belirlenen iyileştirmeleri ve eklenecek özellikleri takip etmek içindir.  
Tarih: 16 Mart 2025 · Yarın buradan devam edilecek.

---

# Yapılış sırası (önce şu, sonra şu)

Aşağıdaki sırayla ilerle. Her madde bitince `[x]` işaretle.

| Sıra | Ne yapılacak | Neden bu sıra |
|------|----------------|----------------|
| **1** | Eksik arka plan görselleri (A.7) | Hızlı; uygulama çökmesini önler. |
| **2** | Silme onayı tutarlılığı (A.8) | Hızlı; yanlışlıkla silmeyi önler. |
| **3** | Karakter ve Mekan detayda "Geri" ile kaydet (A.2) | Veri kaybını önler. |
| **4** | Arama sonucunda doğrudan detay (A.3) | UX düzeltmesi. |
| **5** | API anahtarını güvenli hale getir (A.1) | Güvenlik; push öncesi yapılmalı. |
| **6** | Favoriler sayfası (B.4) | Mevcut `Favori` alanı kullanılır; basit. |
| **7** | Evren kopyalama (B.14) | Giriş sayfasına tek özellik; basit. |
| **8** | Yedekleme / dışa aktarma (B.6) | Veriyi dışarı alıp geri yükle; çöp kutusundan önce iyi. |
| **9** | Çöp kutusu (B.17) | Veri modeli değişir; tüm “silme” davranışına dokunur. Yedekleme sonrası yap. |
| **10** | Son açılanlar – hızlı erişim (B.16) | Küçük veri (son 5–10 öğe); Mainpage’e blok ekle. |
| **11** | İstatistikler sayfası (B.9) | Sadece mevcut veriden hesaplama. |
| **12** | Not / bölüm içinde metin arama (B.15) | Mevcut aramayı genişlet. |
| **13** | Bölüm sıralama – sürükle bırak (B.13) | Roman detay sayfasında drag & drop. |
| **14** | Okuma modu (B.8) | Roman bölümü için sadece okuma ekranı. |
| **15** | Notlar 2. aşama (B.5) | Görsel, paylaş, etiket; not modeli genişler. |
| **16** | Karakter / mekan ilişki haritası (B.7) | Görsel ağır; sade ve zoom’lu başla. |
| **17** | Tema sistemi (B.10) | Karanlık/krem/lacivert + arka plan; dosya boyutu/izin dikkat. |
| **18** | Uygulama ikonu (B.11) | Tasarım: defter + kılıç. |
| **19** | Android / iOS hazırlık (B.12) | İzinler, splash, ad, versiyon, store metinleri. |
| **20** | İsteğe bağlı: Refaktör (A.5), isimlendirme (A.6), Shell (A.4), Not araç çubuğu (A.10), AI prompt uyumu (A.9) | Zaman kalırsa. |

**Kısa özet:** Önce 1–5 (düzeltmeler + güvenlik), sonra 6–7 (basit özellikler), 8–9 (veri: yedekleme + çöp kutusu), 10–14 (liste/roman/okuma), 15–17 (notlar gelişmiş + harita + tema), 18–19 (ikon + store), 20 (isteğe bağlı).

---

# Bölüm A: İyileştirmeler (detay)

## 1. API anahtarını güvenli hale getir **(kesinlikle yapılacak)**
- **Dosya:** `Yapay_zeka_motoru.cs`
- **Sorun:** Gemini API anahtarı kod içinde düz metin; repo'ya çıkarsa güvenlik riski. **API anahtarı asla açıkta kalmamalı.**
- **Öneri:** Ortam değişkeni veya MAUI'nin güvenli yapılandırması (örn. User Secrets / appsettings) kullan.

---

## 2. Karakter ve Mekan detayda "Geri" ile kaydet
- **Dosyalar:** `Karakter_detay_page.xaml.cs`, `Mekan_detay_page.xaml.cs`
- **Sorun:** Sadece "Kaydet"e basınca kaydediliyor; "← Geri" ile çıkınca değişiklikler kaybolabiliyor.
- **Öneri:** Geri butonuna basıldığında da kaydet (Bolum_detay ve Not_detay gibi) veya "Kaydedilmemiş değişiklik var" uyarısı göster.

---

## 3. Arama sonucunda doğrudan detay sayfasına git
- **Dosya:** `MainPage.xaml.cs` – `Sonuc_ekle` ve arama sonuçları tıklanınca açılan sayfalar.
- **Sorun:** Önce liste sayfası, sonra detay açılıyor; liste kısa süre görünüp kapanıyor.
- **Öneri:** Arama sonucuna tıklanınca sadece ilgili detay sayfasını aç (liste sayfasını modal olarak açmadan).

---

## 4. Shell kullanımı (isteğe bağlı)
- **Dosya:** `AppShell.xaml` / `App.xaml.cs`
- **Not:** Şu an tek sayfa + modal yapı işini görüyor. İleride tab bar veya flyout menü eklenirse Shell'i genişletmek mantıklı. Acil değil.

---

## 5. Liste sayfalarını sadeleştir (refaktör)
- **Dosyalar:** Karakterlerpage, Mekanlarpage, Olaylarpage, Romanlarpage, Notlarpage (xaml + code-behind)
- **Sorun:** Liste gösterme / ekleme / silme / kaydetme mantığı her sayfada tekrarlanıyor.
- **Öneri:** Ortak bir base sınıf veya yardımcı metodlarla tekrarları azalt.

---

## 6. İsimlendirme tutarlılığı
- **Örnekler:** `Appshell` → `AppShell`, `Mainpage` → `MainPage` (PascalCase)
- **Not:** Tüm projede sınıf/adları PascalCase'e çekmek için dikkatli bir replace gerekir; büyük refaktörde yapılabilir.

---

## 7. Eksik arka plan görselleri
- **Referans verilen:** `karakter_detay_bg.jpg`, `mekan_detay_bg.jpg`, `olay_detay_bg.jpg`
- **Kontrol:** `Resources/Images` klasöründe bu dosyalar var mı bak; yoksa varsayılan bir görsel ekle veya referansı kaldır.

---

## 8. Silme onayı tutarlılığı (küçük)
- **Dosya:** `Olaylarpage.xaml.cs` (ve varsa benzeri)
- **Sorun:** Karakter, Roman, Not silerken "Silinsin mi?" onayı var; Olay silerken yok. Kullanıcı yanlışlıkla silebilir.
- **Öneri:** Tüm liste sayfalarında silme öncesi `DisplayAlert` ile onay iste (veya hepsini kaldır; tutarlı olsun).

---

## 9. Yapay zeka prompt / model uyumu (küçük)
- **Dosyalar:** `Yapay_zeka_motoru.cs`, `Yapay_zeka_page.xaml.cs`
- **Sorun:** Prompt'ta Mekan için "Tarihi" yazıyor, modelde alan `Aciklama`. Olay_ekle formatı da satır sırasına göre parse ediliyor; AI farklı sıra verirse kırılabilir.
- **Öneri:** Prompt metnini modele göre güncelle; parse ederken daha toleranslı ol (veya net alan adları kullan).

---

## 10. Not araç çubuğu placeholder (UX)
- **Dosya:** `Not_detay_page.xaml.cs`
- **Sorun:** Kalın/italik vb. butonlara basınca metne "buraya yaz" ekleniyor; kullanıcı silip kendi metnini yazmak zorunda.
- **Öneri:** Seçili metni sarmalayacak şekilde değiştir (seçili yoksa imleç yerine "buraya yaz" kalsın) veya kısa bir açıklama göster.

---

**Sıra:** Yukarıdaki **Yapılış sırası** tablosuna bak; Bölüm A maddeleri orada 1–5 ve 20 içinde. Tamamlanan maddeleri `[x]` ile işaretle.

---

# Bölüm B: Eklenecek özellikler (yol haritası)

Detaylar netleştirildi; yarın buradan devam edilecek.

---

## 4. Favoriler sayfası
- Mainpage'e **"Favoriler"** butonu ekle.
- Ayrı bir sayfa açılsın; yıldızlanmış **karakterler, mekanlar, olaylar ve notlar** listelensin.
- Öğeye tıklanınca **direkt ilgili detay sayfasına** git (liste sayfası açılmadan).

---

## 5. Notlar 2. aşama
- **Görüntü ekle:** Galeriden fotoğraf seçip nota göm.
- **Paylaş:** Notun içeriğini WhatsApp, kopyala vb. ile paylaş.
- **Etiket sistemi:** Notlara tag ekle; sonra tag'e göre filtrele.

---

## 6. Yedekleme / dışa aktarma
- Tüm evren verisini **JSON** olarak dışa aktar.
- Başka cihaza aktar veya **yedekten geri yükle**.
- **Dikkat:** Çok büyük evrende tek JSON + her kayıtta tüm listeyi yazmak ileride yavaşlatabilir; şimdilik mevcut yapı yeterli, ileride sayfalama/ayrı dosya düşünülebilir.

---

## 7. Karakter / mekan ilişki haritası
- Karakterlerin ve mekanların birbirleriyle **bağlantısını görsel** olarak göster.
- Hangi karakter hangi olayda, hangi mekanda vb.
- **Kesinlikle:** Sade ve **zoom'lu** bir görünümle başla; çok düğüm olunca ekran karışmasın, 3D/aşırı animasyon şart değil.

---

## 8. Okuma modu
- Roman bölümlerini **düzenlemeden sadece okumak** için temiz bir görünüm.

---

## 9. İstatistikler sayfası
- Toplam **kelime sayısı**.
- Karakter / mekan / olay **sayıları**.
- **En çok düzenlenen bölüm** vb.

---

## 10. Tema sistemi
- **Karanlık, krem, lacivert** gibi tema seçenekleri.
- **Arka plan fotoğrafı** değiştirme.
- **Kesinlikle dikkat:** Kullanıcı görsel seçecekse **dosya boyutu** ve **izinlere** dikkat et; çok büyük görsel bellek tüketir.

---

## 11. Uygulama ikonu
- Tasarım: **Not defteri + arkasından kılıç geçmiş**.

---

## 12. Android / iOS hazırlık
- İzinler, **splash screen**, uygulama adı, **versiyon** ayarları.
- (Store için: gizlilik metni, ekran görüntüleri, açıklama metinleri.)

---

## 13. Bölüm sıralama (sürükle-bırak) **(kesinlikle)**
- Roman bölümlerini **sürükleyip bırakarak** sıra değiştirme.
- Roman detay sayfasında bölüm listesi için drag & drop.

---

## 14. Evren kopyalama **(kesinlikle)**
- Bir evreni **"Kopyala"** ile çoğaltıp üzerinde oynama (yeni evren adı ile).
- Giriş sayfasında evren satırında kopyala seçeneği.

---

## 15. Not / bölüm içinde metin arama **(kesinlikle)**
- **Not içinde** ve **bölüm içinde** metin arama (şu an sadece isim/başlık aranıyor).
- Ana arama veya ayrı "içerikte ara" alanı.

---

## 16. Son açılanlar – hızlı erişim **(kesinlikle)**
- "Son baktığın" karakter, mekan, bölüm, not (örn. son 5–10 öğe).
- Mainpage veya ayrı bir blokta hızlı erişim.

---

## 17. Çöp kutusu **(kesinlikle)**
- Silinen öğeler hemen kalıcı silinmesin; **çöp kutusu**na gitsin.
- Çöp kutusu sayfası: listele, geri al (geri getir), kalıcı sil.
- Veri modeline: silinen öğelerin tutulduğu alan + silinme tarihi (örn. 30 gün sonra otomatik temizleme isteğe bağlı).

---

**Sıra:** En üstteki **Yapılış sırası** tablosunu takip et (1 → 2 → … → 20).

---

# Bölüm C: İsteğe bağlı ek fikirler

- **Widget / kısayol:** Ana ekranda "Son not" veya "Yazmaya devam et" (platform izinleri gerekir).

(Bölüm sıralama, evren kopyalama, içerik arama, son açılanlar, çöp kutusu artık Bölüm B'de kesinlikle yapılacaklar listesinde.)

---

# Bölüm D: Dikkat (yaparken aklında tut)

- **API anahtarı:** Asla commit'te, ekran görüntüsünde veya istemci tarafında açık bırakma; güvenli yapılandırma → Bölüm A.1 kesinlikle yapılacak.
- **Çok büyük evren:** Not B.6'da; ileride performans gerekirse sayfalama/ayrı dosya düşün.
- **İlişki haritası:** Not B.7'de; sade ve zoom'lu başla.

- **Tema / arka plan:** Not B.10'da; kullanıcı görsel seçecekse dosya boyutu ve izinler.