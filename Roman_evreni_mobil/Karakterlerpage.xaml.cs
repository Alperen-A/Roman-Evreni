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

   protected override async void OnAppearing()
{
    base.OnAppearing();
    Karakterleri_yukle();
    await Ana_kutu.FadeTo(1, 300);
}
    private void Karakterleri_yukle()
    {
        Karakter_listesi.Children.Clear();

        if (Aktif_evren.Mevcut != null && Aktif_evren.Mevcut.Karakterler != null)
        {
            foreach (var k in Aktif_evren.Mevcut.Karakterler)
            {
                Ekrana_karakter_ekle(k);
            }
        }
    }

    private void On_karakter_ekle_clicked(object sender, EventArgs e)
    {
        string? yeni_ad = Entry_karakter_adi.Text?.Trim();

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

            Entry_karakter_adi.Text = string.Empty;
        }
    }

    private void Ekrana_karakter_ekle(Karakter_bilgisi karakter)
    {
        var dikey_kutu = new VerticalStackLayout();

        var yatay_kutu = new Grid 
        { 
           ColumnDefinitions = new ColumnDefinitionCollection 
{ 
    new ColumnDefinition { Width = GridLength.Star }, 
    new ColumnDefinition { Width = GridLength.Auto },
    new ColumnDefinition { Width = GridLength.Auto },
    new ColumnDefinition { Width = GridLength.Auto }
}, 
Padding = new Thickness(0, 15)
        };

       var isim_etiketi = new Label 
    { 
         Text = karakter.Ad, 
      TextColor = Color.FromArgb("#f5f5f5"), 
      FontSize = 18,
      FontFamily = "Serif",
      VerticalOptions = LayoutOptions.Center 
        };
       var tap_isim = new TapGestureRecognizer();
      tap_isim.Tapped += async (s, e) =>
        {
    await Navigation.PushModalAsync(new Karakter_detay_page(karakter));
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
            await Navigation.PushModalAsync(new Karakter_detay_page(karakter));
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
            bool onay = await DisplayAlert("Uyarı", $"{karakter.Ad} silinsin mi?", "Evet", "Hayır");
            if (!onay)
                return;
                var silinen = new Silinen_oge
{
    Tur = "Karakter",
    Ad = karakter.Ad,
    Veri_json = System.Text.Json.JsonSerializer.Serialize(karakter)
};
Aktif_evren.Mevcut?.Cop_kutusu.Add(silinen);
        
            Aktif_evren.Mevcut?.Karakterler.Remove(karakter);
            var tum_evrenler = Json_motoru.Yukle();
            var guncel_evren = tum_evrenler.FirstOrDefault(x => x.Evren_adi == Aktif_evren.Mevcut?.Evren_adi);
            if (guncel_evren != null && Aktif_evren.Mevcut != null)
            {
                guncel_evren.Karakterler = Aktif_evren.Mevcut.Karakterler;
                Json_motoru.Kaydet(tum_evrenler);
            }
            Karakter_listesi.Children.Remove(dikey_kutu);
        };
        sil_butonu.GestureRecognizers.Add(tap_sil);

      var tarih_etiketi = new Label { Text = karakter.Son_duzenleme != default ? karakter.Son_duzenleme.ToString("dd.MM.yyyy") : "", TextColor = Color.FromArgb("#525252"), FontSize = 11, FontFamily = "Serif" };
var isim_grup = new VerticalStackLayout { Spacing = 2, VerticalOptions = LayoutOptions.Center };
isim_grup.Children.Add(isim_etiketi);
isim_grup.Children.Add(tarih_etiketi);
isim_grup.GestureRecognizers.Add(tap_isim);
var favori_butonu = new Label { Text = karakter.Favori ? "★" : "☆", TextColor = karakter.Favori ? Color.FromArgb("#f5c518") : Color.FromArgb("#737373"), FontSize = 20, VerticalOptions = LayoutOptions.Center, Padding = new Thickness(10, 0, 0, 0) };
var tap_favori = new TapGestureRecognizer();
tap_favori.Tapped += (s, e) => { karakter.Favori = !karakter.Favori; favori_butonu.Text = karakter.Favori ? "★" : "☆"; favori_butonu.TextColor = karakter.Favori ? Color.FromArgb("#f5c518") : Color.FromArgb("#737373"); var tum = Json_motoru.Yukle(); var guncel = tum.FirstOrDefault(x => x.Evren_adi == Aktif_evren.Mevcut?.Evren_adi); if (guncel != null && Aktif_evren.Mevcut != null) { guncel.Karakterler = Aktif_evren.Mevcut.Karakterler; Json_motoru.Kaydet(tum); } };
favori_butonu.GestureRecognizers.Add(tap_favori);

yatay_kutu.Add(isim_grup, 0, 0);
yatay_kutu.Add(favori_butonu, 1, 0);
yatay_kutu.Add(detay_butonu, 2, 0);
yatay_kutu.Add(sil_butonu, 3, 0);
        
        var ayrac = new BoxView { HeightRequest = 1, Color = Color.FromArgb("#171717") };
        
        dikey_kutu.Children.Add(yatay_kutu);
        dikey_kutu.Children.Add(ayrac);

        Karakter_listesi.Children.Add(dikey_kutu);
    }

    private async void On_geri_clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}