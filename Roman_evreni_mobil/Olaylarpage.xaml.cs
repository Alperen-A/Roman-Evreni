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

    private void Olaylari_yukle()
    {
        Olay_listesi.Children.Clear();

        if (Aktif_evren.Mevcut != null && Aktif_evren.Mevcut.Olaylar != null)
        {
            foreach (var O in Aktif_evren.Mevcut.Olaylar)
            {
                Ekrana_olay_ekle(O);
            }
        }
    }

    private void On_olay_ekle_clicked(object Sender, EventArgs E)
    {
        string Yeni_ad = Entry_olayadi.Text?.Trim() ?? "";

        if (!string.IsNullOrWhiteSpace(Yeni_ad) && Aktif_evren.Mevcut != null)
        {
            var Yeni_olay = new Olay_bilgisi { Ad = Yeni_ad };

            Ekrana_olay_ekle(Yeni_olay);
            Aktif_evren.Mevcut.Olaylar.Add(Yeni_olay);

            var Tum_evrenler = Json_motoru.Yukle();
            var Guncellenecek_evren = Tum_evrenler.FirstOrDefault(X => X.Evren_adi == Aktif_evren.Mevcut.Evren_adi);

            if (Guncellenecek_evren != null)
            {
                Guncellenecek_evren.Olaylar = Aktif_evren.Mevcut.Olaylar;
                Json_motoru.Kaydet(Tum_evrenler);
            }

            Entry_olayadi.Text = string.Empty;
        }
    }

    private void Ekrana_olay_ekle(Olay_bilgisi Olay)
    {
        var Dikey_kutu = new VerticalStackLayout();

        var Yatay_kutu = new Grid
        {
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Auto },
                new ColumnDefinition { Width = GridLength.Auto }
            },
            Padding = new Thickness(0, 15)
        };

        var Isim_etiketi = new Label
        {
            Text = Olay.Ad,
            TextColor = Color.FromArgb("#a3a3a3"),
            FontSize = 18,
            FontFamily = "Serif",
            VerticalOptions = LayoutOptions.Center
        };

        var Detay_butonu = new Label
        {
            Text = "→",
            TextColor = Color.FromArgb("#737373"),
            FontSize = 22,
            VerticalOptions = LayoutOptions.Center,
            Padding = new Thickness(15, 0, 10, 0)
        };
        var Tap_detay = new TapGestureRecognizer();
        Tap_detay.Tapped += async (S, E) =>
        {
            await Navigation.PushModalAsync(new Olay_detay_page(Olay));
        };
        Detay_butonu.GestureRecognizers.Add(Tap_detay);

        var Sil_butonu = new Label
        {
            Text = "✕",
            TextColor = Color.FromArgb("#525252"),
            FontSize = 18,
            VerticalOptions = LayoutOptions.Center,
            Padding = new Thickness(10, 0, 0, 0)
        };
        var Tap_sil = new TapGestureRecognizer();
        Tap_sil.Tapped += (S, E) =>
        {
            Aktif_evren.Mevcut?.Olaylar.Remove(Olay);
            var Tum_evrenler = Json_motoru.Yukle();
            var Guncel_evren = Tum_evrenler.FirstOrDefault(X => X.Evren_adi == Aktif_evren.Mevcut?.Evren_adi);
            if (Guncel_evren != null && Aktif_evren.Mevcut != null)
            {
                Guncel_evren.Olaylar = Aktif_evren.Mevcut.Olaylar;
                Json_motoru.Kaydet(Tum_evrenler);
            }
            Olay_listesi.Children.Remove(Dikey_kutu);
        };
        Sil_butonu.GestureRecognizers.Add(Tap_sil);

        Yatay_kutu.Add(Isim_etiketi, 0, 0);
        Yatay_kutu.Add(Detay_butonu, 1, 0);
        Yatay_kutu.Add(Sil_butonu, 2, 0);

        var Ayrac = new BoxView { HeightRequest = 1, Color = Color.FromArgb("#171717") };

        Dikey_kutu.Children.Add(Yatay_kutu);
        Dikey_kutu.Children.Add(Ayrac);

        Olay_listesi.Children.Add(Dikey_kutu);
    }

    private async void On_geri_clicked(object Sender, EventArgs E)
    {
        await Navigation.PopModalAsync();
    }
}