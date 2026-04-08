using System;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Roman_evreni_mobil;

public partial class Olaylarpage : ContentPage
{
    public Olaylarpage()
    {
        InitializeComponent();
        Olaylari_yukle();
    }

    protected override async void OnAppearing()
{
    base.OnAppearing();
    await Ana_kutu.FadeTo(1, 300);
}

    private void Olaylari_yukle()
    {
        Olay_listesi.Children.Clear();

        if (Aktif_evren.Mevcut != null && Aktif_evren.Mevcut.Olaylar != null)
        {
            foreach (var o in Aktif_evren.Mevcut.Olaylar)
            {
                Ekrana_olay_ekle(o);
            }
        }
    }

    private void On_olay_ekle_clicked(object sender, EventArgs e)
    {
        string? yeni_ad = Entry_olay_adi.Text?.Trim();

        if (!string.IsNullOrWhiteSpace(yeni_ad) && Aktif_evren.Mevcut != null)
        {
            var yeni_olay = new Olay_bilgisi { Ad = yeni_ad };

            Ekrana_olay_ekle(yeni_olay);
            Aktif_evren.Mevcut.Olaylar.Add(yeni_olay);

            var tum_evrenler = Json_motoru.Yukle();
            var guncellenecek_evren = tum_evrenler.FirstOrDefault(x => x.Evren_adi == Aktif_evren.Mevcut.Evren_adi);

            if (guncellenecek_evren != null)
            {
                guncellenecek_evren.Olaylar = Aktif_evren.Mevcut.Olaylar;
                Json_motoru.Kaydet(tum_evrenler);
            }

            Entry_olay_adi.Text = string.Empty;
        }
    }

    private void Ekrana_olay_ekle(Olay_bilgisi olay)
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
    Text = olay.Ad,
    TextColor = Color.FromArgb("#f5f5f5"),
    FontSize = 18,
    FontFamily = "Serif",
    VerticalOptions = LayoutOptions.Center
};
var tap_isim = new TapGestureRecognizer();
tap_isim.Tapped += async (s, e) =>
{
    await Navigation.PushModalAsync(new Olay_detay_page(olay));
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
            await Navigation.PushModalAsync(new Olay_detay_page(olay));
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
            bool onay = await DisplayAlert("Uyarı", $"{olay.Ad} silinsin mi?", "Evet", "Hayır");
            if (!onay)
                return;
                
            var silinen = new Silinen_oge { Tur = "Olay", Ad = olay.Ad, Veri_json = System.Text.Json.JsonSerializer.Serialize(olay) };
            Aktif_evren.Mevcut?.Cop_kutusu.Add(silinen);
            Aktif_evren.Mevcut?.Olaylar.Remove(olay);
            var tum_evrenler = Json_motoru.Yukle();
            var guncel_evren = tum_evrenler.FirstOrDefault(x => x.Evren_adi == Aktif_evren.Mevcut?.Evren_adi);
            if (guncel_evren != null && Aktif_evren.Mevcut != null)
            {
                guncel_evren.Olaylar = Aktif_evren.Mevcut.Olaylar;
                Json_motoru.Kaydet(tum_evrenler);
            }
            Olay_listesi.Children.Remove(dikey_kutu);
        };
        sil_butonu.GestureRecognizers.Add(tap_sil);

        var tarih_etiketi = new Label { Text = olay.Son_duzenleme != default ? olay.Son_duzenleme.ToString("dd.MM.yyyy") : "", TextColor = Color.FromArgb("#525252"), FontSize = 11, FontFamily = "Serif" };
var isim_grup = new VerticalStackLayout { Spacing = 2, VerticalOptions = LayoutOptions.Center };
isim_grup.Children.Add(isim_etiketi);
isim_grup.Children.Add(tarih_etiketi);
isim_grup.GestureRecognizers.Add(tap_isim);
var favori_butonu = new Label { Text = olay.Favori ? "★" : "☆", TextColor = olay.Favori ? Color.FromArgb("#f5c518") : Color.FromArgb("#737373"), FontSize = 20, VerticalOptions = LayoutOptions.Center, Padding = new Thickness(10, 0, 0, 0) };
var tap_favori = new TapGestureRecognizer();
tap_favori.Tapped += (s, e) => { olay.Favori = !olay.Favori; favori_butonu.Text = olay.Favori ? "★" : "☆"; favori_butonu.TextColor = olay.Favori ? Color.FromArgb("#f5c518") : Color.FromArgb("#737373"); var tum = Json_motoru.Yukle(); var guncel = tum.FirstOrDefault(x => x.Evren_adi == Aktif_evren.Mevcut?.Evren_adi); if (guncel != null && Aktif_evren.Mevcut != null) { guncel.Olaylar = Aktif_evren.Mevcut.Olaylar; Json_motoru.Kaydet(tum); } };
favori_butonu.GestureRecognizers.Add(tap_favori);

yatay_kutu.Add(isim_grup, 0, 0);
yatay_kutu.Add(favori_butonu, 1, 0);
yatay_kutu.Add(detay_butonu, 2, 0);
yatay_kutu.Add(sil_butonu, 3, 0);

        dikey_kutu.Children.Add(yatay_kutu);
        dikey_kutu.Children.Add(new BoxView { HeightRequest = 1, Color = Color.FromArgb("#171717") });

        Olay_listesi.Children.Add(dikey_kutu);
    }

    private async void On_geri_clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}