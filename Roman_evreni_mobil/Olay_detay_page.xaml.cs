using System;
using System.Linq;
using Microsoft.Maui.Controls;

namespace Roman_evreni_mobil;

public partial class Olay_detay_page : ContentPage
{
    private Olay_bilgisi _secili_olay;

    public Olay_detay_page(Olay_bilgisi Olay)
    {
        InitializeComponent();
        _secili_olay = Olay;
        
        Entry_ad.Text = _secili_olay.Ad;
        Entry_mekanlar.Text = _secili_olay.Mekanlar;
        Entry_karakterler.Text = _secili_olay.Karakterler;
        Editor_aciklama.Text = _secili_olay.Aciklama;
    }

    private async void On_kaydet_clicked(object Sender, EventArgs E)
    {
        _secili_olay.Mekanlar = Entry_mekanlar.Text ?? "";
        _secili_olay.Karakterler = Entry_karakterler.Text ?? "";
        _secili_olay.Aciklama = Editor_aciklama.Text ?? "";

        var Tum_evrenler = Json_motoru.Yukle();
        var Guncel_evren = Tum_evrenler.FirstOrDefault(X => X.Evren_adi == Aktif_evren.Mevcut?.Evren_adi);
        
        if (Guncel_evren != null && Aktif_evren.Mevcut != null)
        {
            Guncel_evren.Olaylar = Aktif_evren.Mevcut.Olaylar;
            Json_motoru.Kaydet(Tum_evrenler);
        }

        await Navigation.PopModalAsync();
    }

    private async void On_geri_clicked(object Sender, EventArgs E)
    {
        await Navigation.PopModalAsync();
    }
}