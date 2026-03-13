using System;
using System.Linq;
using System.Text.Json;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Roman_evreni_mobil;

public partial class Yapay_zeka_page : ContentPage
{
    private bool _roman_yaz_modu = false;
    private string _son_cevap = "";

    public Yapay_zeka_page()
    {
        InitializeComponent();
    }

    private void On_analiz_mod_clicked(object sender, EventArgs e)
    {
        _roman_yaz_modu = false;
        Analiz_mod_label.TextColor = Color.FromArgb("#f5f5f5");
        Yaz_mod_label.TextColor = Color.FromArgb("#737373");
        Entry_soru.Placeholder = "Yapay zekaya sor...";
    }

    private void On_yaz_mod_clicked(object sender, EventArgs e)
    {
        _roman_yaz_modu = true;
        Yaz_mod_label.TextColor = Color.FromArgb("#f5f5f5");
        Analiz_mod_label.TextColor = Color.FromArgb("#737373");
        Entry_soru.Placeholder = "Hangi sahneyi yazayım?";
    }

    private async void On_sor_clicked(object Sender, EventArgs E)
    {
        string Soru = Entry_soru.Text?.Trim() ?? "";
        if (string.IsNullOrEmpty(Soru)) return;

        Ekrana_mesaj_ekle(Soru, true);
        Entry_soru.Text = string.Empty;

        string Evren_json = "{}";
        if (Aktif_evren.Mevcut != null)
            Evren_json = JsonSerializer.Serialize(Aktif_evren.Mevcut, new JsonSerializerOptions { WriteIndented = true });

        var Bekliyor_etiketi = new Label 
        { 
            Text = _roman_yaz_modu ? "Yazıyor..." : "Okuyor ve analiz ediyor...", 
            TextColor = Color.FromArgb("#525252"), 
            FontSize = 14, FontFamily = "Serif", 
            FontAttributes = FontAttributes.Italic 
        };
        Sohbet_kutusu.Children.Add(Bekliyor_etiketi);

        string Cevap;
        if (_roman_yaz_modu)
        {
            Cevap = await Yapay_zeka_motoru.Roman_yaz(Soru, Evren_json);
        }
        else
        {
            Cevap = await Yapay_zeka_motoru.Tavsiye_al(Soru, Evren_json);
        }

        Sohbet_kutusu.Children.Remove(Bekliyor_etiketi);

        if (!_roman_yaz_modu)
        {
            bool Yeni_kayit_yapildi = false;
            string Gosterilecek_cevap = Cevap;

            var Satirlar = Cevap.Split('\n');
            foreach (var Satir in Satirlar)
            {
                if (Satir.Contains("[Karakter_ekle:"))
                {
                    try {
                        var Icerik = Satir.Split("Karakter_ekle:")[1].Replace("]", "").Split('|');
                        var Yeni_k = new Karakter_bilgisi {
                            Ad = Icerik[0].Trim(),
                            Dis_gorunus = Icerik.Length > 1 ? Icerik[1].Trim() : "",
                            Hikaye = Icerik.Length > 2 ? Icerik[2].Trim() : ""
                        };
                        Aktif_evren.Mevcut?.Karakterler.Add(Yeni_k);
                        Yeni_kayit_yapildi = true;
                        Gosterilecek_cevap = Gosterilecek_cevap.Replace(Satir, $"✓ {Yeni_k.Ad} karakteri listeye eklendi.");
                    } catch {}
                }
                else if (Satir.Contains("[Mekan_ekle:"))
                {
                    try {
                        var Icerik = Satir.Split("Mekan_ekle:")[1].Replace("]", "").Split('|');
                        var Yeni_m = new Mekan_bilgisi {
                            Ad = Icerik[0].Trim(),
                            Iklim = Icerik.Length > 1 ? Icerik[1].Trim() : "",
                            Hukumdar = Icerik.Length > 2 ? Icerik[2].Trim() : "",
                            Aciklama = Icerik.Length > 3 ? Icerik[3].Trim() : ""
                        };
                        Aktif_evren.Mevcut?.Mekanlar.Add(Yeni_m);
                        Yeni_kayit_yapildi = true;
                        Gosterilecek_cevap = Gosterilecek_cevap.Replace(Satir, $"✓ {Yeni_m.Ad} mekanı listeye eklendi.");
                    } catch {}
                }
                else if (Satir.Contains("[Olay_ekle:"))
                {
                    try {
                        var Icerik = Satir.Split("Olay_ekle:")[1].Replace("]", "").Split('|');
                        var Yeni_o = new Olay_bilgisi {
                            Ad = Icerik[0].Trim(),
                            Aciklama = Icerik.Length > 3 ? Icerik[3].Trim() : ""
                        };
                        Aktif_evren.Mevcut?.Olaylar.Add(Yeni_o);
                        Yeni_kayit_yapildi = true;
                        Gosterilecek_cevap = Gosterilecek_cevap.Replace(Satir, $"✓ {Yeni_o.Ad} olayı listeye eklendi.");
                    } catch {}
                }
            }

            if (Yeni_kayit_yapildi)
            {
                var Tum_evrenler = Json_motoru.Yukle();
                var Guncel_evren = Tum_evrenler.FirstOrDefault(X => X.Evren_adi == Aktif_evren.Mevcut?.Evren_adi);
                if (Guncel_evren != null && Aktif_evren.Mevcut != null) {
                    Guncel_evren.Karakterler = Aktif_evren.Mevcut.Karakterler;
                    Guncel_evren.Mekanlar = Aktif_evren.Mevcut.Mekanlar;
                    Guncel_evren.Olaylar = Aktif_evren.Mevcut.Olaylar;
                    Json_motoru.Kaydet(Tum_evrenler);
                }
            }

            Ekrana_mesaj_ekle(Gosterilecek_cevap, false);
        }
        else
        {
            _son_cevap = Cevap;
            Ekrana_mesaj_ekle_kaydet_butonuyla(Cevap);
        }
    }

