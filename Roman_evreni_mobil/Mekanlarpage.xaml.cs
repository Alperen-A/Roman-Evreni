using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Roman_evreni_mobil;

public partial class Mekanlarpage : ContentPage
{
    public Mekanlarpage()
    {
        InitializeComponent();
        Mekanlari_yukle();
    }

    private void Mekanlari_yukle()
    {
        Mekan_listesi.Children.Clear();

        if (Aktif_evren.Mevcut != null && Aktif_evren.Mevcut.Mekanlar != null)
        {
            foreach (var m in Aktif_evren.Mevcut.Mekanlar)
            {
                Ekrana_mekan_ekle(m);
            }
        }
    }

    private void On_mekan_ekle_clicked(object sender, EventArgs e)
    {
        string? yeni_ad = Entry_mekanadi.Text?.Trim();

        if (!string.IsNullOrWhiteSpace(yeni_ad) && Aktif_evren.Mevcut != null)
        {
            var yeni_mekan = new Mekan_bilgisi { Ad = yeni_ad };

            Ekrana_mekan_ekle(yeni_mekan);
            Aktif_evren.Mevcut.Mekanlar.Add(yeni_mekan);

            var tum_evrenler = Json_motoru.Yukle();
            var guncellenecek_evren = tum_evrenler.FirstOrDefault(x => x.Evren_adi == Aktif_evren.Mevcut.Evren_adi);

            if (guncellenecek_evren != null)
            {
                guncellenecek_evren.Mekanlar = Aktif_evren.Mevcut.Mekanlar;
                Json_motoru.Kaydet(tum_evrenler);
            }

            Entry_mekanadi.Text = string.Empty;
        }
    }

    private void Ekrana_mekan_ekle(Mekan_bilgisi mekan)
    {
        var dikey_kutu = new VerticalStackLayout();

        var yatay_kutu = new Grid 
        { 
            ColumnDefinitions = new ColumnDefinitionCollection 
            { 
                new ColumnDefinition { Width = GridLength.Star }, 
                new ColumnDefinition { Width = GridLength.Auto },
                new ColumnDefinition { Width = GridLength.Auto }
            }, 
            Padding = new Thickness(0, 15) 
        };

        var isim_etiketi = new Label 
        { 
            Text = mekan.Ad, 
            TextColor = Color.FromArgb("#f5f5f5"), 
            FontSize = 18,
            FontFamily = "Serif",
            VerticalOptions = LayoutOptions.Center 
        };

        var detay_butonu = new Label
        {
            Text = "→",
            TextColor = Color.FromArgb("#737373"),
            FontSize = 22,
            VerticalOptions = LayoutOptions.Center,
            Padding = new Thickness(15, 0, 10, 0)
        };
        var tap_detay = new TapGestureRecognizer();
        tap_detay.Tapped += async (s, e) =>
        {
            await Navigation.PushModalAsync(new Mekan_detay_page(mekan));
        };
        detay_butonu.GestureRecognizers.Add(tap_detay);

        var sil_butonu = new Label
        {
            Text = "✕",
            TextColor = Color.FromArgb("#525252"),
            FontSize = 18,
            VerticalOptions = LayoutOptions.Center,
            Padding = new Thickness(10, 0, 0, 0)
        };
        var tap_sil = new TapGestureRecognizer();
        tap_sil.Tapped += (s, e) =>
        {
            Aktif_evren.Mevcut?.Mekanlar.Remove(mekan);
            var tum_evrenler = Json_motoru.Yukle();
            var guncel_evren = tum_evrenler.FirstOrDefault(x => x.Evren_adi == Aktif_evren.Mevcut?.Evren_adi);
            if (guncel_evren != null && Aktif_evren.Mevcut != null)
            {
                guncel_evren.Mekanlar = Aktif_evren.Mevcut.Mekanlar;
                Json_motoru.Kaydet(tum_evrenler);
            }
            Mekan_listesi.Children.Remove(dikey_kutu);
        };
        sil_butonu.GestureRecognizers.Add(tap_sil);

        yatay_kutu.Add(isim_etiketi, 0, 0);
        yatay_kutu.Add(detay_butonu, 1, 0);
        yatay_kutu.Add(sil_butonu, 2, 0);
        
        var ayrac = new BoxView { HeightRequest = 1, Color = Color.FromArgb("#171717") };
        
        dikey_kutu.Children.Add(yatay_kutu);
        dikey_kutu.Children.Add(ayrac);

        Mekan_listesi.Children.Add(dikey_kutu);
    }

    private async void On_geri_clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}