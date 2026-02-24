using System;
using Microsoft.Maui.Controls;

namespace Roman_evreni_mobil;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnKarakterlerClicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new Karakterlerpage());
    }

    private void OnMekanlarClicked(object sender, EventArgs e)
    {
        DisplayAlert("Bilgi", "Mekanlar bolumu cok yakinda eklenecek!", "Tamam");
    }

    private void OnOlaylarClicked(object sender, EventArgs e)
    {
        DisplayAlert("Bilgi", "Olaylar bolumu cok yakinda eklenecek!", "Tamam");
    }
}