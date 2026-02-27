using System;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Roman_evreni_mobil;

public partial class Olaylarpage : ContentPage
{
    public Olaylarpage()
    {
        InitializeComponent();
        Olaylari_yukle();
    }

    private void Olaylari_yukle()
    {
        Olay_listesi.Children.Clear();
        
        if (Aktif_evren.Mevcut != null && Aktif_evren.Mevcut.Olaylar != null)
        {
            foreach (var o in Aktif_evren.Mevcut.Olaylar)
            {
                Ekrana_olay_ekle(o);
            }
        }
    }

    private void OnOlayEkleClicked(object sender, EventArgs e)
    {
        string? yeni_ad = Entry_olayadi.Text?.Trim();
        
        if (!string.IsNullOrWhiteSpace(yeni_ad) && Aktif_evren.Mevcut != null)
        {
            Ekrana_olay_ekle(yeni_ad);
            Aktif_evren.Mevcut.Olaylar.Add(yeni_ad);

            var tum_evrenler = Json_motoru.Yukle();
            var guncellenecek_evren = tum_evrenler.FirstOrDefault(x => x.Evren_adi == Aktif_evren.Mevcut.Evren_adi);
            
            if (guncellenecek_evren != null)
            {
                guncellenecek_evren.Olaylar = Aktif_evren.Mevcut.Olaylar;
                Json_motoru.Kaydet(tum_evrenler);
            }

            Entry_olayadi.Text = string.Empty; 
        }
    }

    private void Ekrana_olay_ekle(string ad)
    {
        var yatay_kutu = new HorizontalStackLayout { Spacing = 15, Margin = new Thickness(0, 5) };

        var isim_etiketi = new Label 
        { 
            Text = "📜 " + ad, 
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
            Aktif_evren.Mevcut?.Olaylar.Remove(ad);

            var tum_evrenler = Json_motoru.Yukle();
            var guncel_evren = tum_evrenler.FirstOrDefault(x => x.Evren_adi == Aktif_evren.Mevcut?.Evren_adi);
            
            if (guncel_evren != null && Aktif_evren.Mevcut != null)
            {
                guncel_evren.Olaylar = Aktif_evren.Mevcut.Olaylar;
                Json_motoru.Kaydet(tum_evrenler);
            }

            Olay_listesi.Children.Remove(yatay_kutu);
        };

        yatay_kutu.Children.Add(isim_etiketi);
        yatay_kutu.Children.Add(sil_butonu);
        Olay_listesi.Children.Add(yatay_kutu);
    }

    private async void OnGeriClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}