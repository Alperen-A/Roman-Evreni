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
        var yatay_kutu = new HorizontalStackLayout { Spacing = 15, Margin = new Thickness(0, 5) };

        var isim_etiketi = new Label 
        { 
            Text = "🏰 " + ad, 
            TextColor = Colors.White, 
            FontSize = 18,
            VerticalOptions = LayoutOptions.Center,
            WidthRequest = 200
        };

        var sil_butonu = new Button
        {
            Text = "Sil",
            BackgroundColor = Colors.Red,
            TextColor = Colors.White,
            WidthRequest = 50,
            HeightRequest = 40,
            CornerRadius = 20
        };

        sil_butonu.Clicked += (s, e) =>
        {
            Aktif_evren.Mevcut?.Mekanlar.Remove(ad);

            var tum_evrenler = Json_motoru.Yukle();
            var guncel_evren = tum_evrenler.FirstOrDefault(x => x.Evren_adi == Aktif_evren.Mevcut?.Evren_adi);
            
            if (guncel_evren != null && Aktif_evren.Mevcut != null)
            {
                guncel_evren.Mekanlar = Aktif_evren.Mevcut.Mekanlar;
                Json_motoru.Kaydet(tum_evrenler);
            }

            Mekan_listesi.Children.Remove(yatay_kutu);
        };

        yatay_kutu.Children.Add(isim_etiketi);
        yatay_kutu.Children.Add(sil_butonu);
        Mekan_listesi.Children.Add(yatay_kutu);
    }

    private async void OnGeriClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}