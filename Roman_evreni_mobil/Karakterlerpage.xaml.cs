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
            Ekrana_karakter_ekle(yeni_ad);
            Aktif_evren.Mevcut.Karakterler.Add(yeni_ad);

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

    private void Ekrana_karakter_ekle(string ad)
    {
        var yatay_kutu = new HorizontalStackLayout { Spacing = 15, Margin = new Thickness(0, 5) };

        var isim_etiketi = new Label
        {
            Text = "🗡️ " + ad,
            TextColor = Colors.White,
            FontSize = 18,
            VerticalOptions = LayoutOptions.Center,
            WidthRequest = 200
        };

        var sil_butonu = new Button
        {
            Text = "Sil",
            BackgroundColor = Colors.Red,
            TextColor = Colors.White,
            WidthRequest = 50,
            HeightRequest = 40,
            CornerRadius = 20
        };

        sil_butonu.Clicked += (s, e) =>
        {
            Aktif_evren.Mevcut?.Karakterler.Remove(ad);

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
        yatay_kutu.Children.Add(sil_butonu);
        KarakterListesi.Children.Add(yatay_kutu);
    }

    private async void OnGeriClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}