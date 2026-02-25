using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace Roman_evreni_mobil;

public partial class Giris_sayfasi : ContentPage
{
    private List<Evren_verisi> _tum_evrenler = new();

    public Giris_sayfasi()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Main_kutu.FadeTo(1, 800, Easing.CubicOut);
        Evrenleri_yukle_ve_ciz();
    }

    private void Evrenleri_yukle_ve_ciz()
    {
        _tum_evrenler = Json_motoru.Yukle(); 
        Evrenler_listesi.Children.Clear();

        if (_tum_evrenler.Count == 0)
        {
            Evrenler_listesi.Children.Add(new Label { Text = "Henuz bir evren yok. Yukaridan olusturabilirsin.", TextColor = Colors.Gray, HorizontalOptions = LayoutOptions.Center });
            return;
        }

        foreach (var evren in _tum_evrenler)
        {
            // Butonlari ekranin tam ortasina hizaliyoruz
            var yatay_kutu = new HorizontalStackLayout { Spacing = 10, HorizontalOptions = LayoutOptions.Center };

            // Estetik evren giris butonu
            var btn_giris = new Button 
            { 
                Text = "🌌 " + evren.Evren_adi, 
                BackgroundColor = Color.FromArgb("#ffff00"), 
                TextColor = Colors.Black, 
                FontAttributes = FontAttributes.Bold,
                WidthRequest = 200,
                HeightRequest = 50,
                CornerRadius = 25
            };
            
            btn_giris.Clicked += async (s, e) => 
            {
                await btn_giris.ScaleTo(0.9, 100);
                await btn_giris.ScaleTo(1, 100);

                Aktif_evren.Mevcut = evren; 
                await Navigation.PushModalAsync(new MainPage()); 
            };

            // Estetik yuvarlak silme butonu
            var btn_sil = new Button 
            { 
                Text = "X", 
                BackgroundColor = Colors.Red, 
                TextColor = Colors.White,
                FontAttributes = FontAttributes.Bold,
                WidthRequest = 50,
                HeightRequest = 50,
                CornerRadius = 25
            };

            btn_sil.Clicked += async (s, e) => 
            {
                bool onay = await DisplayAlert("Uyari", $"{evren.Evren_adi} evreni silinsin mi?", "Evet", "Hayir");
                if(onay)
                {
                    _tum_evrenler.Remove(evren);
                    Json_motoru.Kaydet(_tum_evrenler);
                    Evrenleri_yukle_ve_ciz(); 
                }
            };

            yatay_kutu.Children.Add(btn_giris);
            yatay_kutu.Children.Add(btn_sil);
            
            Evrenler_listesi.Children.Add(yatay_kutu);
        }
    }

    private void On_evren_olustur_clicked(object sender, EventArgs e)
    {
        // Soru isareti ile mavi uyariyi engelledik
        string? yeni_ad = Entry_yeni_evren.Text?.Trim();
        
        if (!string.IsNullOrEmpty(yeni_ad))
        {
            if (_tum_evrenler.Any(x => x.Evren_adi.ToLower() == yeni_ad.ToLower()))
            {
                DisplayAlert("Hata", "Bu isimde bir evren zaten var!", "Tamam");
                return;
            }

            var yeni_evren = new Evren_verisi { Evren_adi = yeni_ad };
            _tum_evrenler.Add(yeni_evren);
            Json_motoru.Kaydet(_tum_evrenler);
            
            Entry_yeni_evren.Text = string.Empty; 
            Evrenleri_yukle_ve_ciz(); 
        }
    }
}