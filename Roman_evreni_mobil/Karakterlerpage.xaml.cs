using System;
using Microsoft.Maui.Controls;

namespace Roman_evreni_mobil;

public partial class Karakterlerpage : ContentPage
{
    public Karakterlerpage()
    {
        InitializeComponent();
    }

    private async void OnGeriClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}