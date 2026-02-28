using System;
using System.Linq;
using Microsoft.Maui.Controls;

namespace Roman_evreni_mobil;

public partial class Karakter_detay_page : ContentPage
{
    private Karakter_bilgisi _secili_karakter;

    public Karakter_detay_page(Karakter_bilgisi karakter)
    {
        InitializeComponent();
        _secili_karakter = karakter;
        
        // Ekrani mevcut bilgilerle dolduruyoruz
        Entry_ad.Text = _secili_karakter.Ad;
        Entry_rol.Text = _secili_karakter.Rol;
        Entry_silah.Text = _secili_karakter.Silah;
        Editor_hikaye.Text = _secili_karakter.Hikaye;
    }

    private async void On_kaydet_clicked(object sender, EventArgs e)
    {
        // Kutulardaki yeni bilgileri karakterimize aktariyoruz
        _secili_karakter.Rol = Entry_rol.Text ?? "";
        _secili_karakter.Silah = Entry_silah.Text ?? "";
        _secili_karakter.Hikaye = Editor_hikaye.Text ?? "";

        // Tum evrenleri okuyup sadece bizimkini guncelleyip geri kaydediyoruz
        var tum_evrenler = Json_motoru.Yukle();
        var guncel_evren = tum_evrenler.FirstOrDefault(x => x.Evren_adi == Aktif_evren.Mevcut?.Evren_adi);
        
        if (guncel_evren != null && Aktif_evren.Mevcut != null)
        {
            guncel_evren.Karakterler = Aktif_evren.Mevcut.Karakterler;
            Json_motoru.Kaydet(tum_evrenler);
        }

        // Islem bitince sayfayi kapat
        await Navigation.PopModalAsync();
    }

    private async void On_geri_clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}