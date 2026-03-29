using System;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Roman_evreni_mobil;

public partial class Mekan_detay_page : ContentPage
{
    private Mekan_bilgisi _secili_mekan;

    public Mekan_detay_page(Mekan_bilgisi mekan)
    {
        InitializeComponent();
        _secili_mekan = mekan;
        Entry_ad.Text = _secili_mekan.Ad;
        Entry_iklim.Text = _secili_mekan.Iklim;
        Entry_hukumdar.Text = _secili_mekan.Hukumdar;
        Editor_aciklama.Text = _secili_mekan.Aciklama;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        Mekan_favori_label.Text = _secili_mekan.Favori ? "★" : "☆";
        Mekan_favori_label.TextColor = _secili_mekan.Favori ? Color.FromArgb("#f5c518") : Color.FromArgb("#737373");
        await Ana_kutu.FadeTo(1, 300);
    }

    private void On_favori_clicked(object sender, EventArgs e)
    {
        _secili_mekan.Favori = !_secili_mekan.Favori;
        Mekan_favori_label.Text = _secili_mekan.Favori ? "★" : "☆";
        Mekan_favori_label.TextColor = _secili_mekan.Favori ? Color.FromArgb("#f5c518") : Color.FromArgb("#737373");
        var tum = Json_motoru.Yukle();
        var guncel = tum.FirstOrDefault(x => x.Evren_adi == Aktif_evren.Mevcut?.Evren_adi);
        if (guncel != null && Aktif_evren.Mevcut != null) { guncel.Mekanlar = Aktif_evren.Mevcut.Mekanlar; Json_motoru.Kaydet(tum); }
    }

    private async void On_kaydet_clicked(object sender, EventArgs e)
    {
        await KaydetVeKapat();
    }

    private async void On_geri_clicked(object sender, EventArgs e)
    {
        await KaydetVeKapat();
    }

    private async Task KaydetVeKapat()
    {
        _secili_mekan.Ad = Entry_ad.Text?.Trim() ?? _secili_mekan.Ad;
        _secili_mekan.Son_duzenleme = DateTime.Now;
        _secili_mekan.Iklim = Entry_iklim.Text ?? "";
        _secili_mekan.Hukumdar = Entry_hukumdar.Text ?? "";
        _secili_mekan.Aciklama = Editor_aciklama.Text ?? "";

        var tum_evrenler = Json_motoru.Yukle();
        var guncel_evren = tum_evrenler.FirstOrDefault(x => x.Evren_adi == Aktif_evren.Mevcut?.Evren_adi);
        if (guncel_evren != null && Aktif_evren.Mevcut != null)
        {
            guncel_evren.Mekanlar = Aktif_evren.Mevcut.Mekanlar;
            Json_motoru.Kaydet(tum_evrenler);
        }

        await Navigation.PopModalAsync();
    }
}