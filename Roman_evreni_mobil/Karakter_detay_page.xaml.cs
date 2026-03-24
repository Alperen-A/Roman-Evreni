using System;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Roman_evreni_mobil;

public partial class Karakter_detay_page : ContentPage
{
    private Karakter_bilgisi _secili_karakter;

    public Karakter_detay_page(Karakter_bilgisi Karakter)
    {
        InitializeComponent();
        _secili_karakter = Karakter;
        Entry_ad.Text = _secili_karakter.Ad;
        Entry_dis_gorunus.Text = _secili_karakter.Dis_gorunus;
        Editor_hikaye.Text = _secili_karakter.Hikaye;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        Karakter_favori_label.Text = _secili_karakter.Favori ? "★" : "☆";
        Karakter_favori_label.TextColor = _secili_karakter.Favori ? Color.FromArgb("#f5c518") : Color.FromArgb("#737373");
        await Ana_kutu.FadeTo(1, 300);
    }

    private void On_favori_clicked(object sender, EventArgs e)
    {
        _secili_karakter.Favori = !_secili_karakter.Favori;
        Karakter_favori_label.Text = _secili_karakter.Favori ? "★" : "☆";
        Karakter_favori_label.TextColor = _secili_karakter.Favori ? Color.FromArgb("#f5c518") : Color.FromArgb("#737373");
        var tum = Json_motoru.Yukle();
        var guncel = tum.FirstOrDefault(x => x.Evren_adi == Aktif_evren.Mevcut?.Evren_adi);
        if (guncel != null && Aktif_evren.Mevcut != null) { guncel.Karakterler = Aktif_evren.Mevcut.Karakterler; Json_motoru.Kaydet(tum); }
    }

    private async void On_kaydet_clicked(object Sender, EventArgs E)
    {
        await KaydetVeKapat();
    }

    private async void On_geri_clicked(object Sender, EventArgs E)
    {
        await KaydetVeKapat();
    }

    private async Task KaydetVeKapat()
    {
        _secili_karakter.Ad = Entry_ad.Text?.Trim() ?? _secili_karakter.Ad;
        _secili_karakter.Son_duzenleme = DateTime.Now;
        _secili_karakter.Dis_gorunus = Entry_dis_gorunus.Text ?? "";
        _secili_karakter.Hikaye = Editor_hikaye.Text ?? "";

        var Tum_evrenler = Json_motoru.Yukle();
        var Guncel_evren = Tum_evrenler.FirstOrDefault(X => X.Evren_adi == Aktif_evren.Mevcut?.Evren_adi);
        if (Guncel_evren != null && Aktif_evren.Mevcut != null)
        {
            Guncel_evren.Karakterler = Aktif_evren.Mevcut.Karakterler;
            Json_motoru.Kaydet(Tum_evrenler);
        }

        await Navigation.PopModalAsync();
    }
}