using System;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Roman_evreni_mobil;

public partial class Roman_detay_page : ContentPage
{
    private Roman _roman;

    public Roman_detay_page(Roman roman)
    {
        InitializeComponent();
        _roman = roman;
        Roman_adi_label.Text = _roman.Ad;
        Bolumleri_yukle();
    }

    protected override async void OnAppearing()
{
    base.OnAppearing();
    await Ana_kutu.FadeTo(1, 300);
}

    private void Bolumleri_yukle()
    {
        Bolum_listesi.Children.Clear();
        foreach (var bolum in _roman.Bolumler)
        {
            Ekrana_bolum_ekle(bolum);
        }
    }

    private void On_bolum_ekle_clicked(object sender, EventArgs e)
    {
        string? yeni_ad = Entry_bolum_adi.Text?.Trim();

        if (!string.IsNullOrWhiteSpace(yeni_ad))
        {
            var yeni_bolum = new Bolum { Baslik = yeni_ad };
            _roman.Bolumler.Add(yeni_bolum);
            Kaydet();
            Ekrana_bolum_ekle(yeni_bolum);
            Entry_bolum_adi.Text = string.Empty;
        }
    }

    private void Ekrana_bolum_ekle(Bolum bolum)
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

        var baslik_etiketi = new Label
        {
            Text = bolum.Baslik,
            TextColor = Color.FromArgb("#f5f5f5"),
            FontSize = 18,
            FontFamily = "Serif",
            VerticalOptions = LayoutOptions.Center
        };

        var tap_baslik = new TapGestureRecognizer();
          tap_baslik.Tapped += async (s, e) =>
{
        await Navigation.PushModalAsync(new Bolum_detay_page(bolum, _roman));
};
         baslik_etiketi.GestureRecognizers.Add(tap_baslik);

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
            await Navigation.PushModalAsync(new Bolum_detay_page(bolum, _roman));
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
            bool onay = await DisplayAlert("Uyarı", $"{bolum.Baslik} silinsin mi?", "Evet", "Hayır");
            if (onay)
            {
                _roman.Bolumler.Remove(bolum);
                Kaydet();
                Bolum_listesi.Children.Remove(dikey_kutu);
            }
        };
        sil_butonu.GestureRecognizers.Add(tap_sil);

        yatay_kutu.Add(baslik_etiketi, 0, 0);
        yatay_kutu.Add(detay_butonu, 1, 0);
        yatay_kutu.Add(sil_butonu, 2, 0);

        dikey_kutu.Children.Add(yatay_kutu);
        dikey_kutu.Children.Add(new BoxView { HeightRequest = 1, Color = Color.FromArgb("#171717") });

        Bolum_listesi.Children.Add(dikey_kutu);
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