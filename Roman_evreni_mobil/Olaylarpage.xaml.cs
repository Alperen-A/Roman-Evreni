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
        // Soru isareti ile mavi uyarilari engelledik
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
        var yeni_olay = new Label 
        { 
            Text = "📜 " + ad, 
            TextColor = Colors.White, 
            FontSize = 18 
        };
        Olay_listesi.Children.Add(yeni_olay);
    }

    private async void OnGeriClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}