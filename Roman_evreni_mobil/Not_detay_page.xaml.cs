using System;
using System.Linq;
using Microsoft.Maui.Controls;

namespace Roman_evreni_mobil;

public partial class Not_detay_page : ContentPage
{
    private Not_kaydi _not;
    private System.Timers.Timer _otomatik_kayit;
    public Not_detay_page(Not_kaydi not)
{
    InitializeComponent();
    _not = not;

    _otomatik_kayit = new System.Timers.Timer(3000);
    _otomatik_kayit.AutoReset = false;
    _otomatik_kayit.Elapsed += async (s, e) =>
    {
        await MainThread.InvokeOnMainThreadAsync(() => Kaydet());
    };

    Not_baslik_label.Text = _not.Baslik;
    Editor_icerik.Text = _not.Icerik;
    Kelime_sayacini_guncelle(_not.Icerik);
}

protected override async void OnAppearing()
{
    base.OnAppearing();
    await Ana_kutu.FadeTo(1, 300);
}

    private void On_metin_degisti(object sender, TextChangedEventArgs e)
    {
        Kelime_sayacini_guncelle(e.NewTextValue);
        _otomatik_kayit.Stop();
        _otomatik_kayit.Start();
    }

    private void Kelime_sayacini_guncelle(string metin)
    {
        if (string.IsNullOrWhiteSpace(metin))
        {
            Kelime_sayaci.Text = "0 kelime";
            return;
        }
        int kelime = metin.Trim().Split(new char[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
        Kelime_sayaci.Text = $"{kelime} kelime";
    }

    private void Kaydet()
    {
        _not.Icerik = Editor_icerik.Text ?? "";
        _not.Son_duzenleme = DateTime.Now;

        var tum_evrenler = Json_motoru.Yukle();
        var guncel = tum_evrenler.FirstOrDefault(x => x.Evren_adi == Aktif_evren.Mevcut?.Evren_adi);
        if (guncel != null && Aktif_evren.Mevcut != null)
        {
            guncel.Notlar = Aktif_evren.Mevcut.Notlar;
            Json_motoru.Kaydet(tum_evrenler);
        }
    }

    private async void On_kaydet_clicked(object sender, EventArgs e)
    {
        Kaydet();
        await DisplayAlert("", "Kaydedildi ✓", "Tamam");
    }

    private async void On_geri_clicked(object sender, EventArgs e)
    {
        Kaydet();
        _otomatik_kayit.Stop();
        await Navigation.PopModalAsync();
    }
}