    private void Ekrana_mesaj_ekle(string Mesaj, bool Kullanici_mi)
    {
        var Etiket = new Label
        {
            Text = Mesaj,
            TextColor = Kullanici_mi ? Color.FromArgb("#ffffff") : Color.FromArgb("#e0e0e0"),
            FontSize = 16,
            FontFamily = "Serif",
            Margin = new Thickness(Kullanici_mi ? 40 : 0, 0, Kullanici_mi ? 0 : 40, 10),
            HorizontalOptions = Kullanici_mi ? LayoutOptions.End : LayoutOptions.Start
        };
        Sohbet_kutusu.Children.Add(Etiket);
    }

    private void Ekrana_mesaj_ekle_kaydet_butonuyla(string Metin)
    {
        var kutu = new VerticalStackLayout { Spacing = 10, Margin = new Thickness(0, 0, 40, 10) };

        var etiket = new Label
        {
            Text = Metin,
            TextColor = Color.FromArgb("#e0e0e0"),
            FontSize = 16,
            FontFamily = "Serif"
        };

        var buton_satiri = new HorizontalStackLayout { Spacing = 10 };

        var roman_kaydet = new Border
        {
            BackgroundColor = Color.FromArgb("#1a1a1a"),
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 15 },
            Padding = new Thickness(12, 6),
            Content = new Label { Text = "→ Romana ekle", TextColor = Color.FromArgb("#f5f5f5"), FontSize = 13 }
        };
        var tap_roman = new TapGestureRecognizer();
        tap_roman.Tapped += async (s, e) => await Romana_kaydet(Metin);
        roman_kaydet.GestureRecognizers.Add(tap_roman);

        var not_kaydet = new Border
        {
            BackgroundColor = Color.FromArgb("#1a1a1a"),
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 15 },
            Padding = new Thickness(12, 6),
            Content = new Label { Text = "→ Nota ekle", TextColor = Color.FromArgb("#f5f5f5"), FontSize = 13 }
        };
        var tap_not = new TapGestureRecognizer();
        tap_not.Tapped += async (s, e) => await Nota_kaydet(Metin);
        not_kaydet.GestureRecognizers.Add(tap_not);

        buton_satiri.Children.Add(roman_kaydet);
        buton_satiri.Children.Add(not_kaydet);
        kutu.Children.Add(etiket);
        kutu.Children.Add(buton_satiri);
        Sohbet_kutusu.Children.Add(kutu);
    }

    private async Task Romana_kaydet(string metin)
    {
        if (Aktif_evren.Mevcut == null || Aktif_evren.Mevcut.Romanlar.Count == 0)
        {
            await DisplayAlert("Uyarı", "Önce bir roman oluştur.", "Tamam");
            return;
        }

        var roman_adlari = Aktif_evren.Mevcut.Romanlar.Select(r => r.Ad).ToArray();
        string secilen_roman = await DisplayActionSheet("Hangi romana eklensin?", "İptal", null, roman_adlari);
        if (secilen_roman == null || secilen_roman == "İptal") return;

        var roman = Aktif_evren.Mevcut.Romanlar.FirstOrDefault(r => r.Ad == secilen_roman);
        if (roman == null) return;

        string yeni_bolum_adi = await DisplayPromptAsync("Bölüm adı", "Bu metin hangi bölüme eklensin?", "Ekle", "İptal", "Yeni bölüm...");
        if (string.IsNullOrWhiteSpace(yeni_bolum_adi)) return;

        var yeni_bolum = new Bolum { Baslik = yeni_bolum_adi, Icerik = metin };
        roman.Bolumler.Add(yeni_bolum);

        var tum_evrenler = Json_motoru.Yukle();
        var guncel = tum_evrenler.FirstOrDefault(x => x.Evren_adi == Aktif_evren.Mevcut?.Evren_adi);
        if (guncel != null && Aktif_evren.Mevcut != null)
        {
            guncel.Romanlar = Aktif_evren.Mevcut.Romanlar;
            Json_motoru.Kaydet(tum_evrenler);
        }

        await DisplayAlert("", $"✓ '{yeni_bolum_adi}' bölümü olarak kaydedildi.", "Tamam");
    }

    private async Task Nota_kaydet(string metin)
    {
        string not_adi = await DisplayPromptAsync("Not başlığı", "Bu metin hangi başlıkla kaydedilsin?", "Kaydet", "İptal", "Yeni not...");
        if (string.IsNullOrWhiteSpace(not_adi)) return;

        var yeni_not = new Not_kaydi { Baslik = not_adi, Icerik = metin };
        Aktif_evren.Mevcut?.Notlar.Add(yeni_not);

        var tum_evrenler = Json_motoru.Yukle();
        var guncel = tum_evrenler.FirstOrDefault(x => x.Evren_adi == Aktif_evren.Mevcut?.Evren_adi);
        if (guncel != null && Aktif_evren.Mevcut != null)
        {
            guncel.Notlar = Aktif_evren.Mevcut.Notlar;
            Json_motoru.Kaydet(tum_evrenler);
        }

        await DisplayAlert("", $"✓ '{not_adi}' olarak notlara kaydedildi.", "Tamam");
    }

    private async void On_geri_clicked(object Sender, EventArgs E)
    {
        await Navigation.PopModalAsync();
    }
}