using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Storage; // Telefonun hafizasina erismek icin eklendi

namespace Roman_evreni_mobil;

public partial class Karakterlerpage : ContentPage
{
    public Karakterlerpage()
    {
        InitializeComponent();
        KarakterleriYukle(); // Sayfa acildiginda eski kayitlari getirir
    }

    private void OnKarakterEkleClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(EntryKarakterAdi.Text))
        {
            string yeniAd = EntryKarakterAdi.Text;
            
            // 1. Ekrana ekle
            EkranaKarakterEkle(yeniAd);

            // 2. Telefon hafizasina kaydet
            string eskiListe = Preferences.Default.Get("KayitliKarakterler", "");
            Preferences.Default.Set("KayitliKarakterler", eskiListe + yeniAd + ",");

            // 3. Kutuyu temizle
            EntryKarakterAdi.Text = string.Empty; 
        }
    }

    private void KarakterleriYukle()
    {
        // Hafizadaki listeyi alip ekrana dizer
        string kayitliListe = Preferences.Default.Get("KayitliKarakterler", "");
        if (!string.IsNullOrEmpty(kayitliListe))
        {
            string[] karakterler = kayitliListe.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var k in karakterler)
            {
                EkranaKarakterEkle(k);
            }
        }
    }

    private void EkranaKarakterEkle(string ad)
    {
        var yeniKarakter = new Label 
        { 
            Text = "🗡️ " + ad, 
            TextColor = Colors.White, 
            FontSize = 18 
        };
        KarakterListesi.Children.Add(yeniKarakter);
    }

    private async void OnGeriClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}