using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Roman_evreni_mobil;

public partial class Karakterlerpage : ContentPage
{
    public Karakterlerpage()
    {
        InitializeComponent();
        Karakterleri_yukle();
    }

    private void Karakterleri_yukle()
    {
        KarakterListesi.Children.Clear();

        if (Aktif_evren.Mevcut != null && Aktif_evren.Mevcut.Karakterler != null)
        {
            foreach (var k in Aktif_evren.Mevcut.Karakterler)
            {
                Ekrana_karakter_ekle(k);
            }
        }
    }

    private void OnKarakterEkleClicked(object sender, EventArgs e)
    {
        string? yeni_ad = EntryKarakterAdi.Text?.Trim();

        if (!string.IsNullOrWhiteSpace(yeni_ad) && Aktif_evren.Mevcut != null)
        {
            var yeni_karakter = new Karakter_bilgisi { Ad = yeni_ad };

            Ekrana_karakter_ekle(yeni_karakter);
            Aktif_evren.Mevcut.Karakterler.Add(yeni_karakter);

            var tum_evrenler = Json_motoru.Yukle();
            var guncellenecek_evren = tum_evrenler.FirstOrDefault(x => x.Evren_adi == Aktif_evren.Mevcut.Evren_adi);

            if (guncellenecek_evren != null)
            {
                guncellenecek_evren.Karakterler = Aktif_evren.Mevcut.Karakterler;
                Json_motoru.Kaydet(tum_evrenler);
            }

            EntryKarakterAdi.Text = string.Empty;
        }
    }

    private void Ekrana_karakter_ekle(Karakter_bilgisi karakter)
    {
        var yatay_kutu = new HorizontalStackLayout { Spacing = 10, Margin = new Thickness(0, 5) };

        var isim_etiketi = new Label
        {
            Text = "🗡️ " + karakter.Ad,
            TextColor = Colors.White,
            FontSize = 16,
            VerticalOptions = LayoutOptions.Center,
            WidthRequest = 140
        };

        // Yeni estetik detay butonu (Koyu mor arkaplan)
        var detay_butonu = new Button
        {
            Text = "Detay",
            BackgroundColor = Color.FromArgb("#2a1b54"), 
            TextColor = Colors.White,
            WidthRequest = 70,
            HeightRequest = 35,
            CornerRadius = 17,
            Padding = 0
        };

        detay_butonu.Clicked += async (s, e) =>
        {
            await Navigation.PushModalAsync(new Karakter_detay_page(karakter));
        };

        // Yeni estetik sil butonu (Arkaplansiz, sadece kirmizi yazi)
        var sil_butonu = new Button
        {
            Text = "Sil",
            BackgroundColor = Colors.Transparent,
            TextColor = Color.FromArgb("#ff4444"), 
            WidthRequest = 50,
            HeightRequest = 35,
            Padding = 0
        };

        sil_butonu.Clicked += (s, e) =>
        {
            Aktif_evren.Mevcut?.Karakterler.Remove(karakter);

            var tum_evrenler = Json_motoru.Yukle();
            var guncel_evren = tum_evrenler.FirstOrDefault(x => x.Evren_adi == Aktif_evren.Mevcut?.Evren_adi);
            
            if (guncel_evren != null && Aktif_evren.Mevcut != null)
            {
                guncel_evren.Karakterler = Aktif_evren.Mevcut.Karakterler;
                Json_motoru.Kaydet(tum_evrenler);
            }

            KarakterListesi.Children.Remove(yatay_kutu);
        };

        yatay_kutu.Children.Add(isim_etiketi);
        yatay_kutu.Children.Add(detay_butonu);
        yatay_kutu.Children.Add(sil_butonu);
        KarakterListesi.Children.Add(yatay_kutu);
    }

    private async void OnGeriClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}