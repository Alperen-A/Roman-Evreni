using System;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Roman_evreni_mobil;

public partial class Notlarpage : ContentPage
{
    public Notlarpage()
    {
        InitializeComponent();
        Notlari_yukle();
    }

    private void Notlari_yukle()
    {
        Not_listesi.Children.Clear();

        if (Aktif_evren.Mevcut?.Notlar != null)
        {
            foreach (var not in Aktif_evren.Mevcut.Notlar)
            {
                Ekrana_not_ekle(not);
            }
        }
    }

    private void On_not_ekle_clicked(object sender, EventArgs e)
    {
        string? yeni_baslik = Entry_not_adi.Text?.Trim();

        if (!string.IsNullOrWhiteSpace(yeni_baslik) && Aktif_evren.Mevcut != null)
        {
            var yeni_not = new Not_kaydi { Baslik = yeni_baslik };
            Aktif_evren.Mevcut.Notlar.Add(yeni_not);
            Kaydet();
            Ekrana_not_ekle(yeni_not);
            Entry_not_adi.Text = string.Empty;
        }
    }

    private void Ekrana_not_ekle(Not_kaydi not)
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

       var baslik_etiketi = new Label
{
    Text = not.Baslik,
    TextColor = Color.FromArgb("#f5f5f5"),
    FontSize = 18,
    FontFamily = "Serif",
    VerticalOptions = LayoutOptions.Center
};
var tap_isim = new TapGestureRecognizer();
tap_isim.Tapped += async (s, e) =>
{
    await Navigation.PushModalAsync(new Not_detay_page(not));
};
baslik_etiketi.GestureRecognizers.Add(tap_isim);

        var tap_baslik = new TapGestureRecognizer();
            tap_baslik.Tapped += async (s, e) =>
            {
        await Navigation.PushModalAsync(new Not_detay_page(not));
            };
            baslik_etiketi.GestureRecognizers.Add(tap_baslik);

        // Favori yıldız butonu
        var favori_butonu = new Label
        {
            Text = not.Favori ? "★" : "☆",
            TextColor = not.Favori ? Color.FromArgb("#f5c518") : Color.FromArgb("#737373"),
            FontSize = 20,
            VerticalOptions = LayoutOptions.Center,
            Padding = new Thickness(10, 0, 0, 0)
        };
        var tap_favori = new TapGestureRecognizer();
        tap_favori.Tapped += (s, e) =>
        {
            not.Favori = !not.Favori;
            favori_butonu.Text = not.Favori ? "★" : "☆";
            favori_butonu.TextColor = not.Favori ? Color.FromArgb("#f5c518") : Color.FromArgb("#737373");
            Kaydet();
        };
        favori_butonu.GestureRecognizers.Add(tap_favori);

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
            await Navigation.PushModalAsync(new Not_detay_page(not));
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
            bool onay = await DisplayAlert("Uyarı", $"{not.Baslik} silinsin mi?", "Evet", "Hayır");
            if (onay)
            {
                Aktif_evren.Mevcut?.Notlar.Remove(not);
                Kaydet();
                Not_listesi.Children.Remove(dikey_kutu);
            }
        };
        sil_butonu.GestureRecognizers.Add(tap_sil);

        yatay_kutu.Add(baslik_etiketi, 0, 0);
        yatay_kutu.Add(favori_butonu, 1, 0);
        yatay_kutu.Add(detay_butonu, 2, 0);
        yatay_kutu.Add(sil_butonu, 3, 0);

        dikey_kutu.Children.Add(yatay_kutu);
        dikey_kutu.Children.Add(new BoxView { HeightRequest = 1, Color = Color.FromArgb("#171717") });

        Not_listesi.Children.Add(dikey_kutu);
    }

    private void Kaydet()
    {
        var tum_evrenler = Json_motoru.Yukle();
        var guncel = tum_evrenler.FirstOrDefault(x => x.Evren_adi == Aktif_evren.Mevcut?.Evren_adi);
        if (guncel != null && Aktif_evren.Mevcut != null)
        {
            guncel.Notlar = Aktif_evren.Mevcut.Notlar;
            Json_motoru.Kaydet(tum_evrenler);
        }
    }

    private async void On_geri_clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}