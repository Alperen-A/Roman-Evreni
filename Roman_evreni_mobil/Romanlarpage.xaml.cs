using System;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Roman_evreni_mobil;

public partial class Romanlarpage : ContentPage
{
    public Romanlarpage()
    {
        InitializeComponent();
        Romanlari_yukle();
    }

    protected override async void OnAppearing()
{
    base.OnAppearing();
    await Ana_kutu.FadeTo(1, 300);
}

    private void Romanlari_yukle()
    {
        Roman_listesi.Children.Clear();

        if (Aktif_evren.Mevcut?.Romanlar != null)
        {
            foreach (var roman in Aktif_evren.Mevcut.Romanlar)
            {
                Ekrana_roman_ekle(roman);
            }
        }
    }

    private void On_roman_ekle_clicked(object sender, EventArgs e)
    {
        string? yeni_ad = Entry_roman_adi.Text?.Trim();

        if (!string.IsNullOrWhiteSpace(yeni_ad) && Aktif_evren.Mevcut != null)
        {
            var yeni_roman = new Roman { Ad = yeni_ad };

            Aktif_evren.Mevcut.Romanlar.Add(yeni_roman);
            Kaydet();
            Ekrana_roman_ekle(yeni_roman);
            Entry_roman_adi.Text = string.Empty;
        }
    }

    private void Ekrana_roman_ekle(Roman roman)
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
    Text = roman.Ad,
    TextColor = Color.FromArgb("#f5f5f5"),
    FontSize = 18,
    FontFamily = "Serif",
    VerticalOptions = LayoutOptions.Center
};
var tap_isim = new TapGestureRecognizer();
tap_isim.Tapped += async (s, e) =>
{
    await Navigation.PushModalAsync(new Roman_detay_page(roman));
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
            await Navigation.PushModalAsync(new Roman_detay_page(roman));
        };
        detay_butonu.GestureRecognizers.Add(tap_detay);

        var sil_butonu = new Label
        {
            Text = "✕",
            TextColor = Color.FromArgb("#737373"),
            FontSize = 18,
            VerticalOptions = LayoutOptions.Center,
            Padding = new Thickness(10, 0, 0, 0)
        };
        var tap_sil = new TapGestureRecognizer();
        tap_sil.Tapped += async (s, e) =>
        {
            bool onay = await DisplayAlert("Uyarı", $"{roman.Ad} silinsin mi?", "Evet", "Hayır");
            if (onay)
            {
                Aktif_evren.Mevcut?.Romanlar.Remove(roman);
                Kaydet();
                Roman_listesi.Children.Remove(dikey_kutu);
            }
        };
        sil_butonu.GestureRecognizers.Add(tap_sil);

        var tarih_etiketi = new Label { Text = roman.Son_duzenleme != default ? roman.Son_duzenleme.ToString("dd.MM.yyyy") : "", TextColor = Color.FromArgb("#525252"), FontSize = 11, FontFamily = "Serif" };
var isim_grup = new VerticalStackLayout { Spacing = 2, VerticalOptions = LayoutOptions.Center };
isim_grup.Children.Add(isim_etiketi);
isim_grup.Children.Add(tarih_etiketi);
isim_grup.GestureRecognizers.Add(tap_isim);
yatay_kutu.Add(isim_grup, 0, 0);
        yatay_kutu.Add(detay_butonu, 1, 0);
        yatay_kutu.Add(sil_butonu, 2, 0);

        dikey_kutu.Children.Add(yatay_kutu);
        dikey_kutu.Children.Add(new BoxView { HeightRequest = 1, Color = Color.FromArgb("#171717") });

        Roman_listesi.Children.Add(dikey_kutu);
    }

    private void Kaydet()
    {
        var tum_evrenler = Json_motoru.Yukle();
        var guncel = tum_evrenler.FirstOrDefault(x => x.Evren_adi == Aktif_evren.Mevcut?.Evren_adi);
        if (guncel != null && Aktif_evren.Mevcut != null)
        {
            guncel.Romanlar = Aktif_evren.Mevcut.Romanlar;
            Json_motoru.Kaydet(tum_evrenler);
        }
    }

    private async void On_geri_clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}