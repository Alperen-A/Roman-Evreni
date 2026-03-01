using System;
using System.Linq;
using System.Text.Json;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Roman_evreni_mobil;

public partial class Yapay_zeka_page : ContentPage
{
    public Yapay_zeka_page()
    {
        InitializeComponent();
    }

    private async void On_sor_clicked(object Sender, EventArgs E)
    {
        string Soru = Entry_soru.Text?.Trim() ?? "";
        if (string.IsNullOrEmpty(Soru)) return;

        Ekrana_mesaj_ekle(Soru, true);
        Entry_soru.Text = string.Empty;

        string Evren_json = "{}";
        if (Aktif_evren.Mevcut != null)
        {
            Evren_json = JsonSerializer.Serialize(Aktif_evren.Mevcut, new JsonSerializerOptions { WriteIndented = true });
        }

        var Bekliyor_etiketi = new Label { Text = "Okuyor ve analiz ediyor...", TextColor = Color.FromArgb("#525252"), FontSize = 14, FontFamily = "Serif", FontAttributes = FontAttributes.Italic, Margin = new Thickness(0, 0, 0, 10) };
        Sohbet_kutusu.Children.Add(Bekliyor_etiketi);

        string Cevap = await Yapay_zeka_motoru.Tavsiye_al(Soru, Evren_json);
        Sohbet_kutusu.Children.Remove(Bekliyor_etiketi);

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
                        Hikaye = Icerik.Length > 3 ? Icerik[3].Trim() : ""
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
                    Gosterilecek_cevap = Gosterilecek_cevap.Replace(Satir, $"✓ {Yeni_m.Ad} mekani listeye eklendi.");
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
                Json_motoru.Kaydet(Tum_evrenler);
            }
        }

        Ekrana_mesaj_ekle(Gosterilecek_cevap, false);
    }

    private void Ekrana_mesaj_ekle(string Mesaj, bool Kullanici_mi)
    {
        var Etiket = new Label
        {
            Text = Mesaj,
            TextColor = Kullanici_mi ? Color.FromArgb("#d4d4d4") : Color.FromArgb("#a3a3a3"),
            FontSize = 16,
            FontFamily = "Serif",
            Margin = new Thickness(Kullanici_mi ? 40 : 0, 0, Kullanici_mi ? 0 : 40, 10),
            HorizontalOptions = Kullanici_mi ? LayoutOptions.End : LayoutOptions.Start
        };
        Sohbet_kutusu.Children.Add(Etiket);
    }

    private async void On_geri_clicked(object Sender, EventArgs E)
    {
        await Navigation.PopModalAsync();
    }
}