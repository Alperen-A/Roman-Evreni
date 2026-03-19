using System;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Roman_evreni_mobil;

public partial class Favorilerpage : ContentPage
{
    public Favorilerpage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        Favorileri_yukle();
        await Ana_kutu.FadeTo(1, 300);
    }

    private void Favorileri_yukle()
    {
        Favori_listesi.Children.Clear();
        if (Aktif_evren.Mevcut == null) return;

        bool hic_favori_yok = true;

        var favori_karakterler = Aktif_evren.Mevcut.Karakterler.Where(k => k.Favori).ToList();
        if (favori_karakterler.Any())
        {
            hic_favori_yok = false;
            Kategori_baslik_ekle("👤 Karakterler");
            foreach (var k in favori_karakterler)
                Favori_satir_ekle(k.Ad, k.Son_duzenleme, async () => await Navigation.PushModalAsync(new Karakter_detay_page(k)));
        }

        var favori_mekanlar = Aktif_evren.Mevcut.Mekanlar.Where(m => m.Favori).ToList();
        if (favori_mekanlar.Any())
        {
            hic_favori_yok = false;
            Kategori_baslik_ekle("🏰 Mekanlar");
            foreach (var m in favori_mekanlar)
                Favori_satir_ekle(m.Ad, m.Son_duzenleme, async () => await Navigation.PushModalAsync(new Mekan_detay_page(m)));
        }

        var favori_olaylar = Aktif_evren.Mevcut.Olaylar.Where(o => o.Favori).ToList();
        if (favori_olaylar.Any())
        {
            hic_favori_yok = false;
            Kategori_baslik_ekle("⚡ Olaylar");
            foreach (var o in favori_olaylar)
                Favori_satir_ekle(o.Ad, o.Son_duzenleme, async () => await Navigation.PushModalAsync(new Olay_detay_page(o)));
        }

        var favori_notlar = Aktif_evren.Mevcut.Notlar.Where(n => n.Favori).ToList();
        if (favori_notlar.Any())
        {
            hic_favori_yok = false;
            Kategori_baslik_ekle("📝 Notlar");
            foreach (var n in favori_notlar)
                Favori_satir_ekle(n.Baslik, n.Son_duzenleme, async () => await Navigation.PushModalAsync(new Not_detay_page(n)));
        }

        if (hic_favori_yok)
        {
            Favori_listesi.Children.Add(new Label
            {
                Text = "Henüz favori eklenmedi.\nKarakter, mekan, olay veya notları yıldızlayarak buraya ekleyebilirsin.",
                TextColor = Color.FromArgb("#525252"),
                FontSize = 16,
                FontFamily = "Serif",
                FontAttributes = FontAttributes.Italic,
                HorizontalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 60, 0, 0)
            });
        }
    }

    private void Kategori_baslik_ekle(string baslik)
    {
        Favori_listesi.Children.Add(new Label
        {
            Text = baslik,
            TextColor = Color.FromArgb("#a3a3a3"),
            FontSize = 13,
            FontFamily = "Serif",
            Margin = new Thickness(0, 20, 0, 5)
        });
        Favori_listesi.Children.Add(new BoxView { HeightRequest = 1, Color = Color.FromArgb("#262626") });
    }

    private void Favori_satir_ekle(string ad, DateTime tarih, Func<System.Threading.Tasks.Task> tikla)
    {
        var satir = new Grid
        {
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Auto }
            },
            Padding = new Thickness(0, 12)
        };

        var isim_grup = new VerticalStackLayout { Spacing = 2, VerticalOptions = LayoutOptions.Center };
        isim_grup.Children.Add(new Label { Text = ad, TextColor = Color.FromArgb("#f5f5f5"), FontSize = 18, FontFamily = "Serif" });
        isim_grup.Children.Add(new Label { Text = tarih != default ? tarih.ToString("dd.MM.yyyy") : "", TextColor = Color.FromArgb("#525252"), FontSize = 11, FontFamily = "Serif" });

        satir.Add(isim_grup, 0, 0);
        satir.Add(new Label { Text = "★", TextColor = Color.FromArgb("#f5c518"), FontSize = 20, VerticalOptions = LayoutOptions.Center }, 1, 0);

        var tap = new TapGestureRecognizer();
        tap.Tapped += async (s, e) => await tikla();
        satir.GestureRecognizers.Add(tap);

        Favori_listesi.Children.Add(satir);
        Favori_listesi.Children.Add(new BoxView { HeightRequest = 1, Color = Color.FromArgb("#171717") });
    }

    private async void On_geri_clicked(object sender, EventArgs e) => await Navigation.PopModalAsync();
}