using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

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
        Evrenleri_yukle_ve_ciz();
        
        await Main_kutu.FadeTo(1, 800); 
    }

    private void Evrenleri_yukle_ve_ciz()
    {
        _tum_evrenler = Json_motoru.Yukle(); 
        Evrenler_listesi.Children.Clear();

        if (_tum_evrenler.Count == 0) return;

        foreach (var evren in _tum_evrenler)
        {
            var secilen_evren = evren; // closure hatasını önlemek için yerel kopya

            var dikey_kutu = new VerticalStackLayout();
            
            var yatay_kutu = new Grid 
            { 
                ColumnDefinitions = new ColumnDefinitionCollection 
                { 
                    new ColumnDefinition { Width = GridLength.Star }, 
                    new ColumnDefinition { Width = GridLength.Auto } 
                }, 
                Padding = new Thickness(0, 15) 
            };

            var isim_etiketi = new Label 
            { 
                Text = secilen_evren.Evren_adi, 
                TextColor = Color.FromArgb("#f5f5f5"),
                FontSize = 20, 
                FontFamily = "Serif", 
                VerticalOptions = LayoutOptions.Center 
            };
            
            var tap_giris = new TapGestureRecognizer();
            tap_giris.Tapped += async (s, e) => 
            { 
                Aktif_evren.Mevcut = secilen_evren; 
                await Navigation.PushModalAsync(new Mainpage()); 
            };
            isim_etiketi.GestureRecognizers.Add(tap_giris);

            var sil_butonu = new Label 
            { 
                Text = "✕", 
                TextColor = Color.FromArgb("#737373"), 
                FontSize = 18, 
                VerticalOptions = LayoutOptions.Center, 
                Padding = new Thickness(15, 0, 0, 0) 
            };
            
            var tap_sil = new TapGestureRecognizer();
            tap_sil.Tapped += async (s, e) => 
            {
                bool onay = await DisplayAlert("Uyarı", $"{secilen_evren.Evren_adi} silinsin mi?", "Evet", "Hayır");
                if(onay) 
                { 
                    _tum_evrenler.Remove(secilen_evren); 
                    Json_motoru.Kaydet(_tum_evrenler); 
                    Evrenleri_yukle_ve_ciz(); 
                }
            };
            sil_butonu.GestureRecognizers.Add(tap_sil);

            yatay_kutu.Add(isim_etiketi, 0, 0);
            yatay_kutu.Add(sil_butonu, 1, 0);
            
            dikey_kutu.Children.Add(yatay_kutu);
            dikey_kutu.Children.Add(new BoxView { HeightRequest = 1, Color = Color.FromArgb("#171717") });
            
            Evrenler_listesi.Children.Add(dikey_kutu);
        }
    }

    private void On_evren_olustur_clicked(object sender, EventArgs e)
    {
        string? yeni_ad = Entry_yeni_evren.Text?.Trim();

        if (!string.IsNullOrWhiteSpace(yeni_ad))
        {
            var yeni_evren = new Evren_verisi { Evren_adi = yeni_ad };
            _tum_evrenler.Add(yeni_evren);
            Json_motoru.Kaydet(_tum_evrenler);
            
            Evrenleri_yukle_ve_ciz();
            Entry_yeni_evren.Text = string.Empty;
        }
    }
}