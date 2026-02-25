using System;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Roman_evreni_mobil;

public partial class Karakterlerpage : ContentPage
{
    public Karakterlerpage()
    {
        InitializeComponent();
        KarakterleriYukle();
    }

    private void KarakterleriYukle()
    {
        KarakterListesi.Children.Clear();
        
        // Aktif evrenin icindeki karakterleri ekrana dizer
        if (Aktif_evren.Mevcut != null && Aktif_evren.Mevcut.Karakterler != null)
        {
            foreach (var k in Aktif_evren.Mevcut.Karakterler)
            {
                EkranaKarakterEkle(k);
            }
        }
    }

    private void OnKarakterEkleClicked(object sender, EventArgs e)
    {
        
        string? yeniAd = EntryKarakterAdi.Text?.Trim();
        
        if (!string.IsNullOrWhiteSpace(yeniAd) && Aktif_evren.Mevcut != null)
        {
            // 1. Ekrana ekle
            EkranaKarakterEkle(yeniAd);

            // 2. Hafizadaki aktif evrenin listesine ekle
            Aktif_evren.Mevcut.Karakterler.Add(yeniAd);

            // 3. Tum listeyi Json dosyasina kalici olarak kaydet
            var tum_evrenler = Json_motoru.Yukle();
            var guncellenecek_evren = tum_evrenler.FirstOrDefault(x => x.Evren_adi == Aktif_evren.Mevcut.Evren_adi);
            
            if (guncellenecek_evren != null)
            {
                guncellenecek_evren.Karakterler = Aktif_evren.Mevcut.Karakterler;
                Json_motoru.Kaydet(tum_evrenler);
            }

            // 4. Kutuyu temizle
            EntryKarakterAdi.Text = string.Empty; 
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