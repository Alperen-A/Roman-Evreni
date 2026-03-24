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
protected override async void OnAppearing()
{
    base.OnAppearing();
    Mekanlari_yukle();
    await Ana_kutu.FadeTo(1, 300);
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
            { new ColumnDefinition { Width = GridLength.Star },
new ColumnDefinition { Width = GridLength.Auto },
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
    var tap_isim = new TapGestureRecognizer();
    tap_isim.Tapped += async (s, e) =>
    {
    await Navigation.PushModalAsync(new Mekan_detay_page(mekan));
    };
    isim_etiketi.GestureRecognizers.Add(tap_isim);

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
        tap_sil.Tapped += async (s, e) =>
        {
            bool onay = await DisplayAlert("Uyarı", $"{mekan.Ad} silinsin mi?", "Evet", "Hayır");
            if (!onay)
                return;

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

        var tarih_etiketi = new Label { Text = mekan.Son_duzenleme != default ? mekan.Son_duzenleme.ToString("dd.MM.yyyy") : "", TextColor = Color.FromArgb("#525252"), FontSize = 11, FontFamily = "Serif" };
var isim_grup = new VerticalStackLayout { Spacing = 2, VerticalOptions = LayoutOptions.Center };
isim_grup.Children.Add(isim_etiketi);
isim_grup.Children.Add(tarih_etiketi);
isim_grup.GestureRecognizers.Add(tap_isim);
var favori_butonu = new Label { Text = mekan.Favori ? "★" : "☆", TextColor = mekan.Favori ? Color.FromArgb("#f5c518") : Color.FromArgb("#737373"), FontSize = 20, VerticalOptions = LayoutOptions.Center, Padding = new Thickness(10, 0, 0, 0) };
var tap_favori = new TapGestureRecognizer();
tap_favori.Tapped += (s, e) => { mekan.Favori = !mekan.Favori; favori_butonu.Text = mekan.Favori ? "★" : "☆"; favori_butonu.TextColor = mekan.Favori ? Color.FromArgb("#f5c518") : Color.FromArgb("#737373"); var tum = Json_motoru.Yukle(); var guncel = tum.FirstOrDefault(x => x.Evren_adi == Aktif_evren.Mevcut?.Evren_adi); if (guncel != null && Aktif_evren.Mevcut != null) { guncel.Mekanlar = Aktif_evren.Mevcut.Mekanlar; Json_motoru.Kaydet(tum); } };
favori_butonu.GestureRecognizers.Add(tap_favori);

yatay_kutu.Add(isim_grup, 0, 0);
yatay_kutu.Add(favori_butonu, 1, 0);
yatay_kutu.Add(detay_butonu, 2, 0);
yatay_kutu.Add(sil_butonu, 3, 0);
        
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