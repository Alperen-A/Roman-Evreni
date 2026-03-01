using System;
using Microsoft.Maui.Controls;

namespace Roman_evreni_mobil;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        // Ismi tasarimla ayni olacak sekilde kucuk harfe cevirdik
        if (Aktif_evren.Mevcut != null)
        {
            Lbl_evrenadi.Text = "🌌 " + Aktif_evren.Mevcut.Evren_adi;
        }
    }

    private async void OnKarakterlerClicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new Karakterlerpage());
    }

    // Buraya async kelimesini ekledik
    private async void OnMekanlarClicked(object sender, EventArgs e)
    {
        // Sonuna noktali virgul eklendi
        await Navigation.PushModalAsync(new Mekanlarpage());
    }

   private async void OnOlaylarClicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new Olaylarpage());
    }

    private async void OnGeriClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
    private async void On_asistan_clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new Yapay_zeka_page());
    }
}