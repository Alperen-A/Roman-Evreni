using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Roman_evreni_mobil;

public partial class Olay_detay_page : ContentPage
{
    private Olay_bilgisi _secili_olay;

    public Olay_detay_page(Olay_bilgisi olay)
    {
        InitializeComponent();
        _secili_olay = olay;
        Entry_ad.Text = _secili_olay.Ad;
        Editor_aciklama.Text = _secili_olay.Aciklama;
        Karakter_secimlerini_yukle();
        Mekan_secimlerini_yukle();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        Olay_favori_label.Text = _secili_olay.Favori ? "★" : "☆";
        Olay_favori_label.TextColor = _secili_olay.Favori ? Color.FromArgb("#f5c518") : Color.FromArgb("#737373");
        await Ana_kutu.FadeTo(1, 300);
    }

    private void On_favori_clicked(object sender, EventArgs e)
    {
        _secili_olay.Favori = !_secili_olay.Favori;
        Olay_favori_label.Text = _secili_olay.Favori ? "★" : "☆";
        Olay_favori_label.TextColor = _secili_olay.Favori ? Color.FromArgb("#f5c518") : Color.FromArgb("#737373");
        var tum = Json_motoru.Yukle();
        var guncel = tum.FirstOrDefault(x => x.Evren_adi == Aktif_evren.Mevcut?.Evren_adi);
        if (guncel != null && Aktif_evren.Mevcut != null) { guncel.Olaylar = Aktif_evren.Mevcut.Olaylar; Json_motoru.Kaydet(tum); }
    }

    private void Karakter_secimlerini_yukle()
    {
        Karakter_secim_kutusu.Children.Clear();
        if (Aktif_evren.Mevcut == null) return;

        foreach (var karakter in Aktif_evren.Mevcut.Karakterler)
        {
            bool secili = _secili_olay.Karakter_idleri.Contains(karakter.Id);
            var chip = Chip_olustur(karakter.Ad, secili, (aktif) =>
            {
                if (aktif) _secili_olay.Karakter_idleri.Add(karakter.Id);
                else _secili_olay.Karakter_idleri.Remove(karakter.Id);
            });
            Karakter_secim_kutusu.Children.Add(chip);
        }
    }

    private void Mekan_secimlerini_yukle()
    {
        Mekan_secim_kutusu.Children.Clear();
        if (Aktif_evren.Mevcut == null) return;

        foreach (var mekan in Aktif_evren.Mevcut.Mekanlar)
        {
            bool secili = _secili_olay.Mekan_idleri.Contains(mekan.Id);
            var chip = Chip_olustur(mekan.Ad, secili, (aktif) =>
            {
                if (aktif) _secili_olay.Mekan_idleri.Add(mekan.Id);
                else _secili_olay.Mekan_idleri.Remove(mekan.Id);
            });
            Mekan_secim_kutusu.Children.Add(chip);
        }
    }

    private Border Chip_olustur(string ad, bool secili, Action<bool> degisti)
    {
        bool aktif = secili;
        var etiket = new Label
        {
            Text = ad,
            FontSize = 14,
            TextColor = aktif ? Color.FromArgb("#0a0a0a") : Color.FromArgb("#f5f5f5"),
            VerticalOptions = LayoutOptions.Center
        };
        var chip = new Border
        {
            BackgroundColor = aktif ? Color.FromArgb("#f5f5f5") : Color.FromArgb("#1a1a1a"),
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 20 },
            Stroke = Color.FromArgb("#333333"),
            Padding = new Thickness(14, 8),
            Margin = new Thickness(0, 0, 8, 8),
            Content = etiket
        };
        var tap = new TapGestureRecognizer();
        tap.Tapped += (s, e) =>
        {
            aktif = !aktif;
            chip.BackgroundColor = aktif ? Color.FromArgb("#f5f5f5") : Color.FromArgb("#1a1a1a");
            etiket.TextColor = aktif ? Color.FromArgb("#0a0a0a") : Color.FromArgb("#f5f5f5");
            degisti(aktif);
        };
        chip.GestureRecognizers.Add(tap);
        return chip;
    }

    private async void On_kaydet_clicked(object sender, EventArgs e)
    {
        _secili_olay.Ad = Entry_ad.Text?.Trim() ?? _secili_olay.Ad;
        _secili_olay.Son_duzenleme = DateTime.Now;
        _secili_olay.Aciklama = Editor_aciklama.Text ?? "";
        var tum_evrenler = Json_motoru.Yukle();
        var guncel_evren = tum_evrenler.FirstOrDefault(x => x.Evren_adi == Aktif_evren.Mevcut?.Evren_adi);
        if (guncel_evren != null && Aktif_evren.Mevcut != null)
        {
            guncel_evren.Olaylar = Aktif_evren.Mevcut.Olaylar;
            Json_motoru.Kaydet(tum_evrenler);
        }
        await Navigation.PopModalAsync();
    }

    private async void On_geri_clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}