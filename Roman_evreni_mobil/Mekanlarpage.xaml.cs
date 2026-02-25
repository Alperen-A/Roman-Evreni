using System;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Roman_evreni_mobil;

public partial class Mekanlarpage : ContentPage
{
    public Mekanlarpage()
    {
        InitializeComponent();
        Mekanlari_yukle();
    }

    private void Mekanlari_yukle()
    {
        Mekan_listesi.Children.Clear();
        
        if (Aktif_evren.Mevcut != null && Aktif_evren.Mevcut.Mekanlar != null)
        {
            foreach (var m in Aktif_evren.Mevcut.Mekanlar)
            {
                Ekrana_mekan_ekle(m);
            }
        }
    }

    private void OnMekanEkleClicked(object sender, EventArgs e)
    {
        string? yeni_ad = Entry_mekanadi.Text?.Trim();
		
        if (!string.IsNullOrWhiteSpace(yeni_ad) && Aktif_evren.Mevcut != null)
        {
            Ekrana_mekan_ekle(yeni_ad);
            Aktif_evren.Mevcut.Mekanlar.Add(yeni_ad);

            var tum_evrenler = Json_motoru.Yukle();
            var guncellenecek_evren = tum_evrenler.FirstOrDefault(x => x.Evren_adi == Aktif_evren.Mevcut.Evren_adi);
            
            if (guncellenecek_evren != null)
            {
                guncellenecek_evren.Mekanlar = Aktif_evren.Mevcut.Mekanlar;
                Json_motoru.Kaydet(tum_evrenler);
            }

            Entry_mekanadi.Text = string.Empty; 
        }
    }

    private void Ekrana_mekan_ekle(string ad)
    {
        var yeni_mekan = new Label 
        { 
            Text = "🏰 " + ad, 
            TextColor = Colors.White, 
            FontSize = 18 
        };
        Mekan_listesi.Children.Add(yeni_mekan);
    }

    private async void OnGeriClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}