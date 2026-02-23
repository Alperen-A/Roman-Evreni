using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Roman_evreni_mobil;

public partial class Karakterlerpage : ContentPage
{
    public Karakterlerpage()
    {
        InitializeComponent();
    }

    private void OnKarakterEkleClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(EntryKarakterAdi.Text))
        {
            var yeniKarakter = new Label 
            { 
                Text = "🗡️ " + EntryKarakterAdi.Text, 
                TextColor = Colors.White, 
                FontSize = 18 
            };
            
            KarakterListesi.Children.Add(yeniKarakter);
            EntryKarakterAdi.Text = string.Empty; 
        }
    }

    private async void OnGeriClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}